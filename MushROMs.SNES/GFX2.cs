using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushROMs.SNES
{
    public abstract class GFX2 : IGFX
    {
        internal GFXTile2[] Tiles
        {
            get;
            set;
        }

        public int Size => Tiles.Length;

        public GFXTile2 this[int index]
        {
            get => Tiles[index];
        }

        public abstract byte[] ToByteArray();

        public virtual GFXData2 CreateData(ITileMapSelection1D selection)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            var dest = new GFXTile2[selection.Size];
            var indexes = selection.GetSelectedIndexes();
            unsafe
            {
                fixed (GFXTile2* src = Tiles)
                {
                    for (int i = indexes.Length; --i >= 0;)
                        dest[i] = src[indexes[i]];
                }
            }

            return new GFXData2(dest);
        }
    }
}
