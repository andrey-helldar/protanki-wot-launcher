namespace _Hell_PRO_Tanki_Launcher
{
    partial class fSettings
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
            this.bCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbForceClose = new System.Windows.Forms.CheckBox();
            this.cbAero = new System.Windows.Forms.CheckBox();
            this.cbKillProcesses = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbVideo = new System.Windows.Forms.CheckBox();
            this.cbNews = new System.Windows.Forms.CheckBox();
            this.llTitle = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvProcessesUser = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bwUserProcesses = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.llUserProcesses = new System.Windows.Forms.LinkLabel();
            this.llGlobalProcesses = new System.Windows.Forms.LinkLabel();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bSave
            // 
            this.bSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSave.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bSave.Location = new System.Drawing.Point(127, 428);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(150, 50);
            this.bSave.TabIndex = 1;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = false;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bCancel
            // 
            this.bCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bCancel.Location = new System.Drawing.Point(298, 428);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(150, 50);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = false;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.cbForceClose);
            this.groupBox2.Controls.Add(this.cbAero);
            this.groupBox2.Controls.Add(this.cbKillProcesses);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 31);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(318, 93);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Оптимизация ПК:";
            // 
            // cbForceClose
            // 
            this.cbForceClose.AutoSize = true;
            this.cbForceClose.Location = new System.Drawing.Point(18, 42);
            this.cbForceClose.Name = "cbForceClose";
            this.cbForceClose.Size = new System.Drawing.Size(227, 17);
            this.cbForceClose.TabIndex = 2;
            this.cbForceClose.Text = "Принудительно завершать приложения";
            this.cbForceClose.UseVisualStyleBackColor = true;
            // 
            // cbAero
            // 
            this.cbAero.AutoSize = true;
            this.cbAero.Location = new System.Drawing.Point(18, 65);
            this.cbAero.Name = "cbAero";
            this.cbAero.Size = new System.Drawing.Size(246, 17);
            this.cbAero.TabIndex = 1;
            this.cbAero.Text = "Отключать Windows Aero при запуске игры";
            this.cbAero.UseVisualStyleBackColor = true;
            // 
            // cbKillProcesses
            // 
            this.cbKillProcesses.AutoSize = true;
            this.cbKillProcesses.Location = new System.Drawing.Point(18, 19);
            this.cbKillProcesses.Name = "cbKillProcesses";
            this.cbKillProcesses.Size = new System.Drawing.Size(240, 17);
            this.cbKillProcesses.TabIndex = 0;
            this.cbKillProcesses.Text = "Закрывать приложения при запуске игры";
            this.cbKillProcesses.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.cbVideo);
            this.groupBox3.Controls.Add(this.cbNews);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(336, 31);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(249, 93);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Другие:";
            // 
            // cbVideo
            // 
            this.cbVideo.AutoSize = true;
            this.cbVideo.Checked = true;
            this.cbVideo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbVideo.Location = new System.Drawing.Point(19, 55);
            this.cbVideo.Name = "cbVideo";
            this.cbVideo.Size = new System.Drawing.Size(165, 17);
            this.cbVideo.TabIndex = 1;
            this.cbVideo.Text = "Уведомлять о новых видео";
            this.cbVideo.UseVisualStyleBackColor = true;
            // 
            // cbNews
            // 
            this.cbNews.AutoSize = true;
            this.cbNews.Checked = true;
            this.cbNews.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNews.Location = new System.Drawing.Point(19, 32);
            this.cbNews.Name = "cbNews";
            this.cbNews.Size = new System.Drawing.Size(181, 17);
            this.cbNews.TabIndex = 0;
            this.cbNews.Text = "Уведомлять о новых новостях";
            this.cbNews.UseVisualStyleBackColor = true;
            // 
            // llTitle
            // 
            this.llTitle.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llTitle.AutoSize = true;
            this.llTitle.BackColor = System.Drawing.Color.Transparent;
            this.llTitle.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.llTitle.Font = new System.Drawing.Font("Sochi2014", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llTitle.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llTitle.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llTitle.Location = new System.Drawing.Point(12, 3);
            this.llTitle.Name = "llTitle";
            this.llTitle.Size = new System.Drawing.Size(80, 19);
            this.llTitle.TabIndex = 5;
            this.llTitle.TabStop = true;
            this.llTitle.Text = "Настройки...";
            this.llTitle.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.llGlobalProcesses);
            this.groupBox1.Controls.Add(this.llUserProcesses);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.lvProcessesUser);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 130);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(573, 292);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Какие процессы НЕЛЬЗЯ закрывать:";
            // 
            // lvProcessesUser
            // 
            this.lvProcessesUser.CheckBoxes = true;
            this.lvProcessesUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvProcessesUser.FullRowSelect = true;
            this.lvProcessesUser.GridLines = true;
            this.lvProcessesUser.Location = new System.Drawing.Point(6, 19);
            this.lvProcessesUser.MultiSelect = false;
            this.lvProcessesUser.Name = "lvProcessesUser";
            this.lvProcessesUser.ShowGroups = false;
            this.lvProcessesUser.Size = new System.Drawing.Size(561, 235);
            this.lvProcessesUser.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvProcessesUser.TabIndex = 1;
            this.lvProcessesUser.UseCompatibleStateImageBehavior = false;
            this.lvProcessesUser.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Процесс";
            this.columnHeader1.Width = 169;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Описание";
            this.columnHeader2.Width = 380;
            // 
            // bwUserProcesses
            // 
            this.bwUserProcesses.WorkerReportsProgress = true;
            this.bwUserProcesses.WorkerSupportsCancellation = true;
            this.bwUserProcesses.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwUserProcesses_DoWork);
            this.bwUserProcesses.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwUserProcesses_RunWorkerCompleted);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGreen;
            this.panel1.Location = new System.Drawing.Point(38, 260);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(25, 25);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Plum;
            this.panel2.Location = new System.Drawing.Point(324, 260);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(25, 25);
            this.panel2.TabIndex = 3;
            // 
            // llUserProcesses
            // 
            this.llUserProcesses.ActiveLinkColor = System.Drawing.Color.White;
            this.llUserProcesses.AutoSize = true;
            this.llUserProcesses.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llUserProcesses.LinkColor = System.Drawing.Color.White;
            this.llUserProcesses.Location = new System.Drawing.Point(72, 266);
            this.llUserProcesses.Name = "llUserProcesses";
            this.llUserProcesses.Size = new System.Drawing.Size(205, 13);
            this.llUserProcesses.TabIndex = 4;
            this.llUserProcesses.TabStop = true;
            this.llUserProcesses.Text = "Процессы, выбранные пользователем";
            this.llUserProcesses.VisitedLinkColor = System.Drawing.Color.White;
            // 
            // llGlobalProcesses
            // 
            this.llGlobalProcesses.ActiveLinkColor = System.Drawing.Color.White;
            this.llGlobalProcesses.AutoSize = true;
            this.llGlobalProcesses.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llGlobalProcesses.LinkColor = System.Drawing.Color.White;
            this.llGlobalProcesses.Location = new System.Drawing.Point(355, 266);
            this.llGlobalProcesses.Name = "llGlobalProcesses";
            this.llGlobalProcesses.Size = new System.Drawing.Size(180, 13);
            this.llGlobalProcesses.TabIndex = 5;
            this.llGlobalProcesses.TabStop = true;
            this.llGlobalProcesses.Text = "Процессы из глобального списка";
            this.llGlobalProcesses.VisitedLinkColor = System.Drawing.Color.White;
            // 
            // fSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::_Hell_PRO_Tanki_Launcher.Properties.Resources.back_settings;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(597, 490);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.llTitle);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки...";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbKillProcesses;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbNews;
        private System.Windows.Forms.CheckBox cbVideo;
        private System.Windows.Forms.LinkLabel llTitle;
        private System.Windows.Forms.CheckBox cbAero;
        private System.Windows.Forms.CheckBox cbForceClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.BackgroundWorker bwUserProcesses;
        private System.Windows.Forms.ListView lvProcessesUser;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.LinkLabel llGlobalProcesses;
        public System.Windows.Forms.LinkLabel llUserProcesses;
    }
}