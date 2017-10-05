using System;
using System.Collections.Generic;
using Helper;

namespace MushROMs
{
    public sealed class GateSelection2D : Selection2D
    {
        public ITileMapSelection2D Left
        {
            get;
            private set;
        }
        public ITileMapSelection2D Right
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
        public GateSelection2D(ITileMapSelection2D left, ITileMapSelection2D right, GateMethod rule)
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
                StartIndex = Position.TopLeft(left.StartIndex, right.StartIndex);

            Left = left;
            Right = right;
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        public override void IterateIndexes(TileMethod2D method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            var indexes = GetSelectedIndexes();
            unsafe
            {
                fixed (Position* src = indexes)
                {
                    for (int i = indexes.Length; --i >= 0;)
                        method(src[i]);
                }
            }
        }

        public override bool ContainsIndex(Position index)
        {
            return Rule(Left.ContainsIndex(index), Right.ContainsIndex(index));
        }

        protected override Position[] InitializeSelectedIndexes()
        {
            var lIndexes = Left.GetSelectedIndexes();
            var rIndexes = Right.GetSelectedIndexes();
            var indexes = new List<Position>();

            var lDelta = Left.StartIndex - StartIndex;
            var rDelta = Right.StartIndex - StartIndex;

            unsafe
            {
                fixed (Position* lPtr = lIndexes)
                fixed (Position* rPtr = rIndexes)
                {
                    for (int i = lIndexes.Length; --i >= 0;)
                    {
                        var lIndex = lPtr[i] + lDelta;
                        if (Rule(true, Right.ContainsIndex(lIndex)))
                            indexes.Add(lIndex - StartIndex);
                    }

                    for (int i = rIndexes.Length; --i >= 0;)
                    {
                        var rIndex = rPtr[i] + rDelta;
                        if (Rule(Left.ContainsIndex(rIndex), true) && !indexes.Contains(rIndex - StartIndex))
                            indexes.Add(rIndex - StartIndex);
                    }
                }
            }

            return indexes.ToArray();
        }

        public override ITileMapSelection2D Copy(Position startIndex)
        {
            var lDelta = Left.StartIndex - StartIndex;
            var rDelta = Right.StartIndex - StartIndex;
            return new GateSelection2D(Left.Copy(startIndex + lDelta), Right.Copy(startIndex + rDelta), Rule);
        }
    }
}
