using System;
using System.ComponentModel;
using System.Windows.Forms;
using Helper;
using MushROMs.Controls;
using MushROMs.Editors;
using MushROMs.GenericEditor.Properties;

namespace MushROMs.GenericEditor
{
    public partial class MasterMenu : Component
    {
        private MasterForm _masterForm;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MenuStrip MenuStrip => menuStrip;[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolStrip ToolStrip => toolStrip;[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StatusStrip StatusStrip => statusStrip;[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ContextMenuStrip ContextMenuStrip => cmsMaster;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MasterForm MasterForm
        {
            get => _masterForm;
            set
            {
                if (MasterForm != null)
                {
                    MasterForm.Controls.Remove(menuStrip);
                    MasterForm.Controls.Remove(toolStrip);
                    MasterForm.MdiChildActivate -= MasterForm_MdiChildActivate;

                    if (MasterEditor != null)
                    {
                        MasterEditor.EditorAdded -= MasterEditor_EditorAdded;
                        MasterEditor.EditorRemoved -= MasterEditor_EditorRemoved;
                        MasterEditor.SelectionDataCopied -= MasterEditor_SelectionDataCopied;
                        MasterEditor.EditorActivated -= MasterEditor_EditorActivated;
                    }
                }

                _masterForm = value;

                if (MasterForm != null)
                {
                    MasterForm.Controls.Add(toolStrip);
                    MasterForm.Controls.Add(menuStrip);
                    MasterForm.MainMenuStrip = menuStrip;
                    MasterForm.MdiChildActivate += MasterForm_MdiChildActivate;

                    if (MasterEditor != null)
                    {
                        MasterEditor.EditorAdded += MasterEditor_EditorAdded;
                        MasterEditor.EditorRemoved += MasterEditor_EditorRemoved;
                        MasterEditor.SelectionDataCopied += MasterEditor_SelectionDataCopied;
                        MasterEditor.EditorActivated += MasterEditor_EditorActivated;
                    }
                }
                
                ToggleMasterMenu();
            }
        }

        private void MasterForm_MdiChildActivate(object sender, EventArgs e)
        {
            if (MasterForm.ActiveMdiChild is IEditorForm)
            {

            }
        }

        private void MasterEditor_EditorActivated(object sender, EventArgs e)
        {
            ToggleMasterMenu();
        }

        private MasterEditor MasterEditor => MasterForm?.MasterEditor;
        private IEditor ActiveEditor => MasterEditor?.ActiveEditor;
        private ISelectionData CopyData => MasterEditor?.CopyData;
        private bool CoreMenuEnabled
        {
            get => tsmFile.Enabled;
            set
            {
                tsmFile.Enabled =
                tsmNewFile.Enabled =
                tsmOpenFile.Enabled =
                tsmExit.Enabled =
                tsmHelp.Enabled =
                tsmAbout.Enabled =
                tsbNewFile.Enabled =
                tsbOpenFile.Enabled =
                tsbAbout.Enabled = value;
            }
        }

        private bool RecentFilesMenuEnabled
        {
            get => tsmRecentFiles.Enabled;
            set => tsmRecentFiles.Enabled =
     tsbRecentFiles.Enabled = value;
        }

        private bool EditMenuEnabled
        {
            get => tsmEdit.Enabled;
            set
            {
                tsmClose.Enabled =
                tsmSaveFile.Enabled =
                tsmSaveAs.Enabled =
                tsmSaveAll.Enabled =
                tsmEdit.Enabled =
                tsmCut.Enabled =
                tsmCopy.Enabled =
                tsmDelete.Enabled =
                tsbCut.Enabled =
                tsbCopy.Enabled =
                tsbSaveFile.Enabled =
                tsbSaveAll.Enabled = value;
            }
        }

        private bool PasteEnabled
        {
            get => tsmPaste.Enabled;
            set
            {
                tsmPaste.Enabled =
                tsbPaste.Enabled = value;
            }
        }

        private bool UndoEnabled
        {
            get => tsmUndo.Enabled;
            set
            {
                tsmUndo.Enabled =
                tsbUndo.Enabled = value;
            }
        }

        private bool RedoEnabled
        {
            get => tsmRedo.Enabled;
            set
            {
                tsmRedo.Enabled =
                tsbRedo.Enabled = value;
            }
        }

        private bool PaletteMenuVisible
        {
            get => tsmPalette.Visible;
            set
            {
                tsmPalette.Visible =
                tsbInvertColors.Visible =
                tsbBlend.Visible =
                tsbColorize.Visible =
                tsbGrayscale.Visible =
                tsbHorizontalGradient.Visible =
                tsbVerticalGradient.Visible =
                tspPaletteSeparator.Visible = value;
            }
        }

        private Settings Settings => Settings.Default;
        public MasterMenu()
        {
            InitializeComponent();
            InitializeMenu();
        }

        public MasterMenu(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            InitializeMenu();
        }

        private void InitializeMenu()
        {
            tsmNewFile.Click += NewFile_Click;
            tsmOpenFile.Click += OpenFile_Click;
            tsmClose.Click += Close_Click;
            tsmSaveFile.Click += SaveFile_Click;
            tsmSaveAs.Click += SaveAs_Click;
            tsmSaveAll.Click += SaveAll_Click;
            tsmExit.Click += Exit_Click;
            tsmUndo.Click += Undo_Click;
            tsmRedo.Click += Redo_Click;
            tsmCut.Click += Cut_Click;
            tsmCopy.Click += Copy_Click;
            tsmPaste.Click += Paste_Click;
            tsmDelete.Click += Delete_Click;
            tsmSelectAll.Click += SelectAll_Click;
            tsmInvertColors.Click += InvertColors_Click;
            tsmBlend.Click += Blend_Click;
            tsmColorize.Click += Colorize_Click;
            tsmGrayscale.Click += Grayscale_Click;
            tsmHorizontalGradient.Click += HorizontalGradient_Click;
            tsmVerticalGradient.Click += VerticalGradient_Click;
            tsmPlugins.Click += Plugins_Click;
            tsmAbout.Click += About_Click;

            tsbNewFile.Click += NewFile_Click;
            tsbOpenFile.Click += OpenFile_Click;
            tsbSaveFile.Click += SaveFile_Click;
            tsbSaveAll.Click += SaveAll_Click;
            tsbRecentFiles.Click += RecentFiles_Click;
            tsbUndo.Click += Undo_Click;
            tsbRedo.Click += Redo_Click;
            tsbCut.Click += Cut_Click;
            tsbCopy.Click += Copy_Click;
            tsbPaste.Click += Paste_Click;
            tsbInvertColors.Click += InvertColors_Click;
            tsbBlend.Click += Blend_Click;
            tsbColorize.Click += Colorize_Click;
            tsbGrayscale.Click += Grayscale_Click;
            tsbHorizontalGradient.Click += HorizontalGradient_Click;
            tsbVerticalGradient.Click += VerticalGradient_Click;
            tsbAbout.Click += About_Click;

            csmCut.Click += Cut_Click;
            csmCopy.Click += Copy_Click;
            csmPaste.Click += Paste_Click;
            csmInvertColors.Click += InvertColors_Click;
            csmBlend.Click += Blend_Click;
            csmColorize.Click += Colorize_Click;
            csmGrayscale.Click += Grayscale_Click;
            csmHorizontalGradient.Click += HorizontalGradient_Click;
            csmVerticalGradient.Click += VerticalGradient_Click;
        }

        private void MasterEditor_EditorAdded(object sender, EditorEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            if (e.Editor != null)
            {
                AddRecentFile(e.Editor.FullPath);
                e.Editor.DataModified += Editor_DataModified;
                e.Editor.DataInitialized += Editor_DataInitialized;
                e.Editor.FileOpened += Editor_FileOpened;
                e.Editor.FileSaved += Editor_FileSaved;
            }
            ToggleMasterMenu();
        }

        private void MasterEditor_EditorRemoved(object sender, EditorEventArgs e)
        {
            e.Editor.DataModified -= Editor_DataModified;
            e.Editor.DataInitialized -= Editor_DataInitialized;
            e.Editor.FileOpened -= Editor_FileOpened;
            e.Editor.FileSaved -= Editor_FileSaved;
            ToggleMasterMenu();
        }

        private void Editor_FileSaved(object sender, EventArgs e)
        {
            ToggleMasterMenu();
        }

        private void Editor_FileOpened(object sender, EventArgs e)
        {
            ToggleMasterMenu();
        }

        private void Editor_DataInitialized(object sender, EventArgs e)
        {
            ToggleMasterMenu();
        }

        private void Editor_DataModified(object sender, EventArgs e)
        {
            if (!ActiveEditor.PreviewMode)
                ToggleMasterMenu();
        }

        private void MasterEditor_SelectionDataCopied(object sender, EventArgs e)
        {
            PasteEnabled = MasterEditor.CopyData != null;
        }

        private void ToggleMasterMenu()
        {
            CoreMenuEnabled = MasterForm != null;
            ToggleRecentFiles();
            EditMenuEnabled = ActiveEditor != null;
            UndoEnabled = ActiveEditor != null && ActiveEditor.CanUndo;
            RedoEnabled = ActiveEditor != null && ActiveEditor.CanRedo;
            TogglePasteEnabled();
            PaletteMenuVisible = ActiveEditor is PaletteEditor;
        }

        private void ToggleRecentFiles()
        {
            var files = Settings.RecentFiles;
            RecentFilesMenuEnabled = files != null && files.Count > 0;

            if (!RecentFilesMenuEnabled)
                return;

            var menus = new ToolStripItemCollection[] { tsmRecentFiles.DropDownItems, cmsRecentFiles.Items };
            foreach (var menu in menus)
            {
                foreach (var tsm in menu)
                    tsm.Click -= RecentFile_Click;
                menu.Clear();

                for (int i = 0; i < files.Count; i++)
                {
                    var tsm = new ToolStripMenuItem()
                    {
                        Tag = files[i],
                        Text = DisplayRecentFile(i, files[i])
                    };
                    tsm.Click += RecentFile_Click;
                    menu.Add(tsm);
                }
            }
        }

        private string DisplayRecentFile(int index, string path)
        {
            return SR.GetString(Resources.DisplayRecentFile, index + 1, path);
        }

        private void AddRecentFile(string path)
        {
            var files = Settings.RecentFiles;
            var max = Settings.MaxRecentFiles;

            if (files.Contains(path))
                files.Remove(path);
            files.Insert(0, path);
            while (files.Count > max)
                files.RemoveAt(max);
        }

        private void TogglePasteEnabled()
        {
            if (CopyData != null && ActiveEditor != null)
                PasteEnabled = ActiveEditor.IsValidSelectionData(CopyData);
            else
                PasteEnabled = false;
        }

        private void NewFile_Click(object sender, EventArgs e)
        {
            if (MasterForm != null)
                MasterForm.CreateNewFile();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            if (MasterForm != null)
                MasterForm.OpenFile();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            if (MasterForm == null)
                return;

            if (MasterForm.ActiveMdiChild != null)
                MasterForm.ActiveMdiChild.Close();
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            if (MasterForm != null)
                MasterForm.SaveFile();
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            if (MasterForm != null)
                MasterForm.SaveFileAs();
        }

        private void SaveAll_Click(object sender, EventArgs e)
        {
            if (MasterForm != null)
                MasterForm.SaveAllFiles();
        }

        private void RecentFiles_Click(object sender, EventArgs e)
        {
            cmsRecentFiles.Show(Cursor.Position);
        }

        private void RecentFile_Click(object sender, EventArgs e)
        {
            if (MasterForm == null)
                return;

            var tsm = (ToolStripMenuItem)sender;
            MasterForm.OpenFile((string)tsm.Tag);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            if (MasterForm != null)
                MasterForm.Close();
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            if (MasterForm != null)
                MasterForm.Undo();
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            if (MasterForm != null)
                MasterForm.Redo();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            if (ActiveEditor != null)
                ActiveEditor.Delete();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (ActiveEditor != null && ActiveEditor.CanCopy)
                ActiveEditor.Copy();
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            if (ActiveEditor != null && ActiveEditor.CanPaste)
                ActiveEditor.Paste();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (ActiveEditor != null && ActiveEditor.CanDelete)
                ActiveEditor.Delete();
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            if (ActiveEditor != null && ActiveEditor.CanSelectAll)
                ActiveEditor.SelectAll();
        }

        private void InvertColors_Click(object sender, EventArgs e)
        {
            if (ActiveEditor is PaletteEditor)
                ((PaletteEditor)ActiveEditor).InvertColors();
        }

        private void Blend_Click(object sender, EventArgs e)
        {
            if (ActiveEditor is PaletteEditor)
                ((PaletteForm)MasterForm.ActiveMdiChild).Blend();
        }

        private void Colorize_Click(object sender, EventArgs e)
        {
            if (ActiveEditor is PaletteEditor)
                ((PaletteForm)MasterForm.ActiveMdiChild).Colorize();
        }

        private void Grayscale_Click(object sender, EventArgs e)
        {
            if (ActiveEditor is PaletteEditor)
                ((PaletteForm)MasterForm.ActiveMdiChild).Grayscale();
        }

        private void HorizontalGradient_Click(object sender, EventArgs e)
        {
            if (ActiveEditor is PaletteEditor)
            {
                var editor = (PaletteEditor)ActiveEditor;
                if (editor.Selection.TileMapSelection is BoxSelection1D)
                    editor.HorizontalGradient();
            }
        }

        private void VerticalGradient_Click(object sender, EventArgs e)
        {
            if (ActiveEditor is PaletteEditor)
            {
                var editor = (PaletteEditor)ActiveEditor;
                if (editor.Selection.TileMapSelection is BoxSelection1D)
                    editor.VerticalGradient();
            }
        }

        private void Plugins_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void About_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
