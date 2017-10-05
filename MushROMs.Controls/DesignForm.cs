using System;
using System.Collections.Generic;
using System.ComponentModel;
using Debug = System.Diagnostics.Debug;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    public class DesignForm : Form
    {
        internal static readonly IList<Keys> FallbackOverrideInputKeys = DesignControl.FallbackOverrideInputKeys;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size FormBorderSize => GetFormBorderSize(this);
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CaptionHeight => GetCaptionHeight(this);
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding BorderPadding => GetFormBorderPadding(this);
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding WindowPadding
        {
            get
            {
                var padding = BorderPadding;
                padding.Top += CaptionHeight;
                return padding;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle AbsoluteCoordinates => WinAPIMethods.GetWindowRectangle(this);
        public DesignForm()
        {
            KeyPreview = true;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            for (int i = FallbackOverrideInputKeys.Count; --i >= 0;)
                if (keyData == FallbackOverrideInputKeys[i])
                    return true;
            return base.IsInputKey(keyData);
        }

        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            for (int i = FallbackOverrideInputKeys.Count; --i >= 0;)
                if (keyData == FallbackOverrideInputKeys[i])
                    return false;

            return base.ProcessDialogKey(keyData);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
            case WindowMessages.Size:
                if (m.WParam == IntPtr.Zero)
                {
                    unsafe
                    {
                        var client = new Size((int)m.LParam & 0xFFFF, (int)m.LParam >> 0x10);
                        var window = WinAPIMethods.InflateSize(client, WindowPadding);
                        window = AdjustSize(window);
                        client = WinAPIMethods.DeflateSize(window, WindowPadding);
                        m.LParam = (IntPtr)((client.Width & 0xFFFF) | ((client.Height & 0xFFFF) << 0x10));
                    }
                }
                break;
            case WindowMessages.Sizing:
                unsafe
                {
                    var pRECT = (WinAPIRectangle*)m.LParam;
                    *pRECT = AdjustSizingRectangle(*pRECT);
                }
                break;
            }
            base.DefWndProc(ref m);
        }

        protected virtual Rectangle AdjustSizingRectangle(Rectangle window)
        {
            return window;
        }

        protected virtual Size AdjustSize(Size window)
        {
            return window;
        }

        public static Size GetFormBorderSize(Form form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            switch (form.FormBorderStyle)
            {
            case FormBorderStyle.None:
                return Size.Empty;

            case FormBorderStyle.FixedSingle:
            case FormBorderStyle.FixedDialog:
            case FormBorderStyle.Sizable:
                return SystemInformation.FrameBorderSize +
                    WinAPIMethods.PaddedBorderSize;

            case FormBorderStyle.FixedToolWindow:
            case FormBorderStyle.SizableToolWindow:
                return SystemInformation.FixedFrameBorderSize +
                    WinAPIMethods.PaddedBorderSize;

            case FormBorderStyle.Fixed3D:
                return SystemInformation.FrameBorderSize +
                    SystemInformation.Border3DSize +
                    WinAPIMethods.PaddedBorderSize;

            default:    //This should never occur.
                return Size.Empty;
            }
        }

        public static Padding GetFormBorderPadding(Form form)
        {
            var sz = GetFormBorderSize(form);
            return new Padding(sz.Width, sz.Height, sz.Width, sz.Height);
        }

        public static int GetCaptionHeight(Form form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            switch (form.FormBorderStyle)
            {
            case FormBorderStyle.None:
                return 0;

            case FormBorderStyle.FixedSingle:
            case FormBorderStyle.Fixed3D:
            case FormBorderStyle.FixedDialog:
            case FormBorderStyle.Sizable:
                return SystemInformation.CaptionHeight;

            case FormBorderStyle.FixedToolWindow:
            case FormBorderStyle.SizableToolWindow:
                return SystemInformation.ToolWindowCaptionHeight;

            default:    //This should never occur.
                Debug.Assert(false, "Invalid BorderStyle enum was passed.");
                return 0;
            }
        }
    }
}