using System;
using System.ComponentModel;
using System.Drawing;
using MushROMs.Controls;
using MushROMs.SNES;

namespace MushROMs.Editors
{
    public class Obj16Control : TileMapEditorControl1D
    {
        private PaletteEditor _palette;
        private GFXColorSelection _colorSelection;
        private PaletteData _paletteData;
        private GFXData _gfxData;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Obj16Editor Editor
        {
            get => (Obj16Editor)base.Editor;
            set
            {
                if (Editor == value)
                    return;

                if (Editor != null)
                {
                    Editor.StartAddressChanged -= Editor_Redraw;
                }

                base.Editor = value;

                if (Editor != null)
                {
                    Editor.StartAddressChanged += Editor_Redraw;
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
                        Palette.StartAddress, Palette.TileMap.ZeroTile, GFXData.GraphicsFormat);
                return _colorSelection;
            }
            set
            {
                _colorSelection = value;
                Invalidate();
            }
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
            set
            {
                _paletteData = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GFXData GFXData
        {
            get => _gfxData;
            set
            {
                _gfxData = value;
                Invalidate();
            }
        }

        public Obj16Control()
        {
            BackgroundPattern = new CheckerPattern(Color.DarkGray, Color.FromArgb(32, 32, 32), new Size(1, 1));
        }

        protected override void DrawDataAsTileMap(IntPtr scan0, int length)
        {
            Editor.DrawDataAsTileMap(scan0, length, PaletteData, GFXData);
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
                        GFXData.GraphicsFormat);
            PaletteData = new PaletteData(Palette, ColorSelection);
            Invalidate();
        }
    }
}
