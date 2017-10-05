using System;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public sealed class RPFPalette : Palette2
    {
        public RPFPalette(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length % Color15BppBgr.SizeOf != 0)
            {
                var ex = new ArgumentException(SR.ErrorRPFSize(nameof(data)), nameof(data));
                ex.Data.Add("IsValidSize", false);
                throw ex;
            }

            Tiles = new Color15BppBgr[data.Length / Color15BppBgr.SizeOf];
            unsafe
            {
                fixed (byte* _ptr = data)
                fixed (Color15BppBgr* dest = Tiles)
                {
                    var src = (Color15BppBgr*)_ptr;
                    for (int i = Tiles.Length; --i >= 0;)
                        dest[i] = src[i];
                }
            }
        }

        public override byte[] ToByteArray()
        {
            var data = new byte[Tiles.Length * Color15BppBgr.SizeOf];
            unsafe
            {
                fixed (byte* _ptr = data)
                fixed (Color15BppBgr* src = Tiles)
                {
                    var dest = (Color15BppBgr*)_ptr;
                    for (int i = Tiles.Length; --i >= 0;)
                        dest[i] = src[i];
                }
            }

            return data;
        }
    }
}
