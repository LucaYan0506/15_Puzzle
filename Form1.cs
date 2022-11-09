using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _15_Puzzle
{
    public partial class Form1 : Form
    {
        //global variable
        Grid grid;
        bool freezeGame = true;

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
                    locations[i * 4 + j] = new Point(100 * j,100 * i);

            //clear panel
            panel1.Controls.Clear();
            //genereate new blocks
            for (int i = 0; i < 15; i++)
            {
                Button cell = new Button();
                cell.Location = locations[i];
                cell.Name = "cell" + (i + 1);
                cell.Text= (i + 1).ToString();
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
            if (freezeGame)
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
                    int distance = 20;
                    while (distance-- > 0)
                    {
                        button.Location = new Point(button.Location.X + dirs[i,1] * 5, button.Location.Y + dirs[i,0] * 5);
                        System.Threading.Thread.Sleep(10);
                    }

                    //checkif the user win 
                    if (grid.Win())
                    {
                        MessageBox.Show("Win");
                        start_btn.PerformClick();
                    }

                }
            }




        }

        private void start_btn_Click(object sender, EventArgs e)
        {
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
            int sec = int.Parse(time_lbl.Text.Substring(6,2));
            int min = int.Parse(time_lbl.Text.Substring(3,2));
            int hour = int.Parse(time_lbl.Text.Substring(0,2));

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
    }
}
