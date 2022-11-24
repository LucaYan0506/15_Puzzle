using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace _15_Puzzle
{
    public partial class Form1 : Form
    {
        //global variable
        Grid grid;
        bool freezeGame = true;
        bool AI = false;
        bool stopDFS = false;

        //get 4 directions 
        int[][] dirs = new int[][]
        {
            new int[]{ 1,0 },   //top
            new int[]{ 0,-1 },  //right
            new int[]{ 0,1 },   //left
            new int[]{ -1,0 },  //bottom    
        };

        int[,] test = new int[,]{
            { 1,2,3 },
            { 4,5,0 },
            { 7,8,9 },
        };

        public Form1()
        {
            InitializeComponent();

            //add items to the combobox of the datagridview
            DataGridViewComboBoxColumn theColumn = (DataGridViewComboBoxColumn)this.dataGridView1.Columns[1];
            theColumn.Items.Add("Solved");
            theColumn.Items.Add("Not Solved");
            theColumn.Items.Add("Solved by AI");

            //add data to datagridview
            dataGridView1.Rows.Add("00:00:05", theColumn.Items[0]);
            dataGridView1.Rows.Add("00:20:15", theColumn.Items[1]);
            dataGridView1.Rows.Add("00:12:05", theColumn.Items[2]);
            dataGridView1.Rows.Add("00:45:05", theColumn.Items[1]);

            InitGame();
        }
        private void InitGame()
        {
            //create a grid that tell me if a cell is empty or not
            grid = new Grid();

            //generate locations for all blocks(buttons)
            Point[] locations = new Point[16];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    locations[i * 4 + j] = new Point(100 * j, 100 * i);

            //clear panel
            panel1.Controls.Clear();
            //genereate new blocks
            for (int i = 0; i < 15; i++)
            {
                Button cell = new Button();
                cell.Location = locations[i];
                cell.Name = "cell" + (i + 1);
                cell.Text = (i + 1).ToString();
                cell.Font = new Font("Times New Roman", 44F, FontStyle.Bold, GraphicsUnit.Point);
                cell.Size = new Size(100, 100);
                cell.TabIndex = i + 1;
                cell.BackColor = Color.FromArgb(149, 112, 79);
                panel1.Controls.Add(cell);
                cell.BringToFront();
                cell.Click += Cell_Click;
            }

            //Shuffle the puzzle
            List<Control> list_c = panel1.Controls.Cast<Control>().OrderBy(el => el.TabIndex).ToList();
            Random rnd = new Random();
            for (int i = 0; i < list_c.Count(); i++)
            {
                int i1 = rnd.Next(1, list_c.Count());
                int i2 = rnd.Next(1, list_c.Count());
                //swap thier position
                swap(list_c[i1], list_c[i2]);

                //swap in the list
                Control temp = list_c[i1];
                list_c[i1] = list_c[i2];
                list_c[i2] = temp;
            }

            //if it is not solvable, spaw first 2
            if (!Solvable(list_c))
                swap(list_c[0], list_c[1]);

            //update grid
            foreach (Control c in list_c)
            {
                int[] button_index = grid.LocationToIndex(c.Location);
                if (int.Parse(c.Text) - 1 != (button_index[0] * 4 + button_index[1]))
                    grid.wrongCells++;
            }
        }

        private void swap(Control btn1, Control btn2)
        {
            Point temp = btn1.Location;
            btn1.Location = btn2.Location;
            btn2.Location = temp;
        }

        private bool Solvable(List<Control> list_c)
        {
            //parity algo
            int n = 0;
            for (int i = 0; i < list_c.Count; i++)
                for (int j = i + 1; j < list_c.Count; j++)
                    if (int.Parse(list_c[j].Text) < int.Parse(list_c[i].Text))
                        n++;

            return n % 2 == 0;
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            //if the game is not started, start the game
            if (freezeGame && !AI)
                start_btn.PerformClick();

            //get button and his indexes
            Button button = (Button)sender;
            int[] button_index = grid.LocationToIndex(button.Location);

            int[,] dirs = new int[,]{
                { -1,0 },   //left
                { 1,0 },    //right   
                { 0,-1 },   //top
                { 0,1 },    //bottom
            };

            //check if the button can go one of the directions
            for (int i = 0; i < 4; i++)
            {
                int[] curr_index = new int[]{
                    button_index[0] + dirs[i,0],
                    button_index[1] + dirs[i, 1]
                };

                //if button can move to this direction 
                if (!grid.OutOfBound(curr_index) && grid.isEmpty[curr_index[0], curr_index[1]])
                {
                    //update grid & number of wrong cells
                    grid.isEmpty[curr_index[0], curr_index[1]] = false;
                    if (int.Parse(button.Text) - 1 == (button_index[0] * 4 + button_index[1]))
                        grid.wrongCells++;
                    grid.isEmpty[button_index[0], button_index[1]] = true;
                    if (int.Parse(button.Text) - 1 == (curr_index[0] * 4 + curr_index[1]))
                        grid.wrongCells--;

                    //move the button (in the GUI)
                    int distance = 10;//speed to move the block is 100 ms

                    while (distance-- > 0)
                    {
                        button.Location = new Point(button.Location.X + dirs[i, 1] * 10, button.Location.Y + dirs[i, 0] * 10);
                        System.Threading.Thread.Sleep(10);
                    }

                    //checkif the user win 
                    if (grid.Win())
                    {
                        MessageBox.Show("Win");
                        if (!AI)
                            start_btn.PerformClick();
                    }

                }
            }




        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            AI = false;

            //start the game
            if (start_btn.Text == "Start")
            {
                timer1.Start();
                start_btn.Text = "Stop";
                freezeGame = false;
            }
            //stop the game
            else
            {
                timer1.Stop();
                start_btn.Text = "Start";
                freezeGame = true;
            }
        }

        //update the time
        private void timer1_Tick(object sender, EventArgs e)
        {
            int sec = int.Parse(time_lbl.Text.Substring(6, 2));
            int min = int.Parse(time_lbl.Text.Substring(3, 2));
            int hour = int.Parse(time_lbl.Text.Substring(0, 2));

            sec++;

            if (sec == 60)
            {
                sec = 0;
                min++;
            }
            if (min == 60)
            {
                min = 0;
                hour++;
            }

            time_lbl.Text = String.Format("{0}:{1}:{2}", NumberToTime(hour), NumberToTime(min), NumberToTime(sec));
        }

        string NumberToTime(int n)
        {
            if (n < 10)
                return "0" + (char)(n + '0');


            return n.ToString();
        }

        private void AI_btn_Click(object sender, EventArgs e)
        {
            if (!backgroundWorkerAI.IsBusy)
            {
                if (start_btn.Text == "Stop")
                    start_btn.PerformClick();
                stopDFS = false;
                AI = true;
                backgroundWorkerAI.RunWorkerAsync();
            }
            else
            {
                stopDFS = true;
                AI = false;
            }
        }

        private void DFS(ref HashSet<string> visited, int[] emptyCell, ref int[,] curr_state, Button[,] blocks, int[] currIndex, int stage)
        {
            if (grid.OutOfBound(currIndex))
                return;

            int temp = curr_state[currIndex[0], currIndex[1]];
            curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
            curr_state[emptyCell[0], emptyCell[1]] = temp;

            string curr_state_string = "";

            foreach (int x in curr_state)
                curr_state_string += x + " ";

            if (visited.Contains(curr_state_string))
            {
                temp = curr_state[currIndex[0], currIndex[1]];
                curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                curr_state[emptyCell[0], emptyCell[1]] = temp;
                return;
            }

            visited.Add(curr_state_string);
            //now emptyCell = currIndex 
            //currIndex = emptycell because 2 buttons are swapped
            Action action = () => blocks[currIndex[0], currIndex[1]].PerformClick();
            blocks[currIndex[0], currIndex[1]].Invoke(action);
            blocks[emptyCell[0], emptyCell[1]] = blocks[currIndex[0], currIndex[1]];
            blocks[currIndex[0], currIndex[1]] = null;

            Console.WriteLine(stage);

            foreach (int[] dir in dirs)
                if (!grid.Win())
                    DFS(ref visited, currIndex, ref curr_state, blocks, new int[] { dir[0] + currIndex[0], dir[1] + currIndex[1] }, stage + 1);

            if (!grid.Win())
            {
                temp = curr_state[currIndex[0], currIndex[1]];
                curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                curr_state[emptyCell[0], emptyCell[1]] = temp;
                action = () => blocks[emptyCell[0], emptyCell[1]].PerformClick();
                blocks[emptyCell[0], emptyCell[1]].Invoke(action);
                blocks[currIndex[0], currIndex[1]] = blocks[emptyCell[0], emptyCell[1]];
                blocks[emptyCell[0], emptyCell[1]] = null;
            }
        }

        private void dfs2()
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
            //locked.Add(1);
            //locked.Add(2);
            //locked.Add(3);
            //locked.Add(4);
            //locked.Add(5);
            //locked.Add(6);
            //locked.Add(7);
            //locked.Add(8);

            string curr_state_string = "";
            foreach (int x in curr_state)
                curr_state_string += x + " ";

            for (int i = 0; i < 14; i++)
                lockBlock(i, curr_state_string, ref locked);

            Stack<KeyValuePair<int, int[]>> st = new Stack<KeyValuePair<int, int[]>>();
            Stack<KeyValuePair<int, int[]>> back = new Stack<KeyValuePair<int, int[]>>();

            Action action;

            foreach (int[] dir in dirs)
                st.Push(new KeyValuePair<int, int[]>(0, new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }));

            while (st.Count > 0)
            {
                if (stopDFS)
                    return;

                var curr = st.Pop();
                int stage = curr.Key;
                int[] currIndex = curr.Value;

                if (grid.OutOfBound(currIndex))
                    continue;

                int temp = curr_state[currIndex[0], currIndex[1]];
                curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                curr_state[emptyCell[0], emptyCell[1]] = temp;


                curr_state_string = "";

                foreach (int x in curr_state)
                    curr_state_string += x + " ";

                if (visited.Contains(curr_state_string) || blocks[currIndex[0], currIndex[1]] == null || locked.Contains(int.Parse(blocks[currIndex[0], currIndex[1]].Text)))
                {
                    temp = curr_state[currIndex[0], currIndex[1]];
                    curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                    curr_state[emptyCell[0], emptyCell[1]] = temp;
                    continue;
                }

                temp = curr_state[currIndex[0], currIndex[1]];
                curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                curr_state[emptyCell[0], emptyCell[1]] = temp;

                visited.Add(curr_state_string);
                if (lockBlock(int.Parse(blocks[currIndex[0], currIndex[1]].Text), curr_state_string, ref locked))
                {
                    st.Clear();
                    back.Clear();
                    visited.Clear();
                }


                if (back.Count > 0 && !grid.Win())
                {
                    if (stage <= back.Peek().Key)
                    {
                        while (back.Count > 0 && stage <= back.Peek().Key)
                        {
                            if (stopDFS)
                                return;

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

                Console.WriteLine(stage);



                action = () => blocks[currIndex[0], currIndex[1]].PerformClick();
                blocks[currIndex[0], currIndex[1]].Invoke(action);
                blocks[emptyCell[0], emptyCell[1]] = blocks[currIndex[0], currIndex[1]];
                blocks[currIndex[0], currIndex[1]] = null;
                temp = curr_state[currIndex[0], currIndex[1]];
                curr_state[currIndex[0], currIndex[1]] = curr_state[emptyCell[0], emptyCell[1]];
                curr_state[emptyCell[0], emptyCell[1]] = temp;

                //now emptyCell = currIndex 
                //currIndex = emptycell because 2 buttons are swapped
                var temp2 = currIndex;
                currIndex = emptyCell;
                emptyCell = temp2;


                foreach (int[] dir in dirs)
                    if (!grid.OutOfBound(new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }))
                        st.Push(new KeyValuePair<int, int[]>(stage + 1, new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }));


                back.Push(new KeyValuePair<int, int[]>(stage, currIndex));

                if (grid.Win())
                    return;
            }

        }

        private bool lockBlock(int n, string curr_state_string, ref HashSet<int> locked)
        {
            if (locked.Contains(n))
                return false;

            bool blockLocked = false;

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

            switch (n)
            {
                case 1:
                    if (curr_state[1] == 1 && curr_state[2] == 2)
                    {
                        blockLocked = true;
                        locked.Add(1);
                        locked.Add(2);
                    }
                    else if (curr_state[1] == 1 && !locked.Contains(4))
                    {
                        blockLocked = true;
                        locked.Add(1);
                    }
                    break;
                case 2:
                    if (curr_state[1] == 1 && curr_state[2] == 2)
                    {
                        blockLocked = true;
                        locked.Add(1);
                        locked.Add(2);
                    }
                    break;
                case 3:
                    if (curr_state[3] == 3 && curr_state[4] == 4)
                    {
                        blockLocked = true;
                        locked.Add(1);
                        locked.Add(2);
                    }
                    break;
                case 4:
                    if (curr_state[3] == 3 && curr_state[4] == 4)
                    {
                        blockLocked = true;
                        locked.Add(1);
                        locked.Add(2);
                    }
                    else if (curr_state[4] == 4 && !locked.Contains(1))
                    {
                        blockLocked = true;
                        locked.Add(4);
                    }
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9: case 13:
                    if (curr_state[9] == 9 && curr_state[13] == 13)
                    {
                        blockLocked = true;
                        locked.Add(9);
                        locked.Add(13);
                    }
                    break;
                default:
                    break;
            }

            if (locked.Contains(1) && locked.Contains(2) && locked.Contains(3) && locked.Contains(4))
                Console.WriteLine("");

            return blockLocked;
        }

        private void restart_btn_Click(object sender, EventArgs e)
        {
            time_lbl.Text = "00:00:00";
            timer1.Stop();
            if (!freezeGame)
                start_btn.PerformClick();

            InitGame();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            //clear rows of selected cells
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                //if cell it's the last row (means that is an uncommitted new raw, so cannot be deleted)
                if (cell.RowIndex == dataGridView1.Rows.Count - 1)
                    continue;
                //delete row
                dataGridView1.Rows.RemoveAt(cell.RowIndex);
            }
        }

        private void clearListBtn_Click(object sender, EventArgs e)
        {
            //clear list
            dataGridView1.Rows.Clear();
        }

        private void backgroundWorkerAI_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            if (grid.Win())
            {
                MessageBox.Show("Win");
                return;
            }

            dfs2();

            timer.Stop();
            AI = false;
            TimeSpan timeTaken = timer.Elapsed;
            Action action = () => time_lbl.Text = string.Format("{0}:{1}:{2}", NumberToTime(timeTaken.Hours), NumberToTime(timeTaken.Minutes), NumberToTime(timeTaken.Seconds));
            time_lbl.Invoke(action);
        }
    }
}
