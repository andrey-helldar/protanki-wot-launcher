namespace _Hell_Process_List
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
            this.bSave = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.clbProcesses = new System.Windows.Forms.CheckedListBox();
            this.bwLoad = new System.ComponentModel.BackgroundWorker();
            this.bwSave = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(67, 523);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(157, 37);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "Сохранить данные";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bClose
            // 
            this.bClose.Location = new System.Drawing.Point(232, 523);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(157, 37);
            this.bClose.TabIndex = 1;
            this.bClose.Text = "Выход";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // clbProcesses
            // 
            this.clbProcesses.FormattingEnabled = true;
            this.clbProcesses.Location = new System.Drawing.Point(12, 12);
            this.clbProcesses.Name = "clbProcesses";
            this.clbProcesses.Size = new System.Drawing.Size(456, 499);
            this.clbProcesses.Sorted = true;
            this.clbProcesses.TabIndex = 2;
            // 
            // bwLoad
            // 
            this.bwLoad.WorkerReportsProgress = true;
            this.bwLoad.WorkerSupportsCancellation = true;
            this.bwLoad.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwLoad_RunWorkerCompleted);
            // 
            // bwSave
            // 
            this.bwSave.WorkerReportsProgress = true;
            this.bwSave.WorkerSupportsCancellation = true;
            this.bwSave.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwSave_DoWork);
            this.bwSave.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwSave_RunWorkerCompleted);
            // 
            // fIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 572);
            this.Controls.Add(this.clbProcesses);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fIndex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "!Hell Process List";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.CheckedListBox clbProcesses;
        private System.ComponentModel.BackgroundWorker bwLoad;
        private System.ComponentModel.BackgroundWorker bwSave;
    }
}

