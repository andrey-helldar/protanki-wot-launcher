using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fLoader : Form
    {
        public fLoader()
        {
            InitializeComponent();
        }

        private void fLoader_Load(object sender, EventArgs e)
        {
            UpdateLauncher update = new UpdateLauncher(); // Инициализируем обновление библиотек
            update.Check().Wait();

            Visible = false;

            fIndex fIndex = new fIndex();
            fIndex.Show();

            //Close();
        }
    }
}
