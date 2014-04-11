namespace FormatXML
{
    partial class fFormatXML
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbFullDesc = new System.Windows.Forms.TextBox();
            this.tbFullLink = new System.Windows.Forms.TextBox();
            this.tbBaseDesc = new System.Windows.Forms.TextBox();
            this.tbBaseLink = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbVersion = new System.Windows.Forms.TextBox();
            this.bSave = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbFullLink);
            this.groupBox1.Controls.Add(this.tbFullDesc);
            this.groupBox1.Location = new System.Drawing.Point(12, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 230);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Расширенная версия";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbBaseLink);
            this.groupBox2.Controls.Add(this.tbBaseDesc);
            this.groupBox2.Location = new System.Drawing.Point(12, 299);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(760, 230);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Базовая версия:";
            // 
            // tbFullDesc
            // 
            this.tbFullDesc.Location = new System.Drawing.Point(6, 19);
            this.tbFullDesc.Multiline = true;
            this.tbFullDesc.Name = "tbFullDesc";
            this.tbFullDesc.Size = new System.Drawing.Size(748, 179);
            this.tbFullDesc.TabIndex = 0;
            // 
            // tbFullLink
            // 
            this.tbFullLink.Location = new System.Drawing.Point(6, 204);
            this.tbFullLink.Name = "tbFullLink";
            this.tbFullLink.Size = new System.Drawing.Size(748, 20);
            this.tbFullLink.TabIndex = 1;
            // 
            // tbBaseDesc
            // 
            this.tbBaseDesc.Location = new System.Drawing.Point(6, 19);
            this.tbBaseDesc.Multiline = true;
            this.tbBaseDesc.Name = "tbBaseDesc";
            this.tbBaseDesc.Size = new System.Drawing.Size(748, 179);
            this.tbBaseDesc.TabIndex = 0;
            // 
            // tbBaseLink
            // 
            this.tbBaseLink.Location = new System.Drawing.Point(6, 204);
            this.tbBaseLink.Name = "tbBaseLink";
            this.tbBaseLink.Size = new System.Drawing.Size(748, 20);
            this.tbBaseLink.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbVersion);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(760, 45);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Версия модпака:";
            // 
            // tbVersion
            // 
            this.tbVersion.Location = new System.Drawing.Point(6, 19);
            this.tbVersion.Name = "tbVersion";
            this.tbVersion.Size = new System.Drawing.Size(748, 20);
            this.tbVersion.TabIndex = 0;
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(498, 535);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(134, 23);
            this.bSave.TabIndex = 3;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.Location = new System.Drawing.Point(638, 535);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(134, 23);
            this.bExit.TabIndex = 4;
            this.bExit.Text = "Выход";
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // fFormatXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "fFormatXML";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "fFormatXML";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbFullLink;
        private System.Windows.Forms.TextBox tbFullDesc;
        private System.Windows.Forms.TextBox tbBaseLink;
        private System.Windows.Forms.TextBox tbBaseDesc;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbVersion;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

