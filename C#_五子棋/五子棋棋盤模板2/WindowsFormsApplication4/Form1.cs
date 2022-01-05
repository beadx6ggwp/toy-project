using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }


        Graphics g;
        Pen p = new Pen(Color.Black, 1);
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = this.CreateGraphics();

            for (int i = 0; i < 7; i++)
            {
                g.DrawLine(p, new Point(i*100, 0), new Point(i*100, 600));
                g.DrawLine(p, new Point(0, i*100), new Point(600, i*100));
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            hit(p,Color.Black);
        }

        private Point hit(Point loc,Color color_)
        {
            int x=0,y=0;
            int locx = 0, locy = 0;

            x = (loc.X+50) / 100;
            y = (loc.Y+50) / 100;

            locx = 100 * x;
            locy = 100 * y;

            g.DrawEllipse(new Pen(color_,5), locx-10, locy-10, 20,20);
            MessageBox.Show(x+","+y+ "\n\r" + loc.X+","+loc.Y);
            return new Point(0,0);
        }
    }
}
