using System;
using System.IO;
using Helper;

namespace MushROMs
{
    public class FileAssociation : IFileAssociation
    {
        public string Extension
        {
            get;
            private set;
        }
        public InitializeEditorMethod InitializeEditorMethod
        {
            get;
            private set;
        }
        public SaveFileDataMethod SaveFileDataMethod
        {
            get;
            private set;
        }
        public FileVisibilityFilters Filter
        {
            get;
            private set;
        }

        public FileAssociation(string extension, InitializeEditorMethod init, SaveFileDataMethod save, FileVisibilityFilters filter)
        {
            if (extension == null)
                throw new ArgumentNullException(nameof(extension));

            InitializeEditorMethod = init ?? throw new ArgumentNullException(nameof(init));
            SaveFileDataMethod = save ?? throw new ArgumentNullException(nameof(save));
            Filter = filter;
            try
            {
                extension = Path.GetExtension(extension);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(
                    SR.ErrorInvalidExtensionName(nameof(extension)),
                    nameof(extension), ex);
            }

            Extension = extension;
        }
    }
}
