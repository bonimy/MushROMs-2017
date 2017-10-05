using System;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    /// <summary>
    /// Provides a mechanism for reading Super NES color data.
    /// </summary>
    public interface IPalette
    {
        /// <summary>
        /// Gets the number of colors in the palette.
        /// </summary>
        int Size
        {
            get;
        }
        /// <summary>
        /// Gets the color at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the color to get.
        /// </param>
        /// <returns>
        /// The color at the specified index.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the <see cref="IPalette"/>.
        /// </exception>
        Color15BppBgr this[int index]
        {
            get;
        }

        /// <summary>
        /// Converts the color data to a formatted byte array.
        /// </summary>
        /// <returns>
        /// A formatted byte array representing the color data.
        /// </returns>
        byte[] ToByteArray();

        PaletteData2 CreateData(ITileMapSelection1D selection);
    }
}
