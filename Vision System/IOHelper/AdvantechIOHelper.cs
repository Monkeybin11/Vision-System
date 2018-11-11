using Automation.BDaq;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;

namespace Vision_System
{
    public class AdvantechIOHelper
    {
        private const int PORTCNT = 2;
        private const int BITCNT = 8;
        private IniFile IOSettingIniFile;
        private InstantDoCtrl instantDoCtrl1 = null;
        private InstantDiCtrl instantDiCtrl1 = null;
        private int _camNum = 2;
        private int _deviceNum = 0;
        private bool _isIODeviceConnected;
        private byte _failureCode = 0;
        private TriggerEdge _trigEdge = TriggerEdge.Rising;
        private ValidInput _validInputType = ValidInput.High;
        private ValidOutput _validOutputType = ValidOutput.High;
        private bool[] _isTriggerSignalHigh;
        private bool _isChangeoverEnableSignalHigh = false;
        private bool _isPLCOnline = true;

        // 定义DataTable存储IO信号的所有配置信息
        // DataTable定义格式如下：
        // 输入还是输出           端口号       位号             信号名称           对应的CCD序号(从1开始计算)
        // Input/output         Port no.    Bit no.          Signal name        CCD index
        // <int>                <int>       <int>            <string>           <int>
        //  input:1/output:2    0~1         0~7                 ""              1~mCamNum
        private DataTable _dataTableIOInfo = new DataTable("IOInfo");

        #region PCI input / output port and bit num definition
        // General input
        private int _plcOnline_PortNum = 0;
        private int _plcOnline_BitNum = 0;
        private int _ChangeoverEnable_PortNum = 0;
        private int _ChangeoverBit1_Portnum = 0;
        private int _ChangeoverBit2_Portnum = 0;
        private int _ChangeoverBit3_Portnum = 0;
        private int _ChangeoverBit4_Portnum = 0;
        private int _ChangeoverBit5_Portnum = 0;

        private int _ChangeoverEnable_Bitnum = 0;
        private int _ChangeoverBit1_Bitnum = 0;
        private int _ChangeoverBit2_Bitnum = 0;
        private int _ChangeoverBit3_Bitnum = 0;
        private int _ChangeoverBit4_Bitnum = 0;
        private int _ChangeoverBit5_Bitnum = 0;

        // General output
        private int _CCDOnline_PortNum = 0;
        private int _CCDOnline_BitNum = 0;

        // Input and output for single camera
        public SingleCameraIO[] mSingleCameraIO;

        #endregion

        public int DeviceNum { get => _deviceNum; set => _deviceNum = value; }
        public bool IsIODeviceConnected { get => _isIODeviceConnected; set => _isIODeviceConnected = value; }
        public byte FailureCode { get => _failureCode; set => _failureCode = value; }
        public int CCDOnline_PortNum { get => _CCDOnline_PortNum; set => _CCDOnline_PortNum = value; }
        public int ChangeoverEnable_PortNum { get => _ChangeoverEnable_PortNum; set => _ChangeoverEnable_PortNum = value; }
        public int ChangeoverBit1_Portnum { get => _ChangeoverBit1_Portnum; set => _ChangeoverBit1_Portnum = value; }
        public int ChangeoverBit2_Portnum { get => _ChangeoverBit2_Portnum; set => _ChangeoverBit2_Portnum = value; }
        public int ChangeoverBit3_Portnum { get => _ChangeoverBit3_Portnum; set => _ChangeoverBit3_Portnum = value; }
        public int ChangeoverBit4_Portnum { get => _ChangeoverBit4_Portnum; set => _ChangeoverBit4_Portnum = value; }
        public int ChangeoverBit5_Portnum { get => _ChangeoverBit5_Portnum; set => _ChangeoverBit5_Portnum = value; }
        public int CCDOnline_BitNum { get => _CCDOnline_BitNum; set => _CCDOnline_BitNum = value; }
        public int ChangeoverEnable_Bitnum { get => _ChangeoverEnable_Bitnum; set => _ChangeoverEnable_Bitnum = value; }
        public int ChangeoverBit1_Bitnum { get => _ChangeoverBit1_Bitnum; set => _ChangeoverBit1_Bitnum = value; }
        public int ChangeoverBit2_Bitnum { get => _ChangeoverBit2_Bitnum; set => _ChangeoverBit2_Bitnum = value; }
        public int ChangeoverBit3_Bitnum { get => _ChangeoverBit3_Bitnum; set => _ChangeoverBit3_Bitnum = value; }
        public int ChangeoverBit4_Bitnum { get => _ChangeoverBit4_Bitnum; set => _ChangeoverBit4_Bitnum = value; }
        public int ChangeoverBit5_Bitnum { get => _ChangeoverBit5_Bitnum; set => _ChangeoverBit5_Bitnum = value; }
        public int CamNum { get => _camNum; set => _camNum = value; }
        public TriggerEdge TrigEdge { get => _trigEdge; set => _trigEdge = value; }
        public ValidInput ValidInputType { get => _validInputType; set => _validInputType = value; }
        public ValidOutput ValidOutputType { get => _validOutputType; set => _validOutputType = value; }
        public bool[] IsTriggerSignalHigh { get => _isTriggerSignalHigh; set => _isTriggerSignalHigh = value; }
        public bool IsChangeoverEnableSignalHigh { get => _isChangeoverEnableSignalHigh; set => _isChangeoverEnableSignalHigh = value; }
        public DataTable DataTableIOInfo { get => _dataTableIOInfo; set => _dataTableIOInfo = value; }
        public int PlcOnline_PortNum { get => _plcOnline_PortNum; set => _plcOnline_PortNum = value; }
        public int PlcOnline_BitNum { get => _plcOnline_BitNum; set => _plcOnline_BitNum = value; }
        public bool IsPLCOnline { get => _isPLCOnline; set => _isPLCOnline = value; }

        public AdvantechIOHelper()
        {

        }

        /// <summary>
        /// 初始化IO定义
        /// </summary>
        /// <param name="strIOSettingIniFile"></param>
        public void InitializeIOTable(string strIOSettingIniFile)
        {
            IOSettingIniFile = new IniFile(strIOSettingIniFile);
            try
            {
                // IO Datatable初始化
                // 注意Column name不能有空格，否则在查询语句中会报错
                DataTableIOInfo.Columns.Add("Type", typeof(Int32));
                DataTableIOInfo.Columns.Add("PortNum", typeof(Int32));
                DataTableIOInfo.Columns.Add("BitNum", typeof(Int32));
                DataTableIOInfo.Columns.Add("Name", typeof(string));

                // 从配置文件中获取
                for (int i = 0; i < PORTCNT; i++)
                {
                    for (int j = 0; j < BITCNT; j++)
                    {
                        string strinput, stroutput;
                        strinput = IOSettingIniFile.IniReadValue("IOInfoTable", "(1" + "," + i + "," + j + ")");
                        DataRow row1 =  DataTableIOInfo.NewRow();
                        row1[0] = 1;
                        row1[1] = i;
                        row1[2] = j;
                        row1[3] = strinput;
                        DataTableIOInfo.Rows.Add(row1);
                        stroutput = IOSettingIniFile.IniReadValue("IOInfoTable", "(2" + "," + i + "," + j + ")");
                        DataRow row2 = DataTableIOInfo.NewRow();
                        row2[0] = 2;
                        row2[1] = i;
                        row2[2] = j;
                        row2[3] = stroutput;
                        DataTableIOInfo.Rows.Add(row2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Read config file fails", "Fails:" + ex);
            }
        }

        // 初始化IO端口的数字
        public void InitializeIODefinition()
        {
            // 根据相机数量初始化mSingleCameraIO变量
            mSingleCameraIO = new SingleCameraIO[CamNum];
            try
            {
                // 读取IO端口的定义
                DataRow[] dataRows;
                for (int i = 0; i < CamNum; i++)
                {
                    mSingleCameraIO[i] = new SingleCameraIO();
                    // 触发信号                    
                    dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = '" +
                        FormMain.jobHelper[i].TriggerSourceName + "'");
                    if (dataRows.Length >= 1)
                    {
                        mSingleCameraIO[i].Trigger_Portnum = (int)dataRows[0]["PortNum"];
                        mSingleCameraIO[i].Trigger_Bitnum = (int)dataRows[0]["BitNum"];
                    }

                    // 光源信号
                    dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = '" +
                        FormMain.jobHelper[i].LightSourceName + "'");
                    if (dataRows.Length >= 1)
                    {
                        mSingleCameraIO[i].LightSource_Portnum = (int)dataRows[0]["PortNum"];
                        mSingleCameraIO[i].LightSource_Bitnum = (int)dataRows[0]["BitNum"];
                    }

                    // OK信号
                    dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = 'OK" + (i + 1) + "'");
                    if (dataRows.Length >= 1)
                    {
                        mSingleCameraIO[i].OK_Portnum = (int)dataRows[0]["PortNum"];
                        mSingleCameraIO[i].OK_Bitnum = (int)dataRows[0]["BitNum"];
                    }

                    // NG信号
                    dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = 'NG" + (i + 1) + "'");
                    if (dataRows.Length >= 1)
                    {
                        mSingleCameraIO[i].NG_Portnum = (int)dataRows[0]["PortNum"];
                        mSingleCameraIO[i].NG_Bitnum = (int)dataRows[0]["BitNum"];
                    }

                    // 检测完成信号
                    dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = 'Complete" + (i + 1) + "'");
                    if (dataRows.Length >= 1)
                    {
                        mSingleCameraIO[i].InspectComplet_PortNum = (int)dataRows[0]["PortNum"];
                        mSingleCameraIO[i].InspectComplet_BitNum = (int)dataRows[0]["BitNum"];
                    }
                }

                // PLC online 信号
                dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'PLCOnline'");
                if (dataRows.Length >= 1)
                {
                    PlcOnline_PortNum = (int)dataRows[0]["PortNum"];
                    PlcOnline_BitNum = (int)dataRows[0]["BitNum"];
                }

                // 换型信号
                dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverEnable'");
                if (dataRows.Length >= 1)
                {
                    ChangeoverEnable_PortNum = (int)dataRows[0]["PortNum"];
                    ChangeoverEnable_Bitnum = (int)dataRows[0]["BitNum"];
                }

                // 换型二进制位信号
                dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverBit1'");
                if (dataRows.Length >= 1)
                {
                    ChangeoverBit1_Portnum = (int)dataRows[0]["PortNum"];
                    ChangeoverBit1_Bitnum = (int)dataRows[0]["BitNum"];
                }

                // 换型二进制位信号
                dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverBit2'");
                if (dataRows.Length >= 1)
                {
                    ChangeoverBit2_Portnum = (int)dataRows[0]["PortNum"];
                    ChangeoverBit2_Bitnum = (int)dataRows[0]["BitNum"];
                }

                // 换型二进制位信号
                dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverBit3'");
                if (dataRows.Length >= 1)
                {
                    ChangeoverBit3_Portnum = (int)dataRows[0]["PortNum"];
                    ChangeoverBit3_Bitnum = (int)dataRows[0]["BitNum"];
                }

                // 换型二进制位信号
                dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverBit4'");
                if (dataRows.Length >= 1)
                {
                    ChangeoverBit4_Portnum = (int)dataRows[0]["PortNum"];
                    ChangeoverBit4_Bitnum = (int)dataRows[0]["BitNum"];
                }

                // 换型二进制位信号
                dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverBit5'");
                if (dataRows.Length >= 1)
                {
                    ChangeoverBit5_Portnum = (int)dataRows[0]["PortNum"];
                    ChangeoverBit5_Bitnum = (int)dataRows[0]["BitNum"];
                }

                // CCD online 信号
                dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = 'CCDOnline'");
                if (dataRows.Length >= 1)
                {
                    CCDOnline_PortNum = (int)dataRows[0]["PortNum"];
                    CCDOnline_BitNum = (int)dataRows[0]["BitNum"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Read config file fails", "Fails:" + ex);
            }
        }

        /// <summary>
        /// 由于触发和唤醒使能信号是捕捉脉冲上升沿或者下降沿，需要获取初始化的端口状态，方便后面作比较
        /// </summary>
        public void ReadInitialPortStatus()
        {
            IsTriggerSignalHigh = new bool[CamNum];
            byte data;
            // 初始化触发信号状态
            for (int i = 0; i < CamNum; i++)
            {
                instantDiCtrl1.ReadBit(mSingleCameraIO[i].Trigger_Portnum, mSingleCameraIO[i].Trigger_Bitnum, out data);
                // 将当前的IO数据保存起来
                IsTriggerSignalHigh[i] = data == 1 ? true : false;
            }
            // 初始化换型使能信号状态
            instantDiCtrl1.ReadBit(ChangeoverEnable_PortNum, ChangeoverEnable_Bitnum, out data);
            IsChangeoverEnableSignalHigh = data == 1 ? true : false;

            // 初始化PLC在线信号状态
            instantDiCtrl1.ReadBit(PlcOnline_PortNum, PlcOnline_BitNum, out data);
            // 初始化PLC在线信号状态
            switch (ValidInputType)
            {
                case ValidInput.High:
                    IsPLCOnline = (data == 1 ? true : false);
                    break;
                case ValidInput.Low:
                    IsPLCOnline = (data == 0 ? true : false);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 初始化输出端口的状态并写入
        /// </summary>
        public void WriteInitialPortStatus()
        {
            // 初始化光源信号，如果是常亮信号，则在软件初始化的时候打开，直至软件关闭
            for (int i = 0; i < CamNum; i++)
            {
                if (FormMain.jobHelper[i].IsLightControl && !FormMain.jobHelper[i].IsLightFlash)
                {
                    TurnOnLight(i);
                }
            }
        }

        /// <summary>
        /// 将DataTable信息加载到IO端口定义中
        /// </summary>
        public void LoadIODefinitionFromDataTable()
        {
            // 读取IO端口的定义
            DataRow[] dataRows;
            string strName;
            for (int i = 0; i < CamNum; i++)
            {
                // 触发信号 
                switch (FormMain.jobHelper[i].TriggerMode)
                {
                    case TriggerMode.FirstOrIndependentTrigger:
                        // 如果是单独触发信号，TriggerSourceName = TriggerN
                        dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = '" +
                            FormMain.jobHelper[i].TriggerSourceName + "'");
                        if (dataRows.Length >= 1)
                        {
                            mSingleCameraIO[i].Trigger_Portnum = (int)dataRows[0]["PortNum"];
                            mSingleCameraIO[i].Trigger_Bitnum = (int)dataRows[0]["BitNum"];
                        }
                        break;
                    case TriggerMode.AttachedTrigger:
                        // 如果是单独触发信号，TriggerSourceName = TriggerNCompleted，需要对字符串进行裁切
                        strName = FormMain.jobHelper[i].TriggerSourceName;
                        dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = '" +
                            strName.Remove(strName.Length - 9) + "'");
                        if (dataRows.Length >= 1)
                        {
                            mSingleCameraIO[i].Trigger_Portnum = (int)dataRows[0]["PortNum"];
                            mSingleCameraIO[i].Trigger_Bitnum = (int)dataRows[0]["BitNum"];
                        }
                        break;
                    default:
                        break;
                }

                // 光源信号
                dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = '" +
                    FormMain.jobHelper[i].LightSourceName + "'");
                if (dataRows.Length >= 1)
                {
                    mSingleCameraIO[i].LightSource_Portnum = (int)dataRows[0]["PortNum"];
                    mSingleCameraIO[i].LightSource_Bitnum = (int)dataRows[0]["BitNum"];
                }

                // OK信号
                dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = 'OK" + (i + 1) + "'");
                if (dataRows.Length >= 1)
                {
                    mSingleCameraIO[i].OK_Portnum = (int)dataRows[0]["PortNum"];
                    mSingleCameraIO[i].OK_Bitnum = (int)dataRows[0]["BitNum"];
                }

                // NG信号
                dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = 'NG" + (i + 1) + "'");
                if (dataRows.Length >= 1)
                {
                    mSingleCameraIO[i].NG_Portnum = (int)dataRows[0]["PortNum"];
                    mSingleCameraIO[i].NG_Bitnum = (int)dataRows[0]["BitNum"];
                }
            }

            // PLC online 信号
            dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'PLCOnline'");
            if (dataRows.Length >= 1)
            {
                PlcOnline_PortNum = (int)dataRows[0]["PortNum"];
                PlcOnline_BitNum = (int)dataRows[0]["BitNum"];
            }

            // 换型信号
            dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverEnable'");
            if (dataRows.Length >= 1)
            {
                ChangeoverEnable_PortNum = (int)dataRows[0]["PortNum"];
                ChangeoverEnable_Bitnum = (int)dataRows[0]["BitNum"];
            }

            // 换型二进制位信号
            dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverBit1'");
            if (dataRows.Length >= 1)
            {
                ChangeoverBit1_Portnum = (int)dataRows[0]["PortNum"];
                ChangeoverBit1_Bitnum = (int)dataRows[0]["BitNum"];
            }

            // 换型二进制位信号
            dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverBit2'");
            if (dataRows.Length >= 1)
            {
                ChangeoverBit2_Portnum = (int)dataRows[0]["PortNum"];
                ChangeoverBit2_Bitnum = (int)dataRows[0]["BitNum"];
            }

            // 换型二进制位信号
            dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverBit3'");
            if (dataRows.Length >= 1)
            {
                ChangeoverBit3_Portnum = (int)dataRows[0]["PortNum"];
                ChangeoverBit3_Bitnum = (int)dataRows[0]["BitNum"];
            }

            // 换型二进制位信号
            dataRows = DataTableIOInfo.Select("Type = " + 1 + " and Name = 'ChangeoverBit4'");
            if (dataRows.Length >= 1)
            {
                ChangeoverBit4_Portnum = (int)dataRows[0]["PortNum"];
                ChangeoverBit4_Bitnum = (int)dataRows[0]["BitNum"];
            }

            // CCD online 信号
            dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = 'CCDOnline'");
            if (dataRows.Length >= 1)
            {
                CCDOnline_PortNum = (int)dataRows[0]["PortNum"];
                CCDOnline_BitNum = (int)dataRows[0]["BitNum"];
            }
        }

        /// <summary>
        /// 初始化IO板卡设备
        /// </summary>
        /// <returns></returns>
        public bool InitializeIODevice()
        {
            try
            {
                instantDiCtrl1 = new InstantDiCtrl();
                instantDoCtrl1 = new InstantDoCtrl();
                instantDiCtrl1.SelectedDevice = new DeviceInformation(DeviceNum);
                instantDoCtrl1.SelectedDevice = new DeviceInformation(DeviceNum);
            }
            catch (Exception)
            {

            }

            //The default device of project is demo device, users can choose other devices according to their needs. 
            if (!instantDiCtrl1.Initialized)
            {
                MessageBox.Show("No device be selected or device open failed!", "StaticDI");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 关闭IO设备，释放资源
        /// </summary>
        public void CloseIO()
        {
            try
            {
                instantDoCtrl1.Write(0, 0);
                instantDoCtrl1.Write(1, 0);
                instantDoCtrl1.Dispose();
                instantDiCtrl1.Dispose();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 将IO端口信息写入配置文件
        /// </summary>
        public void WriteIOInfoToIniFile()
        {
            try
            {
                for (int i = 0; i < PORTCNT; i++)
                {
                    for (int j = 0; j < BITCNT; j++)
                    {
                        DataRow[] dataRows;
                        dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 1 + " and PortNum = " + i + " and BitNum = " + j);
                        if (dataRows.Length >= 1)
                            IOSettingIniFile.IniWriteValue("IOInfoTable", "(1" + "," + i + "," + j + ")",
                                dataRows[0]["Name"].ToString());
                        dataRows = FormMain.iOHelper.DataTableIOInfo.Select(
                            "Type = " + 2 + " and PortNum = " + i + " and BitNum = " + j);
                        if (dataRows.Length >= 1)
                            IOSettingIniFile.IniWriteValue("IOInfoTable", "(2" + "," + i + "," + j + ")", 
                                dataRows[0]["Name"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Write config file fails", "Fails:" + ex);
            }
        }

        #region PCI board communicate with PLC and light source controller

        /// <summary>
        /// 检查Trigger信号
        /// </summary>
        /// <returns></returns>
        public bool[] CaptureTriggerSignal()
        {
            bool[] triggerData = new bool[CamNum];
            for (int i = 0; i < CamNum; i++)
            {
                byte data;
                triggerData[i] = false;
                instantDiCtrl1.ReadBit(mSingleCameraIO[i].Trigger_Portnum, mSingleCameraIO[i].Trigger_Bitnum, out data);
                switch (TrigEdge)
                {
                    case TriggerEdge.Rising:
                        if (!IsTriggerSignalHigh[i] && data == 1)
                            triggerData[i] = true;
                        break;
                    case TriggerEdge.Falling:
                        if (IsTriggerSignalHigh[i] && data == 0)
                            triggerData[i] = true;
                        break;
                    default:
                        break;
                }
                // 将当前的IO数据保存起来
                IsTriggerSignalHigh[i] = data == 1 ? true : false;
            }
            return triggerData;
        }

        /// <summary>
        /// 检查changeover enable信号
        /// </summary>
        /// <returns></returns>
        public bool CaptureChangeoverSignal()
        {
            byte data;
            bool changeoverData = false;
            instantDiCtrl1.ReadBit(ChangeoverEnable_PortNum, ChangeoverEnable_Bitnum, out data);
            switch (TrigEdge)
            {
                case TriggerEdge.Rising:
                    if (!IsChangeoverEnableSignalHigh && data == 1)
                        changeoverData = true;
                    break;
                case TriggerEdge.Falling:
                    if (IsChangeoverEnableSignalHigh && data == 0)
                        changeoverData = true;
                    break;
                default:
                    break;
            }
            // 将当前的换型使能数据保存起来
            IsChangeoverEnableSignalHigh = data == 1 ? true : false;
            return changeoverData;
        }

        /// <summary>
        /// 解码PN数据对应的数字
        /// </summary>
        /// <returns></returns>
        public int CapturePartNoSignal()
        {
            int partNoIndex = 0;
            byte num1 = 0;
            byte num2 = 0;
            byte num3 = 0;
            byte num4 = 0;
            byte num5 = 0;
            byte product_num = 0x00;
            instantDiCtrl1.ReadBit(ChangeoverBit1_Portnum, ChangeoverBit1_Bitnum, out num1);
            instantDiCtrl1.ReadBit(ChangeoverBit2_Portnum, ChangeoverBit2_Bitnum, out num2);
            instantDiCtrl1.ReadBit(ChangeoverBit3_Portnum, ChangeoverBit3_Bitnum, out num3);
            instantDiCtrl1.ReadBit(ChangeoverBit4_Portnum, ChangeoverBit4_Bitnum, out num4);
            instantDiCtrl1.ReadBit(ChangeoverBit5_Portnum, ChangeoverBit5_Bitnum, out num5);
            switch (ValidInputType)
            {
                case ValidInput.High:
                    if (num1 == 1) product_num |= 0x01;
                    if (num2 == 1) product_num |= 0x01 << 1;
                    if (num3 == 1) product_num |= 0x01 << 2;
                    if (num4 == 1) product_num |= 0x01 << 3;
                    if (num5 == 1) product_num |= 0x01 << 4;
                    //      PLC 定义的 product no.    ---------------     CCD 软件定义的 Recipe no.
                    //          1                    ---------------         1
                    //          2                    ---------------         2
                    //      (1 based index)            and so on...      (zero based index)
                    partNoIndex = product_num;
                    break;
                case ValidInput.Low:
                    if (num1 == 0) product_num |= 0x01;
                    if (num2 == 0) product_num |= 0x01 << 1;
                    if (num3 == 0) product_num |= 0x01 << 2;
                    if (num4 == 0) product_num |= 0x01 << 3;
                    if (num5 == 0) product_num |= 0x01 << 4;
                    //      PLC 定义的 product no.    ---------------     CCD 软件定义的 Recipe no.
                    //          1                    ---------------         1
                    //          2                    ---------------         2
                    //      (1 based index)            and so on...      (zero based index)
                    partNoIndex = product_num;
                    break;
                default:
                    break;
            }

            return partNoIndex;
        }

        /// <summary>
        /// 捕获PLC 在线信息，PLC在线时，PLC online端口不停闪烁，时间间隔为200ms
        /// 如果PLC online端口超过3分钟不改变状态，表示PLC没有工作，工控机自动关机
        /// </summary>
        /// <returns></returns>
        public bool CapturePLCOnlineSignal()
        {
            byte data;
            bool isOnline = false;
            instantDiCtrl1.ReadBit(PlcOnline_PortNum, PlcOnline_BitNum, out data);
            // 初始化PLC在线信号状态
            switch (ValidInputType)
            {
                case ValidInput.High:
                    isOnline = (data == 1 ? true : false);
                    break;
                case ValidInput.Low:
                    isOnline = (data == 0 ? true : false);
                    break;
                default:
                    break;
            }
            return isOnline;
        }

        /// <summary>
        /// turn on light source
        /// index为相机的序号
        /// </summary>
        /// <param name="index"></param>
        public void TurnOnLight(int index)
        {
            ErrorCode err = ErrorCode.Success;
            byte data = 0x01;
            try
            {
                switch (ValidOutputType)
                {
                    case ValidOutput.High:
                        data = 0x01;
                        break;
                    case ValidOutput.Low:
                        data = 0x00;
                        break;
                    default:
                        break;
                }
                err = instantDoCtrl1.WriteBit(mSingleCameraIO[index].LightSource_Portnum, 
                    mSingleCameraIO[index].LightSource_Bitnum, data);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }

        /// <summary>
        /// turn off light source,
        /// index为相机的序号
        /// </summary>
        /// <param name="index"></param>
        public void TurnOffLight(int index)
        {
            ErrorCode err = ErrorCode.Success;
            byte data = 0x00;
            try
            {
                switch (ValidOutputType)
                {
                    case ValidOutput.High:
                        data = 0x00;
                        break;
                    case ValidOutput.Low:
                        data = 0x01;
                        break;
                    default:
                        break;
                }
                err = instantDoCtrl1.WriteBit(mSingleCameraIO[index].LightSource_Portnum,
                    mSingleCameraIO[index].LightSource_Bitnum, data);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }

        /// <summary>
        /// turn on light source
        /// index为相机的序号
        /// </summary>
        /// <param name="index"></param>
        public void TurnOnLightByName(string name)
        {
            ErrorCode err = ErrorCode.Success;
            byte data = 0x01;
            DataRow[] dataRows;
            int port, bit;

            try
            {
                // 通过光源信号名称查找IO Table中对应的Port和Bit号
                dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = '" + name + "'");
                if (dataRows.Length >= 1)
                {
                    port = (int)dataRows[0]["PortNum"];
                    bit = (int)dataRows[0]["BitNum"];
                    switch (ValidOutputType)
                    {
                        case ValidOutput.High:
                            data = 0x01;
                            break;
                        case ValidOutput.Low:
                            data = 0x00;
                            break;
                        default:
                            break;
                    }
                    err = instantDoCtrl1.WriteBit(port, bit, data);
                    if (err != ErrorCode.Success)
                    {
                        HandleError(err);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }

        /// <summary>
        /// turn off light source,
        /// index为相机的序号
        /// </summary>
        /// <param name="index"></param>
        public void TurnOffLightByName(string name)
        {
            ErrorCode err = ErrorCode.Success;
            byte data = 0x00;
            DataRow[] dataRows;
            int port, bit;

            try
            {
                // 通过光源信号名称查找IO Table中对应的Port和Bit号
                dataRows = DataTableIOInfo.Select("Type = " + 2 + " and Name = '" + name + "'");
                if (dataRows.Length >= 1)
                {
                    port = (int)dataRows[0]["PortNum"];
                    bit = (int)dataRows[0]["BitNum"];
                    switch (ValidOutputType)
                    {
                        case ValidOutput.High:
                            data = 0x00;
                            break;
                        case ValidOutput.Low:
                            data = 0x01;
                            break;
                        default:
                            break;
                    }
                    err = instantDoCtrl1.WriteBit(port, bit, data);
                    if (err != ErrorCode.Success)
                    {
                        HandleError(err);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }

        /*
        /// <summary>
        /// Send CCD available signal (wait for trigger signal)
        /// </summary>
        public void SendCCDReadySingal(int index, bool isReady)
        {
            ErrorCode err = ErrorCode.Success;
            try
            {
                err = instantDoCtrl1.WriteBit(mSingleCameraIO[index].Ready_Portnum,
                    mSingleCameraIO[index].Ready_Bitnum, isReady ? (byte)1 : (byte)0);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }*/

        /*
        public void SetInspectCompletedSignalHigh(int index)
        {
            ErrorCode err = ErrorCode.Success;
            try
            {
                err = instantDoCtrl1.WriteBit(mSingleCameraIO[index].InspectCompleted_PortNum,
                    mSingleCameraIO[index].InspectCompleted_BitNum, 1);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }*/

        /*
        public void SetInspectCompletedSignalLow(int index)
        {
            ErrorCode err = ErrorCode.Success;
            try
            {
                err = instantDoCtrl1.WriteBit(mSingleCameraIO[index].InspectCompleted_PortNum,
                    mSingleCameraIO[index].InspectCompleted_BitNum, 0);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }*/

        /// <summary>
        /// send ok signal high to PLC
        /// </summary>
        public void SetOKSignalHigh(int index)
        {
            ErrorCode err = ErrorCode.Success;
            byte data = 0x01;
            try
            {
                switch (ValidOutputType)
                {
                    case ValidOutput.High:
                        data = 0x01;
                        break;
                    case ValidOutput.Low:
                        data = 0x00;
                        break;
                    default:
                        break;
                }
                err = instantDoCtrl1.WriteBit(mSingleCameraIO[index].OK_Portnum,
                    mSingleCameraIO[index].OK_Bitnum, data);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }

        /// <summary>
        /// send ok signal low to PLC
        /// </summary>
        public void SetOKSignalLow(int index)
        {
            ErrorCode err = ErrorCode.Success;
            byte data = 0x00;
            try
            {
                switch (ValidOutputType)
                {
                    case ValidOutput.High:
                        data = 0x00;
                        break;
                    case ValidOutput.Low:
                        data = 0x01;
                        break;
                    default:
                        break;
                }
                err = instantDoCtrl1.WriteBit(mSingleCameraIO[index].OK_Portnum,
                    mSingleCameraIO[index].OK_Bitnum, data);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }

        /// <summary>
        /// send NG signal high to PLC
        /// </summary>
        public void SetNGSignalHigh(int index)
        {
            ErrorCode err = ErrorCode.Success;
            byte data = 0x01;
            try
            {
                switch (ValidOutputType)
                {
                    case ValidOutput.High:
                        data = 0x01;
                        break;
                    case ValidOutput.Low:
                        data = 0x00;
                        break;
                    default:
                        break;
                }
                err = instantDoCtrl1.WriteBit(mSingleCameraIO[index].NG_Portnum, 
                    mSingleCameraIO[index].NG_Bitnum, data);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }

        /// <summary>
        /// send NG signal high to PLC
        /// </summary>
        public void SetNGSignalLow(int index)
        {
            ErrorCode err = ErrorCode.Success;
            byte data = 0x00;
            try
            {
                switch (ValidOutputType)
                {
                    case ValidOutput.High:
                        data = 0x00;
                        break;
                    case ValidOutput.Low:
                        data = 0x01;
                        break;
                    default:
                        break;
                }
                err = instantDoCtrl1.WriteBit(mSingleCameraIO[index].NG_Portnum,
                    mSingleCameraIO[index].NG_Bitnum, data);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }

        /*
        public void SendFailureCodeSignal(byte failureCode)
        {
            byte bit1, bit2, bit3, bit4;
            bit1 = failureCode &= 0x01;
            bit2 = (byte)((failureCode &= 0x02) >> 1);
            bit3 = (byte)((failureCode &= 0x04) >> 2);
            bit4 = (byte)((failureCode &= 0x08) >> 3);
            ErrorCode err = ErrorCode.Success;
            try
            {
                err = instantDoCtrl1.WriteBit(FailureModeBit1_PortNum,
                    FailureModeBit1_Bitnum, bit1);
                err = instantDoCtrl1.WriteBit(FailureModeBit2_PortNum,
                    FailureModeBit2_Bitnum, bit2);
                err = instantDoCtrl1.WriteBit(FailureModeBit3_PortNum,
                    FailureModeBit3_Bitnum, bit3);
                err = instantDoCtrl1.WriteBit(FailureModeBit4_PortNum,
                    FailureModeBit4_Bitnum, bit4);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }*/

        /// <summary>
        /// send to PLC: vision system is ready !
        /// </summary>
        public void SendCCDOnlineSignal(bool isOnline)
        {
            ErrorCode err = ErrorCode.Success;
            byte data = 0x01;
            try
            {
                switch (ValidOutputType)
                {
                    case ValidOutput.High:
                        data = isOnline ? (byte)1 : (byte)0;
                        break;
                    case ValidOutput.Low:
                        data = isOnline ? (byte)0 : (byte)1;
                        break;
                    default:
                        break;
                }
                err = instantDoCtrl1.WriteBit(CCDOnline_PortNum, CCDOnline_BitNum, data);
                if (err != ErrorCode.Success)
                {
                    HandleError(err);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + err.ToString());
            }
        }
        #endregion

        /// <summary>
        /// message box to show the error if there is one
        /// </summary>
        /// <param name="err"></param>
        private void HandleError(ErrorCode err)
        {
            if (err != ErrorCode.Success)
            {
                MessageBox.Show("Sorry ! Some errors happened, the error code is: " + err.ToString(), "Static DI");
            }
        }
    }
}