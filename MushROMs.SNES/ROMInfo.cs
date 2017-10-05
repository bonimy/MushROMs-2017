using System;

namespace MushROMs.SNES
{
    public class ROMInfo
    {
        public HeaderType Header
        {
            get;
            set;
        }
        public AddressMode AddressMode
        {
            get;
            set;
        }

        public int MakerCode
        {
            get;
            set;
        }
        public int GameCode
        {
            get;
            set;
        }
        public int ExpandedRAMSize
        {
            get;
            set;
        }
        public int SpecialVersion
        {
            get;
            set;
        }
        public int CartType
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public int MapMode
        {
            get;
            set;
        }
        public int Type
        {
            get;
            set;
        }
        public int PageSize
        {
            get;
            set;
        }
        public int SRAMSize
        {
            get;
            set;
        }
        public int DestCode
        {
            get;
            set;
        }
        public int FixedValue
        {
            get;
            set;
        }
        public int VersionNumber
        {
            get;
            set;
        }
        public int ComplementCheckSum
        {
            get;
            set;
        }
        public int CheckSum
        {
            get;
            set;
        }

        public int NativeCOPVector
        {
            get;
            set;
        }
        public int NativeBRKVector
        {
            get;
            set;
        }
        public int NativeAbortVector
        {
            get;
            set;
        }
        public int NativeNMIVector
        {
            get;
            set;
        }
        public int NativeResetVector
        {
            get;
            set;
        }
        public int NativeIRQVector
        {
            get;
            set;
        }

        public int EmuCOPVector
        {
            get;
            set;
        }
        public int EmuBRKVector
        {
            get;
            set;
        }
        public int EmuAbortVector
        {
            get;
            set;
        }
        public int EmuNMIVector
        {
            get;
            set;
        }
        public int EmuResetVector
        {
            get;
            set;
        }
        public int EmuIRQVector
        {
            get;
            set;
        }

        public bool IsFastROM
        {
            get;
            set;
        }
        public bool IsHiROM
        {
            get;
            set;
        }

        public ROMInfo()
        {
            Header = HeaderType.NoHeader;
            AddressMode = AddressMode.HiROM;
            MakerCode = 0;
            GameCode = 0;
            ExpandedRAMSize = 0;
            SpecialVersion = 0;
            CartType = 0;
            Name = String.Empty;
            MapMode = 0;
            Type = 0;
            PageSize = 7;
            SRAMSize = 0;
            DestCode = 0;
            FixedValue = 0;
            VersionNumber = 0;
            ComplementCheckSum = 0xFFFF;
            CheckSum = 0;

            NativeCOPVector = 0xFFFF;
            NativeBRKVector = 0xFFFF;
            NativeAbortVector = 0xFFFF;
            NativeNMIVector = 0xFFFF;
            NativeResetVector = 0xFFFF;
            NativeIRQVector = 0xFFFF;

            EmuCOPVector = 0xFFFF;
            EmuBRKVector = 0xFFFF;
            EmuAbortVector = 0xFFFF;
            EmuNMIVector = 0xFFFF;
            EmuResetVector = 0xFFFF;
            EmuIRQVector = 0xFFFF;

            IsFastROM = false;
            IsHiROM = true;
        }

        public ROMInfo(ROM rom)
        {
            if (rom == null)
                throw new ArgumentNullException(nameof(rom));

            Header = rom.Header;
            AddressMode = rom.AddressMode;
            MakerCode = rom.MakerCode;
            GameCode = rom.GameCode;
            ExpandedRAMSize = rom.ExpandedRAMSize;
            SpecialVersion = rom.SpecialVersion;
            CartType = rom.CartType;
            Name = rom.Name;
            MapMode = rom.MapMode;
            Type = rom.Type;
            PageSize = rom.PageSize;
            SRAMSize = rom.SRAMSize;
            DestCode = rom.DestCode;
            FixedValue = rom.FixedValue;
            VersionNumber = rom.VersionNumber;
            ComplementCheckSum = rom.ComplementChecksum;
            CheckSum = rom.Checksum;

            NativeCOPVector = rom.NativeCOPVector;
            NativeBRKVector = rom.NativeBRKVector;
            NativeAbortVector = rom.NativeAbortVector;
            NativeNMIVector = rom.NativeNMIVector;
            NativeResetVector = rom.NativeResetVector;
            NativeIRQVector = rom.NativeIRQVector;

            EmuCOPVector = rom.EmuCOPVector;
            EmuBRKVector = rom.EmuBRKVector;
            EmuAbortVector = rom.EmuAbortVector;
            EmuNMIVector = rom.EmuNMIVector;
            EmuResetVector = rom.EmuResetVector;
            EmuIRQVector = rom.EmuIRQVector;

            IsFastROM = rom.IsFastROM;
            IsHiROM = rom.IsFastROM;
        }
    }
}
