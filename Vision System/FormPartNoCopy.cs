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
    public partial class FormPartNoCopy : Form
    {
        private bool _isFinishGoodPartNo = true;
        private int _camNum = 1;
        private string _strProductName = "";
        private bool _isConfirmed = false;
        private List<string> productList = new List<string>();

        public string strProductName { get => _strProductName; set => _strProductName = value; }
        public bool IsConfirmed { get => _isConfirmed; set => _isConfirmed = value; }
        public bool IsFinishGoodPartNo { get => _isFinishGoodPartNo; set => _isFinishGoodPartNo = value; }
        public int CamNum { get => _camNum; set => _camNum = value; }

        public FormPartNoCopy()
        {
            InitializeComponent();
        }

        // Copy料号时用该函数构造体
        public FormPartNoCopy(int newIndex, List<string> list)
        {
            InitializeComponent();
            strProductName = "Product" + newIndex;
            productList = list;
        }

        // Rename料号时用该函数构造体
        public FormPartNoCopy(string newName, List<string> list)
        {
            InitializeComponent();
            strProductName = newName;
            productList = list;
        }

        /// <summary>
        /// 窗体加载时初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormProductName_Load(object sender, EventArgs e)
        {
            txtProductName.Text = strProductName;
        }

        /// <summary>
        /// 多按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            string strName = txtProductName.Text;
            if (string.IsNullOrEmpty(strName))
            {
                MessageBox.Show("料号名称不能为空!");
                return;
            }
            else if (productList.IndexOf(strName) != -1)
            {
                MessageBox.Show("料号名称已存在，请更改其他名字!");
                return;
            }
            else
            {
                strProductName = txtProductName.Text;
            }
            IsConfirmed = true;
            this.Close();
        }
    }
}
