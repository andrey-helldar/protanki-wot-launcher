﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Hell_PRO_Tanki_Launcher
{
    public partial class fPing : Form
    {
        XDocument doc = XDocument.Load("settings.xml");

        public fPing()
        {
            InitializeComponent();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void fPing_Load(object sender, EventArgs e)
        {
            
        }
    }
}
