using System;

namespace MushROMs.SNES
{
    public sealed class Obj16Data : ISelectionData
    {
        private Obj16Tile[] Data
        {
            get;
            set;
        }

        public Obj16Selection Selection
        {
            get;
            private set;
        }

        public Obj16Data(Obj16Editor editor, Obj16Selection selection)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));

            Selection = selection ?? throw new ArgumentNullException(nameof(selection));
            Data = new Obj16Tile[Selection.NumTiles];
            var startIndex = Selection.StartIndex;
            var data = editor.GetData();
            var length = data.Length;

            unsafe
            {
                fixed (int* indexes = Selection.TileMapSelection.GetSelectedIndexes())
                fixed (byte* ptr = &data[Selection.StartAddress])
                fixed (Obj16Tile* dest = Data)
                {
                    var src = (Obj16Tile*)ptr + startIndex;
                    for (int i = Selection.NumTiles; --i >= 0;)
                        if (indexes[i] + startIndex < length)
                            dest[i] = src[indexes[i]];
                }

            }
        }

        private Obj16Data(Obj16Data data, Obj16Selection selection)
        {
            Data = new Obj16Tile[data.Data.Length];
            Array.Copy(data.Data, Data, Data.Length);

            var tilemap = data.Selection.TileMapSelection.Copy(selection.StartIndex);
            Selection = new Obj16Selection(data.Selection.StartAddress, tilemap);
        }

        public Obj16Tile[] GetData()
        {
            return Data;
        }

        public Obj16Data Copy(Obj16Selection selection)
        {
            return new Obj16Data(this, selection);
        }

        public void WriteToEditor(Obj16Editor editor)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));

            var startIndex = Selection.StartIndex;
            var length = Obj16Editor.GetIndexFromAddress(editor.GetData().Length);

            unsafe
            {
                fixed (int* indexes = Selection.TileMapSelection.GetSelectedIndexes())
                fixed (byte* ptr = &editor.GetData()[Selection.StartAddress])
                fixed (Obj16Tile* src = Data)
                {
                    var dest = (Obj16Tile*)ptr + startIndex;
                    for (int i = Selection.NumTiles; --i >= 0;)
                        if (indexes[i] + startIndex < length)
                            dest[indexes[i]] = src[i];
                }
            }
        }

        ISelection ISelectionData.Selection => Selection;
        ISelectionData ISelectionData.Copy(ISelection selection)
        {
            return Copy((Obj16Selection)selection);
        }

        void ISelectionData.WriteToEditor(IEditor editor)
        {
            WriteToEditor((Obj16Editor)editor);
        }
    }
}
