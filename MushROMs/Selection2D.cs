using Helper;

namespace MushROMs
{
    public abstract class Selection2D : ITileMapSelection2D
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Selection2D Empty = new EmptySelection2D();

        public Position StartIndex
        {
            get;
            protected set;
        }
        public abstract int Size
        {
            get;
        }

        private Position[] SelectedIndexes
        {
            get;
            set;
        }

        public Position[] GetSelectedIndexes()
        {
            if (SelectedIndexes == null)
                SelectedIndexes = InitializeSelectedIndexes();
            return SelectedIndexes;
        }

        public abstract void IterateIndexes(TileMethod2D method);

        protected abstract Position[] InitializeSelectedIndexes();
        public abstract bool ContainsIndex(Position index);

        public abstract ITileMapSelection2D Copy(Position startIndex);

        public ITileMapSelection LogicalAnd(ITileMapSelection value)
        {
            return new GateSelection2D(this, (ITileMapSelection2D)value, (left, right) => left & right);
        }
        public ITileMapSelection LogicalOr(ITileMapSelection value)
        {
            return new GateSelection2D(this, (ITileMapSelection2D)value, (left, right) => left | right);
        }
        public ITileMapSelection LogicalXor(ITileMapSelection value)
        {
            return new GateSelection2D(this, (ITileMapSelection2D)value, (left, right) => left ^ right);
        }
        public ITileMapSelection LogicalNegate(ITileMapSelection value)
        {
            return new GateSelection2D(this, (ITileMapSelection2D)value, (left, right) => left & !right);
        }
    }
}
