using System;
using System.ComponentModel;
using System.Windows.Forms;
using Helper.PixelFormats;

namespace MushROMs.Editors
{
    public partial class PaletteStatus : UserControl
    {
        private Color15BppBgr _activeColor;

        public event EventHandler ActiveColorChanged;

        public event EventHandler ZoomScaleChanged
        {
            add { cbxZoom.SelectedIndexChanged += value; }
            remove { cbxZoom.SelectedIndexChanged -= value; }
        }

        public event EventHandler NextByte
        {
            add { btnNextByte.Click += value; }
            remove { btnNextByte.Click -= value; }
        }
        public event EventHandler LastByte
        {
            add { btnLastByte.Click += value; }
            remove { btnLastByte.Click -= value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color15BppBgr ActiveColor
        {
            get => _activeColor;
            set
            {
                _activeColor = value;
                var color = (Color24BppRgb)ActiveColor;
                lblPcValue.Text = "0x" + color.Value.ToString("X6");
                lblSnesValue.Text = "0x" + ActiveColor.Value.ToString("X4");
                lblRedValue.Text = color.Red.ToString();
                lblGreenValue.Text = color.Green.ToString();
                lblBlueValue.Text = color.Blue.ToString();
                OnActiveColorChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ZoomScaleCount => cbxZoom.Items.Count;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PaletteZoomIndex
        {
            get => cbxZoom.SelectedIndex;
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > ZoomScaleCount)
                    value = ZoomScaleCount - 1;
                cbxZoom.SelectedIndex = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PaletteZoomScale PaletteZoomScale
        {
            get => (PaletteZoomScale)(8 * (PaletteZoomIndex + 1));
            set => PaletteZoomIndex = ((int)value / 8) - 1;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowAddressScrolling
        {
            get => gbxROMViewing.Visible;
            set => gbxROMViewing.Visible = value;
        }

        public PaletteStatus()
        {
            InitializeComponent();
        }

        protected virtual void OnActiveColorChanged(EventArgs e)
        {
            ActiveColorChanged?.Invoke(this, e);
        }
    }
}
