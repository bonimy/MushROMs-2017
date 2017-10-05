using System;

namespace MushROMs.SNES
{
    public delegate GFXTile2 GFXTileMethod(GFXTile2 color);

    public class GFXData2
    {
        public GraphicsFormat GraphicsFormat
        {
            get;
            set;
        }

        private GFXTile2[] Tiles
        {
            get;
            set;
        }

        public int BitsPerPixel => CHRGFX.GetBitsPerPixel(GraphicsFormat);
        public int ColorsPerPixel => CHRGFX.GetColorsPerPixel(GraphicsFormat);
        public int TileDataSize => CHRGFX.GetTileDataSize(GraphicsFormat);

        public int Size => Tiles.Length;

        public GFXTile2 this[int index]
        {
            get => Tiles[index];
            set => Tiles[index] = value;
        }

        internal GFXData2(GFXTile2[] tiles)
        {
            Tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        }

        public void Clear()
        {
            Clear(GFXTile2.Empty);
        }

        public void Clear(GFXTile2 tile)
        {
            AlterTiles(x => tile);
        }

        public void SwitchColors(byte pixel1, byte pixel2)
        {
            AlterTiles(tile => tile.SwitchPixelValues(pixel1, pixel2));
        }

        public void RotateIndividualTiles90()
        {
            AlterTiles(tile => tile.Rotate90());
        }

        public void AlterTiles(GFXTileMethod method)
        {
            for (int i = Tiles.Length; --i >= 0;)
                Tiles[i] = method(Tiles[i]);
        }
    }
}
