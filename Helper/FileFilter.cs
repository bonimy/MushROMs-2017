using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    /// <summary>
    /// Provides a filter of extensions to file types.
    /// </summary>
    /// <remarks>
    /// The <see cref="FileFilter"/> class is used for file dialogs. Give it a display name (e.g. Image files)
    /// and a collection of extensions (e.g. ".bmp", ".png", ".jpg") and it provides a method to return a string
    /// "Image files|.bmp;.png;.jpg".
    /// </remarks>
    public class FileFilter
    {
        public const char Wildcard = '*';
        public const char FilterSeparator = '|';
        public const char ExtensionSeparator = ';';
        public const string AnyExtension = ".*";

        public string DisplayName
        {
            get;
            private set;
        }

        public string Extensions
        {
            get;
            private set;
        }
        public string Filter
        {
            get;
            private set;
        }

        public FileFilter(string displayName, IReadOnlyCollection<string> extensions)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Extensions = GenerateExtensionsList(extensions);
            Filter = GenerateFileFilter(true);
        }

        public string GenerateFileFilter(bool showExtensions)
        {
            var sb = new StringBuilder();

            sb.Append(DisplayName);

            if (showExtensions)
            {
                sb.Append(" (");
                sb.Append(Extensions);
                sb.Append(')');
            }

            sb.Append(FilterSeparator);
            sb.Append(Extensions);

            return sb.ToString();
        }

        public static string GenerateExtensionsList(IReadOnlyCollection<string> extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException(nameof(extensions));

            var sb = new StringBuilder();

            foreach (var extension in extensions)
            {
                sb.Append(Wildcard);
                sb.Append(extension);
                sb.Append(ExtensionSeparator);
            }
            sb.Length -= 1; //Removes the last extension separator.

            return sb.ToString();
        }
    }
}
