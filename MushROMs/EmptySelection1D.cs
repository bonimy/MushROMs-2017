using System;

namespace MushROMs
{
    public sealed class EmptySelection1D : Selection1D
    {
        public override int Size => 0;
        internal EmptySelection1D()
        {
            StartIndex = -1;
        }

        public override void IterateIndexes(TileMethod1D method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
        }

        public override bool ContainsIndex(int index)
        {
            return false;
        }

        protected override int[] InitializeSelectedIndexes()
        {
            return new int[0];
        }

        public override ITileMapSelection1D Copy(int startIndex)
        {
            return new EmptySelection1D();
        }
    }
}
