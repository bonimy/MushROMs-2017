using System;
using Helper;
using MushROMs.Controls;

namespace MushROMs.Editors
{
    public partial class RGBForm : DialogForm
    {
        public event EventHandler ColorValueChanged;

        protected readonly DialogProxy DialogForm;

        protected override object ProxySender
        {
            get
            {
                if (DialogForm != null)
                    return DialogForm;
                return base.ProxySender;
            }
        }

        protected bool RunEvent
        {
            get;
            set;
        }

        public float Red
        {
            get => (float)ltbRed.Value / ltbRed.Maximum;
            set => ltbRed.Value = (int)(value * ltbRed.Maximum + 0.5f);
        }

        public float Green
        {
            get => (float)ltbGreen.Value / ltbGreen.Maximum;
            set => ltbGreen.Value = (int)(value * ltbGreen.Maximum + 0.5f);
        }

        public float Blue
        {
            get => (float)ltbBlue.Value / ltbBlue.Maximum;
            set => ltbBlue.Value = (int)(value * ltbBlue.Maximum + 0.5f);
        }

        public ColorF Color
        {
            get => ColorF.FromArgb(Red, Green, Blue);
            set
            {
                RunEvent = false;
                Red = Color.Red;
                Green = Color.Green;
                Blue = Color.Blue;
                RunEvent = true;

                OnColorValueChanged(EventArgs.Empty);
            }
        }

        public bool Preview
        {
            get => chkPreview.Checked;
            set => chkPreview.Checked = value;
        }

        public RGBForm()
        {
            InitializeComponent();

            ResetValues();
            RunEvent = true;
        }

        internal RGBForm(DialogProxy dialogForm) : this()
        {
            DialogForm = dialogForm;
        }

        public void ResetValues()
        {
            RunEvent = false;
            ltbRed.Value = ltbRed.Maximum;
            ltbGreen.Value = ltbGreen.Maximum;
            ltbBlue.Value = ltbBlue.Maximum;
            RunEvent = true;

            OnColorValueChanged(EventArgs.Empty);
        }

        protected virtual void OnColorValueChanged(EventArgs e)
        {
            if (RunEvent)
                ColorValueChanged?.Invoke(ProxySender, e);
        }

        private void RGB_ValueChanged(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }

        private void chkPreview_CheckedChanged(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }

        private void Form_Shown(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }
    }
}