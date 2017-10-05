using System;
using System.Threading.Tasks;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public abstract class Palette2 : IPalette
    {
        internal Color15BppBgr[] Tiles
        {
            get;
            set;
        }

        public int Size
        {
            get => Tiles.Length;
        }

        public Color15BppBgr this[int index]
        {
            get => Tiles[index];
        }

        public abstract byte[] ToByteArray();

        public virtual PaletteData2 CreateData(ITileMapSelection1D selection)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            var dest = new Color15BppBgr[selection.Size];
            var indexes = selection.GetSelectedIndexes();
            unsafe
            {
                fixed (Color15BppBgr* src = Tiles)
                {
                    for (int i = indexes.Length; --i >= 0;)
                        dest[i] = src[indexes[i]];
                }
            }

            return new PaletteData2(dest);
        }

        public void DrawDataAsTileMap(IntPtr scan0, int length, TileMap1D tileMap, ITileMapSelection1D selection)
        {
            if (scan0 == IntPtr.Zero)
                throw new ArgumentNullException(nameof(scan0));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (tileMap == null)
                throw new ArgumentNullException(nameof(tileMap));
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            var zero = tileMap.ZeroTile;
            var span = tileMap.VisibleGridSpan;
            var cellw = tileMap.CellWidth;
            var cellh = tileMap.CellHeight;
            var vieww = tileMap.ViewWidth;
            var viewh = tileMap.ViewHeight;
            var width = tileMap.Width;
            var height = tileMap.Height;
            var area = width * height;
            var cellr = cellh * width;

            if (zero + span > Tiles.Length)
                throw new ArgumentException(nameof(tileMap));
            if (area * Color32BppArgb.SizeOf > length)
                throw new ArgumentException(nameof(tileMap));

            var darkness = selection is GateSelection1D ? 2 : 1;

            unsafe
            {
                var image = (Color32BppArgb*)scan0;

                fixed (Color15BppBgr* ptr = &Tiles[zero])
                {
                    // Get color data to draw
                    var src = ptr;

                    // Loop is already capped at data size, so no worry of exceeding array bounds.
                    Parallel.For(0, span, i =>
                    {
                        // Get current color
                        var color = (Color32BppArgb)src[i];

                        // Turn on alpha no matter what
                        color.Alpha = Byte.MaxValue;

                        // Darken regions that are not in the selection
                        if (selection != null && !selection.ContainsIndex(i + zero))
                        {
                            color.Red >>= darkness;
                            color.Green >>= darkness;
                            color.Blue >>= darkness;
                        }

                        // Get destination pointer address.
                        var dest = image +
                            ((i % vieww) * cellh) +
                            ((i / vieww) * cellr);

                        // Draw the tile.
                        for (int h = cellh; --h >= 0; dest += width)
                            for (int w = cellw; --w >= 0;)
                                dest[w] = color;
                    });
                }
            }
        }
    }
}
