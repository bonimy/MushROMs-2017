using System;
using System.Diagnostics;
using System.Drawing;

namespace Helper
{
    [DebuggerDisplay("H = {Horizontal}, V = {Vertical}")]
    public struct Range
    {
        public static readonly Range Empty = new Range();

        public int Horizontal
        {
            get;
            private set;
        }
        public int Vertical
        {
            get;
            private set;
        }

        public int Area => Horizontal * Vertical;
        public Range(int value) : this(value, value)
        {
        }
        public Range(int horizontal, int vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public bool Contains(Position position)
        {
            return position.X >= 0 && position.X < Horizontal &&
                position.Y >= 0 && position.Y < Vertical;
        }

        public Range Add(Range value)
        {
            return this + value;
        }
        public Range Subtract(Range value)
        {
            return this - value;
        }
        public Range Negate()
        {
            return -this;
        }
        public Range Multiply(Range value)
        {
            return this * value;
        }
        public Range Divide(Range value)
        {
            return this / value;
        }

        public static Range TopLeft(Range value1, Range value2)
        {
            return new Range(Math.Min(value1.Horizontal, value2.Horizontal),
                Math.Min(value1.Vertical, value2.Vertical));
        }
        public static Range TopRight(Range value1, Range value2)
        {
            return new Range(Math.Max(value1.Horizontal, value2.Horizontal),
                Math.Min(value1.Vertical, value2.Vertical));
        }
        public static Range BottomLeft(Range value1, Range value2)
        {
            return new Range(Math.Min(value1.Horizontal, value2.Horizontal),
                Math.Max(value1.Vertical, value2.Vertical));
        }
        public static Range BottomRight(Range value1, Range value2)
        {
            return new Range(Math.Max(value1.Horizontal, value2.Horizontal),
                Math.Max(value1.Vertical, value2.Vertical));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Range))
                return false;

            return (Range)obj == this;
        }
        public override int GetHashCode()
        {
            return ((Size)this).GetHashCode();
        }
        public override string ToString()
        {
            return ((Size)this).ToString();
        }

        public static bool operator ==(Range left, Range right)
        {
            return left.Horizontal == right.Horizontal &&
                left.Vertical == right.Vertical;
        }
        public static bool operator !=(Range left, Range right)
        {
            return !(left == right);
        }

        public static Range operator +(Range left, Range right)
        {
            return new Range(left.Horizontal + right.Horizontal, left.Vertical + right.Vertical);
        }
        public static Range operator -(Range left, Range right)
        {
            return new Range(left.Horizontal - right.Horizontal, left.Vertical - right.Vertical);
        }
        public static Range operator -(Range right)
        {
            return new Range(-right.Horizontal, -right.Vertical);
        }
        public static Range operator *(Range left, Range right)
        {
            return new Range(left.Horizontal * right.Horizontal, left.Vertical * right.Vertical);
        }
        public static Range operator /(Range left, Range right)
        {
            return new Range(left.Horizontal / right.Horizontal, left.Vertical / right.Vertical);
        }

        public static implicit operator Range(int range)
        {
            return new Range(range, range);
        }

        public static implicit operator Size(Range range)
        {
            return new Size(range.Horizontal, range.Vertical);
        }
        public static implicit operator Range(Size size)
        {
            return new Range(size.Width, size.Height);
        }
    }
}
