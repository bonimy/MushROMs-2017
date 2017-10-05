using MushROMs.Controls;

namespace MushROMs.Editors
{
    public class CreateEditorDialog : DialogProxy
    {
        private readonly CreateEditorForm _baseForm = new CreateEditorForm();

        protected override DialogForm BaseForm => _baseForm;
        public Editor CreateEditor()
        {
            return _baseForm.CreateEditor();
        }
    }
}
