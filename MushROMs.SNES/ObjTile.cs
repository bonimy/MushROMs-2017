using HelperSR = Helper.SR;

namespace MushROMs.SNES
{
    public struct ObjTile
    {
        public const int SizeOf = sizeof(ushort);

        private const int TileIndexMask = 0x3FF;
        private const int PaletteNumberBitShift = 10;
        private const int PaletteNumberMask = 7;
        private const int PriorityBitShift = 13;
        private const int PriorityMask = 1;
        private const int FlipBitShift = 14;
        private const int FlipMask = 3;

        private ushort _value;

        public int Value
        {
            get => _value;
            set => this = new ObjTile(value);
        }

        public int TileIndex
        {
            get => Value & TileIndexMask;
            set
            {
                Value &= ~TileIndexMask;
                Value |= value & TileIndexMask;
            }
        }

        public int PaletteNumber
        {
            get => (Value >> PaletteNumberBitShift) & PaletteNumberMask;
            set
            {
                Value &= ~(PaletteNumberMask << PaletteNumberBitShift);
                Value |= (value & PaletteNumberMask) << PaletteNumberBitShift;
            }
        }

        public LayerPriority Priority
        {
            get => (LayerPriority)((Value >> PriorityBitShift) & PriorityMask);
            set
            {
                if (value != LayerPriority.Priority0)
                    Value |= (PriorityMask << PriorityBitShift);
                else
                    Value &= ~(PriorityMask << PriorityBitShift);
            }
        }

        public TileFlipModes TileFlipMode
        {
            get => (TileFlipModes)((Value >> FlipBitShift) & FlipMask);
            set
            {
                Value &= ~(FlipMask << FlipBitShift);
                Value |= ((int)value & FlipMask) << FlipBitShift;
            }
        }

        public bool XFlipped
        {
            get => (TileFlipMode & TileFlipModes.FlipHorizontal) != 0;
            set
            {
                if (value)
                    TileFlipMode &= ~TileFlipModes.FlipHorizontal;
                else
                    TileFlipMode |= TileFlipModes.FlipHorizontal;
            }
        }

        public bool YFlipped
        {
            get => (TileFlipMode & TileFlipModes.FlipVeritcal) != 0;
            set
            {
                if (value)
                    TileFlipMode &= ~TileFlipModes.FlipVeritcal;
                else
                    TileFlipMode |= TileFlipModes.FlipVeritcal;
            }
        }

        private ObjTile(int value)
        {
            _value = (ushort)value;
        }

        public ObjTile FlipX()
        {
            var tile = this;
            tile.TileFlipMode ^= TileFlipModes.FlipHorizontal;
            return tile;
        }

        public ObjTile FlipY()
        {
            var tile = this;
            tile.TileFlipMode ^= TileFlipModes.FlipVeritcal;
            return tile;
        }

        public static implicit operator int(ObjTile tile)
        {
            return tile.Value;
        }
        public static implicit operator ObjTile(int value)
        {
            return new ObjTile(value);
        }

        public static bool operator ==(ObjTile left, ObjTile right)
        {
            return left.Value == right.Value;
        }
        public static bool operator !=(ObjTile left, ObjTile right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ObjTile))
                return false;

            return (ObjTile)obj == this;
        }
        public override int GetHashCode()
        {
            return Value;
        }
        public override string ToString()
        {
            return HelperSR.GetString(Value, "X4");
        }
    }
}
