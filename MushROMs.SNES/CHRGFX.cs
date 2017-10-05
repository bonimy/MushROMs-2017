using System;
using System.ComponentModel;

namespace MushROMs.SNES
{
    public class CHRGFX : GFX2
    {
        public GraphicsFormat GraphicsFormat
        {
            get;
            set;
        }

        public int BitsPerPixel => GetBitsPerPixel(GraphicsFormat);
        public int ColorsPerPixel => GetColorsPerPixel(GraphicsFormat);
        public int TileDataSize => GetTileDataSize(GraphicsFormat);

        public CHRGFX(byte[] data, GraphicsFormat format)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var tileSize = GetTileDataSize(format);
            if (data.Length % tileSize != 0)
            {
                var ex = new ArgumentException(SR.ErrorCHRFormat(nameof(data), format, tileSize), nameof(data));
                ex.Data.Add("IsValidTileSize", false);
                throw ex;
            }

            Tiles = new GFXTile2[data.Length / tileSize];
            unsafe
            {
                fixed (byte* src = data)
                fixed (GFXTile2* dest = Tiles)
                {
                    for (int i = Tiles.Length; --i >= 0;)
                    {
                        GetTileData(src + i * tileSize, dest[i].X, format);
                    }
                }
            }
        }

        public CHRGFX(GFX2 gfx, GraphicsFormat format)
        {
            if (gfx == null)
                throw new ArgumentNullException(nameof(gfx));

            var data = gfx.ToByteArray();
        }

        public override byte[] ToByteArray()
        {
            return ToByteArray(GraphicsFormat);
        }

        public byte[] ToByteArray(GraphicsFormat format)
        {
            var tileSize = GetTileDataSize(format);

            var data = new byte[Tiles.Length * tileSize];
            unsafe
            {
                fixed (GFXTile2* src = Tiles)
                fixed (byte* dest = data)
                {
                    for (int i = Tiles.Length; --i >= 0;)
                    {
                        GetFormatData(src[i].X, dest + i * tileSize, format);
                    }
                }
            }

            return data;
        }

        public static int GetBitsPerPixel(GraphicsFormat format)
        {
            var bpp = (int)format & 0x0F;
            if (bpp == 0 || bpp > 8)
                throw new InvalidEnumArgumentException(nameof(format), (int)format, typeof(GraphicsFormat));
            return bpp;
        }

        public static int GetColorsPerPixel(GraphicsFormat format)
        {
            return 1 << GetBitsPerPixel(format);
        }

        public static int GetTileDataSize(GraphicsFormat format)
        {
            return GetBitsPerPixel(format) * GFXTile2.PlanesPerTile;
        }

        private unsafe static void GetTileData(byte* src, byte* dest, GraphicsFormat format)
        {
            switch (format)
            {
            case GraphicsFormat.Format1Bpp8x8:
                GetTileData1Bpp(src, dest);
                return;
            case GraphicsFormat.Format2BppNes:
                GetTileData2BppNes(src, dest);
                return;
            case GraphicsFormat.Format2BppGb:
                GetTileData2BppGb(src, dest);
                return;
            case GraphicsFormat.Format2BppNgp:
                GetTileData2BppNgp(src, dest);
                return;
            case GraphicsFormat.Format2BppVb:
                GetTileData2BppVb(src, dest);
                return;
            case GraphicsFormat.Format3BppSnes:
                GetTileData3BppSnes(src, dest);
                return;
            case GraphicsFormat.Format3Bpp8x8:
                GetTileData3Bpp8x8(src, dest);
                return;
            case GraphicsFormat.Format4BppSnes:
                GetTileData4BppSnes(src, dest);
                return;
            case GraphicsFormat.Format4BppGba:
                GetTileData4BppGba(src, dest);
                return;
            case GraphicsFormat.Format4BppSms:
                GetTileData4BppSms(src, dest);
                return;
            case GraphicsFormat.Format4BppMsx2:
                GetTileData4BppMsx2(src, dest);
                return;
            case GraphicsFormat.Format4Bpp8x8:
                GetTileData4Bpp8x8(src, dest);
                return;
            case GraphicsFormat.Format8BppSnes:
                GetTileData8BppSnes(src, dest);
                return;
            case GraphicsFormat.Format8BppMode7:
                GetTileData8BppMode7(src, dest);
                return;
            default:
                throw new InvalidEnumArgumentException(nameof(format), (int)format, typeof(GraphicsFormat));
            }
        }

        private unsafe static void GetFormatData(byte* src, byte* dest, GraphicsFormat format)
        {
            switch (format)
            {
            case GraphicsFormat.Format1Bpp8x8:
                GetFormatData1Bpp(src, dest);
                return;
            case GraphicsFormat.Format2BppNes:
                GetFormatData2BppNes(src, dest);
                return;
            case GraphicsFormat.Format2BppGb:
                GetFormatData2BppGb(src, dest);
                return;
            case GraphicsFormat.Format2BppNgp:
                GetFormatData2BppNgp(src, dest);
                return;
            case GraphicsFormat.Format2BppVb:
                GetFormatData2BppVb(src, dest);
                return;
            case GraphicsFormat.Format3BppSnes:
                GetFormatData3BppSnes(src, dest);
                return;
            case GraphicsFormat.Format3Bpp8x8:
                GetFormatData3Bpp8x8(src, dest);
                return;
            case GraphicsFormat.Format4BppSnes:
                GetFormatData4BppSnes(src, dest);
                return;
            case GraphicsFormat.Format4BppGba:
                GetFormatData4BppGba(src, dest);
                return;
            case GraphicsFormat.Format4BppSms:
                GetFormatData4BppSms(src, dest);
                return;
            case GraphicsFormat.Format4BppMsx2:
                GetFormatData4BppMsx2(src, dest);
                return;
            case GraphicsFormat.Format4Bpp8x8:
                GetFormatData4Bpp8x8(src, dest);
                return;
            case GraphicsFormat.Format8BppSnes:
                GetFormatData8BppSnes(src, dest);
                return;
            case GraphicsFormat.Format8BppMode7:
                GetFormatData8BppMode7(src, dest);
                return;
            default:
                throw new InvalidEnumArgumentException(nameof(format), (int)format, typeof(GraphicsFormat));
            }
        }

        private unsafe static void GetTileData1Bpp(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; src++)
            {
                var val = *src;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; dest++)
                    *dest = (byte)((val >> x) & 1);
            }
        }

        private unsafe static void GetFormatData1Bpp(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest++)
            {
                var val = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; src++)
                    if (*src != 0)
                        val |= 1 << x;
                *dest = (byte)val;
            }
        }

        private unsafe static void GetTileData2BppNes(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; src++)
            {
                var val1 = src[0];
                var val2 = src[GFXTile2.PlanesPerTile];
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; dest++)
                    *dest = (byte)(((val1 >> x) & 1) |
                        ((val2 >> x) & 1) << 1);
            }
        }

        private unsafe static void GetFormatData2BppNes(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest++)
            {
                var val1 = 0;
                var val2 = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                        val1 |= 1 << x;
                    if ((src[x] & 2) != 0)
                        val2 |= 1 << x;
                }
                dest[0] = (byte)val1;
                dest[GFXTile2.PlanesPerTile] = (byte)val2;
            }
        }

        private unsafe static void GetTileData2BppGb(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; src += 2)
            {
                var val1 = src[0];
                var val2 = src[1];
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; dest++)
                    *dest = (byte)(((val1 >> x) & 1) |
                        ((val2 >> x) & 1) << 1);
            }
        }

        private unsafe static void GetFormatData2BppGb(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest += 2)
            {
                var val1 = 0;
                var val2 = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                        val1 |= 1 << x;
                    if ((src[x] & 2) != 0)
                        val2 |= 1 << x;
                }
                dest[0] = (byte)val1;
                dest[1] = (byte)val2;
            }
        }

        private unsafe static void GetTileData2BppNgp(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; src += sizeof(ushort))
            {
                var val = *(ushort*)src;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; dest++)
                    *dest = (byte)((val >> (x << 1)) & 3);
            }
        }

        private unsafe static void GetFormatData2BppNgp(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest += sizeof(ushort))
            {
                var val = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; src++)
                    val |= (*src & 3) << (x << 1);
                *(ushort*)dest = (ushort)val;
            }
        }

        private unsafe static void GetTileData2BppVb(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest += GFXTile2.DotsPerPlane, src += sizeof(ushort))
            {
                var val = *(ushort*)src;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0;)
                    dest[x] = (byte)((val >> (x << 1)) & 3);
            }
        }

        private unsafe static void GetFormatData2BppVb(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest += sizeof(ushort))
            {
                var val = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0;)
                    val |= (src[x] & 3) << (x << 1);
                *(ushort*)dest = (ushort)val;
            }
        }

        private unsafe static void GetTileData3BppSnes(byte* src, byte* dest)
        {
            for (int y = 0; y < GFXTile2.PlanesPerTile; y++)
            {
                var val1 = src[y << 1];
                var val2 = src[(y << 1) + 1];
                var val3 = src[y + (GFXTile2.PlanesPerTile << 1)];
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; dest++)
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2));
            }
        }

        private unsafe static void GetFormatData3BppSnes(byte* src, byte* dest)
        {
            for (int y = 0; y < GFXTile2.PlanesPerTile; y++)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                        val1 |= 1 << x;
                    if ((src[x] & 2) != 0)
                        val2 |= 1 << x;
                    if ((src[x] & 4) != 0)
                        val3 |= 1 << x;
                }
                dest[y << 1] = (byte)val1;
                dest[(y << 1) + 1] = (byte)val2;
                dest[y + (GFXTile2.PlanesPerTile << 1)] = (byte)val3;
            }
        }

        private unsafe static void GetTileData3Bpp8x8(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; src++)
            {
                var val1 = src[0 * GFXTile2.PlanesPerTile];
                var val2 = src[1 * GFXTile2.PlanesPerTile];
                var val3 = src[2 * GFXTile2.PlanesPerTile];
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; dest++)
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2));
            }
        }

        private unsafe static void GetFormatData3Bpp8x8(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest++)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                        val1 |= 1 << x;
                    if ((src[x] & 2) != 0)
                        val2 |= 1 << x;
                    if ((src[x] & 4) != 0)
                        val3 |= 1 << x;
                }
                dest[0 * GFXTile2.PlanesPerTile] = (byte)val1;
                dest[1 * GFXTile2.PlanesPerTile] = (byte)val2;
                dest[2 * GFXTile2.PlanesPerTile] = (byte)val3;
            }
        }

        private unsafe static void GetTileData4BppSnes(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; src += 2)
            {
                var val1 = src[0];
                var val2 = src[1];
                var val3 = src[0 + (2 * GFXTile2.PlanesPerTile)];
                var val4 = src[1 + (2 * GFXTile2.PlanesPerTile)];
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; dest++)
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val4 >> x) & 1) << 3));
            }
        }

        private unsafe static void GetFormatData4BppSnes(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest += 2)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                        val1 |= 1 << x;
                    if ((src[x] & 2) != 0)
                        val2 |= 1 << x;
                    if ((src[x] & 4) != 0)
                        val3 |= 1 << x;
                    if ((src[x] & 8) != 0)
                        val4 |= 1 << x;
                }
                dest[0] = (byte)val1;
                dest[1] = (byte)val2;
                dest[0 + (2 * GFXTile2.PlanesPerTile)] = (byte)val3;
                dest[1 + (2 * GFXTile2.PlanesPerTile)] = (byte)val4;
            }
        }

        private unsafe static void GetTileData4BppGba(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; src += sizeof(uint), dest += GFXTile2.PlanesPerTile)
            {
                var val = *(uint*)src;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0;)
                    dest[x] = (byte)((val >> (x << 2)) & 0x0F);
            }
        }

        private unsafe static void GetFormatData4BppGba(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest += sizeof(uint), src += GFXTile2.PlanesPerTile)
            {
                var val = 0u;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0;)
                    val |= (uint)((src[x] & 3) << (x << 2));
                *(uint*)dest = val;
            }
        }

        private unsafe static void GetTileData4BppSms(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; src += 4)
            {
                var val1 = src[0];
                var val2 = src[1];
                var val3 = src[2];
                var val4 = src[3];
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; dest++)
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val4 >> x) & 1) << 3));
            }
        }

        private unsafe static void GetFormatData4BppSms(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest += 4)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                        val1 |= 1 << x;
                    if ((src[x] & 2) != 0)
                        val2 |= 1 << x;
                    if ((src[x] & 4) != 0)
                        val3 |= 1 << x;
                    if ((src[x] & 8) != 0)
                        val4 |= 1 << x;
                }
                dest[0] = (byte)val1;
                dest[1] = (byte)val2;
                dest[2] = (byte)val3;
                dest[3] = (byte)val4;
            }
        }

        private unsafe static void GetTileData4BppMsx2(byte* src, byte* dest)
        {
            
            for (int i = 0; i < GFXTile2.Size; i += 2, src++)
            {
                dest[i] = (byte)((*src >> 4) & 0x0F);
                dest[i + 1] = (byte)(*src & 0x0F);
            }
        }

        private unsafe static void GetFormatData4BppMsx2(byte* src, byte* dest)
        {
            for (int i = 0; i < GFXTile2.Size; i += 2, dest++)
            {
                var val1 = src[i] & 0x0F;
                var val2 = src[i + 1] & 0x0F;
                *dest = (byte)((val1 << 4) | val2);
            }
        }

        private unsafe static void GetTileData4Bpp8x8(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; src++)
            {
                var val1 = src[0 * GFXTile2.PlanesPerTile];
                var val2 = src[1 * GFXTile2.PlanesPerTile];
                var val3 = src[2 * GFXTile2.PlanesPerTile];
                var val4 = src[3 * GFXTile2.PlanesPerTile];
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; dest++)
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val4 >> x) & 1) << 3));
            }
        }

        private unsafe static void GetFormatData4Bpp8x8(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest++)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1) != 0)
                        val1 |= 1 << x;
                    if ((src[x] & 2) != 0)
                        val2 |= 1 << x;
                    if ((src[x] & 4) != 0)
                        val3 |= 1 << x;
                    if ((src[x] & 8) != 0)
                        val4 |= 1 << x;
                }
                dest[0 * GFXTile2.PlanesPerTile] = (byte)val1;
                dest[1 * GFXTile2.PlanesPerTile] = (byte)val2;
                dest[2 * GFXTile2.PlanesPerTile] = (byte)val3;
                dest[3 * GFXTile2.PlanesPerTile] = (byte)val4;
            }
        }

        private unsafe static void GetTileData8BppSnes(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; src += 2)
            {
                var val1 = src[0 + (0 * GFXTile2.PlanesPerTile)];
                var val2 = src[1 + (0 * GFXTile2.PlanesPerTile)];
                var val3 = src[0 + (2 * GFXTile2.PlanesPerTile)];
                var val4 = src[1 + (2 * GFXTile2.PlanesPerTile)];
                var val5 = src[0 + (4 * GFXTile2.PlanesPerTile)];
                var val6 = src[1 + (4 * GFXTile2.PlanesPerTile)];
                var val7 = src[0 + (6 * GFXTile2.PlanesPerTile)];
                var val8 = src[1 + (6 * GFXTile2.PlanesPerTile)];
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; dest++)
                    *dest = (byte)(((val1 >> x) & 1) |
                        (((val2 >> x) & 1) << 1) |
                        (((val3 >> x) & 1) << 2) |
                        (((val2 >> x) & 1) << 3) |
                        (((val3 >> x) & 1) << 4) |
                        (((val4 >> x) & 1) << 5) |
                        (((val2 >> x) & 1) << 6) |
                        (((val3 >> x) & 1) << 7));
            }
        }

        private unsafe static void GetFormatData8BppSnes(byte* src, byte* dest)
        {
            for (int y = GFXTile2.PlanesPerTile; --y >= 0; dest += 2)
            {
                var val1 = 0;
                var val2 = 0;
                var val3 = 0;
                var val4 = 0;
                var val5 = 0;
                var val6 = 0;
                var val7 = 0;
                var val8 = 0;
                for (int x = GFXTile2.DotsPerPlane; --x >= 0; src++)
                {
                    if ((src[x] & 1 << 0) != 0)
                        val1 |= 1 << x;
                    if ((src[x] & 1 << 1) != 0)
                        val2 |= 1 << x;
                    if ((src[x] & 1 << 2) != 0)
                        val3 |= 1 << x;
                    if ((src[x] & 1 << 3) != 0)
                        val4 |= 1 << x;
                    if ((src[x] & 1 << 4) != 0)
                        val5 |= 1 << x;
                    if ((src[x] & 1 << 5) != 0)
                        val6 |= 1 << x;
                    if ((src[x] & 1 << 6) != 0)
                        val7 |= 1 << x;
                    if ((src[x] & 1 << 7) != 0)
                        val8 |= 1 << x;
                }
                dest[0 + (0 * GFXTile2.PlanesPerTile)] = (byte)val1;
                dest[1 + (0 * GFXTile2.PlanesPerTile)] = (byte)val2;
                dest[0 + (2 * GFXTile2.PlanesPerTile)] = (byte)val3;
                dest[1 + (2 * GFXTile2.PlanesPerTile)] = (byte)val4;
                dest[0 + (4 * GFXTile2.PlanesPerTile)] = (byte)val5;
                dest[1 + (4 * GFXTile2.PlanesPerTile)] = (byte)val6;
                dest[0 + (6 * GFXTile2.PlanesPerTile)] = (byte)val7;
                dest[1 + (6 * GFXTile2.PlanesPerTile)] = (byte)val8;
            }
        }


        private unsafe static void GetTileData8BppMode7(byte* src, byte* dest)
        {
            for (int i = GFXTile2.Size; --i >= 0;)
                dest[i] = src[i];
        }

        private unsafe static void GetFormatData8BppMode7(byte* src, byte* dest)
        {
            for (int i = GFXTile2.Size; --i >= 0;)
                dest[i] = src[i];
        }
    }
}
