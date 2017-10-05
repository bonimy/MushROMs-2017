namespace MushROMs.SNES.SMAS.SMB1
{
    public static class GameInfo
    {
        public const int PaletteIndexAddress = 0x0497CD;

        public static void LoadPalette(byte[] data, int map)
        {
            int map2 = map & 0x1F;
            int bgType = (map & (0x7F & ~0x1F)) >> 5;
        }
    }
}
