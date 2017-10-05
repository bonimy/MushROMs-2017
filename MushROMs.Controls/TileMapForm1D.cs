namespace MushROMs.Controls
{
    public class TileMapForm1D : TileMapForm
    {
        public new TileMapControl1D TileMapControl
        {
            get => (TileMapControl1D)base.TileMapControl;
            set => base.TileMapControl = value;
        }

        public new TileMap1D TileMap => (TileMap1D)base.TileMap;
    }
}
