using System;
using Helper;

namespace MushROMs
{
    public sealed class EmptySelection2D : Selection2D
    {
        public static readonly Range NullIndex = new Range(-1, -1);

        public override int Size => 0;
        internal EmptySelection2D()
        {
            StartIndex = NullIndex;
        }

        public override void IterateIndexes(TileMethod2D method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
        }

        public override bool ContainsIndex(Position index)
        {
            return false;
        }

        protected override Position[] InitializeSelectedIndexes()
        {
            return new Position[0];
        }

        public override ITileMapSelection2D Copy(Position startIndex)
        {
            return new EmptySelection2D();
        }
    }
}
