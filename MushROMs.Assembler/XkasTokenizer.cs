using System;
using System.Collections.Generic;
using System.Text;

namespace MushROMs.Assembler
{
    public unsafe class XkasTokenizer
    {
        private List<Node> Tokens
        {
            get;
            set;
        }

        public string Text
        {
            get;
            private set;
        }

        private char* UnsafeText
        {
            get;
            set;
        }
    }
}
