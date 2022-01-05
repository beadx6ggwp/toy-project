using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace MODE
{
    public partial class Form1 : Form
    {
        Panel Panel1;
        //------------------------------------------------
        public static int Rows = 15;
        public static int p_size = 562;//修改棋盤
        //------------------------------------------------

        int[,] board = new int[15,15];

        Color W = Color.White;
        Color B = Color.Black;

        public GFX engine;
        public Form1()
        {
            InitializeComponent();
            //panel生成
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //設定Form1
            this.Size = new Size(700, 700);

            //生成Panel
            Panel1 = new Panel();
            Panel1.Location = new Point(0, 0);
            Panel1.Size = this.Size;
            Panel1.BorderStyle = BorderStyle.None;
            this.BackColor = Color.BlanchedAlmond;
            this.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.Panel1.MouseDown += new MouseEventHandler(this.panel1_MouseDown);
            this.Controls.Add(Panel1);
        }

        private void panel1_Paint(object sender, EventArgs e)//使用panel1.Refresh()來觸發
        {
            board = new int[15, 15];
            Graphics pg = Panel1.CreateGraphics();
            engine = new GFX(pg,Rows,p_size,50);
        }

        private void panel1_MouseDown(object sender, EventArgs e)//下棋
        {
            Point mouse = Cursor.Position;
            mouse = Panel1.PointToClient(mouse);
            MessageBox.Show(location(mouse).X+","+ location(mouse).Y);
        }

        private void Game()
        {
            
        }

        private Point location(Point loc)
        {
            int x = 0;int y = 0;
            int xp = 0;int yp = 0;
            int gap = GFX.gap;
            
            for (int i = 0; i < Rows; i++)
            {
                if (i * gap - gap / 2 < loc.X && loc.X < i * gap + gap / 2)
                {
                    x = i;
                    xp = i * gap;
                }
                if (i * gap - gap / 2 < loc.Y && loc.Y < i * gap + gap / 2)
                {
                    y = i;
                    yp = i * gap;
                }
            }
            return new Point(x,y);
        }
    }
}
