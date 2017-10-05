/* A note on struct layout.

Using a fixed size buffer is a lot slower than an array. However, the GFX Tile
should be considered a struct/ValueType. I can't have it be a standard array because
empty initializations will make the array null. And I can't explicitly write the struct
as 64 individual private parameters with a public switch statement indexer, because
the function is large that it doesn't become inlined in release mode. We can force
inlining, but the method is quite large, and likely not worth it given the frequency of
calls the indexer can get. So we'll stick with unsafe fixed buffers because they're compact,
readable, and comparitivaly not too slow when considering their common uses. If C# ever
has a more direct implement of fixed size buffers in structs, then this one should be
redesigned to take advantage of it.

*/

using System;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public struct GFXTile2
    {
        public static readonly GFXTile2 Empty = new GFXTile2();

        public const int DotsPerPlane = BitArray.BitsPerByte;
        public const int PlanesPerTile = DotsPerPlane;
        public const int DotsPerTile = DotsPerPlane * PlanesPerTile;
        public const int Size = DotsPerTile;

        internal unsafe fixed byte X[Size];

        public int this[int index]
        {
            get
            {
                unsafe
                {
                    fixed (byte* ptr = X)
                        return ptr[index];
                }
            }
            set
            {
                unsafe
                {
                    fixed (byte* ptr = X)
                        ptr[index] = (byte)value;
                }
            }
        }

        public GFXTile2 FlipX()
        {
            var result = Empty;

            for (int xForward = 0, xReverse = DotsPerPlane; --xReverse >= 0; xForward++)
            {
                for (int yIndex = Size; (yIndex -= DotsPerPlane) >= 0;)
                {
                    result[yIndex + xForward] = this[yIndex + xReverse];
                }
            }

            return result;
        }

        public GFXTile2 FlipY()
        {
            var result = Empty;

            for (int yForward = 0, yReverse = Size; (yReverse -= DotsPerPlane) >= 0; yForward += DotsPerPlane)
            {
                for (int xIndex = DotsPerPlane; --xIndex >= 0;)
                {
                    result[yForward + xIndex] = this[yReverse + xIndex];
                }
            }

            return result;
        }

        public GFXTile2 Rotate90()
        {
            var result = Empty;

            for (int yDest = 0, xSrc = 0; xSrc < DotsPerPlane; yDest += DotsPerPlane, xSrc++)
            {
                for (int xDest = 0, ySrc = Size; (ySrc -= DotsPerPlane) >= 0; xDest++)
                {
                    result[xDest + yDest] = this[xSrc + ySrc];
                }
            }

            return result;
        }

        public GFXTile2 Rotate180()
        {
            var result = Empty;

            for (int yDest = 0, ySrc = Size; (ySrc -= DotsPerPlane) >= 0; yDest += DotsPerPlane)
            {
                for (int xDest = 0, xSrc = DotsPerPlane; --xSrc >= 0; xDest++)
                {
                    result[xDest + yDest] = this[xSrc + ySrc];
                }
            }

            return result;
        }

        public GFXTile2 Rotate270()
        {
            var result = Empty;

            for (int xDest = 0, ySrc = 0; xDest < DotsPerPlane; ySrc += DotsPerPlane, xDest++)
            {
                for (int yDest = Size, xSrc = 0; (yDest -= DotsPerPlane) >= 0; xSrc++)
                {
                    result[xDest + yDest] = this[xSrc + ySrc];
                }
            }

            return result;
        }

        public GFXTile2 Transpose()
        {
            var result = Empty;

            for (int yDest = 0, xSrc = 0; xSrc < DotsPerPlane; yDest += DotsPerPlane, xSrc++)
            {
                for (int xDest = 0, ySrc = 0; ySrc < Size; xDest++, ySrc += DotsPerPlane)
                {
                    result[xDest + yDest] = this[xSrc + ySrc];
                }
            }

            return result;
        }

        public GFXTile2 MirrorTranspose()
        {
            var result = Empty;

            for (int xDest = 0, ySrc = Size; (ySrc -= DotsPerPlane) >= 0; xDest++)
            {
                for (int yDest = 0, xSrc = DotsPerPlane; --xSrc >= 0; yDest += DotsPerPlane)
                {
                    result[xDest + yDest] = this[xSrc + ySrc];
                }
            }

            return result;
        }

        public GFXTile2 ReplacePixelValue(byte oldValue, byte newValue)
        {
            var result = this;

            unsafe
            {
                fixed (byte* pixels = X)
                {
                    for (int i = Size; --i >= 0;)
                    {
                        if (pixels[i] == oldValue)
                            pixels[i] = newValue;
                    }
                }
            }

            return result;
        }

        public GFXTile2 SwitchPixelValues(byte value1, byte value2)
        {
            var result = this;

            unsafe
            {
                fixed (byte* pixels = X)
                {
                    for (int i = Size; --i >= 0;)
                    {
                        if (pixels[i] == value1)
                            pixels[i] = value2;
                        else if (pixels[i] == value2)
                            pixels[i] = value1;
                    }
                }
            }

            return result;
        }

        public GFXTile2 RotatePixelValues(byte min, byte max, int delta)
        {
            var result = this;

            var range = max - min + 1;

            if (delta > range)
                delta %= range;
            else if (delta < 0)
                delta = (range - delta) % range;

            unsafe
            {
                fixed (byte* pixels = X)
                {
                    for (int i = Size; --i >= 0;)
                    {
                        if (pixels[i] >= min && pixels[i] <= max)
                        {
                            var value = pixels[i] - min;
                            value += delta;
                            value %= range;
                            value += min;
                            pixels[i] = (byte)value;
                        }
                    }
                }
            }

            return result;
        }

        public static bool operator ==(GFXTile2 left, GFXTile2 right)
        {
            for (int i = PlanesPerTile; --i >= 0;)
                if (left[i] != right[i])
                    return false;
            return true;
        }
        public static bool operator !=(GFXTile2 left, GFXTile2 right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is GFXTile2 tile)
                return tile == this;
            return false;
        }
        public override int GetHashCode()
        {
            var code = 0;
            for (int i = Size; --i >= 0;)
                code ^= (this[i] << (i & 0x1F));
            return code;
        }
    }
}