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
    public enum CellState
    {
        Empty,
        Player1,
        Player2
    }

    public partial class ReversiBoardControl : UserControl
    {
        private const int GridSize = 8; // 8x8
        private const int CellSize = 75;
        private CellState[,] actualBoard = new CellState[GridSize, GridSize];
        private bool[,] visibleBoard = new bool[GridSize, GridSize];
        private CellState? firstPlayer = null; // 最初に置いたプレイヤー
        private CellState currentPlayer = CellState.Player1;

        public ReversiBoardControl()
        {
            this.DoubleBuffered = true;
            this.Width = CellSize * GridSize + 20;
            this.Height = CellSize * GridSize + 20;
            InitializeBoards();
        }

        private void InitializeBoards()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    actualBoard[row, col] = CellState.Empty;
                    visibleBoard[row, col] = false;
                }
            }

            // 初期状態：石だけは置いてある（見た目のみ）
            visibleBoard[3, 3] = true;
            visibleBoard[3, 4] = true;
            visibleBoard[4, 3] = true;
            visibleBoard[4, 4] = true;

            // ただし、誰の石か（actualBoard）は未定
        }

        // プレイヤーが初めて石を置いたときに、firstPlayer を確定
        private void RegisterFirstPlayerMove(int row, int col)
        {
            if (firstPlayer == null)
            {
                firstPlayer = currentPlayer;

                // 最初の置き石と、それに関連する裏返しの場所を
                // actualBoard に記録する処理（後で追加）

                // 例：
                actualBoard[row, col] = currentPlayer;
                // → 裏返した箇所もここで記録する必要あり
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // 背景とグリッド線
            using (Pen gridPen = new Pen(Color.Black, 1))
            using (Brush cellBrush = new SolidBrush(Color.LightGreen))
            {
                for (int row = 0; row < GridSize; row++)
                {
                    for (int col = 0; col < GridSize; col++)
                    {
                        int x = col * CellSize;
                        int y = row * CellSize;

                        g.FillRectangle(cellBrush, x, y, CellSize, CellSize);
                        g.DrawRectangle(gridPen, x, y, CellSize, CellSize);
                    }
                }
            }

            // 石を描画（visibleBoardに基づく）
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    if (visibleBoard[row, col])
                    {
                        DrawStone(g, row, col, Brushes.White);
                    }
                }
            }

            using (Pen borderPen = new Pen(Color.Black, 3)) // 線の太さ3
            {
                int size = GridSize * CellSize;
                g.DrawRectangle(borderPen, 0, 0, size, size);
            }

            DrawStarPoints(g);
        }

        private void DrawStarPoints(Graphics g)
        {
            int[,] stars = new int[,]
            {
        {2, 2},
        {2, 6},
        {4, 4},
        {6, 2},
        {6, 6}
            };

            int radius = 5;
            for (int i = 0; i < stars.GetLength(0); i++)
            {
                int row = stars[i, 0];
                int col = stars[i, 1];
                int x = col * CellSize;
                int y = row * CellSize;
                g.FillEllipse(Brushes.Black, x - radius, y - radius, radius * 2, radius * 2);
            }
        }

        private void DrawStone(Graphics g, int row, int col, Brush brush)
        {
            int margin = 10;
            int x = col * CellSize + margin;
            int y = row * CellSize + margin;
            int size = CellSize - margin * 2;
            g.FillEllipse(brush, x, y, size, size);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int col = e.X / CellSize;
            int row = e.Y / CellSize;

            if (col < 0 || col >= GridSize || row < 0 || row >= GridSize)
                return;

            if (visibleBoard[row, col])
                return; // すでに石があるなら無視

            visibleBoard[row, col] = true;

            // 最初の一手なら、記録を開始する
            if (firstPlayer == null)
            {
                firstPlayer = currentPlayer;
                actualBoard[row, col] = currentPlayer;

                // ★あとで：裏返した場所も actualBoard に記録する
            }

            this.Invalidate(); // 再描画
        }

    }
}
