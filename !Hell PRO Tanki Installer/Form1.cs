using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace _Hell_PRO_Tanki_Installer
{
    public partial class fIndex : Form
    {
        int nowPage = 0;

        public fIndex()
        {
            InitializeComponent();

            setMenu();
        }

        // Определяем какой пункт меню открыт, что отображается справа
        private void setMenu(int newPage = 0)
        {
            if (newPage > 7) { newPage = 7; }

            pbMenu.Visible = true;

            //Так как во время разработки панели были сдвинуты, восстанавливаем их расположение
            panel1.Location = new Point(348, 37); panel1.Size = new Size(484, 337);
            panel2.Location = new Point(348, 37); panel2.Size = new Size(484, 337);
            panel3.Location = new Point(348, 37); panel3.Size = new Size(484, 337);
            panel4.Location = new Point(348, 37); panel4.Size = new Size(484, 337);
            panel5.Location = new Point(348, 37); panel5.Size = new Size(484, 337);
            panel6.Location = new Point(348, 37); panel6.Size = new Size(484, 337);
            panel7.Location = new Point(348, 37); panel7.Size = new Size(484, 337);

            // Чтобы по 100 раз не копировать код, отключаем видимость всех панелей
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel3_1.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;

            switch (newPage)
            {
                case 1: 
                    pbMenu.Image = Properties.Resources.menu_1; 
                    nowPage = 1; 
                    bPrev.Visible = false; 
                    bNext.Text = "Далее >";
                    panel1.Visible = true;
                    break;

                case 2:
                    pbMenu.Image = Properties.Resources.menu_2; 
                    nowPage = 2; 
                    bPrev.Visible = true; 
                    bNext.Text = "Далее >";
                    panel2.Visible = true;
                    break;

                case 3: 
                    // Проверяем верно ли указан путь, так как присутствует возможность выбора пути вручную
                    // Если путь не существует, то возвращаемся к пункту 2
                    if (!Directory.Exists(tbPath.Text))
                    {
                        MessageBox.Show(this, "Выбранный Вами путь не существует!"+Environment.NewLine+"Проверьте правильность пути", "Папка с World of Tanks не обнаружена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        setMenu(2);
                    }
                    else
                    {
                        pbMenu.Image = Properties.Resources.menu_3;
                        nowPage = 3;
                        bPrev.Visible = true;
                        bNext.Text = "Далее >";
                        panel3.Visible = true;

                        pbMenu.Visible = false;
                        panel3_1.Visible = true;
                    }
                    break;

                case 4: 
                    pbMenu.Image = Properties.Resources.menu_4; 
                    nowPage = 4;
                    bPrev.Visible = true; 
                    bNext.Text = "Далее >"; 
                    panel4.Visible = true;
                    break;

                case 5:
                    pbMenu.Image = Properties.Resources.menu_5; 
                    nowPage = 5; 
                    bPrev.Visible = true; 
                    bNext.Text = "Далее >";
                    panel5.Visible = true;
                    break;

                case 6:
                    pbMenu.Image = Properties.Resources.menu_6; 
                    nowPage = 6; 
                    bPrev.Visible = true; 
                    bNext.Text = "Далее >";
                    panel6.Visible = true;
                    break;

                case 7: 
                    pbMenu.Image = Properties.Resources.menu_7;
                    nowPage = 7;
                    bPrev.Visible = true; 
                    bNext.Text = "Готово"; 
                    panel7.Visible = true;
                    break;

                default: 
                    pbMenu.Image = Properties.Resources.menu_1; 
                    nowPage = 1;
                    bPrev.Visible = false;
                    bNext.Text = "Далее >";
                    panel1.Visible = true;
                    break;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("http://goo.gl/gr6pFl");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("http://goo.gl/sPbGhw");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("http://goo.gl/MKy1OZ");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            setMenu(++nowPage);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            setMenu(--nowPage);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            tbPath.Text = fbd.SelectedPath;
        }
    }
}
