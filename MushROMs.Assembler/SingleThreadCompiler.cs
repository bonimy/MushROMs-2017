using System.Collections.Generic;

namespace MushROMs.Assembler
{
    public class SingleThreadCompiler : ICompiler
    {
        public string Text
        {
            get;
            private set;
        }
        
        private Dictionary<string, Define> Defines
        {
            get;
            set;
        }
        private Dictionary<string, Macro> Macros
        {
            get;
            set;
        }

        public Builder Builder
        {
            get;
            private set;
        }

        private IList<Token> Tokens
        {
            get;
            set;
        }
        private IList<Token> ResolvedTokens
        {
            get;
            set;
        }

        internal SingleThreadCompiler(string text, MultiThreadedCompiler parent)
        {
            Text = text;

            Defines = new Dictionary<string, Define>();
            Macros = new Dictionary<string, Macro>();

            Builder = new Builder();
        }

        public void Compile()
        {
            Tokens = TextPreprocessor.Tokenize(Text);
            ResolvedTokens = TokenPreprocessor.ResolveTokens(this, Tokens);
            AbstractSyntaxTree.GenerateTree(Builder, ResolvedTokens);
        }

        public IDictionary<string, Define> GetDefines()
        {
            return Defines;
        }
        public IDictionary<string, Macro> GetMacros()
        {
            return Macros;
        }

        public IList<Token> GetTokens()
        {
            return Tokens;
        }
        public IList<Token> GetResolvedTokens()
        {
            return ResolvedTokens;
        }

        public Define AddDefine(string name, string value)
        {
            var tokens = TextPreprocessor.Tokenize(value);
            return Defines[name] = new Define(this, -1, name, tokens);
        }

        public Macro AddMacro(string name, IList<string> args, string content)
        {
            var tokens = TextPreprocessor.Tokenize(content);
            return Macros[name] = new Macro(this, -1, name, args, tokens);
        }
    }
}