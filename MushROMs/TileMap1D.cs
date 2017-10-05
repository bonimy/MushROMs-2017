using System;
using Helper;

namespace MushROMs
{
    public sealed class TileMap1D : TileMap
    {
        private int _gridSize;
        private int _zeroTile;
        private int _activeGridTile;

        private ITileMapSelection1D _selection;

        public int GridSize
        {
            get => _gridSize;
            set
            {
                if (GridSize == value)
                    return;

                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        SR.ErrorLowerBoundInclusive(nameof(value), value, 0));

                _gridSize = value;
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        public int ZeroTile
        {
            get => _zeroTile;
            set
            {
                if (ZeroTile == value)
                    return;

                _zeroTile = value;
                OnZeroTileChanged(EventArgs.Empty);
            }
        }

        public int VisibleGridSpan => Math.Min(GridSize - ZeroTile, ViewSize.Area);
        public int ActiveGridTile
        {
            get => _activeGridTile;
            set
            {
                if (ActiveGridTile == value)
                    return;

                _activeGridTile = value;
                OnActiveGridTileChanged(EventArgs.Empty);
            }
        }

        public override Position ActiveViewTile
        {
            get => GetViewTile(ActiveGridTile);
            set => ActiveGridTile = GetGridTile(value);
        }

        public ITileMapSelection1D Selection
        {
            get => _selection;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (Selection == value)
                    return;

                _selection = value;
                OnSelectionChanged(EventArgs.Empty);
            }
        }

        public TileMap1D()
        {
            _selection = new EmptySelection1D();
        }

        public bool TileIsInGrid(int tile)
        {
            return tile >= 0 && tile < GridSize;
        }

        public void InitializeSelection(ITileMapSelection1D selection)
        {
            Selection = selection ?? throw new ArgumentNullException(nameof(selection));
            OnSelectionInitialized(EventArgs.Empty);
        }
        public void CreateSelection(ITileMapSelection1D selection)
        {
            Selection = selection ?? throw new ArgumentNullException(nameof(selection));
            OnSelectionCreated(EventArgs.Empty);
        }

        public int GetViewTileX(int gridTile)
        {
            return GetViewTileX(gridTile, ViewWidth, ZeroTile);
        }
        public int GetViewTileY(int gridTile)
        {
            return GetViewTileY(gridTile, ViewWidth, ZeroTile);
        }
        public Position GetViewTile(int gridTile)
        {
            return GetViewTile(gridTile, ViewWidth, ZeroTile);
        }

        public static int GetViewTileX(int gridTile, int viewWidth)
        {
            return GetViewTileX(gridTile, viewWidth, 0);
        }
        public static int GetViewTileY(int gridTile, int viewWidth)
        {
            return GetViewTileY(gridTile, viewWidth, 0);
        }
        public static Position GetViewTile(int gridTile, int viewWidth)
        {
            return GetViewTile(gridTile, viewWidth, 0);
        }

        public static int GetViewTileX(int gridTile, int viewWidth, int zeroIndex)
        {
            if (viewWidth <= 0)
                throw new ArgumentOutOfRangeException(nameof(viewWidth),
                    SR.ErrorUpperBoundExclusive(nameof(viewWidth), viewWidth, 0));

            return (gridTile - zeroIndex) % viewWidth;
        }
        public static int GetViewTileY(int gridTile, int viewWidth, int zeroIndex)
        {
            if (viewWidth <= 0)
                throw new ArgumentOutOfRangeException(nameof(viewWidth),
                    SR.ErrorUpperBoundExclusive(nameof(viewWidth), viewWidth, 0));

            return (gridTile - zeroIndex) / viewWidth;
        }
        public static Position GetViewTile(int gridTile, int viewWidth, int zeroIndex)
        {
            return new Position(GetViewTileX(gridTile, viewWidth, zeroIndex),
                GetViewTileY(gridTile, viewWidth, zeroIndex));
        }

        public int GetGridTile(Position viewTile)
        {
            return GetGridTile(viewTile, ViewWidth, ZeroTile);
        }
        public int GetGridTile(int viewTileX, int viewTileY)
        {
            return GetGridTile(viewTileX, viewTileY, ViewWidth, ZeroTile);
        }

        public static int GetGridTile(Position viewTile, int viewWidth)
        {
            return GetGridTile(viewTile, viewWidth, 0);
        }
        public static int GetGridTile(int viewTileX, int viewTileY, int viewWidth)
        {
            return GetGridTile(viewTileX, viewTileY, viewWidth, 0);
        }

        public static int GetGridTile(Position viewTile, int viewWidth, int zeroIndex)
        {
            return GetGridTile(viewTile.X, viewTile.Y, viewWidth, zeroIndex);
        }
        public static int GetGridTile(int viewTileX, int viewTileY, int viewWidth, int zeroIndex)
        {
            if (viewWidth <= 0)
                throw new ArgumentOutOfRangeException(nameof(viewWidth),
                    SR.ErrorUpperBoundExclusive(nameof(viewWidth), viewWidth, 0));

            return (viewTileY * viewWidth) + viewTileX + zeroIndex;
        }
    }
}
