using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Basic_Tile_Based_Platformer
{
    class Player
    {
        private Color playerColor = Color.Red;

        private double x;
        private double y;
        private double dx;
        private double dy;

        private int width;
        private int height;

        private bool topLeft;
        private bool topRight;
        private bool bottomLeft;
        private bool bottomRight;

        private bool left;
        private bool right;
        private bool jumping;
        private bool falling;

        private double moveSpeed;
        private double maxSpeed;
        private double maxFallingSpeed;
        private double stopSpeed;
        private double jumpStart;
        private double gravity;

        private TileMap tileMap;
        public Player(TileMap tm)
        {
            tileMap = tm;

            width = 20;
            height = 20;

            moveSpeed = 0.6;
            maxSpeed = 4.2;
            maxFallingSpeed = 12;
            stopSpeed = 0.3;
            jumpStart = -11.0;
            gravity = 0.64;

        }

        public void setx(int i) { x = i; }
        public void sety(int i) { y = i; }
        public void setLeft(bool b) { left = b; }
        public void setRight(bool b) { right = b; }
        public void setJumping(bool b) {
            if (!falling)
            {
                jumping = true;
            }
        }

        private void calculateCorners(double x,double y)
        {
            //取得玩家範圍
            int leftTile = tileMap.getColTile((int)(x - width / 2));
            int rightTile = tileMap.getColTile((int)(x + width / 2) - 1);
            int topTile = tileMap.getRowTile((int)(y - height / 2));
            int bottomTile = tileMap.getRowTile((int)(y + height / 2) - 1);
            //取得是否狀到 0:障礙物 1:行走區
            topLeft = (tileMap.getTile(topTile, leftTile) == 0);
            topRight = (tileMap.getTile(topTile, rightTile) == 0);
            bottomLeft = (tileMap.getTile(bottomTile, leftTile) == 0);
            bottomRight = (tileMap.getTile(bottomTile, rightTile) == 0);

            if (tileMap.getTile(bottomTile, leftTile) == 0)
                bottomLeft = true;
            if (tileMap.getTile(bottomTile, rightTile) == 0)
                bottomRight = true;
        }
        //-------------------------------------------------------------
        public void update()
        {
            if (left)
            {
                dx -= moveSpeed;
                if (dx < -maxSpeed) dx = -maxSpeed;
            }
            else if (right)
            {
                dx += moveSpeed;
                if (dx > maxSpeed) dx = maxSpeed;
            }
            else
            {
                if (dx > 0)
                {
                    dx -= stopSpeed;
                    if (dx < 0) dx = 0;
                }
                else if (dx < 0)
                {
                    dx += stopSpeed;
                    if (dx > 0) dx = 0;
                }
            }

            if (jumping)
            {
                dy = jumpStart;
                falling = true;
                jumping = false;
            }

            if (falling)
            {
                dy += gravity;
                if (dy > maxFallingSpeed) dy = maxFallingSpeed;
            }
            else
            {
                dy = 0;
            }

            // check collisions

            //取得原位置
            int currCol = tileMap.getColTile((int)x);
            int currRow = tileMap.getRowTile((int)y);
            //取得移動後的位置
            double tox = x + dx;
            double toy = y + dy;
            //保存位置
            double tempx = x;
            double tempy = y;

            //檢查移動
            calculateCorners(x, toy);
            if (dy < 0)
            {
                //往上跳，撞到了。就回到原位
                if (topLeft || topRight)
                {
                    dy = 0;
                    tempy = currRow * tileMap.getTileSize() + height / 2;
                }
                else
                {
                    tempy += dy;
                }
            }
            if (dy > 0)
            {
                // 往下掉，撞到後，移動到下面一個
                if (bottomLeft || bottomRight)
                {
                    dy = 0;
                    falling = false;
                    tempy = (currRow + 1) * tileMap.getTileSize() - height / 2;
                }
                else
                {
                    tempy += dy;
                }
            }

            calculateCorners(tox, y);
            if (dx < 0)
            {
                if (topLeft || bottomLeft)
                {
                    dx = 0;
                    tempx = currCol * tileMap.getTileSize() + width / 2;
                }
                else
                {
                    tempx += dx;
                }
            }
            if (dx > 0)
            {
                if (bottomRight || topRight)
                {
                    dx = 0;
                    tempx = (currCol + 1) * tileMap.getTileSize() - width / 2;
                }
                else
                {
                    tempx += dx;
                }
            }

            if (!falling)
            {
                calculateCorners(x, y + 1);
                if (!bottomLeft && !bottomRight)
                {
                    falling = true;
                }
            }

            x = tempx;
            y = tempy;

            //move map
            tileMap.X = ((int)(400 / 2 - x));
            tileMap.Y = ((int)(400 / 2 - y));
        }

        public void draw(Graphics g)
        {
            int tx = tileMap.X;
            int ty = tileMap.Y;
            //讓x,y在中心
            g.FillRectangle(new SolidBrush(playerColor),
                (int)(tx + x - width / 2),
                (int)(ty + y - height / 2),
                width, height);
        }
        
    }
}
