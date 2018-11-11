using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision_System
{
    public enum LanguageType
    {
        Chinese,
        English
    }

    /// <summary>
    /// 触发是上升沿还是下降沿
    /// </summary>
    public enum ValidInput
    {
        High,
        Low
    }

    /// <summary>
    /// 触发是上升沿还是下降沿
    /// </summary>
    public enum ValidOutput
    {
        High,
        Low
    }

    /// <summary>
    /// 触发是上升沿还是下降沿
    /// </summary>
    public enum TriggerEdge
    {
        Rising,
        Falling
    }

    // View的类型：DataGrid适合大量数据的显示，如多个Pin的位置度、高度等信息
    // Chart适合数据量比较少时用来显示数据的动态变化趋势
    public enum ViewType
    {
        DataGrid,
        Chart
    }

    // View图表的排版结构
    // Horizontal, //图表水平布局
    // Vertical,   //图表垂直布局，默认情况下是垂直布局
    public enum ViewLayout
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// 图片保存的大小，是否需要压缩节省空间
    /// </summary>
    public enum SaveImageSize
    {
        Full,
        Half,
        Quater
    }

    /// <summary>
    /// 图片保存格式
    /// </summary>
    public enum SaveImageFormat
    {
        BMP,
        JPG
    }

    /// <summary>
    /// 数据保存格式
    /// </summary>
    public enum SaveDataFormat
    {
        Csv,
        DataBase
    }

    public class SettingHelper
    {
        private string _softwareName; // 软件名称，显示在软件标题中
        private LanguageType _language = LanguageType.Chinese; // 软件语言
        private int _maximumProductNoNum = 16;
        private List<string> _partNoList = new List<string>(); // 成品料号列表
        private bool _isSaveData = false;
        private SaveImageSize _imageSize = SaveImageSize.Full;
        private SaveImageFormat _imageFormat = SaveImageFormat.BMP;
        private SaveDataFormat _dataFormat = SaveDataFormat.Csv;
        private bool _isSaveOKImage = false;
        private bool _isSaveNGImage = false;
        private int _dataKeepDays = 180;
        private int _imageKeepDays = 180;
        private string _strSaveDataDirectoryPath = "";
        private string _strSaveImageDirectoryPath = "";
        private ViewType _viewType = ViewType.DataGrid;
        private ViewLayout _viewLayout = ViewLayout.Vertical;
        private string _AdsAmsNetID = "5.54.159.254.1.1";
        private int _AdsPortNumber = 851;
        private bool _isShowDataView = false;
        private bool _isIODeviceEnabled = false;
        private int _IODeviceNumber = 0;
        private TriggerEdge _trigEdge = TriggerEdge.Rising;
        private ValidInput _validInputType = ValidInput.High;
        private ValidOutput _validOutputType = ValidOutput.High;
        private string _PLCIP;
        private short _PLCPort;
        private short _DMStartAddress;
        private short _DMDataLength;
        private List<int> _FinsDataSelectedIndex = new List<int>();

        // 定义DataTable存储IO信号的所有配置信息
        // DataTable定义格式如下：
        // 序号           料号名称       CCD1料号         ...         CCD N 料号
        // Index         Part No       CCD1 Part No.    ...         CCD N part No.
        // <int>         <string>      <string>        ...          <string>
        //  0+             ""          ""             ...           ""
        private DataTable _dataTablePartNoInfo = new DataTable("PartNoInfo");

        public bool IsSaveData { get => _isSaveData; set => _isSaveData = value; }
        public bool IsSaveOKImage { get => _isSaveOKImage; set => _isSaveOKImage = value; }
        public bool IsSaveNGImage { get => _isSaveNGImage; set => _isSaveNGImage = value; }
        public int DataKeepDays { get => _dataKeepDays; set => _dataKeepDays = value; }
        public int ImageKeepDays { get => _imageKeepDays; set => _imageKeepDays = value; }
        public string StrSaveDataDirectoryPath { get => _strSaveDataDirectoryPath; set => _strSaveDataDirectoryPath = value; }
        public string StrSaveImageDirectoryPath { get => _strSaveImageDirectoryPath; set => _strSaveImageDirectoryPath = value; }
        public ViewType ViewType { get => _viewType; set => _viewType = value; }
        public bool IsShowDataView { get => _isShowDataView; set => _isShowDataView = value; }
        public ViewLayout ViewLayout { get => _viewLayout; set => _viewLayout = value; }
        public bool IsIODeviceEnabled { get => _isIODeviceEnabled; set => _isIODeviceEnabled = value; }
        public int IODeviceNumber { get => _IODeviceNumber; set => _IODeviceNumber = value; }
        public TriggerEdge TrigEdge { get => _trigEdge; set => _trigEdge = value; }
        public ValidInput ValidInputType { get => _validInputType; set => _validInputType = value; }
        public ValidOutput ValidOutputType { get => _validOutputType; set => _validOutputType = value; }
        public string AdsAmsNetID { get => _AdsAmsNetID; set => _AdsAmsNetID = value; }
        public int AdsPortNumber { get => _AdsPortNumber; set => _AdsPortNumber = value; }
        public string PLCIP { get => _PLCIP; set => _PLCIP = value; }
        public short PLCPort { get => _PLCPort; set => _PLCPort = value; }
        public SaveImageSize ImageSize { get => _imageSize; set => _imageSize = value; }
        public SaveImageFormat ImageFormat { get => _imageFormat; set => _imageFormat = value; }
        public SaveDataFormat DataFormat { get => _dataFormat; set => _dataFormat = value; }
        public List<string> PartNoList { get => _partNoList; set => _partNoList = value; }
        public int MaximumProductNoNum { get => _maximumProductNoNum; set => _maximumProductNoNum = value; }
        public short DMStartAddress { get => _DMStartAddress; set => _DMStartAddress = value; }
        public short DMDataLength { get => _DMDataLength; set => _DMDataLength = value; }
        public List<int> FinsDataSelectedIndex { get => _FinsDataSelectedIndex; set => _FinsDataSelectedIndex = value; }
        public DataTable DataTablePartNoInfo { get => _dataTablePartNoInfo; set => _dataTablePartNoInfo = value; }
        public string SoftwareName { get => _softwareName; set => _softwareName = value; }
        public LanguageType SoftwareLanguage { get => _language; set => _language = value; }

        /// <summary>
        /// 根据配置文件初始化Part No的Table
        /// </summary>
        /// <param name="maxPNNum"></param>
        /// <param name="camNum"></param>
        /// <param name="strIniFile"></param>
        public void InitializePartNoTable(int maxPNNum, int camNum, string strIniFile)
        {
            string strTemp;
            string[] strArr, strArr1;
            DataRow row;
            IniFile SettingIniFile = new IniFile(strIniFile);
            try
            {
                // Part No Datatable初始化
                // 注意Column name不能有空格，否则在查询语句中会报错
                DataTablePartNoInfo.Columns.Add("PNIndex", typeof(Int32));
                DataTablePartNoInfo.Columns.Add("PNName", typeof(string));
                for (int i = 0; i < camNum; i++)
                {
                    DataTablePartNoInfo.Columns.Add("PNCCD" + (i + 1), typeof(string));
                }


                // 从配置文件中获取
                for (int i = 0; i < maxPNNum; i++)
                {
                    strTemp = SettingIniFile.IniReadValue("PartNo", (i + 1).ToString());
                    // 如果料号不为空或者null,添加料号到料号列表中
                    if (!string.IsNullOrEmpty(strTemp))
                    {
                        strArr = strTemp.Split(':');
                        PartNoList.Add(strArr[0]);
                        row = DataTablePartNoInfo.NewRow();
                        row[0] = i + 1;
                        row[1] = strArr[0];
                        strArr1 = strArr[1].Split(',');
                        for (int j = 0; j < camNum; j++)
                        {
                            row[j + 2] = strArr1[j];
                        }
                        DataTablePartNoInfo.Rows.Add(row);
                    }  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Read config file fails", "Fails:" + ex);
            }
        }
    }
}
