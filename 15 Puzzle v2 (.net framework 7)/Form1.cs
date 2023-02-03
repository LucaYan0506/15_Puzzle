using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;

namespace _15_Puzzle_v2__.net_framework_7_
{
    public partial class Form1 : Form
    {
        //global variable
        Grid grid;
        bool freezeGame = true;
        bool AI_is_working = false;

        public Form1()
        {
            InitializeComponent();

            //add items to the combobox of the datagridview
            DataGridViewComboBoxColumn theColumn = (DataGridViewComboBoxColumn)this.dataGridView1.Columns[1];
            theColumn.Items.Add("Solved by user");
            theColumn.Items.Add("Solved by AI");

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
            for (int i = 0; i < 15; i++)
            {
                int i1 = rnd.Next(0, list_c.Count());
                int i2 = rnd.Next(0, list_c.Count());

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
                //if c is not in the correct cell
                if (int.Parse(c.Text) - 1 != (button_index[0] * 4 + button_index[1]))
                    grid.wrongCells++;
            }
        }

        private void swap(Control btn1, Control btn2)
        {
            //swap location of btn1 & btn2
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
            if (freezeGame && !AI_is_working)
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

                    //add a timer
                    //move the button (in the GUI)
                    int distance = 10;//speed to move the block is 160 ms

                    while (distance-- > 0)
                    {
                        button.Location = new Point(button.Location.X + dirs[i, 1] * 10, button.Location.Y + dirs[i, 0] * 10);
                        System.Threading.Thread.Sleep(10);
                    }
                    //check if the user win 
                    if (grid.Win() && !AI_is_working)
                    {
                        MessageBox.Show("Win");
                        //save the result into the table
                        DataGridViewComboBoxColumn theColumn = (DataGridViewComboBoxColumn)this.dataGridView1.Columns[1];

                        //if user is playing stop the timer
                        refreshList();
                        dataGridView1.Rows.Add(time_lbl.Text, theColumn.Items[0]);
                        updateFile();
                        start_btn.PerformClick();
                    }

                }
            }
        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            //if AI is working, stop it
            if (AI_is_working)
            {
                AI_btn.PerformClick();
                AI_is_working = false;
            }

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
            //make sure that return 2 digits. E.g. "01", instead of "1"
            if (n < 10)
                return "0" + (char)(n + '0');


            return n.ToString();
        }

        private void AI_btn_Click(object sender, EventArgs e)
        {
            //start AI (in the background)
            if (!backgroundWorkerAI.IsBusy)
            {
                //if user is playing, stop the timer
                if (start_btn.Text == "Stop")
                    start_btn.PerformClick();

                //set AI_is_working = true (since AI starts to work)
                AI_is_working = true;
                backgroundWorkerAI.RunWorkerAsync();
            }
            else //stop AI
                AI_is_working = false;
        }

        private void restart_btn_Click(object sender, EventArgs e)
        {
            //reset timer
            time_lbl.Text = "00:00:00";
            //stop timer
            timer1.Stop();

            //if the game started, stop it
            if (!freezeGame)
                start_btn.PerformClick();

            //re-generate the game
            InitGame();
        }

        private void backgroundWorkerAI_DoWork(object sender, DoWorkEventArgs e)
        {
            //if the grid is not shuffled, stop it 
            if (grid.Win())
            {
                MessageBox.Show("Win");
                return;
            }

            //add a timer
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            //start AI (beginning and advanced)
            Action action;
            AI ai = new AI();
            Stack<KeyValuePair<int, int[]>> AiMoves = ai.DFS2(ref grid, ref panel1);
            Queue<int[]> AiMoves2 = ai.DFS3(ref grid, ref panel1);
            CustomMsgBox customMsgBox = new CustomMsgBox(AiMoves, AiMoves2, this);
            var result = customMsgBox.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (customMsgBox.Result == "beginner")
                {
                    AI_is_working = true;
                    Button[,] blocks = new Button[grid.isEmpty.GetLength(0), grid.isEmpty.GetLength(1)];
                    //init blocks based on the panel (GUI)
                    int[] emptyCell = new int[2];

                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            blocks[i, j] = null;

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

                    Stack<KeyValuePair<int, int[]>> right_move_reversed = new Stack<KeyValuePair<int, int[]>>();
                    while (AiMoves.Count > 0)
                        right_move_reversed.Push(AiMoves.Pop());

                    //start to move buttons (based on the right_move)
                    foreach (var x in right_move_reversed)
                    {
                        if (!AI_is_working)
                            break;
                        //move block (in the GUI)
                        action = () => blocks[x.Value[0], x.Value[1]].PerformClick();
                        blocks[x.Value[0], x.Value[1]].Invoke(action);
                        //move block (in the 2d array)
                        blocks[emptyCell[0], emptyCell[1]] = blocks[x.Value[0], x.Value[1]];
                        blocks[x.Value[0], x.Value[1]] = null;

                        //update empty cell
                        emptyCell = x.Value;
                    }
                    //stop timer
                    timer.Stop();
                    AI_is_working = false;

                    //update time
                    TimeSpan timeTaken = timer.Elapsed;
                    action = () => time_lbl.Text = string.Format("{0}:{1}:{2}", NumberToTime(timeTaken.Hours), NumberToTime(timeTaken.Minutes), NumberToTime(timeTaken.Seconds));
                    time_lbl.Invoke(action);

                }
                else if (customMsgBox.Result == "advanced")
                {
                    //declare currstate
                    string currState = "";

                    AI_is_working = true;
                    Button[,] blocks = new Button[grid.isEmpty.GetLength(0), grid.isEmpty.GetLength(1)];
                    //init blocks based on the panel (GUI)
                    int[] emptyCell = new int[2];

                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            blocks[i, j] = null;

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

                    //start to move buttons (based on the right_move)
                    while (AiMoves2.Count > 0)
                    {
                        if (!AI_is_working)
                            return;

                        var x = AiMoves2.Dequeue();

                        //move block (in the GUI)
                        action = () => blocks[x[0], x[1]].PerformClick();
                        blocks[x[0], x[1]].Invoke(action);
                        //move block (in the 2d array)
                        blocks[emptyCell[0], emptyCell[1]] = blocks[x[0], x[1]];
                        blocks[x[0], x[1]] = null;

                        //update empty cell
                        emptyCell = x;
                    }

                    Stack<KeyValuePair<int, int>> rightMoves = ai.A_STAR(ref grid, ref panel1);

                    //avoid repeat moves
                    HashSet<string> visited = new HashSet<string>();
                    //store possible combination/moves
                    Stack<KeyValuePair<int, int[]>> st = new Stack<KeyValuePair<int, int[]>>();
                    //store previous combination/moves
                    Stack<KeyValuePair<int, int[]>> back = new Stack<KeyValuePair<int, int[]>>();

                    //init st (start from 4 directions of the emptycell)
                    foreach (int[] dir in ai._dirs)
                        st.Push(new KeyValuePair<int, int[]>(0, new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }));

                    int count = 0;
                    while (rightMoves.Count == 0)
                    {
                        if (count++ == 10)
                        {
                            AiMoves2 = ai.DFS3(ref grid, ref panel1, true);

                            while (AiMoves2.Count > 0)
                            {
                                if (!AI_is_working)
                                    break;

                                var x = AiMoves2.Dequeue();

                                //move block (in the GUI)
                                action = () => blocks[x[0], x[1]].PerformClick();
                                blocks[x[0], x[1]].Invoke(action);
                                //move block (in the 2d array)
                                blocks[emptyCell[0], emptyCell[1]] = blocks[x[0], x[1]];
                                blocks[x[0], x[1]] = null;

                                //update empty cell
                                emptyCell = x;
                            }

                            //stop timer
                            timer.Stop();
                            AI_is_working = false;

                            //update time
                            TimeSpan timeTaken = timer.Elapsed;
                            action = () => time_lbl.Text = string.Format("{0}:{1}:{2}", NumberToTime(timeTaken.Hours), NumberToTime(timeTaken.Minutes), NumberToTime(timeTaken.Seconds));
                            time_lbl.Invoke(action);
                            break;
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            //pop st
                            var curr = st.Pop();
                            int stage = curr.Key;
                            int[] currIndex = curr.Value;

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
                                        action = () => blocks[backIndex[0], backIndex[1]].PerformClick();
                                        blocks[backIndex[0], backIndex[1]].Invoke(action);
                                        blocks[emptyCell[0], emptyCell[1]] = blocks[backIndex[0], backIndex[1]];
                                        blocks[backIndex[0], backIndex[1]] = null;

                                        emptyCell = backIndex;
                                    }

                                }
                            }

                            //if currindex is outside the grid, skip it
                            if (grid.OutOfBound(currIndex) || int.Parse(blocks[currIndex[0], currIndex[1]].Text) < 9)
                            {
                                i--;
                                continue;
                            }

                            //move block (in the 2d array)
                            blocks[emptyCell[0], emptyCell[1]] = blocks[currIndex[0], currIndex[1]];
                            blocks[currIndex[0], currIndex[1]] = null;

                            //update currstate
                            currState = "";
                            foreach (var block in blocks)
                                if (block != null)
                                    currState += block.Text + " ";
                                else
                                    currState += "16 ";

                            if (visited.Contains(currState))
                            {
                                //move block back (in the 2d array)
                                blocks[currIndex[0], currIndex[1]] = blocks[emptyCell[0], emptyCell[1]];
                                blocks[emptyCell[0], emptyCell[1]] = null;

                                //skip curr move
                                i--;
                                continue;
                            }

                            //add curr_state into visited, so, later on, we know if the curr_state is visited.
                            visited.Add(currState);

                            //move block (in the GUI)
                            action = () => blocks[emptyCell[0], emptyCell[1]].PerformClick();
                            blocks[emptyCell[0], emptyCell[1]].Invoke(action);

                            //now emptyCell = currIndex 
                            //currIndex = emptycell because 2 buttons are swapped
                            var temp2 = currIndex;
                            currIndex = emptyCell;
                            emptyCell = temp2;

                            //other possible combinations
                            foreach (int[] dir in ai._dirs)
                                if (!grid.OutOfBound(new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }))
                                    st.Push(new KeyValuePair<int, int[]>(stage + 1, new int[] { emptyCell[0] + dir[0], emptyCell[1] + dir[1] }));

                            //save curr move
                            back.Push(new KeyValuePair<int, int[]>(stage, currIndex));
                        }

                        //if win, stop DFS
                        rightMoves = ai.A_STAR(ref grid, ref panel1);
                    }

                    if (AI_is_working && rightMoves.Count > 0)
                    {
                        //reset blocks
                        for (int i = 0; i < 4; i++)
                            for (int j = 0; j < 4; j++)
                                blocks[i, j] = null;

                        foreach (Button control in panel1.Controls)
                        {
                            int[] index = grid.LocationToIndex(control.Location);
                            blocks[index[0], index[1]] = control;
                        }

                        //start to move buttons (based on the right_move)
                        while (rightMoves.Count > 0)
                        {
                            KeyValuePair<int, int> x = rightMoves.Pop();
                            if (!AI_is_working)
                                break;
                            //move block (in the GUI)
                            action = () => blocks[x.Key, x.Value].PerformClick();
                            blocks[x.Key, x.Value].Invoke(action);
                            //move block (in the 2d array)
                            blocks[emptyCell[0], emptyCell[1]] = blocks[x.Key, x.Value];
                            blocks[x.Key, x.Value] = null;

                            //update empty cell
                            emptyCell = new int[] { x.Key, x.Value };
                        }

                        //stop timer
                        timer.Stop();
                        AI_is_working = false;

                        //update time
                        TimeSpan timeTaken = timer.Elapsed;
                        action = () => time_lbl.Text = string.Format("{0}:{1}:{2}", NumberToTime(timeTaken.Hours), NumberToTime(timeTaken.Minutes), NumberToTime(timeTaken.Seconds));
                        time_lbl.Invoke(action);
                    }
                }
            }

            //if win, add to the records list
            if (grid.Win())
            {
                MessageBox.Show("Solved");
                refreshList();
                //update datagrid
                DataGridViewComboBoxColumn theColumn = (DataGridViewComboBoxColumn)dataGridView1.Columns[1];
                action = () => dataGridView1.Rows.Add(time_lbl.Text, theColumn.Items[1]);
                dataGridView1.Invoke(action);
                updateFile();
            }
        }

        private void refreshListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshList();
        }
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            //clear rows of selected cells
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                //if cell it's the last row (means that is an uncommitted new raw, so cannot be deleted)
                if (cell.RowIndex == dataGridView1.Rows.Count - 1)
                    continue;

                //double check with the user
                DialogResult result = MessageBox.Show("Are you sure to delete this row '" + cell.Value.ToString() + " (" + dataGridView1.Rows[cell.RowIndex].Cells[1].Value.ToString() + ")'", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    dataGridView1.Rows.RemoveAt(cell.RowIndex);//delete row
                    updateFile();
                }
            }
        }

        private void clearListBtn_Click(object sender, EventArgs e)
        {
            //double check with the user
            DialogResult result = MessageBox.Show("Are you sure to clear the list", "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //clear list
                dataGridView1.Rows.Clear();
                updateFile();
            }
        }

        private void refreshList()
        {
            Action action = () => dataGridView1.Rows.Clear();
            dataGridView1.Invoke(action);

            //refresh the list
            DataGridViewComboBoxColumn theColumn = (DataGridViewComboBoxColumn)dataGridView1.Columns[1];
            FileStream f2 = new FileStream("records.txt", FileMode.OpenOrCreate);
            StreamReader s2 = new StreamReader(f2);
            while (!s2.EndOfStream)
            {
                action = () => dataGridView1.Rows.Add(s2.ReadLine(), theColumn.Items[int.Parse(s2.ReadLine())]);
                dataGridView1.Invoke(action);
            }
            s2.Close();
            f2.Close();
        }

        private void updateFile()
        {
            FileStream f1 = new FileStream("records.txt", FileMode.Create);
            StreamWriter s1 = new StreamWriter(f1);
            foreach (DataGridViewRow x in dataGridView1.Rows)
            {
                if (x.Cells[0].Value is null)
                    continue;

                s1.WriteLine(x.Cells[0].Value);
                if (x.Cells[1].Value.ToString() == "Solved by user")
                    s1.WriteLine("0");
                else
                    s1.WriteLine("1");
            }
            s1.Close();
            f1.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //add data to datagridview
            refreshList();
        }

    }
}