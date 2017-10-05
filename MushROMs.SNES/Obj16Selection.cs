using System;

namespace MushROMs.SNES
{
    public class Obj16Selection : ISelection
    {
        public ITileMapSelection1D TileMapSelection
        {
            get;
            private set;
        }

        public int StartAddress
        {
            get;
            private set;
        }
        public int StartIndex => TileMapSelection.StartIndex; public int NumTiles => TileMapSelection.Size;
        public Obj16Selection(int startAddress, ITileMapSelection1D selection)
        {
            StartAddress = startAddress;
            TileMapSelection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public Obj16Data GetEditorData(Obj16Editor editor)
        {
            return new Obj16Data(editor, this);
        }

        ISelectionData ISelection.GetSelectionData(IEditor editor)
        {
            return GetEditorData((Obj16Editor)editor);
        }
    }
}
