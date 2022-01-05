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
        bool gameover;


        GameObj player;
        const float PLAYER_SPEED = 5;
        const float PLAYER_R = 15;

        Monster[] monster;
        const int MONSTER_MAX = 20;
        const float MOVE_SPEED = 4f;
        const float MONSTER_R = 18;
        const float MONSTER_CD = 10;
        int cd = 0;


        Bullet[] bullet;
        const int BULLET_MAX = 10;
        const float BULLET_SPEED = 10f;
        const float BULLET_R = 10;

        GameObj mou;//滑鼠

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
            player.r = PLAYER_R;

            monster = new Monster[MONSTER_MAX];
            for (int i = 0; i < MONSTER_MAX; i++)
            {
                monster[i] = new Monster();
                monster[i].x = ran.Next(0, FORM_W);
                monster[i].y = ran.Next(0, FORM_H);
                monster[i].r = MONSTER_R;
            }

            bullet = new Bullet[BULLET_MAX];
        }
        
        private void timer1_tick(object sender, EventArgs e)
        {
            if (w) player.y -= PLAYER_SPEED; if (s) player.y += PLAYER_SPEED;
            if (a) player.x -= PLAYER_SPEED; if (d) player.x += PLAYER_SPEED;
            //玩家

            for (int i = 0; i < MONSTER_MAX; i++)
            {
                if (monster[i] != null)
                {
                    monster[i].monsterMove(player, MOVE_SPEED);
                }               
            }
            
            if (cd < MONSTER_CD)
                cd++;
            else
                cd = 0;

            for (int i = 0; i < MONSTER_MAX; i++)
            {
                if (monster[i] == null && cd == MONSTER_CD)
                {
                    monster[i] = new Monster();
                    monster[i].x = ran.Next(0, FORM_W);
                    monster[i].y = ran.Next(0, FORM_H);
                    monster[i].r = MONSTER_R;
                    break;
                }
            }
            //怪物

            for (int i = 0; i < BULLET_MAX; i++)
            {
                if (bullet[i] != null)
                {
                    bullet[i].bulletMove(BULLET_SPEED);
                    if (bullet[i].x <= 0 || bullet[i].x >= this.Width ||
                        bullet[i].y <= 0 || bullet[i].y >= this.Width) bullet[i] = null;
                }
            }
            //子彈


            //碰撞判斷
            for (int i = 0; i < MONSTER_MAX; i++)
            {
                if (GameObj.collision(monster[i], player) && monster[i] != null)
                {
                    monster[i] = null;
                    break;
                }
            }

            for (int i = 0; i < BULLET_MAX; i++)
            {
                if (bullet[i] == null) continue;
                for (int j = 0; j < MONSTER_MAX; j++)
                {
                    if (monster[j] == null) continue;
                    if (GameObj.collision(monster[j], bullet[i]))
                    {
                        monster[j] = null;
                        bullet[i] = null;
                        break;
                    }
                }
            }

            //重畫背景
            backGraphics.FillRectangle(new SolidBrush(Color.FromArgb(240,240,240)), 0, 0, FORM_W, FORM_H);
            //
            //player.darwObj(backGraphics,player, new Pen(Color.Red, 3));//畫玩家
            player.darwObj(backGraphics, new Pen(Color.Red, 3), PLAYER_R);

            int bullet_total = BULLET_MAX, monster_total = 0;

            for (int i = 0; i < BULLET_MAX; i++)//畫子彈
            {
                if (bullet[i] != null)
                {
                    //bullet[i].darwObj(backGraphics, bullet[i], new Pen(Color.Black, 3));
                    bullet[i].darwObj(backGraphics, new Pen(Color.Black, 3), BULLET_R);
                    bullet_total--;
                }
            }

            for (int i = 0; i < MONSTER_MAX; i++)//畫敵人
            {
                if (monster[i] != null)
                {
                    //monster[i].darwObj(backGraphics, monster[i], new Pen(Color.Blue, 3));
                    monster[i].darwObj(backGraphics, new Pen(Color.Blue, 3), MONSTER_R);
                    Image img = new Bitmap("WWW.jpg");
                    backGraphics.DrawImage(img, monster[i].x -20, monster[i].y -20,40,40);
                    monster_total++;
                }
            }

            string str = string.Format("子彈數量:{0}\r\n敵人數量:{1}", bullet_total.ToString(), monster_total.ToString());

            backGraphics.DrawString(str,DefaultFont,Brushes.Black,5,5);

            //this.Invalidate();
            //覆蓋
            this.CreateGraphics().DrawImageUnscaled(this.backBmp, 0, 0);

            if (gameover) timer1.Enabled = false;
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //發射子彈
            mou = new GameObj();
            mou.x = e.X;
            mou.y = e.Y;
            for (int i = 0; i < BULLET_MAX; i++)
            {
                if (bullet[i] == null)
                {
                    bullet[i] = new Bullet(player,mou);
                    bullet[i].r = BULLET_R;
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
        public float r;
        public GameObj()
        {           


        }
        
        public void darwObj(Graphics g, Pen pen, float r)//畫圓
        {
            g.DrawEllipse(pen, x - r, y - r, r * 2, r * 2);
            //g.DrawEllipse(pen, x, y, 1, 1);//圓心
            g.FillEllipse(new SolidBrush(Color.Orange), x-3, y-3, 6, 6);
        }

        public float getLength(GameObj t)//求我(x,y)到他(px,py)的距離
        {
            return (float)Math.Sqrt((t.x - x) * (t.x - x) + (t.y - y) * (t.y - y));
        }

        public static bool collision(GameObj obj1 , GameObj obj2)
        {
            if (obj1 == null || obj2 == null) return false;
            float dist = (float)Math.Sqrt((obj1.x - obj2.x) * (obj1.x - obj2.x) + (obj1.y - obj2.y) * (obj1.y - obj2.y));

            return dist <= (obj1.r + obj2.r);
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

    class Bullet : GameObj
    {
        private float dist, dx, dy;
        public Bullet(GameObj startloc,GameObj target)
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
