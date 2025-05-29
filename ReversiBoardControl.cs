using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryReversi
{
    public partial class ReversiBoardControl : UserControl
    {
        private const int GridSize = 8; // 8x8
        private const int CellSize = 75;
        private readonly Brush boardBrush = Brushes.LightGreen;
        private readonly Pen gridPen = Pens.White;

        public ReversiBoardControl()
        {
            this.DoubleBuffered = true;
            this.Width = CellSize * GridSize + 20;
            this.Height = CellSize * GridSize + 20;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            using (Pen blackPen = new Pen(Color.Black, 1)) // グリッドと外枠両用
            {
                // 背景 + 緑の円
                for (int row = 0; row < GridSize; row++)
                {
                    for (int col = 0; col < GridSize; col++)
                    {
                        // 緑の円
                        g.FillEllipse(Brushes.LightGreen, col * CellSize, row * CellSize, CellSize, CellSize);

                        // 黒い枠（各セル）
                        g.DrawRectangle(blackPen, col * CellSize, row * CellSize, CellSize, CellSize);
                    }
                }

                // 外枠（全体）
                int size = GridSize * CellSize;
                g.DrawRectangle(new Pen(Color.Black, 3), 0, 0, size, size);
            }

            // 中央4つに白石を描画
            DrawStone(g, 3, 3, Brushes.White);
            DrawStone(g, 3, 4, Brushes.White);
            DrawStone(g, 4, 3, Brushes.White);
            DrawStone(g, 4, 4, Brushes.White);

            using (Pen borderPen = new Pen(Color.Black, 3)) // 線の太さ3
            {
                int size = GridSize * CellSize;
                g.DrawRectangle(borderPen, 0, 0, size, size);
            }

            DrawStarPoints(g);
        }

        private void DrawStarPoints(Graphics g)
        {
            // グリッド交差点上の座標（行・列）
            int[,] starPositions = new int[,]
            {
        {2, 2},
        {2, 6},
        {4, 4},
        {6, 2},
        {6, 6}
            };

            int starRadius = 5;
            Brush starBrush = Brushes.Black;

            for (int i = 0; i < starPositions.GetLength(0); i++)
            {
                int row = starPositions[i, 0];
                int col = starPositions[i, 1];

                // セルの交差点の中央座標を計算（行と列の交点）
                int centerX = col * CellSize;
                int centerY = row * CellSize;

                g.FillEllipse(starBrush,
                    centerX - starRadius,
                    centerY - starRadius,
                    starRadius * 2,
                    starRadius * 2);
            }
        }


        private void DrawStone(Graphics g, int col, int row, Brush color)
        {
            int margin = 5;
            int x = col * CellSize + margin;
            int y = row * CellSize + margin;
            int size = CellSize - margin * 2;
            g.FillEllipse(color, x, y, size, size);
        }
    }
}
