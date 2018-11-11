using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision_System
{
    public partial class FormPortBitSelect : Form
    {
        private int _portIndex, _bitIndex;

        public int PortIndex { get => _portIndex; set => _portIndex = value; }
        public int BitIndex { get => _bitIndex; set => _bitIndex = value; }

        public FormPortBitSelect()
        {
            InitializeComponent();
        }

        private void FormPortBitSelect_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                cmbPortSelect.Items.Add(i);
            }
            for (int i = 0; i < 8; i++)
            {
                cmbBitSelect.Items.Add(i);
            }
            cmbPortSelect.SelectedIndex = 0;
            cmbBitSelect.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PortIndex = Convert.ToInt32(cmbPortSelect.SelectedItem.ToString());
            BitIndex = Convert.ToInt32(cmbBitSelect.SelectedItem.ToString());
        }
    }
}
