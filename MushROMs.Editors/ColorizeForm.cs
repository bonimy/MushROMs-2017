using System;
using MushROMs.Controls;
using MushROMs.Editors.Properties;
using Helper;

namespace MushROMs.Editors
{
    public sealed partial class ColorizeForm : DialogForm
    {
        private readonly DialogProxy DialogProxy;

        protected override object ProxySender
        {
            get
            {
                if (DialogProxy != null)
                    return DialogProxy;
                return base.ProxySender;
            }
        }

        public event EventHandler ColorValueChanged;
        
        private static readonly ColorF FallbackAdjust = ColorF.FromHsl(1, 0, 0, 0);
        private static readonly ColorF FallbackColorize = ColorF.FromHsl(1, 0.25f, 0.50f, 0.50f);
        
        private ColorF SavedAdjust, SavedColorize;
        private bool RunEvent;

        public float Alpha
        {
            get => (float)ltbAlpha.Value / ltbAlpha.Maximum;
            set => ltbAlpha.Value = (int)(value * ltbAlpha.Maximum + 0.5f);
        }

        public float Hue
        {
            get => (float)ltbHue.Value / ltbHue.Maximum;
            set => ltbHue.Value = (int)(value * ltbHue.Maximum + 0.5f);
        }

        public float Saturation
        {
            get => (float)ltbSaturation.Value / ltbSaturation.Maximum;
            set => ltbSaturation.Value = (int)(value * ltbSaturation.Maximum + 0.5f);
        }

        public float Lightness
        {
            get => (float)ltbLightness.Value / ltbLightness.Maximum;
            set => ltbLightness.Value = (int)(value * ltbLightness.Maximum + 0.5f);
        }

        private ColorF CurrentHSL
        {
            get => ColorF.FromHsl(Alpha, Hue, Saturation, Lightness);
            set
            {
                RunEvent = false;
                Alpha = value.Alpha;
                Hue = value.Hue;
                Saturation = value.Saturation;
                Lightness = value.Lightness;
                RunEvent = true;
                OnColorValueChanged(EventArgs.Empty);
            }
        }

        public bool ColorizerMode
        {
            get => chkColorize.Checked;
            set => chkColorize.Checked = value;
        }

        public bool Luma
        {
            get => chkLuma.Checked;
            set => chkLuma.Checked = value;
        }

        public bool Preview
        {
            get => chkPreview.Checked;
            set => chkPreview.Checked = value;
        }

        public ColorizeForm()
        {
            InitializeComponent();

            SavedAdjust = FallbackAdjust;
            SavedColorize = FallbackColorize;

            ResetValues();

            RunEvent = true;
        }

        public ColorizeForm(DialogProxy dialogProxy) : this()
        {
            DialogProxy = dialogProxy;
        }

        public void ResetValues()
        {
            if (ColorizerMode)
            {
                CurrentHSL = FallbackColorize;
            }
            else
                CurrentHSL = FallbackAdjust;
            
            btnReset.Enabled = false;
        }

        private void SwitchValues()
        {
            if (ColorizerMode)
            {
                SavedAdjust = CurrentHSL;

                ltbHue.Minimum = 0;
                ltbHue.Maximum = 360;
                ltbSaturation.Minimum = ltbLightness.Minimum = 0;
                ltbSaturation.Maximum = ltbLightness.Maximum = 100;
                ltbSaturation.TickFrequency = ltbLightness.TickFrequency = 5;

                CurrentHSL = SavedColorize;
            }
            else
            {
                SavedColorize = CurrentHSL;

                ltbHue.Maximum = 180;
                ltbHue.Minimum = -ltbHue.Maximum;
                ltbSaturation.Maximum = 100;
                ltbSaturation.Minimum = -ltbSaturation.Maximum;
                ltbLightness.Maximum = 100;
                ltbLightness.Minimum = -ltbLightness.Maximum;
                ltbSaturation.TickFrequency = ltbLightness.TickFrequency = 10;

                CurrentHSL = SavedAdjust;
            }
        }

        private void OnColorValueChanged(EventArgs e)
        {
            if (RunEvent)
                ColorValueChanged?.Invoke(ProxySender, e);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ResetValues();
        }

        private void Colorize_CheckedChanged(object sender, EventArgs e)
        {
            SwitchValues();
        }

        private void Preview_CheckedChanged(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }

        private void HSLE_ValueChanged(object sender, EventArgs e)
        {
            btnReset.Enabled = true;
            OnColorValueChanged(EventArgs.Empty);
        }

        private void ColorizeForm_Shown(object sender, EventArgs e)
        {
            OnColorValueChanged(EventArgs.Empty);
        }

        private void chkLuma_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLuma.Checked)
            {
                lblSaturation.Text = Resources.ChromaText;
                lblLightness.Text = Resources.LumaText;
            }
            else
            {
                lblSaturation.Text = Resources.SaturationText;
                lblLightness.Text = Resources.LightnessText;
            }

            OnColorValueChanged(EventArgs.Empty);
        }
    }
}