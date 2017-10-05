using System;
using Helper;
using MushROMs.Controls;

namespace MushROMs.Editors
{
    public abstract class RGBDialog : DialogProxy
    {
        protected abstract RGBForm RGBForm
        {
            get;
        }
        protected sealed override DialogForm BaseForm => RGBForm;
        public event EventHandler ValueChanged
        {
            add { RGBForm.ColorValueChanged += value; }
            remove { RGBForm.ColorValueChanged -= value; }
        }

        public float Red
        {
            get => RGBForm.Red;
            set => RGBForm.Red = value;
        }
        public float Green
        {
            get => RGBForm.Green;
            set => RGBForm.Green = value;
        }
        public float Blue
        {
            get => RGBForm.Blue;
            set => RGBForm.Blue = value;
        }

        public ColorF Color
        {
            get => RGBForm.Color;
            set => RGBForm.Color = value;
        }

        public bool Preview
        {
            get => RGBForm.Preview;
            set => RGBForm.Preview = value;
        }

        public void ResetValues()
        {
            RGBForm.ResetValues();
        }
    }
}
