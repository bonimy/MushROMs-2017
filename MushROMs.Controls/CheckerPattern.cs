using System;
using System.Drawing;
using System.Text;
using Helper;

namespace MushROMs.Controls
{
    public struct CheckerPattern
    {
        public static readonly CheckerPattern Null = new CheckerPattern();

        public Color Color1
        {
            get;
            private set;
        }
        public Color Color2
        {
            get;
            private set;
        }
        public Size Size
        {
            get;
            private set;
        }

        public CheckerPattern(Color color1, Color color2, Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
                throw new ArgumentOutOfRangeException(nameof(size),
                    SR.ErrorUpperBoundExclusive(nameof(size), size, Size.Empty));

            Color1 = color1;
            Color2 = color2;
            Size = size;
        }

        public static bool operator ==(CheckerPattern left, CheckerPattern right)
        {
            return left.Color1 == right.Color1 && left.Color2 == right.Color2 &&
                left.Size == right.Size;

        }
        public static bool operator !=(CheckerPattern left, CheckerPattern right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CheckerPattern))
                return false;

            return (CheckerPattern)obj == this;
        }
        public override int GetHashCode()
        {
            return Hash.Generate(Color1.GetHashCode(), Color2.GetHashCode(), Size.GetHashCode());
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append(nameof(Color1));
            sb.Append(": ");
            sb.Append(Color1);
            sb.Append(", ");
            sb.Append(nameof(Color2));
            sb.Append(": ");
            sb.Append(Color2);
            sb.Append(", ");
            sb.Append(nameof(Size));
            sb.Append(": ");
            sb.Append(Size);
            sb.Append('}');
            return sb.ToString();
        }
    }
}