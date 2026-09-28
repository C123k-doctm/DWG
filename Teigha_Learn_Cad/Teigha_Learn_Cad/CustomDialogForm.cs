using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Teigha_Learn_Cad
{
    public partial class CustomDialogForm : Form
    {
        public string SelectedOption { get; private set; }

        public CustomDialogForm()
        {
            InitializeComponent();
        }

        private void btnWireDiagram_Click(object sender, EventArgs e)
        {
            SelectedOption = "线束图";
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnElectricalDiagram_Click(object sender, EventArgs e)
        {
            SelectedOption = "电器图";
            DialogResult = DialogResult.OK;
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
