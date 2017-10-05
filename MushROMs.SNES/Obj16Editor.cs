using System;
using Helper;
using HelperSR = Helper.SR;
using Helper.PixelFormats;
using MushROMs.SNES.Properties;

namespace MushROMs.SNES
{
    public class Obj16Editor : Editor, ITileMapEditor1D
    {
        private readonly TileMap1D _tileMap = new TileMap1D();

        public static readonly EditorInfo EditorInfo =
            new EditorInfo(typeof(Obj16Editor), Resources.Obj16EditorName);

        private byte[] _data;
        private int _startAddress;

        public event EventHandler StartAddressChanged;

        private byte[] Data
        {
            get => _data;
            set
            {
                _data = value;
                OnDataInitialized(EventArgs.Empty);
            }
        }

        public int StartAddress
        {
            get => _startAddress;
            private set
            {
                if (StartAddress == value)
                    return;

                _startAddress = value;
                OnStartAddressChanged(EventArgs.Empty);
            }
        }

        public TileMap1D TileMap
        {
            get;
            private set;
        }

        public new Obj16Selection Selection
        {
            get => (Obj16Selection)base.Selection;
            set => base.Selection = value;
        }

        public Obj16Editor()
        {
            TileMap = new TileMap1D()
            {
                TileSize = 0x10,
                ViewSize = 0x10,
                ZoomSize = 1
            };
            TileMap.SelectionChanged += TileMap_SelectionChanged;
            TileMap.Selection = new SingleSelection1D(TileMap.ZeroTile);
        }

        public void InitializeData(int numTiles)
        {
            Data = new byte[numTiles * Obj16Tile.SizeOf];
        }

        public void InitializeData(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (!MAP16File.IsValidData(data))
                throw new ArgumentException(nameof(data));

            Data = new byte[data.Length];
            Array.Copy(data, Data, Data.Length);
        }

        public void InitializeData(Obj16Tile[] tiles)
        {
            if (tiles == null)
                throw new ArgumentNullException(nameof(tiles));

            var data = new byte[tiles.Length * Obj16Tile.SizeOf];
            unsafe
            {
                fixed (byte* ptr = data)
                fixed (Obj16Tile* src = tiles)
                {
                    var dest = (Obj16Tile*)ptr;
                    for (int i = tiles.Length; --i >= 0;)
                        dest[i] = src[i];
                }
            }

            Data = data;
        }

        public byte[] GetData()
        {
            return Data;
        }

        public Obj16Tile[] GetTiles()
        {
            return null;
        }

        public new Obj16Data GetSelectionData()
        {
            return (Obj16Data)base.GetSelectionData();
        }

        public override bool IsValidSelectionData(ISelectionData data)
        {
            return data is Obj16Data;
        }

        public Obj16Tile GetColorAtAddress(int address)
        {
            if (address < 0)
                throw new ArgumentOutOfRangeException(nameof(address),
                    HelperSR.ErrorLowerBoundInclusive(nameof(address), address, 0));

            var max = Data.Length - Obj16Tile.SizeOf;
            if (address > max)
                throw new ArgumentOutOfRangeException(nameof(address),
                    HelperSR.ErrorUpperBoundInclusive(nameof(address), address, max));

            unsafe
            {
                fixed (byte* ptr = &Data[address])
                    return *(Obj16Tile*)ptr;
            }
        }

        public override void Save(string path)
        {
            var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
            var fileAssociations = MasterEditor.GetFileAssociations();
            if (!fileAssociations.ContainsKey(ext))
                throw new FileFormatException(path);

            var fileAssociation = fileAssociations[ext];

            SetFileData(fileAssociation.SaveFileDataMethod(this));
            base.Save(path);
        }

        public void EditColor(Obj16Tile tile)
        {
            EditColor(Selection, tile);
        }

        public void EditColor(Obj16Selection selection, Obj16Tile color)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));
            if (!(selection.TileMapSelection is SingleSelection1D))
                throw new ArgumentException(nameof(selection), Resources.ErrorNotSingleSelection);

            var data = GetEditorData(selection);
            data.GetData()[0] = color;
            WriteData(data);
        }

        public override void Delete()
        {
            Delete(Selection);
        }

        public void Delete(Obj16Selection selection)
        {
            var data = selection.GetEditorData(this);
            var src = data.GetData();
            for (int i = src.Length; --i >= 0;)
                src[i] = new Obj16Tile();
            WriteData(data);
        }

        public override void SelectAll()
        {
            TileMap.Selection = new LineSelection1D(0, TileMap.GridSize - 1);
        }

        private Obj16Data GetEditorData(Obj16Selection selection)
        {
            return PreviewMode ?
                (Obj16Data)PreviewData.Copy(selection) : selection.GetEditorData(this);
        }

        protected virtual void OnStartAddressChanged(EventArgs e)
        {
            StartAddressChanged?.Invoke(this, e);
        }

        protected override void OnDataInitialized(EventArgs e)
        {
            TileMap.GridSize = Data.Length / Obj16Tile.SizeOf;
            base.OnDataInitialized(e);
        }

        public int GetAddressFromIndex(int index)
        {
            return GetAddressFromIndex(index, StartAddress);
        }
        public static int GetAddressFromIndex(int index, int startAddress)
        {
            return (index * Obj16Tile.SizeOf) + startAddress;
        }
        public static int GetIndexFromAddress(int address)
        {
            return address / Obj16Tile.SizeOf;
        }

        private void TileMap_SelectionChanged(object sender, EventArgs e)
        {
            Selection = new Obj16Selection(StartAddress, TileMap.Selection);
        }

        public virtual void DrawDataAsTileMap(IntPtr scan0, int length, Palette2 palette, GFXData2 gfx)
        {
            if (scan0 == IntPtr.Zero)
                throw new ArgumentNullException(nameof(scan0));
            if (palette == null)
                throw new ArgumentNullException(nameof(palette));
            if (gfx == null)
                throw new ArgumentNullException(nameof(gfx));

            if (palette.Size < gfx.ColorsPerPixel * 8)
                throw new ArgumentOutOfRangeException(nameof(palette));

            // The total image size
            var width = TileMap.CellWidth * TileMap.ViewWidth;
            var height = TileMap.CellHeight * TileMap.ViewHeight;

            // The size, in bytes, of the pixels to be drawn
            var alloc = width * height * Color32BppArgb.SizeOf;

            // Ensure the drawing size does not exceed the IntPtr size.
            if (alloc > length)
                throw new ArgumentOutOfRangeException(nameof(length),
                    HelperSR.ErrorArrayBounds(nameof(length), length, alloc));

            // Get the number of tiles being drawn (limited to the data size).
            var tiles = Math.Min(TileMap.GridSize - TileMap.ZeroTile, TileMap.ViewSize.Area);

            // The address to begin reading data at.
            var address = GetAddressFromIndex(TileMap.ZeroTile);

            // Make sure the passed selection is viewable.
            var validSelection = Selection.StartAddress == StartAddress;

            // Determine if it is a gated selection (to darken the excluded regions).
            var gateSelection = validSelection ? Selection.TileMapSelection : null;
            //if (!(gateSelection is TileMapGateSelection1D))
            //    gateSelection = null;

            var plane = (width * TileMap.CellHeight - TileMap.CellWidth) / 2;
            var pixel = (width * TileMap.CellHeight - TileMap.ZoomWidth) / 2;

            var normal = new Color32BppArgb[palette.Size];
            for (int i = normal.Length; --i >= 0;)
            {
                normal[i] = palette[i];
                normal[i].Alpha = Byte.MaxValue;
            }

            var dark = new Color32BppArgb[normal.Length];
            for (int i = dark.Length; --i >= 0;)
            {
                dark[i].Alpha = normal[i].Alpha;
                for (int j = 3; --j >= 0;)
                    dark[i][j] = (byte)(normal[i][j] / 2);
            }

            var format = gfx.GraphicsFormat;

            unsafe
            {
                fixed (byte* ptr = &Data[address])
                {
                    var obj16Tiles = (Obj16Tile*)ptr;

                    // Loop is already capped at data size, so no worry of exceeding array bounds.
                    for (int i = tiles; --i >= 0;)
                    {
                        var obj16 = obj16Tiles[i];

                        // Darken regions that are not in gated selections. 
                        var colors32 = gateSelection != null && !gateSelection.ContainsIndex(i + TileMap.ZeroTile) ?
                            dark : normal;

                        // Get destination pointer address.
                        var start = (Color32BppArgb*)scan0 +
                            ((i % TileMap.ViewWidth) * TileMap.CellWidth) +
                            ((i / TileMap.ViewWidth) * TileMap.CellHeight * width);

                        for (int j = Obj16Tile.NumberOfTiles; --j >= 0;)
                        {
                            var obj8 = obj16[j];

                            if (obj8.TileIndex >= gfx.Size)
                                obj8 = new ObjTile();

                            var tile = gfx[obj8.TileIndex];

                            var tx = obj16.GetXCoordinate(j);
                            var ty = obj16.GetYCoordinate(j);

                            var dest = start + (TileMap.CellWidth / 2) * tx;
                            dest += (TileMap.CellHeight / 2) * ty * width;

                            var cstart = obj8.PaletteNumber * 0x10;

                            var xscale = obj8.XFlipped ? -1 : 1;
                            var yscale = obj8.YFlipped ? -1 : 1;

                            for (int y = 0; y < GFXTile2.PlanesPerTile; y++, dest += width * TileMap.ZoomHeight)
                            {
                                var pixels = obj8.YFlipped ?
                                    (tile.X + (GFXTile2.PlanesPerTile - 1 - y) * GFXTile2.DotsPerPlane) :
                                    (tile.X + (y * GFXTile2.DotsPerPlane));

                                if (obj8.XFlipped)
                                    pixels += (GFXTile2.DotsPerPlane - 1);

                                var src = dest;
                                for (int x = 0; x < GFXTile2.DotsPerPlane; x++, src += TileMap.ZoomWidth, pixels += xscale)
                                {
                                    var line = src;
                                    var color = *pixels == 0 ? new Color32BppArgb() : colors32[cstart + *pixels];
                                    for (int n = TileMap.ZoomHeight; --n >= 0; line += width)
                                        for (int m = TileMap.ZoomWidth; --m >= 0;)
                                            line[m] = color;
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static unsafe void DrawTile(Color32BppArgb scan0, int length, Color15BppBgr* colors, byte* pixels, TileFlipModes flip, Position pos, Range zoom, int width, int height)
        {
            var dest = scan0 + pos.Y * width + pos.X;

            switch (flip)
            {
            case TileFlipModes.None:
                for (int y = 0; y < GFXTile2.DotsPerPlane; y++)
                {
                    for (int x = 0; x < GFXTile2.DotsPerPlane; x++)
                    {
                        var color = colors[pixels[x]];
                        for (int n = zoom.Vertical; --n >= 0;)
                        {
                            for (int m = zoom.Horizontal; --m >= 0;)
                            {

                            }
                        }
                    }
                }
                break;
            }
        }
    }
}
