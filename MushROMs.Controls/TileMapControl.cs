using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Timers;
using Timer = System.Timers.Timer;
using Helper;

namespace MushROMs.Controls
{
    public abstract class TileMapControl : DesignControl
    {
        private const float FallbackAnimationTime = 300;

        private static readonly Size FallbackZoomSize = new Size(0x10, 0x10);
        private static readonly Size FallbackViewSize = new Size(0x10, 0x10);

        private static readonly DashedPenPair FallbackSelectionPens =
            new DashedPenPair(SystemColors.Highlight, SystemColors.HighlightText, 4, 4);
        private static readonly DashedPenPair FallbackActiveTilePens =
            new DashedPenPair(SystemColors.Highlight, SystemColors.HighlightText, 2, 2);

        private int _dashOffset;

        private DashedPenPair _selectionPens;
        private DashedPenPair _activeTilePens;

        private TileMap _tileMap;

        private ScrollBar _vScrollBar;
        private ScrollBar _hScrollBar;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal TileMapResizeMode TileMapResizeMode
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TileMap TileMap
        {
            get => _tileMap;
            protected set
            {
                if (TileMap == value)
                    return;

                if (TileMap != null)
                {
                    TileMap.TileSizeChanged -= TileMap_CellSizeChanged;
                    TileMap.ZoomSizeChanged -= TileMap_CellSizeChanged;
                    TileMap.ViewSizeChanged -= TileMap_ViewSizeChanged;
                    TileMap.GridSizeChanged -= TileMap_GridLengthChanged;
                    TileMap.ZeroTileChanged -= TileMap_ZeroIndexChanged;
                    TileMap.ActiveGridTileChanged -= TileMap_ActiveGridTileChanged;
                }

                _tileMap = value;

                if (TileMap != null)
                {
                    TileMap.TileSizeChanged += TileMap_CellSizeChanged;
                    TileMap.ZoomSizeChanged += TileMap_CellSizeChanged;
                    TileMap.ViewSizeChanged += TileMap_ViewSizeChanged;
                    TileMap.GridSizeChanged += TileMap_GridLengthChanged;
                    TileMap.ZeroTileChanged += TileMap_ZeroIndexChanged;
                    TileMap.ActiveGridTileChanged += TileMap_ActiveGridTileChanged;
                }

                ResetScrollBars();
            }
        }

        private void TileMap_ActiveGridTileChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        public ScrollBar VerticalScrollBar
        {
            get => _vScrollBar;
            set
            {
                if (VerticalScrollBar == value)
                    return;

                if (VerticalScrollBar != null)
                {
                    VerticalScrollBar.Scroll -= VerticalScrollBar_Scroll;
                    VerticalScrollBar.ValueChanged -= VerticalScrollBar_ValueChanged;
                }

                _vScrollBar = value;

                if (VerticalScrollBar != null)
                {
                    VerticalScrollBar.Scroll += VerticalScrollBar_Scroll;
                    VerticalScrollBar.ValueChanged += VerticalScrollBar_ValueChanged;
                }

                ResetVerticalScrollBar();
            }
        }

        public ScrollBar HorizontalScrollBar
        {
            get => _hScrollBar;
            set
            {
                if (HorizontalScrollBar == value)
                    return;

                if (HorizontalScrollBar != null)
                {
                    HorizontalScrollBar.Scroll -= HorizontalScrollBar_Scroll;
                    HorizontalScrollBar.ValueChanged -= HorizontalScrollBar_ValueChanged;
                }

                _hScrollBar = value;

                if (HorizontalScrollBar != null)
                {
                    HorizontalScrollBar.Scroll += HorizontalScrollBar_Scroll;
                    HorizontalScrollBar.ValueChanged += HorizontalScrollBar_ValueChanged;
                }

                ResetHorizontalScrollBar();
            }
        }

        private IContainer Components
        {
            get;
            set;
        }

        private Timer Timer
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DashedPenPair SelectionPens
        {
            get => _selectionPens;
            set
            {
                _selectionPens = value;
                Invalidate();
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DashedPenPair ActiveTilePens
        {
            get => _activeTilePens;
            set
            {
                _activeTilePens = value;
                Invalidate();
            }
        }

        private int DashOffset
        {
            get => _dashOffset;
            set
            {
                _dashOffset = value;
                Invalidate();
            }
        }

        protected TileMapControl()
        {
            SelectionPens = FallbackSelectionPens;
            ActiveTilePens = FallbackActiveTilePens;

            Components = new Container();

            Timer = new Timer()
            {
                Interval = FallbackAnimationTime
            };
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();

            Components.Add(Timer);
        }

        protected override void SetClientSizeCore(int x, int y)
        {
            if (TileMapResizeMode == TileMapResizeMode.ControlResize)
                return;

            if (TileMapResizeMode == TileMapResizeMode.None)
                TileMapResizeMode = TileMapResizeMode.ControlResize;

            base.SetClientSizeCore(x, y);

            if (TileMapResizeMode == TileMapResizeMode.ControlResize)
                TileMapResizeMode = TileMapResizeMode.None;
        }

        public void ResetScrollBars()
        {
            ResetVerticalScrollBar();
            ResetHorizontalScrollBar();
        }
        protected abstract void ResetHorizontalScrollBar();
        protected abstract void ResetVerticalScrollBar();
        protected abstract void AdjustScrollBarPositions();

        protected abstract void ScrollTileMapVertical(int value);
        protected abstract void ScrollTileMapHorizontal(int value);

        private void SetClientSizeFromTileMap()
        {
            if ((Size)TileMap.Size == ClientSize || TileMapResizeMode == TileMapResizeMode.TileMapCellResize)
                return;

            if (TileMapResizeMode == TileMapResizeMode.None)
                TileMapResizeMode = TileMapResizeMode.TileMapCellResize;
            
            SetClientSizeCore(TileMap.Width, TileMap.Height);

            if (TileMapResizeMode == TileMapResizeMode.TileMapCellResize)
                TileMapResizeMode = TileMapResizeMode.None;
        }

        public void DrawViewTilePath(GraphicsPath path, Position tile, Padding padding)
        {
            if (TileMap == null)
                return;
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            path.Reset();

            var dot = tile * TileMap.CellSize;
            path.AddRectangle(new Rectangle(
                dot.X + padding.Left,
                dot.Y + padding.Top,
                TileMap.CellWidth - 1 - padding.Horizontal,
                TileMap.CellHeight - 1 - padding.Vertical));
        }

        public abstract void GenerateSelectionPath(GraphicsPath path);


        protected override void OnPaint(PaintEventArgs e)
        {
            if (TileMap != null)
            {
                DrawEditorData(e);
                DrawActiveTileBorder(e);
                //if (TileMap.Selection != null)
                    DrawSelection(e);
            }
            base.OnPaint(e);
        }

        protected virtual void DrawEditorData(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            using (var bmp = new Bitmap(
                TileMap.Width, TileMap.Height, PixelFormat.Format32bppArgb))
            {
                var data = bmp.LockBits(
                    new Rectangle(Point.Empty, bmp.Size),
                    ImageLockMode.ReadWrite,
                    bmp.PixelFormat);

                DrawDataAsTileMap(data.Scan0, data.Height * data.Stride);

                bmp.UnlockBits(data);

                e.Graphics.DrawImageUnscaled(bmp, Point.Empty);
            }
        }

        protected virtual void DrawDataAsTileMap(IntPtr scan0, int length)
        {
            // Do nothing

            // We cannot mark the method as abstract because the Visual Studio designer will not
            // load the form. For the same reason, we cannot throw a NotImplementedException. So
            // we accept an empty method. Thankfully, this is non-breaking.
        }

        protected virtual void DrawActiveTileBorder(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            using (var path = new GraphicsPath())
            {
                using (Pen pen1 = new Pen(Color.Empty, 1),
                           pen2 = new Pen(Color.Empty, 1))
                {
                    DrawViewTilePath(
                        path, TileMap.ActiveViewTile, new Padding(2));

                    ActiveTilePens.SetPenProperties(pen1, pen2);
                    pen1.DashOffset += DashOffset;
                    pen2.DashOffset += DashOffset;

                    e.Graphics.DrawPath(pen1, path);
                    e.Graphics.DrawPath(pen2, path);
                }
            }
        }

        protected virtual void DrawSelection(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            using (var path = new GraphicsPath())
            {
                using (Pen pen1 = new Pen(Color.Empty, 1),
                           pen2 = new Pen(Color.Empty, 1))
                {
                    GenerateSelectionPath(path);

                    SelectionPens.SetPenProperties(pen1, pen2);
                    pen1.DashOffset -= DashOffset;
                    pen2.DashOffset -= DashOffset;

                    e.Graphics.DrawPath(pen1, path);
                    e.Graphics.DrawPath(pen2, path);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (TileMap != null)
                GetActiveTileFromMouse(e);
            base.OnMouseMove(e);
        }

        protected virtual void GetActiveTileFromMouse(MouseEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            if (!ClientRectangle.Contains(e.Location))
                return;

            if (!MouseHovering)
                TileMap.ActiveViewTile = (Position)e.Location / TileMap.CellSize;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (TileMap != null)
                GetActiveTileFromKeys(e);
            base.OnKeyDown(e);
        }

        protected virtual void GetActiveTileFromKeys(KeyEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            var active = TileMap.ActiveViewTile;
            switch (e.KeyCode)
            {
            case Keys.Left:
                active.X--;
                break;
            case Keys.Right:
                active.X++;
                break;
            case Keys.Up:
                active.Y--;
                break;
            case Keys.Down:
                active.Y++;
                break;
            }
            if (TileMap.ActiveViewTile != active)
                TileMap.ActiveViewTile = active;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Components != null)
                Components.Dispose();
            base.Dispose(disposing);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Enabled && Visible)
                DashOffset++;
        }

        private void TileMap_CellSizeChanged(object sender, EventArgs e)
        {
            SetClientSizeFromTileMap();
            Invalidate();
        }

        private void TileMap_ViewSizeChanged(object sender, EventArgs e)
        {
            SetClientSizeFromTileMap();
            ResetScrollBars();
            Invalidate();
        }

        private void TileMap_GridLengthChanged(object sender, EventArgs e)
        {
            ResetScrollBars();
            Invalidate();
        }

        private void TileMap_ZeroIndexChanged(object sender, EventArgs e)
        {
            AdjustScrollBarPositions();
            Invalidate();
        }

        private void HorizontalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                ScrollTileMapHorizontal(e.NewValue);
        }

        private void VerticalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                ScrollTileMapVertical(e.NewValue);
        }

        private void VerticalScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapVertical(VerticalScrollBar.Value);
        }

        private void HorizontalScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapHorizontal(HorizontalScrollBar.Value);
        }
    }
}