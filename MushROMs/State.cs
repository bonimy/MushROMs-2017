namespace MushROMs
{
    internal struct State
    {
        public ISelectionData Undo
        {
            get;
            private set;
        }
        public ISelectionData Redo
        {
            get;
            private set;
        }

        private State(ISelectionData undo, ISelectionData redo)
        {
            Undo = undo;
            Redo = redo;
        }

        public State(ISelectionData data, Editor editor) :
            this(data.Selection.GetSelectionData(editor), data)
        {
        }
    }
}
