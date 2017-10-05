using System;
using System.Drawing;
using System.Windows.Forms;
using MushROMs.Controls;
using Helper.PixelFormats;

namespace MushROMs.Editors
{
    public partial class PaletteForm : TileMapForm, IEditorForm
    {
        public new PaletteControl TileMapControl
        {
            get => (PaletteControl)base.TileMapControl;
            private set => base.TileMapControl = value;
        }

        public event EventHandler ShowContextMenu;

        public PaletteEditor Palette => TileMapControl.Editor;
        public IEditor Editor => Palette;
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

        public PaletteForm() : this(new PaletteEditor())
        { }
        public PaletteForm(PaletteEditor editor)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));

            InitializeComponent();

            BindFormSize();
            TileMapControl.Editor = editor;
            ZoomedViewSizes = new Size[estMain.ZoomScaleCount];
            for (int i = ZoomedViewSizes.Length; --i >= 0;)
                ZoomedViewSizes[i] = TileMap.ViewSize;
        }

        private void SetFormTitle()
        {
            var title = Editor.Name + Editor.Extension;
            if (!Editor.Saved)
                title += '*';
            Text = title;
        }

        private void BindFormSize()
        {
            var width = estMain.Location.X + estMain.Width;
            var height = hsbMain.Location.Y + hsbMain.Height + stsMain.Height;
            ClientSize = new Size(width, height);
        }

        private void PaletteForm_Load(object sender, EventArgs e)
        {
            Editor.NameChanged += Editor_NameChanged;
            Editor.ExtensionChanged += Editor_NameChanged;
            Editor.DataInitialized += Editor_DataInitialized;
            Editor.DataModified += Editor_DataModified;
            Editor.FileSaved += Editor_FileSaved;
            SetFormTitle();

            TileMap.ActiveGridTileChanged += TileMap_ActiveViewPointChanged;

            SetTileMapPadding();

            vsbMain.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            hsbMain.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            estMain.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            TileMapControl.ClientSizeChanged += TileMapControl_ClientSizeChanged;

            estMain.PaletteZoomScale = PaletteZoomScale.Zoom32x;
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

        private void plcMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Palette.Selection?.TileMapSelection is SingleSelection1D)
                EditColor();
        }

        private void plcMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.None && e.KeyCode == Keys.Space)
                EditColor();
        }

        private void EditColor()
        {
            using (var dlg = new ColorDialog())
            {
                dlg.FullOpen = true;
                dlg.Color = Palette.GetSelectionData().GetData()[0];
                if (dlg.ShowDialog() == DialogResult.OK)
                    Palette.EditColor((Color15BppBgr)dlg.Color);
            }
        }

        public void Blend()
        {
            using (var dlg = new BlendDialog())
            {
                Palette.EnablePreviewMode();
                dlg.ValueChanged += BlendDialog_ValueChanged;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Palette.DisablePreviewMode();
                    Palette.Blend(dlg.BlendMode, dlg.Color);
                }
                else
                    Palette.DisablePreviewMode();
            }
        }

        private void BlendDialog_ValueChanged(object sender, EventArgs e)
        {
            var dlg = (BlendDialog)sender;
            if (dlg.Preview)
            {
                Palette.EnablePreviewMode();
                Palette.Blend(dlg.BlendMode, dlg.Color);
            }
            else
                Palette.DisablePreviewMode();
        }

        public void Colorize()
        {
            using (var dlg = new ColorizeDialog())
            {
                Palette.EnablePreviewMode();
                dlg.ValueChanged += ColorizeDialog_ValueChanged;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Palette.DisablePreviewMode();
                    if (dlg.ColorizerMode == ColorizerMode.Colorize)
                        Palette.Colorize(dlg.Hue, dlg.Saturation, dlg.Lightness, dlg.Luma, dlg.Alpha);
                    else
                        Palette.Adjust(dlg.Hue, dlg.Saturation, dlg.Lightness, dlg.Luma, dlg.Alpha);
                }
                else
                    Palette.DisablePreviewMode();
            }
        }

        private void ColorizeDialog_ValueChanged(object sender, EventArgs e)
        {
            var dlg = (ColorizeDialog)sender;
            if (dlg.Preview)
            {
                Palette.EnablePreviewMode();
                if (dlg.ColorizerMode == ColorizerMode.Colorize)
                    Palette.Colorize(dlg.Hue, dlg.Saturation, dlg.Lightness, dlg.Luma, dlg.Alpha);
                else
                    Palette.Adjust(dlg.Hue, dlg.Saturation, dlg.Lightness, dlg.Luma, dlg.Alpha);
            }
            else
                Palette.DisablePreviewMode();
        }

        public void Grayscale()
        {
            using (var dlg = new GrayscaleDialog())
            {
                Palette.EnablePreviewMode();
                dlg.ValueChanged += GrayscaleDialog_ValueChanged;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Palette.DisablePreviewMode();
                    Palette.Grayscale((ColorWeight)dlg.Color);
                }
                else
                    Palette.DisablePreviewMode();
            }
        }

        private void GrayscaleDialog_ValueChanged(object sender, EventArgs e)
        {
            var dlg = (GrayscaleDialog)sender;
            if (dlg.Preview)
            {
                Palette.EnablePreviewMode();
                Palette.Grayscale((ColorWeight)dlg.Color);
            }
            else
                Palette.DisablePreviewMode();
        }

        private void TileMap_ActiveViewPointChanged(object sender, EventArgs e)
        {
            TileMapControl.Invalidate();

            var view = TileMap.ActiveViewTile;
            int index = TileMap.GetGridTile(view);
            if (index >= TileMap.GridSize || index < 0)
                return;

            var address = Palette.GetAddressFromIndex(index);
            var color = Palette.GetColorAtAddress(address);
            estMain.ActiveColor = color;
        }

        private void TileMapControl_ClientSizeChanged(object sender, EventArgs e)
        {
            SetSizeFromTileMapControl();
        }

        private void PaletteStatus_ZoomScaleChanged(object sender, EventArgs e)
        {
            ZoomedViewSizes[LastZoomScaleIndex] = TileMap.ViewSize;
            
            var zoom = (int)estMain.PaletteZoomScale;
            TileMap.ZoomSize = new Size(zoom, zoom);
            TileMap.ViewSize = ZoomedViewSizes[estMain.PaletteZoomIndex];

            LastZoomScaleIndex = estMain.PaletteZoomIndex;
        }
        
        private void PaletteForm_ResizeEnd(object sender, EventArgs e)
        {
            if (TileMap == null)
                return;

            for (int i = ZoomedViewSizes.Length; --i >= 0;)
                ZoomedViewSizes[i] = TileMap.ViewSize;
        }

        private void plcMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                OnShowContextMenu(EventArgs.Empty);
        }

        protected virtual void OnShowContextMenu(EventArgs e)
        {
            ShowContextMenu?.Invoke(this, e);
        }
    }
}