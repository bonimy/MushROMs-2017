using System;
using System.Collections.Generic;
using System.Text;

namespace MushROMs.Assembler
{
    internal unsafe class TextPreprocessor
    {
        private delegate void ParseAction();

        private string Text
        {
            get;
            set;
        }
        private int Length
        {
            get;
            set;
        }

        private char* UnsafeText
        {
            get;
            set;
        }

        private int StartIndex
        {
            get;
            set;
        }
        private int CurrentIndex
        {
            get;
            set;
        }
        private int CurrentLength => CurrentIndex - StartIndex; private char CurrentChar => UnsafeText[CurrentIndex]; private string CurrentText => new string(UnsafeText, StartIndex, CurrentLength);
        private Dictionary<char, ParseAction> PreprocessorDictionary
        {
            get;
            set;
        }

        private List<Token> PreprocessorTokens
        {
            get;
            set;
        }

        public static IList<Token> Tokenize(string text)
        {
            var tp = new TextPreprocessor(text);
            return tp.PreprocessorTokens;
        }

        private TextPreprocessor(string text)
        {
            Text = text??throw new ArgumentNullException(nameof(text));
            Length = Text.Length;
            ResolveTrigraphs();
            TrimWhite();

            InitializePreprocessorDictionary();

            fixed(char* ptr = Text)
            {
                UnsafeText = ptr;
                ParseTokens();
            }
        }

        private void ResolveTrigraphs()
        {
            var sb = new StringBuilder(Length);

            fixed (char* str = Text)
            {
                for (int i = 0; i < Length;)
                {
                    if (str[i] == '\r')
                    {
                        sb.Append('\n');

                        i++;
                        if (str[i] == '\n')
                            i++;
                        continue;
                    }

                    if (str[i] == '?' && str[i + 1] == '?')
                    {
                        char c = str[i];
                        if (IsTrigraphChar(str[i + 2], ref c))
                        {
                            sb.Append(c);
                            i += 3;
                            continue;
                        }
                        else
                        {
                            sb.Append('?');
                            sb.Append('?');
                            i += 2;
                            continue;
                        }
                    }

                    sb.Append(str[i++]);
                }
            }

            if (sb.Length > 0)
            {
                if (sb[sb.Length - 1] != '\n')
                    sb.Append('\n');
            }

            Length = sb.Length;
            Text = sb.ToString();
        }

        private void TrimWhite()
        {
            var sb = new StringBuilder(Length);
            var last = '\n';

            fixed (char* str = Text)
            {
                for (int i = 0; i < Length;)
                {
                    _loop:
                    if (Grammar.IsWhiteSpace(str[i]))
                    {
                        do i++;
                        while (Grammar.IsWhiteSpace(str[i]));

                        goto _white;
                    }

                    if (str[i] == ';')
                    {
                        do i++;
                        while (!Grammar.IsLineSeparator(str[i]));

                        goto _white;
                    }

                    if (str[i] == '/' && str[i + 1] == '*')
                    {
                        i += 2;
                        if (last != ' ')
                            sb.Append(' ');

                        for (; i < Length; i++)
                        {
                            if (str[i] == '*' && str[i + 1] == '/')
                            {
                                i += 2;
                                break;
                            }

                            if (str[i] == '\n')
                            {
                                int len = sb.Length;
                                if (len >= 1 && sb[len - 1] == ' ')
                                    sb[len - 1] = '\n';
                                else
                                    sb.Append('\n');
                                continue;
                            }
                        }

                        // Block comment not closed.
                        continue;
                    }

                    if (str[i] == '\"' || str[i] == '\'')
                    {
                        sb.Append(last = str[i++]);

                        while (str[i] != '\n')
                        {
                            if (str[i] == last)
                            {
                                sb.Append(last);
                                i++;
                                goto _loop;
                            }

                            sb.Append(str[i++]);
                        }

                        //String not closed.
                        sb.Append(last);
                        continue;
                    }

                    if (str[i] == '\n')
                    {
                        int len = sb.Length;
                        if (len >= 1 && sb[len - 1] == ' ')
                            sb[len - 1] = '\n';
                        else
                            sb.Append('\n');

                        i++;
                        last = '\n';
                        continue;
                    }

                    sb.Append(last = str[i++]);
                    continue;

                    _white:
                    if (last != ' ' && last != '\n')
                        sb.Append(last = ' ');
                }
            }

            Length = sb.Length;
            Text = sb.ToString();
        }

        private void ParseTokens()
        {
            PreprocessorTokens = new List<Token>();

            for (CurrentIndex = 0; CurrentIndex < Length; )
            {
                if (IsValidChar(CurrentChar))
                    PreprocessorDictionary[CurrentChar]();
                else
                    AddMaskToken(c => !IsValidChar(c), TokenType.InvalidCharSequence);
            }

            PreprocessorDictionary['\0']();
        }

        private bool IsValidChar(char c)
        {
            return PreprocessorDictionary.ContainsKey(c);
        }
        
        private void InitializePreprocessorDictionary()
        {
            PreprocessorDictionary = new Dictionary<char, ParseAction>(0x80);

            PreprocessorDictionary.Add('\0', () => AddSingleCharToken(TokenType.EOF));
            PreprocessorDictionary.Add('\n', () => AddSingleCharToken(TokenType.NewLine));
            PreprocessorDictionary.Add(' ', () => AddMaskToken(Grammar.IsWhiteSpace, TokenType.WhiteSpace));

            for (char x = 'a'; x <= 'z'; x++)
                PreprocessorDictionary.Add(x, AddKeywordOrLabel);

            for (char x = 'A'; x <= 'Z'; x++)
                PreprocessorDictionary.Add(x, AddKeywordOrLabel);

            for (char x = '0'; x <= '9'; x++)
                PreprocessorDictionary.Add(x, () => AddMaskToken(Grammar.IsDigit, TokenType.DecimalNumber));

            PreprocessorDictionary.Add('_', AddKeywordOrLabel);
            PreprocessorDictionary.Add('{', () => AddSingleCharToken(TokenType.LeftBrace));
            PreprocessorDictionary.Add('}', () => AddSingleCharToken(TokenType.RightBrace));
            PreprocessorDictionary.Add('[', () => AddSingleCharToken(TokenType.LeftBracket));
            PreprocessorDictionary.Add('\\', () => AddSingleCharToken(TokenType.FileSeparator));
            PreprocessorDictionary.Add(']', () => AddSingleCharToken(TokenType.RightBracket));
            PreprocessorDictionary.Add('#', () => AddSingleCharToken(TokenType.DirectAddress));
            PreprocessorDictionary.Add('(', () => AddSingleCharToken(TokenType.LeftParenthesis));
            PreprocessorDictionary.Add(')', () => AddSingleCharToken(TokenType.RightParenthesis));
            PreprocessorDictionary.Add('?', AddMacroSubabel);
            PreprocessorDictionary.Add('<', AddBitShiftLeft);
            PreprocessorDictionary.Add('>', AddBitShiftRight);
            PreprocessorDictionary.Add('%', AddBinaryModuloOrMacro);
            PreprocessorDictionary.Add(':', AddBlockOrNamespaceSeparator);
            PreprocessorDictionary.Add('.', () => AddSingleCharToken(TokenType.SublabelOrOpSizeSpecifier));
            PreprocessorDictionary.Add('*', () => AddSingleCharToken(TokenType.Multiplication));
            PreprocessorDictionary.Add('+', AddAddition);
            PreprocessorDictionary.Add('-', AddSubtraction);
            PreprocessorDictionary.Add('/', () => AddSingleCharToken(TokenType.Division));
            PreprocessorDictionary.Add('^', () => AddSingleCharToken(TokenType.BitwiseXOR));
            PreprocessorDictionary.Add('&', () => AddSingleCharToken(TokenType.BitwiseAND));
            PreprocessorDictionary.Add('|', () => AddSingleCharToken(TokenType.BitwiseOR));
            PreprocessorDictionary.Add('~', () => AddSingleCharToken(TokenType.Negation));
            PreprocessorDictionary.Add('!', AddDefine);
            PreprocessorDictionary.Add('=', () => AddSingleCharToken(TokenType.Assignment));
            PreprocessorDictionary.Add(',', () => AddSingleCharToken(TokenType.CommaSeparator));
            PreprocessorDictionary.Add('"', AddDString);
            PreprocessorDictionary.Add('\'', AddString);
            PreprocessorDictionary.Add('$', () => AddMaskToken(Grammar.IsHexDigit, TokenType.HexadecimalNumber));
        }

        private void AddSingleCharToken(TokenType type)
        {
            StartIndex = CurrentIndex++;
            AddToken(type);
        }

        private void AddMaskToken(CharTypeTest test, TokenType type)
        {
            StartIndex = CurrentIndex++;
            while (test(CurrentChar))
                CurrentIndex++;
            AddToken(type);
        }

        private void AddNewLine()
        {
            StartIndex = CurrentIndex++;
            AddToken(TokenType.NewLine);
        }

        private void AddKeywordOrLabel()
        {
            StartIndex = CurrentIndex++;

            while (Grammar.IsKeywordChar(CurrentChar))
                CurrentIndex++;

            if (CurrentChar == ':')
            {
                CurrentIndex++;
                AddToken(TokenType.Label);
                return;
            }

            if (CurrentChar == '.')
            {
                AddToken(TokenType.Keyword);
                StartIndex = CurrentIndex++;
                AddToken(TokenType.OpSizeSeparator);
                return;
            }

            if (String.Compare(CurrentText, "equ", StringComparison.InvariantCultureIgnoreCase) == 0)
                AddToken(TokenType.Assignment);
            else
                AddToken(TokenType.Keyword);
        }

        private void AddMacroSubabel()
        {
            StartIndex = CurrentIndex++;
            if (Grammar.IsNonDigit(CurrentChar))
            {
                CurrentIndex++;
                while (Grammar.IsKeywordChar(CurrentChar))
                    CurrentIndex++;

                AddToken(TokenType.MacroLabel);
                return;
            }

            AddToken(TokenType.InvalidCharSequence);
        }

        private void AddBitShiftLeft()
        {
            StartIndex = CurrentIndex++;
            if (CurrentChar == '<')
            {
                CurrentIndex++;
                AddToken(TokenType.BitShiftLeft);
                return;
            }

            if (Grammar.IsNonDigit(CurrentChar))
            {
                var index = CurrentIndex++;
                while (Grammar.IsKeywordChar(UnsafeText[index]))
                    index++;
                if (UnsafeText[index] == '>')
                {
                    CurrentIndex = index + 1;
                    AddToken(TokenType.MacroArg);
                    return;
                }
            }

            AddToken(TokenType.InvalidCharSequence);
        }

        private void AddBitShiftRight()
        {
            StartIndex = CurrentIndex++;
            if (CurrentChar == '>')
            {
                CurrentIndex++;
                AddToken(TokenType.BitShiftRight);
                return;
            }

            AddToken(TokenType.InvalidCharSequence);
        }

        private void AddBinaryModuloOrMacro()
        {
            StartIndex = CurrentIndex++;
            if (Grammar.IsBinaryDigit(CurrentChar))
            {
                do CurrentIndex++;
                while (Grammar.IsBinaryDigit(CurrentChar) || CurrentChar == '_');

                AddToken(TokenType.BinaryNumber);
                return;
            }

            if (Grammar.IsNonDigit(CurrentChar))
            {
                CurrentIndex++;
                while (Grammar.IsKeywordChar(CurrentChar))
                    CurrentIndex++;

                AddToken(TokenType.MacroCall);
                return;
            }

            AddToken(TokenType.Modulo);
        }

        private void AddBlockOrNamespaceSeparator()
        {
            StartIndex = CurrentIndex++;
            if (CurrentChar == ':')
            {
                StartIndex = CurrentIndex++;
                AddToken(TokenType.NamespaceSeparator);
                return;
            }

            AddToken(TokenType.BlockSeparator);
        }

        private void AddAddition()
        {
            StartIndex = CurrentIndex++;
            if (CurrentChar == '+')
            {
                do CurrentIndex++;
                while (CurrentChar == '+');

                AddToken(TokenType.ForwardLabel);
                return;
            }

            AddToken(TokenType.Addition);
        }

        private void AddSubtraction()
        {
            StartIndex = CurrentIndex++;
            if (CurrentChar == '-')
            {
                do CurrentIndex++;
                while (CurrentChar == '-');

                AddToken(TokenType.BackwardLabel);
                return;
            }

            AddToken(TokenType.Subtraction);
        }

        private void AddDefine()
        {
            StartIndex = CurrentIndex++;
            if (Grammar.IsNonDigit(CurrentChar))
            {
                do CurrentIndex++;
                while (Grammar.IsKeywordChar(CurrentChar));

                AddToken(TokenType.Define);

                return;
            }

            AddToken(TokenType.InvalidCharSequence);
        }

        private void AddDString()
        {
            // We don't include the quotations in the string value.
            for (StartIndex = ++CurrentIndex; CurrentChar != '\0'; CurrentIndex++)
            {
                if (CurrentChar == '\"')
                {
                    AddToken(TokenType.String);
                    CurrentIndex++;
                    return;
                }
                if (Grammar.IsLineSeparator(CurrentChar))
                    break;
            }
        }

        private void AddString()
        {
            // We don't include the quotations in the string value.
            for (StartIndex = ++CurrentIndex; CurrentChar != '\0'; CurrentIndex++)
            {
                if (CurrentChar == '\'')
                {
                    AddToken(TokenType.String);
                    CurrentIndex++;
                    return;
                }
                if (Grammar.IsLineSeparator(CurrentChar))
                    break;
            }
        }
        
        private void AddToken(TokenType tokenType)
        {
            if (CurrentLength > 0)
                PreprocessorTokens.Add(new Token(CurrentText, tokenType));
        }

        private static bool IsTrigraphChar(char code, ref char value)
        {
            switch (code)
            {
            case '=':
                value = '#';
                return true;
            case ')':
                value = ']';
                return true;
            case '!':
                value = '|';
                return true;
            case '(':
                value = '[';
                return true;
            case '\'':
                value = '^';
                return true;
            case '>':
                value = '}';
                return true;
            case '/':
                value = '\\';
                return true;
            case '<':
                value = '{';
                return true;
            case '-':
                value = '~';
                return true;
            default:
                return false;
            }
        }
    }
}
