using System;
using System.Collections.Generic;
using System.Text;

namespace MushROMs.Assembler
{
    public struct Token
    {
        public static readonly Token Empty = new Token();
        
        public string Value
        {
            get;
            private set;
        }

        public TokenType TokenType
        {
            get;
            private set;
        }

        public bool IsNumberType
        {
            get
            {
                switch (TokenType)
                {
                case TokenType.BinaryNumber:
                case TokenType.DecimalNumber:
                case TokenType.HexadecimalNumber:
                    return true;
                default:
                    return false;
                }
            }
        }

        public bool IsUnaryOperator
        {
            get
            {
                switch (TokenType)
                {
                case TokenType.Addition:
                case TokenType.Subtraction:
                case TokenType.Negation:
                    return true;
                default:
                    return false;
                }
            }
        }

        public bool IsBinaryOperator
        {
            get
            {
                switch (TokenType)
                {
                case TokenType.Addition:
                case TokenType.Subtraction:
                case TokenType.Multiplication:
                case TokenType.Division:
                case TokenType.Modulo:
                case TokenType.BitShiftLeft:
                case TokenType.BitShiftRight:
                case TokenType.BitwiseAND:
                case TokenType.BitwiseOR:
                case TokenType.BitwiseXOR:
                    return true;
                default:
                    return false;
                }
            }
        }

        public Token(string value, TokenType type)
        {
            Value = value;
            TokenType = type;
        }

        internal Token(Token first, Token second)
        {
            Value = first.Value + second.Value;
            TokenType = first;
        }

        public static implicit operator TokenType(Token token)
        {
            return token.TokenType;
        }

        public override string ToString()
        {
            return Value;
        }

        public static string GetListText(ICollection<Token> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            var sb = new StringBuilder();
            foreach (var token in tokens)
                sb.Append(token.Value);
            return sb.ToString();
        }
    }
}