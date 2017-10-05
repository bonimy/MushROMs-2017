using MushROMs.SNES;

namespace MushROMs.Assembler
{
    partial class Builder
    {
        private bool HeaderSet
        {
            get;
            set;
        }
        private bool AddressModeSet
        {
            get;
            set;
        }

        private bool MakerCodeSet
        {
            get;
            set;
        }
        private bool GameCodeSet
        {
            get;
            set;
        }
        private bool ExpandedRAMSizeSet
        {
            get;
            set;
        }
        private bool SpecialVersionSet
        {
            get;
            set;
        }
        private bool CartTypeSet
        {
            get;
            set;
        }
        private bool NameSet
        {
            get;
            set;
        }
        private bool MapModeSet
        {
            get;
            set;
        }
        private bool TypeSet
        {
            get;
            set;
        }
        private bool PageSizeSet
        {
            get;
            set;
        }
        private bool SRAMSizeSet
        {
            get;
            set;
        }
        private bool DestCodeSet
        {
            get;
            set;
        }
        private bool FixedValueSet
        {
            get;
            set;
        }
        private bool VersionNumberSet
        {
            get;
            set;
        }
        private bool ComplementCheckSumSet
        {
            get;
            set;
        }
        private bool CheckSumSet
        {
            get;
            set;
        }

        private bool NativeCOPVectorSet
        {
            get;
            set;
        }
        private bool NativeBRKVectorSet
        {
            get;
            set;
        }
        private bool NativeAbortVectorSet
        {
            get;
            set;
        }
        private bool NativeNMIVectorSet
        {
            get;
            set;
        }
        private bool NativeResetVectorSet
        {
            get;
            set;
        }
        private bool NativeIRQVectorSet
        {
            get;
            set;
        }

        private bool EmuCOPVectorSet
        {
            get;
            set;
        }
        private bool EmuBRKVectorSet
        {
            get;
            set;
        }
        private bool EmuAbortVectorSet
        {
            get;
            set;
        }
        private bool EmuNMIVectorSet
        {
            get;
            set;
        }
        private bool EmuResetVectorSet
        {
            get;
            set;
        }
        private bool EmuIRQVectorSet
        {
            get;
            set;
        }

        public void SetHeader(HeaderType header)
        {
            if (HeaderSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.Header = header;
            HeaderSet = true;
        }

        public void SetAddressMode(AddressMode addressMode)
        {
            if (AddressModeSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.AddressMode = addressMode;
            AddressModeSet = true;
        }

        public void SetMakerCode(int makerCode)
        {
            if (MakerCodeSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.MakerCode = makerCode;
            MakerCodeSet = true;
        }

        public void SetGameCode(int gameCode)
        {
            if (GameCodeSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.GameCode = gameCode;
            GameCodeSet = true;
        }

        public void SetExpandedRAMSize(int expandedRAMSize)
        {
            if (ExpandedRAMSizeSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.ExpandedRAMSize = expandedRAMSize;
            ExpandedRAMSizeSet = true;
        }

        public void SetSpecialVersion(int specialVersion)
        {
            if (SpecialVersionSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.SpecialVersion = specialVersion;
            SpecialVersionSet = true;
        }

        public void setCartType(int cartType)
        {
            if (CartTypeSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.CartType = cartType;
            CartTypeSet = true;
        }

        public void SetName(string name)
        {
            if (NameSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.Name = name;
            NameSet = true;
        }

        public void SetType(int type)
        {
            if (TypeSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.Type = type;
            TypeSet = true;
        }

        public void SetPageSize(int pageSize)
        {
            if (PageSizeSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.PageSize = pageSize;
            PageSizeSet = true;
        }

        public void SetSRAMSize(int sramSize)
        {
            if (SRAMSizeSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.SRAMSize = sramSize;
            SRAMSizeSet = true;
        }

        public void SetDestCode(int destCode)
        {
            if (DestCodeSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.DestCode = destCode;
            DestCodeSet = true;
        }

        public void SetFixedValue(int fixedValue)
        {
            if (FixedValueSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.FixedValue = fixedValue;
            FixedValueSet = true;
        }

        public void SetVersionNumber(int versionNumber)
        {
            if (VersionNumberSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.VersionNumber = versionNumber;
            VersionNumberSet = true;
        }

        public void SetComplementCheckSum(int complementCheckSum)
        {
            if (ComplementCheckSumSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.ComplementCheckSum = complementCheckSum;
            ComplementCheckSumSet = true;
        }

        public void SetCheckSum(int checkSum)
        {
            if (CheckSumSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.CheckSum = checkSum;
            CheckSumSet = true;
        }

        public void SetNativeCOPVector(int nativeCOPVector)
        {
            if (NativeCOPVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.NativeCOPVector = nativeCOPVector;
            NativeCOPVectorSet = true;
        }

        public void SetNativeBRKVector(int nativeBRKVector)
        {
            if (NativeBRKVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.NativeBRKVector = nativeBRKVector;
            NativeBRKVectorSet = true;
        }

        public void SetNativeAbortVector(int nativeAbortVector)
        {
            if (NativeAbortVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.NativeAbortVector = nativeAbortVector;
            NativeAbortVectorSet = true;
        }

        public void SetNativeNMIVector(int nativeNMIVector)
        {
            if (NativeNMIVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.NativeNMIVector = nativeNMIVector;
            NativeNMIVectorSet = true;
        }

        public void SetNativeResetVector(int nativeResetVector)
        {
            if (NativeResetVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.NativeResetVector = nativeResetVector;
            NativeResetVectorSet = true;
        }

        public void SetNativeIRQVector(int nativeIRQVector)
        {
            if (NativeIRQVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.NativeIRQVector = nativeIRQVector;
            NativeIRQVectorSet = true;
        }

        public void SetEmuCOPVector(int EmuCOPVector)
        {
            if (EmuCOPVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.EmuCOPVector = EmuCOPVector;
            EmuCOPVectorSet = true;
        }

        public void SetEmuBRKVector(int EmuBRKVector)
        {
            if (EmuBRKVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.EmuBRKVector = EmuBRKVector;
            EmuBRKVectorSet = true;
        }

        public void SetEmuAbortVector(int EmuAbortVector)
        {
            if (EmuAbortVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.EmuAbortVector = EmuAbortVector;
            EmuAbortVectorSet = true;
        }

        public void SetEmuNMIVector(int EmuNMIVector)
        {
            if (EmuNMIVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.EmuNMIVector = EmuNMIVector;
            EmuNMIVectorSet = true;
        }

        public void SetEmuResetVector(int EmuResetVector)
        {
            if (EmuResetVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.EmuResetVector = EmuResetVector;
            EmuResetVectorSet = true;
        }

        public void SetEmuIRQVector(int EmuIRQVector)
        {
            if (EmuIRQVectorSet)
            {
                Log.AddError(ErrorCode.UnknownErrorCode);
                return;
            }

            ROMInfo.EmuIRQVector = EmuIRQVector;
            EmuIRQVectorSet = true;
        }
    }
}
