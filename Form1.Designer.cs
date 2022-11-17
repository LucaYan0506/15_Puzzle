
namespace _15_Puzzle
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelBorder = new System.Windows.Forms.PictureBox();
            this.title_container = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.time_container = new System.Windows.Forms.PictureBox();
            this.time_lbl = new System.Windows.Forms.Label();
            this.restart_btn = new System.Windows.Forms.Button();
            this.AI_btn = new System.Windows.Forms.Button();
            this.start_btn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ListMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.clearListBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.backgroundWorkerAI = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.panelBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.title_container)).BeginInit();
            this.title_container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.time_container)).BeginInit();
            this.time_container.SuspendLayout();
            this.ListMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(56)))), ((int)(((byte)(40)))));
            this.panel1.Location = new System.Drawing.Point(66, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 400);
            this.panel1.TabIndex = 0;
            // 
            // panelBorder
            // 
            this.panelBorder.BackgroundImage = global::_15_Puzzle.Properties.Resources.wood_border2;
            this.panelBorder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBorder.Location = new System.Drawing.Point(0, 0);
            this.panelBorder.Name = "panelBorder";
            this.panelBorder.Size = new System.Drawing.Size(532, 532);
            this.panelBorder.TabIndex = 1;
            this.panelBorder.TabStop = false;
            // 
            // title_container
            // 
            this.title_container.BackgroundImage = global::_15_Puzzle.Properties.Resources.wood_border2;
            this.title_container.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.title_container.Controls.Add(this.title);
            this.title_container.Location = new System.Drawing.Point(538, 12);
            this.title_container.Name = "title_container";
            this.title_container.Size = new System.Drawing.Size(156, 61);
            this.title_container.TabIndex = 1;
            this.title_container.TabStop = false;
            // 
            // title
            // 
            this.title.BackColor = System.Drawing.Color.Transparent;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.ForeColor = System.Drawing.Color.White;
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(156, 61);
            this.title.TabIndex = 2;
            this.title.Text = "Timer";
            this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // time_container
            // 
            this.time_container.BackgroundImage = global::_15_Puzzle.Properties.Resources.wood_border2;
            this.time_container.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.time_container.Controls.Add(this.time_lbl);
            this.time_container.Location = new System.Drawing.Point(538, 85);
            this.time_container.Name = "time_container";
            this.time_container.Size = new System.Drawing.Size(156, 63);
            this.time_container.TabIndex = 2;
            this.time_container.TabStop = false;
            // 
            // time_lbl
            // 
            this.time_lbl.BackColor = System.Drawing.Color.Transparent;
            this.time_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.time_lbl.ForeColor = System.Drawing.Color.White;
            this.time_lbl.Location = new System.Drawing.Point(0, 0);
            this.time_lbl.Name = "time_lbl";
            this.time_lbl.Size = new System.Drawing.Size(156, 61);
            this.time_lbl.TabIndex = 2;
            this.time_lbl.Text = "00:00:00";
            this.time_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // restart_btn
            // 
            this.restart_btn.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.restart_btn.Location = new System.Drawing.Point(538, 408);
            this.restart_btn.Name = "restart_btn";
            this.restart_btn.Size = new System.Drawing.Size(156, 53);
            this.restart_btn.TabIndex = 3;
            this.restart_btn.Text = "New Game";
            this.restart_btn.UseVisualStyleBackColor = true;
            this.restart_btn.Click += new System.EventHandler(this.restart_btn_Click);
            // 
            // AI_btn
            // 
            this.AI_btn.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AI_btn.Location = new System.Drawing.Point(538, 467);
            this.AI_btn.Name = "AI_btn";
            this.AI_btn.Size = new System.Drawing.Size(156, 53);
            this.AI_btn.TabIndex = 4;
            this.AI_btn.Text = "Complete by AI";
            this.AI_btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AI_btn.UseVisualStyleBackColor = true;
            this.AI_btn.Click += new System.EventHandler(this.AI_btn_Click);
            // 
            // start_btn
            // 
            this.start_btn.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_btn.Location = new System.Drawing.Point(538, 349);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(156, 53);
            this.start_btn.TabIndex = 5;
            this.start_btn.Text = "Start";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.start_btn_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ListMenu
            // 
            this.ListMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteBtn,
            this.clearListBtn});
            this.ListMenu.Name = "ListMenu";
            this.ListMenu.Size = new System.Drawing.Size(177, 48);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(176, 22);
            this.DeleteBtn.Text = "Delete selected row";
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // clearListBtn
            // 
            this.clearListBtn.Name = "clearListBtn";
            this.clearListBtn.Size = new System.Drawing.Size(176, 22);
            this.clearListBtn.Text = "Clear List";
            this.clearListBtn.Click += new System.EventHandler(this.clearListBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.time,
            this.status});
            this.dataGridView1.ContextMenuStrip = this.ListMenu;
            this.dataGridView1.Location = new System.Drawing.Point(538, 157);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(156, 186);
            this.dataGridView1.TabIndex = 8;
            // 
            // time
            // 
            this.time.FillWeight = 98.47716F;
            this.time.HeaderText = "time";
            this.time.Name = "time";
            // 
            // status
            // 
            this.status.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.status.FillWeight = 101.5228F;
            this.status.HeaderText = "Status";
            this.status.MinimumWidth = 100;
            this.status.Name = "status";
            this.status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // backgroundWorkerAI
            // 
            this.backgroundWorkerAI.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerAI_DoWork);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(591, 274);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 532);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.start_btn);
            this.Controls.Add(this.AI_btn);
            this.Controls.Add(this.restart_btn);
            this.Controls.Add(this.time_container);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelBorder);
            this.Controls.Add(this.title_container);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.panelBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.title_container)).EndInit();
            this.title_container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.time_container)).EndInit();
            this.time_container.ResumeLayout(false);
            this.ListMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox panelBorder;
        private System.Windows.Forms.PictureBox title_container;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.PictureBox time_container;
        private System.Windows.Forms.Label time_lbl;
        private System.Windows.Forms.Button restart_btn;
        private System.Windows.Forms.Button AI_btn;
        private System.Windows.Forms.Button start_btn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip ListMenu;
        private System.Windows.Forms.ToolStripMenuItem DeleteBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewComboBoxColumn status;
        private System.Windows.Forms.ToolStripMenuItem clearListBtn;
        private System.ComponentModel.BackgroundWorker backgroundWorkerAI;
        private System.Windows.Forms.Button button1;
    }
}

