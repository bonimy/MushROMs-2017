using System;
using Helper;

namespace MushROMs
{
    public abstract partial class TileMap
    {
        public static readonly Range FallbackTileSize = 8;
        public static readonly Range FallbackZoomSize = 2;
        public static readonly Range FallbackViewSize = new Range(0x10, 8);

        private Range _tileSize;
        private Range _zoomSize;
        private Range _viewSize;

        public event EventHandler GridSizeChanged;
        public event EventHandler ZeroTileChanged;
        public event EventHandler ActiveGridTileChanged;

        public event EventHandler TileSizeChanged;
        public event EventHandler ZoomSizeChanged;
        public event EventHandler ViewSizeChanged;

        public event EventHandler SelectionInitialized;
        public event EventHandler SelectionChanged;
        public event EventHandler SelectionCreated;

        public abstract Position ActiveViewTile
        {
            get;
            set;
        }

        public bool Selecting
        {
            get;
            private set;
        }

        public Range TileSize
        {
            get => _tileSize;
            set
            {
                if (TileSize == value)
                    return;

                _tileSize = value;
                OnTileSizeChanged(EventArgs.Empty);
            }
        }
        public int TileWidth
        {
            get => TileSize.Horizontal;
            set => TileSize = new Range(value, TileHeight);
        }
        public int TileHeight
        {
            get => TileSize.Vertical;
            set => TileSize = new Range(TileWidth, value);
        }

        public Range ZoomSize
        {
            get => _zoomSize;
            set
            {
                if (ZoomSize == value)
                    return;

                _zoomSize = value;
                OnZoomSizeChanged(EventArgs.Empty);
            }
        }
        public int ZoomWidth
        {
            get => ZoomSize.Horizontal;
            set => ZoomSize = new Range(value, ZoomHeight);
        }
        public int ZoomHeight
        {
            get => ZoomSize.Vertical;
            set => ZoomSize = new Range(ZoomWidth, value);
        }

        public Range ViewSize
        {
            get => _viewSize;
            set
            {
                if (ViewSize == value)
                    return;

                _viewSize = value;
                OnViewSizeChanged(EventArgs.Empty);
            }
        }
        public int ViewWidth
        {
            get => ViewSize.Horizontal;
            set => ViewSize = new Range(value, ViewHeight);
        }
        public int ViewHeight
        {
            get => ViewSize.Vertical;
            set => ViewSize = new Range(ViewWidth, value);
        }

        public Range CellSize => TileSize * ZoomSize; public int CellWidth => CellSize.Horizontal; public int CellHeight => CellSize.Vertical;
        public Range Size => CellSize * ViewSize; public int Width => Size.Horizontal; public int Height => Size.Vertical;
        protected TileMap()
        {
            _tileSize = FallbackTileSize;
            _zoomSize = FallbackZoomSize;
            _viewSize = FallbackViewSize;
        }

        public Position GetViewTileFromScreenDot(Position dot, bool zoom)
        {
            return dot / (zoom ? CellSize : TileSize);
        }

        public Position GetScreenDotFromViewTile(Position tile, bool zoom)
        {
            return tile * (zoom ? CellSize : TileSize);
        }

        public bool TileIsInViewRegion(Position tile)
        {
            return ViewSize.Contains(tile);
        }

        protected virtual void OnTileSizeChanged(EventArgs e)
        {
            TileSizeChanged?.Invoke(this, e);
        }
        protected virtual void OnZoomSizeChanged(EventArgs e)
        {
            ZoomSizeChanged?.Invoke(this, e);
        }
        protected virtual void OnViewSizeChanged(EventArgs e)
        {
            ViewSizeChanged?.Invoke(this, e);
        }

        protected virtual void OnGridSizeChanged(EventArgs e)
        {
            GridSizeChanged?.Invoke(this, e);
        }
        protected virtual void OnZeroTileChanged(EventArgs e)
        {
            ZeroTileChanged?.Invoke(this, e);
        }
        protected virtual void OnActiveGridTileChanged(EventArgs e)
        {
            ActiveGridTileChanged?.Invoke(this, e);
        }

        protected virtual void OnSelectionInitialized(EventArgs e)
        {
            Selecting = true;
            SelectionInitialized?.Invoke(this, e);
        }
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }
        protected virtual void OnSelectionCreated(EventArgs e)
        {
            Selecting = false;
            SelectionCreated?.Invoke(this, e);
        }
    }
}
