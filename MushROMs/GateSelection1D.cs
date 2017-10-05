using System;
using System.Collections.Generic;

namespace MushROMs
{
    public delegate bool GateMethod(bool left, bool right);

    public sealed class GateSelection1D : Selection1D
    {
        public ITileMapSelection1D Left
        {
            get;
            private set;
        }
        public ITileMapSelection1D Right
        {
            get;
            private set;
        }
        public GateMethod Rule
        {
            get;
            private set;
        }

        public override int Size => GetSelectedIndexes().Length;
        public GateSelection1D(ITileMapSelection1D left, ITileMapSelection1D right, GateMethod rule)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.StartIndex == Empty.StartIndex)
                StartIndex = right.StartIndex;
            else if (right.StartIndex == Empty.StartIndex)
                StartIndex = left.StartIndex;
            else
                StartIndex = Math.Min(left.StartIndex, right.StartIndex);

            Left = left;
            Right = right;
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        public override void IterateIndexes(TileMethod1D method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            var indexes = GetSelectedIndexes();
            unsafe
            {
                fixed (int* src = indexes)
                {
                    for (int i = indexes.Length; --i >= 0;)
                        method(src[i] + StartIndex);
                }
            }
        }

        public override bool ContainsIndex(int index)
        {
            return Rule(Left.ContainsIndex(index), Right.ContainsIndex(index));
        }

        protected override int[] InitializeSelectedIndexes()
        {
            var lIndexes = Left.GetSelectedIndexes();
            var rIndexes = Right.GetSelectedIndexes();
            var indexes = new List<int>();

            int lDelta = Left.StartIndex - StartIndex;
            int rDelta = Right.StartIndex - StartIndex;

            unsafe
            {
                fixed (int* lPtr = lIndexes)
                fixed (int* rPtr = rIndexes)
                {
                    for (int i = lIndexes.Length; --i >= 0;)
                    {
                        int lIndex = lPtr[i] + lDelta;
                        if (Rule(true, Right.ContainsIndex(lIndex)))
                            indexes.Add(lIndex);
                    }

                    for (int i = rIndexes.Length; --i >= 0;)
                    {
                        int rIndex = rPtr[i] + rDelta;
                        if (Rule(Left.ContainsIndex(rIndex), true) && !indexes.Contains(rIndex - StartIndex))
                            indexes.Add(rIndex);
                    }
                }
            }

            return indexes.ToArray();
        }

        public override ITileMapSelection1D Copy(int startIndex)
        {
            int lDelta = Left.StartIndex - StartIndex;
            int rDelta = Right.StartIndex - StartIndex;
            return new GateSelection1D(Left.Copy(startIndex + lDelta), Right.Copy(startIndex + rDelta), Rule);
        }
    }
}
