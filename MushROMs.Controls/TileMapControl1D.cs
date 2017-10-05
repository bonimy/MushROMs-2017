using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Helper;

namespace MushROMs.Controls
{
    public class TileMapControl1D : TileMapControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new TileMap1D TileMap
        {
            get => (TileMap1D)base.TileMap;
            protected set
            {
                if (TileMap == value)
                    return;

                if (TileMap != null)
                {
                    TileMap.SelectionInitialized -= TileMap_SelectionInitialized;
                    TileMap.SelectionChanged -= TileMap_SelectionChanged;
                    TileMap.SelectionCreated -= TileMap_SelectionCreated;
                }

                base.TileMap = value;

                if (TileMap != null)
                {
                    TileMap.SelectionInitialized += TileMap_SelectionInitialized;
                    TileMap.SelectionChanged += TileMap_SelectionChanged;
                    TileMap.SelectionCreated += TileMap_SelectionCreated;
                }
            }
        }

        private Position ZeroPosition
        {
            get
            {
                var zero = Position.Empty;
                if (HorizontalScrollBar != null)
                    zero.X = HorizontalScrollBar.Value;
                if (VerticalScrollBar != null)
                    zero.Y = VerticalScrollBar.Value;
                return zero;
            }
        }

        private ITileMapSelection1D InitialSelection
        {
            get;
            set;
        }

        private int FirstSelectedIndex => TileMap.GetGridTile(FirstViewPoint); private Position FirstViewPoint
        {
            get => GetViewPoint(FirstAbsolutePoint);
            set => FirstAbsolutePoint = GetAbsolutePoint(value);
        }
        private Position FirstAbsolutePoint
        {
            get;
            set;
        }

        private int SecondSelectedIndex => TileMap.GetGridTile(SecondViewPoint); private Position SecondViewPoint
        {
            get => GetViewPoint(SecondAbsolutePoint);
            set => SecondAbsolutePoint = GetAbsolutePoint(value);
        }
        private Position SecondAbsolutePoint
        {
            get;
            set;
        }

        protected virtual bool CreateAndSelection => false; protected virtual bool CreateOrSelection => ControlKeyHeld; protected virtual bool CreateXorSelection => false; protected virtual bool CreateNegatedSelection => AltKeyHeld; protected virtual bool CreateGateSelection => CreateAndSelection || CreateOrSelection || CreateXorSelection || CreateNegatedSelection;
        public override void GenerateSelectionPath(GraphicsPath path)
        {
            if (TileMap == null)
                return;

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var selection = TileMap.Selection;

            path.Reset();

            for (int y = TileMap.ViewHeight; --y >= 0; )
            {
                for (int x = TileMap.ViewWidth; --x >= 0; )
                {
                    var index = TileMap.GetGridTile(new Point(x, y));
                    if (selection.ContainsIndex(index) && TileMap.TileIsInGrid(index))
                    {
                        var edges = new Point[]
                        {
                            new Point(x - 1, y),
                            new Point(x, y - 1),
                            new Point(x + 1, y),
                            new Point(x, y + 1)
                        };
                        var clips = new int[]
                        {
                            x * TileMap.CellWidth,
                            y * TileMap.CellHeight,
                            ((x + 1) * TileMap.CellWidth) - 1,
                            ((y + 1) * TileMap.CellHeight) - 1
                        };
                        var corners = new Point[4];
                        corners[0] = new Point(clips[0], clips[1]);
                        corners[1] = new Point(clips[2], clips[1]);
                        corners[2] = new Point(clips[2], clips[3]);
                        corners[3] = new Point(clips[0], clips[3]);

                        for (int i = edges.Length; --i >= 0; )
                        {
                            var index2 = TileMap.GetGridTile(edges[i]);
                            if (!selection.ContainsIndex(index2) || !TileMap.TileIsInGrid(index2))
                            {
                                path.StartFigure();
                                path.AddLine(corners[((i - 1) & 3)], corners[i]);
                            }
                        }
                    }
                }
            }
        }

        protected override void ResetHorizontalScrollBar()
        {
            if (TileMap == null)
                return;

            if (HorizontalScrollBar != null)
            {
                if (HorizontalScrollBar.Enabled = TileMap.ViewWidth > 1)
                {
                    HorizontalScrollBar.SmallChange = 1;
                    HorizontalScrollBar.LargeChange = TileMap.ViewWidth - 1;
                    HorizontalScrollBar.Minimum = 0;
                    HorizontalScrollBar.Maximum = ((TileMap.ViewWidth - 1) * 2) - 1;
                    HorizontalScrollBar.Value = TileMap.ZeroTile % TileMap.ViewWidth;
                }
            }
        }

        protected override void ResetVerticalScrollBar()
        {
            if (TileMap == null)
                return;

            if (VerticalScrollBar != null)
            {
                var rows = (TileMap.GridSize / TileMap.ViewWidth);
                var enabled = rows > TileMap.ViewHeight;

                if (enabled)
                {
                    VerticalScrollBar.Enabled = true;
                    VerticalScrollBar.Minimum = 0;
                    VerticalScrollBar.Maximum = rows - 1;
                    VerticalScrollBar.SmallChange = 1;
                    VerticalScrollBar.LargeChange = TileMap.ViewHeight;

                    var value = TileMap.ZeroTile / TileMap.ViewWidth;
                    if (rows <= value + TileMap.ViewHeight)
                        value = rows - TileMap.ViewHeight;
                    VerticalScrollBar.Value = value;
                }
                else
                {
                    VerticalScrollBar.Value = 0;
                    VerticalScrollBar.Enabled = false;
                }
            }
        }

        protected override void AdjustScrollBarPositions()
        {
            HorizontalScrollBar.Value = TileMap.ZeroTile % TileMap.ViewWidth;
            VerticalScrollBar.Value = TileMap.ZeroTile / TileMap.ViewWidth;
        }

        protected override void ScrollTileMapHorizontal(int value)
        {
            var zeroY = TileMap.ZeroTile / TileMap.ViewWidth;
            TileMap.ZeroTile = value + (zeroY * TileMap.ViewHeight);
        }

        protected override void ScrollTileMapVertical(int value)
        {
            var zeroX = TileMap.ZeroTile % TileMap.ViewWidth;
            TileMap.ZeroTile = (value * TileMap.ViewWidth) + zeroX;
        }

        protected virtual bool BeginMouseSelection(MouseEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            return e.Button == MouseButtons.Left;
        }

        protected virtual bool EndMouseSelection(MouseEventArgs e)
        {
            return BeginMouseSelection(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (TileMap != null)
            {
                if (BeginMouseSelection(e))
                    InitializeSelectionMouseDown(e);
            }

            base.OnMouseDown(e);
        }

        protected virtual void InitializeSelectionMouseDown(MouseEventArgs e)
        {
            if (TileMap == null)
                return;

            if (e == null)
                throw new ArgumentNullException(nameof(e));

            var tile = TileMap.GetViewTileFromScreenDot(e.Location, true);
            FirstViewPoint = tile;
            if (CreateGateSelection)
                InitialSelection = TileMap.Selection;
            else
                InitialSelection = Selection1D.Empty;
            TileMap.InitializeSelection(InitialSelection);
            GetSelectionMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (TileMap != null)
            {
                if (EndMouseSelection(e) && TileMap.Selecting)
                    CreateSelectionMouseUp(e);
            }

            base.OnMouseUp(e);
        }

        protected virtual void CreateSelectionMouseUp(MouseEventArgs e)
        {
            if (TileMap == null)
                return;

            GetSelectionMouseMove(e);
            TileMap.CreateSelection(TileMap.Selection);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (TileMap != null)
            {
                if (TileMap.Selecting)
                    GetSelectionMouseMove(e);
            }
            base.OnMouseMove(e);
        }

        protected virtual void GetSelectionMouseMove(MouseEventArgs e)
        {
            if (TileMap == null)
                return;

            if (e == null)
                throw new ArgumentNullException(nameof(e));

            ModifySelection(TileMap.GetViewTileFromScreenDot(e.Location, true));
        }

        protected virtual void ModifySelection(Position view)
        {
            var max = Point.Empty;
            var min = Point.Empty;

            if (HorizontalScrollBar != null)
            {
                max.X = HorizontalScrollBar.Maximum;
                min.X = HorizontalScrollBar.Minimum;
            }
            if (VerticalScrollBar != null)
            {
                max.Y = VerticalScrollBar.Maximum;
                min.Y = VerticalScrollBar.Minimum;
            }

            SecondViewPoint = view;

            var x2 = Math.Max(SecondAbsolutePoint.X, 0);
            var y2 = Math.Max(SecondAbsolutePoint.Y, 0);

            x2 = Math.Min(SecondAbsolutePoint.X, max.X + 1);
            y2 = Math.Min(SecondAbsolutePoint.Y, max.Y);

            SecondAbsolutePoint = new Position(x2, y2);

            Range range = SecondAbsolutePoint - FirstAbsolutePoint;
            
            if (SecondViewPoint.X < 0 && -range.Horizontal < TileMap.ViewWidth)
            {
                HorizontalScrollBar.Value = Math.Max(SecondAbsolutePoint.X, 0);
            }
            else if (SecondViewPoint.X >= TileMap.ViewWidth && range.Horizontal < TileMap.ViewWidth)
            {
                HorizontalScrollBar.Value = Math.Min(SecondAbsolutePoint.X - TileMap.ViewWidth + 1, max.X);
            }

            if (SecondViewPoint.Y < 0 && -range.Vertical < TileMap.ViewHeight)
            {
                VerticalScrollBar.Value = Math.Max(SecondViewPoint.Y, 0);
            }
            else if (SecondViewPoint.Y >= TileMap.ViewHeight && range.Vertical < TileMap.ViewWidth)
            {
                VerticalScrollBar.Value = Math.Min(SecondAbsolutePoint.Y - TileMap.ViewHeight + 1, max.Y - TileMap.ViewHeight + 1);
            }

            if (SecondViewPoint.X < 0)
                SecondViewPoint = new Position(0, SecondViewPoint.Y);
            if (SecondViewPoint.X >= TileMap.ViewWidth)
                SecondViewPoint = new Position(TileMap.ViewWidth - 1, SecondViewPoint.Y);

            if (SecondViewPoint.Y < 0)
                SecondViewPoint = new Position(SecondViewPoint.X, 0);
            if (SecondViewPoint.Y >= TileMap.ViewHeight)
                SecondViewPoint = new Position(SecondViewPoint.X, TileMap.ViewHeight - 1);
            
            if (SecondSelectedIndex == FirstSelectedIndex)
                ModifySelection(new SingleSelection1D(FirstSelectedIndex));
            else if (ShiftKeyHeld)
                ModifySelection(new LineSelection1D(FirstSelectedIndex, SecondSelectedIndex));
            else
                ModifySelection(new BoxSelection1D(TileMap.ViewWidth, TileMap.ZeroTile, FirstViewPoint, SecondViewPoint));
        }

        private void ModifySelection(ITileMapSelection1D selection)
        {
            if (selection == null)
                throw new ArgumentNullException(nameof(selection));

            if (CreateAndSelection)
                TileMap.Selection = (ITileMapSelection1D)InitialSelection.LogicalAnd(selection);
            else if (CreateOrSelection)
                TileMap.Selection = (ITileMapSelection1D)InitialSelection.LogicalOr(selection);
            else if (CreateNegatedSelection)
                TileMap.Selection = (ITileMapSelection1D)InitialSelection.LogicalNegate(selection);
            else if (CreateXorSelection)
                TileMap.Selection = (ITileMapSelection1D)InitialSelection.LogicalXor(selection);
            else
                TileMap.Selection = selection;
        }

        private Position GetAbsolutePoint(Position view)
        {
            return view + ZeroPosition;
        }

        private Position GetViewPoint(Position absolute)
        {
            return absolute - ZeroPosition;
        }

        private int GetGridTileFromAbsolutePoint(Position absolute)
        {
            return TileMap.GetGridTile(GetViewPoint(absolute));
        }

        private void TileMap_SelectionInitialized(object sender, EventArgs e)
        {
            InitialSelection = TileMap.Selection;
            Invalidate();
        }

        private void TileMap_SelectionChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void TileMap_SelectionCreated(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}