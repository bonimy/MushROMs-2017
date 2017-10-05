using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Helper;
using MushROMs.SNES;

namespace MushROMs.Assembler
{
    public class MultiThreadedCompiler
    {
        private List<string> IncludeDirectories
        {
            get;
            set;
        }

        private List<string> LibraryDirectories
        {
            get;
            set;
        }

        private PathDictionary<CompilerThread> CompilerThreadDictionary
        {
            get;
            set;
        }

        public string AssemlblyPath
        {
            get;
            set;
        }

        public string SourceROMPath
        {
            get;
            set;
        }

        public string OutputROMPath
        {
            get;
            set;
        }

        internal ROM InputROM
        {
            get;
            private set;
        }

        internal ROM OutputROM
        {
            get;
            private set;
        }

        internal ROMInfo ROMInfo
        {
            get;
            private set;
        }

        public Log Log
        {
            get;
            private set;
        }

        public MultiThreadedCompiler()
        {
            CompilerThreadDictionary = new PathDictionary<CompilerThread>();
            Log = new Log();
        }

        public void Compile()
        {
            LoadSourceROM();
            InitializeDestROM();
            LoadAssemblyFile(AssemlblyPath);

            var compilerThreads = CompilerThreadDictionary.Values;
            foreach (var compilerThread in compilerThreads)
                compilerThread.Thread.Start();

            WaitForCompletion();

            foreach (var compilerThread in compilerThreads)
            {
                var resolved = compilerThread.Compiler.GetResolvedTokens();
            }
        }

        private void WaitForCompletion()
        {
            while (!IsComplete())
                continue;
        }

        private bool IsComplete()
        {
            var compilerThreads = CompilerThreadDictionary.Values;
            foreach (var compilerThread in compilerThreads)
            {
                if (compilerThread.Thread.IsAlive)
                    return false;
            }

            return true;
        }

        private void LoadAssemblyFile(string path)
        {
            var actual = GetTruePath(path);
            if (String.IsNullOrEmpty(actual))
            {
                Log.AddError(ErrorCode.CouldNotFindAssemblyFile, path);
                return;
            }

            if (CompilerThreadDictionary.ContainsKey(actual))
            {
                Log.AddWarning(WarningCode.AssemblyFileLoadedTwice, path, actual);
                return;
            }

            string text;
            try
            {
                text = File.ReadAllText(actual);
            }
            catch (IOException ex)
            {
                Log.AddError(ErrorCode.CouldNotReadAssemblyFile, ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Log.AddError(ErrorCode.UnknownAssemblyFileLoadError, ex.Message);
                return;
            }

            var compiler = new SingleThreadCompiler(text, this);
            var thread = new Thread(compiler.Compile);
            CompilerThreadDictionary[path] = new CompilerThread(compiler, thread);
        }

        private void LoadSourceROM()
        {
            // This is a homebrew assemnly
            if (SourceROMPath == null)
            {
                InputROM = null;
                return;
            }

            // We can still build, but the user should be warned.
            if (!File.Exists(SourceROMPath))
            {
                Log.AddWarning(WarningCode.CantFindSourceROMFile, SourceROMPath);
                InputROM = null;
                return;
            }

            byte[] data;
            try
            {
                data = File.ReadAllBytes(SourceROMPath);
            }
            catch (IOException ex)
            {
                Log.AddWarning(WarningCode.CantReadSourceROMFile, ex.Message);
                InputROM = null;
                return;
            }
            catch (Exception ex)
            {
                Log.AddWarning(WarningCode.UnknownErrorReadingSourceROM, ex.Message);
                InputROM = null;
                return;
            }

            ROM rom;
            try
            {
                rom = ROM.CreateFromData(data);
            }
            catch (Exception ex)
            {
                Log.AddWarning(WarningCode.SourceROMDataBadFormat, ex.Message);
                InputROM = null;
                return;
            }

            InputROM = rom;
        }

        private void InitializeDestROM()
        {
            if (InputROM != null)
            {
                OutputROM = InputROM;
                return;
            }

            if (InputROM != null)
                ROMInfo = new ROMInfo(InputROM);
            else
                ROMInfo = new ROMInfo();

            OutputROM = new ROM(ROMInfo);
        }

        public string GetTruePath(string path)
        {
            return GetTruePath(IncludeDirectories, path);
        }

        private static string GetTruePath(List<string> dirs, string path)
        {
            if (File.Exists(path))
                return path;

            foreach (var dir in dirs)
            {
                if (File.Exists(dir + path))
                    return dir + path;
            }

            return String.Empty;
        }

        private class CompilerThread
        {
            public SingleThreadCompiler Compiler
            {
                get;
                private set;
            }

            public Thread Thread
            {
                get;
                private set;
            }

            public CompilerThread(SingleThreadCompiler compiler, Thread thread)
            {
                Compiler = compiler;
                Thread = thread;
            }
        }
    }
}
