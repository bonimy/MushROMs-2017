using System;
using System.Drawing;
using System.Windows.Forms;
using MushROMs.Controls;
using MushROMs.SNES;

namespace MushROMs.Editors
{
    public partial class Obj16Form : TileMapForm, IEditorForm
    {

        public event EventHandler ShowContextMenu;

        public new Obj16Control TileMapControl
        {
            get => (Obj16Control)base.TileMapControl;
            private set => base.TileMapControl = value;
        }

        public Obj16Editor Obj16GFX => TileMapControl.Editor;
        public IEditor Editor => Obj16GFX;
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

        public Obj16Form() : this(new Obj16Editor())
        {
        }

        public Obj16Form(Obj16Editor editor)
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

        private void Obj16Form_Load(object sender, EventArgs e)
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

            TileMapControl.ClientSizeChanged += TileMapControl_ClientSizeChanged;

            estMain.ZoomScale = GFXZoomScale.Zoom2x;
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

        private void Obj16Status_ZoomScaleChanged(object sender, EventArgs e)
        {
            ZoomedViewSizes[LastZoomScaleIndex] = TileMap.ViewSize;

            var zoom = (int)estMain.ZoomScale;
            TileMap.ZoomSize = new Size(zoom, zoom);
            TileMap.ViewSize = ZoomedViewSizes[estMain.ZoomIndex];

            LastZoomScaleIndex = estMain.ZoomIndex;
        }

        private void Obj16Form_ResizeEnd(object sender, EventArgs e)
        {
            if (TileMap == null)
                return;

            for (int i = ZoomedViewSizes.Length; --i >= 0;)
                ZoomedViewSizes[i] = TileMap.ViewSize;
        }

        private void Obj16Status_GraphicsFormatChanged(object sender, EventArgs e)
        {
            obj16Main.Invalidate();
        }
    }
}
