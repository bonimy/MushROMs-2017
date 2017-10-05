using System;
using System.Drawing;
using System.Windows.Forms;
using MushROMs.Controls;
using MushROMs.SNES;
using MushROMs.Editors.Properties;

namespace MushROMs.Editors
{
    public partial class GFXForm : TileMapForm, IEditorForm
    {
        private static PaletteEditor _defaultPalette;

        public event EventHandler GraphicsFormatChanged
        {
            add { estMain.GraphicsFormatChanged += value; }
            remove { estMain.GraphicsFormatChanged -= value; }
        }

        public event EventHandler ShowContextMenu;

        public static PaletteEditor DefaultPalette
        {
            get
            {
                if (_defaultPalette == null)
                {
                    _defaultPalette = new PaletteEditor();
                    _defaultPalette.InitializeData(PALFile.GetColors(Resources.DefaultPalette));
                }

                return _defaultPalette;
            }
        }
        public new GFXControl TileMapControl
        {
            get => (GFXControl)base.TileMapControl;
            private set => base.TileMapControl = value;
        }

        public GFXEditor GFX => TileMapControl.Editor;
        public PaletteEditor Palette
        {
            get => TileMapControl.Palette;
            set => TileMapControl.Palette = value;
        }

        public GraphicsFormat GraphicsFormat
        {
            get => estMain.GraphicsFormat;
            set => estMain.GraphicsFormat = value;
        }

        public IEditor Editor => GFX;
        public new TileMap1D TileMap => (TileMap1D)base.TileMap;
        private int LastZoomScaleIndex
        {
            get;
            set;
        }
        private Size[] ZoomedViewSizes
        {
            get;
            set;
        }

        public GFXForm() : this(new GFXEditor())
        {
        }

        public GFXForm(GFXEditor editor)
        {
            InitializeComponent();

            BindFormSize();
            TileMapControl.Editor = editor??throw new ArgumentNullException(nameof(editor));
            ZoomedViewSizes = new Size[estMain.ZoomScaleCount];
            for (int i = ZoomedViewSizes.Length; --i >= 0;)
                ZoomedViewSizes[i] = TileMap.ViewSize;
        }

        private void BindFormSize()
        {

        }

        private void GFXForm_Load(object sender, EventArgs e)
        {
            Editor.NameChanged += Editor_NameChanged;
            Editor.ExtensionChanged += Editor_NameChanged;
            Editor.DataInitialized += Editor_DataInitialized;
            Editor.DataModified += Editor_DataModified;
            Editor.FileSaved += Editor_FileSaved;
            SetFormTitle();

            SetTileMapPadding();

            vsbMain.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            hsbMain.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            estMain.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            plcColors.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

            plcColors.Editor = Palette;

            TileMapControl.ClientSizeChanged += TileMapControl_ClientSizeChanged;

            estMain.ZoomScale = GFXZoomScale.Zoom4x;
            estMain.GraphicsFormat = GFX.GraphicsFormat;
        }

        private void TileMapControl_ClientSizeChanged(object sender, EventArgs e)
        {
            Size = AdjustSize(WinAPIMethods.InflateSize(TileMapControl.ClientSize, MainTileMapPadding));
        }

        private void Editor_FileSaved(object sender, EventArgs e)
        {
            SetFormTitle();
        }

        private void Editor_DataModified(object sender, EventArgs e)
        {
            SetFormTitle();
        }

        private void Editor_DataInitialized(object sender, EventArgs e)
        {
            SetFormTitle();
        }

        private void Editor_NameChanged(object sender, EventArgs e)
        {
            SetFormTitle();
        }

        private void SetFormTitle()
        {
            var title = Editor.Name + Editor.Extension;
            if (!Editor.Saved)
                title += '*';
            Text = title;
        }
        
        private void GFXStatus_ZoomScaleChanged(object sender, EventArgs e)
        {
            ZoomedViewSizes[LastZoomScaleIndex] = TileMap.ViewSize;

            var zoom = (int)estMain.ZoomScale;
            TileMap.ZoomSize = new Size(zoom, zoom);
            TileMap.ViewSize = ZoomedViewSizes[estMain.ZoomIndex];

            LastZoomScaleIndex = estMain.ZoomIndex;
        }

        private void GFXForm_ResizeEnd(object sender, EventArgs e)
        {
            if (TileMap == null)
                return;

            for (int i = ZoomedViewSizes.Length; --i >= 0;)
                ZoomedViewSizes[i] = TileMap.ViewSize;
        }

        private void GFXStatus_GraphicsFormatChanged(object sender, EventArgs e)
        {
            GFX.GraphicsFormat = estMain.GraphicsFormat;
            gfcMain.Invalidate();
        }
    }
}
