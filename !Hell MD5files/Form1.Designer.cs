namespace _Hell_MD5files
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
            this.lvMD5 = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bSave = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvMD5
            // 
            this.lvMD5.CheckBoxes = true;
            this.lvMD5.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader5,
            this.columnHeader4});
            this.lvMD5.FullRowSelect = true;
            this.lvMD5.Location = new System.Drawing.Point(12, 12);
            this.lvMD5.Name = "lvMD5";
            this.lvMD5.ShowGroups = false;
            this.lvMD5.Size = new System.Drawing.Size(760, 281);
            this.lvMD5.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvMD5.TabIndex = 0;
            this.lvMD5.UseCompatibleStateImageBehavior = false;
            this.lvMD5.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Файл";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Версия";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Размер";
            this.columnHeader3.Width = 110;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Расширение";
            this.columnHeader5.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "MD5";
            this.columnHeader4.Width = 320;
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(12, 299);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(530, 43);
            this.bSave.TabIndex = 1;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bClose
            // 
            this.bClose.Location = new System.Drawing.Point(548, 299);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(224, 43);
            this.bClose.TabIndex = 2;
            this.bClose.Text = "Выход";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 350);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.lvMD5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvMD5;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}

