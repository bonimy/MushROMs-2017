using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MushROMs.Assembler
{
    internal unsafe class TokenPreprocessor
    {
        private delegate void KeywordAction();

        private ICompiler Compiler
        {
            get;
            set;
        }
        
        private IDictionary<string, Define> Defines
        {
            get;
            set;
        }
        private IDictionary<string, Macro> Macros
        {
            get;
            set;
        }

        private IList<Token> Tokens
        {
            get;
            set;
        }

        private List<Token> ResolvedTokens
        {
            get;
            set;
        }
        
        private int CurrentIndex
        {
            get;
            set;
        }
        private int LineNumber
        {
            get;
            set;
        }
        
        private int MacroCallCount
        {
            get;
            set;
        }

        private int MacroDepth
        {
            get;
            set;
        }
        private bool InMacro => MacroArgDictionary != null; private IDictionary<string, string> MacroArgDictionary
        {
            get;
            set;
        }

        private Token CurrentToken => Tokens[CurrentIndex];
        private List<Token> Arguments
        {
            get;
            set;
        }
        private Token FirstArgument => Arguments[0]; private Token SecondArgument => Arguments[1];
        private IDictionary<string, KeywordAction> KeywordDictionary
        {
            get;
            set;
        }

        public static IList<Token> ResolveTokens(ICompiler compiler, IList<Token> tokens)
        {
            var tp = new TokenPreprocessor(compiler, tokens);
            return tp.ResolvedTokens;
        }

        private TokenPreprocessor(ICompiler compiler, IList<Token> tokens)
        {
            Compiler = compiler;
            Tokens = tokens;
            MacroDepth = 0;
            LineNumber = -1;
            MacroCallCount = -1;

            Defines = compiler.GetDefines();
            Macros = compiler.GetMacros();

            InitializeKeywordDictionary();

            ResolveDefinesMacrosAndIncludes();
        }

        private TokenPreprocessor(Macro macro, IList<string> args, int depth, int calls)
        {
            Compiler = macro.Compiler;
            Tokens = macro.GetTokens();
            MacroDepth = depth;
            LineNumber = macro.LineNumber;
            MacroCallCount = calls;

            Arguments = new List<Token>();
            Defines = Compiler.GetDefines();
            Macros = Compiler.GetMacros();
            InitializeKeywordDictionary();

            var margs = macro.GetArgs();
            MacroArgDictionary = new Dictionary<string, string>(margs.Count);
            for (int i = 0; i < margs.Count; i++)
                MacroArgDictionary.Add(margs[i], args[i]);

            ResolveDefinesMacrosAndIncludes();
        }

        private void ResolveDefinesMacrosAndIncludes()
        {
            ResolvedTokens = new List<Token>();
            CurrentIndex = 0;

            for (int depth = 0; CurrentToken != TokenType.EOF;)
            {
                GetArguments();

                depth = 0;
            _loop:
                TrimWhiteTokens(false);

                if (FirstArgument == TokenType.Define)
                {
                    if (SecondArgument == TokenType.Assignment)
                    {
                        AddDefine();
                        continue;
                    }
                }

                if (ResolveDefines())
                {
                    if (++depth < 0x1000)
                        goto _loop;

                    //Error: define recursion exceeded limit.
                }
                else
                    depth = 0;

                var name = FirstArgument.Value;

                if (FirstArgument == TokenType.Keyword)
                {
                    if (KeywordDictionary.ContainsKey(name))
                    {
                        KeywordDictionary[name]();
                        continue;
                    }
                }

                if (FirstArgument == TokenType.MacroCall)
                {
                    name = name.Substring(1);
                    if (Macros.ContainsKey(name))
                    {
                        ResolveMacro(Macros[name]);
                        continue;
                    }

                    // Error: Unrecognized macro.
                    continue;
                }

                TrimWhiteTokens(true);
                ResolvedTokens.AddRange(Arguments);
            }
        }

        private void AddDefine()
        {
            var name = FirstArgument.Value;
            Arguments.RemoveRange(0, 2);
            Arguments.RemoveAt(Arguments.Count - 1);

            Defines[name] = new Define(Compiler, LineNumber, name, Arguments.ToArray());
        }

        private bool ResolveDefines()
        {
            var result = false;
            for (int i = 0; i < Arguments.Count;)
            {
                if (Arguments[i] == TokenType.Define)
                {
                    var name = Arguments[i].Value;

                    if (Defines.ContainsKey(name))
                    {
                        Arguments.RemoveAt(i);
                        Arguments.InsertRange(i, Defines[name].Tokens);
                        i = 0;
                        result = true;
                        continue;
                    }
                }

                if (Arguments[i] == TokenType.MacroLabel)
                {
                    Arguments[i] = new Token(
                        Arguments[i].Value + ".Macro" + MacroCallCount.ToString(),
                        Arguments[i]);
                }

                result |= JoinKeywords();

                i++;
            }

            return result;
        }

        private bool JoinKeywords()
        {
            var result = false;
            for (int i = 0; Arguments[i] != TokenType.NewLine;)
            {
                var first = Arguments[i];
                var second = Arguments[i + 1];
                var repeated = RepeatedKeyword(first, second);
                var tt = first;

                if (repeated &&
                   (tt == TokenType.Keyword ||
                    tt == TokenType.String ||
                    tt == TokenType.InvalidCharSequence))
                {
                    Arguments[i] = new Token(Arguments[i], Arguments[i + 1]);
                    Arguments.RemoveAt(i + 1);
                    result = true;
                    continue;
                }

                i++;
            }

            return result;
        }

        private static bool RepeatedKeyword(Token first, Token second)
        {
            return first.TokenType == second.TokenType;
        }

        private void IncludeSourceFile()
        {
            RemoveFirstToken();

            var sb = new StringBuilder();
            for (int i = 0; Arguments[i] != TokenType.NewLine; i++)
                sb.Append(Arguments[i]);

            var path = sb.ToString();
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                var tokens = TextPreprocessor.Tokenize(text);
                var resolved = ResolveTokens(Compiler, tokens);

                ResolvedTokens.AddRange(resolved);
            }
        }

        private void AddMacro()
        {
            RemoveFirstToken();
            var name = FirstArgument.Value;

            RemoveFirstToken();

            var args = GetCommaSeparatedTokens();
            if (args == null)
                return;
            
            var tokens = new List<Token>();
            do
            {
                GetArguments();
                if (String.Compare(FirstArgument.Value, "endmacro", true) == 0)
                {
                    Macros[name] = new Macro(Compiler, LineNumber, name, args, tokens);
                    return;
                }
                tokens.AddRange(Arguments);

            } while (FirstArgument != TokenType.EOF);

            //Error: macro not closed.
        }

        private void ResolveMacro(Macro macro)
        {
            RemoveFirstToken();

            var args = GetCommaSeparatedTokens();
            if (args == null)
                return;

            if (args.Length != macro.GetArgs().Count)
            {
                // Error: macro parameter mismatch
                return;
            }

            if (MacroDepth >= 0x1000)
            {
                // Error: exceeded macro depth
                return;
            }

            var tp = new TokenPreprocessor(macro, args, MacroDepth + 1, MacroCallCount + 1);
            ResolvedTokens.AddRange(tp.ResolvedTokens);
            MacroCallCount = tp.MacroCallCount;
        }

        private string[] GetCommaSeparatedTokens()
        {
            if (FirstArgument != TokenType.LeftParenthesis)
            {
                // Error: Macro does not have opening parenthesis
                return null;
            }

            RemoveFirstToken();

            var args = new List<string>();
            if (FirstArgument == TokenType.RightParenthesis)
                return args.ToArray();

            for (bool comma = false; FirstArgument != TokenType.NewLine; RemoveFirstToken())
            {
                var token = FirstArgument;

                if (!comma)
                {

                    if (token != TokenType.Keyword &&
                        token != TokenType.MacroArg)
                    {
                        //Error: Macro has invalid keyword arg
                        return null;
                    }

                    args.Add(token.Value);
                    comma = true;
                    continue;
                }

                if (token == TokenType.RightParenthesis)
                    return args.ToArray();

                if (token != TokenType.CommaSeparator)
                {
                    // Error: Macro arg list is not comma separated.
                    return null;
                }

                comma = false;
            }

            return null;
        }

        private void RemoveFirstToken()
        {
            Arguments.RemoveAt(0);
            while (FirstArgument == TokenType.WhiteSpace)
                Arguments.RemoveAt(0);
            TrimWhiteTokens(true);
        }

        private void GetArguments()
        {
            Arguments = new List<Token>();
            for (; CurrentToken != TokenType.NewLine; CurrentIndex++)
            {
                Arguments.Add(CurrentToken);
            }
            Arguments.Add(CurrentToken);
            CurrentIndex++;
            LineNumber++;
        }

        private void InitializeKeywordDictionary()
        {
            KeywordDictionary = new Dictionary<string, KeywordAction>(StringComparer.InvariantCultureIgnoreCase);

            KeywordDictionary.Add("incsrc", IncludeSourceFile);
            KeywordDictionary.Add("incasm", IncludeSourceFile);
            KeywordDictionary.Add("macro", AddMacro);
        }

        private void TrimWhiteTokens(bool aggressive)
        {
            for (int i = 0; Arguments[i] != TokenType.NewLine;)
            {
                var value = Arguments[i];

                if (value == TokenType.WhiteSpace)
                {
                    if (aggressive)
                    {
                        Arguments.RemoveAt(i);
                        continue;
                    }

                    var left = Arguments[i - 1];
                    var right = Arguments[i + 1];
                    if (left == TokenType.Define)
                    {
                        if (right == TokenType.Assignment)
                        {
                            Arguments.RemoveAt(i);
                            continue;
                        }
                        i++;
                        continue;
                    }

                    if (right == TokenType.Define)
                    {
                        if (left == TokenType.Assignment)
                        {
                            Arguments.RemoveAt(i);
                            continue;
                        }
                        i++;
                        continue;
                    }

                    if (left == TokenType.Keyword &&
                        right == TokenType.Keyword)
                    {
                        i++;
                        continue;
                    }

                    Arguments.RemoveAt(i);
                    continue;
                }

                i++;
            }
        }
    }
}