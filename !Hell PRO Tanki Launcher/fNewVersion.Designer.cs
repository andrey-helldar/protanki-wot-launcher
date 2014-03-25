namespace _Hell_PRO_Tanki_Launcher
{
    partial class fNewVersion
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
            this.llContent = new System.Windows.Forms.LinkLabel();
            this.bDownload = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.cbNotification = new System.Windows.Forms.CheckBox();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.llVersion = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // llContent
            // 
            this.llContent.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llContent.BackColor = System.Drawing.Color.Transparent;
            this.llContent.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llContent.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llContent.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llContent.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llContent.Location = new System.Drawing.Point(12, 37);
            this.llContent.Name = "llContent";
            this.llContent.Size = new System.Drawing.Size(676, 290);
            this.llContent.TabIndex = 0;
            this.llContent.TabStop = true;
            this.llContent.Text = "llContent";
            this.llContent.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            // 
            // bDownload
            // 
            this.bDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bDownload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bDownload.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bDownload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bDownload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bDownload.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bDownload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bDownload.Location = new System.Drawing.Point(144, 353);
            this.bDownload.Margin = new System.Windows.Forms.Padding(0);
            this.bDownload.Name = "bDownload";
            this.bDownload.Size = new System.Drawing.Size(200, 50);
            this.bDownload.TabIndex = 1;
            this.bDownload.Text = "Скачать";
            this.bDownload.UseVisualStyleBackColor = false;
            this.bDownload.Click += new System.EventHandler(this.bDownload_Click);
            // 
            // bCancel
            // 
            this.bCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bCancel.Location = new System.Drawing.Point(368, 353);
            this.bCancel.Margin = new System.Windows.Forms.Padding(0);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(200, 50);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Не надо";
            this.bCancel.UseVisualStyleBackColor = false;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // cbNotification
            // 
            this.cbNotification.AutoSize = true;
            this.cbNotification.ForeColor = System.Drawing.Color.White;
            this.cbNotification.Location = new System.Drawing.Point(368, 330);
            this.cbNotification.Name = "cbNotification";
            this.cbNotification.Size = new System.Drawing.Size(212, 17);
            this.cbNotification.TabIndex = 3;
            this.cbNotification.Text = "Не уведомлять меня об этой версии";
            this.cbNotification.UseVisualStyleBackColor = true;
            // 
            // linkLabel2
            // 
            this.linkLabel2.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel2.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.linkLabel2.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel2.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.linkLabel2.Location = new System.Drawing.Point(11, 5);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(314, 24);
            this.linkLabel2.TabIndex = 4;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Обнаружена новая версия Мультипака:";
            this.linkLabel2.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            // 
            // llVersion
            // 
            this.llVersion.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llVersion.AutoSize = true;
            this.llVersion.BackColor = System.Drawing.Color.Transparent;
            this.llVersion.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llVersion.Font = new System.Drawing.Font("Sochi2014", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.llVersion.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llVersion.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.llVersion.Location = new System.Drawing.Point(331, 5);
            this.llVersion.Name = "llVersion";
            this.llVersion.Size = new System.Drawing.Size(66, 24);
            this.llVersion.TabIndex = 5;
            this.llVersion.TabStop = true;
            this.llVersion.Text = "0.0.0.0";
            this.llVersion.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            // 
            // fNewVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(700, 408);
            this.Controls.Add(this.llVersion);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.cbNotification);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bDownload);
            this.Controls.Add(this.llContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fNewVersion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обнаружена новая версия Мультипака";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bDownload;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.CheckBox cbNotification;
        public System.Windows.Forms.LinkLabel llContent;
        private System.Windows.Forms.LinkLabel linkLabel2;
        public System.Windows.Forms.LinkLabel llVersion;
    }
}