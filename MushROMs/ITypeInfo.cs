using System;

namespace MushROMs
{
    public interface ITypeInfo
    {
        Type Type
        {
            get;
        }
        string DisplayName
        {
            get;
        }
        string Description
        {
            get;
        }
    }
}
