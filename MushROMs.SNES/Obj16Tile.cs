using System;
using HelperSR = Helper.SR;

namespace MushROMs.SNES
{
    public struct Obj16Tile
    {
        public const int NumberOfTiles = 4;

        public const int Tile00Index = 0;
        public const int Tile01Index = 1;
        public const int Tile10Index = 2;
        public const int Tile11Index = 3;

        public const int SizeOf = NumberOfTiles * ObjTile.SizeOf;

        public ObjTile Tile00
        {
            get;
            set;
        }
        public ObjTile Tile01
        {
            get;
            set;
        }
        public ObjTile Tile10
        {
            get;
            set;
        }
        public ObjTile Tile11
        {
            get;
            set;
        }

        public ObjTile this[int index]
        {
            get
            {
                switch (index)
                {
                case Tile00Index:
                    return Tile00;
                case Tile01Index:
                    return Tile01;
                case Tile10Index:
                    return Tile10;
                case Tile11Index:
                    return Tile11;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
            set
            {
                switch (index)
                {
                case Tile00Index:
                    Tile00 = value;
                    return;
                case Tile01Index:
                    Tile01 = value;
                    return;
                case Tile10Index:
                    Tile10 = value;
                    return;
                case Tile11Index:
                    Tile11 = value;
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        public Obj16Tile(ObjTile tile00, ObjTile tile01, ObjTile tile10, ObjTile tile11)
        {
            Tile00 = tile00;
            Tile01 = tile01;
            Tile10 = tile10;
            Tile11 = tile11;
        }

        public Obj16Tile FlipX()
        {
            return new Obj16Tile(Tile10.FlipX(), Tile11.FlipX(),
                Tile00.FlipX(), Tile01.FlipX());
        }

        public Obj16Tile FlipY()
        {
            return new Obj16Tile(Tile01.FlipY(), Tile00.FlipY(),
                Tile11.FlipY(), Tile10.FlipY());
        }

        public int GetXCoordinate(int index)
        {
            return index / 2;
        }

        public int GetYCoordinate(int index)
        {
            return index % 2;
        }

        public static bool operator ==(Obj16Tile left, Obj16Tile right)
        {
            return
                left.Tile00 == right.Tile00 &&
                left.Tile01 == right.Tile01 &&
                left.Tile10 == right.Tile10 &&
                left.Tile11 == right.Tile11;
        }
        public static bool operator !=(Obj16Tile left, Obj16Tile right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Obj16Tile))
                return false;

            return (Obj16Tile)obj == this;
        }
        public override int GetHashCode()
        {
            return (Tile00.Value | (Tile01.Value << 0x10)) ^
                (Tile10.Value | (Tile11.Value << 0x10));
        }
        public override string ToString()
        {
            return HelperSR.GetString("{0} {1} {2} {3}",
                Tile00.Value.ToString("X4"),
                Tile01.Value.ToString("X4"),
                Tile10.Value.ToString("X4"),
                Tile11.Value.ToString("X4"));
        }
    }
}
