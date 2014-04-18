namespace _Hell_PRO_Tanki_Launcher
{
    partial class fWarning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fWarning));
            this.lDesc = new System.Windows.Forms.Label();
            this.tbTicket = new System.Windows.Forms.TextBox();
            this.bSend = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.rbWish = new System.Windows.Forms.RadioButton();
            this.rbBug = new System.Windows.Forms.RadioButton();
            this.lMessAboutNewVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lDesc
            // 
            this.lDesc.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.lDesc.Location = new System.Drawing.Point(12, 9);
            this.lDesc.Name = "lDesc";
            this.lDesc.Size = new System.Drawing.Size(671, 46);
            this.lDesc.TabIndex = 0;
            this.lDesc.Text = "Если у Вас возникли проблемы в работе лаунчера или есть какие-либо пожелания, Вы " +
    "можете заполнить форму ниже и отправить сообщение разработчику:";
            // 
            // tbTicket
            // 
            this.tbTicket.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(9)))), ((int)(((byte)(0)))));
            this.tbTicket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTicket.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbTicket.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.tbTicket.Location = new System.Drawing.Point(16, 58);
            this.tbTicket.Multiline = true;
            this.tbTicket.Name = "tbTicket";
            this.tbTicket.Size = new System.Drawing.Size(667, 235);
            this.tbTicket.TabIndex = 1;
            // 
            // bSend
            // 
            this.bSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bSend.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bSend.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bSend.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSend.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bSend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bSend.Location = new System.Drawing.Point(437, 393);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(120, 34);
            this.bSend.TabIndex = 4;
            this.bSend.Text = "Отправить";
            this.bSend.UseVisualStyleBackColor = false;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // bCancel
            // 
            this.bCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.bCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.bCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.bCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(46)))));
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.bCancel.Location = new System.Drawing.Point(563, 393);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(120, 34);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Выход";
            this.bCancel.UseVisualStyleBackColor = false;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // rbWish
            // 
            this.rbWish.AutoSize = true;
            this.rbWish.BackColor = System.Drawing.Color.Transparent;
            this.rbWish.Checked = true;
            this.rbWish.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbWish.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.rbWish.Location = new System.Drawing.Point(16, 398);
            this.rbWish.Name = "rbWish";
            this.rbWish.Size = new System.Drawing.Size(177, 24);
            this.rbWish.TabIndex = 2;
            this.rbWish.TabStop = true;
            this.rbWish.Text = "Пожелания к лаунчеру";
            this.rbWish.UseVisualStyleBackColor = false;
            // 
            // rbBug
            // 
            this.rbBug.AutoSize = true;
            this.rbBug.BackColor = System.Drawing.Color.Transparent;
            this.rbBug.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbBug.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.rbBug.Location = new System.Drawing.Point(219, 398);
            this.rbBug.Name = "rbBug";
            this.rbBug.Size = new System.Drawing.Size(137, 24);
            this.rbBug.TabIndex = 3;
            this.rbBug.Text = "Найдена ошибка";
            this.rbBug.UseVisualStyleBackColor = false;
            // 
            // lMessAboutNewVersion
            // 
            this.lMessAboutNewVersion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lMessAboutNewVersion.Font = new System.Drawing.Font("Sochi2014", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lMessAboutNewVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(123)))), ((int)(((byte)(16)))));
            this.lMessAboutNewVersion.Location = new System.Drawing.Point(12, 302);
            this.lMessAboutNewVersion.Name = "lMessAboutNewVersion";
            this.lMessAboutNewVersion.Size = new System.Drawing.Size(671, 88);
            this.lMessAboutNewVersion.TabIndex = 6;
            this.lMessAboutNewVersion.Text = resources.GetString("lMessAboutNewVersion.Text");
            this.lMessAboutNewVersion.Click += new System.EventHandler(this.lMessAboutNewVersion_Click);
            // 
            // fWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(23)))), ((int)(((byte)(0)))));
            this.BackgroundImage = global::_Hell_PRO_Tanki_Launcher.Properties.Resources.fWarning;
            this.ClientSize = new System.Drawing.Size(695, 441);
            this.Controls.Add(this.lMessAboutNewVersion);
            this.Controls.Add(this.rbBug);
            this.Controls.Add(this.rbWish);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSend);
            this.Controls.Add(this.tbTicket);
            this.Controls.Add(this.lDesc);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fWarning";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fWarning";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lDesc;
        private System.Windows.Forms.TextBox tbTicket;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.RadioButton rbWish;
        private System.Windows.Forms.RadioButton rbBug;
        private System.Windows.Forms.Label lMessAboutNewVersion;
    }
}