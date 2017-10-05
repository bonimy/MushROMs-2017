using System;
using System.ComponentModel;
using System.Windows.Forms;
using Helper;
using MushROMs.Controls;

namespace MushROMs.Editors
{
    internal partial class BlendForm : RGBForm
    {
        public BlendMode BlendMode
        {
            get
            {
                if (rdbMultiply.Checked)
                    return BlendMode.Multiply;
                if (rdbScreen.Checked)
                    return BlendMode.Screen;
                if (rdbOverlay.Checked)
                    return BlendMode.Overlay;
                if (rdbHardLight.Checked)
                    return BlendMode.HardLight;
                if (rdbSoftLight.Checked)
                    return BlendMode.SoftLight;
                if (rdbColorDodge.Checked)
                    return BlendMode.ColorDodge;
                if (rdbLinearDodge.Checked)
                    return BlendMode.LinearDodge;
                if (rdbColorBurn.Checked)
                    return BlendMode.ColorBurn;
                if (rdbLinearBurn.Checked)
                    return BlendMode.LinearBurn;
                if (rdbVividLight.Checked)
                    return BlendMode.VividLight;
                if (rdbLinearLight.Checked)
                    return BlendMode.LinearLight;
                if (rdbDifference.Checked)
                    return BlendMode.Difference;
                if (rdbDarken.Checked)
                    return BlendMode.Darken;
                if (rdbLighten.Checked)
                    return BlendMode.Lighten;
                throw new InvalidEnumArgumentException();
            }
            set
            {
                switch (value)
                {
                case BlendMode.Multiply:
                    rdbMultiply.Checked = true;
                    break;
                case BlendMode.Screen:
                    rdbScreen.Checked = true;
                    break;
                case BlendMode.Overlay:
                    rdbOverlay.Checked = true;
                    break;
                case BlendMode.HardLight:
                    rdbHardLight.Checked = true;
                    break;
                case BlendMode.SoftLight:
                    rdbSoftLight.Checked = true;
                    break;
                case BlendMode.ColorDodge:
                    rdbColorDodge.Checked = true;
                    break;
                case BlendMode.LinearDodge:
                    rdbLinearDodge.Checked = true;
                    break;
                case BlendMode.ColorBurn:
                    rdbColorBurn.Checked = true;
                    break;
                case BlendMode.LinearBurn:
                    rdbLinearBurn.Checked = true;
                    break;
                case BlendMode.VividLight:
                    rdbVividLight.Checked = true;
                    break;
                case BlendMode.LinearLight:
                    rdbLinearLight.Checked = true;
                    break;
                case BlendMode.Difference:
                    rdbDifference.Checked = true;
                    break;
                case BlendMode.Darken:
                    rdbDarken.Checked = true;
                    break;
                case BlendMode.Lighten:
                    rdbLighten.Checked = true;
                    break;
                default:
                    throw new InvalidEnumArgumentException();
                }
            }
        }

        public BlendForm()
        {
            InitializeComponent();
        }

        internal BlendForm(DialogProxy dialogProxy) : base(dialogProxy)
        {
            InitializeComponent();
        }

        private void BlendModes_CheckedChanged(object sender, EventArgs e)
        {
            var rdb = (RadioButton)sender;
            if (rdb.Checked)
                OnColorValueChanged(EventArgs.Empty);
        }
    }
}
