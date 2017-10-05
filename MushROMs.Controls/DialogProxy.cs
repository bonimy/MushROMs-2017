using System;
using System.ComponentModel;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    [ToolboxItem(true)]
    [DesignTimeVisible(true)]
    public abstract class DialogProxy : MarshalByRefObject, IComponent, IDisposable
    {
        protected abstract DialogForm BaseForm
        {
            get;
        }

        public bool ShowHelp
        {
            get => BaseForm.HelpButton;
            set => BaseForm.HelpButton = value;
        }

        public event HelpEventHandler HelpRequested
        {
            add { BaseForm.HelpRequested += value; }
            remove { BaseForm.HelpRequested -= value; }
        }

        public string Title
        {
            get => BaseForm.Text;
            set => BaseForm.Text = value;
        }

        public object Tag
        {
            get => BaseForm.Tag;
            set => BaseForm.Tag = value;
        }

        public ISite Site
        {
            get => BaseForm.Site;
            set => BaseForm.Site = value;
        }

        public event EventHandler Disposed
        {
            add { BaseForm.Disposed += value; }
            remove { BaseForm.Disposed -= value; }
        }

        ~DialogProxy()
        {
            Dispose(false);
        }

        public DialogResult ShowDialog()
        {
            return BaseForm.ShowDialog();
        }
        public DialogResult ShowDialog(IWin32Window owner)
        {
            return BaseForm.ShowDialog(owner);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override ObjRef CreateObjRef(Type requestedType)
        {
            return BaseForm.CreateObjRef(requestedType);
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return BaseForm.InitializeLifetimeService();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                BaseForm.Dispose();
        }
    }
}