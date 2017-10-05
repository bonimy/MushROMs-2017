using System;
using System.IO;
using System.Collections.Generic;
using MushROMs.Properties;

namespace MushROMs
{
    public abstract class Editor : IEditor
    {
        private MasterEditor _masterEditor;

        private string _name;
        private string _extension;
        private string _directory;

        private ISelection _selection;

        public event EventHandler MasterEditorChanged;

        public event EventHandler NameChanged;
        public event EventHandler ExtensionChanged;
        public event EventHandler DirectoryChanged;
        public event EventHandler PathChanged;

        public event EventHandler FileOpened;
        public event EventHandler FileSaved;

        public event EventHandler DataInitialized;
        public event EventHandler DataModified;
        public event EventHandler UndoApplied;
        public event EventHandler RedoApplied;

        public event EventHandler SelectionChanged;

        public event EventHandler PreviewModeChanged;

        public MasterEditor MasterEditor
        {
            get => _masterEditor;
            set
            {
                _masterEditor = value;
                OnMasterEditorChanged(EventArgs.Empty);
            }
        }

        public string FullPath
        {
            get
            {
                if (String.IsNullOrEmpty(Directory) || String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Extension))
                    return null;

                return Directory + Path.DirectorySeparatorChar + Name + Extension;
            }
            set
            {
                Directory = Path.GetDirectoryName(value);
                Name = Path.GetFileNameWithoutExtension(value);
                Extension = Path.GetExtension(value);
                OnPathChanged(EventArgs.Empty);
            }
        }

        public string Directory
        {
            get => _directory;
            set
            {
                _directory = value;
                OnDirectoryChanged(EventArgs.Empty);
                OnPathChanged(EventArgs.Empty);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnNameChanged(EventArgs.Empty);
                OnPathChanged(EventArgs.Empty);
            }
        }

        public string Extension
        {
            get => _extension;
            set
            {
                _extension = value;
                OnExtensionChanged(EventArgs.Empty);
                OnPathChanged(EventArgs.Empty);
            }
        }

        private byte[] FileData
        {
            get;
            set;
        }

        public int SaveIndex
        {
            get;
            private set;
        }
        public int HistoryIndex
        {
            get;
            private set;
        }
        private List<State> History
        {
            get;
            set;
        }
        public virtual bool Saved => HistoryIndex == SaveIndex; public virtual bool CanUndo => HistoryIndex > 0 && !PreviewMode; public virtual bool CanRedo => HistoryIndex < History.Count && !PreviewMode; public virtual bool CanCut => CanDelete && CanCopy; public virtual bool CanCopy => true; public virtual bool CanPaste => true; public virtual bool CanDelete => true; public virtual bool CanSelectAll => true;
        public ISelection Selection
        {
            get => _selection;
            set
            {
                _selection = value ?? throw new ArgumentNullException(nameof(value));
                OnSelectionChanged(EventArgs.Empty);
            }
        }

        private ISelectionData CopyData
        {
            get => MasterEditor.CopyData;
            set => MasterEditor.CopyData = value;
        }

        public bool PreviewMode
        {
            get;
            private set;
        }

        protected ISelectionData PreviewData
        {
            get;
            private set;
        }

        protected Editor()
        {
            History = new List<State>();
        }

        public byte[] GetFileData()
        {
            return FileData;
        }

        protected virtual void SetFileData(byte[] data)
        {
            FileData = data;
        }

        public virtual void Open(string path)
        {
            SetFileData(File.ReadAllBytes(path));
            OnFileOpened(EventArgs.Empty);
        }

        public virtual void Save()
        {
            Save(FullPath);
        }

        public virtual void Save(string path)
        {
            File.WriteAllBytes(path, FileData);
            FullPath = path;
            OnFileSaved(EventArgs.Empty);
        }

        private void AddHistoryData(ISelectionData data)
        {
            if (HistoryIndex < History.Count)
                History.RemoveRange(HistoryIndex, History.Count - HistoryIndex);

            History.Add(new State(data, this));
            HistoryIndex++;
        }

        public void Undo()
        {
            if (CanUndo)
            {
                if (HistoryIndex <= 0)
                    throw new InvalidOperationException(Resources.ErrorNoCopyData);

                WriteData(History[--HistoryIndex].Undo, false);
                OnUndoApplied(EventArgs.Empty);
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                if (HistoryIndex >= History.Count)
                    throw new InvalidOperationException(Resources.ErrorNoRedoData);

                WriteData(History[HistoryIndex++].Redo, false);
                OnRedoApplied(EventArgs.Empty);
            }
        }

        public virtual void Cut()
        {
            Copy();
            Delete();
        }

        public virtual void Copy()
        {
            if (Selection == null)
                throw new InvalidOperationException(Resources.ErrorNoSelection);

            CopyData = Selection.GetSelectionData(this);
        }

        public virtual void Paste()
        {
            if (CopyData == null)
                throw new InvalidOperationException(Resources.ErrorNoCopyData);

            WriteData(CopyData.Copy(Selection));
        }

        public abstract void Delete();
        public abstract void SelectAll();

        public ISelectionData GetSelectionData()
        {
            if (Selection == null)
                return null;

            return Selection.GetSelectionData(this);
        }

        public virtual bool IsValidSelectionData(ISelectionData data)
        {
            return data != null;
        }

        public void EnablePreviewMode()
        {
            if (Selection == null)
                throw new InvalidOperationException(Resources.ErrorNoSelection);

            if (PreviewMode)
                return;

            PreviewMode = true;
            PreviewData = GetSelectionData().Copy(Selection);
            OnPreviewModeChanged(EventArgs.Empty);
        }

        public void DisablePreviewMode()
        {
            if (Selection == null)
                throw new InvalidOperationException(Resources.ErrorNoSelection);

            if (!PreviewMode)
                return;

            WriteData(PreviewData, false);
            PreviewMode = false;
            OnPreviewModeChanged(EventArgs.Empty);
        }

        public void WriteData(ISelectionData data)
        {
            WriteData(data, !PreviewMode);
        }

        private void WriteData(ISelectionData data, bool history)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (!IsValidSelectionData(data))
                throw new ArgumentException(Resources.ErrorSelectionDataInvalid, nameof(data));

            if (history && !PreviewMode)
                AddHistoryData(data);

            data.WriteToEditor(this);

            OnDataModified(EventArgs.Empty);
        }

        protected virtual void OnMasterEditorChanged(EventArgs e)
        {
            MasterEditorChanged?.Invoke(this, e);
        }

        protected virtual void OnDirectoryChanged(EventArgs e)
        {
            DirectoryChanged?.Invoke(this, e);
        }

        protected virtual void OnNameChanged(EventArgs e)
        {
            NameChanged?.Invoke(this, e);
        }

        protected virtual void OnExtensionChanged(EventArgs e)
        {
            ExtensionChanged?.Invoke(this, e);
        }

        protected virtual void OnPathChanged(EventArgs e)
        {
            PathChanged?.Invoke(this, e);
        }

        protected virtual void OnFileOpened(EventArgs e)
        {
            FileOpened?.Invoke(this, e);
        }

        protected virtual void OnFileSaved(EventArgs e)
        {
            SaveIndex = HistoryIndex;
            FileSaved?.Invoke(this, e);
        }

        protected virtual void OnDataInitialized(EventArgs e)
        {
            DataInitialized?.Invoke(this, e);
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        protected virtual void OnUndoApplied(EventArgs e)
        {
            UndoApplied?.Invoke(this, e);
        }

        protected virtual void OnRedoApplied(EventArgs e)
        {
            RedoApplied?.Invoke(this, e);
        }

        protected virtual void OnDataModified(EventArgs e)
        {
            DataModified?.Invoke(this, e);
        }

        protected virtual void OnPreviewModeChanged(EventArgs e)
        {
            PreviewModeChanged?.Invoke(this, e);
        }
    }
}
