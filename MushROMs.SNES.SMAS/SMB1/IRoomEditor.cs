using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushROMs.SNES.SMAS.SMB1
{
    public interface IRoomEditor : ICollection<IRoomObject>
    {
        void Undo();
        void Redo();

        void Add(IRoomObject value);
        void Remove(IRoomObject value);
    }
}
