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
    public partial class FormFailureSelect : Form
    {
        private List<string> _failureList;
        private string _failureKWSelected;

        public List<string> FailureList { get => _failureList; set => _failureList = value; }
        public string FailureKWSelected { get => _failureKWSelected; set => _failureKWSelected = value; }

        public FormFailureSelect()
        {
            InitializeComponent();
        }

        public FormFailureSelect(List<string> list)
        {
            InitializeComponent();
            FailureList = list;
        }

        private void FormCommDataSelect_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < FailureList.Count; i++)
            {
                cmbFailureSelect.Items.Add(FailureList[i]);
            }
            cmbFailureSelect.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FailureKWSelected = cmbFailureSelect.SelectedItem.ToString();
        }
    }
}
