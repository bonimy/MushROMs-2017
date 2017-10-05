using System;

namespace MushROMs.Controls
{
    public interface IIntegerComponent
    {
        event EventHandler ValueChanged;

        int Value
        {
            get;
            set;
        }
    }
}