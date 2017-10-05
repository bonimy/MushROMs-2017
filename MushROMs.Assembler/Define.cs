using System.Collections.Generic;

namespace MushROMs.Assembler
{
    public class Define
    {
        public ICompiler Compiler
        {
            get;
            private set;
        }
        public int LineNumber
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        internal ICollection<Token> Tokens
        {
            get;
            private set;
        }

        internal Define(ICompiler compiler, int lineNumber, string name, ICollection<Token> tokens)
        {
            Compiler = compiler;
            LineNumber = lineNumber;
            Name = name;
            Tokens = tokens;
        }

        public ICollection<Token> GetTokens()
        {
            return Tokens;
        }

        public override string ToString()
        {
            return Token.GetListText(Tokens);
        }
    }
}
