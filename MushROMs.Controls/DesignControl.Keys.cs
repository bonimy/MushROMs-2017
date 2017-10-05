using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    partial class DesignControl
    {
        internal static readonly IList<Keys> FallbackOverrideInputKeys = new Keys[] {
            Keys.Up,    Keys.Up    | Keys.Shift, Keys.Up    | Keys.Control, Keys.Up    | Keys.Shift | Keys.Control,
            Keys.Left,  Keys.Left  | Keys.Shift, Keys.Left  | Keys.Control, Keys.Left  | Keys.Shift | Keys.Control,
            Keys.Down,  Keys.Down  | Keys.Shift, Keys.Down  | Keys.Control, Keys.Down  | Keys.Shift | Keys.Control,
            Keys.Right, Keys.Right | Keys.Shift, Keys.Right | Keys.Control, Keys.Right | Keys.Shift | Keys.Control };

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Keys CurrentKeys
        {
            get;
            private set;
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Keys PreviousKeys
        {
            get;
            private set;
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Keys ActiveKeys
        {
            get;
            private set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool ControlKeyHeld => (ModifierKeys & Keys.Control) != Keys.None;[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool ShiftKeyHeld => (ModifierKeys & Keys.Shift) != Keys.None;[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool AltKeyHeld => (ModifierKeys & Keys.Alt) != Keys.None;
        protected override bool IsInputKey(Keys keyData)
        {
            for (int i = FallbackOverrideInputKeys.Count; --i >= 0; )
                if (keyData == FallbackOverrideInputKeys[i])
                    return true;

            return base.IsInputKey(keyData);
        }
        
        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            for (int i = FallbackOverrideInputKeys.Count; --i >= 0; )
                if (keyData == FallbackOverrideInputKeys[i])
                        return false;

            return base.ProcessDialogKey(keyData);
        }
    }
}