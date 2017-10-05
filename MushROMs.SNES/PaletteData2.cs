using System;
using Helper;
using SRHelper = Helper.SR;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public delegate Color15BppBgr PaletteColorMethod(Color15BppBgr color);

    public class PaletteData2
    {
        private Color15BppBgr[] Tiles
        {
            get;
            set;
        }

        public int Size => Tiles.Length;

        public Color15BppBgr this[int index]
        {
            get => Tiles[index];
            set => Tiles[index] = value;
        }

        internal PaletteData2(Color15BppBgr[] tiles)
        {
            Tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        }

        public void Clear()
        {
            Clear(Color15BppBgr.Empty);
        }

        public void Clear(Color15BppBgr color)
        {
            AlterTiles(x => color);
        }

        public void InvertColors()
        {
            AlterTiles(invert);

            Color15BppBgr invert(Color15BppBgr color)
            {
                return new Color15BppBgr(
                    color.Red ^ 0xFF,
                    color.Green ^ 0xFF,
                    color.Blue ^ 0xFF
                    );
            }
        }

        public void Blend(BlendMode blendMode, ColorF bottom)
        {
            AlterTiles(blend);

            Color15BppBgr blend(Color15BppBgr color)
            {
                var colorF = (ColorF)color;
                var result = colorF.BlendWith(bottom, blendMode);
                return (Color15BppBgr)result;
            }
        }

        public void HorizontalGradient(int width)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width),
                    SRHelper.ErrorLowerBoundExclusive(nameof(width), width, 0));

            for (int y = Size / width; --y >= 0;)
            {
                var last = (y + 1) * width > Size ?
                    width - 1 :
                    (Size % width) - 1;

                var c1 = (ColorF)Tiles[y * width];
                var c2 = (ColorF)Tiles[y * width + last];

                for (int x = 0; x <= last; x++)
                {
                    var alpha = (float)x / last;
                    var color = c1.AlphaBlend(ColorF.FromArgb(alpha, c2));

                    Tiles[y * width + x] = (Color15BppBgr)color;
                }
            }
        }

        public void AlterTiles(PaletteColorMethod method)
        {
            for (int i = Tiles.Length; --i >= 0;)
                Tiles[i] = method(Tiles[i]);
        }
    }
}
