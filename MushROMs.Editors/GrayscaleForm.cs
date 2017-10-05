using System;
using MushROMs.Controls;

namespace MushROMs.Editors
{
    public partial class GrayscaleForm : RGBForm
    {
        public GrayscaleForm()
        {
            InitializeComponent();
        }

        public GrayscaleForm(DialogProxy dialogProxy) : base(dialogProxy)
        {
            InitializeComponent();
        }

        private void Luma_Click(object sender, EventArgs e)
        {
            RunEvent = false;
            ltbRed.Value = (int)((ColorHcy.RedWeight * 100.0f) + 0.5f);
            ltbGreen.Value = (int)((ColorHcy.GreenWeight * 100.0f) + 0.5f);
            ltbBlue.Value = (int)((ColorHcy.BlueWeight * 100.0f) + 0.5f);
            RunEvent = true;

            OnColorValueChanged(EventArgs.Empty);
        }

        private void Even_Click(object sender, EventArgs e)
        {
            RunEvent = false;
            ltbRed.Value = 100;
            ltbGreen.Value = 100;
            ltbBlue.Value = 100;
            RunEvent = true;

            OnColorValueChanged(EventArgs.Empty);
        }
    }
}
