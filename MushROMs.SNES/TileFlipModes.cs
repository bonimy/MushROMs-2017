using System;

namespace MushROMs.SNES
{
    [Flags]
    public enum TileFlipModes
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVeritcal = 2,
        FlipBoth = FlipHorizontal | FlipVeritcal
    }
}
