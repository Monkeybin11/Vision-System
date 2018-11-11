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
    public partial class FormCommAddressEdit : Form
    {
        private bool _isConfirmed = false;
        private int _index;
        private string _outputName;
        private short _value;
        private short _address;

        public bool IsConfirmed { get => _isConfirmed; set => _isConfirmed = value; }
        public int Index { get => _index; set => _index = value; }
        public string OutputName { get => _outputName; set => _outputName = value; }
        public short Value { get => _value; set => _value = value; }
        public short Address { get => _address; set => _address = value; }

        public FormCommAddressEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造体
        /// </summary>
        /// <param name="list"></param>
        /// <param name="camnum"></param>
        /// <param name="name"></param>
        public FormCommAddressEdit(string index, string name, string value, string address)
        {
            InitializeComponent();
            Index = Convert.ToInt32(index);
            OutputName = name;
            Value = Convert.ToInt16(value);
            Address = Convert.ToInt16(address);
        }

        /// <summary>
        /// 窗体加载时初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormProductName_Load(object sender, EventArgs e)
        {
            lblIndexNo.Text = Index.ToString();
            lblName.Text = OutputName.Length > 10 ? OutputName.Substring(0, 10) + "..." : OutputName;
            lblCurrentValue.Text = Value.ToString();
            txtStartAddress.Text = Address.ToString();
        }

        /// <summary>
        /// 多按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            string strAddress = txtStartAddress.Text;
            if (string.IsNullOrEmpty(strAddress))
            {
                MessageBox.Show("地址不能为空!");
                return;
            }
            Address = Convert.ToInt16(strAddress);
            IsConfirmed = true;
            this.Close();
        }
    }
}
