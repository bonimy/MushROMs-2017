using System;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    public class LinkedTrackBar : TrackBar, IIntegerComponent
    {
        private IIntegerComponent _integerComponent;

        public IIntegerComponent IntegerComponent
        {
            get => _integerComponent;
            set
            {
                // Do not link to itself
                if (this == value)
                    return;

                // Avoid redundant setting.
                if (IntegerComponent == value)
                    return;

                // Remove event from last component
                if (IntegerComponent != null)
                    IntegerComponent.ValueChanged -= NumericControl_ValueChanged;

                _integerComponent = value;

                // Observe value of component
                if (IntegerComponent != null)
                    IntegerComponent.ValueChanged += new EventHandler(NumericControl_ValueChanged);
            }
        }

        protected override void OnValueChanged(EventArgs e)
        {
            IntegerComponent.Value = Value;
            base.OnValueChanged(e);
        }

        private void NumericControl_ValueChanged(object sender, EventArgs e)
        {
            if (IntegerComponent.Value >= Minimum &&
                IntegerComponent.Value <= Maximum)
                Value = IntegerComponent.Value;
        }
    }
}