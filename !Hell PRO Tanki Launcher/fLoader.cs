using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fLoader : Form
    {
        int start = 0;

        public fLoader()
        {
            InitializeComponent();
        }

        private void fLoader_Load(object sender, EventArgs e)
        {
            CountTime();
        }

        private async Task CountTime()
        {
            await Task.Delay(1000);

            ++start;

            // Если ждем больше 30 секунд, то перезапускаем прогу
            if (start > 30)
            {
                Process.Start("restart.exe", "\"" + Process.GetCurrentProcess().ProcessName + ".exe\"");
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
