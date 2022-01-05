using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;

namespace Basic_Tile_Based_Platformer
{
    class TileMap
    {
        private int x = 0;
        private int y = 0;

        private int tileSize;
        private int[,] map;
        private int mapWidth;
        private int mapHeight;

        public TileMap(string filename,int tileSize)
        {
            this.tileSize = tileSize;

            StreamReader sr = new StreamReader(@filename);
            mapWidth = int.Parse(sr.ReadLine());
            mapHeight = int.Parse(sr.ReadLine());
            map = new int[mapHeight, mapWidth];

            char delimiter = ' '; //定義符號
            for (int row = 0; row < mapHeight; row++)
            {
                string line = sr.ReadLine();
                string[] token = line.Split(delimiter);
                for (int col = 0; col < mapWidth; col++)
                {
                    map[row, col] = int.Parse(token[col]);
                }
            }
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        // 判斷在地圖的哪裡
        public int getColTile(int x)
        {
            return x / tileSize;
        }
        public int getRowTile(int y)
        {
            return y / tileSize;
        }
        //取得地圖資訊
        public int getTile(int row,int col)//y,x
        {
            return map[row, col];
        }
        public int getTileSize()
        {
            return tileSize;
        }
        //---------------------------------------------------
        public void update()
        {

        }

        public void draw(Graphics g)
        {
            Color c = Color.White;

            for (int row = 0; row < mapHeight; row++)
            {
                for (int col = 0; col < mapWidth; col++)
                {
                    int rc = map[row, col];
                    if (rc == 0) c = Color.Black;
                    if (rc == 1) c = Color.White;

                    g.FillRectangle(new SolidBrush(c),
                        x + col * tileSize, y + row * tileSize,
                        tileSize, tileSize);
                }
            }
        }
    }
}
