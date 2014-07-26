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
            this.gbOptimization = new System.Windows.Forms.GroupBox();
            this.cbVideoQualityWeak = new System.Windows.Forms.CheckBox();
            this.cbVideoQuality = new System.Windows.Forms.CheckBox();
            this.cbForceClose = new System.Windows.Forms.CheckBox();
            this.cbAero = new System.Windows.Forms.CheckBox();
            this.cbKillProcesses = new System.Windows.Forms.CheckBox();
            this.gbOther = new System.Windows.Forms.GroupBox();
            this.cbMinimize = new System.Windows.Forms.ComboBox();
            this.cbVideo = new System.Windows.Forms.CheckBox();
            this.llTitle = new System.Windows.Forms.LinkLabel();
            this.gbProcesses = new System.Windows.Forms.GroupBox();
            this.lDescProcesses = new System.Windows.Forms.Label();
            this.llGlobalProcesses = new System.Windows.Forms.LinkLabel();
            this.llUserProcesses = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lvProcessesUser = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bwUserProcesses = new System.ComponentModel.BackgroundWorker();
            this.gbPriority = new System.Windows.Forms.GroupBox();
            this.cbPriority = new System.Windows.Forms.ComboBox();
            this.llRecoverySettings = new System.Windows.Forms.LinkLabel();
            this.gbOptimization.SuspendLayout();
            this.gbOther.SuspendLayout();
            this.gbProcesses.SuspendLayout();
            this.gbPriority.SuspendLayout();
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
            this.bSave.Location = new System.Drawing.Point(129, 509);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(150, 50);
            this.bSave.TabIndex = 14;
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
            this.bCancel.Location = new System.Drawing.Point(300, 509);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(150, 50);
            this.bCancel.TabIndex = 15;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = false;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // gbOptimization
            // 
            this.gbOptimization.BackColor = System.Drawing.Color.Transparent;
            this.gbOptimization.Controls.Add(this.cbVideoQualityWeak);
            this.gbOptimization.Controls.Add(this.cbVideoQuality);
            this.gbOptimization.Controls.Add(this.cbForceClose);
            this.gbOptimization.Controls.Add(this.cbAero);
            this.gbOptimization.Controls.Add(this.cbKillProcesses);
            this.gbOptimization.ForeColor = System.Drawing.Color.White;
            this.gbOptimization.Location = new System.Drawing.Point(12, 29);
            this.gbOptimization.Name = "gbOptimization";
            this.gbOptimization.Size = new System.Drawing.Size(573, 85);
            this.gbOptimization.TabIndex = 0;
            this.gbOptimization.TabStop = false;
            this.gbOptimization.Text = "Оптимизация:";
            // 
            // cbVideoQualityWeak
            // 
            this.cbVideoQualityWeak.AutoSize = true;
            this.cbVideoQualityWeak.Location = new System.Drawing.Point(337, 43);
            this.cbVideoQualityWeak.Name = "cbVideoQualityWeak";
            this.cbVideoQualityWeak.Size = new System.Drawing.Size(158, 17);
            this.cbVideoQualityWeak.TabIndex = 5;
            this.cbVideoQualityWeak.Text = "Очень слабый компьютер";
            this.cbVideoQualityWeak.UseVisualStyleBackColor = true;
            this.cbVideoQualityWeak.Click += new System.EventHandler(this.cbVideoQualityVeryLow_Click);
            // 
            // cbVideoQuality
            // 
            this.cbVideoQuality.AutoSize = true;
            this.cbVideoQuality.Location = new System.Drawing.Point(312, 19);
            this.cbVideoQuality.Name = "cbVideoQuality";
            this.cbVideoQuality.Size = new System.Drawing.Size(215, 17);
            this.cbVideoQuality.TabIndex = 4;
            this.cbVideoQuality.Text = "Уменьшить качество графики в игре";
            this.cbVideoQuality.ThreeState = true;
            this.cbVideoQuality.UseVisualStyleBackColor = true;
            this.cbVideoQuality.Click += new System.EventHandler(this.cbVideoQuality_Click);
            // 
            // cbForceClose
            // 
            this.cbForceClose.AutoSize = true;
            this.cbForceClose.Location = new System.Drawing.Point(42, 43);
            this.cbForceClose.Name = "cbForceClose";
            this.cbForceClose.Size = new System.Drawing.Size(227, 17);
            this.cbForceClose.TabIndex = 2;
            this.cbForceClose.Text = "Принудительно завершать приложения";
            this.cbForceClose.UseVisualStyleBackColor = true;
            // 
            // cbAero
            // 
            this.cbAero.AutoSize = true;
            this.cbAero.Location = new System.Drawing.Point(312, 66);
            this.cbAero.Name = "cbAero";
            this.cbAero.Size = new System.Drawing.Size(246, 17);
            this.cbAero.TabIndex = 3;
            this.cbAero.Text = "Отключать Windows Aero при запуске игры";
            this.cbAero.UseVisualStyleBackColor = true;
            // 
            // cbKillProcesses
            // 
            this.cbKillProcesses.AutoSize = true;
            this.cbKillProcesses.Location = new System.Drawing.Point(18, 20);
            this.cbKillProcesses.Name = "cbKillProcesses";
            this.cbKillProcesses.Size = new System.Drawing.Size(240, 17);
            this.cbKillProcesses.TabIndex = 1;
            this.cbKillProcesses.Text = "Закрывать приложения при запуске игры";
            this.cbKillProcesses.UseVisualStyleBackColor = true;
            // 
            // gbOther
            // 
            this.gbOther.BackColor = System.Drawing.Color.Transparent;
            this.gbOther.Controls.Add(this.cbMinimize);
            this.gbOther.Controls.Add(this.cbVideo);
            this.gbOther.ForeColor = System.Drawing.Color.White;
            this.gbOther.Location = new System.Drawing.Point(12, 120);
            this.gbOther.Name = "gbOther";
            this.gbOther.Size = new System.Drawing.Size(276, 71);
            this.gbOther.TabIndex = 7;
            this.gbOther.TabStop = false;
            this.gbOther.Text = "Другие:";
            // 
            // cbMinimize
            // 
            this.cbMinimize.BackColor = System.Drawing.Color.White;
            this.cbMinimize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMinimize.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbMinimize.ForeColor = System.Drawing.Color.Black;
            this.cbMinimize.FormattingEnabled = true;
            this.cbMinimize.Items.AddRange(new object[] {
            "Не закрывать лаунчер",
            "Сворачивать в трей при запуске игры",
            "Минимизировать лаунчер на панель задач",
            "Закрывать при запуске игры"});
            this.cbMinimize.Location = new System.Drawing.Point(12, 19);
            this.cbMinimize.Name = "cbMinimize";
            this.cbMinimize.Size = new System.Drawing.Size(252, 23);
            this.cbMinimize.TabIndex = 9;
            // 
            // cbVideo
            // 
            this.cbVideo.AutoSize = true;
            this.cbVideo.Checked = true;
            this.cbVideo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbVideo.ForeColor = System.Drawing.Color.Black;
            this.cbVideo.Location = new System.Drawing.Point(12, 49);
            this.cbVideo.Name = "cbVideo";
            this.cbVideo.Size = new System.Drawing.Size(165, 17);
            this.cbVideo.TabIndex = 8;
            this.cbVideo.Text = "Уведомлять о новых видео";
            this.cbVideo.UseVisualStyleBackColor = true;
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
            this.llTitle.Location = new System.Drawing.Point(12, 7);
            this.llTitle.Name = "llTitle";
            this.llTitle.Size = new System.Drawing.Size(80, 19);
            this.llTitle.TabIndex = 0;
            this.llTitle.TabStop = true;
            this.llTitle.Text = "Настройки...";
            this.llTitle.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            // 
            // gbProcesses
            // 
            this.gbProcesses.BackColor = System.Drawing.Color.Transparent;
            this.gbProcesses.Controls.Add(this.lDescProcesses);
            this.gbProcesses.Controls.Add(this.llGlobalProcesses);
            this.gbProcesses.Controls.Add(this.llUserProcesses);
            this.gbProcesses.Controls.Add(this.panel2);
            this.gbProcesses.Controls.Add(this.panel1);
            this.gbProcesses.Controls.Add(this.lvProcessesUser);
            this.gbProcesses.ForeColor = System.Drawing.Color.Black;
            this.gbProcesses.Location = new System.Drawing.Point(12, 197);
            this.gbProcesses.Name = "gbProcesses";
            this.gbProcesses.Size = new System.Drawing.Size(573, 306);
            this.gbProcesses.TabIndex = 12;
            this.gbProcesses.TabStop = false;
            this.gbProcesses.Text = "Какие процессы НЕЛЬЗЯ закрывать при запуске игры:";
            // 
            // lDescProcesses
            // 
            this.lDescProcesses.Font = new System.Drawing.Font("Sochi2014", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lDescProcesses.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.lDescProcesses.Location = new System.Drawing.Point(7, 260);
            this.lDescProcesses.Name = "lDescProcesses";
            this.lDescProcesses.Size = new System.Drawing.Size(561, 41);
            this.lDescProcesses.TabIndex = 0;
            this.lDescProcesses.Text = "ВНИМАНИЕ!!! В список исключений рекомендуется добавлять действительно важные прог" +
    "раммы!";
            // 
            // llGlobalProcesses
            // 
            this.llGlobalProcesses.ActiveLinkColor = System.Drawing.Color.White;
            this.llGlobalProcesses.AutoSize = true;
            this.llGlobalProcesses.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llGlobalProcesses.ForeColor = System.Drawing.Color.White;
            this.llGlobalProcesses.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llGlobalProcesses.LinkColor = System.Drawing.Color.White;
            this.llGlobalProcesses.Location = new System.Drawing.Point(352, 238);
            this.llGlobalProcesses.Name = "llGlobalProcesses";
            this.llGlobalProcesses.Size = new System.Drawing.Size(199, 15);
            this.llGlobalProcesses.TabIndex = 0;
            this.llGlobalProcesses.TabStop = true;
            this.llGlobalProcesses.Text = "Процессы из глобального списка";
            this.llGlobalProcesses.VisitedLinkColor = System.Drawing.Color.White;
            // 
            // llUserProcesses
            // 
            this.llUserProcesses.ActiveLinkColor = System.Drawing.Color.White;
            this.llUserProcesses.AutoSize = true;
            this.llUserProcesses.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llUserProcesses.ForeColor = System.Drawing.Color.White;
            this.llUserProcesses.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llUserProcesses.LinkColor = System.Drawing.Color.White;
            this.llUserProcesses.Location = new System.Drawing.Point(50, 238);
            this.llUserProcesses.Name = "llUserProcesses";
            this.llUserProcesses.Size = new System.Drawing.Size(233, 15);
            this.llUserProcesses.TabIndex = 0;
            this.llUserProcesses.TabStop = true;
            this.llUserProcesses.Text = "Процессы, выбранные пользователем";
            this.llUserProcesses.VisitedLinkColor = System.Drawing.Color.White;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Plum;
            this.panel2.Location = new System.Drawing.Point(313, 232);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(25, 25);
            this.panel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGreen;
            this.panel1.Location = new System.Drawing.Point(19, 232);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(25, 25);
            this.panel1.TabIndex = 0;
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
            this.lvProcessesUser.Size = new System.Drawing.Size(561, 207);
            this.lvProcessesUser.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvProcessesUser.TabIndex = 13;
            this.lvProcessesUser.UseCompatibleStateImageBehavior = false;
            this.lvProcessesUser.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Процесс";
            this.columnHeader1.Width = 170;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Описание";
            this.columnHeader2.Width = 365;
            // 
            // bwUserProcesses
            // 
            this.bwUserProcesses.WorkerReportsProgress = true;
            this.bwUserProcesses.WorkerSupportsCancellation = true;
            this.bwUserProcesses.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwUserProcesses_DoWork);
            this.bwUserProcesses.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwUserProcesses_RunWorkerCompleted);
            // 
            // gbPriority
            // 
            this.gbPriority.BackColor = System.Drawing.Color.Transparent;
            this.gbPriority.Controls.Add(this.cbPriority);
            this.gbPriority.ForeColor = System.Drawing.Color.White;
            this.gbPriority.Location = new System.Drawing.Point(294, 120);
            this.gbPriority.Name = "gbPriority";
            this.gbPriority.Size = new System.Drawing.Size(291, 71);
            this.gbPriority.TabIndex = 9;
            this.gbPriority.TabStop = false;
            this.gbPriority.Text = "Приоритет игры в системе:";
            // 
            // cbPriority
            // 
            this.cbPriority.BackColor = System.Drawing.Color.White;
            this.cbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPriority.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbPriority.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbPriority.ForeColor = System.Drawing.Color.Black;
            this.cbPriority.FormattingEnabled = true;
            this.cbPriority.Items.AddRange(new object[] {
            "Высокий",
            "Выше среднего",
            "Средний",
            "Ниже среднего",
            "Низкий"});
            this.cbPriority.Location = new System.Drawing.Point(6, 19);
            this.cbPriority.Name = "cbPriority";
            this.cbPriority.Size = new System.Drawing.Size(279, 23);
            this.cbPriority.TabIndex = 10;
            // 
            // llRecoverySettings
            // 
            this.llRecoverySettings.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llRecoverySettings.AutoSize = true;
            this.llRecoverySettings.BackColor = System.Drawing.Color.Transparent;
            this.llRecoverySettings.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.llRecoverySettings.DisabledLinkColor = System.Drawing.Color.Gray;
            this.llRecoverySettings.Enabled = false;
            this.llRecoverySettings.Font = new System.Drawing.Font("Sochi2014", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llRecoverySettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llRecoverySettings.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llRecoverySettings.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llRecoverySettings.Location = new System.Drawing.Point(400, 7);
            this.llRecoverySettings.Name = "llRecoverySettings";
            this.llRecoverySettings.Size = new System.Drawing.Size(153, 19);
            this.llRecoverySettings.TabIndex = 6;
            this.llRecoverySettings.TabStop = true;
            this.llRecoverySettings.Text = "Восстановить настройки";
            this.llRecoverySettings.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llRecoverySettings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llRecoverySettings_LinkClicked);
            // 
            // fSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::_Hell_PRO_Tanki_Launcher.Properties.Resources.FonSetting;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(597, 570);
            this.Controls.Add(this.llRecoverySettings);
            this.Controls.Add(this.gbPriority);
            this.Controls.Add(this.gbProcesses);
            this.Controls.Add(this.llTitle);
            this.Controls.Add(this.gbOther);
            this.Controls.Add(this.gbOptimization);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки...";
            this.Load += new System.EventHandler(this.fSettings_Load);
            this.gbOptimization.ResumeLayout(false);
            this.gbOptimization.PerformLayout();
            this.gbOther.ResumeLayout(false);
            this.gbOther.PerformLayout();
            this.gbProcesses.ResumeLayout(false);
            this.gbProcesses.PerformLayout();
            this.gbPriority.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.GroupBox gbOptimization;
        private System.Windows.Forms.CheckBox cbKillProcesses;
        private System.Windows.Forms.GroupBox gbOther;
        private System.Windows.Forms.CheckBox cbVideo;
        private System.Windows.Forms.LinkLabel llTitle;
        private System.Windows.Forms.CheckBox cbAero;
        private System.Windows.Forms.CheckBox cbForceClose;
        private System.Windows.Forms.GroupBox gbProcesses;
        private System.ComponentModel.BackgroundWorker bwUserProcesses;
        private System.Windows.Forms.ListView lvProcessesUser;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.LinkLabel llGlobalProcesses;
        public System.Windows.Forms.LinkLabel llUserProcesses;
        private System.Windows.Forms.GroupBox gbPriority;
        private System.Windows.Forms.ComboBox cbPriority;
        private System.Windows.Forms.Label lDescProcesses;
        private System.Windows.Forms.CheckBox cbVideoQuality;
        private System.Windows.Forms.LinkLabel llRecoverySettings;
        private System.Windows.Forms.CheckBox cbVideoQualityWeak;
        private System.Windows.Forms.ComboBox cbMinimize;
    }
}