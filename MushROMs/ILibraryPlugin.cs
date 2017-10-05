namespace MushROMs
{
    public interface ILibraryPlugin
    {
        ITypeInfo[] GetEditorInfoList();
        IFileAssociation[] GetFileAssociations();
    }
}
