using System;
using System.Collections.Generic;

namespace MushROMs.NES.SMB1
{
    public class LevelData
    {
        public const int NumberOfWorlds = 8;
        public const int LevelsPerWorldPointer = 0x9CB4;
        public const int MapListPointer = 0x9CBC;
        public const int LevelAddressLowBytePointer = 0x9D2C;
        public const int LevelAddressHighBytePointer = 0x9D4E;
        public const int EnemyAddressLowBytePointer = 0x9CE4;
        public const int EnemyAddressHighBytePointer = 0x9D06;
        public const int AreaTypeEnemyOffsetPointer = 0x9CE0;
        public const int AreaTypeLevelOffsetPointer = 0x9D28;
        public const int NumberOfMaps = LevelAddressHighBytePointer - LevelAddressLowBytePointer;

        public int MapNumber
        {
            get;
            private set;
        }
        public AreaType AreaType
        {
            get;
            private set;
        }
        public LevelHeader LevelHeader
        {
            get;
            private set;
        }

        private LevelObject[] LevelObjects
        {
            get;
            set;
        }
        public LevelObject[] GetLevelObjects()
        {
            return LevelObjects;
        }
        private EnemyObject[] EnemyObjects
        {
            get;
            set;
        }
        public EnemyObject[] GetEnemyObjects()
        {
            return EnemyObjects;
        }

        public LevelData(byte[] src, int mapNumber)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (mapNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(mapNumber));

            var maps = AddressConverter.NesToPc(MapListPointer);
            var world = AddressConverter.NesToPc(LevelsPerWorldPointer);
            var lo = AddressConverter.NesToPc(LevelAddressLowBytePointer);
            var hi = AddressConverter.NesToPc(LevelAddressHighBytePointer);
            var areas = AddressConverter.NesToPc(AreaTypeLevelOffsetPointer);

            MapNumber = mapNumber;
            AreaType = (AreaType)((mapNumber >> 5) & 3);
            var reducedMapNumer = mapNumber & 0x1F;
            var index = reducedMapNumer + src[areas + (int)AreaType];
            var address = src[lo + index] | (src[hi + index] << 8);
            address = AddressConverter.NesToPc(address);

            LevelHeader = new LevelHeader(src[address], src[address + 1]);
            address += 2;

            var objects = new List<LevelObject>();
            for (; src[address] != 0xFD; address += 2)
            {
                objects.Add(new LevelObject(src[address], src[address + 1]));
            }
            LevelObjects = objects.ToArray();

            lo = AddressConverter.NesToPc(EnemyAddressLowBytePointer);
            hi = AddressConverter.NesToPc(EnemyAddressHighBytePointer);
            areas = AddressConverter.NesToPc(AreaTypeEnemyOffsetPointer);

            index = reducedMapNumer + src[areas + (int)AreaType];

            address = src[lo + index] | (src[hi + index] << 8);
            address = AddressConverter.NesToPc(address);

            var enemies = new List<EnemyObject>();
            for (; src[address] != 0xFF;)
            {
                var enemy = new EnemyObject(src[address], src[address + 1], src[address + 2]);
                enemies.Add(enemy);
                address += enemy.Size;
            }
            EnemyObjects = enemies.ToArray();
        }

        public static LevelData[] GetAllLevels(byte[] src)
        {
            var levels = new LevelData[NumberOfMaps];

            int areas = AddressConverter.NesToPc(AreaTypeLevelOffsetPointer);
            int ws = src[areas + 0];
            int gs = src[areas + 1];
            int us = src[areas + 2];
            int cs = src[areas + 3];
            var list = new List<AreaIndex>(new AreaIndex[] {
                new AreaIndex(ws, AreaType.Water),
                new AreaIndex(gs, AreaType.Grassland),
                new AreaIndex(us, AreaType.Underground),
                new AreaIndex(cs, AreaType.Castle),
                new AreaIndex(Int32.MaxValue, 0)});
            list.Sort((x, y) => x.Index - y.Index);

            for (int i = 0; i < NumberOfMaps; i++)
            {
                int map = i;
                for (int j = 0; j < 4; j++)
                {
                    if (i >= list[j].Index && i < list[j + 1].Index)
                    {
                        map -= list[j].Index;
                        map |= (int)list[j].AreaType << 5;
                        break;
                    }
                }

                levels[i] = new LevelData(src, map);
            }

            return levels;
        }

        public override string ToString()
        {
            return "Map" + MapNumber.ToString("X2");
        }

        private struct AreaIndex
        {
            public int Index;
            public AreaType AreaType;

            public AreaIndex(int index, AreaType areaType)
            {
                Index = index;
                AreaType = areaType;
            }
        }
    }
}
