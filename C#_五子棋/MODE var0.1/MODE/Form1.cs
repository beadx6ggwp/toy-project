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
        public int Rows = 15;
        public int size = 560;//修改棋盤
        public int slant = 50;
        //------------------------------------------------

        int[,] board = new int[15,15];

        Color W = Color.White;
        Color B = Color.Black;

        bool turn = true;//t=b f=w

        public GFX PicEngine;
        public Form1()
        {
            InitializeComponent();
            //panel生成
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //設定Form1
            this.Size = new Size(680, 700);

            //生成Panel
            Panel1 = new Panel();
            Panel1.Location = new Point(0, 0);
            Panel1.Size = this.Size;
            Panel1.BorderStyle = BorderStyle.None;
            this.BackColor = Color.SandyBrown;//BlanchedAlmond
            this.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.Panel1.MouseDown += new MouseEventHandler(this.panel1_MouseDown);
            this.Controls.Add(Panel1);
        }

        private void panel1_Paint(object sender, EventArgs e)//使用panel1.Refresh()來觸發
        {
            board = new int[15, 15];
            Graphics pg = Panel1.CreateGraphics();
            PicEngine = new GFX(pg,Rows,size,slant);
        }

        private void panel1_MouseDown(object sender, EventArgs e)//下棋
        {
            Point mouse = Cursor.Position;
            mouse = Panel1.PointToClient(mouse);
            if (mouse.X < slant - GFX.gap / 2 || mouse.X > size + slant + GFX.gap / 2 ||
                mouse.Y < slant - GFX.gap / 2 || mouse.Y > size + slant + GFX.gap / 2) { return; }//是否出界
            Game(location(mouse));

            MessageBox.Show(mouse.ToString()+" "+location(mouse).ToString());
        }

        private void Game(Point P)
        {
            //因Arr和直角坐標相反
            if (board[P.Y, P.X] != 0) { return; }
            if (turn)
            {
                PicEngine.drawB(P);
                board[P.Y, P.X] = 1;
            }
            else
            {
                PicEngine.drawW(P);
                board[P.Y, P.X] = 2;
            }
            turn = !turn;
        }

        private Point location(Point loc)
        {
            int x = 0;int y = 0;
            int xp = 0;int yp = 0;
            int gap = GFX.gap;

            for (int i = 0; i < Rows; i++)
            {                
                if ((i * gap - gap / 2) + slant - 1 < loc.X && loc.X < (i * gap + gap / 2) + slant)
                {
                    x = i;
                    xp = i * gap;
                }
                if ((i * gap - gap / 2) + slant - 1 < loc.Y && loc.Y < (i * gap + gap / 2) + slant)
                {
                    y = i;
                    yp = i * gap;
                }
            }
            return new Point(x,y);
        }
    }
}
