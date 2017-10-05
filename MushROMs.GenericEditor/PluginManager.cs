using System;
using System.Collections.Generic;
using System.Reflection;

namespace MushROMs.GenericEditor
{
    public static class PluginManager
    {
        internal static List<IFileAssociation> FileAssociations = new List<IFileAssociation>();
        internal static List<ITypeInfo> EditorInfoList = new List<ITypeInfo>();

        public static IFileAssociation[] GetFileAssociations()
        {
            return FileAssociations.ToArray();
        }

        public static ITypeInfo[] GetEditorInfoList()
        {
            return EditorInfoList.ToArray();
        }

        public static void LoadPlugins(MasterForm masterForm)
        {
            if (masterForm == null)
                throw new ArgumentNullException(nameof(masterForm));
            var master = masterForm.MasterEditor;
            
            foreach (var fileAssociation in FileAssociations)
            {
                master.AddFileAssociation(fileAssociation);
            }
        }

        public static void LoadPlugin(string path)
        {
            var assemblyName = AssemblyName.GetAssemblyName(path);
            var assembly = Assembly.Load(assemblyName);

            var pluginType = typeof(ILibraryPlugin);
            var pluginTypeName = pluginType.FullName;

            var assemblyTypes = assembly.GetTypes();
            foreach (var assemblyType in assemblyTypes)
            {
                if (assemblyType.GetInterface(pluginTypeName) == null)
                    continue;
                if (assemblyType.IsAbstract || assemblyType.IsInterface)
                    continue;

                var plugin = (ILibraryPlugin)Activator.CreateInstance(assemblyType);
                if (plugin == null)
                    continue;

                var fileAssociations = plugin.GetFileAssociations();
                if (fileAssociations != null)
                {
                    foreach (var fileAssociation in fileAssociations)
                    {
                        if (fileAssociation == null)
                            continue;

                        if (!FileAssociations.Contains(fileAssociation))
                            FileAssociations.Add(fileAssociation);
                    }
                }

                var editorInfoList = plugin.GetEditorInfoList();
                if (editorInfoList != null)
                {
                    foreach (var editorInfo in editorInfoList)
                    {
                        if (editorInfo == null)
                            continue;

                        if (!EditorInfoList.Contains(editorInfo))
                            EditorInfoList.Add(editorInfo);
                    }
                }
            }
        }
    }
}
