using MushROMs.GenericEditor.Properties;
using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace MushROMs.GenericEditor
{
    public static class Program
    {
        private static readonly string[] FallbackPluginAssemblies = new string[] {
                @"Plugins\MushROMs.SNES.dll"
            };
        private const int FallbackMaxRecentFiles = 10;

        private static MasterForm MasterForm;

        private static Settings Settings => Settings.Default;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += Application_ApplicationExit;
            /*
            const string path = @"smas.asm";

            var start = DateTime.Now;
            var compiler = new MultiThreadedCompiler();
            compiler.AssemlblyPath = path;
            compiler.Compile();
            var length = DateTime.Now - start;
            Console.WriteLine(length.TotalSeconds);
            return;
            */
            MasterForm = new MasterForm();

            if (Settings.FirstTime)
            {
                ResetSettings();

                Settings.FirstTime = false;
                Settings.Save();
            }

            foreach (var pluginPath in Settings.PluginAssemblies)
                PluginManager.LoadPlugin(pluginPath);
            PluginManager.LoadPlugins(MasterForm);

            Application.Run(MasterForm);
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Settings.Save();
        }

        public static void ResetSettings()
        {
            Settings.RecentFiles = new StringCollection();

            Settings.PluginAssemblies = new StringCollection();
            Settings.PluginAssemblies.AddRange(FallbackPluginAssemblies);
        }
    }
}