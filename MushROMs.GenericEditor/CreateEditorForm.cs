using System;
using System.Drawing;
using System.Windows.Forms;
using MushROMs.Controls;

namespace MushROMs.GenericEditor
{
    internal partial class CreateEditorForm : DialogForm
    {
        private static readonly Color HoveringCellColor = Color.FromArgb(0xFF, 0xFF, 0xBF);

        private GridItem[] GridItems
        {
            get;
            set;
        }

        private DataGridViewRowCollection Rows => dgvNewFileList.Rows;
        private int CurrentRowIndex => dgvNewFileList.CurrentCell.RowIndex;
        public CreateEditorForm()
        {
            InitializeComponent();

            GridItems = new GridItem[]
            {
                /*
                new GridItem(null, Resources.PaletteFileType, Resources.PaletteFileDescription, PaletteOptions),
                new GridItem(null, Resources.GFXFileType, Resources.GFXFileDescription, GFXOptions),
                new GridItem(null, Resources.TileMapFileType, Resources.TileMapFileDescription, null),
                */
            };

            for (int i = 0; i < GridItems.Length; i++)
                Rows.Add(GridItems[i].Icon, GridItems[i].FileType, String.Empty);
        }

        public Editor CreateEditor()
        {
            var options = GridItems[CurrentRowIndex].Options;
            /*
            if (options == PaletteOptions)
            {
                PaletteEditor palette = new PaletteEditor();
                palette.InitializeData(PaletteOptions.NumColors);
                return palette;
            }
            */
            return null;
        }

        private void SetRowBackColor(int rowIndex, Color color)
        {
            Rows[rowIndex].DefaultCellStyle.BackColor = color;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && AcceptButton != null)
            {
                AcceptButton.PerformClick();
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        private void dgvNewFileList_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            SetRowBackColor(e.RowIndex, HoveringCellColor);
        }

        private void dgvNewFileList_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            SetRowBackColor(e.RowIndex, SystemColors.ControlLightLight);
        }

        private void dgvNewFileList_CurrentCellChanged(object sender, EventArgs e)
        {
            lblDescription.Text = GridItems[CurrentRowIndex].FileDescription;

            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(GridItems[CurrentRowIndex].Options);
        }

        private void pnlMain_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.Location = new Point(
                (pnlMain.Width - e.Control.Width) / 2, (pnlMain.Height - e.Control.Height) / 2);
        }

        private struct GridItem
        {
            public Image Icon
            {
                get;
                set;
            }
            public string FileType
            {
                get;
                set;
            }
            public string FileDescription
            {
                get;
                set;
            }
            public UserControl Options
            {
                get;
                set;
            }

            public GridItem(Image icon, string fileType, string fileDescription, UserControl options)
            {
                Icon = icon;
                FileType = fileType;
                FileDescription = fileDescription;
                Options = options;
            }
        }
    }
}