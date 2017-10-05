using Helper;

namespace MushROMs
{
    public delegate IEditor InitializeEditorMethod(byte[] data);
    public delegate byte[] SaveFileDataMethod(IEditor editor);

    public interface IFileAssociation
    {
        string Extension
        {
            get;
        }
        InitializeEditorMethod InitializeEditorMethod
        {
            get;
        }
        SaveFileDataMethod SaveFileDataMethod
        {
            get;
        }
        FileVisibilityFilters Filter
        {
            get;
        }
    }
}
