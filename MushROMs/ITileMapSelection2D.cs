using Helper;

namespace MushROMs
{
    public delegate void TileMethod2D(Position index);

    public interface ITileMapSelection2D : ITileMapSelection
    {
        Position StartIndex
        {
            get;
        }
        int Size
        {
            get;
        }

        void IterateIndexes(TileMethod2D method);
        Position[] GetSelectedIndexes();
        bool ContainsIndex(Position index);

        ITileMapSelection2D Copy(Position startIndex);
    }
}
