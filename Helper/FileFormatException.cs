using System;
using System.Runtime.Serialization;

namespace Helper
{
    /// <summary>
    /// An <see cref="Exception"/> that is thrown when file data is not in format that the program requires.
    /// </summary>
    [Serializable]
    public class FileFormatException : Exception
    {
        public string Path
        {
            get;
            private set;
        }

        public FileFormatException() :
            this(null)
        {
        }
        public FileFormatException(string path) :
            this(path, SR.ErrorFileFormat(path))
        {
        }
        public FileFormatException(string path, string message) :
            base(message)
        {
            Path = path;
        }

        public FileFormatException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
        protected FileFormatException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Path = info.GetString(nameof(Path));
        }

        [System.Security.SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            base.GetObjectData(info, context);
            info.AddValue(nameof(Path), Path, Path.GetType());
        }
    }
}
