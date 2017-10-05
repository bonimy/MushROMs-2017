using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.UndoRedo
{
    public interface IUndoRedo
    {
        bool CanUndo { get; }
        bool CanRedo { get; }

        void Undo();
        void Redo();
    }
}
