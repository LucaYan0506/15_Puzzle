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

        public Form1()
        {
            InitializeComponent();
            InitGame();
        }
        private void InitGame()
        {
            grid = new Grid();

            Point[] locations = new Point[16];

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    locations[i * 4 + j] = new Point(100 * j,100 * i);

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
            for (int i = 0; i < list_c.Count(); i++)
            {
                Console.WriteLine(list_c[i].Name);
            }
            
        }


        private bool Solvable()
        {
            return true;   
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int[] button_index = grid.LocationToIndex(button.Location);

            int[,] dirs = new int[,]{
                { -1,0 },   //left
                { 1,0 },    //right   
                { 0,-1 },   //top
                { 0,1 },    //bottom
            };


            for (int i = 0; i < 4; i++)
            {
                int[] curr_index = new int[]{
                    button_index[0] + dirs[i,0],
                    button_index[1] + dirs[i, 1]
                };

                if (!grid.OutOfBound(curr_index) && grid.isEmpty[curr_index[0], curr_index[1]])
                {
                    grid.isEmpty[curr_index[0], curr_index[1]] = false;
                    if (int.Parse(button.Text) - 1 == (button_index[0] * 4 + button_index[1]))
                        grid.wrongCells --;
                    grid.isEmpty[button_index[0], button_index[1]] = true;
                    if (int.Parse(button.Text) - 1 == (curr_index[0] * 4 + curr_index[1]))
                        grid.wrongCells++;

                    int distance = 20;
                    while (distance-- > 0)
                    {
                        button.Location = new Point(button.Location.X + dirs[i,1] * 5, button.Location.Y + dirs[i,0] * 5);
                        System.Threading.Thread.Sleep(10);
                    }

                    if (grid.Win())
                        MessageBox.Show("Win");

                }
            }




        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            if (start_btn.Text == "Start")
            {
                timer1.Start();
                start_btn.Text = "Stop";
            }
            else
            {
                timer1.Stop();
                start_btn.Text = "Start";
            }
        }

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

    }
}
