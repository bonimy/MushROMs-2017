using System;

namespace MushROMs.SNES
{
    public class ROM
    {
        public const int StandardHeaderSize = 0x200;

        public const int MakerCodeAddress = 0xFFB0;
        public const int GameCodeAddress = 0xFFB2;
        public const int ExpandedRAMSizeAddress = 0xFFBD;
        public const int SpecialVersionAddress = 0xFFBE;
        public const int CartTypeAddress = 0xFFBF;
        public const int NameAddress = 0xFFC0;
        public const int MaxNameLength = 21;
        public const int MapModeAddress = 0xFFD5;
        public const int TypeAddress = 0xFFD6;
        public const int PageSizeAddress = 0xFFD7;
        public const int MinPageSize = 7;
        public const int MaxPageSize = 0x0D;
        public const int PageBase = 0x400;
        public const int SRAMSizeAddress = 0xFFD8;
        public const int DestCodeAddress = 0xFFD9;
        public const int FixedValueAddress = 0xFFDA;
        public const int VersionNumberAddress = 0xFFDB;
        public const int ComplementCheckSumAddress = 0xFFDC;
        public const int CheckSumAddress = 0xFFDE;

        public const int NativeCOPVectorAddress = 0xFFE4;
        public const int NativeBRKVectorAddress = 0xFFE6;
        public const int NativeAbortVectorAddress = 0xFFE8;
        public const int NativeNMIVectorAddress = 0xFFEA;
        public const int NativeResetVectorAddress = 0xFFEC;
        public const int NativeIRQVectorAddress = 0xFFEE;

        public const int EmuCOPVectorAddress = 0xFFF4;
        public const int EmuBRKVectorAddress = 0xFFF6;
        public const int EmuAbortVectorAddress = 0xFFF8;
        public const int EmuNMIVectorAddress = 0xFFFA;
        public const int EmuResetVectorAddress = 0xFFFC;
        public const int EmuIRQVectorAddress = 0xFFFE;

        private byte[] Data
        {
            get;
            set;
        }
        public byte[] GetData()
        {
            return Data;
        }
        public int HeaderSize => GetHeaderSize(Length); public HeaderType Header => (HeaderType)HeaderSize; public AddressMode AddressMode
        {
            get;
            private set;
        }
        public int Length => Data.Length; public int HeaderlessLength => Length - (int)Header;
        public int MakerCode
        {
            get => GetInt16(MakerCodeAddress);
            set => SetInt16(MakerCodeAddress, value);
        }
        public int GameCode
        {
            get => GetInt32(GameCodeAddress);
            set => SetInt32(GameCodeAddress, value);
        }
        public int ExpandedRAMSize
        {
            get => GetInt8(ExpandedRAMSizeAddress);
            set => SetInt8(ExpandedRAMSizeAddress, value);
        }
        public int SpecialVersion
        {
            get => GetInt8(SpecialVersionAddress);
            set => SetInt8(SpecialVersionAddress, value);
        }
        public int CartType
        {
            get => GetInt8(CartTypeAddress);
            set => SetInt8(CartTypeAddress, value);
        }

        public string Name
        {
            get
            {
                unsafe
                {
                    fixed (byte* ptr = Data)
                        return new string((sbyte*)ptr, NameAddress, MaxNameLength);
                }
            }
            set
            {
                unsafe
                {
                    fixed (byte* ptr = &Data[NameAddress])
                    fixed (char* str = value)
                        for (int i = Math.Min(value.Length, MaxNameLength); --i >= 0;)
                            ptr[i] = (byte)str[i];
                }
            }
        }
        public int MapMode
        {
            get => GetInt8(MapModeAddress);
            set => SetInt8(MapModeAddress, value);
        }
        public int Type
        {
            get => GetInt8(TypeAddress);
            set => SetInt8(TypeAddress, value);
        }
        public int PageSize
        {
            get => GetInt8(PageSizeAddress);
            set => SetInt8(PageSizeAddress, value);
        }
        public int SRAMSize
        {
            get => GetInt8(SRAMSizeAddress);
            set => SetInt8(SRAMSizeAddress, value);
        }
        public int DestCode
        {
            get => GetInt8(DestCodeAddress);
            set => SetInt8(DestCodeAddress, value);
        }
        public int FixedValue
        {
            get => GetInt8(FixedValueAddress);
            set => SetInt8(FixedValueAddress, value);
        }
        public int VersionNumber
        {
            get => GetInt8(VersionNumberAddress);
            set => SetInt8(VersionNumberAddress, value);
        }
        public int ComplementChecksum
        {
            get => GetInt16(ComplementCheckSumAddress);
            set => SetInt16(ComplementCheckSumAddress, value);
        }
        public int Checksum
        {
            get => GetInt16(CheckSumAddress);
            set => SetInt16(CheckSumAddress, value);
        }

        public int NativeCOPVector
        {
            get => GetInt16(NativeCOPVectorAddress);
            set => SetInt16(NativeCOPVectorAddress, value);
        }
        public int NativeBRKVector
        {
            get => GetInt16(NativeBRKVectorAddress);
            set => SetInt16(NativeBRKVectorAddress, value);
        }
        public int NativeAbortVector
        {
            get => GetInt16(NativeAbortVectorAddress);
            set => SetInt16(NativeAbortVectorAddress, value);
        }
        public int NativeNMIVector
        {
            get => GetInt16(NativeNMIVectorAddress);
            set => SetInt16(NativeNMIVectorAddress, value);
        }
        public int NativeResetVector
        {
            get => GetInt16(NativeResetVectorAddress);
            set => SetInt16(NativeResetVectorAddress, value);
        }
        public int NativeIRQVector
        {
            get => GetInt16(NativeIRQVectorAddress);
            set => SetInt16(NativeIRQVectorAddress, value);
        }

        public int EmuCOPVector
        {
            get => GetInt16(EmuCOPVectorAddress);
            set => SetInt16(EmuCOPVectorAddress, value);
        }
        public int EmuBRKVector
        {
            get => GetInt16(EmuBRKVectorAddress);
            set => SetInt16(EmuBRKVectorAddress, value);
        }
        public int EmuAbortVector
        {
            get => GetInt16(EmuAbortVectorAddress);
            set => SetInt16(EmuAbortVectorAddress, value);
        }
        public int EmuNMIVector
        {
            get => GetInt16(EmuNMIVectorAddress);
            set => SetInt16(EmuNMIVectorAddress, value);
        }
        public int EmuResetVector
        {
            get => GetInt16(EmuResetVectorAddress);
            set => SetInt16(EmuResetVectorAddress, value);
        }
        public int EmuIRQVector
        {
            get => GetInt16(EmuIRQVectorAddress);
            set => SetInt16(EmuIRQVectorAddress, value);
        }

        public bool IsFastROM
        {
            get => (MapMode & 0x30) == 0x30;
            set
            {
                if (value)
                    MapMode |= 0x30;
                else
                    MapMode &= ~0x30;
            }
        }
        public bool IsHiROM
        {
            get => (MapMode & 1) != 0;
            set
            {
                if (value)
                    MapMode |= 1;
                else
                    MapMode &= ~1;
            }
        }

        public byte this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        private ROM()
        {
        }

        public ROM(ROMInfo info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (info.PageSize < MinPageSize || info.PageSize > MaxPageSize)
            {
                throw new ArgumentException("ROM Info's page size is an invalid size.",
                    nameof(info));
            }

            switch (info.Header)
            {
            case HeaderType.Header:
            case HeaderType.NoHeader:
                break;
            default:
                throw new ArgumentException("ROM Info's header value is invalid.", nameof(info));
            }

            switch (info.AddressMode)
            {
            case AddressMode.HiROM2:
                info.AddressMode = AddressMode.HiROM;
                break;
            case AddressMode.LoROM2:
                info.AddressMode = AddressMode.LoROM;
                break;
            }

            switch (info.AddressMode)
            {
            case AddressMode.HiROM:
            case AddressMode.ExHiROM:
                if (!info.IsHiROM)
                    throw new ArgumentException("ROM Info's HiROM bit does not match given Address mode.",
                        nameof(info));
                break;
            case AddressMode.LoROM:
            case AddressMode.ExLoROM:
                if (info.IsHiROM)
                    throw new ArgumentException("ROM Info's HiROM bit does not match given Address mode.",
                        nameof(info));
                break;
            default:
                throw new ArgumentException("ROM Info's address mode is invalid.", nameof(info));
            }

            var minSize = new int[] { 0, 0x8000, 0x10000, 0x410000, 0x408000 };
            var size = CalculatePageSize(info.PageSize);
            if (size < minSize[(int)info.AddressMode])
                throw new ArgumentException("ROM Info's page size does not specify a size that fits the address mode.",
                    nameof(info));

            Data = new byte[size + (int)info.Header];

            AddressMode = info.AddressMode;
            MakerCode = info.MakerCode;
            GameCode = info.GameCode;
            ExpandedRAMSize = info.ExpandedRAMSize;
            SpecialVersion = info.SpecialVersion;
            CartType = info.CartType;
            Name = info.Name;
            MapMode = info.MapMode;
            Type = info.Type;
            PageSize = info.PageSize;
            SRAMSize = info.SRAMSize;
            DestCode = info.DestCode;
            FixedValue = info.FixedValue;
            VersionNumber = info.VersionNumber;

            NativeCOPVector = info.NativeCOPVector;
            NativeBRKVector = info.NativeBRKVector;
            NativeAbortVector = info.NativeAbortVector;
            NativeNMIVector = info.NativeNMIVector;
            NativeResetVector = info.NativeResetVector;
            NativeIRQVector = info.NativeIRQVector;

            EmuCOPVector = info.EmuCOPVector;
            EmuBRKVector = info.EmuBRKVector;
            EmuAbortVector = info.EmuAbortVector;
            EmuNMIVector = info.EmuNMIVector;
            EmuResetVector = info.EmuResetVector;
            EmuIRQVector = info.EmuIRQVector;

            IsFastROM = info.IsFastROM;
            IsHiROM = info.IsFastROM;
        }

        public static ROM CreateFromData(byte[] data)
        {
            var rom = new ROM()
            {
                Data = data ?? throw new ArgumentNullException(nameof(data))
            };

            // The header is automatically calculated based on the Data length remainder.
            switch (rom.Header)
            {
            case HeaderType.NoHeader:
            case HeaderType.Header:
                break;
            default:
                throw new Exception("Invalid header size.");
            }

            rom.VerifyAddressMode();
            if (rom.AddressMode == AddressMode.NoBank)
                throw new Exception("Could not verify address mode of data.");

            return rom;
        }

        private void VerifyAddressMode()
        {
            // Try to find the right address mode.
            // Check exrom modes first as first bank is usually mirrored
            var modes = new AddressMode[] {
                AddressMode.ExHiROM,
                AddressMode.ExLoROM,
                AddressMode.HiROM,
                AddressMode.LoROM};
            var minSize = new int[] { 0x410000, 0x408000, 0x10000, 0x8000 };

            for (int i = 0; i < modes.Length; i++)
            {
                // If the ROM data is less than a required size, we know it cannot be that address mode
                if (HeaderlessLength < minSize[i])
                    continue;

                // We need to set the address mode to properly use address conversions
                AddressMode = modes[i];

                // The checksum and its complement should match.
                if ((Checksum ^ ComplementChecksum) == UInt16.MaxValue)
                {
                    // Now we check that the address mode is matching.
                    switch (AddressMode)
                    {
                    case AddressMode.HiROM:
                        if (IsFastROM)
                            AddressMode = AddressMode.HiROM2;
                        if (!IsHiROM)
                            continue;
                        break;
                    case AddressMode.LoROM:
                        if (IsFastROM)
                            AddressMode = AddressMode.LoROM2;
                        if (IsHiROM)
                            continue;
                        break;
                    case AddressMode.ExHiROM:
                        if (!IsHiROM)
                            continue;
                        break;
                    case AddressMode.ExLoROM:
                        if (IsHiROM)
                            continue;
                        break;
                    default:
                        continue;
                    }

                    // Finally we check the internal size
                    if (PageSize < MinPageSize || PageSize > MaxPageSize)
                        continue;

                    var length = CalculatePageSize(PageSize);
                    if (HeaderlessLength > length || HeaderlessLength <= (length >> 1))
                        continue;

                    // Everything in the header is valid.
                    return;
                }
            }

            // None of the verifications were successful.
            AddressMode = AddressMode.NoBank;
        }

        private static int CalculatePageSize(int size)
        {
            return PageBase << size;
        }

        public int ComputeChecksum()
        {
            var sum = 0;
            unsafe
            {
                fixed (byte* ptr = &Data[(int)Header])
                    for (int i = HeaderlessLength; --i >= 0;)
                        sum += ptr[i];
            }
            return sum & UInt16.MaxValue;
        }

        public int SNESToPC(int pointer)
        {
            return SNESToPC(pointer, AddressMode, Header);
        }
        public int PCToSNES(int pointer)
        {
            return PCToSNES(pointer, AddressMode, Header);
        }

        public int GetInt8(int pointer)
        {
            return this[SNESToPC(pointer)];
        }
        public int GetInt16(int pointer)
        {
            return this[SNESToPC(pointer)] | (this[SNESToPC(pointer + 1)] << 8);
        }
        public int GetInt24(int pointer)
        {
            return this[SNESToPC(pointer)] | (this[SNESToPC(pointer + 1)] << 8) |
                (this[SNESToPC(pointer + 2)] << 0x10);
        }
        public int GetInt32(int pointer)
        {
            return this[SNESToPC(pointer)] | (this[SNESToPC(pointer + 1)] << 8) |
                (this[SNESToPC(pointer + 2)] << 0x10) | (this[SNESToPC(pointer + 3)] << 0x18);
        }

        public void SetInt8(int pointer, int value)
        {
            this[SNESToPC(pointer)] = (byte)value;
        }
        public void SetInt16(int pointer, int value)
        {
            this[SNESToPC(pointer)] = (byte)value;
            this[SNESToPC(pointer + 1)] = (byte)(value >> 8);
        }
        public void SetInt24(int pointer, int value)
        {
            this[SNESToPC(pointer)] = (byte)value;
            this[SNESToPC(pointer + 1)] = (byte)(value >> 8);
            this[SNESToPC(pointer + 2)] = (byte)(value >> 0x10);
        }
        public void SetInt32(int pointer, int value)
        {
            this[SNESToPC(pointer)] = (byte)value;
            this[SNESToPC(pointer + 1)] = (byte)(value >> 8);
            this[SNESToPC(pointer + 2)] = (byte)(value >> 0x10);
            this[SNESToPC(pointer + 3)] = (byte)(value >> 0x18);
        }

        public static int GetHeaderSize(int length)
        {
            return length & 0x7FFF;
        }

        public static int SNESToPC(int pointer, AddressMode mode, HeaderType header)
        {
            switch (mode)
            {
            case AddressMode.LoROM:
            case AddressMode.LoROM2:
                return (((pointer & 0x7F0000) >> 1) | (pointer & 0x7FFF)) + (int)header;
            case AddressMode.HiROM:
            case AddressMode.HiROM2:
                return (pointer & 0x3FFFFF) + (int)header;
            case AddressMode.ExHiROM:
                if (pointer >= 0xC00000)
                    return (pointer & 0x3FFFFF) + (int)header;
                return (0x400000 | (pointer & 0x3FFFFF)) + (int)header;
            case AddressMode.ExLoROM:
                if (pointer >= 0x800000)
                    return (((pointer & 0x7F0000) >> 1) | (pointer & 0x7FFF)) + (int)header;
                return (0x400000 | (((pointer & 0x7F0000) >> 1) | (pointer & 0x7FFF))) + (int)header;
            default:
                return 0;
            }
        }

        public static int PCToSNES(int pointer, AddressMode mode, HeaderType header)
        {
            switch (mode)
            {
            case AddressMode.LoROM:
                if (pointer >= 0x380000)
                    return (0x800000 | (((pointer << 1) & 0x7F0000) | 0x8000 | (pointer & 0x7FFF))) + (int)header;
                return (((pointer << 1) & 0x7F0000) | 0x8000 | (pointer & 0x7FFF)) + (int)header;
            case AddressMode.HiROM:
                return (pointer | 0xC00000) + (int)header;
            case AddressMode.ExHiROM:
                if (pointer >= 0x7E0000)
                    return (pointer & ~0x400000) + (int)header;
                if (pointer >= 0x4000000)
                    return pointer + (int)header;
                return (0xC00000 | pointer) + (int)header;
            case AddressMode.ExLoROM:
                if (pointer >= 0x400000)
                    return (0x80000 | (((pointer << 1) & 0x7F0000) | 0x8000 | (pointer & 0x7FFF))) + (int)header;
                return (((pointer << 1) & 0x7F0000) | 0x8000 | (pointer & 0x7FFF)) + (int)header;
            case AddressMode.LoROM2:
                return (0x800000 | (((pointer << 1) & 0x7F0000) | 0x8000 | (pointer & 0x7FFF))) + (int)header;
            case AddressMode.HiROM2:
                if (pointer >= 0x300000)
                    return (0xC00000 | pointer) + (int)header;
                return (0x400000 | pointer) + (int)header;
            default:
                return 0;
            }
        }
    }
}
