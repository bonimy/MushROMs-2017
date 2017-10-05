namespace MushROMs
{
    public interface ISelectionData
    {
        ISelection Selection
        {
            get;
        }

        ISelectionData Copy(ISelection selection);

        void WriteToEditor(IEditor editor);
    }
}
