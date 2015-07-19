using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChasingTheCursor
{
    public partial class Main : Form
    {
        Bitmap bmp = new Bitmap(700, 700);
        Graphics g;
        Point preCursor = new Point(0, 0);
        Point cursor = new Point(0, 0);
        Point ball = new Point(350, 350);
        Point block1 = new Point(130, 0);
        Point block2 = new Point(150, 120);
        List<Point> ListLine = new List<Point>();
        int step = 0;
        int radius = 15;
        int points = 0;
        int inc = 10;
        int level = 1;
        int sat = 0;

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = Graphics.FromImage(bmp); // Binds the graphics to the Bitmap
            pictureBox1.Image = bmp;
        }

        private void ChaseTimer_Tick(object sender, EventArgs e)
        {
            g = Graphics.FromImage(bmp); // Binds the graphics to the Bitmap
            g.Clear(Color.White);

            cursor = new Point(PointToClient(Cursor.Position).X, (PointToClient(Cursor.Position).Y) + 10);

            try
            {
                CollisionWithBlock();
                // Calculate "inc" value by the level
                if (points == (level * 100))
                {
                    level++;
                    inc = inc + 2;
                    block1 = RandomiseBlocks()[0];
                    block2 = RandomiseBlocks()[1];
                }

                // Calculate background
                if (points > 100)
                {
                    sat = (points - ((level - 1) * 100)) * 2;
                }
                else
                {
                    sat = points;
                }
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255 - (sat + 1), 255 - (sat + 1))), 0, 0, 700, 700 / 2);

                // Render Blocks
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 200)), block1.X, block1.Y, 100, 100);
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 200)), block2.X, block2.Y, 100, 100);

                // Calculate
                if (preCursor == cursor)
                {
                    step = step + inc; // Increament the step by the "inc" value
                }
                else
                {
                    IEnumerable<Point> Line = DrawingLine(ball.X, ball.Y, PointToClient(Cursor.Position).X, (PointToClient(Cursor.Position).Y) + 10); // Calculate a new Line
                    ListLine = Line.ToList<Point>(); // Convert it to a List, so you can get the points by "step"
                    step = inc;
                }

                // Set the ball to a new Point
                ball = new Point(ListLine.ElementAt(step).X, ListLine.ElementAt(step).Y);
                
                // If you want to see the line between the ball and the cursor
                // g.DrawLine(new Pen(Color.Blue, 3f), ball, cursor);

                preCursor = cursor; // Store this cursor, so the next Timer Tick it can be compared
                points++;
                label1.Text = "Points: " + points;
            }
            catch
            {
                CollisionWithBall();
            }

            // Draw
            g.FillEllipse(new SolidBrush(Color.Green), ball.X - radius, ball.Y - radius, radius * 2, radius * 2);

            g.Dispose();
            pictureBox1.Image = bmp;
        }

        private IEnumerable<Point> DrawingLine(int x, int y, int x2, int y2)
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                yield return new Point(x, y);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        private Point[] RandomiseBlocks()
        {
            Point[] points = new Point[2];
            Random rnd = new Random();

            for (int i = 0; i < points.Length; i++)
            {
                int x = rnd.Next(0, 200);
                int y = rnd.Next(0, 200);
                if (rnd.Next(0, 2) == 0)
                {
                    points[i] = new Point(x, y);
                }
                else
                {
                    points[i] = new Point(bmp.Width - x - 50, y);
                }
            }

            return points;
        }

        private void CollisionWithBlock()
        {
            Point[] blocks = new Point[2];
            blocks[0] = block1;
            blocks[1] = block2;

            foreach (Point p in blocks)
            {
                if (cursor.X > p.X && cursor.X < (p.X + 100))
                {
                    if (cursor.Y > p.Y && cursor.Y < (p.Y + 100))
                    {
                        GameOver();
                    }
                }
            }
        }

        private void CollisionWithBall()
        {
            if (Math.Abs(ball.X - cursor.X) < 30 || Math.Abs(ball.Y - cursor.Y) < 30)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            ChaseTimer.Stop();
            DialogResult result = MessageBox.Show("Game Over!" + Environment.NewLine + "You have " + points + " points!" + Environment.NewLine + "Do you want to restart?", "Chasing The Cursor!", MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                step = 0;
                points = 0;
                ball = new Point(350, 350);
            }
            else
            {
                Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            preCursor = new Point(350, 350);
            cursor = new Point(PointToClient(Cursor.Position).X, (PointToClient(Cursor.Position).Y) + 10);
            IEnumerable<Point> Line = DrawingLine(ball.X, ball.Y, PointToClient(Cursor.Position).X, (PointToClient(Cursor.Position).Y) + 10);
            ChaseTimer.Start();
        }
    }
}
