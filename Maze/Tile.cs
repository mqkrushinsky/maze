using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public Tile PathParent { get; set; }

        public int DrawX
        {
            get
            {
                return (X * 50) + 20;
            }
        }

        public int DrawY
        {
            get
            {
                return (Y * 50) + 20;
            }
        }

        public bool TopBorder { get; set; }
        public bool BottomBorder { get; set; }
        public bool LeftBorder { get; set; }
        public bool RightBorder { get; set; }

        public Tile(int x, int y)
        {
            X = x;
            Y = y;

            TopBorder = (y > 0) ? Randomizer.CoinFlip() : false;
            BottomBorder = Randomizer.CoinFlip();
            LeftBorder = (x > 0) ? Randomizer.CoinFlip() : false;
            RightBorder = Randomizer.CoinFlip();
        }
    }
}
