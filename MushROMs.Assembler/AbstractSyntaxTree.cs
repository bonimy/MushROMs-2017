using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MushROMs.SNES;

namespace MushROMs.Assembler
{
    internal class AbstractSyntaxTree
    {
        private delegate void TokenAction();

        private Builder Builder
        {
            get;
            set;
        }

        private IList<TokenAction> BuildCommands
        {
            get;
            set;
        }

        private IList<Token> Tokens
        {
            get;
            set;
        }

        private IDictionary<string, Label> Labels
        {
            get;
            set;
        }

        private IDictionary<string, TokenAction> KeywordDictionary
        {
            get;
            set;
        }

        private IDictionary<TokenType, TokenAction> TokenDictionary
        {
            get;
            set;
        }

        private int CurrentIndex
        {
            get;
            set;
        }
        private Token CurrentToken => Tokens[CurrentIndex];
        private int BlockNumber
        {
            get;
            set;
        }
        private TokenType LastSeparator
        {
            get;
            set;
        }

        private IList<Token> Arguments
        {
            get;
            set;
        }
        private int ArgumentIndex
        {
            get;
            set;
        }
        private Token CurrentArgument => Arguments[ArgumentIndex];
        public static void GenerateTree(Builder builder, IList<Token> tokens)
        {
            var tree = new AbstractSyntaxTree(builder, tokens);
        }

        internal AbstractSyntaxTree(Builder builder, IList<Token> tokens)
        {
            Builder = builder;
            Tokens = tokens;

            InitializeTokenDictionary();
            InitializeKeywordDictionary();
            Labels = new Dictionary<string, Label>();

            Build();
        }

        private void Build()
        {
            LastSeparator = TokenType.EOF;
            
            for (CurrentIndex = 0; CurrentToken != TokenType.EOF;)
            {
                GetArguments();

                if (Arguments.Count <= 1)
                    continue;

                if (TokenDictionary.ContainsKey(CurrentArgument))
                    TokenDictionary[CurrentArgument]();
            }
        }

        private void GetArguments()
        {
            Arguments = new List<Token>();

            for (; CurrentToken != TokenType.NewLine; CurrentIndex++)
            {
                if (CurrentToken == TokenType.BlockSeparator)
                {
                    if (LastSeparator == TokenType.BlockSeparator)
                        BlockNumber++;

                    Arguments.Add(CurrentToken);
                    LastSeparator = TokenType.BlockSeparator;
                    CurrentIndex++;
                    return;
                }
                Arguments.Add(CurrentToken);
            }

            Arguments.Add(CurrentToken);
            LastSeparator = TokenType.NewLine;
            CurrentIndex++;
            BlockNumber = 0;
        }

        private void ReadKeyword()
        {
            if (KeywordDictionary.ContainsKey(CurrentArgument.Value))
                KeywordDictionary[CurrentArgument.Value]();
        }

        private void InitializeTokenDictionary()
        {
            TokenDictionary = new Dictionary<TokenType, TokenAction>();
            TokenDictionary.Add(TokenType.Keyword, ReadKeyword);
        }

        private string GetString()
        {
            var sb = new StringBuilder();
            while (CurrentToken != TokenType.NewLine)
                sb.Append(CurrentToken.Value);
            return sb.ToString();
        }

        private Calculation GetCalculation()
        {
            var expressions = GetMathExpressions();

            return () => CalculateMathExpressions(expressions);
        }

        private List<MathExpression> GetMathExpressions()
        {
            var expressions = new List<MathExpression>();
            for (int index = 0; true; index++)
            {
                var expression = GetMathExpression();
                expressions.Add(expression);
                if (expression.BinaryOperator == BinaryOperator.None)
                    break;

            }
            return expressions;
        }

        private int CalculateMathExpressions(IList<MathExpression> expressions)
        {
            for (int order = 0; order < 20; order++)
            {
                for (int i = 0; expressions[i].BinaryOperator != BinaryOperator.None;)
                {
                    var binary = expressions[i].BinaryOperator;
                    if (BinaryOperatorPrecedenceComparer.GetPrecedence(binary) != order)
                    {
                        i++;
                        continue;
                    }

                    var expr = new BinaryExpression(expressions[i].Calculation, expressions[i + 1].Calculation, binary);
                    expressions.RemoveAt(i);
                    expressions[i].Calculation = expr.Result;
                }
            }
            return expressions[0].Calculation(); ;
        }

        private MathExpression GetMathExpression()
        {
            ArgumentIndex++;
            var unary = UnaryOperator.None;
            if (CurrentArgument.IsUnaryOperator)
            {
                unary = (UnaryOperator)CurrentArgument.TokenType;
                ArgumentIndex++;
            }

            Calculation calculation;
            if (CurrentArgument.IsNumberType)
            {
                var value = ParseNumber(CurrentArgument);
                calculation = () => value;
            }
            else if (CurrentArgument == TokenType.Keyword)
            {
                var name = CurrentArgument.Value;
                if (!Labels.ContainsKey(name))
                    Labels.Add(CurrentArgument.Value, new Label(name));

                calculation = () => Labels[name].Address;
            }
            else if (CurrentArgument == TokenType.LeftParenthesis)
            {
                ArgumentIndex++;
                calculation = GetCalculation();
                if (CurrentArgument != TokenType.RightParenthesis)
                {
                    // Error. Parenthesis not closed.
                    return new MathExpression(unary, calculation, BinaryOperator.None);
                }
            }
            else
            {
                // Error: Expected a value.
                return new MathExpression(unary, () => 0, BinaryOperator.None);
            }

            ArgumentIndex++;
            if (!CurrentArgument.IsBinaryOperator)
                return new MathExpression(unary, calculation, BinaryOperator.None);

            var binary = (BinaryOperator)CurrentArgument.TokenType;
            ArgumentIndex++;
            return new MathExpression(unary, calculation, binary);
        }

        private static int ParseNumber(Token token)
        {
            switch (token.TokenType)
            {
            case TokenType.DecimalNumber:
                return ParseDecimal(token);
            case TokenType.HexadecimalNumber:
                return ParseHexadecimal(token);
            case TokenType.BinaryNumber:
                return ParseBinary(token);
            default:
                throw new ArgumentException(nameof(token));
            }
        }

        private static int ParseDecimal(Token token)
        {
            return Int32.Parse(token.Value);
        }

        private static int ParseHexadecimal(Token token)
        {
            return Int32.Parse(token.Value.Substring(1), NumberStyles.AllowHexSpecifier);
        }

        private static int ParseBinary(Token token)
        {
            var s = token.Value;
            var len = s.Length;
            var value = 0;

            for (int i = 1; i < len; i++)
            {
                if (s[i] == '_')
                    continue;

                value <<= 1;
                value |= s[i] - '0';
            }

            return value;
        }

        private void InitializeKeywordDictionary()
        {
            KeywordDictionary = new Dictionary<string, TokenAction>(StringComparer.InvariantCultureIgnoreCase);

            KeywordDictionary.Add("header"  , () => Builder.SetHeader(HeaderType.Header));
            KeywordDictionary.Add("noheader", () => Builder.SetHeader(HeaderType.NoHeader));
            KeywordDictionary.Add("hirom"   , () => Builder.SetAddressMode(AddressMode.HiROM));
            KeywordDictionary.Add("lorom"   , () => Builder.SetAddressMode(AddressMode.LoROM));
            KeywordDictionary.Add("exhirom" , () => Builder.SetAddressMode(AddressMode.ExHiROM));
            KeywordDictionary.Add("exlorom" , () => Builder.SetAddressMode(AddressMode.ExLoROM));
            KeywordDictionary.Add("org", () => Builder.SetPosition(GetCalculation()()));
            /*
             * incbin
             * incchr
             * inclz2
             * inctpl
             * incmw3
             * incpal
             * incrpf
             * print
             * assume
             * fill
             * fillbyte
             * db
             * dw
             * dl
             * dd
             * pad
             * padbyte
             * table
             * cleartable
             * skip
             * namespace
             * import
             * export
             * org
             * loadpc
             * savepc
             * warnpc
             * base
             * 
             * adc
             * ...
             * */
        }
    }
}
