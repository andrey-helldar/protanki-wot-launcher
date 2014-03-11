namespace _Hell_PRO_Tanki_Launcher
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
            this.lVersion = new System.Windows.Forms.Label();
            this.bPlay = new System.Windows.Forms.Button();
            this.bLauncher = new System.Windows.Forms.Button();
            this.bUpdate = new System.Windows.Forms.Button();
            this.bVideo = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lVersion
            // 
            this.lVersion.AutoSize = true;
            this.lVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lVersion.Location = new System.Drawing.Point(8, 6);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new System.Drawing.Size(68, 18);
            this.lVersion.TabIndex = 0;
            this.lVersion.Text = "0.8.11.10";
            // 
            // bPlay
            // 
            this.bPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bPlay.Location = new System.Drawing.Point(680, 36);
            this.bPlay.Name = "bPlay";
            this.bPlay.Size = new System.Drawing.Size(120, 40);
            this.bPlay.TabIndex = 1;
            this.bPlay.Text = "Играть";
            this.bPlay.UseVisualStyleBackColor = true;
            // 
            // bLauncher
            // 
            this.bLauncher.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bLauncher.Location = new System.Drawing.Point(680, 111);
            this.bLauncher.Name = "bLauncher";
            this.bLauncher.Size = new System.Drawing.Size(120, 40);
            this.bLauncher.TabIndex = 2;
            this.bLauncher.Text = "Лаунчер";
            this.bLauncher.UseVisualStyleBackColor = true;
            // 
            // bUpdate
            // 
            this.bUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bUpdate.Location = new System.Drawing.Point(680, 193);
            this.bUpdate.Name = "bUpdate";
            this.bUpdate.Size = new System.Drawing.Size(120, 40);
            this.bUpdate.TabIndex = 3;
            this.bUpdate.Text = "Обновить";
            this.bUpdate.UseVisualStyleBackColor = true;
            // 
            // bVideo
            // 
            this.bVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bVideo.Location = new System.Drawing.Point(680, 283);
            this.bVideo.Name = "bVideo";
            this.bVideo.Size = new System.Drawing.Size(120, 40);
            this.bVideo.TabIndex = 4;
            this.bVideo.Text = "Видео";
            this.bVideo.UseVisualStyleBackColor = true;
            // 
            // bExit
            // 
            this.bExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bExit.Location = new System.Drawing.Point(680, 379);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(120, 40);
            this.bExit.TabIndex = 5;
            this.bExit.Text = "Выход";
            this.bExit.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.Location = new System.Drawing.Point(12, 438);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(145, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Developed by Andrey Helldar";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 460);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bVideo);
            this.Controls.Add(this.bUpdate);
            this.Controls.Add(this.bLauncher);
            this.Controls.Add(this.bPlay);
            this.Controls.Add(this.lVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lVersion;
        private System.Windows.Forms.Button bPlay;
        private System.Windows.Forms.Button bLauncher;
        private System.Windows.Forms.Button bUpdate;
        private System.Windows.Forms.Button bVideo;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}

