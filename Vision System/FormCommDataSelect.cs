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
    public partial class FormCommDataSelect : Form
    {
        private List<string> _dataList;
        private string _dataNameSelected;

        public List<string> DataList { get => _dataList; set => _dataList = value; }
        public string DataNameSelected { get => _dataNameSelected; set => _dataNameSelected = value; }

        public FormCommDataSelect()
        {
            InitializeComponent();
        }

        public FormCommDataSelect(List<string> data)
        {
            InitializeComponent();
            DataList = data;
        }

        private void FormCommDataSelect_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < DataList.Count; i++)
            {
                cmbDataSelect.Items.Add(DataList[i]);
            }
            cmbDataSelect.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DataNameSelected = cmbDataSelect.SelectedItem.ToString();
        }
    }
}
