using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MushROMs.Editors
{
    public partial class Obj16StatusControl : UserControl
    {
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
        public int ZoomScaleCount => cbxZoom.Items.Count;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ZoomIndex
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
        public GFXZoomScale ZoomScale
        {
            get => (GFXZoomScale)(ZoomIndex + 1);
            set => ZoomIndex = (int)value - 1;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowAddressScrolling
        {
            get => gbxROMViewing.Visible;
            set => gbxROMViewing.Visible = value;
        }

        public Obj16StatusControl()
        {
            InitializeComponent();
        }
    }
}
