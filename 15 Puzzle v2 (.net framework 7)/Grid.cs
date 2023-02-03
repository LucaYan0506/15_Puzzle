namespace _15_Puzzle_v2__.net_framework_7_
{
    class Grid
    {
        private bool[,] _isEmpty = new bool[4, 4];
        public int wrongCells = 0;

        public Grid()
        {
            //set as default a empty grid
            _isEmpty[3, 3] = true;
        }

        public bool[,] isEmpty
        {
            get { return _isEmpty; }
            set { _isEmpty = value; }
        }

        //check if index in outside of the grid
        public bool OutOfBound(int[] index)
        {
            return index[0] >= 4 || index[0] < 0 || index[1] >= 4 || index[1] < 0;
        }

        //convert location to indexes (i & j)
        public int[] LocationToIndex(Point location)
        {
            int[] res = new int[2];

            res[0] = location.Y / 100;
            res[1] = location.X / 100;


            return res;
        }

        //check if it wins
        public bool Win()
        {
            return wrongCells == 0;
        }
    }
}
