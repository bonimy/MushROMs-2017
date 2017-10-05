namespace MushROMs
{
    public abstract class Selection1D : ITileMapSelection1D
    {
        public static Selection1D Empty = new EmptySelection1D();

        public int StartIndex
        {
            get;
            protected set;
        }
        public abstract int Size
        {
            get;
        }

        private int[] SelectedIndexes
        {
            get;
            set;
        }

        public int[] GetSelectedIndexes()
        {
            if (SelectedIndexes == null)
                SelectedIndexes = InitializeSelectedIndexes();
            return SelectedIndexes;
        }

        public abstract void IterateIndexes(TileMethod1D method);

        protected abstract int[] InitializeSelectedIndexes();
        public abstract bool ContainsIndex(int index);

        public abstract ITileMapSelection1D Copy(int startIndex);

        public ITileMapSelection LogicalAnd(ITileMapSelection value)
        {
            return new GateSelection1D(this, (ITileMapSelection1D)value, (left, right) => left & right);
        }
        public ITileMapSelection LogicalOr(ITileMapSelection value)
        {
            return new GateSelection1D(this, (ITileMapSelection1D)value, (left, right) => left | right);
        }
        public ITileMapSelection LogicalXor(ITileMapSelection value)
        {
            return new GateSelection1D(this, (ITileMapSelection1D)value, (left, right) => left ^ right);
        }
        public ITileMapSelection LogicalNegate(ITileMapSelection value)
        {
            return new GateSelection1D(this, (ITileMapSelection1D)value, (left, right) => left & !right);
        }
    }
}
