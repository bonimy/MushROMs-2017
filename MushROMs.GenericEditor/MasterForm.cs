using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Helper;
using MushROMs.Controls;
using MushROMs.Editors;
using MushROMs.GenericEditor.Properties;
using MushROMs.SNES;

namespace MushROMs.GenericEditor
{
    public delegate Form CreateEditorFormMethod(IEditor editor);

    public partial class MasterForm : Form
    {
        public event EventHandler MasterEditorLoaded;

        private Dictionary<IEditor, Form> EditorFormDictionary
        {
            get;
            set;
        }
        
        private Dictionary<Type, CreateEditorFormMethod> CreateFormDictionary
        {
            get;
            set;
        }

        public MasterEditor MasterEditor
        {
            get;
            private set;
        }

        public IEditor ActiveEditor => MasterEditor.ActiveEditor;
        private Settings Settings => Settings.Default;
        public MasterForm()
        {
            InitializeComponent();   
            InitializeMasterEditor();
                     
            masterMenu.MasterForm = this;

            EditorFormDictionary = new Dictionary<IEditor, Form>();

            CreateFormDictionary = new Dictionary<Type, CreateEditorFormMethod>();
            CreateFormDictionary.Add(typeof(PaletteEditor), editor => new PaletteForm((PaletteEditor)editor));
            CreateFormDictionary.Add(typeof(GFXEditor), editor => new GFXForm((GFXEditor)editor));
        }

        private void InitializeMasterEditor()
        {
            MasterEditor = new MasterEditor();
            MasterEditor.EditorAdded += MasterEditor_EditorAdded;
        }

        protected virtual void OnMasterEditorLoaded(EventArgs e)
        {
            MasterEditorLoaded?.Invoke(this, e);
        }

        public void CreateNewFile()
        {
            using (var dlg = new CreateEditorDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    MasterEditor.AddEditor(dlg.CreateEditor());
            }
        }

        private string GenerateFileFilter(bool allFiles, FileVisibilityFilters filter)
        {
            /*
             * Example output:
             * "Palette Files (*.rpf;*.tpl)|*.rpf;*.tpl|All Files (*.*)|*.*"
             */
            
            // The extensions associated with each editor.
            var editorAssociations = new Dictionary<Type, List<string>>();
            var fileAssociations = PluginManager.GetFileAssociations();
            foreach (var fileAssociation in fileAssociations)
            {
                if ((fileAssociation.Filter & filter) == FileVisibilityFilters.None)
                   continue;

                var type = fileAssociation.InitializeEditorMethod.Method.ReturnType;
                if (!editorAssociations.ContainsKey(type))
                    editorAssociations.Add(type, new List<string>());

                var list = editorAssociations[type];
                if (!list.Contains(fileAssociation.Extension))
                    list.Add(fileAssociation.Extension);
            }

            // The editor names associated with each editor, and the extensions that go with it.
            var infoList = PluginManager.GetEditorInfoList();
            var fileFilters = new List<FileFilter>();
            foreach (var info in infoList)
            {
                if (!editorAssociations.ContainsKey(info.Type))
                    continue;

                var extensions = editorAssociations[info.Type];
                fileFilters.Add(new FileFilter(info.DisplayName, extensions));
            }

            // Compiles file filter as in example output.
            var sb = new StringBuilder();
            if (allFiles)
            {
                sb.Append("All Files (*.*)");
                sb.Append(FileFilter.FilterSeparator);
                sb.Append("*.*");
            }

            if (fileFilters.Count > 0)
            {
                if (allFiles)
                    sb.Append(FileFilter.FilterSeparator);
                foreach (var fileFilter in fileFilters)
                {
                    sb.Append(fileFilter.Filter);
                    sb.Append(FileFilter.FilterSeparator);
                }
                sb.Length -= 1;
            }
            return sb.ToString();
        }

        private int GetFilterIndex(string filter, string extension)
        {
            var sets = filter.Split(FileFilter.FilterSeparator);
            for (int i = 1; i < sets.Length; i += 2)
            {
                if (sets[i].Contains(extension))
                    return ((i - 1) >> 2);
            }
            return -1;
        }
        
        public void OpenFile()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = GenerateFileFilter(true, FileVisibilityFilters.OpenFile);
                dlg.Multiselect = true;
                dlg.Title = Resources.OpenFileTitle;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in dlg.FileNames)
                    {
                        loop:
                        try
                        {
                            OpenFile(file);
                        }
                        catch (Exception ex)
                        {
                            var result = RtlAwareMessageBox.Show(
                                this, ex.Message, Text, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);

                            if (DialogResult == DialogResult.Abort)
                                break;
                            else if (DialogResult == DialogResult.Retry)
                                goto loop;
                            else if (DialogResult == DialogResult.OK)
                                continue;
                        }
                    }
                }
            }
        }

        public void OpenFile(string path)
        {
            try
            {
                MasterEditor.OpenFile(path);
            }
            catch (IOException ex)
            {
                RtlAwareMessageBox.Show(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void AddEditorForm(IEditor editor)
        {
            if (editor == null)
                throw new  ArgumentNullException(nameof(editor));
            
            if (EditorFormDictionary.ContainsKey(editor))
            {
                EditorFormDictionary[editor].Activate();
                return;
            }
            var type = editor.GetType();
            if (!CreateFormDictionary.ContainsKey(type))
                return;

            var form = CreateFormDictionary[type](editor);
            form.MdiParent = this;
            form.FormClosed += EditorForm_FormClosed;
            form.Show();
            EditorFormDictionary.Add(editor, form);
            ((IEditorForm)form).ShowContextMenu += MasterForm_ShowContextMenu;
        }

        private void MasterForm_ShowContextMenu(object sender, EventArgs e)
        {

        }

        public void RemoveEditorForm(IEditor editor)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));

            if (EditorFormDictionary.ContainsKey(editor))
                ((IEditorForm)EditorFormDictionary[editor]).ShowContextMenu -= MasterForm_ShowContextMenu;

            EditorFormDictionary.Remove(editor);
            MasterEditor.RemoveEditor(editor);
        }

        public void SaveFile()
        {
            if (ActiveEditor == null)
                return;

            SaveFile(ActiveEditor.FullPath);
        }

        public void SaveFileAs()
        {
            if (ActiveEditor == null)
                return;

            using (var dlg = new SaveFileDialog())
            {
                dlg.DefaultExt = ActiveEditor.Extension;
                dlg.Filter = GenerateFileFilter(true, FileVisibilityFilters.SaveFile);
                dlg.Title = Resources.SaveFileTitle;
                if (dlg.ShowDialog() == DialogResult.OK)
                    SaveFile(dlg.FileName);
            }
        }

        public void SaveAllFiles()
        {
            MasterEditor.SaveAllFiles();
        }

        public void SaveFile(string path)
        {
            if (ActiveEditor == null)
                return;

            ActiveEditor.Save(path);
        }

        public void Undo()
        {
            if (ActiveEditor != null && ActiveEditor.CanUndo)
                ActiveEditor.Undo();
        }

        public void Redo()
        {
            if (ActiveEditor != null && ActiveEditor.CanRedo)
                ActiveEditor.Redo();
        }

        private void MasterEditor_EditorAdded(object sender, EditorEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            AddEditorForm(e.Editor);
        }

        private void EditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = (IEditorForm)sender;
            RemoveEditorForm(form.Editor);
        }

        private void MasterForm_MdiChildActivate(object sender, EventArgs e)
        {
            if (ActiveMdiChild is IEditorForm)
            {
                var form = (IEditorForm)ActiveMdiChild;
                if (form.Editor != null)
                    MasterEditor.ActivateEditor(form.Editor);
            }
        }
    }
}