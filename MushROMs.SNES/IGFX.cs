using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushROMs.SNES
{
    public interface IGFX
    {
        int Size
        {
            get;
        }
        GFXTile2 this[int index]
        {
            get;
        }

        byte[] ToByteArray();

        GFXData2 CreateData(ITileMapSelection1D selection);
    }
}
