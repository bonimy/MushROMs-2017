using System;
using System.Collections.Generic;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public class TPLPalette
    {
        private static readonly IList<byte> Header = new byte[] {
            (byte)'T', (byte)'P', (byte)'L', 0x02 };

        private Color15BppBgr[] Colors
        {
            get;
            set;
        }

        public Color15BppBgr this[int index]
        {
            get => Colors[index];
        }

        public TPLPalette(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length < Header.Count)
            {
                var ex = new ArgumentException(SR.ErrorTPLHeaderSize(nameof(data)), nameof(data));
                ex.Data.Add("IsValidHeaderSize", false);
                throw ex;
            }

            for (int i = Header.Count; --i >= 0;)
            {
                if (data[i] != Header[i])
                {
                    var ex = new ArgumentException(SR.ErrorTPLFormat(nameof(data)), nameof(data));
                    ex.Data.Add("IsValidHeaderSize", true);
                    ex.Data.Add("IsValidHeader", false);
                    throw ex;
                }
            }

            if ((data.Length - Header.Count) % Color15BppBgr.SizeOf != 0)
            {
                var ex = new ArgumentException(SR.ErrorTPLSize(nameof(data)), nameof(data));
                ex.Data.Add("IsValidHeaderSize", true);
                ex.Data.Add("IsValidHeader", true);
                ex.Data.Add("IsValidSize", false);
            }

            Colors = new Color15BppBgr[(data.Length - Header.Count) / Color15BppBgr.SizeOf];
            unsafe
            {
                fixed (byte* _ptr = &data[Header.Count])
                fixed (Color15BppBgr* dest = Colors)
                {
                    var src = (Color15BppBgr*)_ptr;
                    for (int i = Colors.Length; --i >= 0;)
                        dest[i] = src[i];
                }
            }
        }

        public byte[] ToByteArray()
        {
            var data = new byte[Header.Count + Colors.Length * Color15BppBgr.SizeOf];
            for (int i = Header.Count; --i >= 0;)
                data[i] = Header[i];

            unsafe
            {
                fixed (byte* _ptr = &data[Header.Count])
                fixed (Color15BppBgr* src = Colors)
                {
                    var dest = (Color15BppBgr*)_ptr;
                    for (int i = Colors.Length; --i >= 0;)
                        dest[i] = src[i];
                }
            }

            return data;
        }
    }
}
