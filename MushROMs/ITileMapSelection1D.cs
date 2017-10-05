namespace MushROMs
{
    public delegate void TileMethod1D(int index);

    public interface ITileMapSelection1D : ITileMapSelection
    {
        int StartIndex
        {
            get;
        }
        int Size
        {
            get;
        }

        void IterateIndexes(TileMethod1D method);
        int[] GetSelectedIndexes();
        bool ContainsIndex(int index);

        ITileMapSelection1D Copy(int startIndex);
    }
}
