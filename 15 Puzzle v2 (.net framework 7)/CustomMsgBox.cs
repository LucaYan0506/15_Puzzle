using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _15_Puzzle_v2__.net_framework_7_
{
    public partial class CustomMsgBox : Form
    {
        Form owner;
        public CustomMsgBox(Stack<KeyValuePair<int, int[]>> _AiMoves, Queue<int[]> AiMoves2, Form _owner)
        {
            Stack<KeyValuePair<int, int[]>> AiMoves = _AiMoves;
            InitializeComponent();
            begAITip.SetToolTip(begAiBtn, "Estimate time is " + (AiMoves.Count() / 375) + " min " + (int)((AiMoves.Count() / 375.0) % 1 * 60) + " sec");
            advAITip.SetToolTip(advAiBtn, "Estimate time is " + (AiMoves2.Count() / 375 + 2) + " min " + (int)((AiMoves2.Count() / 375.0) % 1 * 60) + " sec");
            owner = _owner;
        }


        private void ShowDialogCentered(Form owner)
        {
            Rectangle ownerRect = GetOwnerRect(this, owner);
            Action action = () => this.Location = new Point(ownerRect.Left + (ownerRect.Width - this.Width) / 2,
                                     ownerRect.Top + (ownerRect.Height - this.Height) / 2);
            this.Invoke(action);
        }

        private Rectangle GetOwnerRect(Form frm, Form owner)
        {
            return owner != null ? owner.DesktopBounds : Screen.GetWorkingArea(frm);
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

        private void CustomMsgBox_Load(object sender, EventArgs e)
        {
            ShowDialogCentered(owner);
        }
    }
}
