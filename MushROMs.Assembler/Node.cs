using System.Diagnostics;
using Helper;

namespace MushROMs.Assembler
{
    [DebuggerDisplay("Index = {StartIndex}, Length = {Length}, Type = {KeywordType}")]
    public struct Node
    {
        public static readonly Node Empty = new Node();

        public int TextLineNumber
        {
            get;
            private set;
        }
        public int ProgramLineNumber
        {
            get;
            private set;
        }
        public int BlockNumber
        {
            get;
            private set;
        }
        public int StartIndex
        {
            get;
            private set;
        }
        public int Length
        {
            get;
            private set;
        }
        public int EndIndex => StartIndex + Length; public KeywordType KeywordType
        {
            get;
            private set;
        }

        public Node(int textLineNumber, int programLineNumber, int blockNumber, int startIndex, int length, KeywordType keywordType)
        {
            TextLineNumber = textLineNumber;
            ProgramLineNumber = programLineNumber;
            BlockNumber = blockNumber;
            StartIndex = startIndex;
            Length = length;
            KeywordType = keywordType;
        }

        public static bool operator ==(Node left, Node right)
        {
            return left.TextLineNumber == right.TextLineNumber &&
                left.ProgramLineNumber == right.ProgramLineNumber &&
                left.BlockNumber == right.BlockNumber &&
                left.StartIndex == right.StartIndex &&
                left.Length == right.Length &&
                left.KeywordType == right.KeywordType;
        }
        public static bool operator !=(Node left, Node right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Node))
                return false;

            return (Node)obj == this;
        }
        public override int GetHashCode()
        {
            return Hash.Generate(TextLineNumber, BlockNumber ^ ProgramLineNumber, StartIndex, Length, (int)KeywordType);
        }
        public override string ToString()
        {
            return SR.GetString("Index = {0}, Length = {1}, Type = {2}", StartIndex, Length, KeywordType);
        }
    }
}
