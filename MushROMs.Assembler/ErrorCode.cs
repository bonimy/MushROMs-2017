namespace MushROMs.Assembler
{
    public enum ErrorCode
    {
        UnknownErrorCode = 0,
        NoAssemblyFilesSpecified = 1000,
        CouldNotFindAssemblyFile = 1001,
        CouldNotReadAssemblyFile = 1002,
        UnknownAssemblyFileLoadError = 1003,
    }
}
