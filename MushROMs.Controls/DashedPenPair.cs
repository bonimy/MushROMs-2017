using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using Helper;

namespace MushROMs.Controls
{
    public struct DashedPenPair
    {
        public static readonly DashedPenPair Null = new DashedPenPair();

        public Color Color1
        {
            get;
            set;
        }
        public Color Color2
        {
            get;
            set;
        }

        public int Length1
        {
            get;
            set;
        }
        public int Length2
        {
            get;
            set;
        }

        public DashedPenPair(Color color1, Color color2, int length1, int length2)
        {
            if (length1 <= 0)
                throw new ArgumentOutOfRangeException(nameof(length1),
                    SR.ErrorUpperBoundExclusive(nameof(length1), length1, 0));
            if (length2 <= 0)
                throw new ArgumentOutOfRangeException(nameof(length2),
                    SR.ErrorUpperBoundExclusive(nameof(length2), length2, 0));

            Color1 = color1;
            Color2 = color2;

            Length1 = length1;
            Length2 = length2;
        }

        public void SetPenProperties(Pen pen1, Pen pen2)
        {
            if (pen1 != null)
            {
                pen1.Color = Color1;
                pen1.DashStyle = DashStyle.Custom;
                pen1.DashPattern = new float[] { Length1, Length2 };
                pen1.DashOffset = 0;
            }

            if (pen2 != null)
            {
                pen2.Color = Color2;
                pen2.DashStyle = DashStyle.Custom;
                pen2.DashPattern = new float[] { Length2, Length1 };
                pen2.DashOffset = Length1;
            }
        }

        public static bool operator ==(DashedPenPair left, DashedPenPair right)
        {
            return left.Color1 == right.Color1 &&
                left.Color2 == right.Color2 &&
                left.Length1 == right.Length1 &&
                left.Length2 == right.Length2;
        }
        public static bool operator !=(DashedPenPair left, DashedPenPair right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DashedPenPair))
                return false;

            return (DashedPenPair)obj == this;
        }
        public override int GetHashCode()
        {
            return Hash.Generate(Color1.GetHashCode(), Color2.GetHashCode(), Length1, Length2);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append(nameof(Color1));
            sb.Append(": ");
            sb.Append(Color1);
            sb.Append(", ");
            sb.Append(nameof(Length1));
            sb.Append(": ");
            sb.Append(SR.GetString(Length1));
            sb.Append(", ");
            sb.Append(nameof(Color2));
            sb.Append(": ");
            sb.Append(Color2);
            sb.Append(", ");
            sb.Append(nameof(Length2));
            sb.Append(": ");
            sb.Append(SR.GetString(Length2));
            sb.Append('}');
            return sb.ToString();
        }
    }
}