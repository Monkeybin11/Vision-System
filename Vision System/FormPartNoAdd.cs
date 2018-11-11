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
    public partial class FormPartNoAdd : Form
    {
        private bool _isFinishGoodPartNo = true;
        private int _ccdIndex = 0;
        private int _camNum = 1;
        private string _strProductName = "";
        private bool _isConfirmed = false;
        private List<string> productList = new List<string>();

        public string strProductName { get => _strProductName; set => _strProductName = value; }
        public bool IsConfirmed { get => _isConfirmed; set => _isConfirmed = value; }
        public bool IsFinishGoodPartNo { get => _isFinishGoodPartNo; set => _isFinishGoodPartNo = value; }
        public int CamNum { get => _camNum; set => _camNum = value; }
        public int CcdIndex { get => _ccdIndex; set => _ccdIndex = value; }

        public FormPartNoAdd()
        {
            InitializeComponent();
        }

        // Add料号时用该函数构造体
        public FormPartNoAdd(List<string> list, int camnum)
        {
            InitializeComponent();
            productList = list;
            CamNum = camnum;
        }

        /// <summary>
        /// 窗体加载时初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormProductName_Load(object sender, EventArgs e)
        {
            txtPartNoName.Text = strProductName;
            cmbPartNoType.Items.Add("成品料号");
            cmbPartNoType.Items.Add("单个相机料号");
            cmbPartNoType.SelectedIndex = 0;

            for (int i = 0; i < CamNum; i++)
            {
                cmbCameraSelect.Items.Add("CCD" + (i + 1));
            }
            cmbCameraSelect.SelectedIndex = 0;

            // 更新窗口大小和控件是否可见
            UpdateFormComponent(true);
        }

        private void cmbPartNoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPartNoType.SelectedIndex ==0)
            {
                labelCameraSelect.Visible = false;
                cmbCameraSelect.Visible = false;
                UpdateFormComponent(true);
            }
            else
            {
                labelCameraSelect.Visible = true;
                cmbCameraSelect.Visible = true;
                UpdateFormComponent(false);
            }
        }

        /// <summary>
        /// 根据是否选择了成品料号，更新界面
        /// </summary>
        /// <param name="select"></param>
        private void UpdateFormComponent(bool select)
        {
            if (select)
            {
                this.ClientSize = new System.Drawing.Size(300, 160);
                labelCameraSelect.Visible = false;
                cmbCameraSelect.Visible = false;
                this.label_PartNoName.Location = new System.Drawing.Point(this.label1.Location.X, 
                    this.label1.Location.Y + 44);
                this.txtPartNoName.Location = new System.Drawing.Point(this.cmbPartNoType.Location.X, 
                    this.cmbPartNoType.Location.Y + 44);
                this.btnOK.Location = new System.Drawing.Point(this.txtPartNoName.Location.X, 
                    this.txtPartNoName.Location.Y + 44);
            }
            else
            {
                this.ClientSize = new System.Drawing.Size(300, 205);
                labelCameraSelect.Visible = true;
                cmbCameraSelect.Visible = true;
                this.label_PartNoName.Location = new System.Drawing.Point(this.labelCameraSelect.Location.X,
                    this.labelCameraSelect.Location.Y + 44);
                this.txtPartNoName.Location = new System.Drawing.Point(this.cmbCameraSelect.Location.X,
                    this.cmbCameraSelect.Location.Y + 44);
                this.btnOK.Location = new System.Drawing.Point(this.txtPartNoName.Location.X,
                    this.txtPartNoName.Location.Y + 44);
            }
        }

        /// <summary>
        /// 多按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            string strName = txtPartNoName.Text;
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
                IsFinishGoodPartNo = cmbPartNoType.SelectedIndex == 0 ? true : false;
                // 如果选择的是单个CCD的料号，获取CCD的序号
                CcdIndex = cmbCameraSelect.SelectedIndex;
                strProductName = txtPartNoName.Text;
            }
            IsConfirmed = true;
            this.Close();
        }
    }
}
