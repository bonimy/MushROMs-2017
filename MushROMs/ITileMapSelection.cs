namespace MushROMs
{
    public interface ITileMapSelection
    {
        ITileMapSelection LogicalAnd(ITileMapSelection value);
        ITileMapSelection LogicalOr(ITileMapSelection value);
        ITileMapSelection LogicalXor(ITileMapSelection value);
        ITileMapSelection LogicalNegate(ITileMapSelection value);
    }
}
