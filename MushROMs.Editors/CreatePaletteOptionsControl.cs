using System.Windows.Forms;

namespace MushROMs.Editors
{
    internal partial class CreatePaletteOptionsControl : UserControl
    {
        public int NumColors
        {
            get => (int)nudNumColors.Value;
            set => nudNumColors.Value = value;
        }

        public bool CopyFrom
        {
            get => chkFromCopy.Enabled && chkFromCopy.Checked;
            set => chkFromCopy.Checked = value;
        }

        public CreatePaletteOptionsControl()
        {
            InitializeComponent();
        }
    }
}