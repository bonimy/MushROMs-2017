using System;

namespace Helper
{
    [Flags]
    public enum FileVisibilityFilters
    {
        None = 0,
        OpenFile = 1,
        SaveFile = 2,
        Any = OpenFile | SaveFile
    }
}
