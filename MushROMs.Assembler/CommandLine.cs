namespace MushROMs.Assembler
{
    public static class CommandLine
    {
        public const char EnableFlag = '+';
        public const char DisableFag = '-';
        public const char ArgumentPrefix = '/';
        public const char ArgumentSuffix = ':';

        public const string PrintMessages = "/pm";
        public const string PrintWarnings = "/pw";
        public const string PrintErrors = "/pe";
        
        public const string MessageFormat = "/mf:";
        public const string WarningFormat = "/wf:";
        public const string ErrorFormat = "/ef:";

        public const string WarningsAsErrors = "/we:";
        public const string DisableWarnings = "/dw:";
        public const string IncludeWarnings = "/iw:";

        public const string WorkingDirectory = "/dir:";
        public const string MainFile = "/main:";
        public const string InputRom = "/input:";
        public const string OutputFile = "/output:";

        public const string ExternalAssembler = "/exe:";
        public const string ExternalOptions = "/cl:";

        public const string Print = "/print:";
        public const string StringEscapeCodes = "/stresc:";

    }
}