using System;
using Helper;

namespace MushROMs
{
    public sealed class BoxSelection2D : Selection2D
    {
        public override int Size => Range.Area;
        public Range Range
        {
            get;
            private set;
        }

        private BoxSelection2D(Position startIndex, Range range)
        {
            if (range.Horizontal <= 0 || range.Vertical <= 0)
                throw new ArgumentOutOfRangeException(nameof(range),
                    SR.ErrorUpperBoundExclusive(nameof(range), range, 0));

            StartIndex = startIndex;
            Range = range;
        }

        public BoxSelection2D(Position index1, Position index2)
        {
            if (index1.X < 0 || index1.Y < 0)
                throw new ArgumentOutOfRangeException(nameof(index1),
                    SR.ErrorLowerBoundInclusive(nameof(index1), index1, Position.Empty));
            if (index2.X < 0 || index2.Y < 0)
                throw new ArgumentOutOfRangeException(nameof(index2),
                    SR.ErrorLowerBoundInclusive(nameof(index2), index2, Position.Empty));

            var min = Position.TopLeft(index1, index2);
            var max = Position.BottomRight(index1, index2);

            StartIndex = min;
            Range = max - min + new Position(1, 1);
        }

        public override void IterateIndexes(TileMethod2D method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            for (int y = Range.Vertical; --y >= 0;)
                for (int x = Range.Horizontal; --x >= 0;)
                    method(new Position(x, y));
        }

        public override bool ContainsIndex(Position index)
        {
            return Range.Contains(index);
        }

        protected override Position[] InitializeSelectedIndexes()
        {
            var indexes = new Position[Range.Horizontal * Range.Vertical];
            for (int y = Range.Vertical; --y >= 0;)
                for (int x = Range.Horizontal; --x >= 0;)
                    indexes[(y * Range.Horizontal) + x] =
                        new Position(x, y);
            return indexes;
        }

        public override ITileMapSelection2D Copy(Position startIndex)
        {
            return new BoxSelection2D(startIndex, Range);
        }
    }
}
