using System.Runtime.InteropServices;
using HelperSR = Helper.SR;

namespace MushROMs.SNES
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ChrTile
    {
        public const int Size = sizeof(ushort);

        private const int TileIndexMask = 0x1FF;
        private const int PaletteNumberBitShift = 9;
        private const int PaletteNumberMask = 7;
        private const int PriorityBitShift = 12;
        private const int PriorityMask = 3;
        private const int FlipBitShift = 14;
        private const int FlipMask = 3;

        [FieldOffset(0)]
        private ushort _value;

        public int Value
        {
            get => _value;
            set => this = new ChrTile(value);
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
                Value &= ~(PriorityMask << PriorityBitShift);
                Value |= ((int)value & PriorityMask) << PriorityBitShift;
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

        private ChrTile(int value)
        {
            _value = (ushort)value;
        }

        public static implicit operator int(ChrTile tile)
        {
            return tile.Value;
        }
        public static implicit operator ChrTile(int value)
        {
            return new ChrTile(value);
        }

        public static bool operator ==(ChrTile left, ChrTile right)
        {
            return left.Value == right.Value;
        }
        public static bool operator !=(ChrTile left, ChrTile right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is ChrTile))
                return false;

            return (ChrTile)obj == this;
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
