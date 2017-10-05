using System;
using System.Diagnostics;
using System.Drawing;

namespace Helper
{
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    public struct Position
    {
        public static readonly Position Empty = new Position();

        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position Add(Position value)
        {
            return this + value;
        }
        public Position Subtract(Position value)
        {
            return this - value;
        }
        public Position Negate()
        {
            return -this;
        }
        public Position Multiply(Range value)
        {
            return this * value;
        }
        public Position Divide(Range value)
        {
            return this / value;
        }

        public static Position TopLeft(Position left, Position right)
        {
            return new Position(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y));
        }
        public static Position TopRight(Position left, Position right)
        {
            return new Position(Math.Max(left.X, right.X), Math.Min(left.Y, right.Y));
        }
        public static Position BottomLeft(Position left, Position right)
        {
            return new Position(Math.Min(left.X, right.X), Math.Max(left.Y, right.Y));
        }
        public static Position BottomRight(Position left, Position right)
        {
            return new Position(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Position))
                return false;

            return (Position)obj == this;
        }
        public override int GetHashCode()
        {
            return ((Point)this).GetHashCode();
        }
        public override string ToString()
        {
            return ((Point)this).ToString();
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public static implicit operator Point(Position position)
        {
            return new Point(position.X, position.Y);
        }
        public static implicit operator Position(Point point)
        {
            return new Position(point.X, point.Y);
        }
        public static implicit operator Range(Position position)
        {
            return new Range(position.X, position.Y);
        }
        public static implicit operator Position(Range range)
        {
            return new Position(range.Horizontal, range.Vertical);
        }

        public static Position operator +(Position left, Position right)
        {
            return new Position(left.X + right.X, left.Y + right.Y);
        }
        public static Position operator -(Position left, Position right)
        {
            return new Position(left.X - right.X, left.Y - right.Y);
        }
        public static Position operator -(Position right)
        {
            return Empty - right;
        }
        public static Position operator *(Position left, Range right)
        {
            return new Position(left.X * right.Horizontal, left.Y * right.Vertical);
        }
        public static Position operator *(Range left, Position right)
        {
            return new Position(left.Horizontal * right.X, left.Vertical * right.Y);
        }

        public static Position operator /(Position left, Range right)
        {
            var position = new Position(left.X / right.Horizontal, left.Y / right.Vertical);

            if (left.X < 0)
                position.X--;
            if (left.Y < 0)
                position.Y--;

            return position;
        }
    }
}
