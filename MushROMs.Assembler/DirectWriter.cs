using System;
using System.Collections.Generic;

namespace MushROMs.Assembler
{
    [Serializable]
    public class DirectWriter
    {
        public int Address
        {
            get;
            set;
        }

        public bool Resolved => Address == 0;
        public int Size => Data.Count;
        internal List<byte> Data
        {
            get;
            private set;
        }

        public DirectWriter(int address)
        {
            Address = address;
            Data = new List<byte>();
        }
    }
}
