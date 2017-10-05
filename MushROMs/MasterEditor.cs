using System;
using System.Collections.Generic;
using System.IO;
using Helper;
using MushROMs.Properties;

namespace MushROMs
{
    public class MasterEditor
    {
        private IEditor _activeEditor;
        private ISelectionData _copyData;

        public event EventHandler SelectionDataCopied;

        public event EventHandler<EditorEventArgs> EditorAdded;
        public event EventHandler<EditorEventArgs> EditorRemoved;
        public event EventHandler EditorActivated;

        private List<IEditor> Editors
        {
            get;
            set;
        }

        private PathDictionary<IEditor> EditorDictionary
        {
            get;
            set;
        }

        public IEditor ActiveEditor
        {
            get => _activeEditor;
            private set
            {
                _activeEditor = value;
                OnEditorActivated(EventArgs.Empty);
            }
        }

        public ISelectionData CopyData
        {
            get => _copyData;
            set
            {
                _copyData = value;
                OnSelectionDataCopied(EventArgs.Empty);
            }
        }

        private ExtensionDictionary<IFileAssociation> FileAssociations
        {
            get;
            set;
        }

        public MasterEditor()
        {
            Editors = new List<IEditor>();
            EditorDictionary = new PathDictionary<IEditor>();
            FileAssociations = new ExtensionDictionary<IFileAssociation>();
        }

        public void AddFileAssociation(IFileAssociation fileAssociation)
        {
            if (fileAssociation == null)
                throw new ArgumentNullException(nameof(fileAssociation));
            FileAssociations.Add(fileAssociation.Extension, fileAssociation);
        }

        public IEditor[] GetEditors()
        {
            return Editors.ToArray();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public ExtensionDictionary<IFileAssociation> GetFileAssociations()
        {
            return new ExtensionDictionary<IFileAssociation>(FileAssociations);
        }

        public IEditor OpenFile(string path)
        {
            if (EditorDictionary.ContainsKey(path))
            {
                var editor = EditorDictionary[path];
                return editor;
            }

            var ext = Path.GetExtension(path);
            if (!FileAssociations.ContainsKey(ext))
                throw new FileFormatException(path, SR.GetString(Resources.ErrorInvalidExtension, ext));
            var fileAssociation = FileAssociations[ext];
            return OpenFileInternal(path, fileAssociation.InitializeEditorMethod);
        }

        public IEditor OpenFile(string path, InitializeEditorMethod initializeEditor)
        {
            if (EditorDictionary.ContainsKey(path))
            {
                var editor = EditorDictionary[path];
                return editor;
            }
            return OpenFileInternal(path, initializeEditor);
        }

        private IEditor OpenFileInternal(string path, InitializeEditorMethod initializeEditor)
        {
            if (initializeEditor == null)
                throw new ArgumentNullException(nameof(initializeEditor));

            var editor = initializeEditor(File.ReadAllBytes(path));
            if (editor == null)
                throw new FileFormatException(path, Resources.ErrorEditorInitialization);

            editor.FullPath = path;
            editor.MasterEditor = this;
            AddEditor(editor);
            return editor;
        }

        public void Save(string path)
        {
            ActiveEditor.Save(path);
        }

        public void SaveAllFiles()
        {
            foreach (var editor in Editors)
                editor.Save();
        }

        public void AddEditor(IEditor editor)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));

            if (!Editors.Contains(editor))
                Editors.Add(editor);
            OnEditorAdded(new EditorEventArgs(editor));
        }

        public void RemoveEditor(IEditor editor)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));

            if (Editors.Contains(editor))
            {
                Editors.Remove(editor);
                OnEditorRemoved(new EditorEventArgs(editor));
            }
        }

        public void ActivateEditor(IEditor editor)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));
            if (!Editors.Contains(editor))
                throw new ArgumentException(Resources.ErrorEditorNotFound, nameof(editor));

            // This feels so hacky but it works exactly as intended.
            Editors.Remove(editor);
            Editors.Add(editor);
            ActiveEditor = Editors[Editors.Count - 1];
        }

        protected virtual void OnEditorAdded(EditorEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            if (e.Editor == null)
                return;

            if (!EditorDictionary.ContainsValue(e.Editor))
                EditorDictionary.Add(e.Editor.FullPath, e.Editor);

            ActivateEditor(e.Editor);

            EditorAdded?.Invoke(this, e);
        }

        protected virtual void OnEditorRemoved(EditorEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            EditorDictionary.Remove(e.Editor.FullPath);

            if (ActiveEditor == e.Editor)
            {
                if (Editors.Count > 0)
                    ActivateEditor(Editors[Editors.Count - 1]);
                else
                    ActiveEditor = null;
            }

            EditorRemoved?.Invoke(this, e);
        }

        protected virtual void OnEditorActivated(EventArgs e)
        {
            EditorActivated?.Invoke(this, e);
        }


        protected virtual void OnSelectionDataCopied(EventArgs e)
        {
            SelectionDataCopied?.Invoke(this, e);
        }
    }
}
