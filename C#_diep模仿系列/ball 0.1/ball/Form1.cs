using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ball
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer1;

        const int FORM_W = 1200;
        const int FORM_H = 800;

        private Graphics backGraphics;
        private Bitmap backBmp;


        bool w, a, s, d;
        bool mouse;


        GameObj player;
        const float PLAYER_SPEED = 5;

        Monster[] monster;
        const int MONSTER_MAX = 5;
        const float MOVE_SPEED = 4f;

        bullet[] Bullet;
        const int BULLET_MAX = 3;
        const int BULLET_SPEED = 10;

        GameObj mou;

        Random ran;

        public Form1()
        {            
            InitializeComponent();

            backBmp = new Bitmap(FORM_W, FORM_H);
            backGraphics = Graphics.FromImage(backBmp); //FromImage(Image):把指定的Image做為畫布
            //this.DoubleBuffered = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_tick);
            timer1.Interval = 1000 / 30;
            timer1.Enabled = true;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Size = new Size(FORM_W, FORM_H);

            ran = new Random();


            player = new GameObj();
            player.x = ran.Next(0, FORM_W);
            player.y = ran.Next(0, FORM_H);

            monster = new Monster[MONSTER_MAX];
            for (int i = 0; i < MONSTER_MAX; i++)
            {
                monster[i] = new Monster();
                monster[i].x = ran.Next(0, FORM_W);
                monster[i].y = ran.Next(0, FORM_H);
            }

            Bullet = new bullet[BULLET_MAX];
        }
        
        private void timer1_tick(object sender, EventArgs e)
        {
            if (w) player.y -= PLAYER_SPEED; if (s) player.y += PLAYER_SPEED;
            if (a) player.x -= PLAYER_SPEED; if (d) player.x += PLAYER_SPEED;
            //玩家

            for (int i = 0; i < MONSTER_MAX; i++)
            {
                monster[i].monsterMove(player,MOVE_SPEED);
            }
            //怪物

            for (int i = 0; i < BULLET_MAX; i++)
            {
                if (Bullet[i] != null)
                {
                    Bullet[i].bulletMove(BULLET_SPEED);
                }
            }
            //子彈

            //重畫背景
            backGraphics.FillRectangle(new SolidBrush(Color.FromArgb(244,244,244)), 0, 0, FORM_W, FORM_H);
            //
            //backGraphics.DrawEllipse(new Pen(Color.Red, 3), player.x, player.y, 30, 30);//畫主角
            GameObj.darwObj(backGraphics,player, 30, new Pen(Color.Red,3));

            for (int i = 0; i < BULLET_MAX; i++)
            {
                if (Bullet[i] != null)
                {
                    //backGraphics.DrawEllipse(new Pen(Color.Black, 3), Bullet[i].x, Bullet[i].y, 30, 30);//畫子彈
                    GameObj.darwObj(backGraphics, Bullet[i], 20, new Pen(Color.Black, 3));
                }
            }

            for (int i = 0; i < MONSTER_MAX; i++)
            {
                //backGraphics.DrawEllipse(new Pen(Color.Blue, 3), monster[i].x, monster[i].y, 30, 30);//畫敵人
                GameObj.darwObj(backGraphics, monster[i], 30, new Pen(Color.Blue, 3));
            }
            
            //this.Invalidate();
            //覆蓋
            this.CreateGraphics().DrawImageUnscaled(this.backBmp, 0, 0);
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //發射子彈
            mou = new GameObj();
            mou.x = e.X;
            mou.y = e.Y;
            for (int i = 0; i < BULLET_MAX; i++)
            {
                if (Bullet[i] == null)
                {
                    Bullet[i] = new bullet(player,mou);
                    break;
                }
            }

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            move(e.KeyCode, true);
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            move(e.KeyCode, false);
        }

        private void move(Keys k,bool t)
        {
            switch (k)
            {
                case Keys.W:
                    w = t;
                    break;
                case Keys.S:
                    s = t;
                    break;
                case Keys.A:
                    a = t;
                    break;
                case Keys.D:
                    d = t;
                    break;
            }
        }
    }

    class GameObj
    {
        public float x, y;
        public GameObj()
        {
            


        }
        
        public static void darwObj(Graphics g,GameObj gObj,float size,Pen pen)//畫圓
        {
            g.DrawEllipse(pen, gObj.x, gObj.y, size, size);
        }

        public float getLength(GameObj t)//求我(x,y)到他(px,py)的距離
        {
            return (float)Math.Sqrt((t.x - x) * (t.x - x) + (t.y - y) * (t.y - y));
        }



        public void collision(GameObj obj1 , GameObj obj2)
        {

        }

    }

    class Monster : GameObj
    {
        public Monster()
        {

        }


        public void monsterMove(GameObj target, float step_)//目標,前進單位距離
        {
            float L = getLength(target);

            if (L > step_)
            {
                x += (target.x - x) * step_ / L;
                y += (target.y - y) * step_ / L;
            }
            else//如果小於下次要走的距離，直接重疊
            {
                x = target.x;
                y = target.y;
            }
        }

    }

    class bullet : GameObj
    {
        private float dist, dx, dy;
        public bullet(GameObj startloc,GameObj target)
        {
            x = startloc.x;
            y = startloc.y;

            dist = getLength(target);
            dx = (target.x - x);
            dy = (target.y - y);
        }
        public void bulletMove(float step_)
        {
            x += dx * step_ / dist;
            y += dy * step_ / dist;            
        }
        

    }


}
