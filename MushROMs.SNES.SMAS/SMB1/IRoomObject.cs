using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushROMs.SNES.SMAS.SMB1
{
    public interface IRoomObject
    {
        int X { get; set; }
        int Y { get; set; }
        int Page { get; set; }
        bool PageFlag { get; set; }
        int Value { get; set; }
    }
}
