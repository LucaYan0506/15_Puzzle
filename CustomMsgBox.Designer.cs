
namespace _15_Puzzle
{
    partial class CustomMsgBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.begAiBtn = new System.Windows.Forms.Button();
            this.advAiBtn = new System.Windows.Forms.Button();
            this.begAITip = new System.Windows.Forms.ToolTip(this.components);
            this.advAITip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose one of the following options:";
            // 
            // begAiBtn
            // 
            this.begAiBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.begAiBtn.Location = new System.Drawing.Point(11, 51);
            this.begAiBtn.Margin = new System.Windows.Forms.Padding(2);
            this.begAiBtn.Name = "begAiBtn";
            this.begAiBtn.Size = new System.Drawing.Size(118, 33);
            this.begAiBtn.TabIndex = 20;
            this.begAiBtn.Text = "Beginner AI (Estimate time)";
            this.begAITip.SetToolTip(this.begAiBtn, "Estimate time is 10 min");
            this.begAiBtn.UseVisualStyleBackColor = true;
            this.begAiBtn.Click += new System.EventHandler(this.begAiBtn_Click);
            // 
            // advAiBtn
            // 
            this.advAiBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.advAiBtn.Location = new System.Drawing.Point(193, 51);
            this.advAiBtn.Margin = new System.Windows.Forms.Padding(2);
            this.advAiBtn.Name = "advAiBtn";
            this.advAiBtn.Size = new System.Drawing.Size(119, 33);
            this.advAiBtn.TabIndex = 21;
            this.advAiBtn.Text = "Advanced AI";
            this.advAITip.SetToolTip(this.advAiBtn, "Estimate time is 5 min");
            this.advAiBtn.UseVisualStyleBackColor = true;
            this.advAiBtn.Click += new System.EventHandler(this.advAiBtn_Click);
            // 
            // CustomMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 95);
            this.Controls.Add(this.advAiBtn);
            this.Controls.Add(this.begAiBtn);
            this.Controls.Add(this.label1);
            this.Name = "CustomMsgBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CustomMsgBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button begAiBtn;
        private System.Windows.Forms.ToolTip begAITip;
        internal System.Windows.Forms.Button advAiBtn;
        private System.Windows.Forms.ToolTip advAITip;
    }
}