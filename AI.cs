using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace _15_Puzzle
{
    class AI
    {
        //get 4 directions 
        int[][] dirs = new int[][]
        {
            new int[]{ 1,0 },   //top
            new int[]{ 0,-1 },  //right
            new int[]{ 0,1 },   //left
            new int[]{ -1,0 },  //bottom    
        };

        //this algorithm is based on DFS (move blocks in GUI, while is trying to find the path)
        public void DFS(ref Grid grid, ref bool AI_is_working, ref Panel panel1)
        {
            int[] emptyCell = new int[2];
            Button[,] blocks = new Button[grid.isEmpty.GetLength(0), grid.isEmpty.GetLength(1)];

            //save buttons in a 4 by 4 grid
            foreach (Button control in panel1.Controls)
            {
                int[] index = grid.LocationToIndex(control.Location);
                blocks[index[0], index[1]] = control;
            }

            //find  the empty cell
            for (int i = 0; i < blocks.GetLength(0); i++)
            {
                for (int j = 0; j < blocks.GetLength(1); j++)
                    if (blocks[i, j] == null)
                    {
                        emptyCell = new int[] { i, j };
                        break;
                    }
            }

            //save position of blocks in a 4 by 4 grid
            //e.g. 2  1  3  4
            //     6  8  11 5
            //     13 14 7  9
            //     9  10 15 16(empty)
            int[,] curr_state = new int[4, 4];
            for (int i = 0; i < blocks.GetLength(0); i++)
                for (int j = 0; j < blocks.GetLength(1); j++)
                {
                    if (blocks[i, j] == null)
                        curr_state[i, j] = 16;
                    else
                        curr_state[i, j] = int.Parse(blocks[i, j].Text);
                }

            //set to check this state is visited or not
            HashSet<string> visited = new HashSet<string>();
            HashSet<int> locked = new HashSet<int>();

            string curr_state_string = "";
            foreach (int x in curr_state)
                curr_state_string += x + " ";

            //check if any of these blocks are in the correct cell, if so lock them
            for (int i = 0; i < 14; i++)
                lockBlock(i, curr_state_string, ref locked);

            //store possible combination/moves
            Stack<KeyValuePair<int, int[]>> st = new Stack<KeyValuePair<int, int[]>>();
            //store previous combination/moves
            Stack<KeyValuePair<int, int[]>> back = new Stack<KeyValuePair<int, int[]>>();

            Action action;

            //init st (start from 4 directions of the emptycell)
            foreach (int[] dir in dirs)
                st.Push(new KeyValuePair<int, int[]>(0, new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }));

            //loop until it tried every combination
            while (st.Count > 0)
            {
                //if user asked to stop AI, stop it
                if (!AI_is_working)
                    return;

                //pop st
                var curr = st.Pop();
                int stage = curr.Key;
                int[] currIndex = curr.Value;
                int temp;
                bool isCorrectSide_prev;

                if (back.Count > 0 && !grid.Win())
                {
                    //if the current stage is lower than the stage of the last move
                    if (stage <= back.Peek().Key)
                    {
                        //repeat last move (in reverse, so that it goes to the previous stage)
                        while (back.Count > 0 && stage <= back.Peek().Key)
                        {
                            //if user asked to stop AI, stop it
                            if (!AI_is_working)
                                return;

                            //repeat last move
                            var last_back = back.Pop();
                            var backIndex = last_back.Value;
                            temp = curr_state[backIndex[0], backIndex[1]];
                            curr_state[backIndex[0], backIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                            curr_state[emptyCell[0], emptyCell[1]] = temp;
                            action = () => blocks[backIndex[0], backIndex[1]].PerformClick();
                            blocks[backIndex[0], backIndex[1]].Invoke(action);
                            blocks[emptyCell[0], emptyCell[1]] = blocks[backIndex[0], backIndex[1]];
                            blocks[backIndex[0], backIndex[1]] = null;

                            emptyCell = backIndex;
                        }

                    }
                }

                //if currindex is outside the grid, skip it
                if (grid.OutOfBound(currIndex))
                    continue;

                //if, before we move the current block, current block is in the correct side (to know what does "CorrectSide" mean click into the function)
                isCorrectSide_prev = IsCorrectSide(int.Parse(blocks[currIndex[0], currIndex[1]].Text), curr_state, ref locked);

                //move block (in the grid, not in the GUI)
                temp = curr_state[currIndex[0], currIndex[1]];
                curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                curr_state[emptyCell[0], emptyCell[1]] = temp;


                curr_state_string = "";

                foreach (int x in curr_state)
                    curr_state_string += x + " ";

                //check if curr_state is already visited (avoid infinite loop), if so skip it
                if (visited.Contains(curr_state_string) || locked.Contains(int.Parse(blocks[currIndex[0], currIndex[1]].Text)))
                {
                    //move the block back (in the grid, not in the GUI)
                    temp = curr_state[currIndex[0], currIndex[1]];
                    curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                    curr_state[emptyCell[0], emptyCell[1]] = temp;

                    //skip curr st
                    continue;
                }

                //add curr_state into visited, so, later on, we know if the curr_state is visited.
                visited.Add(curr_state_string);

                //if before we move the block, block is in the correct side, but after moved, it isn't. skip it (this used to avoid useless move)
                if (isCorrectSide_prev && !IsCorrectSide(int.Parse(blocks[currIndex[0], currIndex[1]].Text), curr_state, ref locked))
                {
                    //move the block back (in the grid, not in the GUI)
                    temp = curr_state[currIndex[0], currIndex[1]];
                    curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                    curr_state[emptyCell[0], emptyCell[1]] = temp;
                    
                    //skip curr st
                    continue;
                }

                //check if curr blocks is in the correct cell, if so lock it
                if (lockBlock(int.Parse(blocks[currIndex[0], currIndex[1]].Text), curr_state_string, ref locked))
                {
                    //restart to do DFS (with locked blocks saved, so it knows which block to move and which not)
                    st.Clear();
                    back.Clear();
                    visited.Clear();
                }

                //move block (in the GUI)
                action = () => blocks[currIndex[0], currIndex[1]].PerformClick();
                blocks[currIndex[0], currIndex[1]].Invoke(action);
                blocks[emptyCell[0], emptyCell[1]] = blocks[currIndex[0], currIndex[1]];
                blocks[currIndex[0], currIndex[1]] = null;

                //now emptyCell = currIndex 
                //currIndex = emptycell because 2 buttons are swapped
                var temp2 = currIndex;
                currIndex = emptyCell;
                emptyCell = temp2;

                //other possible combinations
                foreach (int[] dir in dirs)
                    if (!grid.OutOfBound(new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }))
                        st.Push(new KeyValuePair<int, int[]>(stage + 1, new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }));

                //save curr move
                back.Push(new KeyValuePair<int, int[]>(stage, currIndex));

                //if win, stop DFS
                if (grid.Win())
                    return;
            }

        }


        //this algorithm is based on DFS (move blocks at end of the algo)
        public Stack<KeyValuePair<int, int[]>> DFS2(ref Grid grid, ref Panel panel1)
        {
            int[] emptyCell = new int[2];
            Button[,] blocks = new Button[grid.isEmpty.GetLength(0), grid.isEmpty.GetLength(1)];

            //save buttons in a 4 by 4 grid
            foreach (Button control in panel1.Controls)
            {
                int[] index = grid.LocationToIndex(control.Location);
                blocks[index[0], index[1]] = control;
            }

            //find  the empty cell
            for (int i = 0; i < blocks.GetLength(0); i++)
            {
                for (int j = 0; j < blocks.GetLength(1); j++)
                    if (blocks[i, j] == null)
                    {
                        emptyCell = new int[] { i, j };
                        break;
                    }
            }

            //save position of blocks in a 4 by 4 grid
            //e.g. 2  1  3  4
            //     6  8  11 5
            //     13 14 7  9
            //     9  10 15 16(empty)
            int[,] curr_state = new int[4, 4];
            for (int i = 0; i < blocks.GetLength(0); i++)
                for (int j = 0; j < blocks.GetLength(1); j++)
                {
                    if (blocks[i, j] == null)
                        curr_state[i, j] = 16;
                    else
                        curr_state[i, j] = int.Parse(blocks[i, j].Text);
                }

            //set to check this state is visited or not
            HashSet<string> visited = new HashSet<string>();
            HashSet<int> locked = new HashSet<int>();

            string goal_state = "1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 ";
            string curr_state_string = "";
            foreach (int x in curr_state)
                curr_state_string += x + " ";

            //check if any of these blocks are in the correct cell, if so lock them
            for (int i = 0; i < 14; i++)
                lockBlock(i, curr_state_string, ref locked);

            //store possible combination/moves
            Stack<KeyValuePair<int, int[]>> st = new Stack<KeyValuePair<int, int[]>>();
            //store previous combination/moves
            Stack<KeyValuePair<int, int[]>> back = new Stack<KeyValuePair<int, int[]>>();
            //store moves that will solve the puzzle
            Stack<KeyValuePair<int, int[]>> right_move = new Stack<KeyValuePair<int, int[]>>();

            //init st (start from 4 directions of the emptycell)
            foreach (int[] dir in dirs)
                st.Push(new KeyValuePair<int, int[]>(0, new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }));

            //loop until it tried every combination
            while (st.Count > 0)
            {
                //pop st
                var curr = st.Pop();
                int stage = curr.Key;
                int[] currIndex = curr.Value;

                int temp;
                bool isCorrectSide_prev;

                if (back.Count > 0 && curr_state_string != goal_state)
                {
                    //if the current stage is lower than the stage of the last move
                    if (stage <= back.Peek().Key)
                    {
                        //repeat last move (in reverse, so that it goes to the previous stage)
                        while (back.Count > 0 && stage <= back.Peek().Key)
                        {
                            //pop the last item from right_move (since it is not the right move)
                            right_move.Pop();

                            //repeat last move
                            var last_back = back.Pop();
                            var backIndex = last_back.Value;
                            temp = curr_state[backIndex[0], backIndex[1]];
                            curr_state[backIndex[0], backIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                            curr_state[emptyCell[0], emptyCell[1]] = temp;
                            blocks[emptyCell[0], emptyCell[1]] = blocks[backIndex[0], backIndex[1]];
                            blocks[backIndex[0], backIndex[1]] = null;

                            emptyCell = backIndex;
                        }

                    }
                }

                //if currindex is outside the grid, skip it
                if (grid.OutOfBound(currIndex))
                    continue;

                //if, before we move the current block, current block is in the correct side (to know what does "CorrectSide" mean click into the function)
                isCorrectSide_prev = IsCorrectSide(int.Parse(blocks[currIndex[0], currIndex[1]].Text), curr_state, ref locked);

                //move block (in the grid, not in the GUI)
                temp = curr_state[currIndex[0], currIndex[1]];
                curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                curr_state[emptyCell[0], emptyCell[1]] = temp;


                curr_state_string = "";

                foreach (int x in curr_state)
                    curr_state_string += x + " ";

                //check if curr_state is already visited (avoid infinite loop), if so skip it
                if (visited.Contains(curr_state_string) || locked.Contains(int.Parse(blocks[currIndex[0], currIndex[1]].Text)))
                {
                    //move the block back (in the grid, not in the GUI)
                    temp = curr_state[currIndex[0], currIndex[1]];
                    curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                    curr_state[emptyCell[0], emptyCell[1]] = temp;

                    //skip curr st
                    continue;
                }

                //add curr_state into visited, so, later on, we know if the curr_state is visited.
                visited.Add(curr_state_string);

                //if before we move the block, block is in the correct side, but after moved, it isn't. skip it (this used to avoid useless move)
                if (isCorrectSide_prev && !IsCorrectSide(int.Parse(blocks[currIndex[0], currIndex[1]].Text), curr_state, ref locked))
                {
                    //move the block back (in the grid, not in the GUI)
                    temp = curr_state[currIndex[0], currIndex[1]];
                    curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                    curr_state[emptyCell[0], emptyCell[1]] = temp;

                    //skip curr st
                    continue;
                }

                //check if curr blocks is in the correct cell, if so lock it
                if (lockBlock(int.Parse(blocks[currIndex[0], currIndex[1]].Text), curr_state_string, ref locked))
                {
                    //restart to do DFS (with locked blocks saved, so it knows which block to move and which not)
                    st.Clear();
                    back.Clear();
                    visited.Clear();
                }

                //move block
                blocks[emptyCell[0], emptyCell[1]] = blocks[currIndex[0], currIndex[1]];
                blocks[currIndex[0], currIndex[1]] = null;

                //push this move to right_move (if it is not the right, it will be poped when we pop back(stack))
                right_move.Push(new KeyValuePair<int, int[]>(stage, currIndex));


                //now emptyCell = currIndex 
                //currIndex = emptycell because 2 buttons are swapped
                var temp2 = currIndex;
                currIndex = emptyCell;
                emptyCell = temp2;

                //other possible combinations
                foreach (int[] dir in dirs)
                    if (!grid.OutOfBound(new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }))
                        st.Push(new KeyValuePair<int, int[]>(stage + 1, new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }));

                //save curr move
                back.Push(new KeyValuePair<int, int[]>(stage, currIndex));

                //if win, stop DFS
                if (curr_state_string == goal_state)
                    return right_move;
            }

            return null;
        }


        private bool IsCorrectSide(int n, int[,] curr_state, ref HashSet<int> locked)
        {
            //when 1 2 5 6 9 13 in in the left side, they are in the correct side
            //when 3 4 7 83 in in the right side, they are in the correct side
            switch (n)
            {
                //left side
                case 1:
                case 2:
                    for (int i = 0; i < curr_state.GetLength(0); i++)
                        for (int j = 0; j < curr_state.GetLength(1) / 2; j++)
                            if (curr_state[i, j] == n)
                                return true;
                    break;
                //left side
                case 5:
                case 6:
                    //if 1 is not locked, 5 & 6 can be moved to the "wrong side". (so that we won't block 1 & 2)
                    if (!locked.Contains(1))
                        return false;

                    for (int i = 0; i < curr_state.GetLength(0); i++)
                        for (int j = 0; j < curr_state.GetLength(1) / 2; j++)
                            if (curr_state[i, j] == n)
                                return true;
                    break;
                //right side
                case 3:
                case 4:
                    //right side
                    for (int i = 0; i < curr_state.GetLength(0); i++)
                        for (int j = curr_state.GetLength(1) / 2; j < curr_state.GetLength(1); j++)
                            if (curr_state[i, j] == n)
                                return true;
                    break;
                case 7:
                case 8:
                    //if 1 is not locked, 5 & 6 can be moved to the "wrong side". (so that we won't block 1 & 2)
                    if (!IsCorrectSide(3,curr_state,ref locked) || !IsCorrectSide(4, curr_state, ref locked))
                        return false;
                    for (int i = 0; i < curr_state.GetLength(0); i++)
                        for (int j = curr_state.GetLength(1) / 2; j < curr_state.GetLength(1); j++)
                            if (curr_state[i, j] == n)
                                return true;
                    break;
                default:
                    return true;
            }

            return false;
        }

        private bool lockBlock(int n, string curr_state_string, ref HashSet<int> locked)
        {
            // if curr block is already locked, we don't want to lock it again, so return false
            if (locked.Contains(n))
                return false;

            //this is used to check if we lock the curr block
            bool blockLocked = false;

            //convert curr_state_string to an array
            int[] curr_state = new int[17];
            int i;
            for (i = 0; i < curr_state.Length; i++)
                curr_state[i] = 0;
            i = 1;
            foreach (char c in curr_state_string)
            {
                if (c == ' ')
                    i++;
                else
                {
                    curr_state[i] *= 10;
                    curr_state[i] += c - '0';
                }
            }

            //check if curr block can be locked
            switch (n)
            {
                //1 & 2 can be locked when both of them are in the correct cell
                case 1:
                case 2:
                    if (curr_state[1] == 1 && curr_state[2] == 2)
                    {
                        blockLocked = true;
                        locked.Add(1);
                        locked.Add(2);
                    }
                    break;
                //3 & 4 can be locked when both of them are in the correct cell
                case 3:
                case 4:
                    if (curr_state[3] == 3 && curr_state[4] == 4)
                    {
                        blockLocked = true;
                        locked.Add(3);
                        locked.Add(4);
                    }
                    break;
                //5 & 6 can be locked when both of them are in the correct cell and 1 & 2 is already locked (if 1 & 2 is not locked, and we lock 5 & 6, 1 & 2 can't be locked in anyways)
                case 5:
                case 6:
                    if (curr_state[5] == 5 && curr_state[6] == 6 && locked.Contains(1))
                    {
                        blockLocked = true;
                        locked.Add(5);
                        locked.Add(6);
                    }
                    break;
                //7 & 8 can be locked when both of them are in the correct cell and 3 & 4 is already locked (same concept as 5 & 6) 
                case 7:
                case 8:
                    if (curr_state[7] == 7 && curr_state[8] == 8 && locked.Contains(3))
                    {
                        blockLocked = true;
                        locked.Add(7);
                        locked.Add(8);
                    }
                    break;
                //9 & 13 can be locked when both of them are in the correct cell and 5 & 6 is already locked (same concept as 5 & 6) 
                case 9:
                case 13:
                    if (curr_state[9] == 9 && curr_state[13] == 13 && locked.Contains(5))
                    {
                        blockLocked = true;
                        locked.Add(9);
                        locked.Add(13);
                    }
                    break;
                default:
                    break;
            }

            return blockLocked;
        }

    }
}
