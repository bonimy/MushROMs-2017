using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MushROMs.SNES;

namespace MushROMs.Editors
{
    internal partial class CreateGFXOptionsControl : UserControl
    {
        private static readonly IList<GraphicsFormat> Formats = new GraphicsFormat[]
        {
            GraphicsFormat.Format1Bpp8x8,
            GraphicsFormat.Format2BppNes,
            GraphicsFormat.Format2BppGb,
            GraphicsFormat.Format2BppNgp,
            GraphicsFormat.Format2BppVb,
            GraphicsFormat.Format3BppSnes,
            GraphicsFormat.Format3Bpp8x8,
            GraphicsFormat.Format4BppSnes,
            GraphicsFormat.Format4BppGba,
            GraphicsFormat.Format4BppSms,
            GraphicsFormat.Format4BppMsx2,
            GraphicsFormat.Format4Bpp8x8,
            GraphicsFormat.Format8BppSnes,
            GraphicsFormat.Format8BppMode7
        };

        public int NumTiles
        {
            get => (int)nudNumTiles.Value;
            set => nudNumTiles.Value = value;
        }

        public GraphicsFormat GraphicsFormat
        {
            get => Formats[cbxGraphicsFormat.SelectedIndex];
            set
            {
                var index = Formats.IndexOf(value);
                if (index == -1)
                    throw new InvalidEnumArgumentException(nameof(value));

                cbxGraphicsFormat.SelectedIndex = index;
            }
        }

        public bool CopyFrom
        {
            get => chkFromCopy.Enabled && chkFromCopy.Checked;
            set => chkFromCopy.Checked = value;
        }

        public CreateGFXOptionsControl()
        {
            InitializeComponent();
        }
    }
}