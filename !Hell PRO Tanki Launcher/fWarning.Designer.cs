﻿namespace _Hell_PRO_Tanki_Launcher
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbTicket = new System.Windows.Forms.TextBox();
            this.bSend = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Sochi2014", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(671, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Если у Вас возникли проблемы в работе лаунчера или есть какие-либо пожелания, Вы " +
    "можете заполнить форму ниже и отправить сообщение разработчику";
            // 
            // tbTicket
            // 
            this.tbTicket.Location = new System.Drawing.Point(16, 58);
            this.tbTicket.Multiline = true;
            this.tbTicket.Name = "tbTicket";
            this.tbTicket.Size = new System.Drawing.Size(667, 235);
            this.tbTicket.TabIndex = 1;
            // 
            // bSend
            // 
            this.bSend.ForeColor = System.Drawing.Color.Black;
            this.bSend.Location = new System.Drawing.Point(196, 299);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(120, 34);
            this.bSend.TabIndex = 2;
            this.bSend.Text = "Отправить";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // bCancel
            // 
            this.bCancel.ForeColor = System.Drawing.Color.Black;
            this.bCancel.Location = new System.Drawing.Point(358, 299);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(120, 34);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Выход";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // fWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(695, 341);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSend);
            this.Controls.Add(this.tbTicket);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTicket;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.Button bCancel;
    }
}