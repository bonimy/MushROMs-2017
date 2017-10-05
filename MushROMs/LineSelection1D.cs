using System;
using Helper;

namespace MushROMs
{
    public class LineSelection1D : Selection1D
    {
        public override int Size => Length;
        private int Length
        {
            get;
            set;
        }

        public LineSelection1D(int index1, int index2)
        {
            if (index1 < 0)
                throw new ArgumentOutOfRangeException(nameof(index1),
                    SR.ErrorLowerBoundInclusive(nameof(index1), index1, 0));
            if (index2 < 0)
                throw new ArgumentOutOfRangeException(nameof(index2),
                    SR.ErrorLowerBoundInclusive(nameof(index2), index2, 0));

            var min = Math.Min(index1, index2);
            var max = Math.Max(index1, index2);

            StartIndex = min;
            Length = max - min + 1;
        }

        public override void IterateIndexes(TileMethod1D method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            for (int i = Length; --i >= 0;)
                method(StartIndex + i);
        }

        public override bool ContainsIndex(int index)
        {
            index -= StartIndex;
            return index >= 0 && index < Length;
        }

        protected override int[] InitializeSelectedIndexes()
        {
            var indexes = new int[Length];
            for (int i = indexes.Length; --i >= 0;)
                indexes[i] = i;
            return indexes;
        }

        public override ITileMapSelection1D Copy(int startIndex)
        {
            return new LineSelection1D(startIndex, startIndex + Length - 1);
        }
    }
}
