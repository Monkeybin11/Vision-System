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
    public partial class FormIOMonitor : Form
    {
        #region private variable
        private InstantDoCtrl instantDoCtrl1 = new InstantDoCtrl();
        private InstantDiCtrl instantDiCtrl1 = new InstantDiCtrl();
        private SettingHelper settingHelper;
        private string strConfigFilePath;
        private IniFile configIniFile;
        private List<string> ioInputName = new List<string>();
        private List<string> ioOutputName = new List<string>();

        //input
        private Label[] m_portNumStaticDI;
        private Label[] m_portHexStaticDI;
        private ComboBox[,] m_portInputName;
        private PictureBox[,] m_pictrueBoxStaticDI;
        private const int m_startPort = 0;
        private const int m_portCountShow = 2;

        //output
        private Label[] m_portNum;
        private Label[] m_portHex;
        private ComboBox[,] m_portOutputName;
        private PictureBox[,] m_pictrueBox;

        public InstantDoCtrl InstantDoCtrl1 { get => instantDoCtrl1; set => instantDoCtrl1 = value; }
        public InstantDiCtrl InstantDiCtrl1 { get => instantDiCtrl1; set => instantDiCtrl1 = value; }

        #endregion

        public FormIOMonitor()
        {
            InitializeComponent();
        }

        public FormIOMonitor(int deviceNumber)
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

            // 初始化IO设置和监控
            //initialize static digital input port
            InstantDoCtrl1.SelectedDevice = new DeviceInformation(FormMain.settingHelper.IODeviceNumber);
            InstantDiCtrl1.SelectedDevice = new DeviceInformation(FormMain.settingHelper.IODeviceNumber);
            //The default device of project is demo device, users can choose other devices according to their needs. 
            if (!InstantDiCtrl1.Initialized)
            {
                MessageBox.Show("No device be selected or device open failed!", "StaticDI");
                // this.Close();
                return;
            }

            this.Text = "Static DI(" + InstantDiCtrl1.SelectedDevice.Description + ")";

            m_portInputName = new ComboBox[m_portCountShow, 8] {
                { IOInputLabel00, IOInputLabel01, IOInputLabel02, IOInputLabel03,
                    IOInputLabel04, IOInputLabel05, IOInputLabel06, IOInputLabel07 },
                { IOInputLabel10, IOInputLabel11, IOInputLabel12, IOInputLabel13,
                    IOInputLabel14, IOInputLabel15, IOInputLabel16, IOInputLabel17 },
            };

            m_portNumStaticDI = new Label[m_portCountShow] { StaticDIPortNum0, StaticDIPortNum1 };
            m_portHexStaticDI = new Label[m_portCountShow] { StaticDIPortHex0, StaticDIPortHex1 };
            m_pictrueBoxStaticDI = new PictureBox[m_portCountShow, 8]{
             {StaticDIpictureBox00, StaticDIpictureBox01, StaticDIpictureBox02, StaticDIpictureBox03,
                    StaticDIpictureBox04, StaticDIpictureBox05,StaticDIpictureBox06, StaticDIpictureBox07},
             {StaticDIpictureBox10, StaticDIpictureBox11, StaticDIpictureBox12, StaticDIpictureBox13,
                    StaticDIpictureBox14, StaticDIpictureBox15,StaticDIpictureBox16, StaticDIpictureBox17},
            };

            //enable the timer to read DI ports status
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Enabled = true;

            //initialize static digital output port
            //The default device of project is demo device, users can choose other devices according to their needs. 
            if (!InstantDoCtrl1.Initialized)
            {
                MessageBox.Show("No device be selected or device open failed!", "StaticDO");
                // this.Close();
                return;
            }

            m_portOutputName = new ComboBox[ConstVal.PortCountShow, 8]
            {
                {IOOutputLabel00, IOOutputLabel01, IOOutputLabel02, IOOutputLabel03,
                    IOOutputLabel04,  IOOutputLabel05, IOOutputLabel06, IOOutputLabel07},
                {IOOutputLabel10, IOOutputLabel11, IOOutputLabel12, IOOutputLabel13,
                    IOOutputLabel14, IOOutputLabel15, IOOutputLabel16, IOOutputLabel17},
            };

            m_portNum = new Label[ConstVal.PortCountShow] { PortNum0, PortNum1 };
            m_portHex = new Label[ConstVal.PortCountShow] { PortHex0, PortHex1 };
            m_pictrueBox = new PictureBox[ConstVal.PortCountShow, 8]{
             {pictureBox00, pictureBox01, pictureBox02, pictureBox03,
                    pictureBox04, pictureBox05,pictureBox06, pictureBox07},
             {pictureBox10, pictureBox11, pictureBox12, pictureBox13,
                    pictureBox14, pictureBox15,pictureBox16, pictureBox17},
            };

            // 初始化端口名称
            foreach (DataRow dr in FormMain.iOHelper.DataTableIOInfo.Rows)
            {
                if (dr["Type"].ToString() == "1" && dr["Name"].ToString() != "NA")
                    ioInputName.Add(dr["Name"].ToString());
                else if (dr["Type"].ToString() == "2" && dr["Name"].ToString() != "NA")
                    ioOutputName.Add(dr["Name"].ToString());
            }
            // 添加"NA"项
            ioInputName.Add("NA");
            ioOutputName.Add("NA");

            // 将端口名称添加到下拉列表中
            for (int i = 0; i < m_portCountShow; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < ioInputName.Count; k++)
                    {
                        m_portInputName[i, j].Items.Add(ioInputName[k]);
                    }
                    for (int k = 0; k < ioOutputName.Count; k++)
                    {
                        m_portOutputName[i, j].Items.Add(ioOutputName[k]);
                    }
                    m_portInputName[i, j].SelectedIndex = 0;
                    m_portOutputName[i, j].SelectedIndex = 0;
                }
            }
            // 初始化端口状态
            InitializePortState();
            // 初始化端口名称
            InitializePortName();
        }

        /// <summary>
        /// 初始化端口状态
        /// </summary>
        private void InitializePortState()
        {
            byte portData = 0;
            byte portDir = 0xFF;
            ErrorCode err = ErrorCode.Success;
            byte[] mask = InstantDoCtrl1.Features.DataMask;
            for (int i = 0; (i + ConstVal.StartPort) < InstantDoCtrl1.Features.PortCount && i < ConstVal.PortCountShow; ++i)
            {
                err = InstantDoCtrl1.Read(i + ConstVal.StartPort, out portData);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }

                m_portNum[i].Text = (i + ConstVal.StartPort).ToString();
                m_portHex[i].Text = portData.ToString("X2");

                if (InstantDoCtrl1.PortDirection != null)
                {
                    portDir = (byte)InstantDoCtrl1.PortDirection[i + ConstVal.StartPort].Direction;
                }

                // Set picture box state
                for (int j = 0; j < 8; ++j)
                {
                    if (((portDir >> j) & 0x1) == 0 || ((mask[i] >> j) & 0x1) == 0)  // Bit direction is input.
                    {
                        m_pictrueBox[i, j].Image = imageList2.Images[2];
                        m_pictrueBox[i, j].Enabled = false;
                    }
                    else
                    {
                        m_pictrueBox[i, j].Click += new EventHandler(PictureBox_Click);
                        m_pictrueBox[i, j].Tag = new DoBitInformation((portData >> j) & 0x1, i + ConstVal.StartPort, j);
                        m_pictrueBox[i, j].Image = imageList2.Images[(portData >> j) & 0x1];
                    }
                    m_pictrueBox[i, j].Invalidate();
                }
            }
        }

        /// <summary>
        /// 初始化端口的名称，从配置文件中获取
        /// </summary>
        private void InitializePortName()
        {
            for (int i = 0; i < ConstVal.PortCountShow; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // 执行条件查询语句
                    DataRow[] dataRows1, dataRows2;
                    dataRows1 = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 1 + " and PortNum = " + i + " and BitNum = " + j);
                    if (dataRows1.Length == 1)
                        m_portInputName[i, j].SelectedIndex = ioInputName.IndexOf(dataRows1[0]["Name"].ToString());
                    dataRows2 = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 2 + " and PortNum = " + i + " and BitNum = " + j);
                    if (dataRows2.Length == 1)
                        m_portOutputName[i, j].SelectedIndex = ioOutputName.IndexOf(dataRows2[0]["Name"].ToString());
                }
            }
        }

        /// <summary>
        /// 输出端口测试按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_Click(object sender, EventArgs e)
        {
            ErrorCode err = ErrorCode.Success;
            PictureBox box = (PictureBox)sender;
            DoBitInformation boxInfo = (DoBitInformation)box.Tag;

            boxInfo.BitValue = (~(int)(boxInfo).BitValue) & 0x1;
            box.Tag = boxInfo;
            box.Image = imageList2.Images[boxInfo.BitValue];
            box.Invalidate();

            // refresh hex
            int state = Int32.Parse(m_portHex[boxInfo.PortNum - ConstVal.StartPort].Text, NumberStyles.AllowHexSpecifier);
            state &= ~(0x1 << boxInfo.BitNum);
            state |= boxInfo.BitValue << boxInfo.BitNum;

            m_portHex[boxInfo.PortNum - ConstVal.StartPort].Text = state.ToString("X2");
            err = InstantDoCtrl1.Write(boxInfo.PortNum, (byte)state);
            if (err != ErrorCode.Success)
            {
                HandleError(err);
            }
        }

        /// <summary>
        /// 定时器更新IO端口状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // read Di port state
            byte portData = 0;
            ErrorCode err = ErrorCode.Success;

            for (int i = 0; (i + m_startPort) < InstantDiCtrl1.Features.PortCount && i < m_portCountShow; ++i)
            {
                err = InstantDiCtrl1.Read(i + m_startPort, out portData);
                if (err != ErrorCode.Success)
                {
                    timer1.Enabled = false;
                    HandleError(err);
                    return;
                }

                m_portNumStaticDI[i].Text = (i + m_startPort).ToString();
                m_portHexStaticDI[i].Text = portData.ToString("X2");

                // Set picture box state
                for (int j = 0; j < 8; ++j)
                {
                    m_pictrueBoxStaticDI[i, j].Image = imageList1.Images[(portData >> j) & 0x1];
                    m_pictrueBoxStaticDI[i, j].Invalidate();
                }
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
            string strName;
            string strCCDIndex;
            switch (btn.Name)
            {
                case "btnOK":
                    // 防错：检查comboBox是否有重复项，不允许有重复项，NA除外（NA可能有很多项）
                    List<string> inputnameList = new List<string>();
                    List<string> outputnameList = new List<string>();

                    for (int i = 0; i < ConstVal.PortCountShow; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            strName = m_portInputName[i, j].SelectedItem.ToString();
                            if (strName != "NA")
                                inputnameList.Add(strName);
                            strName = m_portOutputName[i, j].SelectedItem.ToString();
                            if (strName != "NA")
                                outputnameList.Add(strName);
                        }
                    }

                    if (CheckStringListDistinct(inputnameList) && CheckStringListDistinct(outputnameList))
                    {
                        for (int i = 0; i < ConstVal.PortCountShow; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                #region 输入端
                                strName = m_portInputName[i, j].SelectedItem.ToString();
                                // 执行条件查询语句
                                dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                                    "Type = " + 1 + " and PortNum = " + i + " and BitNum = " + j);
                                if (dataRows.Length >= 1)
                                {
                                    rowIndex = FormMain.iOHelper.DataTableIOInfo.Rows.IndexOf(dataRows[0]);
                                    FormMain.iOHelper.DataTableIOInfo.Rows[rowIndex]["Name"] = strName;
                                }
                                #endregion

                                #region 输出端
                                strName = m_portOutputName[i, j].SelectedItem.ToString();
                                // 执行条件查询语句
                                dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                                "Type = " + 2 + " and PortNum = " + i + " and BitNum = " + j);
                                if (dataRows.Length >= 1)
                                {
                                    rowIndex = FormMain.iOHelper.DataTableIOInfo.Rows.IndexOf(dataRows[0]);
                                    FormMain.iOHelper.DataTableIOInfo.Rows[rowIndex]["Name"] = strName;
                                }
                                #endregion
                            }
                        }
                        FormMain.iOHelper.WriteIOInfoToIniFile();
                        FormMain.iOHelper.LoadIODefinitionFromDataTable();
                        this.Close();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 更新端口的名称
        /// </summary>
        private void UpdatePortName()
        {
            ioInputName.Clear();
            ioOutputName.Clear();
            // 初始化端口名称
            foreach (DataRow dr in FormMain.iOHelper.DataTableIOInfo.Rows)
            {
                if (dr["Type"].ToString() == "1" && dr["Name"].ToString() != "NA")
                    ioInputName.Add(dr["Name"].ToString());
                else if (dr["Type"].ToString() == "2" && dr["Name"].ToString() != "NA")
                    ioOutputName.Add(dr["Name"].ToString());
            }
            // 添加"NA"项
            ioInputName.Add("NA");
            ioOutputName.Add("NA");

            for (int i = 0; i < m_portCountShow; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    m_portInputName[i, j].Items.Clear();
                    for (int k = 0; k < ioInputName.Count; k++)
                    {
                        m_portInputName[i, j].Items.Add(ioInputName[k]);
                    }

                    m_portOutputName[i, j].Items.Clear();
                    for (int k = 0; k < ioOutputName.Count; k++)
                    {
                        m_portOutputName[i, j].Items.Add(ioOutputName[k]);
                    }
                }
            }

            for (int i = 0; i < ConstVal.PortCountShow; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // 执行条件查询语句
                    DataRow[] dataRows1, dataRows2;
                    dataRows1 = FormMain.iOHelper.DataTableIOInfo.Select(
                        "Type = " + 1 + " and PortNum = " + i + " and BitNum = " + j);
                    if (dataRows1.Length == 1)
                    {
                        m_portInputName[i, j].SelectedIndex = ioInputName.IndexOf(dataRows1[0]["Name"].ToString());
                    }
                        dataRows2 = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 2 + " and PortNum = " + i + " and BitNum = " + j);
                    if (dataRows2.Length == 1)
                    {
                        m_portOutputName[i, j].SelectedIndex = ioOutputName.IndexOf(dataRows2[0]["Name"].ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 检查一个string List里面是否有重复字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool CheckStringListDistinct(List<string> list)
        {
            if (list.Distinct<string>().Count() < list.Count())
            {
                MessageBox.Show("两个不同端口不允许定义同一个信号！");
                return false;
            }
            return true;
        }

        private void HandleError(ErrorCode err)
        {
            if (err != ErrorCode.Success)
            {
                MessageBox.Show("Sorry ! Some errors happened, the error code is: " + err.ToString(), "Static DI");
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
