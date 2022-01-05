using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ball
{
    class GameClass
    {
        int width, height;

        Graphics g;

        public GameObj player;

        public List<Monster> monst_list;
        public int monstMax;

        public List<Bullet> bullet_list;
        Random ran = new Random();
        public GameClass(Graphics g_, int w,int h)
        {
            g = g_;
            width = w;
            height = h;

            player = new GameObj();
            monst_list = new List<Monster>();
            bullet_list = new List<Bullet>();
        }
        //Create obj
        public void CraetePlayer(float x, float y, float r)
        {
            player = new GameObj(x, y, r, 5);
        }
        public void CraeteMonster(float x, float y, float r)
        {
            monst_list.Add(new Monster(x, y, r, 3));
        }
        public void CreateMonster(float r, int max)
        {
            monstMax = max;
            for (int i = 0; i < max; i++)
                CraeteMonster(ran.Next(0, width), ran.Next(0, height), r);
        }
        public void CraeteBullet(GameObj startloc, GameObj target, float r)
        {
            bullet_list.Add(new Bullet(startloc, target, r));
        }
        //-----------------------------------

        //Move obj
        public void MoveMonster(GameObj target, float speed)
        {
            for (int i = 0; i < monst_list.Count; i++)
            {
                monst_list[i].monsterMove(target, speed);
            }
        }
        public void MoveBullet(float speed)
        {
            for (int i = 0; i < bullet_list.Count; i++)
            {
                bullet_list[i].bulletMove(speed);

                if (bullet_list[i].x < 0 || bullet_list[i].x > width ||
                    bullet_list[i].y < 0 || bullet_list[i].y > height) bullet_list.RemoveAt(i);
            }
        }
        //-------------------------------------
        //Check Collison
        public void Check()
        {
            for (int i = 0; i < monst_list.Count; i++)//monster->player monster = X
            {
                if (GameObj.collision(monst_list[i], player))//check monster && player
                {
                    monst_list.RemoveAt(i);
                    player.hp--;
                    break;
                }
                for (int j = 0; j < bullet_list.Count; j++)//check bullet && player
                {
                    if (GameObj.collision(monst_list[i], bullet_list[j]))
                    {
                        monst_list[i].hp--;
                        if(monst_list[i].hp<=0)
                            monst_list.RemoveAt(i);
                        bullet_list.RemoveAt(j);
                        break;//to cheak next bullet
                    }
                }
            }
        }

    }

    class GameObj
    {
        public float x, y;
        public float r;
        public int maxHp;
        public int hp;

        public GameObj()
        {

        }
        public GameObj(float x_, float y_, float r_)
        {
            x = x_;
            y = y_;
            r = r_;
        }
        public GameObj(float x_, float y_, float r_, int hp_)
        {
            x = x_;
            y = y_;
            r = r_;
            maxHp = hp_;
            hp = hp_;
        }

        public void darwObj(Graphics g, Color c, float r)//Draw circle
        {
            SolidBrush sb = new SolidBrush(c);
            g.FillEllipse(sb, x - r, y - r, r * 2, r * 2);
            g.DrawEllipse(new Pen(Color.Black, 3), x - r, y - r, r * 2, r * 2); //Circle center = x,y
            g.FillEllipse(new SolidBrush(Color.Orange), x-1, y-1, 2, 2); //mark
        }

        public float getLength(GameObj t)//求我(x,y)到他(px,py)的向量長
        {
            if (t == null) return 0;
            return (float)Math.Sqrt((t.x - x) * (t.x - x) + (t.y - y) * (t.y - y));
        }

        public static bool collision(GameObj obj1, GameObj obj2)
        {
            if (obj1 == null || obj2 == null) return false;
            float dist = (float)Math.Sqrt((obj1.x - obj2.x) * (obj1.x - obj2.x) + (obj1.y - obj2.y) * (obj1.y - obj2.y));

            return dist <= (obj1.r + obj2.r);
        }

        public void drawStatus(Graphics g, int w, int h, int val, int maxval)
        {
            float wp = (float)(((double)val / (double)maxval) * w);
            float xr = x - w / 2;
            float yr = y - h / 2;
            g.FillRectangle(Brushes.White, xr, yr - r - 10, wp, h);
            g.DrawRectangle(Pens.White, xr, yr - r - 10, w, h);
        }
        public void drawStatus(Graphics g, int w, int h)
        {
            drawStatus(g, w, h, hp, maxHp);
        }

    }

    class Monster : GameObj
    {
        public Monster(float x_, float y_, float r_)
        {
            x = x_;
            y = y_;
            r = r_;
        }
        public Monster(float x_, float y_, float r_, int hp_)
        {
            x = x_;
            y = y_;
            r = r_;
            maxHp = hp_;
            hp = hp_;
        }
        public void monsterMove(GameObj target, float step_)//[目標,前進單位距離]
        {
            float L = getLength(target);

            if (L > step_)
            {
                x += (target.x - x) * step_ / L;
                y += (target.y - y) * step_ / L;
            }
            else//如果小於下次要走的距離，直接重疊 避免溢位
            {
                x = target.x;
                y = target.y;
            }
        }

    }
    class Bullet : GameObj
    {
        public Bullet()
        {

        }
        private float dist, dx, dy;
        public Bullet(GameObj startloc, GameObj target, float r_)
        {
            r = r_;
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
