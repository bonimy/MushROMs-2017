namespace MushROMs.Assembler
{
    internal class FreeSpaceData
    {
        public int Size
        {
            get;
            private set;
        }

        private byte[] Data
        {
            get;
            set;
        }

        private struct LabelReference
        {
            public int RelativeAddress
            {
                get;
                private set;
            }


        }
    }
}
