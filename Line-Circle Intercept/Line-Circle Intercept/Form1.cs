using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Line_Circle_Intercept
{
    public partial class Form1 : Form
    {
        Graphics g; // Graphics
        Bitmap bmp = new Bitmap(400, 400); // Picture
        int rectX = 0;
        int rectY = 0;
        int radiusCircle = 1;
        int dots = 0; // Number of dots
        Point dot1 = new Point(0, 0); // The first dot point
        Point dot2 = new Point(0, 0); // The first dot point

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) // Form Load Function
        {
            g = Graphics.FromImage(bmp); // Binds the graphics to the Bitmap
            g.Clear(Color.White);
            pictureBox1.Image = bmp;
        }

        private void button1_Click(object sender, EventArgs e) // Render Circle Button Click Function
        {
            rectX = Convert.ToInt16(cx.Value - radius.Value);
            rectY = Convert.ToInt16(cy.Value - radius.Value);
            radiusCircle = Convert.ToInt16(radius.Value);
            g.Clear(Color.White);
            g.FillEllipse(new SolidBrush(Color.CadetBlue), rectX, rectY, radiusCircle * 2, radiusCircle * 2);
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_Click(object sender, EventArgs e) // When you click on the picture box
        {
            if (dots != 1)
            {
                dots++;
                dot1 = new Point(PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y);
                g.FillEllipse(new SolidBrush(Color.Black), dot1.X - 3, dot1.Y - 3, 6, 6); // Draw a dot where you have clicked
            }
            else
            {
                // When there is the information for both of the dots [THE HEART]
                dot2 = new Point(PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y);
                g.FillEllipse(new SolidBrush(Color.Black), dot2.X - 3, dot2.Y - 3, 6, 6); // Draw a dot where you have clicked
                IEnumerable<Point> Line = DrawingLine(dot1.X, dot1.Y, dot2.X, dot2.Y);
                for (int i = 0; i < Line.Count(); i++)
                {
                    if (Line.ElementAt(i).X >= rectX && Line.ElementAt(i).X <= rectX + radiusCircle * 2 && Line.ElementAt(i).Y >= rectY && Line.ElementAt(i).Y <= rectY + radiusCircle * 2)
                    {
                        int diffX = rectX + radiusCircle - Line.ElementAt(i).X;
                        int diffY = rectY + radiusCircle - Line.ElementAt(i).Y;
                        int result = Convert.ToInt16(Math.Sqrt(Math.Pow(Math.Abs(diffX), 2) + Math.Pow(Math.Abs(diffY), 2)));
                        if (checkBox1.Checked == true)
                        {
                            if (result == radiusCircle)
                            {
                                bmp.SetPixel(Line.ElementAt(i).X, Line.ElementAt(i).Y, Color.Purple); // INTERCEPT RIGHT ON THE OUTLINE OF THE CIRCLE
                            }
                            else if (result < radiusCircle)
                            {
                                bmp.SetPixel(Line.ElementAt(i).X, Line.ElementAt(i).Y, Color.Red); // INTERCEPT
                            }
                            else { bmp.SetPixel(Line.ElementAt(i).X, Line.ElementAt(i).Y, Color.Green); } // NO INTERCEPT
                        }
                        else
                        {
                            if (result <= radiusCircle)
                            {
                                bmp.SetPixel(Line.ElementAt(i).X, Line.ElementAt(i).Y, Color.Red); // INTERCEPT
                            }
                            else { bmp.SetPixel(Line.ElementAt(i).X, Line.ElementAt(i).Y, Color.Green); } // NO INTERCEPT
                        }
                    }
                    else { bmp.SetPixel(Line.ElementAt(i).X, Line.ElementAt(i).Y, Color.Green); } // NO INTERCEPT
                }

                dots = 0; // Clear out the dots stat
            }
            
            pictureBox1.Image = bmp;
        }

        // This Line Drawing Algorithm can be found at https://github.com/mitkonikov/Line-Drawing-Algorithm/blob/master/ChasingTheCursor/ChasingTheCursor/Main.cs
        // Link in description
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
    }
}
