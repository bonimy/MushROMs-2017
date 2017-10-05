using System;
using System.ComponentModel;
using MushROMs.Controls;
using MushROMs.SNES;

namespace MushROMs.Editors
{
    public class GFXControl : TileMapEditorControl1D
    {
        private PaletteEditor _palette;
        private GFXColorSelection _colorSelection;
        private PaletteData _paletteData;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new GFXEditor Editor
        {
            get => (GFXEditor)base.Editor;
            set
            {
                if (Editor == value)
                    return;

                if (Editor != null)
                {
                    Editor.StartAddressChanged -= Editor_Redraw;
                    Editor.GraphicsFormatChanged -= Editor_GraphicsFormatChanged;
                }

                base.Editor = value;

                if (Editor != null)
                {
                    Editor.StartAddressChanged += Editor_Redraw;
                    Editor.GraphicsFormatChanged += Editor_GraphicsFormatChanged;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PaletteEditor Palette
        {
            get
            {
                if (_palette == null)
                {
                    _palette = GFXForm.DefaultPalette;
                    _palette.DataInitialized += Editor_Redraw;
                    _palette.DataModified += Editor_Redraw;
                }
                return _palette;
            }
            set
            {
                if (Palette != null)
                {
                    Palette.DataInitialized -= Editor_Redraw;
                    Palette.DataModified -= Editor_Redraw;
                }

                _palette = value;

                if (Palette != null)
                {
                    Palette.DataInitialized += Editor_Redraw;
                    Palette.DataModified += Editor_Redraw;
                }
                Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GFXColorSelection ColorSelection
        {
            get
            {
                if (_colorSelection == null)
                    _colorSelection = new GFXColorSelection(
                        Palette.StartAddress, Palette.TileMap.ZeroTile, Editor.GraphicsFormat);
                return _colorSelection;
            }
            set => _colorSelection = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PaletteData PaletteData
        {
            get
            {
                if (_paletteData == null)
                    _paletteData = new PaletteData(Palette, ColorSelection);
                return _paletteData;
            }
            set => _paletteData = value;
        }

        protected override void DrawDataAsTileMap(IntPtr scan0, int length)
        {
            Editor.DrawDataAsTileMap(scan0, length, PaletteData);
        }

        private void Editor_Redraw(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Editor_GraphicsFormatChanged(object sender, EventArgs e)
        {
            ColorSelection = new GFXColorSelection(
                        ColorSelection.StartAddress,
                        ColorSelection.StartIndex,
                        Editor.GraphicsFormat);
            PaletteData = new PaletteData(Palette, ColorSelection);
            Invalidate();
        }
    }
}
