using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MushROMs.SNES;

namespace MushROMs.Editors
{
    public partial class GFXStatusControl : UserControl
    {
        private const GraphicsFormat FallbackGraphicsFormat = GraphicsFormat.Format1Bpp8x8;
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

        public event EventHandler GraphicsFormatChanged
        {
            add { cbxGraphicsFormat.SelectedIndexChanged += value; }
            remove { cbxGraphicsFormat.SelectedIndexChanged -= value; }
        }

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
        public GraphicsFormat GraphicsFormat
        {
            get => Formats[cbxGraphicsFormat.SelectedIndex];
            set => cbxGraphicsFormat.SelectedIndex = Formats.IndexOf(value);
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

        public GFXStatusControl()
        {
            InitializeComponent();
        }
    }
}
