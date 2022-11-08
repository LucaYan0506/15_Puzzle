using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace _15_Puzzle
{
    class Grid
    {
        private bool[,] _isEmpty = new bool[4,4];
        public int wrongCells = 0;

        public Grid()
        {
            _isEmpty[3, 3] = true;
        }

        public bool[,] isEmpty
        {
            get { return _isEmpty; }
            set { _isEmpty = value; }
        }

        public bool OutOfBound(int[] index)
        {
            return index[0] >= 4 || index[0] < 0 || index[1] >= 4 || index[1] < 0;
        }

        public int[] LocationToIndex(Point location)
        {
            int[] res = new int[2];

            res[0] = location.Y / 100;
            res[1] = location.X / 100;


            return res;
        }

        public bool Win()
        {
            return wrongCells == 0;
        }
    }
}
