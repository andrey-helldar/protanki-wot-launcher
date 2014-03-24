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
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.bDownload = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.cbNotification = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(12, 37);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(676, 301);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            // 
            // bDownload
            // 
            this.bDownload.Location = new System.Drawing.Point(222, 353);
            this.bDownload.Name = "bDownload";
            this.bDownload.Size = new System.Drawing.Size(75, 23);
            this.bDownload.TabIndex = 1;
            this.bDownload.Text = "Скачать";
            this.bDownload.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(389, 353);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Не надо";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // cbNotification
            // 
            this.cbNotification.AutoSize = true;
            this.cbNotification.Location = new System.Drawing.Point(389, 330);
            this.cbNotification.Name = "cbNotification";
            this.cbNotification.Size = new System.Drawing.Size(212, 17);
            this.cbNotification.TabIndex = 3;
            this.cbNotification.Text = "Не уведомлять меня об этой версии";
            this.cbNotification.UseVisualStyleBackColor = true;
            // 
            // fNewVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 408);
            this.Controls.Add(this.cbNotification);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bDownload);
            this.Controls.Add(this.linkLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fNewVersion";
            this.Text = "Обнаружена новая версия Мультипака";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button bDownload;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.CheckBox cbNotification;
    }
}