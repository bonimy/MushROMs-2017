using System;
using Helper;
using MushROMs.Properties;

namespace MushROMs
{
    public class EditorInfo : ITypeInfo
    {
        public Type Type
        {
            get;
            private set;
        }
        public string DisplayName
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }

        public EditorInfo(Type type, string displayName) :
            this(type, displayName, null)
        {
        }
        public EditorInfo(Type editorType, string displayName, string description)
        {
            if (editorType == null)
                throw new ArgumentNullException(nameof(editorType));
            if (editorType.GetInterface(typeof(IEditor).FullName) == null)
                throw new ArgumentException(SR.GetString(Resources.ErrorEditorType, editorType), nameof(editorType));

            Type = editorType;
            DisplayName = displayName ?? editorType.Name;
            Description = description;
        }
    }
}
