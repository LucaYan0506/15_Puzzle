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
    public partial class CustomMsgBox : Form
    {
        public CustomMsgBox(Stack<KeyValuePair<int, int[]>> _AiMoves)
        {
            Stack<KeyValuePair<int, int[]>> AiMoves = _AiMoves;
            InitializeComponent();
            begAITip.SetToolTip(begAiBtn, "Estimate time is " + (AiMoves.Count() / 375) + " min " + (int)((AiMoves.Count() / 375.0) % 1 * 60) + " sec");
        }
        public string Result { get; set; }

        private void begAiBtn_Click(object sender, EventArgs e)
        {
            this.Result = "beginner";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void advAiBtn_Click(object sender, EventArgs e)
        {
            this.Result = "advanced";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
