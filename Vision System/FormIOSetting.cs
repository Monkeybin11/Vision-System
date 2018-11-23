using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Automation.BDaq;

namespace Vision_System
{
    public partial class FormIOSetting : Form
    {
        #region private variable
        private InstantDoCtrl instantDoCtrl1 = new InstantDoCtrl();
        private InstantDiCtrl instantDiCtrl1 = new InstantDiCtrl();
        private SettingHelper settingHelper;
        private string strConfigFilePath;
        private IniFile configIniFile;

        //input
        private const int m_startPort = 0;
        private const int m_portCountShow = 2;

        //output
        public InstantDoCtrl InstantDoCtrl1 { get => instantDoCtrl1; set => instantDoCtrl1 = value; }
        public InstantDiCtrl InstantDiCtrl1 { get => instantDiCtrl1; set => instantDiCtrl1 = value; }

        #endregion

        public FormIOSetting()
        {
            InitializeComponent();
        }

        public FormIOSetting(int deviceNumber)
        {
            InitializeComponent();
            InstantDiCtrl1.SelectedDevice = new DeviceInformation(deviceNumber);
            InstantDoCtrl1.SelectedDevice = new DeviceInformation(deviceNumber);
        }

        public void SetSettingHelper(SettingHelper helper)
        {
            this.settingHelper = helper;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormIOSetting_Load(object sender, EventArgs e)
        {
            strConfigFilePath = Utility.GetThisExecutableDirectory() + "Config\\Setting.ini";
            // strIOSettingFilePath = strBaseDirectory + "Config\\IO Table.ini";
            configIniFile = new IniFile(strConfigFilePath);

            // 初始化DataGridView
            InitializeDataGridView();
        }

        /// <summary>
        /// 初始化总IO端口信息的DataGridView：初始化Column以及样式
        /// </summary>
        private void InitializeDataGridView()
        {
            // 设置表格样式
            DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            //dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font(
                "Microsoft YaHei", 7.8F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvIOInfoList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dgvIOInfoList.RowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvIOInfoList.BackgroundColor = System.Drawing.SystemColors.Control;
            dgvIOInfoList.GridColor = System.Drawing.SystemColors.Control;

            // 设置表格交替样式
            dgvIOInfoList.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            dgvIOInfoList.Columns[0].Width = 100;
            dgvIOInfoList.Columns[1].Width = (dgvIOInfoList.Width - 220)/ 2 - 5;
            dgvIOInfoList.Columns[2].Width = (dgvIOInfoList.Width - 220) / 2 - 5;
            dgvIOInfoList.Columns[3].Width = 120;

            // 设置单元格的值为"Y", 对应的checkbox为true, 单元格值为"N", 对应的checkbox为false
            //((DataGridViewCheckBoxColumn)dgvIOInfoList.Columns[0]).TrueValue = "Y";
            //((DataGridViewCheckBoxColumn)dgvIOInfoList.Columns[0]).FalseValue = "N";

            // 初始化表格数据
            try
            {
                dgvIOInfoList.Rows.Clear();
                foreach (DataRow dr in FormMain.iOHelper.DataTableIOInfo.Rows)
                {
                    object[] newRow = new object[4];
                    newRow[0] = (dr["Type"].ToString() == "1" ? "输入" : "输出");
                    newRow[1] = dr["PortNum"].ToString();
                    newRow[2] = dr["BitNum"].ToString();
                    newRow[3] = dr["Name"].ToString();
                    dgvIOInfoList.Rows.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("表格创建失败!");
            }
        }

        /// <summary>
        /// 多个按钮的点击触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMultiple_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            DataRow[] dataRows;
            int signalCnt;
            int rowIndex;
            switch (btn.Name)
            {
                case "btnAddTrigger":
                    FormPortBitSelect frmAddTrigger = new FormPortBitSelect();
                    if (frmAddTrigger.ShowDialog() == DialogResult.OK)
                    {
                        // 执行条件查询语句, 由于Trigger为输入信号，Type=1
                        // 计算Trigger的个数，新添加的Trigger在原有基础上+1
                        dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 1 + " and Name LIKE 'Trigger%'");
                        signalCnt = dataRows.Length;
                        dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 1 + " and PortNum = " + frmAddTrigger.PortIndex + " and BitNum = " + 
                            frmAddTrigger.BitIndex  + " and Name = 'NA'");
                        if (dataRows.Length < 1)
                        {
                            MessageBox.Show("该端口已经被占用，请选择其他端口！");
                        }
                        else
                        {
                            rowIndex = FormMain.iOHelper.DataTableIOInfo.Rows.IndexOf(dataRows[0]);
                            FormMain.iOHelper.DataTableIOInfo.Rows[rowIndex]["Name"] = "Trigger" + (signalCnt +1);
                            FormMain.iOHelper.WriteIOInfoToIniFile();
                            UpdateDataGridView();
                        }
                    }
                    break;
                case "btnDeleteTrigger":
                    // 执行条件查询语句, 由于Trigger为输入信号，Type=1
                    // 计算Trigger的个数，新添加的Trigger在原有基础上+1
                    dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 1 + " and Name LIKE 'Trigger%'");
                    signalCnt = dataRows.Length;
                    if (signalCnt < 1)
                    {
                        MessageBox.Show("Trigger端口数量小于1，不能删除！");
                        return;
                    }
                    dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 1 + " and Name = 'Trigger" + signalCnt + "'");

                    rowIndex = FormMain.iOHelper.DataTableIOInfo.Rows.IndexOf(dataRows[0]);
                    // 定义为NA
                    FormMain.iOHelper.DataTableIOInfo.Rows[rowIndex]["Name"] = "NA";
                    FormMain.iOHelper.WriteIOInfoToIniFile();
                    UpdateDataGridView();
                    break;
                case "btnAddLight":
                    FormPortBitSelect frmAddLight = new FormPortBitSelect();
                    if (frmAddLight.ShowDialog() == DialogResult.OK)
                    {
                        // 执行条件查询语句, 由于Light为输出信号，Type=2
                        // 计算Light的个数，新添加的Light在原有基础上+1
                        dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 2 + " and Name LIKE 'Light%'");
                        signalCnt = dataRows.Length;
                        dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 2 + " and PortNum = " + frmAddLight.PortIndex + " and BitNum = " + 
                            frmAddLight.BitIndex + " and Name = 'NA'");
                        if (dataRows.Length < 1)
                        {
                            MessageBox.Show("该端口已经被占用，请选择其他端口！");
                        }
                        else
                        {
                            rowIndex = FormMain.iOHelper.DataTableIOInfo.Rows.IndexOf(dataRows[0]);
                            FormMain.iOHelper.DataTableIOInfo.Rows[rowIndex]["Name"] = "Light" + (signalCnt + 1);
                            FormMain.iOHelper.WriteIOInfoToIniFile();
                            UpdateDataGridView();
                        }
                    }
                    break;
                case "btnDeleteLight":
                    // 执行条件查询语句, 由于Light为输出信号，Type=2
                    // 计算Light的个数，新添加的Light在原有基础上+1
                    dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 2 + " and Name LIKE 'Light%'");
                    signalCnt = dataRows.Length;
                    if (signalCnt < 1)
                    {
                        MessageBox.Show("Light端口数量小于1，不能删除！");
                        return;
                    }
                    dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 2 + " and Name = 'Light" + signalCnt + "'");

                    rowIndex = FormMain.iOHelper.DataTableIOInfo.Rows.IndexOf(dataRows[0]);
                    // 定义为NA
                    FormMain.iOHelper.DataTableIOInfo.Rows[rowIndex]["Name"] = "NA";
                    FormMain.iOHelper.WriteIOInfoToIniFile();
                    UpdateDataGridView();
                    break;
                case "btnAddOK":
                    FormPortBitSelect frmAddOK = new FormPortBitSelect();
                    if (frmAddOK.ShowDialog() == DialogResult.OK)
                    {
                        // 执行条件查询语句, 由于 OK 为输入信号，Type=2
                        // 计算 OK 的个数，新添加的 OK 在原有基础上+1
                        dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 2 + " and Name LIKE 'OK%'");
                        signalCnt = dataRows.Length;
                        dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 2 + " and PortNum = " + frmAddOK.PortIndex + " and BitNum = " + 
                            frmAddOK.BitIndex + " and Name = 'NA'");
                        if (dataRows.Length < 1)
                        {
                            MessageBox.Show("该端口已经被占用，请选择其他端口！");
                        }
                        else
                        {
                            rowIndex = FormMain.iOHelper.DataTableIOInfo.Rows.IndexOf(dataRows[0]);
                            FormMain.iOHelper.DataTableIOInfo.Rows[rowIndex]["Name"] = "OK" + (signalCnt + 1);
                            FormMain.iOHelper.WriteIOInfoToIniFile();
                            UpdateDataGridView();
                        }
                    }
                    break;
                case "btnDeleteOK":
                    // 执行条件查询语句, 由于 OK 为输入信号，Type=2
                    // 计算 OK 的个数，新添加的 OK 在原有基础上+1
                    dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 2 + " and Name LIKE 'OK%'");
                    signalCnt = dataRows.Length;
                    if (signalCnt < 1)
                    {
                        MessageBox.Show("OK端口数量小于1，不能删除！");
                        return;
                    }
                    dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 2 + " and Name = 'OK" + signalCnt + "'");

                    rowIndex = FormMain.iOHelper.DataTableIOInfo.Rows.IndexOf(dataRows[0]);
                    // 定义为NA
                    FormMain.iOHelper.DataTableIOInfo.Rows[rowIndex]["Name"] = "NA";
                    FormMain.iOHelper.WriteIOInfoToIniFile();
                    UpdateDataGridView();
                    break;
                case "btnAddNG":
                    FormPortBitSelect frmAddNG = new FormPortBitSelect();
                    if (frmAddNG.ShowDialog() == DialogResult.OK)
                    {
                        // 执行条件查询语句, 由于 NG 为输入信号，Type=2
                        // 计算 NG 的个数，新添加的 NG 在原有基础上+1
                        dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 2 + " and Name LIKE 'NG%'");
                        signalCnt = dataRows.Length;
                        dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 2 + " and PortNum = " + frmAddNG.PortIndex + " and BitNum = " + 
                            frmAddNG.BitIndex + " and Name = 'NA'");
                        if (dataRows.Length < 1)
                        {
                            MessageBox.Show("该端口已经被占用，请选择其他端口！");
                        }
                        else
                        {
                            rowIndex = FormMain.iOHelper.DataTableIOInfo.Rows.IndexOf(dataRows[0]);
                            FormMain.iOHelper.DataTableIOInfo.Rows[rowIndex]["Name"] = "NG" + (signalCnt + 1);
                            FormMain.iOHelper.WriteIOInfoToIniFile();
                            UpdateDataGridView();
                        }
                    }
                    break;
                case "btnDeleteNG":
                    // 执行条件查询语句, 由于 NG 为输入信号，Type=2
                    // 计算 NG 的个数，新添加的 NG 在原有基础上+1
                    dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 2 + " and Name LIKE 'NG%'");
                    signalCnt = dataRows.Length;
                    if (signalCnt < 1)
                    {
                        MessageBox.Show("NG端口数量小于1，不能删除！");
                        return;
                    }
                    dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 2 + " and Name = 'NG" + signalCnt + "'");

                    rowIndex = FormMain.iOHelper.DataTableIOInfo.Rows.IndexOf(dataRows[0]);
                    // 定义为NA
                    FormMain.iOHelper.DataTableIOInfo.Rows[rowIndex]["Name"] = "NA";
                    FormMain.iOHelper.WriteIOInfoToIniFile();
                    UpdateDataGridView();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 更新IO表格
        /// </summary>
        private void UpdateDataGridView()
        {
            // 更新表格数据
            try
            {
                dgvIOInfoList.Rows.Clear();
                foreach (DataRow dr in FormMain.iOHelper.DataTableIOInfo.Rows)
                {
                    object[] newRow = new object[4];
                    newRow[0] = (dr["Type"].ToString() == "1" ? "输入" : "输出");
                    newRow[1] = dr["PortNum"].ToString();
                    newRow[2] = dr["BitNum"].ToString();
                    newRow[3] = dr["Name"].ToString();
                    dgvIOInfoList.Rows.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("表格创建失败!");
            }
        }

        public struct DoBitInformation
        {
            #region fields
            private int m_bitValue;
            private int m_portNum;
            private int m_bitNum;
            #endregion

            public DoBitInformation(int bitvalue, int portNum, int bitNum)
            {
                m_bitValue = bitvalue;
                m_portNum = portNum;
                m_bitNum = bitNum;
            }

            #region Properties
            public int BitValue
            {
                get { return m_bitValue; }
                set
                {
                    m_bitValue = value & 0x1;
                }
            }
            public int PortNum
            {
                get { return m_portNum; }
                set
                {
                    if ((value - ConstVal.StartPort) >= 0
                       && (value - ConstVal.StartPort) <= (ConstVal.PortCountShow - 1))
                    {
                        m_portNum = value;
                    }
                }
            }
            public int BitNum
            {
                get { return m_bitNum; }
                set
                {
                    if (value >= 0 && value <= 7)
                    {
                        m_bitNum = value;
                    }
                }
            }
            #endregion
        }

        public static class ConstVal
        {
            public const int StartPort = 0;
            public const int PortCountShow = 2;
        }
    }
}
