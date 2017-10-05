using System;
using Helper;

namespace MushROMs
{
    public sealed class SingleSelection2D : Selection2D
    {
        public override int Size => 1;
        public SingleSelection2D(Position index)
        {
            if (index.X < 0 || index.Y < 0)
                throw new ArgumentOutOfRangeException(nameof(index),
                    SR.ErrorLowerBoundInclusive(nameof(index), index, Position.Empty));
            StartIndex = index;
        }

        public override void IterateIndexes(TileMethod2D method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            method(StartIndex);
        }

        public override bool ContainsIndex(Position index)
        {
            return index == StartIndex;
        }

        protected override Position[] InitializeSelectedIndexes()
        {
            return new Position[] { Position.Empty };
        }

        public override ITileMapSelection2D Copy(Position startIndex)
        {
            return new SingleSelection2D(startIndex);
        }
    }
}
