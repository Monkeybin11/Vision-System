using System;
using System.Windows.Forms;
using Cognex.VisionPro;
using System.Threading;
using System.IO;
using Cognex.VisionPro.ToolBlock;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using Cognex.VisionPro.ImageFile;
using System.Runtime.InteropServices;
using TwinCAT.Ads;
using System.Linq;
using System.Data;
using ReadWriteCsv;

namespace Vision_System
{
    #region Enum class
    // different access level to access different button or menu,
    // password is required for Supervisor and Administrator level
    public enum AccessLevel { Operator, Supervisor, Administrator }
    // Run state to indicate the status of job running
    public enum RunState { Stopped, RunningContinuous, RunningOnce, RunningLive }
    // capture mode to indicate how to capture the trigger or other signal from PLC,
    // default is scan mode
    public enum CommunicationType { TwinCat, Fins, RS232, None}
    public enum IOCaptureMode { Scan, Interrupt }
    public enum PageCollection { Home, CameraSetting, Edit, Setting, PlayBack, Statistics}
    #endregion

    public partial class FormMain : Form
    {
        #region Page
        private PageHome pageHome = new PageHome();
        private PageEdit pageEdit = new PageEdit();
        private PageCameraSetting pageCamerasetting = new PageCameraSetting();
        private PagePlayBack pagePlayback = new PagePlayBack();
        private PageStatistics pageStatistic = new PageStatistics();
        #endregion

        #region Private variable
        private const int INTERVALBETWEENOKNGANDCOMPLETESIGNAL = 10;
        private const int INTERVALBETWEENLIGHTONANDIMAGECAPTURE = 20;
        private const int INTERVALBETWEENIMAGECAPTUREANDLIGHTOFF = 20;
        private PageCollection page = PageCollection.Home; 
        private CommunicationType communicationType;
        private TwincatHelper twinCat = new TwincatHelper();
        private OmronFinsHelper omronFins = new OmronFinsHelper();
        private CogAcqFifoTool[] ccdAcqFifoTool;
        private CogToolBlock[] CCDToolBlock;
        private ICogImage[] originalImage;
        private bool[] imageAcqCompleted;
        private bool[] jobInspectCompleted;
        private DateTime[] dateTime;
        private double[] jobCTTime;
        private bool[] acqLoadSuccess;
        private bool[] jobLoadSuccess;
        private bool[] openCameraSuccess;
        private bool isPLCOnline = true;
        private bool isCCDReady = true; // 表示CCD可以工作
        private bool isCCDOnline = false; // 表示CCD在线，可以接收触发信号，处于工作状态
        private bool isTwinCatConnected = false;
        private bool isOmronFinsConnected = false;
        private CsvRow[] csvList;      // csv data to be restored in the computer
        private DateTime dtPLCOnline;
        private BackgroundWorker[] bgwCCDTrigger;
        private BackgroundWorker[] bgwWritePLC;
        private BackgroundWorker[] bgwSaveImage;
        private BackgroundWorker[] bgwSaveData;
        private BackgroundWorker bgwTwinCatReConnect;
        private BackgroundWorker bgwFinsTotalResultSender;
        #endregion

        #region Public static variable
        public static int camNumber;
        public static JobHelper[] jobHelper;
        public static string strSelectedPartNoName = "";
        public static int selectedPartNoIndex = 0;
        public static string strTemplateAcqFifoPath;
        public static string strTemplateJobPath;
        public static string strRecipeFolderPath;
        public static string[] strLoadedAcqFifoSettingVppFilePath;
        public static string[] strLoadedVppFilePath;
        public static string strRecipeBackupFolderBase;     //vpp文件备份上层文件夹
        public static string strCCDProgramBackupFolderPath; //vpp文件备份实际文件夹
        public static string strBaseDirectory;
        public static string strConfigFilePath;
        public static string[] strCCDConfigFilePath;
        public static string strIOSettingFilePath;
        public static string strProductSettingPath;
        public static string strSaveImageFolderPath;
        public static string strSaveDataFolderPath;
        public static string strLogFilePath;
        public static AccessLevel currentAccessLevel = AccessLevel.Operator;
        public static IniFile mainConfigIniFile;
        public static IniFile[] ccdConfigIniFile;
        public static SettingHelper settingHelper = new SettingHelper();
        public static IOHelper iOHelper = new IOHelper();

        // 定义DataTable存储预先定义可供选择的Omron通信数据
        // 为了更好支持中文，添加一列中文描述
        // DataTable定义格式如下：
        // 英文名称      中文描述           当前值           是否选择作为最终Fins输出       再Output中的序号
        // Name         Description       Value           IsSelected                    IndexInOutput
        // <string>     <string>          <short>         <bool>                        int
        // 第一个为可供选择的所有Fins数据
        public static DataTable dataTableFinsData = new DataTable("FinsDataTable");
        // 第二个为用户实际选择作为输出的数据
        public static DataTable dataTableFinsOutput = new DataTable("FinsOutputTable");
        // 第三个为单个相机对应的数据，用于防止多线程访问同一个DataTable时造成的数据冲突
        public static DataTable[] dataTableSingleCCDFins;
        // 以下用于保存触发源名字和对应的相机序号
        public static Dictionary<string, string> triggerNameAndCCDIndex = new Dictionary<string, string>();
        #endregion

        #region Form load, close, initialization
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// FormMain的构造函数，主界面显示前，先显示SplashForm界面，
        /// 先处理耗时程序：如加载vpp文件，初始化通讯，检查通讯状态等
        /// </summary>
        public FormMain()
        {
            // 创建主窗口，初始化控件
            Splasher.Status = "Creating MainForm, initialize components...";
            Splasher.Progress = 10;
            InitializeComponent();
            strBaseDirectory = Utility.GetThisExecutableDirectory();
            StatusLabel_StartupPath.Text = Application.ExecutablePath;
            CheckForIllegalCrossThreadCalls = false;

            // 加载配置文件
            Splasher.Status = "Loading configuration file...";
            Splasher.Progress = 20;
            Thread.Sleep(100);
            ReadSettingConfigFile();
            InitVariable();
            SetLanguage(settingHelper.SoftwareLanguage);
            ResolveFilePath();

            // 加载vpp文件到主程序
            Splasher.Status = "Loading Vpp Files...";
            Splasher.Progress = 25;
            LoadVppFile();
            // 读取单个CCD的设置参数
            for (int i = 0; i < camNumber; i++)
            {
                jobHelper[i] = new JobHelper();
                strCCDConfigFilePath[i] = strBaseDirectory + "Config\\CCD" + (i + 1) + ".ini";
                ccdConfigIniFile[i] = new IniFile(strCCDConfigFilePath[i]);
                jobHelper[i].ReadJobSetting(i, strCCDConfigFilePath[i], CCDToolBlock[i]);
            }
            // 读取IO端口设置参数
            ReadIOConfigFile();

            // 删除过期的图像和数据
            Splasher.Status = "Delete obsolete data and image...";
            Splasher.Progress = 50;
            Thread.Sleep(100);
            DeleteObsoleteDataImage();

            // 初始化主页面
            Splasher.Status = "Initialize home page ...";
            Splasher.Progress = 60;
            Thread.Sleep(100);
            // 初始化状态栏
            InitStatusLabel();

            // Debug 定时器，调试用
            Timer_Debug.Enabled = false;

            // 切换软件为自动运行状态
            isCCDOnline = true;

            // 根据登陆状态更新界面控件
            InitAccessLevel();

            // 将ToolBlock和AcqFifoTool变量传递到子页面类中
            pageHome.SetToolBlock(CCDToolBlock, ccdAcqFifoTool);
            pageCamerasetting.SetupAcqToolEditSubject(ccdAcqFifoTool);
            pageEdit.SetupToolBlockEditSubject(CCDToolBlock);
            pagePlayback.SetToolBlock(CCDToolBlock);

            // IO板卡连接状态
            Splasher.Status = "Initialize IO board ...";
            Splasher.Progress = 70;
            Thread.Sleep(100);
            if (settingHelper.IsIODeviceEnabled)
            {
                // 初始化IO板卡
                // 检查IO板卡是否已经打开
                iOHelper.IsIODeviceConnected = iOHelper.InitializeIODevice();
                isCCDReady &= iOHelper.IsIODeviceConnected;
            }

            // Fins TCP/ TwinCat Ads 或串口通讯连接状态
            // 初始化TCP或者串口通讯:TwinCat Ads/Fins/RS232
            Splasher.Status = "Initialize TCP network ...";
            Splasher.Progress = 80;
            switch (communicationType)
            {
                case CommunicationType.TwinCat:
                    // 初始化TwinCat ADS通讯
                    isTwinCatConnected = InitTwinCat();
                    isCCDReady &= isTwinCatConnected;
                    break;
                case CommunicationType.Fins:
                    // 初始化Fins连接
                    isOmronFinsConnected = omronFins.InitializeOmronFins();
                    isCCDReady &= isOmronFinsConnected;
                    for (int i = 0; i < camNumber; i++)
                        dataTableSingleCCDFins[i] = new DataTable("SingleFinsData" + i);
                    InitializeFinsDataTable();
                    break;
                case CommunicationType.RS232:
                    break;
                case CommunicationType.None:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 加载主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            // 初始界面为主页面
            pageHome.Dock = DockStyle.Fill;
            ResetToolStripBackColor();
            toolStripHome.BackColor = Color.LightGray;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(pageHome);
            // 初始化DataGridView中的初始数据
            pageHome.InitDatagirdViewResult();
            // 添加Log信息到Home窗口
            pageHome.LogMessage("主页面加载成功 ...");

            // IO板卡连接状态
            if (settingHelper.IsIODeviceEnabled)
            {
                // 添加Log信息到Home窗口
                if (iOHelper.IsIODeviceConnected) pageHome.LogPass("IO 板卡连接成功");
                else pageHome.LogFail("IO 板卡连接失败");
            }

            // Fins TCP/ TwinCat Ads 或串口通讯连接状态
            // 初始化TCP或者串口通讯:TwinCat Ads/Fins/RS232
            switch (communicationType)
            {
                case CommunicationType.TwinCat:
                    // 添加Log信息到Home窗口
                    if (isTwinCatConnected)
                    {
                        pageHome.LogPass("TwinCat ADS 连接成功");
                        // 如果连接成功，发送CCD Ready信号表示相机可以正常工作
                        for (int i = 0; i < camNumber; i++)
                            // 告诉PLC： CCD Ready，可以开始工作
                            twinCat.WritePLC(TwincatHelper.VARIABLECCDREADY[i], isCCDOnline);
                    }
                    else
                    {
                        pageHome.LogFail("TwinCat ADS 连接失败");
                    }
                    // 将TwinCat的初始连接状态设置为true，通过定时器不停检查连接状态，
                    // 当isTwinCatConnected由true变为false时，启动后台线程尝试重新连接
                    isTwinCatConnected = true;
                    Timer_TwinCatAds.Enabled = true;
                    break;
                case CommunicationType.Fins:
                    // 添加Log信息到窗口
                    if (isOmronFinsConnected) pageHome.LogPass("Omron Fins 连接成功");
                    else pageHome.LogFail("Omron Fins 连接失败");
                    // 如果Omron Fins连接正常，初始化Fins总结果发送后台线程
                    InitFinsTotalResultBackWorker();
                    bgwFinsTotalResultSender.RunWorkerAsync();
                    break;
                case CommunicationType.RS232:
                    break;
                case CommunicationType.None:
                    break;
                default:
                    break;
            }

            // 初始化相机触发后台线程
            InitTriggerBackWorker();

            // 初始化输出到PLC信号的后台线程
            InitWritePLCBackWorker();

            // 初始化图片保存后台线程
            InitImageSaveBackWorker();

            // 初始化数据保存后台线程
            InitDataSaveBackWorker();

            // 更新控件的可用状态
            UpdateControlsEnabled();

            // 更新状态栏
            UpdateStatusLabel();

            // 界面加载完成，可以工作
            // 添加Log信息到Home窗口
            pageHome.LogMessage("软件准备好开始工作 ...");

            // 初始化端口状态
            if (settingHelper.IsIODeviceEnabled)
            {
                iOHelper.ReadInitialPortStatus();
                iOHelper.WriteInitialPortStatus();
            }

            // 最后打开自动运行定时器，程序开始工作
            if (isCCDReady)
            {
                Timer_Auto.Enabled = true;
            }
            else
            {
                Timer_Auto.Enabled = false;
            }

            // 准备好开始工作
            Splasher.Status = "Ready to work...";
            Splasher.Progress = 100;
            Thread.Sleep(100);

            // 关闭Splasher启动页
            Splasher.Close();

            // 将主页面置于最前端显示
            IntPtr hWnd = this.Handle;
            SetForegroundWindow(hWnd);
        }

        private void SetLanguage(LanguageType language)
        {
            switch (language)
            {
                case LanguageType.Chinese:
                    this.toolStripHome.Text = "主页";
                    this.toolStripCameraSetting.Text = "取像";
                    this.toolStripEdit.Text = "配方";
                    this.toolStripSetting.Text = "系统";
                    this.toolStripPlayBack.Text = "回放";
                    this.toolStripStatistics.Text = "统计";
                    this.toolStripResetLayout.Text = "排列";
                    this.toolStripBackup.Text = "备份";
                    this.toolStripRestore.Text = "还原";
                    this.toolStripLogin.Text = "登录";
                    this.toolStripSave.Text = "保存";
                    this.toolStripPause.Text = "暂停";
                    break;
                case LanguageType.English:
                    this.toolStripHome.Text = "Home";
                    this.toolStripCameraSetting.Text = "Capture";
                    this.toolStripEdit.Text = "Recipe";
                    this.toolStripSetting.Text = "Setting";
                    this.toolStripPlayBack.Text = "Playback";
                    this.toolStripStatistics.Text = "Statis.";
                    this.toolStripResetLayout.Text = "Layout";
                    this.toolStripBackup.Text = "Backup";
                    this.toolStripRestore.Text = "Restore";
                    this.toolStripLogin.Text = "Login";
                    this.toolStripSave.Text = "Save";
                    this.toolStripPause.Text = "Pause";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 软件关闭之前执行的操作：文件保存，资源释放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 弹出“确认退出”消息框
            // 如果要取消退出，软件恢复
            if (MessageBox.Show("是否要退出检测程序?",
                "请确认！",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question)
                == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            // 如果确认退出程序，保存相应的设置到文件
            SaveSettingConfigFile();
            Timer_Auto.Enabled = false;
            try
            {
                // 关闭TCP资源
                switch (communicationType)
                {
                    case CommunicationType.TwinCat:
                        Timer_TwinCatAds.Enabled = false;
                        if(bgwTwinCatReConnect.IsBusy)
                            bgwTwinCatReConnect.CancelAsync();
                        bgwTwinCatReConnect.Dispose();
                        twinCat.CloseTwinCatConnection();
                        break;
                    case CommunicationType.Fins:
                        omronFins.CloseOmronFins();
                        break;
                    case CommunicationType.RS232:
                        break;
                    case CommunicationType.None:
                        break;
                    default:
                        break;
                }
                // 关闭IO资源并将端口状态复位
                if (settingHelper.IsIODeviceEnabled)
                    iOHelper.CloseIO();

                // 释放图像捕获资源
                for (int i = 0; i < camNumber; i++)
                    this.ccdAcqFifoTool[i].Dispose();
                CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
                foreach (ICogFrameGrabber fg in frameGrabbers)
                    fg.Disconnect(false);
            }
            catch (Exception)
            {
            }

            //防止以下错误提示框出现：
            //C# runtime error R6025 pure virtual function call
            Environment.Exit(0);
        }

        /// <summary>
        /// 初始化变量
        /// </summary>
        private void InitVariable()
        {
            strLoadedAcqFifoSettingVppFilePath = new string[camNumber];
            strLoadedVppFilePath = new string[camNumber];
            strCCDConfigFilePath = new string[camNumber];
            jobHelper = new JobHelper[camNumber];
            ccdAcqFifoTool = new CogAcqFifoTool[camNumber];
            CCDToolBlock = new CogToolBlock[camNumber];
            originalImage = new ICogImage[camNumber];
            imageAcqCompleted = new bool[camNumber];
            jobInspectCompleted = new bool[camNumber];
            dateTime = new DateTime[camNumber];
            jobCTTime = new double[camNumber];
            acqLoadSuccess = new bool[camNumber];
            jobLoadSuccess = new bool[camNumber];
            openCameraSuccess = new bool[camNumber];
            bgwCCDTrigger = new BackgroundWorker[camNumber];
            bgwWritePLC = new BackgroundWorker[camNumber];
            bgwSaveImage = new BackgroundWorker[camNumber];
            bgwSaveData = new BackgroundWorker[camNumber];
            csvList = new CsvRow[camNumber];
            dataTableSingleCCDFins = new DataTable[camNumber];
            ccdConfigIniFile = new IniFile[camNumber];
    }

    /// <summary>
    /// 软件启动时删除过期的图片和数据文件
    /// </summary>
    private void DeleteObsoleteDataImage()
        {
            // get current Date time to specify the folder name
            string strDate = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day;

            try
            {
                // Delete obsolete data in csv file
                DirectoryInfo folder = new DirectoryInfo(strSaveDataFolderPath);

                foreach (DirectoryInfo subfolder in folder.GetDirectories())
                {
                    if ((DateTime.Now - subfolder.CreationTime).TotalDays > settingHelper.DataKeepDays)
                        subfolder.Delete();
                }

                // Delete obsolete image in folder
                folder = new DirectoryInfo(strSaveImageFolderPath);

                foreach (DirectoryInfo subfolder in folder.GetDirectories())
                {
                    if ((DateTime.Now - subfolder.CreationTime).TotalDays > settingHelper.ImageKeepDays)
                        subfolder.Delete(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 工具栏点击触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            switch (btn.Name)
            {
                case "toolStripHome":
                    if (page != PageCollection.Home)
                    {
                        pageHome.Dock = DockStyle.Fill;
                        ResetToolStripBackColor();
                        toolStripHome.BackColor = Color.LightGray;
                        panelMain.Controls.Clear();
                        panelMain.Controls.Add(pageHome);
                        page = PageCollection.Home;
                    }
                    break;
                case "toolStripSetting":
                    FormSetting frmSettingSelect = new FormSetting(settingHelper.IsIODeviceEnabled,
                        communicationType == CommunicationType.TwinCat,
                        communicationType == CommunicationType.Fins,
                        communicationType == CommunicationType.Fins
                        );
                    frmSettingSelect.ShowDialog(this);
                    switch (frmSettingSelect.Result)
                    {
                        case FormSettingOptionResult.None:
                            break;
                        case FormSettingOptionResult.PartNoManage:
                            FormPartNoManage frmProduct = new FormPartNoManage();
                            frmProduct.ShowDialog(this);
                            frmProduct.Dispose();
                            // 更新HomePage的料号选择
                            pageHome.UpdateComboBoxPartNumber(settingHelper.PartNoList, strSelectedPartNoName);
                            break;
                        case FormSettingOptionResult.ImageDataSetting:
                            FormSaveSetting frmSave = new FormSaveSetting();
                            frmSave.ShowDialog(this);
                            frmSave.Dispose();
                            break;
                        case FormSettingOptionResult.IOSetting:
                            FormIOSetting frmIO = new FormIOSetting();
                            frmIO.ShowDialog(this);
                            frmIO.Dispose();
                            break;
                        case FormSettingOptionResult.IOMonitor:
                            FormIOMonitor frmIOMonitor = new FormIOMonitor();
                            frmIOMonitor.ShowDialog(this);
                            frmIOMonitor.Dispose();
                            break;
                        case FormSettingOptionResult.TwinCatSetting:
                            FormTwinCatSetting frmTwinCat = new FormTwinCatSetting();
                            frmTwinCat.ShowDialog(this);
                            frmTwinCat.Dispose();
                            break;
                        case FormSettingOptionResult.OmronFinsSetting:
                            FormOmronFinsSetting frmOmron = new FormOmronFinsSetting();
                            frmOmron.ShowDialog(this);
                            frmOmron.Dispose();
                            break;
                        case FormSettingOptionResult.CommFormatSetting:
                            FormCommFormatSetting frmCommFormat = new FormCommFormatSetting();
                            frmCommFormat.ShowDialog(this);
                            frmCommFormat.Dispose();
                            break;
                        case FormSettingOptionResult.FailureSetting:
                            FormFailureSetting frmFailure = new FormFailureSetting();
                            frmFailure.ShowDialog(this);
                            frmFailure.Dispose();
                            // 更新dataTableFinsData和dataTableFinsOutput
                            InitializeFinsDataTable();
                            break;
                        default:
                            break;
                    }
                    break;
                case "toolStripCameraSetting":
                    if (page != PageCollection.CameraSetting)
                    {
                        pageCamerasetting.Dock = DockStyle.Fill;
                        ResetToolStripBackColor();
                        toolStripCameraSetting.BackColor = Color.LightGray;
                        panelMain.Controls.Clear();
                        panelMain.Controls.Add(pageCamerasetting);
                        page = PageCollection.CameraSetting;
                    }
                    break;
                case "toolStripEdit":
                    if (page != PageCollection.Edit)
                    {
                        pageEdit.Dock = DockStyle.Fill;
                        ResetToolStripBackColor();
                        toolStripEdit.BackColor = Color.LightGray;
                        panelMain.Controls.Clear();
                        panelMain.Controls.Add(pageEdit);
                        page = PageCollection.Edit;
                    }
                    break;
                case "toolStripPlayBack":
                    if (page != PageCollection.PlayBack)
                    {
                        pagePlayback.Dock = DockStyle.Fill;
                        ResetToolStripBackColor();
                        toolStripPlayBack.BackColor = Color.LightGray;
                        panelMain.Controls.Clear();
                        panelMain.Controls.Add(pagePlayback);
                        page = PageCollection.PlayBack;
                    }
                    break;
                case "toolStripStatistics":
                    if (page != PageCollection.Statistics)
                    {
                        pageStatistic.Dock = DockStyle.Fill;
                        ResetToolStripBackColor();
                        toolStripStatistics.BackColor = Color.LightGray;
                        panelMain.Controls.Clear();
                        panelMain.Controls.Add(pageStatistic);
                        page = PageCollection.Statistics;
                    }
                    break;
                case "toolStripLogin":
                    FormLogin form = new FormLogin(currentAccessLevel);
                    form.ShowDialog();
                    currentAccessLevel = form.GetCurrentAccessLevel;
                    UpdateControlsEnabled();
                    UpdateStatusLabel();
                    // 根据登陆状态更新界面控件
                    UpdateAccessLevel();
                    break;
                case "toolStripSave":
                    SaveVppFileToLocalFolder();
                    // 保存配置文件到本地文件夹
                    SaveSettingConfigFile();
                    break;
                case "toolStripPause":
                    if (isCCDOnline)
                    {
                        isCCDOnline = false;
                        UpdateControlsEnabled();
                        UpdateStatusLabel();
                        UpdateAccessLevel();
                        Timer_Auto.Enabled = false;
                    }
                    else
                    {
                        isCCDOnline = true;
                        UpdateControlsEnabled();
                        UpdateStatusLabel();
                        UpdateAccessLevel();
                        // 初始化端口状态
                        if (settingHelper.IsIODeviceEnabled)
                        {
                            iOHelper.ReadInitialPortStatus();
                        }
                        // 程序开始自动运行，接受触发信号
                        if (isCCDReady) Timer_Auto.Enabled = true;
                        // 自动运行时，自动切换到主界面
                        if (page != PageCollection.Home)
                        {
                            pageHome.Dock = DockStyle.Fill;
                            ResetToolStripBackColor();
                            toolStripHome.BackColor = Color.White;
                            panelMain.Controls.Clear();
                            panelMain.Controls.Add(pageHome);
                            page = PageCollection.Home;
                        }
                        pageHome.StopAllCCDLiveMode();
                    }
                    if (settingHelper.IsIODeviceEnabled)
                    {
                        iOHelper.SendCCDOnlineSignal(isCCDOnline);
                    }
                    break;
                case "toolStripBackup":
                    // 程序备份
                    FormProgramBackup formBackup = new FormProgramBackup(strRecipeBackupFolderBase);
                    formBackup.ShowDialog(this);
                    if (formBackup.IsConfirmed)
                    {
                        strCCDProgramBackupFolderPath = formBackup.FolderPath;
                        BackupProgram();
                    }
                    formBackup.Dispose();
                    break;
                case "toolStripRestore":
                    // 程序恢复
                    FormProgramRestore formRestore = new FormProgramRestore(strRecipeBackupFolderBase);
                    formRestore.ShowDialog(this);
                    if (formRestore.IsConfirmed)
                    {
                        strCCDProgramBackupFolderPath = formRestore.FolderPath;
                        RestoreProgram();
                    }
                    formRestore.Dispose();
                    break;
                case "toolStripResetLayout":
                    pageHome.ResetLayout();
                    break;
                case "btnTest":
                    // 保存数据
                    if (!bgwSaveData[0].IsBusy)
                        bgwSaveData[0].RunWorkerAsync(0);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 更新控件
        /// </summary>
        public void UpdateControlsEnabled()
        {
            if (isCCDOnline)
            {
                pageHome.SetDisplayToolBarVisible(false);
                toolStripPause.Image = Properties.Resources.pause;
                switch (settingHelper.SoftwareLanguage)
                {
                    case LanguageType.Chinese:
                        toolStripPause.Text = "暂停";
                        break;
                    case LanguageType.English:
                        toolStripPause.Text = "Pause";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                pageHome.SetDisplayToolBarVisible(true);
                toolStripPause.Image = Properties.Resources.start;
                switch (settingHelper.SoftwareLanguage)
                {
                    case LanguageType.Chinese:
                        toolStripPause.Text = "继续";
                        break;
                    case LanguageType.English:
                        toolStripPause.Text = "Continue";
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 还原ToolStrip的背景颜色
        /// </summary>
        public void ResetToolStripBackColor()
        {
            this.toolStripHome.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSetting.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripCameraSetting.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripEdit.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripPlayBack.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatistics.BackColor = System.Drawing.SystemColors.Control;
        }

        /// <summary>
        /// 重新定义各种文件的路径
        /// </summary>
        public void ResolveFilePath()
        {
            // 程序文件夹
            strRecipeFolderPath = strBaseDirectory + "Recipe" +
                "\\" + strSelectedPartNoName;

            // create the folder if the directory doesn't exist
            CheckFolderExist(strRecipeFolderPath);

            // 模板Vpp程序文件夹
            strTemplateAcqFifoPath = strBaseDirectory + "Template" +
                "\\AcqFifo_CCD.vpp";
            strTemplateJobPath = strBaseDirectory + "Template" +
                "\\Job_CCD.vpp";

            // 所有相机的Vpp程序文件路径
            DataRow[] dataRows;
            string strName;
            dataRows = settingHelper.DataTablePartNoInfo.Select("PNIndex = " + selectedPartNoIndex);
            if (dataRows.Length >= 1)
            {
                for (int i = 0; i < camNumber; i++)
                {

                    strName = dataRows[0]["PNCCD" + (i + 1)].ToString();
                    // 如果为NA，该工位配方不共享，从成品料号文件夹中加载程序
                    if (strName == "NA")
                    {
                        // 加载相机设定vpp文件
                        strLoadedAcqFifoSettingVppFilePath[i] = strRecipeFolderPath + "\\" +
                                            "AcqFifo_CCD" + (i + 1) + ".vpp";
                        // 加载图像处理vpp文件
                        strLoadedVppFilePath[i] = strRecipeFolderPath + "\\" +
                                            "Job_CCD" + (i + 1) + ".vpp";
                    }
                    else
                    {
                        // 加载相机设定vpp文件
                        strLoadedAcqFifoSettingVppFilePath[i] = strBaseDirectory + "Recipe" +
                            "\\" + "CCD" + (i + 1) + " Share" + "\\" + "AcqFifo_" + strName + ".vpp";
                        // 加载图像处理vpp文件
                        strLoadedVppFilePath[i] = strBaseDirectory + "Recipe" +
                            "\\" + "CCD" + (i + 1) + " Share" + "\\" + "Job_" + strName + ".vpp";
                    }
                }
            }

            // 程序备份文件夹，按照日期命名，保存在每个料号程序的Backup文件夹内
            strRecipeBackupFolderBase = strRecipeFolderPath + "\\Backup";

            // create the folder if the directory doesn't exist
            CheckFolderExist(strRecipeBackupFolderBase);

            // ReadSettingConfig();
            // vpp file doesn't exist in the folder, prompt for checking
            for (int i = 0; i < camNumber; i++)
            {
                CheckFileExist(strLoadedVppFilePath[i]);
            }

            // construct image directory path
            strSaveImageFolderPath = settingHelper.StrSaveImageDirectoryPath +
                "\\" + strSelectedPartNoName;

            // create the folder if the directory doesn't exist
            CheckFolderExist(strSaveImageFolderPath);

            // construct data directory path
            strSaveDataFolderPath = settingHelper.StrSaveDataDirectoryPath + 
                "\\" + strSelectedPartNoName;

            // create the folder if the directory doesn't exist
            CheckFolderExist(strSaveDataFolderPath);

            // create the log file if the file doesn't exist
            strLogFilePath = strBaseDirectory + "Log" +
                "\\" + strSelectedPartNoName + "\\" + "Log.txt";
            CheckFileExist(strLogFilePath);
        }

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <returns></returns>
        public bool CheckFileExist(string path)
        {
            try
            {
                if (!File.Exists(path)) return false;
            }
            catch (Exception)
            {
            }
            return true;
        }

        /// <summary>
        /// 检查文件夹是否存在
        /// </summary>
        /// <returns></returns>
        public bool CheckFolderExist(string path)
        {
            try
            {
                // create the folder if the directory doesn't exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception)
            {
            }
            return true;
        }

        /// <summary>
        /// Load Vpp file from local folder and attach to even handle
        /// retun true if vpp file loading is successful
        /// return false is not, such as vpp file doesn't exist, folder doesn't exist, so on.
        /// </summary>
        public bool LoadVppFile()
        {

            // Step 1 - Create the CogFrameGrabbers

            //CogFrameGrabbers mFrameGrabbers = new CogFrameGrabbers();
            //if (mFrameGrabbers.Count < 1)
            //    throw new CogAcqNoFrameGrabberException("No frame grabbers found");

            // Step 2 - Select the first frame grabber even if there is more than one.
            //mFrameGrabber = mFrameGrabbers[0];

            // Step 3: Create the acq fifo with the selected video format.
            //string videoFormat = mFrameGrabber.AvailableVideoFormats[0];
            // note that CreateAcqFifo will throw an exception if it cannot create the
            // acq fifo with the specified video format.
            //mAcqFifo = mFrameGrabber.CreateAcqFifo(videoFormat,
            //    CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
            //CCD_AcqFifoTool[0].Operator = mAcqFifo;
            //CCD_AcqFifoTool[1].Operator = mAcqFifo;

            // 加载取像文件
            for (int i = 0; i < camNumber; i++)
            {
                if (!File.Exists(strLoadedAcqFifoSettingVppFilePath[i]))
                {
                    MessageBox.Show("CCD" + (i + 1) + "取像文件不存在！请确认路径：" + strLoadedAcqFifoSettingVppFilePath[i]);
                    Environment.Exit(0);
                }
                try
                {
                    ccdAcqFifoTool[i] = (CogAcqFifoTool)CogSerializer.LoadObjectFromFile(
                        strLoadedAcqFifoSettingVppFilePath[i]);
                    // this.CCD_AcqFifoTool[1] = (CogAcqFifoTool)CogSerializer.LoadObjectFromFile(strLoadedCameraSettingVppFilePath[1]);
                    // CCDSetting_ToolGroup = (CogToolGroup)CogSerializer.LoadObjectFromFile(strLoadedCameraSettingFilePath);
                    acqLoadSuccess[i] = true;
                }
                catch (Exception ex)
                {
                    acqLoadSuccess[i] = false;
                    MessageBox.Show("CCD" + (i + 1) + "取像文件作业加载异常，请检查加密狗是否激活!");
                    Environment.Exit(0);
                }
            }

            // 加载Job文件
            for (int i = 0; i < camNumber; i++)
            {
                if (!File.Exists(strLoadedVppFilePath[i]))
                {
                    MessageBox.Show("CCD" + (i + 1) + "作业文件不存在！请确认路径：" + strLoadedVppFilePath[i]);
                    Environment.Exit(0);
                }
                try
                {
                    CCDToolBlock[i] = (CogToolBlock)CogSerializer.LoadObjectFromFile(
                        strLoadedVppFilePath[i]);
                    jobLoadSuccess[i] = true;
                }

                catch (Exception ex)
                {
                    jobLoadSuccess[i] = false;
                    MessageBox.Show("CCD" + (i + 1) + "作业文件作业加载异常，请检查加密狗是否激活!");
                    Environment.Exit(0);
                }
            }

            for (int i = 0; i < camNumber; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(ccdAcqFifoTool[i].RunStatus.Message))
                        openCameraSuccess[i] = true;
                    else openCameraSuccess[i] = false;
                }
                catch (Exception)
                {
                    isCCDOnline = false;
                }
            }

            for (int i = 0; i < camNumber; i++)
            {
                isCCDReady &= acqLoadSuccess[i] & jobLoadSuccess[i];
            }
            return true;
        }

        /// <summary>
        /// 读取配置文件 config.ini 中的各项信息
        /// </summary>
        private void ReadSettingConfigFile()
        {
            string[] strArr;
            string strTemp;
            strConfigFilePath = strBaseDirectory + "Config\\Setting.ini";
            // strIOSettingFilePath = strBaseDirectory + "Config\\IO Table.ini";
            mainConfigIniFile = new IniFile(strConfigFilePath);

            // 软件名称
            settingHelper.SoftwareName = mainConfigIniFile.IniReadValue("Main", "SoftwareName");
            this.Text = "Vision System- " + settingHelper.SoftwareName;

            // 软件语言
            settingHelper.SoftwareLanguage = mainConfigIniFile.IniReadValue("Main", "Language") == "Chinese" ? 
                LanguageType.Chinese : LanguageType.English;

            // 相机数量
            camNumber = Convert.ToInt16(mainConfigIniFile.IniReadValue("Main", "CameraNum"));

            // 读取当前选中的product No.，默认料号序号为0
            selectedPartNoIndex = Convert.ToInt16(mainConfigIniFile.IniReadValue("PartNo", "PNSelected"));
            
            // 允许兼容的最多料号数量
            settingHelper.MaximumProductNoNum = Convert.ToInt16(mainConfigIniFile.IniReadValue("PartNo", "MaxProductNum"));

            // 读取每个CCD对应的料号，如果是来料检，则为独立的料号，如果是半成品或者成品检，则为“NA”
            strTemp = mainConfigIniFile.IniReadValue("PartNo", selectedPartNoIndex.ToString());

            if (!string.IsNullOrEmpty(strTemp))
            {
                strArr = strTemp.Split(':');
                strSelectedPartNoName = strArr[0];
            }

            // 初始化料号Table数据
            settingHelper.InitializePartNoTable(settingHelper.MaximumProductNoNum, camNumber, strConfigFilePath);

            // 更新HomePage的料号选择并添加事件
            pageHome.InitializeComboBoxPartNumber(settingHelper.PartNoList, strSelectedPartNoName);
            pageHome.ChangePartNumber += new ChangePartNumberEventHandler(ChangePartNumberInHomePage);

            // 通信方式定义
            string strCaptureMode = mainConfigIniFile.IniReadValue("Main", "CaptureMode");
            switch (strCaptureMode)
            {
                case "RS232":
                    communicationType = CommunicationType.RS232;
                    break;
                case "TwinCat":
                    communicationType = CommunicationType.TwinCat;
                    twinCat.CamNum = camNumber;
                    // 读取TwinCat ADS通讯NetID 和PortID
                    settingHelper.AdsAmsNetID = mainConfigIniFile.IniReadValue("TwinCat ADS", "AMSNetID");
                    settingHelper.AdsPortNumber = Convert.ToInt16(
                        mainConfigIniFile.IniReadValue("TwinCat ADS", "PortNum"));
                    twinCat.AdsAmsNetID = settingHelper.AdsAmsNetID;
                    twinCat.AdsPortNumber = settingHelper.AdsPortNumber;
                    break;
                case "Fins":
                    communicationType = CommunicationType.Fins;
                    // 读取PLC地址
                    settingHelper.PLCIP = mainConfigIniFile.IniReadValue("PLC Parameter", "IP");
                    settingHelper.PLCPort = Convert.ToInt16(
                        mainConfigIniFile.IniReadValue("PLC Parameter", "Port"));
                    settingHelper.DMStartAddress = Convert.ToInt16(
                        mainConfigIniFile.IniReadValue("PLC Parameter", "StartAdd"));
                    settingHelper.DMDataLength = Convert.ToInt16(
                        mainConfigIniFile.IniReadValue("PLC Parameter", "DataLength"));
                    omronFins.mPLCIP = settingHelper.PLCIP;
                    omronFins.mPLCPort = settingHelper.PLCPort;
                    omronFins.mDMStartAddress = settingHelper.DMStartAddress;
                    omronFins.mDMDataLength = settingHelper.DMDataLength;
                    break;
                case "None":
                    communicationType = CommunicationType.None;
                    break;
                default:
                    break;
            }
            // IO板卡品牌，型号
            strTemp = mainConfigIniFile.IniReadValue("Main", "IOBoardType");
            if (!string.IsNullOrEmpty(strTemp))
            {
                strArr = strTemp.Split(',');
                // 品牌
                if (strArr[0] == "Advantech")
                {
                    settingHelper.IoBoardBrand = IOBoardBrand.Advantech;
                }
                else if (strArr[0] == "ADLINK")
                {
                    settingHelper.IoBoardBrand = IOBoardBrand.ADLINK;
                }
                // 型号
                settingHelper.IoBoardType = strArr[1];
            }
            iOHelper.IoBoardBrand = settingHelper.IoBoardBrand;
            iOHelper.IoBoardType = settingHelper.IoBoardType;

            // IO板卡是否使能
            settingHelper.IsIODeviceEnabled = mainConfigIniFile.IniReadValue("Main", "IsIODeviceEnabled") == "1" ? true : false;

            // IO板卡号码
            settingHelper.IODeviceNumber = Convert.ToInt16(mainConfigIniFile.IniReadValue("Main", "IOBoardNumber"));

            // IO trigger信号是捕捉上升沿还是下降沿
            settingHelper.TrigEdge = mainConfigIniFile.IniReadValue("Main", "TriggerEdge")
                == "R" ? TriggerEdge.Rising : TriggerEdge.Falling;
            iOHelper.TrigEdge = settingHelper.TrigEdge;
            iOHelper.DeviceNum = settingHelper.IODeviceNumber;
            iOHelper.CamNum = camNumber;

            // IO 输入信号高电平有效，还是低电平有效
            settingHelper.ValidInputType = mainConfigIniFile.IniReadValue("Main", "ValidInput")
                == "H" ? ValidInput.High : ValidInput.Low;
            iOHelper.ValidInputType = settingHelper.ValidInputType;

            // IO 输出信号高电平有效，还是低电平有效
            settingHelper.ValidOutputType = mainConfigIniFile.IniReadValue("Main", "ValidOutput")
                == "H" ? ValidOutput.High : ValidOutput.Low;
            iOHelper.ValidOutputType = settingHelper.ValidOutputType;

            // 是否保存数据
            settingHelper.IsSaveData = mainConfigIniFile.IniReadValue("Main", "IsSaveData") 
                == "1" ? true : false;

            // 数据保存格式
            string strDataFormat;
            strDataFormat = mainConfigIniFile.IniReadValue("Main", "DataFormat");
            switch (strDataFormat)
            {
                case "Csv":
                    settingHelper.DataFormat = SaveDataFormat.Csv;
                    break;
                case "Database":
                    settingHelper.DataFormat = SaveDataFormat.DataBase;
                    break;
                default:
                    break;
            }

            // 是否保存OK图片
            settingHelper.IsSaveOKImage = mainConfigIniFile.IniReadValue("Main", "IsSaveOKImage") 
                == "1" ? true : false;

            // 是否保存NG图片
            settingHelper.IsSaveNGImage = mainConfigIniFile.IniReadValue("Main", "IsSaveNGImage") 
                == "1" ? true : false;

            // 是否保存原图还是压缩图片
            string strImageSize;
            strImageSize = mainConfigIniFile.IniReadValue("Main", "ImageSize");
            switch (strImageSize)
            {
                case "Full":
                    settingHelper.ImageSize = SaveImageSize.Full;
                    break;
                case "Half":
                    settingHelper.ImageSize = SaveImageSize.Half;
                    break;
                case "Quater":
                    settingHelper.ImageSize = SaveImageSize.Quater;
                    break;
                default:
                    break;
            }

            // 保存图片的格式：BMP还是JPG
            settingHelper.ImageFormat = mainConfigIniFile.IniReadValue("Main", "ImageFormat") == "BMP" ? 
                SaveImageFormat.BMP : SaveImageFormat.JPG;

            // 数据保存天数
            settingHelper.DataKeepDays = Convert.ToInt16(mainConfigIniFile.IniReadValue("Main", "DataKeepDays"));
            // 图片保存天数
            settingHelper.ImageKeepDays = Convert.ToInt16(mainConfigIniFile.IniReadValue("Main", "ImageKeepDays"));

            //图片和数据保存的文件夹
            settingHelper.StrSaveImageDirectoryPath = mainConfigIniFile.IniReadValue("SavePath", "ImagePath");
            settingHelper.StrSaveDataDirectoryPath = mainConfigIniFile.IniReadValue("SavePath", "DataPath");

            // 读取DataView的类型：DataGrid或者Chart
            settingHelper.ViewType = mainConfigIniFile.IniReadValue("Main", "DataViewType") 
                == "DataGrid" ? ViewType.DataGrid : ViewType.Chart;

            // 是否需要显示DataView
            settingHelper.IsShowDataView = mainConfigIniFile.IniReadValue("Main", "IsShowDataView") 
                == "1" ? true : false;

            // 读取ViewLayout的布局形式：Horizontal还是Vertical
            settingHelper.ViewLayout = mainConfigIniFile.IniReadValue("Main", "DataViewLayout") 
                == "H" ? ViewLayout.Horizontal : ViewLayout.Vertical;

            // 读取Fins输出数据的选择格式
            string strFins = mainConfigIniFile.IniReadValue("FinsOutput", "SelectedIndex");
            if (!string.IsNullOrEmpty(strFins))
            {
                strArr = strFins.Split(',');
                foreach (string item in strArr)
                {
                    settingHelper.FinsDataSelectedIndex.Add(Convert.ToInt32(item));
                }
            }

            // 读取Fins输出数据的地址
            /*string strFinsAdd = mainConfigIniFile.IniReadValue("FinsOutput", "Address");
            if (!string.IsNullOrEmpty(strFinsAdd))
            {
                strArr = strFinsAdd.Split(',');
                foreach (string item in strArr)
                {
                    settingHelper.FinsDataAddress.Add(Convert.ToInt16(item));
                }
            }*/
        }
            
        /// <summary>
        /// 读取 IO Setting.ini file
        /// </summary>
        private void ReadIOConfigFile()
        {
            strIOSettingFilePath = strBaseDirectory + "Config\\IO Table.ini";
            iOHelper.InitializeIOTable(strIOSettingFilePath);
            iOHelper.InitializeIODefinition();

            // 初始化可供使用的触发源列表
            DataRow[] dataRows;
            string strName;
            dataRows = iOHelper.DataTableIOInfo.Select("Type = " + 1 + " and Name LIKE 'Trigger%'");
            if (dataRows.Length >= 1)
            {
                foreach (DataRow dr in dataRows)
                {
                    strName = (string)dr["Name"];
                    triggerNameAndCCDIndex.Add(strName,"");
                    triggerNameAndCCDIndex.Add(strName + "Completed", "");
                }
            }
            // 初始化对应的相机序号
            for (int i = 0; i < camNumber; i++)
            {
                switch (jobHelper[i].TriggerMode)
                {
                    case TriggerMode.FirstOrIndependentTrigger:
                        triggerNameAndCCDIndex[jobHelper[i].TriggerSourceName] = i.ToString();
                        break;
                    case TriggerMode.AttachedTrigger:
                        triggerNameAndCCDIndex[jobHelper[i].TriggerSourceName] = jobHelper[i].AttachedCCDIndex.ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 保存设置到config.ini文件
        /// </summary>
        public void SaveSettingConfigFile()
        {
            // ...
        }
        #endregion

        #region TwinCat ADS communication initialization and notification
        private int mTwinCatAdsReconnectCnt = 0;
        // private FormShowMsg frmShowMsg;
        /// <summary>
        /// 初始化TwinCat PLC ADS连接
        /// </summary>
        private bool InitTwinCat()
        {
            bool success;
            try
            {
                success = twinCat.InitTwincatADS();
                twinCat.AdsClient.AdsNotification += new AdsNotificationEventHandler(OnADSNotification);
            }
            catch (Exception)
            {
                pageHome.LogError("TwinCat ADS 初始化出错");
                return false;
            }
            return success;
        }

        /// <summary>
        /// 当TwinCat PLC变量发生变化时，触发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnADSNotification(object sender, AdsNotificationEventArgs e)
        {
            e.DataStream.Position = e.Offset;
            try
            {
                // 如果是触发信号有变化，判断是否为上升沿，并启动Trigger的后台程序
                for (int i = 0; i < camNumber; i++)
                {
                    if (e.NotificationHandle == twinCat.HConnect[i])
                    {
                        twinCat.TriggerFlag[i] = twinCat.BinRead.ReadBoolean();
                        if (twinCat.TriggerFlag[i] && isCCDOnline)
                        {
                            imageAcqCompleted[i] = false;
                            jobInspectCompleted[i] = false;
                            // 复位inspectCompleted信号
                            twinCat.WritePLC(TwincatHelper.VARIABLEINSPECTCOMPLETED[i], false);
                            // 复位OK和NG结果到PLC
                            twinCat.WritePLC(TwincatHelper.VARIABLERESULTOK[i], false);
                            twinCat.WritePLC(TwincatHelper.VARIABLERESULTNG[i], false);
                            // CCD Ready设置为false，表示相机正忙
                            twinCat.WritePLC(TwincatHelper.VARIABLECCDREADY[i], false);
                            // 启动Trigger后台程序
                            if (!bgwCCDTrigger[i].IsBusy)
                                bgwCCDTrigger[i].RunWorkerAsync(i);
                        }
                    }
                }
                // 如果是PLCOnline信号，读取该信号
                if (e.NotificationHandle == twinCat.HConnect[camNumber])
                    twinCat.PlcOnline = twinCat.BinRead.ReadBoolean();
                // 如果是料号信息改变，程序切换
                else if (e.NotificationHandle == twinCat.HConnect[camNumber + 1])
                {
                    twinCat.PartNoIndex = twinCat.BinRead.ReadInt16();
                    // 跟现在的料号做对比，确认料号是否有变化，如果有变化，执行料号程序切换
                    if (twinCat.PartNoIndex != selectedPartNoIndex)
                    {
                        ChangePartNumber(twinCat.PartNoIndex);
                        pageHome.CancleComboBoxIndexChange(false);
                    }
                }
            }
            catch (Exception ex)
            {
                pageHome.LogError("TwinCat ADS 通知事件通讯出错");
            }
        }

        /// <summary>
        /// 检查TwinCat连接状态，如果连接失败，尝试重新连接，定时器时间间隔为100ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_TwinCatAds_Tick(object sender, EventArgs e)
        {
            Timer_TwinCatAds.Stop();
            bool connect = twinCat.CheckAdsConnectionStatus();
            // 如果从能连接上到不能连接上，启动后台程序不停地尝试连接
            if (isTwinCatConnected && !connect)
            {
                twinCat.AdsClient.AdsNotification -= new AdsNotificationEventHandler(OnADSNotification);
                pageHome.LogError("TwinCat Ads断开连接");
                //frmShowMsg = new FormShowMsg();
                //frmShowMsg.Show();
                mTwinCatAdsReconnectCnt = 0;
                InitTwinCatReConnectBackWorker();
                if (!bgwTwinCatReConnect.IsBusy)
                    bgwTwinCatReConnect.RunWorkerAsync();
            }
            else if (!isTwinCatConnected && connect)
            {
                // 重新连接成功后，取消后台线程
                bgwTwinCatReConnect.CancelAsync();
                for (int i = 0; i < camNumber; i++)
                {
                    // 告诉PLC： CCD Ready，可以开始工作
                    twinCat.WritePLC(TwincatHelper.VARIABLECCDREADY[i], isCCDOnline);
                }
            }

            // 如果连接状态有变化，更新连接状态并显示log
            if (connect != isTwinCatConnected)
            {
                isTwinCatConnected = connect;
                UpdateStatusLabel();
            }

            // 如果连接上了，定时发送CCD online状态
            if (isTwinCatConnected)
                twinCat.WritePLC(TwincatHelper.VARIABLECCDONLINE, isCCDOnline);

            Timer_TwinCatAds.Start();
        }

        /// <summary>
        /// 初始化TwinCat重新连接的后台线程
        /// </summary>
        private void InitTwinCatReConnectBackWorker()
        {
            bgwTwinCatReConnect = new BackgroundWorker();
            bgwTwinCatReConnect.WorkerReportsProgress = true;
            bgwTwinCatReConnect.WorkerSupportsCancellation = true;
            bgwTwinCatReConnect.DoWork += BgW_TwinCatReConnect_DoWork;
            bgwTwinCatReConnect.ProgressChanged += BgW_TwinCatReConnect_ProgressChanged;
            bgwTwinCatReConnect.RunWorkerCompleted += BgW_TwinCatReConnect_RunWorkerCompleted;
        }

        /// <summary>
        /// TwinCat断开连接后，尝试重新连接，时间间隔为10s
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_TwinCatReConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                twinCat.ReConnectToTwinCat();
                // 汇报连接的状态，是否连接上
                bgwTwinCatReConnect.ReportProgress(10);
                Thread.Sleep(10000);
            }
            while (!bgwTwinCatReConnect.CancellationPending);
        }

        /// <summary>
        /// 重新连接次数报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_TwinCatReConnect_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (mTwinCatAdsReconnectCnt < 100)
            {
                mTwinCatAdsReconnectCnt++;
                pageHome.LogError("TwinCat Ads尝试重新连接第 " + mTwinCatAdsReconnectCnt + "次");
                //frmShowMsg.UpdateMsg(mTwinCatAdsReconnectCnt, "TwinCat Ads尝试重新连接第 " + mTwinCatAdsReconnectCnt + "次");
            }
            else
            {
                pageHome.LogError("TwinCat Ads尝试重新连接第 100+ 次");
                //frmShowMsg.UpdateMsg(mTwinCatAdsReconnectCnt, "TwinCat Ads尝试重新连接第 100+ 次");
            }
        }

        /// <summary>
        /// 重新连接成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_TwinCatReConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //frmShowMsg.Close();
            pageHome.LogPass("TwinCat Ads重新连接成功!");
            twinCat.AddVariableOnChangeNotificaiton();
            twinCat.AdsClient.AdsNotification += new AdsNotificationEventHandler(OnADSNotification);
            // 告诉PLC： CCD Ready，可以开始工作
            for (int i = 0; i < camNumber; i++)
                twinCat.WritePLC(TwincatHelper.VARIABLECCDREADY[i], isCCDOnline);
        }
        #endregion

        #region Timer Auto run, check trigger, changeover signal, interval is 10ms
        private void Timer_Auto_Tick(object sender, EventArgs e)
        {
            bool[] triggerData = new bool[camNumber];
            bool changeoverData = false;
            int interval;
            Timer_Auto.Stop();
            if(settingHelper.IsIODeviceEnabled)
            {
                iOHelper.SendCCDOnlineSignal(isCCDOnline);

                #region 相机触发信号处理
                if (isCCDOnline)
                {
                    triggerData = iOHelper.CaptureTriggerSignal();
                    for (int i = 0; i < camNumber; i++)
                    {
                        switch (jobHelper[i].TriggerMode)
                        {
                            case TriggerMode.FirstOrIndependentTrigger:
                                // 如果是独立的触发信号，直接触发相机拍照
                                if (triggerData[i])
                                {
                                    imageAcqCompleted[i] = false;
                                    jobInspectCompleted[i] = false;
                                    // 复位inspectCompleted信号
                                    // mIOHelper.SetInspectCompletedSignalLow(i);
                                    // 复位OK和NG结果到PLC
                                    //iOHelper.SetOKSignalLow(i);
                                    //iOHelper.SetNGSignalLow(i);
                                    // 启动Trigger后台程序
                                    if (!bgwCCDTrigger[i].IsBusy)
                                    {
                                        bgwCCDTrigger[i].RunWorkerAsync(i);
                                    }
                                }
                                break;
                            case TriggerMode.AttachedTrigger:
                                // 如果是附属的触发信号，等待其他相机拍照完成后再触发拍照
                                if (jobInspectCompleted[(int)jobHelper[i].AttachedCCDIndex])
                                {
                                    // 启动Trigger后台程序
                                    if (!bgwCCDTrigger[i].IsBusy)
                                    {
                                        bgwCCDTrigger[i].RunWorkerAsync(i);
                                    }
                                    jobInspectCompleted[(int)jobHelper[i].AttachedCCDIndex] = false;
                                    imageAcqCompleted[i] = false;
                                    jobInspectCompleted[i] = false;
                                    // 复位OK和NG结果到PLC
                                    //iOHelper.SetOKSignalLow(i);
                                    //iOHelper.SetNGSignalLow(i);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                #endregion

                #region 换型使能信号处理
                // 只扫描换型使能信号，当有使能信号时检查PN对应的信号，如果有变化才会换型
                changeoverData = iOHelper.CaptureChangeoverEnableSignal();
                if (changeoverData && isCCDOnline)
                {
                    int index;
                    // 触发换型程序
                    index = iOHelper.CapturePartNoSignal();
                    // 检查料号是否有变化，如果有变化，执行换型
                    // 跟现在的料号做对比，确认料号是否有变化，如果有变化，执行料号程序切换
                    if (index != selectedPartNoIndex)
                    {
                        ChangePartNumber(index);
                    }
                }
                #endregion

                #region PLC Online信号处理
                isPLCOnline = iOHelper.CapturePLCOnlineSignal();
                // 如果PLC Online信号从Online变为Offline，并且在三分钟内一直没有变化，工控机关机
                if (!isPLCOnline && iOHelper.IsPLCOnline)
                {
                    dtPLCOnline = DateTime.Now;
                }
                else if (!isPLCOnline && !iOHelper.IsPLCOnline)
                {
                    interval = DateTime.Now.Subtract(dtPLCOnline).Minutes;
                    if (interval > 3)
                    {
                        // 关闭计算机
                    }
                }
                else if (isPLCOnline && !iOHelper.IsPLCOnline)
                {
                    // PLC从Online变成Offline时，初始化Omron Fins通讯
                    switch (communicationType)
                    {
                        case CommunicationType.TwinCat:
                            break;
                        case CommunicationType.Fins:
                            // 重新初始化Fins通讯
                            omronFins.InitializeOmronFins();
                            break;
                        case CommunicationType.RS232:
                            break;
                        case CommunicationType.None:
                            break;
                        default:
                            break;
                    }
                }
                iOHelper.IsPLCOnline = isPLCOnline;
                UpdateStatusLabel();
                #endregion
            }
            Timer_Auto.Start();
        }
        #endregion

        #region Interrupt for changeover signal, Advantech IO board
        /// <summary>
        /// Automation PCI board Model: 1750, two interrupt channels
        /// Device specification:
        /************************************************************
         *          16 Optically-Isolated Inputs:
         *         Input range : 5 to 50 VDC or dry contact
         *         Isolation voltage : 2,500 VDC
         *         Optical Isolator response time: 100us
         *         
         *         16 Optically-Isolated Outputs:         
         *         Output range : Open collector 5 to 40 VDC
         *         Sink Current: 200mA max. / channel (PCI-1750)
         *         Source Current: 200mA max. / channel (PCI-1750SO)
         *         Isolation voltage : 2,500 VDC
         *         Optical Isolator response time: 100us
         *         Each PCOM Withstanding current: 1.6A max.
         *         Over current ground: CN5 ( wiring necessary when totally current over 1.6A)
         *         
         *         One 16-bit Optically-Isolated Counter:
         *         Shares Pin with isolated input 15
         *         Throughput : 1 MHz Max
         *         Isolation voltage : 2,500 VDC
         *         
         *         One 32-bit Timer:
         *         10 MHz internal clock source
         *         
         *         Interrupt Source:
         *         Interrupt Inputs: 2 ( IDI0,IDI8 )
         *         Optical Isolator response time: 100us
         ************************************************************/

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*private void instantDiCtrl1_Interrupt(object sender, Automation.BDaq.DiSnapEventArgs e)
        {
            switch (e.SrcNum)
            {
                // channel 0 is to capture trigger signal
                case 0:
                    // do nothing here
                    break;
                // channel 1  is to capture the changeover enable signal
                case 8:
                    try
                    {
                        if (IsPCIBoardEnabled)
                        {
                            // the changeover signal is a four-bit signal from PLC
                            // low bit is num1, and high bit is num4
                            // for example, if the PLC send "1", then it is "0" product.
                            byte num1 = 1;
                            byte num2 = 1;
                            byte num3 = 1;
                            byte num4 = 1;
                            // read the PCI board output
                            instantDiCtrl1.ReadBit(mIOHelper.mChangeoverBit1_Portnum, mIOHelper.mChangeoverBit1_Bitnum, out num1);
                            instantDiCtrl1.ReadBit(mIOHelper.mChangeoverBit2_Portnum, mIOHelper.mChangeoverBit2_Bitnum, out num2);
                            instantDiCtrl1.ReadBit(mIOHelper.mChangeoverBit3_Portnum, mIOHelper.mChangeoverBit3_Bitnum, out num3);
                            instantDiCtrl1.ReadBit(mIOHelper.mChangeoverBit4_Portnum, mIOHelper.mChangeoverBit4_Bitnum, out num4);
                            byte product_num = 0x00;
                            if (num1 == 0) product_num |= 0x01;
                            if (num2 == 0) product_num |= 0x01 << 1;
                            if (num3 == 0) product_num |= 0x01 << 2;
                            if (num4 == 0) product_num |= 0x01 << 3;
                            //      PLC product no.    ---------------     Recipe no.
                            //          1              ---------------         0
                            //          2              ---------------         1
                            //    (1 based index)        and so on...    (zero based index)
                            int index = product_num - 1;
                            // check if the index is out of range
                            if (index >= 0 && index <= MaxProductNum)
                            {
                                // read the product name from configuration file according to the product no.
                                mSelectedProductName = ConfigIniFile.IniReadValue("main", index.ToString());
                                if (mSelectedProductName != "" && mSelectedProductName != null)
                                {
                                    // change the product name in the main page
                                    comboBox_ProductName.SelectedIndex = index;
                                    // write the selected product no. to the configuration file
                                    ConfigIniFile.IniWriteValue("main", "mSelectedProduct", index.ToString());
                                    // Set selected product Part No
                                    this.txtPartNo.Text = ConfigIniFile.IniReadValue("PartNo", index.ToString());
                                    // Set Lot No
                                    DateTime dateTime = DateTime.Now;
                                    strLotNo = dateTime.Year.ToString("0000") + dateTime.Month.ToString("00") + dateTime.Day.ToString("00");
                                    txtLotNo.Text = strLotNo;
                                    // load vpp file
                                    AttachToJobManager(false);
                                    LoadVppFile();
                                    AttachToJobManager(true);
                                    ReadProductConfig();
                                    // reset all the statistic data in the main page
                                    ResetStatistics();
                                    ResetStatisticsForAllJobs();
                                    ClearHistoryQueues(sender, e);
                                    InitializeDataGridView();
                                }
                                else
                                {
                                    MessageBox.Show("The product index doesn't exist, please check the product index first!");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Changeover error: " + ex);
                    }
                    break;
                default:
                    break;
            }
        }
        */
        #endregion

        #region Backgroundworker for trigger signal, capture image and inspection
        /// <summary>
        /// 初始化触发后台线程
        /// </summary>
        private void InitTriggerBackWorker()
        {
            for (int i = 0; i < camNumber; i++)
            {
                // 类的数组要对每个元素进行初始化实例
                bgwCCDTrigger[i] = new BackgroundWorker();
                bgwCCDTrigger[i].WorkerReportsProgress = true;
                bgwCCDTrigger[i].DoWork += new DoWorkEventHandler(this.BgW_Trigger_CCD_DoWork);
                bgwCCDTrigger[i].ProgressChanged += new ProgressChangedEventHandler(BgW_Trigger_CCD_ProgressChanged);
                bgwCCDTrigger[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BgW_Trigger_CCD_RunWorkerCompleted);
            }
        }

        /// <summary>
        /// 触发信号处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_Trigger_CCD_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // 判断是哪个CCD的引发的触发信号
            int index = (int)e.Argument;
            dateTime[index] = DateTime.Now;

            // 复位失效模式数据
            switch (communicationType)
            {
                case CommunicationType.TwinCat:
                    break;
                case CommunicationType.Fins:
                    // 复位PLC信号
                    // ResetOmronFinsDataToPLC(index);
                    break;
                case CommunicationType.RS232:
                    break;
                case CommunicationType.None:
                    break;
                default:
                    break;
            }

            // CCD采集图像
            CCDCaptureImage(index);
            imageAcqCompleted[index] = true;

            // 更新进度
            bgwCCDTrigger[index].ReportProgress(50, index);

            // 运行图像处理工具ToolBlock
            RunInspectToolBlock(originalImage[index], index);

            // 获取ToolBlcok中的结果数据
            CollectInspectToolBlockResult(index);
            CollectInspectFailuremodeStatisticsResult(index);
            UpdateFinsDataTable(index);

            // 图像处理完成标志位
            jobInspectCompleted[index] = true;

            // 如果CCD完成了检测，启动后台线程发送结果给PLC并保存图像和数据
            // 发送OK,NG结果给PLC
            if (!bgwWritePLC[index].IsBusy) bgwWritePLC[index].RunWorkerAsync(index);

            /*Dictionary<short, short> mData = new Dictionary<short, short>();
            switch (communicationType)
            {
                case CommunicationType.TwinCat:
                    break;
                case CommunicationType.Fins:
                    // 通过Omron Fins发送失效模式数据给PLC
                    if (omronFins.mOmronFins.FinsConnected && isCCDOnline)
                    {
                        switch (jobHelper[index].TriggerMode)
                        {
                            case TriggerMode.FirstOrIndependentTrigger:
                                // 独立触发源，检查是否有其他相机是跟随该相机二次触发，如果有，则不需要输出信号
                                // 如果是独立的触发源且不是二次拍照，则需要输出
                                if (CheckCCDOutputRequired(index))
                                {
                                    // 发送失效详细数据给PLC
                                    // 获取第i个相机的Fins数据
                                    mData = CollectOmronFinsData(index);
                                    // 发送Fins数据
                                    omronFins.FinsSendData(mData);
                                }
                                break;
                            case TriggerMode.AttachedTrigger:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case CommunicationType.RS232:
                    break;
                case CommunicationType.None:
                    break;
                default:
                    break;
            }*/

            // 检查是否需要保存OK或NG图片
            if (jobHelper[index].IsSaveImage)
            {
                if ((settingHelper.IsSaveOKImage && jobHelper[index].JobTotalResult) ||
                    (settingHelper.IsSaveNGImage && !jobHelper[index].JobTotalResult))
                {
                    // 如果满足以上条件：总设置里面保存OK或NG图片，同时单个CCD保存图像，启动保存图片线程进行图片保存
                    if (!bgwSaveImage[index].IsBusy)
                        bgwSaveImage[index].RunWorkerAsync(index);
                }
            }

            // 检查是否需要保存数据
            if (settingHelper.IsSaveData)
            {
                // 保存数据
                if (!bgwSaveData[index].IsBusy)
                    bgwSaveData[index].RunWorkerAsync(index);
            }

            // 计算从触发到图像处理完成的间隔时间
            jobCTTime[index] = Math.Round(DateTime.Now.Subtract(dateTime[index]).TotalMilliseconds);

            // 更新进度
            bgwCCDTrigger[index].ReportProgress(90, index);

            // 传递index数据给DoWorkEventArgs,这个参数可以被RunWorkerCompleted句柄访问
            e.Result = index;
        }

        /// <summary>
        /// 触发信号任务更新进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_Trigger_CCD_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 50:
                    pageHome.LogMessage("CCD" + ((int)e.UserState + 1) + "取像完成");
                    break;
                case 90:
                    pageHome.LogMessage("CCD" + ((int)e.UserState + 1) + "图像处理完成");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 触发完成后处理事件，更新主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_Trigger_CCD_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            int index = (int)e.Result;
            // 将图像结果更新到CogRecordDisplay
            ICogRecords records = CCDToolBlock[index].CreateLastRunRecord().SubRecords;
            pageHome.UpdateRecordInCogDisplayCtrlByExternalTrigger(index,
                originalImage[index],
                records,
                jobCTTime[index].ToString("0.00") + "ms ",
                jobHelper[index].JobTotalResult);
            // 更新DataGridView中的结果数据
            pageHome.UpdateDatagridViewResult(index);
            Application.DoEvents();
        }

        /// <summary>
        /// 从相机捕获图片
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private ICogImage CCDCaptureImage(int index)
        {
            try
            {
                // 打开光源可能需要延时一段时间
                if (settingHelper.IsIODeviceEnabled && jobHelper[index].IsLightControl && jobHelper[index].IsLightFlash)
                {
                    iOHelper.TurnOnLight(index);
                    Thread.Sleep(INTERVALBETWEENLIGHTONANDIMAGECAPTURE);
                }
                ccdAcqFifoTool[index].Run();
                if (settingHelper.IsIODeviceEnabled && jobHelper[index].IsLightControl && jobHelper[index].IsLightFlash)
                {
                    Thread.Sleep(INTERVALBETWEENIMAGECAPTUREANDLIGHTOFF);
                    iOHelper.TurnOffLight(index);
                }
                originalImage[index] = ccdAcqFifoTool[index].OutputImage;
            }
            catch (Exception)
            {
                pageHome.LogMessage("相机" + (index + 1) + "取像失败");
            }
            return originalImage[index];
        }

        /// <summary>
        /// 运行图像检测程序
        /// </summary>
        /// <param name="image"></param>
        /// <param name="index"></param>
        public void RunInspectToolBlock(ICogImage image, int index)
        {
            try
            {
                CCDToolBlock[index].Inputs["InputImage"].Value = image;
                CCDToolBlock[index].Run();
            }
            catch (Exception)
            {
                pageHome.LogMessage("相机" + (index + 1) + "检测失败");
            }
        }
        #endregion

        #region Backgroundworker to write OK or NG result to PLC
        /// <summary>
        /// 初始化发送PLC信号的后台线程
        /// </summary>
        private void InitWritePLCBackWorker()
        {
            for (int i = 0; i < camNumber; i++)
            {
                // 类的数组要对每个元素进行初始化实例
                bgwWritePLC[i] = new BackgroundWorker();
                bgwWritePLC[i].DoWork += new DoWorkEventHandler(this.BgW_WritePLC_CCD_DoWork);
                bgwWritePLC[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgW_WritePLC_CCD_RunWorkerCompleted);
            }
        }

        /// <summary>
        /// PLC发送信号线程工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_WritePLC_CCD_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int index = (int)e.Argument;
            int? attachedIndex;
            if (settingHelper.IsIODeviceEnabled)
            {
                switch (jobHelper[index].TriggerMode)
                {
                    // 独立触发源，检查是否有其他相机是跟随该相机二次触发，如果有，则不需要输出信号
                    case TriggerMode.FirstOrIndependentTrigger:
                        // 如果是独立的触发源且不是二次拍照，则需要输出
                        if (CheckCCDOutputRequired(index))
                        {
                            if (jobHelper[index].JobTotalResult)
                            {
                                iOHelper.SetOKSignalHigh(index);
                            }
                            else
                            {
                                iOHelper.SetNGSignalHigh(index);
                            }
                            Thread.Sleep(jobHelper[index].DoOutTimePeriod);
                            iOHelper.SetOKSignalLow(index);
                            iOHelper.SetNGSignalLow(index);
                        }
                        break;
                    case TriggerMode.AttachedTrigger:
                        attachedIndex = jobHelper[index].AttachedCCDIndex;
                        if (jobHelper[index].JobTotalResult && jobHelper[(int)attachedIndex].JobTotalResult)
                        {
                            iOHelper.SetOKSignalHigh(index);
                        }
                        else
                        {
                            iOHelper.SetNGSignalHigh(index);
                        }
                        Thread.Sleep(jobHelper[index].DoOutTimePeriod);
                        iOHelper.SetOKSignalLow(index);
                        iOHelper.SetNGSignalLow(index);
                        break;
                    default:
                        break;
                }
            }

            switch (communicationType)
            {
                case CommunicationType.Fins:
                    break;
                case CommunicationType.TwinCat:
                    // 写OK和NG结果到PLC
                    if (jobHelper[index].JobTotalResult)
                    {
                        twinCat.WritePLC(TwincatHelper.VARIABLERESULTOK[index], true);
                        twinCat.WritePLC(TwincatHelper.VARIABLERESULTNG[index], false);
                    }
                    else
                    {
                        twinCat.WritePLC(TwincatHelper.VARIABLERESULTOK[index], false);
                        twinCat.WritePLC(TwincatHelper.VARIABLERESULTNG[index], true);
                    }
                    // 延时后写检测完成信号
                    Thread.Sleep(INTERVALBETWEENOKNGANDCOMPLETESIGNAL);
                    // PLC先读取检测完成信号，如果检测完成，发送OK/NG结果
                    twinCat.WritePLC(TwincatHelper.VARIABLEINSPECTCOMPLETED[index], true);
                    // 输出结果保持延时时间
                    // Thread.Sleep(mSettingHelper.DoOutTimePeriod);
                    // CCD Ready设置为true，表示相机可以进行下一次触发了
                    twinCat.WritePLC(TwincatHelper.VARIABLECCDREADY[index], true);
                    break;
                case CommunicationType.None:
                    break;
                default:
                    break;
            }
            // 传递index数据给DoWorkEventArgs,这个参数可以被RunWorkerCompleted句柄访问
            e.Result = index;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool CheckCCDOutputRequired(int index)
        {
            int? attachedIndex;
            if (jobHelper[index].TriggerMode == TriggerMode.FirstOrIndependentTrigger)
            {
                for (int i = 0; i < camNumber; i++)
                {
                    attachedIndex = jobHelper[i].AttachedCCDIndex;
                    if (attachedIndex != null)
                    {
                        if (attachedIndex == index)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// PLC发送信号线程结束后处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_WritePLC_CCD_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int index = (int)e.Result;
            pageHome.LogMessage("CCD" + (index + 1) + "OK,NG信号已经发送给PLC");
        }
        #endregion

        #region Backgroundworker to failure result through Omron Fins
        /// <summary>
        /// 初始化TwinCat重新连接的后台线程
        /// </summary>
        private void InitFinsTotalResultBackWorker()
        {
            bgwFinsTotalResultSender = new BackgroundWorker();
            bgwFinsTotalResultSender.DoWork += BgW_FinsTotalResultSender_DoWork;
        }

        /// <summary>
        /// TwinCat断开连接后，尝试重新连接，时间间隔为10s
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_FinsTotalResultSender_DoWork(object sender, DoWorkEventArgs e)
        {
            List<short> mData;
            do
            {
                if (omronFins.mOmronFins.FinsConnected)
                {
                    omronFins.mFinsConnStatus = true;
                    isOmronFinsConnected = true;
                    if (isCCDOnline)
                    {
                        // 换型完成后，通过Fins通信反馈料号给PLC，默认情况下料号信息保存在D5000地址里面
                        omronFins.FinsSendPN((short)selectedPartNoIndex);
                        // 发送失效详细数据给PLC
                        mData = CollectOmronFinsData();
                        // 发送Fins数据，默认情况下失效模式信息以位的形式保存在D5001起始的地址里面
                        omronFins.FinsSendData(mData.ToArray(), (short)mData.Count);
                    }
                }
                else
                {
                    omronFins.mFinsConnStatus = false;
                    isOmronFinsConnected = false;
                }
                Thread.Sleep(50);
            }
            while (!bgwFinsTotalResultSender.CancellationPending);
        }

        /// <summary>
        /// 一次性获取所有相机的失效模式数据，并以二进制位的形式保存失效模式，Dictionary key为地址，value为对应的数据
        /// </summary>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        private List<short> CollectOmronFinsData()
        {
            List<short> dataArr = new List<short>(); // 每一个数据对应一个short类型
            List<short> allData = new List<short>(); // 每一个数据对应一个Bit为，0为NG，1为OK
            for (int index = 0; index < camNumber; index++)
            {
                if (dataTableSingleCCDFins[index].Rows.Count >= 1)
                {
                    foreach (DataRow dr in dataTableSingleCCDFins[index].Rows)
                    {
                        dataArr.Add((short)dr["Value"]);
                    }
                }
            }

            // 将short型数据转化为对应Bit型数据，重新组合成新的short型数据进行输出
            short mTemp = 0;
            int count = 0;
            for (short i = 0; i < dataArr.Count; i++)
            {
                if (dataArr[i] == 1)
                {
                    mTemp = (short)(mTemp + (short)Math.Pow(2, count));
                }
                count++;
                if (count == 16)
                {
                    allData.Add(mTemp);
                    mTemp = 0;
                    count = 0;
                }
            }

            // 当dataArr的列表长度不是16的整数倍时，最后一个mTemp没有加入到allData里面，需要通过以下代码添加
            if (dataArr.Count % 16 != 0)
            {
                allData.Add(mTemp);
            }
            return allData;
        }

        /// <summary>
        /// 从Dictionary中搜索value对应的key值，如果没找到，返回null
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private int? GetValueFromDictionary(Dictionary<int, string> dict, string key)
        {
            var query = dict.Where(p => p.Value.ToLower().Contains(key.ToLower())).Select(p => p.Key);
            List<int> keys = query.ToList();
            if (keys != null && keys.Count > 0)
                // 如果有多个结果，只考虑第一个查找到的结果
                return keys[0];
            else
                return null;
        }
        #endregion

        #region Collect data and do analysis, update dataTable
        /// <summary>
        /// 初始化Fins发送数据列表
        /// </summary>
        private void InitializeFinsDataTable()
        {
            // Datatable初始化：清除所有内容
            dataTableFinsData.Rows.Clear();
            dataTableFinsOutput.Rows.Clear();
            dataTableFinsData.Columns.Clear();
            dataTableFinsOutput.Columns.Clear();
            for (int i = 0; i < camNumber; i++)
            {
                dataTableSingleCCDFins[i].Rows.Clear();
                dataTableSingleCCDFins[i].Columns.Clear();
            }

            // 注意Column name不能有空格，否则在查询语句中会报错
            dataTableFinsData.Columns.Add("No", typeof(int));
            dataTableFinsData.Columns.Add("Name", typeof(string));
            dataTableFinsData.Columns.Add("Description", typeof(string));
            dataTableFinsData.Columns.Add("Value", typeof(short));
            dataTableFinsData.Columns.Add("IsSelected", typeof(bool));
            dataTableFinsData.Columns.Add("IndexInOutput", typeof(int));
            dataTableFinsData.Columns.Add("CCDIndex", typeof(int));         // 所属的CCD序号，从0开始

            DataRow row;
            int index = 0;

            // 相机序号
            /*for (int i = 0; i < camNumber; i++)
            {
                row = dataTableFinsData.NewRow();
                row[0] = index;
                row[1] = "CCD index:" + (i + 1);
                row[2] = jobHelper[i].CcdName + "相机序号";
                row[3] = i + 1;                
                row[4] = false;
                row[5] = 0;
                row[6] = i;
                dataTableFinsData.Rows.Add(row);
                index++;
            }*/

            // 单个相机检测总数
            /*for (int i = 0; i < camNumber; i++)
            {
                row = dataTableFinsData.NewRow();
                row[0] = index;
                row[1] = "CCD total count:" + (i + 1);
                row[2] = jobHelper[i].CcdName + "相机检测总计数";
                row[3] = 0;
                row[4] = false;
                row[5] = 0;
                row[6] = i;
                dataTableFinsData.Rows.Add(row);
                index++;
            }*/

            // 单个相机检测总结果
            for (int i = 0; i < camNumber; i++)
            {
                row = dataTableFinsData.NewRow();
                row[0] = index;
                row[1] = "CCD result:" + (i + 1);
                row[2] = jobHelper[i].CcdName + "相机检测结果 " + "(OK为1, NG为0)";
                row[3] = 1;
                row[4] = false;
                row[5] = 0;
                row[6] = i;
                dataTableFinsData.Rows.Add(row);
                index++;
            }

            // 如果是二次触发，汇总两次拍照的总结果
            for (int i = 0; i < camNumber; i++)
            {
                if (jobHelper[i].TriggerMode == TriggerMode.AttachedTrigger)
                {
                    row = dataTableFinsData.NewRow();
                    row[0] = index;
                    row[1] = "CCD total result:" + (i + 1);
                    row[2] = jobHelper[i].CcdName + "相机二次拍照汇总结果 " + "(OK为1, NG为0)";
                    row[3] = 1;
                    row[4] = false;
                    row[5] = 0;
                    row[6] = i;
                    dataTableFinsData.Rows.Add(row);
                    index++;
                }
            }

            // 单个相机检测通过数
            /*for (int i = 0; i < camNumber; i++)
            {
                row = dataTableFinsData.NewRow();
                row[0] = index;
                row[1] = "CCD total run pass count:" + (i + 1);
                row[2] = jobHelper[i].CcdName + "相机检测Pass计数";
                row[3] = 0;
                row[4] = false;
                row[5] = 0;
                row[6] = i;
                dataTableFinsData.Rows.Add(row);
                index++;
            }*/

            // 相机失效模式对应的检测结果
            for (int i = 0; i < camNumber; i++)
            {
                for (int j = 0; j < jobHelper[i].FailuremodeKeyWd.Count; j++)
                {
                    row = dataTableFinsData.NewRow();
                    row[0] = index;
                    row[1] = "CCD" + (i + 1) + "failure result: " + jobHelper[i].FailuremodeKeyWd[j];
                    row[2] = jobHelper[i].CcdName + "失效模式结果: " + jobHelper[i].FailuremodeKeyWd[j] + "  (OK为1, NG为0)";
                    row[3] = 1;
                    row[4] = false;
                    row[5] = 0;
                    row[6] = i;
                    dataTableFinsData.Rows.Add(row);
                    index++;
                }
            }

            // 相机失效模式统计数
            /*for (int i = 0; i < camNumber; i++)
            {
                for (int j = 0; j < jobHelper[i].FailuremodeKeyWd.Count; j++)
                {
                    row = dataTableFinsData.NewRow();
                    row[0] = index;
                    row[1] = "CCD" + (i + 1) + "failure count: " + jobHelper[i].FailuremodeKeyWd[j];
                    row[2] = jobHelper[i].CcdName + "失效模式统计数: " + jobHelper[i].FailuremodeKeyWd[j];
                    row[3] = 0;
                    row[4] = false;
                    row[5] = 0;
                    row[6] = i;
                    dataTableFinsData.Rows.Add(row);
                    index++;
                }
            }*/

            // 根据配置文件是否选中数据作为输出，初始化DataTableSelected
            DataRow[] dataRows;
            int rowIndex;
            foreach (int item in settingHelper.FinsDataSelectedIndex)
            {
                dataRows = dataTableFinsData.Select("No = " + item);
                if (dataRows.Length >= 1)
                {
                    rowIndex = dataTableFinsData.Rows.IndexOf(dataRows[0]);
                    dataTableFinsData.Rows[rowIndex]["IsSelected"] = true;
                    int indexInOutput = settingHelper.FinsDataSelectedIndex.IndexOf(item);
                    dataTableFinsData.Rows[rowIndex]["IndexInOutput"] = indexInOutput;
                }
            }

            // 选出IsSelected为true的行，将其数据拷贝到mDataTableFinsOutput中
            dataRows = dataTableFinsData.Select("IsSelected = " + true, "IndexInOutput ASC");
            if (dataRows.Count() >= 1)
            {
                dataTableFinsOutput = dataRows.CopyToDataTable();
            }

            // 单个相机Fins数据，防止多线程操作时出错
            for (int i = 0; i < camNumber; i++)
            {
                // 单个CCD的Fins数据
                dataRows = dataTableFinsData.Select("IsSelected = " + true +
                " and CCDIndex = " + i, "IndexInOutput ASC");
                if (dataRows.Count() >= 1)
                {
                    dataTableSingleCCDFins[i] = dataRows.CopyToDataTable();
                }
            }
        }

        /// <summary>
        /// 更新Fins发送数据列表
        /// </summary>
        private void UpdateFinsDataTable(int index)
        {
            int rowIndex = 0;
            DataRow[] dataRows;

            // 相机序号，不需要更新

            // 单个相机检测总数
            /*dataRows = dataTableSingleCCDFins[index].Select("Name = " + "'CCD total count:" + (index + 1) + "'");
            if (dataRows.Length >= 1)
            {
                rowIndex = dataTableSingleCCDFins[index].Rows.IndexOf(dataRows[0]);
                dataTableSingleCCDFins[index].Rows[rowIndex]["Value"] = (short)jobHelper[index].JobTotalRunCnt;
            }*/

            // 单个相机检测总结果
            dataRows = dataTableSingleCCDFins[index].Select("Name = " + "'CCD result:" + (index + 1) + "'");
            if (dataRows.Length >= 1)
            {
                rowIndex = dataTableSingleCCDFins[index].Rows.IndexOf(dataRows[0]);
                dataTableSingleCCDFins[index].Rows[rowIndex]["Value"] = (short)(jobHelper[index].JobTotalResult ? 1 : 0);
            }

            // 如果是二次触发，汇总两次拍照的总结果
            if (jobHelper[index].TriggerMode == TriggerMode.AttachedTrigger)
            {
                int attachedIndex = (int)jobHelper[index].AttachedCCDIndex;
                dataRows = dataTableSingleCCDFins[index].Select("Name = " + "'CCD total result:" + (index + 1) + "'");
                if (dataRows.Length >= 1)
                {
                    rowIndex = dataTableSingleCCDFins[index].Rows.IndexOf(dataRows[0]);
                    dataTableSingleCCDFins[index].Rows[rowIndex]["Value"] =
                        (short)(jobHelper[index].JobTotalResult && jobHelper[attachedIndex].JobTotalResult ? 1 : 0);
                }
            }

            // 单个相机检测通过数
            /*dataRows = dataTableSingleCCDFins[index].Select("Name = " + "'CCD total run pass count:" + (index + 1) + "'");
            if (dataRows.Length >= 1)
            {
                rowIndex = dataTableSingleCCDFins[index].Rows.IndexOf(dataRows[0]);
                dataTableSingleCCDFins[index].Rows[rowIndex]["Value"] = (short)jobHelper[index].JobTotalRunPass;
            }*/

            // 相机失效模式对应的检测结果
            for (int j = 0; j < jobHelper[index].FailuremodeKeyWd.Count; j++)
            {
                dataRows = dataTableSingleCCDFins[index].Select("Name = " + "'CCD" + (index + 1) +
                "failure result: " + jobHelper[index].FailuremodeKeyWd[j] + "'");
                if (dataRows.Length >= 1)
                {
                    rowIndex = dataTableSingleCCDFins[index].Rows.IndexOf(dataRows[0]);
                    dataTableSingleCCDFins[index].Rows[rowIndex]["Value"] = (short)(jobHelper[index].FailResultForKeyWd[j] ? 1 : 0);
                }
            }

            // 相机失效模式统计数
            /*for (int j = 0; j < jobHelper[index].FailuremodeKeyWd.Count; j++)
            {
                dataRows = dataTableSingleCCDFins[index].Select("Name = " + "'CCD" + (index + 1) + 
                    "failure count: " + jobHelper[index].FailuremodeKeyWd[j] + "'");
                if (dataRows.Length >= 1)
                {
                    rowIndex = dataTableSingleCCDFins[index].Rows.IndexOf(dataRows[0]);
                    dataTableSingleCCDFins[index].Rows[rowIndex]["Value"] = (short)jobHelper[index].FailCountForKeyWd[j];
                }
            }*/
        }

        /// <summary>
        /// 获取检测结果
        /// </summary>
        /// <param name="index"></param>
        private void CollectInspectToolBlockResult(int index)
        {
            try
            {
                CogToolBlock tool;

                //适用于编写脚本判断结果的情况下，ToolBlock有一个输出为"Result"，
                //脚本控制Result的结果，这种情况下ToolBlock的判断结果为输出"Result" + 
                //工具运行的结果是否为Accept
                bool outputResult;

                bool toolResult; //每个toolBlock的分结果
                bool totalResult = true; //每个job的总结果
                for (int j = 0; j < jobHelper[index].ToolBlockCnt; j++)
                {
                    //每一个单项检测结果
                    tool = (CogToolBlock)CCDToolBlock[index].Tools[j];
                    jobHelper[index].TotalCountForItem[j] += 1;
                    outputResult = true; //默认情况下outputResult为Pass
                    for (int k = 0; k < tool.Outputs.Count; k++)
                    {
                        //检查ToolBlock的输出中是否有"result"一项，如果有，则需要取其判断结果
                        if (tool.Outputs[k].ValueType.ToString().ToLower() == "system.boolean" &&
                            tool.Outputs[k].Name.ToLower().Equals("result"))
                        {
                            outputResult = (bool)tool.Outputs[k].Value;
                        }
                    }
                    toolResult = outputResult && (tool.RunStatus.Result == CogToolResultConstants.Accept);

                    // 如果检测工具使能，计数才有效，否则不计数
                    if (jobHelper[index].IsToolBlockEnabledForItem[j])
                    {
                        jobHelper[index].PassFailResultForItem[j] = toolResult ? true : false;
                        jobHelper[index].FailCountForItem[j] += toolResult ? 0 : 1;
                    }
                    else
                    {
                        // 如果检测工具没有使能，结果为true
                        jobHelper[index].PassFailResultForItem[j] = true;
                        jobHelper[index].FailCountForItem[j] += 0;
                    }
                    //计算总结果
                    totalResult &= jobHelper[index].PassFailResultForItem[j];

                    // 如果总数为0，为避免除数为0的异常错误，将yield设置为null
                    if (jobHelper[index].TotalCountForItem[j] == 0)
                    {
                        jobHelper[index].YieldForItem[j] = null;
                    }
                    else
                    {
                        jobHelper[index].YieldForItem[j] = 1 - (double)jobHelper[index].FailCountForItem[j]
                            / jobHelper[index].TotalCountForItem[j];
                    }
                }
                //job运行数加一
                jobHelper[index].JobTotalRunCnt += 1;
                //job通过计数
                jobHelper[index].JobTotalRunPass += totalResult ? 1 : 0;
                jobHelper[index].JobTotalResult = totalResult;
                //计算通过率
                if (jobHelper[index].JobTotalRunCnt == 0)
                {
                    jobHelper[index].JobYield = null;
                }
                else
                {
                    jobHelper[index].JobYield = jobHelper[index].JobTotalRunPass / (double)jobHelper[index].JobTotalRunCnt;
                }

                //整个ToolBlock的input输入
                jobHelper[index].InputData.Clear();
                for (int i = 0; i < CCDToolBlock[index].Inputs.Count; i++)
                {
                    jobHelper[index].InputData.Add(CCDToolBlock[index].Inputs[i].Value);
                }

                //整个ToolBlock的output数据输出
                jobHelper[index].OutputData.Clear();
                for (int i = 0; i < CCDToolBlock[index].Outputs.Count; i++)
                {
                    jobHelper[index].OutputData.Add(CCDToolBlock[index].Outputs[i].Value);
                }
            }
            catch (Exception)
            {
                pageHome.LogError("获取检测结果失败！");
            }
        }

        /// <summary>
        /// 失效模式统计结果更新
        /// </summary>
        /// <param name="index"></param>
        private void CollectInspectFailuremodeStatisticsResult(int index)
        {
            int? mFailureIndex;
            try
            {
                // 查找失效模式关键字对应的OK、NG结果，如果ToolBlock名称里不能找到关键字，默认结果为True
                for (int i = 0; i < jobHelper[index].FailuremodeKeyWd.Count; i++)
                {
                    mFailureIndex = GetValueFromDictionary(jobHelper[index].ToolBlockNameDictionary,
                        jobHelper[index].FailuremodeKeyWd[i]);
                    if (mFailureIndex != null)
                    {
                        jobHelper[index].FailResultForKeyWd[i] = jobHelper[index].PassFailResultForItem[(int)mFailureIndex];
                    }
                    else
                    {
                        jobHelper[index].FailResultForKeyWd[i] = true;
                    }
                    // 如果指定关键字的失效模式为False，总数+1
                    if (!jobHelper[index].FailResultForKeyWd[i])
                        jobHelper[index].FailCountForKeyWd[i]++;
                }
            }
            catch (Exception)
            {
                // do nothing here
            }
        }
        #endregion

        #region Update status label: PLC, Camera, TCP, IO board, etc.
        /// <summary>
        /// 初始化状态栏及可见状态
        /// </summary>
        private void InitStatusLabel()
        {
            if (settingHelper.IsIODeviceEnabled)
            {
                StatusLabel_IO.Text = "";
                StatusLabel_IO.BackColor = Color.Lime;
            }
            else
            {
                StatusLabel_IO.Visible = false;
            }
        }

        /// <summary>
        /// 更新状态栏
        /// </summary>
        private void UpdateStatusLabel()
        {
            // IO board status
            if(settingHelper.IsIODeviceEnabled)
            {
                if (iOHelper.IsIODeviceConnected)
                {
                    StatusLabel_IO.BackColor = Color.Lime;
                    switch (settingHelper.SoftwareLanguage)
                    {
                        case LanguageType.Chinese:
                            StatusLabel_IO.Text = "IO板卡已连接";
                            break;
                        case LanguageType.English:
                            StatusLabel_IO.Text = "IO Connected";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    StatusLabel_IO.BackColor = Color.Red;
                    switch (settingHelper.SoftwareLanguage)
                    {
                        case LanguageType.Chinese:
                            StatusLabel_IO.Text = "IO板卡异常";
                            break;
                        case LanguageType.English:
                            StatusLabel_IO.Text = "IO Disconnected";
                            break;
                        default:
                            break;
                    }
                }

                if (iOHelper.IsPLCOnline)
                {
                    StatusLabel_PLCOnline.BackColor = Color.Lime;
                    switch (settingHelper.SoftwareLanguage)
                    {
                        case LanguageType.Chinese:
                            StatusLabel_PLCOnline.Text = "PLC 在线";
                            break;
                        case LanguageType.English:
                            StatusLabel_PLCOnline.Text = "PLC Online";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    StatusLabel_PLCOnline.BackColor = Color.Red;
                    switch (settingHelper.SoftwareLanguage)
                    {
                        case LanguageType.Chinese:
                            StatusLabel_PLCOnline.Text = "PLC 离线";
                            break;
                        case LanguageType.English:
                            StatusLabel_PLCOnline.Text = "PLC Offline";
                            break;
                        default:
                            break;
                    }
                }
            }

            switch (communicationType)
            {
                case CommunicationType.TwinCat:
                    // TwinCat connection status
                    if (isTwinCatConnected)
                    {
                        StatusLabel_SerialPort.BackColor = Color.Lime;
                        switch (settingHelper.SoftwareLanguage)
                        {
                            case LanguageType.Chinese:
                                StatusLabel_SerialPort.Text = "TCP 已连接";
                                break;
                            case LanguageType.English:
                                StatusLabel_SerialPort.Text = "TCP Connected";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        StatusLabel_SerialPort.BackColor = Color.Red;
                        switch (settingHelper.SoftwareLanguage)
                        {
                            case LanguageType.Chinese:
                                StatusLabel_SerialPort.Text = "TCP 断开";
                                break;
                            case LanguageType.English:
                                StatusLabel_SerialPort.Text = "TCP Disconnected";
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case CommunicationType.Fins:
                    // Fins connection status
                    if (isOmronFinsConnected)
                    {
                        StatusLabel_SerialPort.BackColor = Color.Lime;
                        switch (settingHelper.SoftwareLanguage)
                        {
                            case LanguageType.Chinese:
                                StatusLabel_SerialPort.Text = "TCP 已连接";
                                break;
                            case LanguageType.English:
                                StatusLabel_SerialPort.Text = "TCP Connected";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        StatusLabel_SerialPort.BackColor = Color.Red;
                        switch (settingHelper.SoftwareLanguage)
                        {
                            case LanguageType.Chinese:
                                StatusLabel_SerialPort.Text = "TCP 断开";
                                break;
                            case LanguageType.English:
                                StatusLabel_SerialPort.Text = "TCP Disconnected";
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case CommunicationType.RS232:
                    break;
                case CommunicationType.None:
                    // 如果没有串口通讯，删掉该标签
                    StatusLabel_SerialPort.Enabled = false;
                    break;
                default:
                    break;
            }

            if (isCCDOnline)
            {
                StatusLabel_CCDStatus.BackColor = Color.Lime;
                switch (settingHelper.SoftwareLanguage)
                {
                    case LanguageType.Chinese:
                        StatusLabel_CCDStatus.Text = "CCD 在线";
                        break;
                    case LanguageType.English:
                        StatusLabel_CCDStatus.Text = "CCD Online";
                        break;
                    default:
                        break;
                }
            }
            else
            {                
                StatusLabel_CCDStatus.BackColor = Color.Red;
                switch (settingHelper.SoftwareLanguage)
                {
                    case LanguageType.Chinese:
                        StatusLabel_CCDStatus.Text = "CCD 离线";
                        break;
                    case LanguageType.English:
                        StatusLabel_CCDStatus.Text = "CCD Offline";
                        break;
                    default:
                        break;
                }
            }

            // show current login information
            switch (settingHelper.SoftwareLanguage)
            {
                case LanguageType.Chinese:
                    StatusLabel_Login.Text = string.Format("当前登陆用户：{0}", currentAccessLevel.ToString());
                    break;
                case LanguageType.English:
                    StatusLabel_Login.Text = string.Format("Current Login：{0}", currentAccessLevel.ToString());
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 根据当前的登陆状态更新控件和界面
        /// </summary>
        private void InitAccessLevel()
        {
            this.toolStripCameraSetting.Enabled = (!isCCDOnline && currentAccessLevel >= AccessLevel.Administrator) ? true : false;
            this.toolStripEdit.Enabled = (!isCCDOnline && currentAccessLevel >= AccessLevel.Administrator) ? true : false;
            this.toolStripPlayBack.Enabled = (!isCCDOnline && currentAccessLevel >= AccessLevel.Operator) ? true : false;
            this.toolStripSetting.Enabled = (!isCCDOnline && currentAccessLevel >= AccessLevel.Supervisor) ? true : false;
        }

        /// <summary>
        /// 根据当前的登陆状态更新控件和界面
        /// </summary>
        private void UpdateAccessLevel()
        {
            this.toolStripCameraSetting.Enabled = (!isCCDOnline && currentAccessLevel >= AccessLevel.Administrator) ? true : false;
            this.toolStripEdit.Enabled = (!isCCDOnline && currentAccessLevel >= AccessLevel.Administrator) ? true : false;
            this.toolStripPlayBack.Enabled = (!isCCDOnline && currentAccessLevel >= AccessLevel.Operator) ? true : false;
            this.toolStripSetting.Enabled = (!isCCDOnline && currentAccessLevel >= AccessLevel.Supervisor) ? true : false;
            pageHome.SetPartNoComboBoxEnabled((!isCCDOnline && currentAccessLevel >= AccessLevel.Supervisor) ? true : false);
            pageHome.SetCogDisplayCtlSettingButtonEnabled(currentAccessLevel >= AccessLevel.Supervisor ? true : false);
        }
        #endregion

        #region Change part number in home page, load vpp file from folder
        private BackgroundWorker bgwLoadVpp;
        private FormShowAsynProg frmLoadVppProgress;        

        /// <summary>
        /// 切换料号子程序
        /// </summary>
        /// <param name="partNoIndex"></param>
        private void ChangePartNumber(int partNoIndex)
        {
            DataRow[] dataRows;
            dataRows = settingHelper.DataTablePartNoInfo.Select("PNIndex = " + partNoIndex);
            // 如果料号不在范围，直接返回，不执行任何操作
            // 查询序号对应的料号名称
            if (dataRows.Length == 0)
            {
                return;
            }
            else
            {
                selectedPartNoIndex = partNoIndex;
                strSelectedPartNoName = (string)dataRows[0]["PNName"];
                mainConfigIniFile.IniWriteValue("PartNo", "PNSelected", selectedPartNoIndex.ToString());
                ResolveFilePath();
                bgwLoadVpp = new BackgroundWorker();
                bgwLoadVpp.DoWork += BgwLoadVpp_DoWork;
                bgwLoadVpp.WorkerReportsProgress = true;
                bgwLoadVpp.ProgressChanged += BgwLoadVpp_ProgressChanged;
                bgwLoadVpp.RunWorkerCompleted += BgwLoadVpp_RunWorkerCompleted;
                frmLoadVppProgress = new FormShowAsynProg();
                frmLoadVppProgress.Show();
                bgwLoadVpp.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 在PageHome中手动切换料号，弹出窗口询问是否确认，如果确认，
        /// 执行程序切换，如果取消，换回之前的料号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="partNoIndex"></param>
        private void ChangePartNumberInHomePage(object sender, int partNoIndex)
        {
            if (MessageBox.Show("确定要换型吗?", "请确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                ChangePartNumber(partNoIndex);
                pageHome.CancleComboBoxIndexChange(false);
            }
            else
            {
                pageHome.CancleComboBoxIndexChange(true);
            }
        }

        private void BgwLoadVpp_DoWork(object sender, DoWorkEventArgs e)
        {
            int progStep = 100 / (camNumber * 2);
            // 传递进度百分比和文件路径信息，用于更新界面
            // "0"表示不需要更新路径信息
            bgwLoadVpp.ReportProgress(10, "0");

            // 加载相机设定Vpp文件
            for (int i = 0; i < camNumber; i++)
            {
                try
                {
                    bgwLoadVpp.ReportProgress(i * progStep, strLoadedAcqFifoSettingVppFilePath[i]);
                    ccdAcqFifoTool[i] = (CogAcqFifoTool)CogSerializer.LoadObjectFromFile(strLoadedAcqFifoSettingVppFilePath[i]);
                    acqLoadSuccess[i] = true;
                }
                catch (Exception ex)
                {
                    acqLoadSuccess[i] = false;
                    isCCDOnline = false;
                    MessageBox.Show("取像文件作业加载异常，请确认作业文件是否有效！！");
                }
                Thread.Sleep(200);
            }

            // 加载图像处理Vpp文件
            for (int i = 0; i < camNumber; i++)
            {
                try
                {
                    bgwLoadVpp.ReportProgress(50 + i * progStep, strLoadedVppFilePath[i]);
                    CCDToolBlock[i] = (CogToolBlock)CogSerializer.LoadObjectFromFile(strLoadedVppFilePath[i]);
                    jobLoadSuccess[i] = true;
                }
                catch (Exception ex)
                {
                    jobLoadSuccess[i] = false;
                    isCCDOnline = false;
                    MessageBox.Show("CCD" + (i + 1) + "作业加载异常，请确认作业文件是否有效！");
                }
                Thread.Sleep(200);
            }
            // 100% 完成
            bgwLoadVpp.ReportProgress(100, "0");
        }

        private void BgwLoadVpp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            frmLoadVppProgress.Close();
            // MessageBox.Show("换型成功！");
            UpdateJobSetting();
            pageCamerasetting.SetupAcqToolEditSubject(ccdAcqFifoTool);
            pageEdit.SetupToolBlockEditSubject(CCDToolBlock);
            pageHome.UpdateToolBlock(CCDToolBlock, ccdAcqFifoTool);
            pageHome.InitDatagirdViewResult();
            pageHome.ClearAllStastics();
            pageHome.ClearAllDisplayImage();
            pageHome.UpdateComboBoxPartNumber(settingHelper.PartNoList, strSelectedPartNoName);
        }

        private void BgwLoadVpp_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string path = e.UserState as string;
            //"0"表示不需要更新路径信息，直接返回
            if (path.Equals("0")) return;
            //如果不为"0"，更新进度条和路径信息
            string strInfo = "正在加载Vpp文件： " + path + "，进度为： " + e.ProgressPercentage.ToString() + "%";
            frmLoadVppProgress.UpdateProgress(e.ProgressPercentage, strInfo);
        }
        
        private void UpdateJobSetting()
        {
            CogToolBlock tool;
            for (int i = 0; i < camNumber; i++)
            {
                jobHelper[i].ToolBlockCnt = CCDToolBlock[i].Tools.Count;
                jobHelper[i].TotalCountForItem.Clear();
                jobHelper[i].FailCountForItem.Clear();
                jobHelper[i].YieldForItem.Clear();
                jobHelper[i].PassFailResultForItem.Clear();
                jobHelper[i].ToolBlockNameDictionary.Clear();
                jobHelper[i].IsToolBlockEnabledForItem.Clear();

                jobHelper[i].ToolBlockCnt = CCDToolBlock[i].Tools.Count;
                for (int j = 0; j < jobHelper[i].ToolBlockCnt; j++)
                {
                    // 读取每个CCD检测工具的使能状态
                    // 默认状态下每个toolBlock是开启的
                    jobHelper[i].IsToolBlockEnabledForItem.Add(true);
                    tool = (CogToolBlock)CCDToolBlock[i].Tools[j];
                    jobHelper[i].TotalCountForItem.Add(0);
                    jobHelper[i].FailCountForItem.Add(0);
                    jobHelper[i].YieldForItem.Add(1.0);
                    jobHelper[i].PassFailResultForItem.Add(true);
                    jobHelper[i].ToolBlockNameDictionary.Add(
                        j,
                        tool.Name);
                }
                jobHelper[i].JobTotalRunCnt = 0;
                jobHelper[i].JobTotalRunPass = 0;
                jobHelper[i].JobYield = null;
            }
        }
        #endregion

        #region Save vpp file to folder, background worker to show progress
        private BackgroundWorker bgwSaveVpp;
        private FormShowAsynProg frmSaveVppProgress;

        private void SaveVppFileToLocalFolder()
        {
            // 启动新的BackgroundWorker线程保存Vpp文件，并通过一个进度条显示状态
            bgwSaveVpp = new BackgroundWorker();
            bgwSaveVpp.DoWork += BgwSaveVpp_DoWork;
            bgwSaveVpp.WorkerReportsProgress = true;
            bgwSaveVpp.ProgressChanged += BgwSaveVpp_ProgressChanged;
            bgwSaveVpp.RunWorkerCompleted += BgwSaveVpp_RunWorkerCompleted;
            frmSaveVppProgress = new FormShowAsynProg();
            frmSaveVppProgress.Show();
            // 开启BackgroundWorker线程
            bgwSaveVpp.RunWorkerAsync();
        }

        private void BgwSaveVpp_DoWork(object sender, DoWorkEventArgs e)
        {
            int progStep = 100 / (camNumber * 2);
            // 传递进度百分比和文件路径信息，用于更新界面
            // "0"表示不需要更新路径信息
            bgwSaveVpp.ReportProgress(10, "0");

            // 保存相机设定Vpp文件到本地文件夹
            for (int i = 0; i < camNumber; i++)
            {
                try
                {
                    bgwSaveVpp.ReportProgress(i * progStep, strLoadedAcqFifoSettingVppFilePath[i]);
                    CogSerializer.SaveObjectToFile(ccdAcqFifoTool[i], strLoadedAcqFifoSettingVppFilePath[i]);
                }
                catch (Exception)
                {
                    MessageBox.Show("CCD" + (i + 1) + "相机设定文件保存异常，请确认文件是否有效！");
                }
                Thread.Sleep(200);
            }

            // 保存图像处理Vpp文件到本地文件夹
            for (int i = 0; i < camNumber; i++)
            {
                try
                {
                    bgwSaveVpp.ReportProgress(50 + i * progStep, strLoadedVppFilePath[i]);
                    CogSerializer.SaveObjectToFile(CCDToolBlock[i], strLoadedVppFilePath[i]);
                }
                catch (Exception)
                {
                    MessageBox.Show("CCD" + (i + 1) + "作业文件加载异常，请确认文件是否有效！");
                }
                Thread.Sleep(200);
            }
            // 100% 完成
            bgwSaveVpp.ReportProgress(100, "0");
        }

        private void BgwSaveVpp_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string path = e.UserState as string;
            //"0"表示不需要更新路径信息，直接返回
            if (path.Equals("0")) return;
            //如果不为"0"，更新进度条和路径信息
            string strInfo = "正在保存Vpp文件： " + path + "，进度为： " + e.ProgressPercentage.ToString() + "%";
            frmSaveVppProgress.UpdateProgress(e.ProgressPercentage, strInfo);
        }

        private void BgwSaveVpp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 关闭进度显示窗口
            frmSaveVppProgress.Close();
            MessageBox.Show("程序保存成功！");
        }
        #endregion

        #region Backgroundworker to save image in local computer
        /// <summary>
        /// 初始化图像保存后台线程
        /// </summary>
        private void InitImageSaveBackWorker()
        {
            for (int i = 0; i < camNumber; i++)
            {
                bgwSaveImage[i] = new BackgroundWorker();
                bgwSaveImage[i].DoWork += new DoWorkEventHandler(BgW_CCDImageSave_DoWork);
                bgwSaveImage[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgW_CCDImageSave_RunWorkerCompleted);
            }
        }

        /// <summary>
        /// 保存图片工作线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_CCDImageSave_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int index = (int)e.Argument;
            // if "IsSaveOKImage == true and mRunResult == true" 
            // or "IsSaveNGResult == true and mRunResult == false"
            // save the image to local folder

            // get current Date time to specify the folder name
            string strDate = DateTime.Now.Year + "_" +
                                DateTime.Now.Month + "_" +
                                DateTime.Now.Day;

            // construct the whole folder name
            string strFolderName = strSaveImageFolderPath + "\\" + 
                jobHelper[index].CcdName + "\\" + strDate + "\\" +
                (jobHelper[index].JobTotalResult ? "OK" : "NG");

            // create the folder if the directory doesn't exist
            if (!Directory.Exists(strFolderName))
            {
                Directory.CreateDirectory(strFolderName);
            }
            try
            {
                // construct the file name of image
                string strTime = DateTime.Now.Hour.ToString("00") + "-" +
                                DateTime.Now.Minute.ToString("00") + "-" +
                                DateTime.Now.Second.ToString("00");
                string strFileName = strFolderName + "\\" + strTime +
                    (settingHelper.ImageFormat == SaveImageFormat.BMP ? ".bmp" : ".jpg");

                // save image if it is not null
                // save original image or scaled image according to config setting
                CogImageFileTool mImageFileTool = new CogImageFileTool();
                if (originalImage[index] != null)
                {
                    switch (settingHelper.ImageSize)
                    {
                        case SaveImageSize.Full:
                            mImageFileTool.InputImage = originalImage[index];
                            break;
                        case SaveImageSize.Half:
                            mImageFileTool.InputImage = originalImage[index].ScaleImage(originalImage[index].Width / 2,
                                originalImage[index].Height / 2);
                            break;
                        case SaveImageSize.Quater:
                            mImageFileTool.InputImage = originalImage[index].ScaleImage(originalImage[index].Width / 4,
                                originalImage[index].Height / 4);
                            break;
                        default:
                            break;
                    }
                    mImageFileTool.Operator.Open(strFileName, CogImageFileModeConstants.Write);
                    mImageFileTool.Run();
                }
                // collect garbage
                GC.Collect();
                e.Result = index;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存图像出错！" + ex.ToString());
            }
        }

        /// <summary>
        /// 图片保存后发送log信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_CCDImageSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int index = (int)e.Result;
            pageHome.LogMessage("CCD" + (index + 1) + "图片已保存到本地电脑中");
        }

        #endregion

        #region Backgroundworker to save data in local computer
        /// <summary>
        /// 初始化图像保存后台线程
        /// </summary>
        private void InitDataSaveBackWorker()
        {
            for (int i = 0; i < camNumber; i++)
            {
                csvList[i] = new CsvRow();
                bgwSaveData[i] = new BackgroundWorker();
                bgwSaveData[i].DoWork += new DoWorkEventHandler(BgW_CCDDataSave_DoWork);
                bgwSaveData[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgW_CCDDataSave_RunWorkerCompleted);
            }
        }

        /// <summary>
        /// 保存图片工作线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_CCDDataSave_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int index = (int)e.Argument;
            bool result;
            FileStream fileStream;
            CsvRow csvRowHeader;
            CsvFileWriter streamWriter;
            try
            {
                // 文件夹名字按照日期命名
                string strDate = DateTime.Now.Year + "_" +
                                DateTime.Now.Month + "_" +
                                DateTime.Now.Day;

                string strFileName = strSaveDataFolderPath + "\\" + strDate + "\\" +
                    jobHelper[index].CcdName + "_Result.csv";
                CheckFolderExist(strSaveDataFolderPath + "\\" + strDate);

                // 如果文件不存在，需要添加文件头
                if (!File.Exists(strFileName))
                {
                    fileStream = new FileStream(strFileName, FileMode.Append, FileAccess.Write);
                    // streamWriter = new CsvFileWriter(fileStream);
                    // 支持中文字符写入使用以下语句
                    streamWriter = new CsvFileWriter(fileStream, System.Text.Encoding.GetEncoding("gb2312"));
                    csvRowHeader = new CsvRow();
                    csvRowHeader.Add("Time");
                    csvRowHeader.Add("Total Result");

                    // 单个ToolBlock的名称
                    for (int i = 0; i < jobHelper[index].ToolBlockCnt; i++)
                    {
                        csvRowHeader.Add(CCDToolBlock[index].Tools[i].Name.ToString());
                        // csvRowHeader.Add("ToolBlock #" + (i + 1));
                    }

                    // Job ToolBlock的输入，除掉一些无法输出的数据：如图片等
                    for (int i = 0; i < CCDToolBlock[index].Inputs.Count; i++)
                    {
                        string[] allString;
                        allString = CCDToolBlock[index].Inputs[i].ValueType.ToString().Split('.');
                        string inputType = allString[allString.Length - 1].ToLower();
                        if (inputType == "int16" || inputType == "int32" || inputType == "double" ||
                            inputType == "boolean" || inputType == "string")
                        {
                            csvRowHeader.Add(CCDToolBlock[index].Inputs[i].Name.ToString());
                        }
                    }

                    // Job ToolBlock的输出，除掉一些无法输出的数据：如图片等
                    for (int i = 0; i < CCDToolBlock[index].Outputs.Count; i++)
                    {
                        string[] allString;
                        allString = CCDToolBlock[index].Outputs[i].ValueType.ToString().Split('.');
                        string outputType = allString[allString.Length - 1].ToLower();
                        if (outputType == "int16" || outputType == "int32" || outputType == "double" ||
                            outputType == "boolean" || outputType == "string")
                        {
                            csvRowHeader.Add(CCDToolBlock[index].Outputs[i].Name.ToString());
                        }
                    }
                    // 添加头一行
                    streamWriter.WriteRow(csvRowHeader);

                    // 第一列添加当前日期和时间
                    string strDateTime = DateTime.Now.Hour.ToString("00") + ":" +
                        DateTime.Now.Minute.ToString("00") + ":" +
                        DateTime.Now.Second.ToString("00");
                    csvList[index].Add(strDateTime);

                    // 单个Job的总结果：Pass/Fail
                    result = jobHelper[index].JobTotalResult;
                    csvList[index].Add(result ? "Pass" : "Fail");

                    // 后面添加每个检测工具的失效信息(OK/NG)
                    for (int i = 0; i < jobHelper[index].ToolBlockCnt; i++)
                    {
                        result = jobHelper[index].PassFailResultForItem[i];
                        csvList[index].Add(result ? "OK" : "NG");
                    }

                    // 整个ToolBlock的Input输入数据(如果有)
                    for (int i = 0; i < jobHelper[index].InputData.Count; i++)
                    {
                        string[] allString;
                        allString = CCDToolBlock[index].Inputs[i].ValueType.ToString().Split('.');
                        string inputType = allString[allString.Length - 1].ToLower();
                        if (inputType == "int16" || inputType == "int32" || inputType == "string")
                        {
                            csvList[index].Add(jobHelper[index].InputData[i].ToString());
                        }
                        else if (inputType == "boolean")
                        {
                            csvList[index].Add(((bool)jobHelper[index].InputData[i]) ? "1" : "0");
                        }
                        else if (inputType == "double")
                        {
                            csvList[index].Add(((double)jobHelper[index].InputData[i]).ToString("0.000"));
                        }
                    }

                    // 整个ToolBlock的Output输出数据(如果有)
                    for (int i = 0; i < jobHelper[index].OutputData.Count; i++)
                    {
                        string[] allString;
                        allString = CCDToolBlock[index].Outputs[i].ValueType.ToString().Split('.');
                        string outputType = allString[allString.Length - 1].ToLower();
                        if (outputType == "int16" || outputType == "int32" || outputType == "string")
                        {
                            csvList[index].Add(jobHelper[index].OutputData[i].ToString());
                        }
                        else if (outputType == "boolean")
                        {
                            csvList[index].Add(((bool)jobHelper[index].OutputData[i]) ? "1" : "0");
                        }
                        else if (outputType == "double")
                        {
                            csvList[index].Add(((double)jobHelper[index].OutputData[i]).ToString("0.000"));
                        }
                    }

                    streamWriter.WriteRow(csvList[index]);
                    streamWriter.Flush();
                    streamWriter.Close();
                    fileStream.Close();
                    csvList[index].Clear();
                }
                else
                {
                    fileStream = new FileStream(strFileName, FileMode.Append, FileAccess.Write);
                    // streamWriter = new CsvFileWriter(fileStream);
                    // 支持中文字符写入使用以下语句
                    streamWriter = new CsvFileWriter(fileStream, System.Text.Encoding.GetEncoding("gb2312"));
                    // 第一列添加当前日期和时间
                    string strDateTime = DateTime.Now.Hour.ToString("00") + ":" +
                        DateTime.Now.Minute.ToString("00") + ":" +
                        DateTime.Now.Second.ToString("00");
                    csvList[index].Add(strDateTime);

                    // 单个Job的总结果：Pass/Fail
                    result = jobHelper[index].JobTotalResult;
                    csvList[index].Add(result ? "Pass" : "Fail");

                    // 后面添加每个检测工具的失效信息(OK/NG)
                    for (int i = 0; i < jobHelper[index].ToolBlockCnt; i++)
                    {
                        result = jobHelper[index].PassFailResultForItem[i];
                        csvList[index].Add(result ? "OK" : "NG");
                    }

                    // 整个ToolBlock的Input输入数据(如果有)
                    for (int i = 0; i < jobHelper[index].InputData.Count; i++)
                    {
                        string[] allString;
                        allString = CCDToolBlock[index].Inputs[i].ValueType.ToString().Split('.');
                        string inputType = allString[allString.Length - 1].ToLower();
                        if (inputType == "int16" || inputType == "int32" || inputType == "string")
                        {
                            csvList[index].Add(jobHelper[index].InputData[i].ToString());
                        }
                        else if (inputType == "boolean")
                        {
                            csvList[index].Add(((bool)jobHelper[index].InputData[i]) ? "1" : "0");
                        }
                        else if (inputType == "double")
                        {
                            csvList[index].Add(((double)jobHelper[index].InputData[i]).ToString("0.000"));
                        }
                    }

                    // 整个ToolBlock的Output输出数据(如果有)
                    for (int i = 0; i < jobHelper[index].OutputData.Count; i++)
                    {
                        string[] allString;
                        allString = CCDToolBlock[index].Outputs[i].ValueType.ToString().Split('.');
                        string outputType = allString[allString.Length - 1].ToLower();
                        if (outputType == "int16" || outputType == "int32" || outputType == "string")
                        {
                            csvList[index].Add(jobHelper[index].OutputData[i].ToString());
                        }
                        else if (outputType == "boolean")
                        {
                            csvList[index].Add(((bool)jobHelper[index].OutputData[i]) ? "1" : "0");
                        }
                        else if (outputType == "double")
                        {
                            csvList[index].Add(((double)jobHelper[index].OutputData[i]).ToString("0.000"));
                        }
                    }

                    streamWriter.WriteRow(csvList[index]);
                    streamWriter.Flush();
                    streamWriter.Close();
                    fileStream.Close();
                    csvList[index].Clear();
                }
                e.Result = index;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错！" + ex.ToString());
            }
        }

        /// <summary>
        /// 图片保存后发送log信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_CCDDataSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int index = (int)e.Result;
            pageHome.LogMessage("CCD" + (index + 1) + "数据已保存到本地电脑中");
        }
        #endregion

        #region Backgroundworker to backup vpp program in folder
        private BackgroundWorker bgwSaveProgram;
        private FormShowAsynProg frmProgramBackupProgress;

        private void BackupProgram()
        {
            // 启动新的BackgroundWorker线程保存程序，并通过一个进度条显示状态
            bgwSaveProgram = new BackgroundWorker();
            bgwSaveProgram.DoWork += BgwBackupProgram_DoWork;
            bgwSaveProgram.WorkerReportsProgress = true;
            bgwSaveProgram.ProgressChanged += BgwBackupProgram_ProgressChanged;
            bgwSaveProgram.RunWorkerCompleted += BgwBackupProgram_RunWorkerCompleted;
            frmProgramBackupProgress = new FormShowAsynProg();
            frmProgramBackupProgress.Show();
            // 开启BackgroundWorker线程
            bgwSaveProgram.RunWorkerAsync();
        }

        private void BgwBackupProgram_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] filesPathArray;
            string sourceFolder, targetFolder;
            sourceFolder = strRecipeFolderPath;
            targetFolder = strCCDProgramBackupFolderPath;
            // 检查目标文件夹不存在
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            filesPathArray = Directory.GetFiles(sourceFolder);
            if (filesPathArray.Length > 0)
            {
                int progStep = 100 / (filesPathArray.Length);
                for (int i = 0; i < filesPathArray.Length; i++)
                {
                    FileInfo fi = new FileInfo(filesPathArray[i]);
                    long fileSize = fi.Length; //文件大小
                    File.Copy(filesPathArray[i], targetFolder + "\\" + fi.Name, true);
                    // 传递进度百分比和文件路径信息，用于更新界面
                    // "0"表示不需要更新路径信息
                    bgwSaveProgram.ReportProgress(progStep * i, fi.FullName);
                    Thread.Sleep(200);
                }
            }
            // 100% 完成
            bgwSaveProgram.ReportProgress(100, "0");
        }

        private void BgwBackupProgram_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string path = e.UserState as string;
            //"0"表示不需要更新路径信息，直接返回
            if (path.Equals("0")) return;
            //如果不为"0"，更新进度条和路径信息
            string strInfo = "正在备份Vpp文件： " + path + "，进度为： " + e.ProgressPercentage.ToString() + "%";
            frmProgramBackupProgress.UpdateProgress(e.ProgressPercentage, strInfo);
        }

        private void BgwBackupProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 关闭进度显示窗口
            frmProgramBackupProgress.Close();
            MessageBox.Show("程序备份成功！");
        }

        #endregion

        #region Backgroundworker to restore program from backup folder
        private BackgroundWorker bgwRestoreProgram;
        private FormShowAsynProg frmProgramRestoreProgress;

        private void RestoreProgram()
        {
            // 启动新的BackgroundWorker线程保存程序，并通过一个进度条显示状态
            bgwRestoreProgram = new BackgroundWorker();
            bgwRestoreProgram.DoWork += BgwRestoreProgram_DoWork;
            bgwRestoreProgram.WorkerReportsProgress = true;
            bgwRestoreProgram.ProgressChanged += BgwRestoreProgram_ProgressChanged;
            bgwRestoreProgram.RunWorkerCompleted += BgwResotoreProgram_RunWorkerCompleted;
            frmProgramRestoreProgress = new FormShowAsynProg();
            frmProgramRestoreProgress.Show();
            // 开启BackgroundWorker线程
            bgwRestoreProgram.RunWorkerAsync();
        }

        private void BgwRestoreProgram_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] filesPathArray;
            string sourceFolder, targetFolder;
            sourceFolder = strCCDProgramBackupFolderPath;
            targetFolder = strRecipeFolderPath;
            // 检查目标文件夹不存在
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            filesPathArray = Directory.GetFiles(sourceFolder);
            if (filesPathArray.Length == 0) return;
            int progStep = 80 / (filesPathArray.Length);
            for (int i = 0; i < filesPathArray.Length; i++)
            {
                FileInfo fi = new FileInfo(filesPathArray[i]);
                // 文件大小
                long fileSize = fi.Length;
                // 只拷贝Vpp格式的文件
                if (fi.Extension == ".vpp")
                    // 允许同名文件拷贝覆盖
                    File.Copy(filesPathArray[i], targetFolder + "\\" + fi.Name, true);
                // 传递进度百分比和文件路径信息，用于更新界面
                // "0"表示不需要更新路径信息
                bgwRestoreProgram.ReportProgress(progStep * i, fi.FullName);
                Thread.Sleep(100);
            }
            // 从文件夹中重新加载vpp文件
            LoadVppFile();
            Thread.Sleep(100);
            // 100% 完成
            bgwRestoreProgram.ReportProgress(100, "0");
        }

        private void BgwRestoreProgram_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string path = e.UserState as string;
            //"0"表示不需要更新路径信息，直接返回
            if (path.Equals("0")) return;
            //如果不为"0"，更新进度条和路径信息
            string strInfo = "正在备份Vpp文件： " + path + "，进度为： " + e.ProgressPercentage.ToString() + "%";
            frmProgramRestoreProgress.UpdateProgress(e.ProgressPercentage, strInfo);
        }

        private void BgwResotoreProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 关闭进度显示窗口
            frmProgramRestoreProgress.Close();
            MessageBox.Show("程序恢复成功！");
        }
        #endregion

        #region Debug timer runtine, interval is 100 ms
        private void Timer_Debug_Tick(object sender, EventArgs e)
        {
            pageHome.LogPass("This is ok");
            pageHome.LogFail("This is fail information");
            pageHome.LogMessage("This is only for message");
        }
        #endregion
    }

    #region Internal helper class
    // helper class to store localized name and enum value, for use in combo box display
    internal class AccessLevel_Localized
    {
        public AccessLevel_Localized(AccessLevel v, string t)
        {
            val = v;
            text = t;
        }

        public override string ToString()
        {
            // return the localized name
            return text;
        }

        public AccessLevel val;
        public string text;
    }
    #endregion
}
