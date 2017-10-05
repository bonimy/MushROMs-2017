using System.Collections.Generic;

namespace MushROMs.Assembler
{
    public interface ICompiler
    {
        IDictionary<string, Define> GetDefines();
        IDictionary<string, Macro> GetMacros();
    }
}
