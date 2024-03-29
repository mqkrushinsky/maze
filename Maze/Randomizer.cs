﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class Randomizer
    {
        private static Random randomizer = new Random();

        public static bool CoinFlip()
        {
            return randomizer.Next(0, 2) == 0;
        }

        public static int RollDie(int numSides)
        {
            return randomizer.Next(0, numSides) + 1;
        }
    }
}
