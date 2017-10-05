using System;

namespace MushROMs
{
    public interface IEditor
    {
        MasterEditor MasterEditor
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }
        string FullPath
        {
            get;
            set;
        }
        string Extension
        {
            get;
            set;
        }
        string Directory
        {
            get;
            set;
        }

        ISelection Selection
        {
            get;
            set;
        }

        bool CanUndo
        {
            get;
        }
        bool CanRedo
        {
            get;
        }
        bool CanCut
        {
            get;
        }
        bool CanCopy
        {
            get;
        }
        bool CanPaste
        {
            get;
        }
        bool CanDelete
        {
            get;
        }
        bool CanSelectAll
        {
            get;
        }
        bool Saved
        {
            get;
        }
        int SaveIndex
        {
            get;
        }
        int HistoryIndex
        {
            get;
        }

        bool PreviewMode
        {
            get;
        }

        event EventHandler NameChanged;
        event EventHandler ExtensionChanged;
        event EventHandler DirectoryChanged;
        event EventHandler FileOpened;
        event EventHandler FileSaved;

        event EventHandler DataInitialized;
        event EventHandler DataModified;
        event EventHandler UndoApplied;
        event EventHandler RedoApplied;

        byte[] GetFileData();

        void Open(string path);
        void Save();
        void Save(string path);

        void Undo();
        void Redo();

        void Cut();
        void Copy();
        void Paste();
        void Delete();
        void SelectAll();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ISelectionData GetSelectionData();
        bool IsValidSelectionData(ISelectionData data);
        void WriteData(ISelectionData data);
    }
}
