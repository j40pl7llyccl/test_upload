﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace uIP.Lib.UsrControl
{
    public partial class frmToolTest : Form
    {
        public frmToolTest()
        {
            InitializeComponent();
        }

        private void frmToolTest_FormClosing( object sender, FormClosingEventArgs e )
        {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
