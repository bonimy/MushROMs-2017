using System;
using System.Collections.Generic;

namespace MushROMs.Assembler
{
    public class Options
    {
        private delegate void OptionSetter(string arg);

        private Dictionary<string, OptionSetter> OptionsDictionary
        {
            get;
            set;
        }
        
        public bool PrintMessages
        {
            get;
            set;
        }
        public bool PrintWanrings
        {
            get;
            set;
        }
        public bool PrintErrors
        {
            get;
            set;
        }

        public string MessageFormat
        {
            get;
            set;
        }
        public string WarningFormat
        {
            get;
            set;
        }
        public string ErrorFormat
        {
            get;
            set;
        }

        private List<int> WarningsAsErrors
        {
            get;
            set;
        }
        private List<int> IgnoredWarnings
        {
            get;
            set;
        }
        private List<int> AdditionalWarnings
        {
            get;
            set;
        }

        public string AssemblyDirectory
        {
            get;
            set;
        }
        public string AsemblyPath
        {
            get;
            set;
        }
        public string InputBasePath
        {
            get;
            set;
        }
        public string DestinationBinaryPath
        {
            get;
            set;
        }

        public string AssemblerPath
        {
            get;
            set;
        }

        public bool UseStringEscapeCodes
        {
            get;
            set;
        }

        public bool ErrorsToErrorStream
        {
            get;
            set;
        }
        public bool ErrorsToOutputStream
        {
            get;
            set;
        }

        public Options()
        {
            PrintMessages =
            PrintWanrings =
            PrintErrors = true;

            //MessageFormat = FallbackMessageFormat;
            //WarningFormat = FallbackWarningFormat;
            //ErrorFormat = FallbackErrorFormat;

            OptionsDictionary = new Dictionary<string, OptionSetter>();
            OptionsDictionary.Add(GetArgument(CommandLine.PrintMessages), arg => PrintMessages = GetBool(arg));
            OptionsDictionary.Add(GetArgument(CommandLine.PrintWarnings), arg => PrintWanrings = GetBool(arg));
            OptionsDictionary.Add(GetArgument(CommandLine.PrintErrors), arg => PrintErrors = GetBool(arg));
        }

        public static string GetArgument(string arg)
        {
            if (String.IsNullOrEmpty(arg))
                return String.Empty;
            if (arg[0] != '/')
                return String.Empty;
            int start = 1;
            for (int end = start; end < arg.Length; end++)
            {
                char x = arg[end];
                if (x == '+' || x == '-' || x == ':')
                    return arg.Substring(start, end - start);
            }
            return arg.Substring(start);
        }

        private static bool GetBool(string arg)
        {
            if (String.IsNullOrEmpty(arg))
                return false;
            char last = arg[arg.Length - 1];
            if (last == CommandLine.EnableFlag)
                return true;
            if (last == CommandLine.DisableFag)
                return false;
            return true;
        }

        private static string GetString(string arg)
        {
            if (String.IsNullOrEmpty(arg))
                return String.Empty;
            int start = arg.IndexOf(CommandLine.ArgumentSuffix);
            if (start == -1)
                return String.Empty;
            start++;
            if (arg[start] == '\"' || arg[start] == '\'')
                start++;

            return null;
        }
    }
}
