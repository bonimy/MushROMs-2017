using System;
using Helper;
using MushROMs.Controls;

namespace MushROMs.Editors
{
    public sealed class ColorizeDialog : DialogProxy
    {
        private readonly ColorizeForm _baseForm;

        protected override DialogForm BaseForm => _baseForm;
        public event EventHandler ValueChanged
        {
            add { _baseForm.ColorValueChanged += value; }
            remove { _baseForm.ColorValueChanged -= value; }
        }

        public float Hue
        {
            get => _baseForm.Hue;
            set => _baseForm.Hue = value;
        }
        public float Saturation
        {
            get => _baseForm.Saturation;
            set => _baseForm.Saturation = value;
        }
        public float Lightness
        {
            get => _baseForm.Lightness;
            set => _baseForm.Lightness = value;
        }

        public float Alpha
        {
            get => _baseForm.Alpha;
            set => _baseForm.Alpha = value;
        }

        public bool ColorizerMode
        {
            get => _baseForm.ColorizerMode;
            set => _baseForm.ColorizerMode = value;
        }

        public bool Luma
        {
            get => _baseForm.Luma;
            set => _baseForm.Luma = value;
        }

        public bool Preview
        {
            get => _baseForm.Preview;
            set => _baseForm.Preview = value;
        }

        public ColorizeDialog()
        {
            _baseForm = new ColorizeForm(this);
        }

        public void ResetValues()
        {
            _baseForm.ResetValues();
        }
    }
}
