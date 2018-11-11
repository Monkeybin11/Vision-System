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
    public partial class FormPartNoEdit: Form
    {
        private int _ccdIndex = 0;
        private int _camNum = 1;
        private int _partNoIndex = 0;
        private string _strProductName = "";
        private bool _isConfirmed = false;
        private DataRow _dataRowEdit;
        private List<string> productList = new List<string>(); // 成品料号列表
        private List<string>[] ccdPartNoList; // 单个CCD料号列表
        private Label[] labelCCDPartNo;
        private ComboBox[] cmbCCDPartNo;

        public string strProductName { get => _strProductName; set => _strProductName = value; }
        public bool IsConfirmed { get => _isConfirmed; set => _isConfirmed = value; }
        public int CamNum { get => _camNum; set => _camNum = value; }
        public int CcdIndex { get => _ccdIndex; set => _ccdIndex = value; }
        public List<string>[] CcdPartNoList { get => ccdPartNoList; set => ccdPartNoList = value; }
        public DataRow DataRowEdit { get => _dataRowEdit; set => _dataRowEdit = value; }
        public int PartNoIndex { get => _partNoIndex; set => _partNoIndex = value; }

        public FormPartNoEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造体
        /// </summary>
        /// <param name="list"></param>
        /// <param name="camnum"></param>
        /// <param name="name"></param>
        public FormPartNoEdit(DataRow row, List<string> list, List<string>[] ccdList,int camnum, string name)
        {
            InitializeComponent();
            DataRowEdit = row;
            productList = list;
            ccdPartNoList = ccdList;
            CamNum = camnum;
            strProductName = name;
            PartNoIndex = (int)DataRowEdit["PNIndex"];
        }

        /// <summary>
        /// 窗体加载时初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormProductName_Load(object sender, EventArgs e)
        {
            int pointX, pointY;
            int width, height;
            txtProductName.Text = strProductName;

            // 窗体大小
            width = this.ClientSize.Width;
            height = this.ClientSize.Height;
            this.ClientSize = new System.Drawing.Size(width, height + 40 * CamNum);

            // 添加CCD 料号的label
            labelCCDPartNo = new Label[CamNum];
            pointX = lblPartNo.Location.X;
            pointY = lblPartNo.Location.Y;
            for (int i = 0; i < CamNum; i++)
            {
                labelCCDPartNo[i] = new Label();
                // 
                // lblCCD1PartNo
                // 
                labelCCDPartNo[i].Font = new System.Drawing.Font("Microsoft YaHei", 
                    7.8F, 
                    System.Drawing.FontStyle.Regular, 
                    System.Drawing.GraphicsUnit.Point, 
                    ((byte)(0)));
                labelCCDPartNo[i].Location = new System.Drawing.Point(pointX, pointY + 40 * (i + 1));
                labelCCDPartNo[i].Name = "lblCCD" + (i +1) + "PartNo";
                labelCCDPartNo[i].AutoSize = true;
                labelCCDPartNo[i].TabIndex = 10 + i;
                labelCCDPartNo[i].Text = "CCD" + (i +1) + "单工位料号：";
                // 添加控件
                this.Controls.Add(this.labelCCDPartNo[i]);
            }

            // 添加料号选择的comboBox
            cmbCCDPartNo = new ComboBox[CamNum];
            pointX = txtProductName.Location.X;
            pointY = txtProductName.Location.Y;
            for (int i = 0; i < CamNum; i++)
            {
                cmbCCDPartNo[i] = new ComboBox();
                // 
                // cmbCCD1PartNo
                // 
                cmbCCDPartNo[i].DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                cmbCCDPartNo[i].Font = new System.Drawing.Font("Microsoft YaHei", 
                    7.8F, 
                    System.Drawing.FontStyle.Regular, 
                    System.Drawing.GraphicsUnit.Point, 
                    ((byte)(0)));
                cmbCCDPartNo[i].FormattingEnabled = true;
                cmbCCDPartNo[i].Location = new System.Drawing.Point(pointX, pointY + 40 * (i + 1));
                cmbCCDPartNo[i].Name = "cmbCCD" + (i +1) + "PartNo";
                cmbCCDPartNo[i].Size = new System.Drawing.Size(txtProductName.Width, txtProductName.Height);
                cmbCCDPartNo[i].TabIndex = 10 + CamNum + i;

                // 初始化下拉列表项目
                // 列表为空，添加"NA"项
                if (ccdPartNoList[i].Count == 0)
                {
                    cmbCCDPartNo[i].Items.Add("NA");
                    cmbCCDPartNo[i].SelectedIndex = 0;
                }
                else
                {
                    foreach (string item in ccdPartNoList[i])
                    {
                        cmbCCDPartNo[i].Items.Add(item);
                    }
                    cmbCCDPartNo[i].SelectedIndex = ccdPartNoList[i].IndexOf(DataRowEdit["PNCCD" + (i + 1)].ToString());
                }

                // 添加控件
                this.Controls.Add(this.cmbCCDPartNo[i]);
            }

            // 调整OK按钮的位置
            btnOK.Location = new System.Drawing.Point(pointX, pointY + 40 * (CamNum + 1) + 10);
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

            strProductName = txtProductName.Text;
            DataRowEdit["PNName"] = strProductName;
            for (int i = 0; i < CamNum; i++)
            {
                DataRowEdit["PNCCD" + (i + 1)] = cmbCCDPartNo[i].SelectedItem.ToString();
            }
            IsConfirmed = true;
            this.Close();
        }
    }
}
