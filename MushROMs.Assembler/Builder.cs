using System.Collections.Generic;
using MushROMs.SNES;

namespace MushROMs.Assembler
{
    public partial class Builder
    {
        private ROM ROM
        {
            get;
            set;
        }

        private ROMInfo ROMInfo
        {
            get;
            set;
        }

        public int CurrentAddress
        {
            get;
            set;
        }

        private Log Log
        {
            get;
            set;
        }

        private List<DirectWriter> DirectWriterList
        {
            get;
            set;
        }

        public DirectWriter DirectWriter
        {
            get;
            private set;
        }

        public Builder()
        {
            ROMInfo = new ROMInfo();

            DirectWriterList = new List<DirectWriter>();
            SetPosition(0x8000);
        }

        public void SetPosition(int address)
        {
            CurrentAddress = address;
            DirectWriterList.Add(DirectWriter = new DirectWriter(address));
        }

        public void WriteInstruction(byte code, int value, int size)
        {
            CurrentAddress += 1 + size;
            DirectWriter.Data.Add(code);
            for (int i = 0; i < size; i++)
                DirectWriter.Data.Add((byte)(value >> (i << 3)));
        }

        public void WriteData(string path)
        {

        }
    }
}
