namespace MushROMs.Assembler
{
    public class Label
    {
        public string Name
        {
            get;
            private set;
        }

        public int Address
        {
            get;
            private set;
        }

        public bool Resolved
        {
            get;
            private set;
        }

        public bool Relative
        {
            get;
            private set;
        }

        public Label(string name)
        {

        }
    }
}
