using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Helper;

namespace MushROMs.Controls
{
    public class TileMapControl2D : TileMapControl
    {
        public new TileMap2D TileMap
        {
            get => (TileMap2D)base.TileMap;
            set => base.TileMap = value;
        }

        protected override void ResetVerticalScrollBar()
        {
            throw new NotImplementedException(nameof(ResetVerticalScrollBar));
        }

        protected override void ResetHorizontalScrollBar()
        {
            throw new NotImplementedException(nameof(ResetHorizontalScrollBar));
        }

        protected override void AdjustScrollBarPositions()
        {
            throw new NotImplementedException(nameof(AdjustScrollBarPositions));
        }

        protected override void ScrollTileMapHorizontal(int value)
        {
            throw new NotImplementedException(nameof(ScrollTileMapHorizontal));
        }

        protected override void ScrollTileMapVertical(int value)
        {
            throw new NotImplementedException(nameof(ScrollTileMapVertical));
        }

        public override void GenerateSelectionPath(GraphicsPath path)
        {
            if (TileMap == null)
                return;

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var selection = TileMap.Selection;

            path.Reset();

            for (int y = TileMap.ViewHeight; --y >= 0;)
            {
                for (int x = TileMap.ViewWidth; --x >= 0;)
                {
                    var index = new Position(x, y);
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

                        for (int i = edges.Length; --i >= 0;)
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
    }
}
