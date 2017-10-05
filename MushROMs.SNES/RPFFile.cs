using System;
using Helper;
using Helper.PixelFormats;

namespace MushROMs.SNES
{
    public static class RPFFile
    {
        public const string DefaultExtension = ".rpf";

        public static readonly FileAssociation FileAssociation =
            new FileAssociation(DefaultExtension, InitializeEditor, SaveFileData_Internal, FileVisibilityFilters.Any);

        public static PaletteEditor InitializeEditor(byte[] data)
        {
            if (!IsValidData(data))
                return null;

            return new PaletteEditor(new Palette(GetColors(data)));
        }

        public static byte[] SaveFileData(PaletteEditor editor)
        {
            return GetData(editor.Palette.GetColors());
        }

        private static byte[] SaveFileData_Internal(IEditor editor)
        {
            return SaveFileData((PaletteEditor)editor);
        }

        public static bool IsValidExtension(string ext)
        {
            if (String.IsNullOrEmpty(ext))
                return false;

            return ExtensionComparer.DefaultComparer.Equals(ext, DefaultExtension);
        }

        public static bool IsValidSize(int length)
        {
            return length >= 0 && length % Color15BppBgr.SizeOf == 0;
        }

        public static bool IsValidData(byte[] data)
        {
            if (data == null)
                return false;
            if (!IsValidSize(data.Length))
                return false;

            unsafe
            {
                fixed (byte* ptr = data)
                {
                    for (int i = GetNumColorsFromSize(data.Length); --i >= 0;)
                        if ((((Color15BppBgr*)ptr)[i] & 0x8000) != 0)
                            return false;
                }
            }
            return true;
        }

        public static int GetNumColorsFromSize(int length)
        {
            return length / Color15BppBgr.SizeOf;
        }

        public static int GetSizeFromNumColors(int numColors)
        {
            return numColors * Color15BppBgr.SizeOf;
        }

        public static Color15BppBgr[] GetColors(byte[] data)
        {
            var colors = new Color15BppBgr[GetNumColorsFromSize(data.Length)];
            unsafe
            {
                fixed (byte* src = data)
                fixed (Color15BppBgr* dest = colors)
                {
                    for (int i = data.Length; --i >= 0;)
                        ((byte*)dest)[i] = src[i];
                }
            }
            return colors;
        }

        public static byte[] GetData(Color15BppBgr[] colors)
        {
            if (colors == null)
                return null;

            var data = new byte[GetSizeFromNumColors(colors.Length)];
            unsafe
            {
                fixed (byte* dest = data)
                fixed (Color15BppBgr* src = colors)
                {
                    for (int i = data.Length; --i >= 0;)
                        dest[i] = ((byte*)src)[i];
                }
            }
            return data;
        }
    }
}