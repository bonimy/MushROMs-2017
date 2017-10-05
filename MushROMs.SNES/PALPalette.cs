using System;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public sealed class PALPalette : Palette2
    {
        public PALPalette(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length % Color24BppRgb.SizeOf != 0)
            {
                var ex = new ArgumentException(SR.ErrorPALSize(nameof(data)), nameof(data));
                ex.Data.Add("IsValidSize", false);
                throw ex;
            }

            Tiles = new Color15BppBgr[data.Length / Color24BppRgb.SizeOf];
            unsafe
            {
                fixed (byte* _ptr = data)
                fixed (Color15BppBgr* dest = Tiles)
                {
                    var src = (Color24BppRgb*)_ptr;
                    for (int i = Tiles.Length; --i >= 0;)
                        dest[i] = (Color15BppBgr)src[i];
                }
            }
        }

        public override byte[] ToByteArray()
        {
            var data = new byte[Tiles.Length * Color24BppRgb.SizeOf];
            unsafe
            {
                fixed (byte* _ptr = data)
                fixed (Color15BppBgr* src = Tiles)
                {
                    var dest = (Color24BppRgb*)_ptr;
                    for (int i = Tiles.Length; --i >= 0;)
                        dest[i] = src[i];
                }
            }

            return data;
        }
    }
}
