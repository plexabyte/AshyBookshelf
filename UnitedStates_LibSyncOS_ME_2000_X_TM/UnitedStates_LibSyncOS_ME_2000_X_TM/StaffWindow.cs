﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitedStates_LibSyncOS_ME_2000_X_TM
{
    public partial class StaffWindow : Form
    {
        LibraryController LibraryController;

        public StaffWindow()
        {
            InitializeComponent();
        }

        public StaffWindow(LibraryController controller) : this()
        {
            this.LibraryController = controller;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
