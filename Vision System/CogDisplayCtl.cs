using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ToolBlock;
using System.IO;
using Cognex.VisionPro.ImageFile;
using System.Threading;
using System.Collections;

namespace Vision_System
{
    public delegate void JobResultAvailableEventHandler(object sender);
    public delegate void UpdateHomePageLayoutEventHandler(object sender);
    public delegate void ResetDisplayEventHandler(object sender); //清除单个Display的图像和计数
    public delegate void TurnOnLightSourceEventHandler(object sender); //委托打开光源
    public delegate void TurnOffLightSourceEventHandler(object sender); //委托关闭光源

    public partial class CogDisplayCtl : UserControl
    {
        // 光源打开等待时间
        private const int INTERVALBETWEENLIGHTONANDTRIGGER = 20;
        // 定义不同的CogDisplay StaticGraphic的groupname，用于显示不同的数据和信息
        private const string LIVEVIDEOLABELGROUPNAME = "Live Video Label";
        private const string OKNGLABELGROUPNAME = "OK/NG info";
        private const string FAILUREINFOGROUPNAME = "Failure Info";
        private const string CAMERANAMELABELGROUPNAME = "Camera Number Lable";
        private const string PROCESSINGTIME = "Processing Time";
        private const CogColorConstants CAMERANAMELABELBACKGROUNDCOLOR = CogColorConstants.White;
        private const CogColorConstants PROCESSINGTIMELABELBACKGROUNDCOLOR = CogColorConstants.White;
        private const CogColorConstants FAILUREINFOBACKGROUNDCOLOR = CogColorConstants.None;
        private const int FAILUREINFOLINEGAP = 80; //失效信息的行间距
        private const int FONTSIZEOKNG = 30; //Camera Label的字体大小
        private const int FONTSIZEFAILUREINFO = 10;
        private const int FONTSIZECAMERANAME = 15;
        private const int FONTSIZEPROCESSINGTIME = 15;
        private const int FONTSIZELIVEVIDEO = 10;
        public CogGraphicLabel labelCameraName;
        private ViewType currentViewType = ViewType.Chart;
        private ViewLayout currentViewLayout = ViewLayout.Horizontal;
        private bool _isProcessingTimeLabelVisible = true; // 默认ProcessingTimeLabel是可见的，如果是小缩略图模式，则隐藏
        private bool _isFailureInfoLabelVisible = true; // 默认ProcessingTimeLabel是可见的，如果是小缩略图模式，则隐藏
        private int _ccdIndex = 0;
        private int _lastRunRecordIndex = 0; // 用于选择不同的last run 纪录

        #region chart variable
        private const int CHARTDATANUM = 100;
        private int index = 1;
        private Queue<double> yLowerLimit = new Queue<double>();
        private Queue<double> yUpperLimit = new Queue<double>();
        private Queue<double> yData = new Queue<double>();
        private Random random = new Random();
        #endregion

        private DataTable dtDataResultDetails = new DataTable("data_result_details");
        private const int HISTORYRECORDMAX = 20;
        private const int HISTORYRECORDNUM = 20; 
        private Queue queueHistoryResult = new Queue(HISTORYRECORDNUM); //定义保存历史数据的队列容量为20
        private PictureBox[] pictureBoxHistoryResult;
        private bool _isCameraConnected;
        private bool _isLive = false;
        private bool _isRunning = false;
        private CogToolBlock _cogToolBlock = new CogToolBlock();
        private CogAcqFifoTool _cogAcqFifoTool;
        private ICogRecord _cogRecord;
        private ICogImage _cogOriginalImage;
        private double _imageProcessingTimeMilliSecond;
        private bool _jobRunResult;
        private bool[] _toolRunResult;
        private int _imageWidth;
        private int _imageHeight;
        private int _liveVideoCnt = 0; // 实时在线模式时用于显示“实时显示模式”字样
        private bool _isShowDataView = true;
        private int _jobRunCnt = 0;
        private int _jobPassCnt = 0;
        private double _jobYield = 0.90;
        private CogDisplayCtrlStatus controlStatus = CogDisplayCtrlStatus.Available;
        private bool _isFirstImageForLiveVideo = true;

        public event JobResultAvailableEventHandler JobResultAvailable = null;
        public event UpdateHomePageLayoutEventHandler UpdateHomePageLayout = null;
        public event ResetDisplayEventHandler ResetDisplay = null;
        public event TurnOnLightSourceEventHandler TurnOnLight = null;
        public event TurnOffLightSourceEventHandler TurnOffLight = null;

        public bool IsLive { get => _isLive; set => _isLive = value; }
        public ICogRecord CogRecord { get => _cogRecord; set => _cogRecord = value; }
        public ICogImage CogOriginalImage { get => _cogOriginalImage; set => _cogOriginalImage = value; }
        public CogToolBlock ToolBlock { get => _cogToolBlock; set => _cogToolBlock = value; }
        public bool IsCameraConnected { get => _isCameraConnected; set => _isCameraConnected = value; }
        public double ImageProcessingTimeMilliSecond { get => _imageProcessingTimeMilliSecond; set => _imageProcessingTimeMilliSecond = value; }
        public bool IsRunning { get => _isRunning; set => _isRunning = value; }
        public bool JobRunResult { get => _jobRunResult; set => _jobRunResult = value; }
        public int ImageWidth { get => _imageWidth; set => _imageWidth = value; }
        public int ImageHeight { get => _imageHeight; set => _imageHeight = value; }
        public CogAcqFifoTool AcqFifoTool { get => _cogAcqFifoTool; set => _cogAcqFifoTool = value; }
        public bool IsShowDataView { get => _isShowDataView; set => _isShowDataView = value; }
        public int JobRunCnt { get => _jobRunCnt; set => _jobRunCnt = value; }
        public int JobPassCnt { get => _jobPassCnt; set => _jobPassCnt = value; }
        public double JobYield { get => _jobYield; set => _jobYield = value; }
        public CogDisplayCtrlStatus ControlStatus { get => controlStatus; set => controlStatus = value; }
        public bool IsFirstImageForLiveVideo { get => _isFirstImageForLiveVideo; set => _isFirstImageForLiveVideo = value; }
        public ViewType CurrentViewType { get => currentViewType; set => currentViewType = value; }
        public ViewLayout CurrentViewLayout { get => currentViewLayout; set => currentViewLayout = value; }
        public int CCDIndex { get => _ccdIndex; set => _ccdIndex = value; }
        public int LastRunRecordIndex { get => _lastRunRecordIndex; set => _lastRunRecordIndex = value; }
        public bool[] ToolRunResult { get => _toolRunResult; set => _toolRunResult = value; }
        public bool IsProcessingTimeLabelVisible { get => _isProcessingTimeLabelVisible; set => _isProcessingTimeLabelVisible = value; }
        public bool IsFailureInfoLabelVisible { get => _isFailureInfoLabelVisible; set => _isFailureInfoLabelVisible = value; }

        // DisplayCtrl的当前状态
        public enum CogDisplayCtrlStatus
        {
            Available,
            LiveVideo,
            SingleRunning,
            ContinuousRunning,
        }

        /// <summary>
        /// 不带任何参数的构造函数
        /// </summary>
        public CogDisplayCtl()
        {
            InitializeComponent();
            // 将PictureBox控件编成数组形式
            pictureBoxHistoryResult = new PictureBox[HISTORYRECORDMAX]{
                pictureBox1,
                pictureBox2,
                pictureBox3,
                pictureBox4,
                pictureBox5,
                pictureBox6,
                pictureBox7,
                pictureBox8,
                pictureBox9,
                pictureBox10,
                pictureBox11,
                pictureBox12,
                pictureBox13,
                pictureBox14,
                pictureBox15,
                pictureBox16,
                pictureBox17,
                pictureBox18,
                pictureBox19,
                pictureBox20
            };
            for (int i = 0; i < pictureBoxHistoryResult.Length; i++)
            {
                pictureBoxHistoryResult[i].Image = null;
            }
        }

        /// <summary>
        /// 构造函数，传递CogToolBlock和AcqFifoTool
        /// </summary>
        /// <param name="toolBlock"></param>
        /// <param name="fifoTool"></param>
        public CogDisplayCtl(CogToolBlock toolBlock, CogAcqFifoTool fifoTool)
        {
            this.ToolBlock = toolBlock;
            this.AcqFifoTool = fifoTool;
        }

        /// <summary>
        /// 加载控件，关联状态栏和cogRecordDisplay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CogDisplayCtl_Load(object sender, EventArgs e)
        {
            CurrentViewType = FormMain.settingHelper.ViewType;
            CurrentViewLayout = FormMain.settingHelper.ViewLayout;
            IsShowDataView = FormMain.settingHelper.IsShowDataView;
            this.cogDisplayStatusBar.Display = this.cogRecordDisplay;
            SetToolBarVisible(false);

            // 初始化运行，通过计数
            this.lblRunData.Text = "Total: --";

            InitializeDataGridView();
            InitializeChartView();
            UpdateViewLayout();
        }

        /// <summary>
        /// 初始化DataGridView：初始化Column以及样式
        /// </summary>
        private void InitializeDataGridView()
        {
            dgvDataResultView.DataSource = dtDataResultDetails;
            dtDataResultDetails.Columns.Add("No.", typeof(string));
            dtDataResultDetails.Columns.Add("项目", typeof(string));
            dtDataResultDetails.Columns.Add("结果", typeof(string));
            dtDataResultDetails.Columns.Add("下限", typeof(string));
            dtDataResultDetails.Columns.Add("上限", typeof(string));
            dtDataResultDetails.Columns.Add("OK/NG", typeof(Bitmap));
            InitializeDataGridViewStyle();
        }

        /// <summary>
        /// 初始化DataGridView样式
        /// </summary>
        private void InitializeDataGridViewStyle()
        {
            // 设置表格样式
            DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.Linen;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvDataResultView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvDataResultView.BackgroundColor = System.Drawing.SystemColors.Control;
            dgvDataResultView.GridColor = System.Drawing.SystemColors.ControlLight;
            // 设置表格交替样式
            dgvDataResultView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dgvDataResultView.Columns[0].Width = 30;
            dgvDataResultView.Columns[1].Width = 80;
            dgvDataResultView.Columns[2].Width = 60;
            dgvDataResultView.Columns[3].Width = 60;
            dgvDataResultView.Columns[4].Width = 60;
            dgvDataResultView.Columns[5].Width = 50;
        }

        /// <summary>
        /// 初始化ChartView：定义数据格式，图表样式
        /// </summary>
        private void InitializeChartView()
        {
            // 添加series到图表中
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            // Series1, series2, series3
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.Color = System.Drawing.Color.Red;
            series1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            series1.BorderWidth = 2;

            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            series2.Color = System.Drawing.Color.Red;
            series2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            series2.BorderWidth = 2;

            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "Series3";
            series3.ToolTip = "#VAL";
            series3.Color = System.Drawing.Color.Black;

            this.chartData.Series.Add(series1);
            this.chartData.Series.Add(series2);
            this.chartData.Series.Add(series3);
        }

        /// <summary>
        /// 设置按钮在登录之后才能打开
        /// </summary>
        /// <param name="enable"></param>
        public void SetParamSettingButtonEnabled(bool enable)
        {
            toolStripBtnParamSetting.Enabled = enable;
        }

        /// <summary>
        /// 更新DataView的Layout
        /// </summary>
        public void UpdateViewLayout()
        {
            if (CurrentViewLayout == ViewLayout.Vertical)
            {
                this.tableLayoutPanel1.Controls.Clear();
                this.tableLayoutPanel1.ColumnCount = 1;
                this.tableLayoutPanel1.ColumnStyles.Clear();
                this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                this.tableLayoutPanel1.RowCount = 1;
                this.tableLayoutPanel1.RowStyles.Clear();
                this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
                // 如果不需要显示图表，则不进行显示
                this.tableLayoutPanel2.Controls.Clear();
                if (IsShowDataView)
                {
                    this.tableLayoutPanel2.ColumnCount = 1;
                    this.tableLayoutPanel2.ColumnStyles.Clear();
                    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                    this.tableLayoutPanel2.RowCount = 3;
                    this.tableLayoutPanel2.RowStyles.Clear();
                    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
                    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
                    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
                    this.tableLayoutPanel2.Controls.Add(this.cogRecordDisplay, 0, 0);
                    this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
                    if (CurrentViewType == ViewType.Chart)
                        this.tableLayoutPanel2.Controls.Add(this.chartData, 0, 2);
                    else if (CurrentViewType == ViewType.DataGrid)
                        this.tableLayoutPanel2.Controls.Add(this.dgvDataResultView, 0, 2);
                }
                else
                {
                    this.tableLayoutPanel2.ColumnCount = 1;
                    this.tableLayoutPanel2.ColumnStyles.Clear();
                    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                    this.tableLayoutPanel2.RowCount = 2;
                    this.tableLayoutPanel2.RowStyles.Clear();
                    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
                    this.tableLayoutPanel2.Controls.Add(this.cogRecordDisplay, 0, 0);
                    this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
                }
            }
            else if (CurrentViewLayout == ViewLayout.Horizontal)
            {
                if (IsShowDataView)
                {
                    this.tableLayoutPanel1.Controls.Clear();
                    this.tableLayoutPanel1.ColumnCount = 2;
                    this.tableLayoutPanel1.ColumnStyles.Clear();
                    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
                    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                    this.tableLayoutPanel1.RowCount = 1;
                    this.tableLayoutPanel1.RowStyles.Clear();
                    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
                    if (CurrentViewType == ViewType.Chart)
                        this.tableLayoutPanel1.Controls.Add(this.chartData, 1, 0);
                    else if (CurrentViewType == ViewType.DataGrid)
                        this.tableLayoutPanel1.Controls.Add(this.dgvDataResultView, 1, 0);

                    // 如果不需要显示图表，则不进行显示
                    this.tableLayoutPanel2.Controls.Clear();
                    this.tableLayoutPanel2.ColumnCount = 1;
                    this.tableLayoutPanel2.ColumnStyles.Clear();
                    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                    this.tableLayoutPanel2.RowCount = 2;
                    this.tableLayoutPanel2.RowStyles.Clear();
                    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
                    this.tableLayoutPanel2.Controls.Add(this.cogRecordDisplay, 0, 0);
                    this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
                }
                else
                {
                    this.tableLayoutPanel1.Controls.Clear();
                    this.tableLayoutPanel1.ColumnCount = 1;
                    this.tableLayoutPanel1.ColumnStyles.Clear();
                    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                    this.tableLayoutPanel1.RowCount = 1;
                    this.tableLayoutPanel1.RowStyles.Clear();
                    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);

                    // 如果不需要显示图表，则不进行显示
                    this.tableLayoutPanel2.Controls.Clear();
                    this.tableLayoutPanel2.ColumnCount = 1;
                    this.tableLayoutPanel2.ColumnStyles.Clear();
                    this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                    this.tableLayoutPanel2.RowCount = 2;
                    this.tableLayoutPanel2.RowStyles.Clear();
                    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
                    this.tableLayoutPanel2.Controls.Add(this.cogRecordDisplay, 0, 0);
                    this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
                }
            }
        }

        /// <summary>
        /// 当布局发生变化时，ProcessingTime的label的位置需要变化，更新ProcessingTime的label
        /// </summary>
        public void UpdateProcessingTimeLabel()
        {
            // 由于布局发生了改变，需要更新ProcessingTime标签的显示位置
            // 先判断是否有Processing Time的标签
            if (this.cogRecordDisplay.StaticGraphics.ZOrderGroups.IndexOf(PROCESSINGTIME) != -1)
                this.cogRecordDisplay.StaticGraphics.Remove(PROCESSINGTIME);
            if (IsProcessingTimeLabelVisible)
                AddProcessingTimeGraphicLabelToCogDisplay(this.ImageProcessingTimeMilliSecond.ToString("0.00") + "ms ");
        }

        /// <summary>
        /// 清楚ProcessingTime的label
        /// </summary>
        public void ClearProcessingTimeLabel()
        {
            // 由于布局发生了改变，需要更新ProcessingTime标签的显示位置
            // 先判断是否有Processing Time的标签
            if (this.cogRecordDisplay.StaticGraphics.ZOrderGroups.IndexOf(PROCESSINGTIME) != -1)
                this.cogRecordDisplay.StaticGraphics.Remove(PROCESSINGTIME);
        }

        /// <summary>
        /// 更新pass,总运行数等
        /// </summary>
        public void UpdateStatisticData(int passCnt, int runCnt)
        {
            this.lblRunData.Text = string.Format("Total: {0} ({1}%, -{2})", 
                runCnt, 
                runCnt > 0 ? ((double)passCnt / runCnt * 100).ToString("0.00") : "-.-", 
                runCnt - passCnt);
        }

        /// <summary>
        /// 清除计数
        /// </summary>
        public void ClearStatisticData()
        {
            // 统计数据清零
            this.lblRunData.Text = "Total: --";

            // 历史纪录清空
            queueHistoryResult.Clear();
            // 清空历史纪录图片
            for (int i = 0; i < pictureBoxHistoryResult.Length; i++)
            {
                pictureBoxHistoryResult[i].Image = null;
            }
        }
        
        /// <summary>
        /// 清除图片显示
        /// </summary>
        public void ClearDisplayImage()
        {
            // 如果有OK NG的标签，清除掉
            if (this.cogRecordDisplay.StaticGraphics.ZOrderGroups.IndexOf(OKNGLABELGROUPNAME) != -1)
                this.cogRecordDisplay.StaticGraphics.Remove(OKNGLABELGROUPNAME);
            // 如果有失效信息的标签，清除掉
            if (this.cogRecordDisplay.StaticGraphics.ZOrderGroups.IndexOf(FAILUREINFOGROUPNAME) != -1)
                this.cogRecordDisplay.StaticGraphics.Remove(FAILUREINFOGROUPNAME);
            // 如果有图像处理时间的标签，清除掉
            if (this.cogRecordDisplay.StaticGraphics.ZOrderGroups.IndexOf(PROCESSINGTIME) != -1)
                this.cogRecordDisplay.StaticGraphics.Remove(PROCESSINGTIME);
            // 如果有其他交互的图形，清除掉
            this.cogRecordDisplay.InteractiveGraphics.Clear();
            // 清除Display
            this.cogRecordDisplay.Record = null;
            this.cogRecordDisplay.Image = null;
            // 当执行以上语句时，cogRecordDisplay默认会清除所有的GraphicLabel, 因此需要重新添加相机名字的Label
            if(labelCameraName != null)
                this.cogRecordDisplay.StaticGraphics.Add(labelCameraName, CAMERANAMELABELGROUPNAME);
        }

        /// <summary>
        /// 更新历史数据
        /// </summary>
        private void UpdateHistoryResult()
        {
            object[] obj = queueHistoryResult.ToArray();
            Array.Reverse(obj);
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] == null)
                {
                    pictureBoxHistoryResult[i].Image = null;
                }
                else if ((bool)obj[i])
                {
                    pictureBoxHistoryResult[i].Image = Properties.Resources.bullet_green;
                }
                else
                {
                    pictureBoxHistoryResult[i].Image = Properties.Resources.bullet_red;
                }
            }
        }

        /// <summary>
        /// 更新Chart图表数据（数据量比较小，且需要显示实时动态的情况下）
        /// </summary>
        private void UpdateChartData()
        {
            bool isHaveLowerLimit = false;
            bool isHaveUpperLimit = false;
            double lowerLimit = 0.0;
            double upperLimit = 0.0;

            try
            {
                if (!string.IsNullOrEmpty(FormMain.jobHelper[CCDIndex].ShowOutputDataName))
                {
                    for (int i = 0; i < ToolBlock.Outputs.Count; i++)
                    {
                        if (ToolBlock.Outputs[i].Name == FormMain.jobHelper[CCDIndex].ShowOutputDataName)
                        {
                            string[] allString = ToolBlock.Outputs[i].ValueType.ToString().Split('.');
                            string outputType = allString[allString.Length - 1].ToLower();
                            if (outputType == "int16" || outputType == "int32" || outputType == "double")
                            {
                                // 搜索ToolBlock的input，如果input terminal中包含Lower limit或者Upper limit
                                // 此时需要判断上下限得出result
                                for (int j = 0; j < ToolBlock.Inputs.Count; j++)
                                {
                                    if (ToolBlock.Inputs[j].Name.ToLower().Contains("lower") &&
                                        ToolBlock.Inputs[j].Name.ToLower().Contains(ToolBlock.Outputs[i].Name.ToLower()))
                                    {
                                        isHaveLowerLimit = true;
                                        lowerLimit = Convert.ToDouble(ToolBlock.Inputs[j].Value.ToString());
                                    }
                                    else if (ToolBlock.Inputs[j].Name.ToLower().Contains("upper") &&
                                         ToolBlock.Inputs[j].Name.ToLower().Contains(ToolBlock.Outputs[i].Name.ToLower()))
                                    {
                                        isHaveUpperLimit = true;
                                        upperLimit = Convert.ToDouble(ToolBlock.Inputs[j].Value.ToString());
                                    }
                                }

                                if (index >= CHARTDATANUM)
                                {
                                    index = CHARTDATANUM;
                                    yLowerLimit.Dequeue();
                                    yUpperLimit.Dequeue();
                                    yData.Dequeue();
                                }
                                index++;
                                yLowerLimit.Enqueue(lowerLimit);
                                yUpperLimit.Enqueue(upperLimit);
                                yData.Enqueue(Convert.ToDouble(ToolBlock.Outputs[i].Value.ToString()));

                                // 将数据绑定到图表中
                                chartData.Series[0].Points.DataBindY(yLowerLimit);
                                chartData.Series[1].Points.DataBindY(yUpperLimit);
                                chartData.Series[2].Points.DataBindY(yData);
                                // 根据是否有上下限判断是否进行显示
                                chartData.Series[0].Enabled = isHaveLowerLimit;
                                chartData.Series[1].Enabled = isHaveUpperLimit;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 更新表格内容（大量数据需要显示的情况下）
        /// </summary>
        private void UpdateDataGrid()
        {
            string dataTypeFull;
            string[] allString;
            string dataType;
            int rowIndex = 0;
            bool isHaveLowerLimit = true;
            bool isHaveUpperLimit = true;
            double lowerLimit = 0.0;
            double upperLimit = 0.0;
            bool result1, result2; // 判断上限的结果和判断下限的结果
            bool result = false;
            try
            {
                DataRow row;
                //清除DataTable的数据
                dtDataResultDetails.Clear();
                for (int i = 0; i < ToolBlock.Outputs.Count; i++)
                {
                    //获取output的数据类型
                    dataTypeFull = ToolBlock.Outputs[i].ValueType.ToString();
                    allString = dataTypeFull.Split('.');
                    dataType = allString[allString.Length - 1];
                    switch (dataType)
                    {
                        case "Double[]":
                            // double[]类型，添加多行数据
                            double[] data = (double[])ToolBlock.Outputs[i].Value;
                            isHaveLowerLimit = false;
                            isHaveUpperLimit = false;
                            // 搜索ToolBlock的input，如果input terminal中包含Lower limit或者Upper limit
                            // 此时需要判断上下限得出result
                            for (int j = 0; j < ToolBlock.Inputs.Count; j++)
                            {
                                if (ToolBlock.Inputs[j].Name.ToLower().Contains("lower") &&
                                    ToolBlock.Inputs[j].Name.ToLower().Contains(ToolBlock.Outputs[i].Name.ToLower()))
                                {
                                    isHaveLowerLimit = true;
                                    lowerLimit = (double)ToolBlock.Inputs[j].Value;
                                }
                                else if (ToolBlock.Inputs[j].Name.ToLower().Contains("upper") &&
                                     ToolBlock.Inputs[j].Name.ToLower().Contains(ToolBlock.Outputs[i].Name.ToLower()))
                                {
                                    isHaveUpperLimit = true;
                                    upperLimit = (double)ToolBlock.Inputs[j].Value;
                                }
                            }

                            // 添加每行的数据
                            for (int k = 0; k < data.Length; k++)
                            {
                                // 判断数据上限
                                if (isHaveLowerLimit) result1 = data[k] >= lowerLimit ? true : false;
                                else result1 = true;
                                // 判断数据下限
                                if (isHaveUpperLimit) result2 = data[k] <= upperLimit ? true : false;
                                else result2 = true;
                                result = result1 && result2;
                                row = dtDataResultDetails.NewRow();
                                row[0] = (rowIndex + 1).ToString();
                                row[1] = ToolBlock.Outputs[i].Name + "[" + k + "]";
                                row[2] = data[k].ToString("0.000");
                                row[3] = isHaveLowerLimit ? lowerLimit.ToString() : "N/A";
                                row[4] = isHaveUpperLimit ? upperLimit.ToString() : "N/A";
                                row[5] = result ? Properties.Resources.accept : Properties.Resources.reject;
                                dtDataResultDetails.Rows.Add(row);
                                rowIndex++;
                            }
                            break;
                        case "Double":
                            // double类型数据，添加单行

                            // 搜索ToolBlock的input，如果input terminal中包含Lower limit或者Upper limit
                            // 此时需要判断上下限得出result
                            for (int j = 0; j < ToolBlock.Inputs.Count; j++)
                            {
                                if (ToolBlock.Inputs[j].Name.ToLower().Contains("lower") &&
                                    ToolBlock.Inputs[j].Name.ToLower().Contains(ToolBlock.Outputs[i].Name.ToLower()))
                                {
                                    isHaveLowerLimit = true;
                                    lowerLimit = (double)ToolBlock.Inputs[j].Value;
                                }
                                else if (ToolBlock.Inputs[j].Name.ToLower().Contains("upper") &&
                                     ToolBlock.Inputs[j].Name.ToLower().Contains(ToolBlock.Outputs[i].Name.ToLower()))
                                {
                                    isHaveUpperLimit = true;
                                    upperLimit = (double)ToolBlock.Inputs[j].Value;
                                }
                            }

                            // 判断数据上限
                            if (isHaveLowerLimit) result1 = (double)ToolBlock.Outputs[i].Value >= lowerLimit ? true : false;
                            else result1 = true;
                            // 判断数据下限
                            if (isHaveUpperLimit) result2 = (double)ToolBlock.Outputs[i].Value <= upperLimit ? true : false;
                            else result2 = true;
                            result = result1 && result2;

                            row = dtDataResultDetails.NewRow();
                            row[0] = (rowIndex + 1).ToString();
                            row[1] = ToolBlock.Outputs[i].Name;
                            row[2] = ((double)ToolBlock.Outputs[i].Value).ToString("0.000");
                            row[3] = isHaveLowerLimit ? lowerLimit.ToString() : "N/A";
                            row[4] = isHaveUpperLimit ? upperLimit.ToString() : "N/A";
                            row[5] = result ? Properties.Resources.accept : Properties.Resources.reject;
                            dtDataResultDetails.Rows.Add(row);
                            rowIndex++;
                            break;
                        case "Int32":
                            // Int32类型数据，添加单行

                            // 搜索ToolBlock的input，如果input terminal中包含Lower limit或者Upper limit
                            // 此时需要判断上下限得出result
                            for (int j = 0; j < ToolBlock.Inputs.Count; j++)
                            {
                                if (ToolBlock.Inputs[j].Name.ToLower().Contains("lower") &&
                                    ToolBlock.Inputs[j].Name.ToLower().Contains(ToolBlock.Outputs[i].Name.ToLower()))
                                {
                                    isHaveLowerLimit = true;
                                    lowerLimit = (double)ToolBlock.Inputs[j].Value;
                                }
                                else if (ToolBlock.Inputs[j].Name.ToLower().Contains("upper") &&
                                     ToolBlock.Inputs[j].Name.ToLower().Contains(ToolBlock.Outputs[i].Name.ToLower()))
                                {
                                    isHaveUpperLimit = true;
                                    upperLimit = (double)ToolBlock.Inputs[j].Value;
                                }
                            }

                            // 判断数据上限
                            if (isHaveLowerLimit) result1 = (double)ToolBlock.Outputs[i].Value >= lowerLimit ? true : false;
                            else result1 = true;
                            // 判断数据下限
                            if (isHaveUpperLimit) result2 = (double)ToolBlock.Outputs[i].Value <= upperLimit ? true : false;
                            else result2 = true;
                            result = result1 && result2;

                            row = dtDataResultDetails.NewRow();
                            row[0] = (rowIndex + 1).ToString();
                            row[1] = ToolBlock.Outputs[i].Name;
                            row[2] = ((double)ToolBlock.Outputs[i].Value).ToString("0.000");
                            row[3] = isHaveLowerLimit ? lowerLimit.ToString() : "N/A";
                            row[4] = isHaveUpperLimit ? upperLimit.ToString() : "N/A";
                            row[5] = result ? Properties.Resources.accept : Properties.Resources.reject;
                            dtDataResultDetails.Rows.Add(row);
                            rowIndex++;
                            break;
                        case "Boolean":
                            // bool类型数据，添加单行
                            row = dtDataResultDetails.NewRow();
                            row[0] = (rowIndex + 1).ToString();
                            row[1] = ToolBlock.Outputs[i].Name;
                            row[2] = ToolBlock.Outputs[i].Value.ToString();
                            row[3] = "N/A";
                            row[4] = "N/A";
                            row[5] = (bool)ToolBlock.Outputs[i].Value ? Properties.Resources.accept : Properties.Resources.reject;
                            dtDataResultDetails.Rows.Add(row);
                            rowIndex++;
                            break;
                        case "String":
                            // string类型数据，添加单行
                            row = dtDataResultDetails.NewRow();
                            row[0] = (rowIndex + 1).ToString();
                            row[1] = ToolBlock.Outputs[i].Name;
                            row[2] = ToolBlock.Outputs[i].Value.ToString();
                            row[3] = "N/A";
                            row[4] = "N/A";
                            row[5] = Properties.Resources.accept;
                            dtDataResultDetails.Rows.Add(row);
                            rowIndex++;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 工具栏触发事件：录像模式，手动触发，导入图片，等...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_ItemClicked(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            switch (btn.Name)
            {
                case "toolStripBtnLive":
                    if (IsLive)
                    {
                        IsLive = false;
                        btn.Text = "实时模式";
                        btn.Image = Properties.Resources.video_mode;
                        UpdateControlEnabled();
                        RemoveLiveVideoLabelToCogDisplay(LIVEVIDEOLABELGROUPNAME);
                        _isFirstImageForLiveVideo = false;
                        bgwCCDLive.CancelAsync();
                        // 延时一段时间关闭光源
                        Thread.Sleep(20);
                        TurnOffLight?.Invoke(this);
                        ControlStatus = CogDisplayCtrlStatus.Available;
                    }
                    else
                    {
                        IsLive = true;
                        btn.Text = "停止实时模式";
                        btn.Image = Properties.Resources.image_mode;
                        UpdateControlEnabled();
                        AddLiveVideoLabelToCogDisplay(LIVEVIDEOLABELGROUPNAME, 20, 0);
                        _isFirstImageForLiveVideo = true;
                        // 打开光源
                        TurnOnLight?.Invoke(this);
                        bgwCCDLive.RunWorkerAsync();
                        ControlStatus = CogDisplayCtrlStatus.LiveVideo;
                    }
                    break;
                case "toolStripBtnImportImage":
                    string imagePath;
                    CogImageFileTool OpenFileImage = new CogImageFileTool();
                    // string[] Filelist_CCDImage;
                    OpenFileDialog OpenImageDialog = new OpenFileDialog();
                    OpenImageDialog.Filter = "所有图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp|BMP File|*.bmp|JPG File|*.jpg;*.jpeg|PNG File|*.png";
                    OpenImageDialog.RestoreDirectory = true;
                    if (OpenImageDialog.ShowDialog() == DialogResult.OK)
                    {
                        imagePath = OpenImageDialog.FileName;
                        // Filelist_CCDImage = Directory.GetFiles(Directory.GetParent(imagePath).FullName);

                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            FileInfo cf1 = new FileInfo(imagePath);
                            if (cf1.Extension.ToLower() != ".bmp" && cf1.Extension.ToLower() != ".jpg")
                            {
                                MessageBox.Show("Image format Not bmp or jpg file!");
                                return;
                            }
                            OpenFileImage.Operator.Open(imagePath, CogImageFileModeConstants.Read);
                            OpenFileImage.Run();
                            this.CogOriginalImage = OpenFileImage.OutputImage;
                            this.ToolBlock.Inputs["InputImage"].Value = this.CogOriginalImage;
                            this.ToolBlock.Run();
                            if (ToolBlock.CreateLastRunRecord().SubRecords.Count == 0) return;
                            this.CogRecord = ToolBlock.CreateLastRunRecord().SubRecords[LastRunRecordIndex];
                            this.ImageWidth = CogOriginalImage.Width;
                            this.ImageHeight = CogOriginalImage.Height;
                            this.ImageProcessingTimeMilliSecond = ToolBlock.RunStatus.TotalTime;
                            // this.CCDRunResult = ToolBlock.RunStatus.Result == CogToolResultConstants.Accept;
                            // UpdateImageProcessingTime(this.ImageProcessingTimeMilliSecond);
                            // 委托其他界面或控件进行更新
                            JobResultAvailable?.Invoke(this);
                            // 添加图像到窗口
                            UpdateCogDisplayRecord(this.CogRecord);
                            // 添加OK/NG信息到窗口
                            AddOKNGGraphicLabelToCogDisplay(OKNGLABELGROUPNAME, ImageWidth / 2, 0);
                            // 添加失效信息到窗口
                            AddFailureInfoLabelToCogDisplay(GetFailureInfoHashTable(ToolRunResult), FAILUREINFOGROUPNAME, 0, ImageHeight);
                            // 添加图像捕获/处理时间到窗口
                            AddProcessingTimeGraphicLabelToCogDisplay(this.ImageProcessingTimeMilliSecond.ToString("0.00") + "ms ");
                            // 如果历史记录条数超过了容量，删除队列最头部的一项
                            while (queueHistoryResult.Count >= HISTORYRECORDNUM)
                                queueHistoryResult.Dequeue();
                            // 添加结果到历史数据记录中
                            queueHistoryResult.Enqueue(JobRunResult);
                            // 更新历史记录
                            UpdateHistoryResult();
                            // 更新图表
                            UpdateChartData();
                            // 更新DataGridView表格
                            UpdateDataGrid();
                        }
                    }
                    break;
                case "toolStripBtnManualTrigger":
                    try
                    {
                        // 打开光源
                        TurnOnLight?.Invoke(this);
                        // 延时一段时间，等待光源打开之后开始取像
                        Thread.Sleep(INTERVALBETWEENLIGHTONANDTRIGGER);
                        AcqFifoTool.Run();
                        CogOriginalImage = AcqFifoTool.OutputImage;
                        if (this.CogOriginalImage != null)
                        {
                            this.ToolBlock.Inputs["InputImage"].Value = this.CogOriginalImage;
                            this.ImageWidth = CogOriginalImage.Width;
                            this.ImageHeight = CogOriginalImage.Height;
                            this.ToolBlock.Run();
                            this.CogRecord = ToolBlock.CreateLastRunRecord().SubRecords[LastRunRecordIndex];
                            this.ImageProcessingTimeMilliSecond = AcqFifoTool.RunStatus.TotalTime + ToolBlock.RunStatus.TotalTime;
                            // this.CCDRunResult = ToolBlock.RunStatus.Result == CogToolResultConstants.Accept;
                            // 委托其他界面或控件进行更新
                            JobResultAvailable?.Invoke(this);
                            // 添加图像到窗口
                            UpdateCogDisplayRecord(this.CogRecord);
                            // 添加OK/NG信息到窗口
                            AddOKNGGraphicLabelToCogDisplay(OKNGLABELGROUPNAME, ImageWidth / 2, 0);
                            // 添加失效信息到窗口
                            AddFailureInfoLabelToCogDisplay(GetFailureInfoHashTable(ToolRunResult), FAILUREINFOGROUPNAME, 0, ImageHeight);
                            // 添加图像捕获/处理时间到窗口
                            AddProcessingTimeGraphicLabelToCogDisplay(this.ImageProcessingTimeMilliSecond.ToString("0.00") + "ms ");
                            // 如果历史记录条数超过了容量，删除队列最头部的一项
                            while (queueHistoryResult.Count >= HISTORYRECORDNUM)
                                queueHistoryResult.Dequeue();
                            // 添加结果到历史数据记录中
                            queueHistoryResult.Enqueue(JobRunResult);
                            // 更新历史记录
                            UpdateHistoryResult();
                        }
                        // 关闭光源
                        TurnOffLight?.Invoke(this);
                    }
                    catch (Exception)
                    {

                    }
                    break;
                case "toolStripBtnSaveImage":
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    //打开的文件选择对话框上的标题  
                    saveFileDialog.Title = "请选择文件";
                    //设置文件类型  
                    saveFileDialog.Filter = "图像文件(*.bmp)|*.bmp|图像文件(*.jpg)|*.jpg";
                    //设置默认文件类型显示顺序  
                    saveFileDialog.FilterIndex = 1;
                    //保存对话框是否记忆上次打开的目录  
                    saveFileDialog.RestoreDirectory = true;
                    //检查目录
                    saveFileDialog.CheckPathExists = true;
                    //按下确定选择的按钮  
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //获得文件路径  
                        string strSaveFileLocation = saveFileDialog.FileName;
                        //获取文件路径，不带文件名  
                        //FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));  
                        //获取文件名，带后缀名，不带路径  
                        //string fileNameWithSuffix = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);
                        //去除文件后缀名  
                        //string fileNameWithoutSuffix = fileNameWithSuffix.Substring(0, fileNameWithSuffix.LastIndexOf("."));
                        //在文件名前加上时间  
                        //string fileNameWithTime = DateTime.Now.ToString("yyyy-MM-dd ") + fileNameExt;
                        //在文件名里加字符  
                        //string newFileName = localFilePath.Insert(1, "Tets");
                        CogImageFileTool mImageFileTool = new CogImageFileTool();
                        if (CogOriginalImage != null)
                        {
                            mImageFileTool.InputImage = CogOriginalImage;
                            mImageFileTool.Operator.Open(strSaveFileLocation, CogImageFileModeConstants.Write);
                            mImageFileTool.Run();
                        }
                    }
                    break;
                case "toolStripBtnShowChart":
                    if (IsShowDataView) IsShowDataView = false;
                    else IsShowDataView = true;
                    UpdateViewLayout();
                    //SetDataViewVisible(this.IsShowDataView);
                    break;
                case "toolStripBtnTest":
                    if (index >= CHARTDATANUM)
                    {
                        index = CHARTDATANUM;
                        yLowerLimit.Dequeue();
                        yUpperLimit.Dequeue();
                        yData.Dequeue();
                    }
                    index++;
                    yLowerLimit.Enqueue(200);
                    yUpperLimit.Enqueue(800);
                    yData.Enqueue(random.Next(400, 600));
                    chartData.Series[0].Points.DataBindY(yLowerLimit);
                    chartData.Series[1].Points.DataBindY(yUpperLimit);
                    chartData.Series[2].Points.DataBindY(yData);
                    break;
                case "toolStripBtnVSplit":
                    CurrentViewLayout = ViewLayout.Vertical;
                    UpdateViewLayout();
                    break;
                case "toolStripBtnHSplit":
                    CurrentViewLayout = ViewLayout.Horizontal;
                    UpdateViewLayout();
                    break;
                case "toolStripBtnParamSetting":
                    //Form form = new FormCameraParam();
                    //form.ShowDialog();
                    FormParamSetting form = new FormParamSetting(ToolBlock, CCDIndex, LastRunRecordIndex);
                    form.ShowDialog();
                    LastRunRecordIndex = form.SelectRecordIndex;
                    break;
                case "toolStripBtnReset":
                    ResetDisplay?.Invoke(this);
                    // 将数据绑定到图表中
                    yLowerLimit.Clear();
                    yUpperLimit.Clear();
                    yData.Clear();
                    chartData.Series[0].Points.Clear();
                    chartData.Series[1].Points.Clear();
                    chartData.Series[2].Points.Clear();
                    UpdateChartData();
                    break;
                case "toolStripBtnInfo":
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 更改tool bar的显示状态，当程序处于离线状态时不可见
        /// </summary>
        /// <param name="visible"></param>
        public void SetToolBarVisible(bool visible)
        {
            this.toolStrip.Visible = visible;
        }

        /// <summary>
        /// 更改Data Chart的显示状态，设置控件是否显示
        /// </summary>
        /// <param name="visible"></param>
        private void SetDataViewVisible(bool visible)
        {
            this.tableLayoutPanel2.Controls.Clear();
            if (visible)
            {
                this.tableLayoutPanel2.ColumnCount = 1;
                this.tableLayoutPanel2.ColumnStyles.Clear();
                this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                this.tableLayoutPanel2.RowCount = 3;
                this.tableLayoutPanel2.RowStyles.Clear();
                this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
                this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
                this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
                this.tableLayoutPanel2.Controls.Add(this.cogRecordDisplay, 0, 0);
                this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
                if (CurrentViewType == ViewType.Chart)
                    this.tableLayoutPanel2.Controls.Add(this.chartData, 0, 2);
                else if (CurrentViewType == ViewType.DataGrid)
                    this.tableLayoutPanel2.Controls.Add(this.dgvDataResultView, 0, 2);
            }
            else
            {
                this.tableLayoutPanel2.ColumnCount = 1;
                this.tableLayoutPanel2.ColumnStyles.Clear();
                this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                this.tableLayoutPanel2.RowCount = 2;
                this.tableLayoutPanel2.RowStyles.Clear();
                this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
                this.tableLayoutPanel2.Controls.Add(this.cogRecordDisplay, 0, 0);
                this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            }
        }

        /// <summary>
        /// 如果有外部有结果时，更新CogDisplayCtrl界面
        /// </summary>
        /// <param name="image"></param>
        /// <param name="runResult"></param>
        public void UpdateUIByExternalTrigger(ICogImage image, bool runResult)
        {
            try
            {
                CogOriginalImage = image;
                if (this.CogOriginalImage != null)
                {
                    this.ImageWidth = CogOriginalImage.Width;
                    this.ImageHeight = CogOriginalImage.Height;
                    this.JobRunResult = runResult;
                    //添加最新的record到界面中
                    UpdateCogDisplayImage(image);
                    //添加OK，NG结果到界面中
                    AddOKNGGraphicLabelToCogDisplay(OKNGLABELGROUPNAME, ImageWidth / 2, 0);
                    //添加失效信息到界面中
                    AddFailureInfoLabelToCogDisplay(GetFailureInfoHashTable(ToolRunResult), FAILUREINFOGROUPNAME, 0, ImageHeight);
                    // 如果历史记录条数超过了容量，删除队列最头部的一项
                    while (queueHistoryResult.Count >= HISTORYRECORDNUM)
                        queueHistoryResult.Dequeue();
                    // 添加结果到历史数据记录中
                    queueHistoryResult.Enqueue(JobRunResult);
                    // 更新历史记录
                    UpdateHistoryResult();
                    // 更新图表
                    UpdateChartData();
                    // 更新DataGridView表格
                    UpdateDataGrid();
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 如果有外部有结果时，更新CogDisplayCtrl界面， 传入参数image为了获取图像的大小和图形位置
        /// </summary>
        /// <param name="image"></param>
        /// <param name="record"></param>
        /// <param name="runResult"></param>
        public void UpdateUIByExternalTrigger(ICogImage image, ICogRecords records, string processingTime, bool runResult)
        {
            try
            {
                CogOriginalImage = image;
                if (image !=null && records != null)
                {
                    this.ImageWidth = CogOriginalImage.Width;
                    this.ImageHeight = CogOriginalImage.Height;
                    this.JobRunResult = runResult;
                    //添加最新的record到界面中
                    UpdateCogDisplayRecords(records);
                    //添加OK，NG结果到界面中
                    AddOKNGGraphicLabelToCogDisplay(OKNGLABELGROUPNAME, ImageWidth / 2, 0);
                    //添加失效信息到界面中
                    AddFailureInfoLabelToCogDisplay(GetFailureInfoHashTable(ToolRunResult), FAILUREINFOGROUPNAME, 0, ImageHeight);
                    AddProcessingTimeGraphicLabelToCogDisplay(processingTime);
                    // 如果历史记录条数超过了容量，删除队列最头部的一项
                    while (queueHistoryResult.Count >= HISTORYRECORDNUM)
                        queueHistoryResult.Dequeue();
                    // 添加结果到历史数据记录中
                    queueHistoryResult.Enqueue(JobRunResult);
                    // 更新历史记录
                    UpdateHistoryResult();
                    // 更新图表
                    UpdateChartData();
                    // 更新DataGridView表格
                    UpdateDataGrid();
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 更新cogRecordDisplay中的图片
        /// </summary>
        /// <param name="image"></param>
        public void UpdateCogDisplayImage(ICogImage image)
        {
            this.cogRecordDisplay.InteractiveGraphics.Clear();
            this.cogRecordDisplay.Image = image;
            // 当执行以上语句时，cogRecordDisplay默认会清除所有的GraphicLabel, 因此需要重新添加相机名字的Label
            this.cogRecordDisplay.StaticGraphics.Add(labelCameraName, CAMERANAMELABELGROUPNAME);
            this.cogRecordDisplay.Fit(true);
        }

        /// <summary>
        /// 更新cogrecordDisplay中的结果记录，以集合的形式
        /// </summary>
        /// <param name="records"></param>
        public void UpdateCogDisplayRecords(ICogRecords records)
        {
            this.cogRecordDisplay.InteractiveGraphics.Clear();
            this.cogRecordDisplay.Record = records[LastRunRecordIndex];
            // 当执行以上语句时，cogRecordDisplay默认会清除所有的GraphicLabel, 因此需要重新添加相机名字的Label
            this.cogRecordDisplay.StaticGraphics.Add(labelCameraName, CAMERANAMELABELGROUPNAME);
            this.cogRecordDisplay.Fit(true);
        }

        /// <summary>
        /// 更新cogrecordDisplay中的结果记录
        /// </summary>
        /// <param name="record"></param>
        public void UpdateCogDisplayRecord(ICogRecord record)
        {
            this.cogRecordDisplay.InteractiveGraphics.Clear();
            this.cogRecordDisplay.Record = record;
            // 当执行以上语句时，cogRecordDisplay默认会清除所有的GraphicLabel, 因此需要重新添加相机名字的Label
            this.cogRecordDisplay.StaticGraphics.Add(labelCameraName, CAMERANAMELABELGROUPNAME);
            this.cogRecordDisplay.Fit(true);
        }

        /// <summary>
        /// 更新图片处理所花的总时间
        /// </summary>
        /// <param name="time"></param>
        public void UpdateImageProcessingTime(double time)
        {
            // this.toolStripLabelProcessingTime.Text = this.ImageProcessingTimeMilliSecond.ToString("0.00") + "ms";
        }

        /// <summary>
        /// 生成失效模式的Dictionary，失效名称和对应的是小结果OK或者NG
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, bool> GetFailureInfoHashTable(bool[] toolResult)
        {
            Dictionary<string, bool> diction = new Dictionary<string, bool>();
            // 添加每个失效模式的结果
            for (int i = 0; i < this.ToolBlock.Tools.Count; i++)
            {
                CogToolBlock tool = (CogToolBlock)this.ToolBlock.Tools[i];
                diction.Add(tool.Name + ": " + (toolResult[i] ? "OK" : "NG"),
                    toolResult[i] ? true : false);
            }
            // 添加总ToolBlock的output数据
            for (int i = 0; i < this.ToolBlock.Outputs.Count; i++)
            {
                string inputFullType;
                string[] allString;
                string inputType;
                inputFullType = ToolBlock.Outputs[i].ValueType.ToString();
                allString = inputFullType.Split('.');
                inputType = allString[allString.Length - 1];
                switch (inputType.ToLower())
                {
                    case "int16":
                        diction.Add(this.ToolBlock.Outputs[i].Name + ": " + 
                            this.ToolBlock.Outputs[i].Value.ToString(), true);
                        break;
                    case "int32":
                        diction.Add(this.ToolBlock.Outputs[i].Name + ": " +
                            this.ToolBlock.Outputs[i].Value.ToString(), true);
                        break;
                    case "double":
                        diction.Add(this.ToolBlock.Outputs[i].Name + ": " +
                            Convert.ToDouble(this.ToolBlock.Outputs[i].Value.ToString()).ToString("F4"), true);
                        break;
                    case "string":
                        diction.Add(this.ToolBlock.Outputs[i].Name + ": " +
                            this.ToolBlock.Outputs[i].Value.ToString(), true);
                        break;
                    default:
                        break;
                }
            }
            return diction;
        }

        /// <summary>
        /// 添加备注信息到cogRecordDisplay
        /// </summary>
        /// <param name="label"></param>
        public void AddFailureInfoLabelToCogDisplay(Dictionary<string, bool> textInfo, string groupname, double x, double y)
        {
            CogGraphicLabel[] label = ConfigFailureInfoCogGraphicLabel(textInfo, x, y);
            foreach (CogGraphicLabel item in label)
            {
                this.cogRecordDisplay.StaticGraphics.Add(item, groupname);
            }
        }

        /// <summary>
        /// 获取备注信息的CogGraphicLabel对象
        /// </summary>
        /// <param name="x">文本信息的原点位置X坐标</param>
        /// <param name="y">文本信息的原点位置Y坐标</param>
        /// <returns></returns>
        private CogGraphicLabel[] ConfigFailureInfoCogGraphicLabel(Dictionary<string, bool> text, double x, double y)
        {
            CogGraphicLabel[] info = new CogGraphicLabel[text.Count];
            int i = 0;
            double tempX = x, tempY = y;
            // 遍历哈希表中的每个元素
            foreach (var item in text)
            {
                info[i] = new CogGraphicLabel();
                info[i].SelectedSpaceName = "#";
                info[i].SetXYText(tempX, tempY, "");
                info[i].Font = new Font("Microsoft YaHei", FONTSIZEFAILUREINFO, FontStyle.Regular);
                info[i].Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                info[i].BackgroundColor = FAILUREINFOBACKGROUNDCOLOR;
                if (item.Value == true) // true means OK
                {
                    info[i].Text = item.Key.ToString();
                    info[i].Color = CogColorConstants.Green;
                }
                else if (item.Value == false) // false means NG
                {
                    info[i].Text = item.Key.ToString();
                    info[i].Color = CogColorConstants.Red;
                }
                ++i;
                tempY -= FAILUREINFOLINEGAP;
            }
            return info;
        }

        /// <summary>
        /// 添加备注信息到cogRecordDisplay
        /// </summary>
        /// <param name="label"></param>
        public void AddGraphicLabelToCogDisplay(CogGraphicLabel label, string groupname)
        {
            this.cogRecordDisplay.StaticGraphics.Add(label, groupname);
        }

        /// <summary>
        /// 添加相机的名字标签到CogDisplay中
        /// </summary>
        /// <param name="index"></param>
        public void AddCameraNameLabelToCogDisplay(int index)
        {
            labelCameraName = ConfigCameraNameCogGraphicLabel(index, 0, 0);
            this.cogRecordDisplay.StaticGraphics.Add(labelCameraName, CAMERANAMELABELGROUPNAME);
        }

        /// <summary>
        /// 添加GraphicLabel到制定坐标位置
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddOKNGGraphicLabelToCogDisplay(string groupname, double x, double y)
        {

            this.cogRecordDisplay.StaticGraphics.Add(ConfigOKNGCogGraphicLabel(x, y), groupname);
        }

        /// <summary>
        /// 获取结果的CogGraphicLabel对象
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private CogGraphicLabel ConfigOKNGCogGraphicLabel(double x, double y)
        {
            CogGraphicLabel total_Result = new CogGraphicLabel();

            total_Result.SelectedSpaceName = "#";
            total_Result.SetXYText(x, y, "");
            total_Result.Font = new Font("Microsoft YaHei", FONTSIZEOKNG, FontStyle.Bold);
            total_Result.Alignment = CogGraphicLabelAlignmentConstants.TopCenter;
            if (this.JobRunResult)
            {
                total_Result.Text = "OK";
                total_Result.Color = CogColorConstants.Green;
            }
            else
            {
                total_Result.Text = "NG";
                total_Result.Color = CogColorConstants.Red;
            }
            return total_Result;
        }

        /// <summary>
        /// 添加GraphicLabel到制定坐标位置
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddProcessingTimeGraphicLabelToCogDisplay(string textMillSecond)
        {
            this.cogRecordDisplay.StaticGraphics.Add(ConfigProcessingTimeCogGraphicLabel(textMillSecond), PROCESSINGTIME);
        }

        /// <summary>
        /// 获取结果的CogGraphicLabel对象
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private CogGraphicLabel ConfigProcessingTimeCogGraphicLabel(string textMillSecond)
        {
            CogGraphicLabel label_ProcessingTime = new CogGraphicLabel();

            label_ProcessingTime.SelectedSpaceName = "*";
            label_ProcessingTime.SetXYText(this.cogRecordDisplay.Width, 0, "");
            label_ProcessingTime.Font = new Font("Microsoft YaHei", FONTSIZEPROCESSINGTIME, FontStyle.Bold);
            label_ProcessingTime.Alignment = CogGraphicLabelAlignmentConstants.TopRight;
            label_ProcessingTime.BackgroundColor = PROCESSINGTIMELABELBACKGROUNDCOLOR;
            label_ProcessingTime.Text = textMillSecond;
            label_ProcessingTime.Color = CogColorConstants.Black;

            return label_ProcessingTime;
        }

        /// <summary>
        /// 添加实时显示模式的GraphicLabel到制定坐标位置
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddLiveVideoLabelToCogDisplay(string groupname, double x, double y)
        {
            // 如果有OK NG的标签，清除掉
            if (this.cogRecordDisplay.StaticGraphics.ZOrderGroups.IndexOf(OKNGLABELGROUPNAME) != -1)
                this.cogRecordDisplay.StaticGraphics.Remove(OKNGLABELGROUPNAME);
            // 如果有失效信息的标签，清除掉
            if (this.cogRecordDisplay.StaticGraphics.ZOrderGroups.IndexOf(FAILUREINFOGROUPNAME) != -1)
                this.cogRecordDisplay.StaticGraphics.Remove(FAILUREINFOGROUPNAME);
            // 如果有图像处理时间的标签，清除掉
            if (this.cogRecordDisplay.StaticGraphics.ZOrderGroups.IndexOf(PROCESSINGTIME) != -1)
                this.cogRecordDisplay.StaticGraphics.Remove(PROCESSINGTIME);
            // 如果有其他交互的图形，清除掉
            this.cogRecordDisplay.InteractiveGraphics.Clear();
            // 添加Live Video的标签到CogDisplay中
            this.cogRecordDisplay.StaticGraphics.Add(ConfigLiveVideoCogGraphicLabel(x, y), groupname);
        }

        /// <summary>
        /// 清除实时在线Label
        /// </summary>
        /// <param name="groupname"></param>
        public void RemoveLiveVideoLabelToCogDisplay(string groupname)
        {
            // 如果有相机在线模式的标签，清除掉
            if (this.cogRecordDisplay.StaticGraphics.ZOrderGroups.IndexOf(LIVEVIDEOLABELGROUPNAME) != -1)
                this.cogRecordDisplay.StaticGraphics.Remove(LIVEVIDEOLABELGROUPNAME);
        }
        /// <summary>
        /// 获取实时显示模式的的CogGraphicLabel对象
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private CogGraphicLabel ConfigLiveVideoCogGraphicLabel(double x, double y)
        {
            CogGraphicLabel label_LiveVideo = new CogGraphicLabel();

            label_LiveVideo.SelectedSpaceName = "#";
            label_LiveVideo.SetXYText(x, y, "");
            label_LiveVideo.Font = new Font("Microsoft YaHei", FONTSIZELIVEVIDEO, FontStyle.Bold);
            label_LiveVideo.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;

            label_LiveVideo.Text = "实时显示中...   ";
            label_LiveVideo.Color = CogColorConstants.Red;

            return label_LiveVideo;
        }

        /// <summary>
        /// 获取实时显示模式的的CogGraphicLabel对象
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private CogGraphicLabel ConfigCameraNameCogGraphicLabel(int index, double x, double y)
        {
            CogGraphicLabel label_CameraName = new CogGraphicLabel();
            label_CameraName.SelectedSpaceName = "*";
            label_CameraName.SetXYText(x, y, "");
            label_CameraName.Font = new Font("Microsoft YaHei", FONTSIZECAMERANAME, FontStyle.Bold);
            label_CameraName.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
            label_CameraName.BackgroundColor = CAMERANAMELABELBACKGROUNDCOLOR;
            label_CameraName.Text = FormMain.jobHelper[index].CcdName;
            label_CameraName.Color = CogColorConstants.Black;

            return label_CameraName;
        }

        /// <summary>
        /// 如果有相机处于在线模式，停止在线模式
        /// </summary>
        public void StopLiveVideoMode()
        {
            if (!IsLive) return;
            else
            {
                IsLive = false;
                toolStripBtnLive.Text = "实时模式";
                toolStripBtnLive.Image = Properties.Resources.video_mode;
                UpdateControlEnabled();
                RemoveLiveVideoLabelToCogDisplay(LIVEVIDEOLABELGROUPNAME);
                bgwCCDLive.CancelAsync();
                ControlStatus = CogDisplayCtrlStatus.Available;
                bgwCCDLive.CancelAsync();
            }
        }

        /// <summary>
        /// 获取实时模式中的一张图片
        /// </summary>
        /// <returns></returns>
        private ICogImage GetliveImage()
        {
            Thread.Sleep(10);
            AcqFifoTool.Run();
            return (ICogImage)AcqFifoTool.OutputImage;
        }

        /// <summary>
        /// 后台线程每隔一段时间采集一张图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_Live_CCD_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                Thread.Sleep(50);
                try { this.CogOriginalImage = GetliveImage(); }
                catch
                {
                }
                bgwCCDLive.ReportProgress(5);
                _liveVideoCnt++;
                if (_liveVideoCnt > 10) _liveVideoCnt = 0;
            } while (!bgwCCDLive.CancellationPending);
        }

        /// <summary>
        /// 将采集的图像更新在界面上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_Live_CCD_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                AcqFifoTool.Operator.Flush();
                UpdateCogDisplayLive(this.CogOriginalImage);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 结束实时显示模式时，清楚图像中的Label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgW_Live_CCD_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.cogRecordDisplay.StaticGraphics.Clear();
            //this.cogRecordDisplay.InteractiveGraphics.Clear();
            this.cogRecordDisplay.Fit(true);
        }

        /// <summary>
        /// Live模式下更新cogRecordDisplay中的图片
        /// </summary>
        /// <param name="image"></param>
        public void UpdateCogDisplayLive(ICogImage image)
        {
            //this.cogRecordDisplay.StaticGraphics.Clear();
            //this.cogRecordDisplay.InteractiveGraphics.Clear();
            this.cogRecordDisplay.Image = image;
            // 不需要自动适应屏幕大小，允许在实时模式下放大或缩小图像
            // 以达到最佳的显示效果（有时候需要观察图像细节）
            if (IsFirstImageForLiveVideo)
            {
                this.cogRecordDisplay.Fit(true);
                IsFirstImageForLiveVideo = false;
            }
        }

        /// <summary>
        /// 更新按钮的可用状态
        /// </summary>
        private void UpdateControlEnabled()
        {
            if (this.IsLive || this.IsRunning)
            {
                this.toolStripBtnParamSetting.Enabled = false;
                this.toolStripBtnImportImage.Enabled = false;
                this.toolStripBtnManualTrigger.Enabled = false;
                this.toolStripBtnSaveImage.Enabled = false;
            }
            else
            {
                this.toolStripBtnParamSetting.Enabled = true;
                this.toolStripBtnImportImage.Enabled = true;
                this.toolStripBtnManualTrigger.Enabled = true;
                this.toolStripBtnSaveImage.Enabled = true;
            }
        }

        /// <summary>
        /// 处理Display控件鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cogRecordDisplay_DoubleClick(object sender, EventArgs e)
        {
            // 委托主界面进行更新
            UpdateHomePageLayout?.Invoke(this);
        }
    }
}
