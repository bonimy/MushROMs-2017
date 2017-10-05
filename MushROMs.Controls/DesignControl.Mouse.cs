using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    partial class DesignControl
    {
        public static readonly Point MouseOutOfRange = new Point(-1, -1);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point CurrentMousePosition
        {
            get;
            private set;
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point PreviousMousePosition
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static MouseButtons CurrentMouseButtons
        {
            get;
            private set;
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static MouseButtons PreviousMouseButtons
        {
            get;
            private set;
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static MouseButtons ActiveMouseButtons
        {
            get;
            private set;
        }

        [Browsable(true)]   // This event is overridden so it can be browsable in the designer.
        [Category("Mouse")]
        [Description("Occurs when the mouse wheel moves while the control has focus.")]
        public new event MouseEventHandler MouseWheel
        {
            add { base.MouseWheel += value; }
            remove { base.MouseWheel -= value; }
        }
    }
}