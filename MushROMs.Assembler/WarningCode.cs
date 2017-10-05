namespace MushROMs.Assembler
{
    public enum WarningCode
    {
        CantFindSourceROMFile = 1000,
        CantReadSourceROMFile = 1001,
        UnknownErrorReadingSourceROM = 1002,

        SourceROMDataBadFormat = 1010,

        AssemblyFileLoadedTwice = 2000,
    }
}
