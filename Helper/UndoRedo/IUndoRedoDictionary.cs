using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.UndoRedo
{
    public interface IUndoRedoDictionary<Tkey, TValue> : IUndoRedo, IDictionary<Tkey, TValue>
    {

    }
}
