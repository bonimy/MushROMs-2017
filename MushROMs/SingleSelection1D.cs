using System;
using Helper;

namespace MushROMs
{
    public sealed class SingleSelection1D : Selection1D
    {
        public override int Size => 1;
        public SingleSelection1D(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index),
                    SR.ErrorLowerBoundInclusive(nameof(index), index, 0));
            StartIndex = index;
        }

        public override void IterateIndexes(TileMethod1D method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            method(StartIndex);
        }

        public override bool ContainsIndex(int index)
        {
            return index == StartIndex;
        }

        protected override int[] InitializeSelectedIndexes()
        {
            return new int[] { 0 };
        }

        public override ITileMapSelection1D Copy(int startIndex)
        {
            return new SingleSelection1D(startIndex);
        }
    }
}
