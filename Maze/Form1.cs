using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze
{
    public partial class Form1 : Form
    {
        Board maze = new Board();
        List<Direction> attemptSteps = new List<Direction>();
        Graphics graphics;
        Tile currentTile;

        public Form1()
        {
            InitializeComponent();

            this.Width = maze.Width * 60;
            this.Height = maze.Height * 60;

            this.button1.Location = new Point(this.button1.Location.X, this.button1.Location.Y + (maze.Height * 15));
            this.button2.Location = new Point(this.button2.Location.X, this.button2.Location.Y + (maze.Height * 15));

            currentTile = maze.StartTile;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    DrawMove(currentTile, Direction.Left);
                    return true;

                case Keys.Right:
                    DrawMove(currentTile, Direction.Right);
                    return true;

                case Keys.Up:
                    DrawMove(currentTile, Direction.Up);
                    return true;

                case Keys.Down:
                    DrawMove(currentTile, Direction.Down);
                    return true;

                default:
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            graphics = e.Graphics;

            DrawBoard();

            graphics = this.CreateGraphics();
        }

        private void DrawBoard()
        {
            Rectangle board = new Rectangle(20, 20, maze.Width * 50, maze.Height * 50);
            Rectangle outline = new Rectangle(board.Left - 1, board.Top - 1, board.Width + 2, board.Height + 2);

            graphics.DrawRectangle(Pens.Black, outline);
            graphics.FillRectangle(Brushes.White, board);

            foreach (Tile t in maze.Tiles)
            {
                DrawTile(t);
            }

            Rectangle startRect = new Rectangle(maze.StartTile.DrawX + 25, maze.StartTile.DrawY + 25, 5, 5);
            Rectangle endRect = new Rectangle(maze.EndTile.DrawX + 25, maze.EndTile.DrawY + 25, 5, 5);

            graphics.DrawEllipse(new Pen(Color.Green, 5), startRect);
            graphics.DrawEllipse(new Pen(Color.Red, 5), endRect);
        }

        private void DrawTile(Tile tile)
        {
            Point topLeft = new Point(tile.DrawX, tile.DrawY);
            Point topRight = new Point(tile.DrawX + 50, tile.DrawY);
            Point bottomLeft = new Point(tile.DrawX, tile.DrawY + 50);
            Point bottomRight = new Point(tile.DrawX + 50, tile.DrawY + 50);

            if (tile.TopBorder && tile.Y > 0)
            {
                graphics.DrawLine(Pens.Black, topLeft, topRight);
            }

            if (tile.BottomBorder && tile.Y < maze.Height - 1)
            {
                graphics.DrawLine(Pens.Black, bottomLeft, bottomRight);
            }

            if (tile.LeftBorder && tile.X > 0)
            {
                graphics.DrawLine(Pens.Black, topLeft, bottomLeft);
            }

            if (tile.RightBorder && tile.X < maze.Width - 1)
            {
                graphics.DrawLine(Pens.Black, topRight, bottomRight);
            }
        }

        private void DrawAttempt(List<Direction> directions)
        {
            Point start = new Point(maze.StartTile.DrawX + 25, maze.StartTile.DrawY + 25);
            Point previousPoint = start;

            foreach(Direction d in directions)
            {
                Point nextPoint = new Point();

                switch (d)
                {
                    case Direction.Up:
                        nextPoint = new Point(previousPoint.X, previousPoint.Y - 50);
                        break;

                    case Direction.Down:
                        nextPoint = new Point(previousPoint.X, previousPoint.Y + 50);
                        break;

                    case Direction.Left:
                        nextPoint = new Point(previousPoint.X - 50, previousPoint.Y);
                        break;

                    case Direction.Right:
                        nextPoint = new Point(previousPoint.X + 50, previousPoint.Y);
                        break;
                }

                graphics.DrawLine(Pens.Blue, previousPoint, nextPoint);

                previousPoint = nextPoint;
            }
        }

        private void DrawMove(Tile previousTile, Direction direction)
        {
            Point start = new Point(previousTile.DrawX + 25, previousTile.DrawY + 25);
            Point previousPoint = start;
            Point nextPoint = previousPoint;

            switch (direction)
            {
                case Direction.Up:
                    if (maze.Tiles.Any(t => t.X == previousTile.X && t.Y == previousTile.Y - 1))
                    {
                        if (!previousTile.TopBorder && !maze.Tiles.Single(t => t.X == previousTile.X && t.Y == previousTile.Y - 1).BottomBorder)
                        {
                            nextPoint = new Point(previousPoint.X, previousPoint.Y - 50);
                            currentTile = maze.Tiles.Single(t => t.X == previousTile.X && t.Y == previousTile.Y - 1);
                        }
                    }
                    break;

                case Direction.Down:
                    if (maze.Tiles.Any(t => t.X == previousTile.X && t.Y == previousTile.Y + 1))
                    {
                        if (!previousTile.BottomBorder && !maze.Tiles.Single(t => t.X == previousTile.X && t.Y == previousTile.Y + 1).TopBorder)
                        {
                            nextPoint = new Point(previousPoint.X, previousPoint.Y + 50);
                            currentTile = maze.Tiles.Single(t => t.X == previousTile.X && t.Y == previousTile.Y + 1);
                        }
                    }
                    break;

                case Direction.Left:
                    if (maze.Tiles.Any(t => t.X == previousTile.X - 1 && t.Y == previousTile.Y))
                    {
                        if (!previousTile.LeftBorder && !maze.Tiles.Single(t => t.X == previousTile.X - 1 && t.Y == previousTile.Y).RightBorder)
                        {
                            nextPoint = new Point(previousPoint.X - 50, previousPoint.Y);
                            currentTile = maze.Tiles.Single(t => t.X == previousTile.X - 1 && t.Y == previousTile.Y);
                        }
                    }
                    break;

                case Direction.Right:
                    if (maze.Tiles.Any(t => t.X == previousTile.X + 1 && t.Y == previousTile.Y))
                    {
                        if (!previousTile.RightBorder && !maze.Tiles.Single(t => t.X == previousTile.X + 1 && t.Y == previousTile.Y).LeftBorder)
                        {
                            nextPoint = new Point(previousPoint.X + 50, previousPoint.Y);
                            currentTile = maze.Tiles.Single(t => t.X == previousTile.X + 1 && t.Y == previousTile.Y);
                        }
                    }
                    break;
            }

            if (nextPoint != previousPoint)
            {
                graphics.DrawLine(Pens.Green, previousPoint, nextPoint);
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            graphics.Clear(Form1.ActiveForm.BackColor);

            maze = new Board();
            currentTile = maze.StartTile;

            DrawBoard();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            attemptSteps = maze.Run();
            DrawAttempt(attemptSteps);
        }
    }
}
