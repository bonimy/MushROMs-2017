using System;
using System.Collections.Generic;

namespace MushROMs.Assembler
{
    public class Macro
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
        private IList<string> Args
        {
            get;
            set;
        }
        private IList<Token> Tokens
        {
            get;
            set;
        }

        internal Macro(ICompiler compiler, int lineNumber, string name, IList<string> args, IList<Token> tokens)
        {
            Compiler = compiler;
            LineNumber = lineNumber;
            Name = name;
            Args = args;
            Tokens = new List<Token>(tokens);
            Tokens.Add(new Token(String.Empty, TokenType.EOF));
        }

        public IList<string> GetArgs()
        {
            return Args;
        }

        public IList<Token> GetTokens()
        {
            return Tokens;
        }

        public override string ToString()
        {
            return Token.GetListText(Tokens);
        }
    }
}
