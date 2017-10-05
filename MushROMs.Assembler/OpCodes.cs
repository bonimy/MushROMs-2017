using System;
using System.Collections.Generic;

namespace MushROMs.Assembler
{
    public delegate void AssembleOp();

    public class OpCodes
    {

        private static Dictionary<string, AssembleOp> _opDictionary;
        private static Dictionary<string, AssembleOp> OpDictionary
        {
            get
            {
                if (_opDictionary == null)
                {
                    _opDictionary = new Dictionary<string, AssembleOp>(StringComparer.OrdinalIgnoreCase);
                    _opDictionary.Add("lda", AssembleLDA);
                }
                return _opDictionary;
            }
        }

        public static bool IsOpcode(string text)
        {
            return OpDictionary.ContainsKey(text);
        }

        public static void AssembleOp(string text)
        {
            if (IsOpcode(text))
                OpDictionary[text]();
        }

        private static void AssembleLDA()
        {

        }
    }
}
