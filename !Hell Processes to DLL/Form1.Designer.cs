namespace _Hell_Processes_to_DLL
{
    partial class fIndex
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
            System.Windows.Forms.ListViewGroup listViewGroup13 = new System.Windows.Forms.ListViewGroup("New items", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup14 = new System.Windows.Forms.ListViewGroup("Unchecked", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup15 = new System.Windows.Forms.ListViewGroup("Checked", System.Windows.Forms.HorizontalAlignment.Left);
            this.bStart = new System.Windows.Forms.Button();
            this.lvProcesses = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lStatus = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(129, 517);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(551, 33);
            this.bStart.TabIndex = 1;
            this.bStart.Text = "Save";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // lvProcesses
            // 
            this.lvProcesses.CheckBoxes = true;
            this.lvProcesses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvProcesses.FullRowSelect = true;
            listViewGroup13.Header = "New items";
            listViewGroup13.Name = "lvgNew";
            listViewGroup14.Header = "Unchecked";
            listViewGroup14.Name = "lvgUnchecked";
            listViewGroup15.Header = "Checked";
            listViewGroup15.Name = "lvgChecked";
            this.lvProcesses.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup13,
            listViewGroup14,
            listViewGroup15});
            this.lvProcesses.Location = new System.Drawing.Point(12, 12);
            this.lvProcesses.Name = "lvProcesses";
            this.lvProcesses.Size = new System.Drawing.Size(760, 499);
            this.lvProcesses.TabIndex = 2;
            this.lvProcesses.UseCompatibleStateImageBehavior = false;
            this.lvProcesses.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Description";
            this.columnHeader3.Width = 430;
            // 
            // lStatus
            // 
            this.lStatus.Location = new System.Drawing.Point(12, 517);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(111, 33);
            this.lStatus.TabIndex = 3;
            this.lStatus.Text = "label1";
            this.lStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(687, 518);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 32);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // fIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lStatus);
            this.Controls.Add(this.lvProcesses);
            this.Controls.Add(this.bStart);
            this.Name = "fIndex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "!Hell Processes to DLL";
            this.Load += new System.EventHandler(this.fIndex_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.ListView lvProcesses;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
    }
}

