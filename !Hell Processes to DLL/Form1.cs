using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Hell_Processes_to_DLL
{
    public partial class fIndex : Form
    {
        public fIndex()
        {
            InitializeComponent();
        }

        private void fIndex_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + " v" + Application.ProductVersion;
        }
    }
}
