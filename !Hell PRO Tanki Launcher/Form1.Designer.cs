namespace _Hell_PRO_Tanki_Launcher
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
            this.bPlay = new System.Windows.Forms.Button();
            this.bLauncher = new System.Windows.Forms.Button();
            this.bUpdate = new System.Windows.Forms.Button();
            this.bVideo = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.llVersion = new System.Windows.Forms.LinkLabel();
            this.llContent = new System.Windows.Forms.LinkLabel();
            this.bwUpdater = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // bPlay
            // 
            this.bPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bPlay.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bPlay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bPlay.Location = new System.Drawing.Point(680, 55);
            this.bPlay.Margin = new System.Windows.Forms.Padding(0);
            this.bPlay.Name = "bPlay";
            this.bPlay.Size = new System.Drawing.Size(150, 50);
            this.bPlay.TabIndex = 1;
            this.bPlay.Text = "Играть";
            this.bPlay.UseVisualStyleBackColor = false;
            this.bPlay.Click += new System.EventHandler(this.bPlay_Click);
            // 
            // bLauncher
            // 
            this.bLauncher.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bLauncher.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bLauncher.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bLauncher.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bLauncher.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bLauncher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bLauncher.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bLauncher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bLauncher.Location = new System.Drawing.Point(680, 130);
            this.bLauncher.Margin = new System.Windows.Forms.Padding(0);
            this.bLauncher.Name = "bLauncher";
            this.bLauncher.Size = new System.Drawing.Size(150, 50);
            this.bLauncher.TabIndex = 2;
            this.bLauncher.Text = "Лаунчер";
            this.bLauncher.UseVisualStyleBackColor = false;
            this.bLauncher.Click += new System.EventHandler(this.bLauncher_Click);
            // 
            // bUpdate
            // 
            this.bUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bUpdate.Enabled = false;
            this.bUpdate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bUpdate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bUpdate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bUpdate.Location = new System.Drawing.Point(680, 205);
            this.bUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.bUpdate.Name = "bUpdate";
            this.bUpdate.Size = new System.Drawing.Size(150, 50);
            this.bUpdate.TabIndex = 3;
            this.bUpdate.Text = "Обновить";
            this.bUpdate.UseVisualStyleBackColor = false;
            // 
            // bVideo
            // 
            this.bVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bVideo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bVideo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bVideo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bVideo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bVideo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bVideo.Location = new System.Drawing.Point(680, 280);
            this.bVideo.Margin = new System.Windows.Forms.Padding(0);
            this.bVideo.Name = "bVideo";
            this.bVideo.Size = new System.Drawing.Size(150, 50);
            this.bVideo.TabIndex = 4;
            this.bVideo.Text = "Видео";
            this.bVideo.UseVisualStyleBackColor = false;
            this.bVideo.Click += new System.EventHandler(this.bVideo_Click);
            // 
            // bExit
            // 
            this.bExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bExit.Location = new System.Drawing.Point(680, 355);
            this.bExit.Margin = new System.Windows.Forms.Padding(0);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(150, 50);
            this.bExit.TabIndex = 5;
            this.bExit.Text = "Выход";
            this.bExit.UseVisualStyleBackColor = false;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Red;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.linkLabel1.Location = new System.Drawing.Point(693, 6);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(145, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Developed by Andrey Helldar";
            // 
            // llVersion
            // 
            this.llVersion.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llVersion.AutoSize = true;
            this.llVersion.BackColor = System.Drawing.Color.Transparent;
            this.llVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llVersion.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llVersion.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llVersion.Location = new System.Drawing.Point(9, 4);
            this.llVersion.Name = "llVersion";
            this.llVersion.Size = new System.Drawing.Size(68, 18);
            this.llVersion.TabIndex = 7;
            this.llVersion.TabStop = true;
            this.llVersion.Text = "0.8.11.10";
            // 
            // llContent
            // 
            this.llContent.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llContent.BackColor = System.Drawing.Color.Transparent;
            this.llContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llContent.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llContent.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llContent.Location = new System.Drawing.Point(22, 55);
            this.llContent.Name = "llContent";
            this.llContent.Size = new System.Drawing.Size(638, 377);
            this.llContent.TabIndex = 8;
            this.llContent.TabStop = true;
            this.llContent.Text = "path";
            // 
            // bwUpdater
            // 
            this.bwUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwUpdater_DoWork);
            this.bwUpdater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwUpdater_RunWorkerCompleted);
            // 
            // fIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::_Hell_PRO_Tanki_Launcher.Properties.Resources.back_1;
            this.ClientSize = new System.Drawing.Size(860, 460);
            this.Controls.Add(this.llContent);
            this.Controls.Add(this.llVersion);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bVideo);
            this.Controls.Add(this.bUpdate);
            this.Controls.Add(this.bLauncher);
            this.Controls.Add(this.bPlay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fIndex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bPlay;
        private System.Windows.Forms.Button bLauncher;
        private System.Windows.Forms.Button bUpdate;
        private System.Windows.Forms.Button bVideo;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel llVersion;
        private System.Windows.Forms.LinkLabel llContent;
        private System.ComponentModel.BackgroundWorker bwUpdater;
    }
}

