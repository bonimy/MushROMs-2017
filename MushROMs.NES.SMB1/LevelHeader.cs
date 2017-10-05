namespace MushROMs.NES.SMB1
{
    public struct LevelHeader
    {
        public byte Value1
        {
            get;
            set;
        }

        public byte Value2
        {
            get;
            set;
        }

        public StartTime StartTime
        {
            get => (StartTime)(Value1 >> 6);
            set
            {
                Value1 &= unchecked((byte)~(3 << 6));
                Value1 |= (byte)(((int)value & 3) << 6);
            }
        }

        public StartYPosition StartYPosition
        {
            get => (StartYPosition)((Value1 >> 3) & 7);
            set
            {
                Value1 &= unchecked((byte)~(7 << 3));
                Value1 |= (byte)(((int)value & 7) << 3);
            }
        }

        public BackgroundType BackgroundType
        {
            get => (BackgroundType)(Value1 & 7);
            set
            {
                Value1 &= unchecked((byte)~7);
                Value1 |= (byte)((int)value & 7);
            }
        }

        public ObjectMode ObjectMode
        {
            get => (ObjectMode)(Value2 >> 5);
            set
            {
                Value2 &= unchecked((byte)~(7 << 5));
                Value2 |= (byte)(((int)value & 7) << 5);
            }
        }

        public SceneryType SceneryType
        {
            get => (SceneryType)((Value2 >> 4) & 3);
            set
            {
                Value2 &= unchecked((byte)~(3 << 4));
                Value2 |= (byte)(((int)value & 3) << 4);
            }
        }

        public TerrainMode TerrainMode
        {
            get => (TerrainMode)(Value2 & 0x0F);
            set
            {
                Value2 &= unchecked((byte)~0x0F);
                Value2 |= (byte)((int)value & 0x0F);
            }
        }

        public LevelHeader(byte val1, byte val2)
        {
            Value1 = val1;
            Value2 = val2;
        }

        public LevelHeader(StartTime startTime, StartYPosition startYPosition, BackgroundType backgroundType, ObjectMode objectMode, SceneryType sceneryType, TerrainMode terrainMode) : this()
        {
            StartTime = startTime;
            StartYPosition = startYPosition;
            BackgroundType = backgroundType;
            ObjectMode = objectMode;
            SceneryType = sceneryType;
            TerrainMode = terrainMode;
        }

        public static bool operator ==(LevelHeader left, LevelHeader right)
        {
            return left.Value1 == right.Value1 &&
                left.Value2 == right.Value2;
        }
        public static bool operator !=(LevelHeader left, LevelHeader right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LevelHeader))
                return false;
            return (LevelHeader)obj == this;
        }

        public override int GetHashCode()
        {
            return (Value1) | (Value2 << 8);
        }

        public override string ToString()
        {
            return System.String.Format("{0} {1}", Value1.ToString("X2"), Value2.ToString("X2"));
        }
    }
}
