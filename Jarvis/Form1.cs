using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jarvis
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Graphics g;
        List<Point> points = new List<Point>();

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            g.DrawEllipse(new Pen(Color.Black), e.Location.X, e.Location.Y, 1, 1);
            points.Add(new Point(e.Location.X, e.Location.Y));
            pictureBox1.Image = bmp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
            points = new List<Point>();
            hull = new List<Point>();
        }

        private double pol_angle(Point first, Point second)
        {
            int x = second.X - first.X;
            int y = second.Y - first.Y;
            double a = Math.Atan2(y, x);
            if (a < 0)
                a += 2 * Math.PI;
            return a;
        }

        private double cos(Point first, Point second, Point third)
        {
            int x1 = second.X - first.X;
            int y1 = second.Y - first.Y;
            int x2 = third.X - second.X;
            int y2 = third.Y - second.Y;

            return ((x1 * x2 + y1 * y2) / Math.Sqrt(x1 * x1 + y1 * y1) * Math.Sqrt(x2 * x2 + y2 * y2));
        }

        private int rotate(Point a, Point b, Point c)
        {
            return ((b.X - a.X) * (c.Y - b.Y) - (b.Y - a.Y) * (c.X - b.X));
        }
        
        List<Point> hull = new List<Point>();
        private void button1_Click(object sender, EventArgs e)
        {
            if (points.Count() < 3)
                MessageBox.Show("Нарисуйте хотя бы три точки");
            else
            {
                Point first = points[0];
                for (int i = 1; i < points.Count(); i++)
                    if (points[i].X < first.X)
                        first = points[i];
                    else if (points[i].X == first.X)
                        if (points[i].Y > first.Y)
                            first = points[i];
                
                Point cur;
                do
                {
                    hull.Add(first);
                    cur = points[0];
                    for (int i = 1; i < points.Count(); i++)
                        if ((cur == first) || (rotate(points[i], first, cur) > 0))
                            cur = points[i];
                    first = cur;
                }
                while (cur != hull[0]);

                for (int i = 0; i < hull.Count() - 1; i++)
                    g.DrawLine(new Pen(Color.Red), hull[i], hull[i + 1]);
                g.DrawLine(new Pen(Color.Red), hull[hull.Count() - 1], hull[0]);
                pictureBox1.Image = bmp;
            }          
        }
    }
}
