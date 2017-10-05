using System;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    public interface IEditorForm : IWin32Window
    {
        event EventHandler ShowContextMenu;

        IEditor Editor
        {
            get;
        }
    }
}
