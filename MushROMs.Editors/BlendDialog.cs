using Helper;

namespace MushROMs.Editors
{
    public sealed class BlendDialog : RGBDialog
    {
        private readonly BlendForm _baseForm;

        protected override RGBForm RGBForm => _baseForm;
        public BlendMode BlendMode
        {
            get => _baseForm.BlendMode;
            set => _baseForm.BlendMode = value;
        }

        public BlendDialog()
        {
            _baseForm = new BlendForm(this);
        }
    }
}
