using System.Drawing;
using System.Windows.Forms;

namespace MushROMs.Editors
{
    public class EditorGridItem
    {
        public Image Icon
        {
            get;
            set;
        }
        public string FileType
        {
            get;
            set;
        }
        public string FileDescription
        {
            get;
            set;
        }
        public UserControl Options
        {
            get;
            set;
        }

        public EditorGridItem(Image icon, string fileType, string fileDescription, UserControl options)
        {
            Icon = icon;
            FileType = fileType;
            FileDescription = fileDescription;
            Options = options;
        }
    }
}
