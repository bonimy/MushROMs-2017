using System;
using System.Collections.Generic;
using MushROMs.NES;
using MushROMs.NES.SMB1;

namespace MushROMs.SNES.SMAS.SMB1
{
    public class LevelData
    {
        public const int NumberOfWorlds = 8;
        public const int LevelsPerWorldPointer = 0x04C11C;
        public const int MapListPointer = 0x04C124;
        public const int LevelAddressLowBytePointer = 0x04C194;
        public const int LevelAddressHighBytePointer = 0x04C1B6;
        public const int EnemyAddressLowBytePointer = 0x04C14C;
        public const int EnemyAddressHighBytePointer = 0x04C16E;
        public const int AreaTypeEnemyOffsetPointer = 0x04C148;
        public const int AreaTypeLevelOffsetPointer = 0x04C190;
        public const int NumberOfMaps = LevelAddressHighBytePointer - LevelAddressLowBytePointer;

        public const int PaletteRowPointers = 0x0497CD;
        public const int PaletteRowIndex = 0x04AE3F;
        public const int PaletteColors = 0x04AEC3;

        public const int BG1BG2GFXAddress = 0x068000;
        public const int BG3GFXAddress = 0x078000;
        public const int CHRGFXAddress = 0x0CF800;

        public const int Map16ChunkAddressLow = 0x039438;
        public const int Map16ChunkAddressHigh = 0x03943C;

        public ROM ROM
        {
            get;
            private set;
        }
        public int MapNumber
        {
            get;
            private set;
        }
        public int MapIndex
        {
            get => MapNumber & 0x1F;
        }
        public AreaType AreaType
        {
            get => (AreaType)((MapNumber >> 5) & 3);
        }
        public int LevelObjectIndex
        {
            get;
            private set;
        }
        public int EnemyObjectIndex
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

        public int LevelSize
        {
            get
            {
                var size = 3; // two byte header and one byte termination value.
                foreach (var obj in LevelObjects)
                    size += obj.Size;
                return size;
            }
        }
        public int EnemiesSize
        {
            get
            {
                var size = 1; // one byte termination value.
                foreach (var spr in EnemyObjects)
                    size += spr.Size;
                return size;
            }
        }
        public int TotalSize
        {
            get => LevelSize + EnemiesSize;
        }

        public LevelData(ROM src, int mapNumber)
        {
            ROM = src ?? throw new ArgumentNullException(nameof(ROM));

            var maps = ROM.SNESToPC(MapListPointer);
            var world = ROM.SNESToPC(LevelsPerWorldPointer);
            var lo = ROM.SNESToPC(LevelAddressLowBytePointer);
            var hi = ROM.SNESToPC(LevelAddressHighBytePointer);
            var areas = ROM.SNESToPC(AreaTypeLevelOffsetPointer);

            MapNumber = mapNumber;
            LevelObjectIndex = MapIndex + ROM[areas + (int)AreaType];

            var address = 0x040000 | ROM[lo + LevelObjectIndex] | (ROM[hi + LevelObjectIndex] << 8);
            address = ROM.SNESToPC(address);

            LevelHeader = new LevelHeader(ROM[address], ROM[address + 1]);
            address += 2;

            var objects = new List<LevelObject>();
            for (; ROM[address] != 0xFD;)
            {
                var mobj = new LevelObject(ROM[address], ROM[address + 1], ROM[address + 2]);
                objects.Add(mobj);
                address += mobj.Size;
            }
            LevelObjects = objects.ToArray();

            lo = ROM.SNESToPC(EnemyAddressLowBytePointer);
            hi = ROM.SNESToPC(EnemyAddressHighBytePointer);
            areas = ROM.SNESToPC(AreaTypeEnemyOffsetPointer);

            EnemyObjectIndex = MapIndex + ROM[areas + (int)AreaType];

            address = ROM[lo + EnemyObjectIndex] | (ROM[hi + EnemyObjectIndex] << 8);
            address = ROM.SNESToPC(address);

            var enemies = new List<EnemyObject>();
            for (; ROM[address] != 0xFF;)
            {
                var enemy = new EnemyObject(ROM[address], ROM[address + 1], ROM[address + 2]);
                enemies.Add(enemy);
                address += enemy.Size;
            }
            EnemyObjects = enemies.ToArray();
        }

        public LevelData(NES.SMB1.LevelData nes)
        {
            if (nes == null)
                throw new ArgumentNullException(nameof(nes));

            MapNumber = nes.MapNumber;
            LevelHeader = nes.LevelHeader;
            var lastTerrain = LevelHeader.TerrainMode;

            var objects = new List<LevelObject>();

            var flagPole = false;
            var page = 0;
            var x = 0;
            var xPage = 0;
            var CastleLedge = AreaType == AreaType.Castle;
            var level = nes.GetLevelObjects();

            for (int i = 0; i < level.Length; i++)
            {
                var obj = (LevelObject)level[i];
                var type = (ObjectType)obj.ObjectType;

                if (CastleLedge)
                {
                    if (type == ObjectType.HorizontalBlocks)
                    {
                        // Turn horizontal bricks into castle specific stairs
                        obj = new LevelObject(obj.X, obj.Y, obj.PageFlag, 3, obj.Parameter, 2);
                    }
                    else
                        CastleLedge = false;
                }

                // Empty tiles are worthless and crash SMAS too.
                if (obj.IsEmpty)
                    continue;

                if (type == ObjectType.BackgroundChange)
                    continue;

                if (obj.PageFlag)
                    page++;
                if (!flagPole && (type == ObjectType.FlagPole || type == ObjectType.AltFlagPole))
                {
                    flagPole = true;
                    x = obj.X;
                    xPage = (page << 4) | x;
                }

                if (flagPole && type == ObjectType.Castle)
                {
                    var x2 = obj.X;
                    var x2Page = (page << 4) | x2;
                    var dif = x2Page - xPage;
                    if (dif < 0x10)
                    {
                        obj.X = x + 4;
                        obj.PageFlag = x + 4 >= 0x10;
                    }

                    flagPole = false;
                }

                if (type == ObjectType.Castle)
                {
                    if (obj.Parameter != 0) // Big castle
                    {
                        obj.Parameter = 6; // Small castle
                    }
                    else if (page != 0)
                    {
                        obj.X -= 2;
                        obj.PageFlag = x + 2 >= 0x10;
                    }
                }

                objects.Add(obj);

                if (type == ObjectType.BrickAndSceneryChange && AreaType == AreaType.Castle)
                {
                    var current = (TerrainMode)(obj.Parameter & 0x0F);
                    InsertCastleTiles(objects, current, lastTerrain);
                    lastTerrain = current;
                }
            }
            objects.Add(new LevelObject(0x7D, 0xC7, 0x00));

            LevelObjects = objects.ToArray();

            var enemies = nes.GetEnemyObjects();
            EnemyObjects = new EnemyObject[enemies.Length];
            Array.Copy(enemies, EnemyObjects, EnemyObjects.Length);
        }

        public Palette2 LoadPalette()
        {
            var data = new byte[0x200];

            int xIndex = ROM.SNESToPC(PaletteRowPointers);
            int yIndex = ROM.SNESToPC(PaletteRowIndex);

            var index = LevelObjectIndex << 4;
            for (int j = 0; j < 0x200; j += 0x20, index++)
            {
                var x = ROM[xIndex + index] << 1;
                var y = ROM[yIndex + x] | (ROM[yIndex + x + 1] << 8);
                int address = ROM.SNESToPC(PaletteColors) + y;
                for (int i = 0; i < 0x20; i++)
                {
                    data[j + i] = ROM[address + i];
                }
            }

            return new RPFPalette(data);
        }

        public GFXEditor GetBG1BG2GFX()
        {
            var data = new byte[0x4000];

            int address = ROM.SNESToPC(0x068000);
            for (int i = data.Length; --i >= 0;)
                data[i] = ROM[address + i];

            return CHRFile.InitializeEditor(data);
        }

        public GFXEditor GetBG3GFX()
        {
            var data = new byte[0x800];

            int address = ROM.SNESToPC(0x0CF800);
            for (int i = data.Length; --i >= 0;)
                data[i] = ROM[address + i];

            return CHRFile.InitializeEditor(data);
        }

        public GFXEditor GetOBJGFX()
        {
            var data = new byte[0x4000];

            int address = ROM.SNESToPC(0x078000);
            for (int i = data.Length; --i >= 0;)
                data[i] = ROM[address + i];

            return CHRFile.InitializeEditor(data);
        }

        public Obj16Editor GetMap16Tiles()
        {
            var data = new byte[0x100 * 8];

            int loPointer = ROM.SNESToPC(Map16ChunkAddressLow);
            int hiPointer = ROM.SNESToPC(Map16ChunkAddressHigh);

            for (int i = 0; i < 4; i++)
            {
                int x = i * 0x40 * 8;
                int address = 0x030000 | ROM[loPointer + i] | (ROM[hiPointer + i] << 8);
                address = ROM.SNESToPC(address);
                for (int j = 0; j < 0x40 * 8; j++)
                {
                    data[x + j] = ROM[address + j];
                }
            }

            return MAP16File.InitializeEditor(data);
        }

        private int GetCeilingSize(TerrainMode terrain)
        {
            switch (terrain)
            {
            case TerrainMode.None:
            case TerrainMode.Ceiling0Floor2:
                return 0;
            case TerrainMode.Ceiling1Floor2:
            case TerrainMode.Ceiling1Floor5:
            case TerrainMode.Ceiling1Floor6:
            case TerrainMode.Ceiling1Floor0:
            case TerrainMode.Ceiling1Floor9:
            case TerrainMode.Ceiling1Middle5Floor2:
            case TerrainMode.Ceiling1Middle4Floor2:
                return 1;
            case TerrainMode.Ceiling3Floor2:
            case TerrainMode.Ceiling3Floor5:
                return 3;
            case TerrainMode.Ceiling4Floor2:
            case TerrainMode.Ceiling4Floor5:
            case TerrainMode.Ceiling4Floor6:
                return 4;
            case TerrainMode.Ceiling8Floor2:
                return 8;
            case TerrainMode.Solid:
                return 0x10;
            default:
                return -1;
            }
        }

        private void InsertCastleTiles(List<LevelObject> objects, TerrainMode terrain, TerrainMode last)
        {
            switch (terrain)
            {
            case TerrainMode.None:
                return;
            case TerrainMode.Ceiling0Floor2:
                return;
            case TerrainMode.Ceiling1Floor2:
                return;
            case TerrainMode.Ceiling3Floor2:
                return;
            case TerrainMode.Ceiling4Floor2:
                return;
            case TerrainMode.Ceiling8Floor2:
                return;
            case TerrainMode.Ceiling1Floor5:
                return;
            case TerrainMode.Ceiling3Floor5:
                return;
            case TerrainMode.Ceiling4Floor5:
                return;
            case TerrainMode.Ceiling1Floor6:
                return;
            case TerrainMode.Ceiling1Floor0:
                return;
            case TerrainMode.Ceiling4Floor6:
                return;
            case TerrainMode.Ceiling1Floor9:
                return;
            case TerrainMode.Ceiling1Middle5Floor2:
                return;
            case TerrainMode.Ceiling1Middle4Floor2:
                return;
            case TerrainMode.Solid:
                return;
            }
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

        public static LevelData[] GetAllLevels(ROM src)
        {
            var levels = new LevelData[NumberOfMaps];

            var areas = src.SNESToPC(AreaTypeLevelOffsetPointer);
            var ws = src[areas + 0];
            var gs = src[areas + 1];
            var us = src[areas + 2];
            var cs = src[areas + 3];
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

        public static bool WriteAllLevels(ROM dest, byte[] src)
        {
            var data = dest.GetData();

            unsafe
            {
                fixed (byte* ptr = data)
                {
                    var first = dest.SNESToPC(0x04C1D8);
                    var last = dest.SNESToPC(0x04D800);
                    var range = last - first;

                    var nlevels = NES.SMB1.LevelData.GetAllLevels(src);
                    var slevels = new LevelData[NumberOfMaps];

                    var size = 0;
                    for (int i = 0; i < NumberOfMaps; i++)
                    {
                        slevels[i] = new LevelData(nlevels[i]);
                        size += slevels[i].TotalSize;
                    }

                    if (size >= range)
                        return false;

                    var msrc = AddressConverter.NesToPc(NES.SMB1.LevelData.MapListPointer);
                    var mdest = dest.SNESToPC(MapListPointer);
                    for (int i = 0; i < NumberOfMaps; i++)
                        ptr[mdest + i] = src[msrc + i];

                    var wsrc = AddressConverter.NesToPc(NES.SMB1.LevelData.LevelsPerWorldPointer);
                    var wdest = dest.SNESToPC(LevelsPerWorldPointer);
                    for (int i = 0; i < 8; i++)
                        ptr[wdest + i] = src[wsrc + i];

                    var atesrc = AddressConverter.NesToPc(NES.SMB1.LevelData.AreaTypeEnemyOffsetPointer);
                    var atedest = dest.SNESToPC(AreaTypeEnemyOffsetPointer);
                    for (int i = 0; i < 4; i++)
                        ptr[atedest + i] = src[atesrc + i];

                    var atlsrc = AddressConverter.NesToPc(NES.SMB1.LevelData.AreaTypeLevelOffsetPointer);
                    var atldest = dest.SNESToPC(AreaTypeLevelOffsetPointer);
                    for (int i = 0; i < 4; i++)
                        ptr[atldest + i] = src[atlsrc + i];

                    var lldest = dest.SNESToPC(LevelAddressLowBytePointer);
                    var hldest = dest.SNESToPC(LevelAddressHighBytePointer);

                    var ledest = dest.SNESToPC(EnemyAddressLowBytePointer);
                    var hedest = dest.SNESToPC(EnemyAddressHighBytePointer);

                    var address = first;
                    for (int i = 0; i < NumberOfMaps; i++)
                    {
                        var level = slevels[i];
                        var map = level.MapNumber;
                        var type = (int)level.AreaType;
                        var reduced = map & 0x1F;
                        var index = reduced + ptr[atldest + type];

                        var snes = dest.PCToSNES(address);
                        ptr[lldest + index] = (byte)snes;
                        ptr[hldest + index] = (byte)(snes >> 8);

                        ptr[address++] = level.LevelHeader.Value1;
                        ptr[address++] = level.LevelHeader.Value2;
                        for (int j = 0; j < level.LevelObjects.Length; j++)
                        {
                            var obj = level.LevelObjects[j];
                            ptr[address++] = obj.Value1;
                            ptr[address++] = obj.Value2;
                            if (obj.Size == 3)
                                ptr[address++] = obj.Value3;
                        }
                        ptr[address++] = 0xFD;

                        index = reduced + ptr[atedest + type];
                        snes = dest.PCToSNES(address);
                        ptr[ledest + index] = (byte)snes;
                        ptr[hedest + index] = (byte)(snes >> 8);

                        for (int j = 0; j < level.EnemyObjects.Length; j++)
                        {
                            var obj = level.EnemyObjects[j];
                            ptr[address++] = obj.Value1;
                            ptr[address++] = obj.Value2;
                            if (obj.Size == 3)
                                ptr[address++] = obj.Value3;
                        }
                        ptr[address++] = 0xFF;
                    }
                }
            }

            return true;
        }

        public override string ToString()
        {
            return "Map " + MapNumber.ToString("X2");
        }
    }
}
