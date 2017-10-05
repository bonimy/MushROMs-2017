using System;

namespace MushROMs.Assembler
{
    public delegate bool CharTypeTest(char c);

    public static class Grammar
    {
        public static bool IsEOF(char c)
        {
            return c == '\0';
        }

        public static bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        public static bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        public static bool IsAlpha(char c)
        {
            return IsLower(c) || IsUpper(c);
        }

        public static bool IsNonDigit(char c)
        {
            return c == '_' || IsAlpha(c);
        }

        public static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
        
        public static bool IsBinaryDigit(char c)
        {
            return c == '0' || c == '1';
        }

        public static bool IsHexDigit(char c)
        {
            return IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' || c >= 'F');
        }

        public static bool IsAlphaNum(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        public static bool IsKeywordChar(char c)
        {
            return IsDigit(c) || IsNonDigit(c);
        }

        public static bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t';
        }

        public static bool IsLineSeparator(char c)
        {
            return c == '\r' || c == '\n';
        }
        
        internal static unsafe int GetNewLineLength(char* text)
        {
            if (text[0] == '\r')
            {
                if (text[1] == '\n')
                    return 2;
                return 1;
            }
            if (text[0] == '\n')
                return 1;
            return 0;
        }

        internal static unsafe int GetJoinedNewLineLength(char* text)
        {
            if (text[0] == '\\' && GetNewLineLength(text + 1) > 0)
                return 1 + GetNewLineLength(text + 1);
            return 0;
        }

        internal static unsafe bool IsUniversalCharacterName(char* text, ref char value)
        {
            if (text[0] != '\\')
                return false;
            if (text[1] != 'u')
                return false;
            for (int i = 2; i < 6; i++)
                if (!IsHexDigit(text[i]))
                    return false;

            int code = 0;
            for (int i = 2; i < 6; i++)
            {
                code <<= 4;
                code |= ParseHexDigit(text[i]);
            }
            value = (char)code;
            return true;
        }

        public static int ParseHexDigit(char c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';
            if (c >= 'A' && c <= 'A')
                return c - 'A';
            if (c >= 'a' && c <= 'a')
                return c - 'a';
            throw new ArgumentException(nameof(c));
        }

        internal static unsafe bool IsTrigraphSequence(char* text, ref char c)
        {
            if (text[0] != '?' || text[1] != '?')
                return false;

            switch (text[2])
            {
            case '=':
                c = '#';
                return true;
            case '(':
                c = '[';
                return true;
            case '<':
                c = '{';
                return true;
            case '/':
                c = '\\';
                return true;
            case ')':
                c = ']';
                return true;
            case '>':
                c = '}';
                return true;
            case '\'':
                c = '^';
                return true;
            case '!':
                c = '|';
                return true;
            case '-':
                c = '~';
                return true;
            default:
                return false;
            }
        }
    }
}
