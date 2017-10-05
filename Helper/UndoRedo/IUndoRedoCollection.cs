using System.Collections.Generic;

namespace Helper.UndoRedo
{
    public interface IUndoRedoCollection<T> : IUndoRedo, ICollection<T>
    {
    }
}
