using System;

namespace MushROMs
{
    public class EditorEventArgs : EventArgs
    {
        public IEditor Editor
        {
            get;
            private set;
        }

        public EditorEventArgs(IEditor editor)
        {
            Editor = editor;
        }
    }
}
