using System;
using Helper;

namespace MushROMs.SNES
{
    public static class MAP16File
    {
        public const string DefaultExtension = ".map16";

        public static readonly FileAssociation FileAssociation =
            new FileAssociation(DefaultExtension, InitializeEditor, SaveFileData_Internal, FileVisibilityFilters.Any);

        public static Obj16Editor InitializeEditor(byte[] data)
        {
            if (!IsValidData(data))
                return null;

            var editor = new Obj16Editor();
            editor.InitializeData(data);
            return editor;
        }

        public static byte[] SaveFileData(Obj16Editor editor)
        {
            return null;
        }

        private static byte[] SaveFileData_Internal(IEditor editor)
        {
            return SaveFileData((Obj16Editor)editor);
        }

        public static bool IsValidExtension(string ext)
        {
            if (String.IsNullOrEmpty(ext))
                return false;

            return ExtensionComparer.DefaultComparer.Equals(ext, DefaultExtension);
        }

        public static bool IsValidSize(int length)
        {
            return length % Obj16Tile.SizeOf == 0;
        }

        public static bool IsValidData(byte[] data)
        {
            if (data == null)
                return false;
            return IsValidSize(data.Length);
        }
    }
}
