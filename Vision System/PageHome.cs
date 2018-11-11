using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Vision_System
{
    public delegate void ChangePartNumberEventHandler(object sender, int index);

    public partial class PageHome : UserControl
    {
        private CogDisplayCtl[] cogDisplayCtl;
        private CogToolBlock[] _cogToolBlock;
        private DataTable dtResultDetails = new DataTable("result_table");
        private bool isPartNoComboBoxEnabled = false;

        public event ChangePartNumberEventHandler ChangePartNumber = null;

        public int comboSelectedIndex { get; set; }
        public DataTable GetDataTable { get => dtResultDetails; set => dtResultDetails = value; }
        public CogToolBlock[] ToolBlock { get => _cogToolBlock; set => _cogToolBlock = value; }
        public bool IsPartNoComboBoxEnabled { get => isPartNoComboBoxEnabled; set => isPartNoComboBoxEnabled = value; }

        public PageHome()
        {
            InitializeComponent();
        }

        private void PageHome_Load(object sender, System.EventArgs e)
        {
            InitializeDataGridView();
            InitializeDisplayView();
            InitializeLayout();
            SetLanguage(FormMain.settingHelper.SoftwareLanguage);
            // 根据登录状态决定是否可以在界面中操作料号切换
            this.comboBox_ProductName.Enabled = IsPartNoComboBoxEnabled;
        }

        /// <summary>
        /// 设置界面语言
        /// </summary>
        /// <param name="language"></param>
        private void SetLanguage(LanguageType language)
        {
            switch (language)
            {
                case LanguageType.Chinese:
                    this.btnClearStatistics.Text = "计数复位";
                    this.btnClearDisplayImage.Text = "清除图像";
                    this.tabPage1.Text = "检测工具结果";
                    this.tabPage2.Text = "Log 信息";
                    this.groupBox1.Text = "料号";
                    break;
                case LanguageType.English:
                    this.btnClearStatistics.Text = "Reset Statistics";
                    this.btnClearDisplayImage.Text = "Reset Display";
                    this.tabPage1.Text = "Tool Result";
                    this.tabPage2.Text = "Log Message";
                    this.groupBox1.Text = "Part No.";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 设置料号选择框是否使能
        /// </summary>
        /// <param name="enable"></param>
        public void SetPartNoComboBoxEnabled(bool enable)
        {
            IsPartNoComboBoxEnabled = enable;
            this.comboBox_ProductName.Enabled = IsPartNoComboBoxEnabled;
        }

        /// <summary>
        /// 设置料号选择框是否使能
        /// </summary>
        /// <param name="enable"></param>
        public void SetCogDisplayCtlSettingButtonEnabled(bool enable)
        {
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                if (cogDisplayCtl[i] != null)
                {
                    cogDisplayCtl[i].SetParamSettingButtonEnabled(enable);
                }
            }
        }

        /// <summary>
        /// 窗口大小变化时，更新界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageHome_Resize(object sender, EventArgs e)
        {
            // 更新界面
            this.Refresh();

            for (int i = 0; i < FormMain.camNumber; i++)
                this.cogDisplayCtl[i].UpdateProcessingTimeLabel();
        }

        private void InitializeDisplayView()
        {
            // 初始化CogDisplayCtrl的布局属性
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                cogDisplayCtl[i].Dock = System.Windows.Forms.DockStyle.Fill;
                cogDisplayCtl[i].Name = "cogDisplayCtl" + (i + 1);
                cogDisplayCtl[i].AddCameraNameLabelToCogDisplay(i);
                cogDisplayCtl[i].JobResultAvailable += new JobResultAvailableEventHandler(JobResultAvailable);
                cogDisplayCtl[i].UpdateHomePageLayout += new UpdateHomePageLayoutEventHandler(UpdateLayoutDisplayDoubleClick);
                cogDisplayCtl[i].ResetDisplay += new ResetDisplayEventHandler(ResetDisplay);
                cogDisplayCtl[i].TurnOnLight += PageHome_TurnOnLight;
                cogDisplayCtl[i].TurnOffLight += PageHome_TurnOffLight;
            }
        }

        private void PageHome_TurnOffLight(object sender)
        {
            // 判断是哪个display触发的事件
            CogDisplayCtl control = sender as CogDisplayCtl;
            int index = Convert.ToInt16(control.Name.Substring(control.Name.Length - 1, 1)) - 1;
            if (FormMain.settingHelper.IsIODeviceEnabled && FormMain.jobHelper[index].IsLightFlash)
                FormMain.iOHelper.TurnOffLight(index);
        }

        private void PageHome_TurnOnLight(object sender)
        {
            // 判断是哪个display触发的事件
            CogDisplayCtl control = sender as CogDisplayCtl;
            int index = Convert.ToInt16(control.Name.Substring(control.Name.Length - 1, 1)) - 1;
            if (FormMain.settingHelper.IsIODeviceEnabled && FormMain.jobHelper[index].IsLightFlash)
                FormMain.iOHelper.TurnOnLight(index);
        }

        private void UpdateLayoutDisplayDoubleClick(object sender)
        {
            // 只有当相机数量大于等于4时，才对布局重新排列，否则不需要
            if (FormMain.camNumber >= 4)
            {
                // 判断是哪个display触发的事件
                CogDisplayCtl control = sender as CogDisplayCtl;
                int index = Convert.ToInt16(control.Name.Substring(control.Name.Length - 1, 1)) - 1;
                UpdateCusotmizedLayout(index);
            }
        }

        private void InitializeLayout()
        {
            this.tableLayoutPanelMain.Controls.Clear();
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Clear();
            this.tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowCount = 1;
            this.tableLayoutPanelMain.RowStyles.Clear();
            this.tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(tableLayoutPanel1, 0, 0);

            // 如果相机数量大于2，排成两排
            if (FormMain.camNumber > 2)
            {
                int columncnt, rowcnt;
                columncnt = FormMain.camNumber / 2 + FormMain.camNumber % 2;
                rowcnt = 2;
                this.tableLayoutPanel1.ColumnCount = columncnt;
                this.tableLayoutPanel1.RowCount = rowcnt;
                this.tableLayoutPanel1.ColumnStyles.Clear();
                this.tableLayoutPanel1.RowStyles.Clear();
                for (int i = 0; i < columncnt; i++)
                {
                    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                        System.Windows.Forms.SizeType.Percent, 
                        100 / (float)columncnt));
                }
                for (int i = 0; i < rowcnt; i++)
                {
                    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(
                        System.Windows.Forms.SizeType.Percent, 
                        100 / (float)rowcnt));
                }
                for (int i = 0; i < FormMain.camNumber; i++)
                {
                    if (i < columncnt)
                        this.tableLayoutPanel1.Controls.Add(this.cogDisplayCtl[i], i, 0);
                    else this.tableLayoutPanel1.Controls.Add(this.cogDisplayCtl[i], i - columncnt, 1);
                }

                int tmp = 0;
                for (int i = 0; i < columncnt; i++)
                {
                    for (int j = 0; j < rowcnt; j++)
                    {
                        tmp = columncnt * j + i + 1;
                        if (tmp > FormMain.camNumber)
                        {
                            PictureBox pictureBox1 = new PictureBox();
                            pictureBox1.BackColor = System.Drawing.SystemColors.Control;
                            pictureBox1.BackgroundImage = global::Vision_System.Properties.Resources.watermark;
                            pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
                            pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
                            pictureBox1.Location = new System.Drawing.Point(3, 254);
                            pictureBox1.Name = "pictureBox1";
                            pictureBox1.Size = new System.Drawing.Size(534, 245);
                            pictureBox1.TabIndex = 2;
                            pictureBox1.TabStop = false;
                            pictureBox1.WaitOnLoad = true;
                            this.tableLayoutPanel1.Controls.Add(pictureBox1, i, j);
                        }
                    }
                }
            }
            else
            {
                int columncnt, rowcnt;
                columncnt = FormMain.camNumber;
                rowcnt = 1;
                this.tableLayoutPanel1.ColumnCount = columncnt;
                this.tableLayoutPanel1.RowCount = rowcnt;
                this.tableLayoutPanel1.ColumnStyles.Clear();
                this.tableLayoutPanel1.RowStyles.Clear();
                for (int i = 0; i < columncnt; i++)
                {
                    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / (float)columncnt));
                }
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

                for (int i = 0; i < FormMain.camNumber; i++)
                {
                    this.tableLayoutPanel1.Controls.Add(this.cogDisplayCtl[i], i, 0);
                }
            }
        }

        /// <summary>
        /// 当双击其中的一个Display时，更新主页面的布局，使该Display最大化，
        /// 其他Display以小窗口呈现
        /// </summary>
        /// <param name="index">index表示作为双击的Display序号</param>
        private void UpdateCusotmizedLayout(int index)
        {
            for (int i = 0; i < FormMain.camNumber; i++)
                this.cogDisplayCtl[i].ClearProcessingTimeLabel();
            this.tableLayoutPanelMain.Controls.Clear();
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Clear();
            this.tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Clear();
            this.tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            this.tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

            // CogDisplay横着依次排开
            int columnCnt = FormMain.camNumber - 1;
            int mColumn = 0;
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.ColumnCount = columnCnt;
            this.tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < columnCnt; i++)
                this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / (float)columnCnt));
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                if (i != index)
                {
                    this.cogDisplayCtl[i].IsShowDataView = false;
                    this.cogDisplayCtl[i].UpdateViewLayout();
                    this.tableLayoutPanel1.Controls.Add(this.cogDisplayCtl[i], mColumn, 0);
                    mColumn++;
                }
            }
            // 优化处理：在此处添加控件，可以防止页面多次刷新导致的控件闪动的情况，
            // 等内部控件初始化完成后一次性添加进来
            this.tableLayoutPanelMain.Controls.Add(cogDisplayCtl[index], 0, 0);
            this.tableLayoutPanelMain.Controls.Add(tableLayoutPanel1, 0, 1);
            // 更新界面
            this.Refresh();

            for (int i = 0; i < FormMain.camNumber; i++)
            {
                //if (i == index) this.cogDisplayCtl[i].IsProcessingTimeLabelVisible = true;
                //else this.cogDisplayCtl[i].IsProcessingTimeLabelVisible = false;
                this.cogDisplayCtl[i].UpdateProcessingTimeLabel();
            }
        }

        /// <summary>
        /// 将界面布局复位到常规默认的模式
        /// </summary>
        public void ResetLayout()
        {
            if (FormMain.camNumber <= 3) return;
            for (int i = 0; i < FormMain.camNumber; i++)
                this.cogDisplayCtl[i].ClearProcessingTimeLabel();
            this.tableLayoutPanelMain.Controls.Clear();
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Clear();
            this.tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowCount = 1;
            this.tableLayoutPanelMain.RowStyles.Clear();
            this.tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // 如果相机数量大于2，排成两排
            if (FormMain.camNumber > 2)
            {
                int columncnt, rowcnt;
                columncnt = FormMain.camNumber / 2 + FormMain.camNumber % 2;
                rowcnt = 2;
                this.tableLayoutPanel1.ColumnCount = columncnt;
                this.tableLayoutPanel1.RowCount = rowcnt;
                this.tableLayoutPanel1.ColumnStyles.Clear();
                this.tableLayoutPanel1.RowStyles.Clear();
                for (int i = 0; i < columncnt; i++)
                {
                    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                        System.Windows.Forms.SizeType.Percent, 
                        100 / (float)columncnt));
                }
                for (int i = 0; i < rowcnt; i++)
                {
                    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(
                        System.Windows.Forms.SizeType.Percent, 
                        100 / (float)rowcnt));
                }
                for (int i = 0; i < FormMain.camNumber; i++)
                {
                    if (i < columncnt)
                        this.tableLayoutPanel1.Controls.Add(this.cogDisplayCtl[i], i, 0);
                    else this.tableLayoutPanel1.Controls.Add(this.cogDisplayCtl[i], i - columncnt, 1);
                }

                int tmp = 0;
                for (int i = 0; i < columncnt; i++)
                {
                    for (int j = 0; j < rowcnt; j++)
                    {
                        tmp = columncnt * j + i + 1;
                        if (tmp > FormMain.camNumber)
                        {
                            PictureBox pictureBox1 = new PictureBox();
                            pictureBox1.BackColor = System.Drawing.SystemColors.Control;
                            pictureBox1.BackgroundImage = global::Vision_System.Properties.Resources.watermark;
                            pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
                            pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
                            pictureBox1.Location = new System.Drawing.Point(3, 254);
                            pictureBox1.Name = "pictureBox1";
                            pictureBox1.Size = new System.Drawing.Size(534, 245);
                            pictureBox1.TabIndex = 2;
                            pictureBox1.TabStop = false;
                            pictureBox1.WaitOnLoad = true;
                            this.tableLayoutPanel1.Controls.Add(pictureBox1, i, j);
                        }
                    }
                }
            }
            else
            {
                int columncnt, rowcnt;
                columncnt = FormMain.camNumber;
                rowcnt = 1;
                this.tableLayoutPanel1.ColumnCount = columncnt;
                this.tableLayoutPanel1.RowCount = rowcnt;
                this.tableLayoutPanel1.ColumnStyles.Clear();
                this.tableLayoutPanel1.RowStyles.Clear();
                for (int i = 0; i < columncnt; i++)
                {
                    this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(
                        System.Windows.Forms.SizeType.Percent, 
                        100 / (float)columncnt));
                }
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Percent, 
                    100F));

                for (int i = 0; i < FormMain.camNumber; i++)
                {
                    this.tableLayoutPanel1.Controls.Add(this.cogDisplayCtl[i], i, 0);
                }
            }
            // 优化处理：在此处添加控件，可以防止页面多次刷新导致的控件闪动的情况，
            // 等内部控件初始化完成后一次性添加进来
            this.tableLayoutPanelMain.Controls.Add(tableLayoutPanel1, 0, 0);
            // 更新界面
            this.Refresh();

            for (int i = 0; i < FormMain.camNumber; i++)
            {
                //this.cogDisplayCtl[i].IsProcessingTimeLabelVisible = true;
                this.cogDisplayCtl[i].UpdateProcessingTimeLabel();
            }
        }

        /// <summary>
        /// 停止所有CogDisplayCtrl的Live模式（如果有）
        /// </summary>
        public void StopAllCCDLiveMode()
        {
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                cogDisplayCtl[i].StopLiveVideoMode();
            }
        }

        /// <summary>
        /// 在CogDisplayCtrl中操作相关控件，CCD有结果时，需要更新home Page中的界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="runResult"></param>
        public void JobResultAvailable(object sender)
        {
            // 判断是哪个display触发的事件
            CogDisplayCtl control = sender as CogDisplayCtl;
            int index = Convert.ToInt16(control.Name.Substring(control.Name.Length - 1, 1)) - 1;
            CogToolBlock tool;
            bool outputResult; //适用于编写脚本判断结果的情况下，ToolBlock有一个输出为"Result"，
                               //脚本控制Result的结果，这种情况下ToolBlock的判断结果为输出"Result" + 
                               //工具运行的结果是否为Accept
            bool toolResult; //每个toolBlock的分结果
            bool totalResult = true;//每个job的总结果
            for (int j = 0; j < FormMain.jobHelper[index].ToolBlockCnt; j++)
            {
                tool = (CogToolBlock)ToolBlock[index].Tools[j];
                FormMain.jobHelper[index].TotalCountForItem[j] += 1;
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
                if (FormMain.jobHelper[index].IsToolBlockEnabledForItem[j])
                {
                    FormMain.jobHelper[index].PassFailResultForItem[j] = toolResult ? true : false;
                    FormMain.jobHelper[index].FailCountForItem[j] += toolResult ? 0 : 1;
                }
                else
                {
                    // 如果检测工具没有使能，结果为true
                    FormMain.jobHelper[index].PassFailResultForItem[j] = true;
                    FormMain.jobHelper[index].FailCountForItem[j] += 0;
                }

                //计算总结果
                totalResult &= FormMain.jobHelper[index].PassFailResultForItem[j];

                // 如果总数为0，为避免除数为0的异常错误，将yield设置为null
                if (FormMain.jobHelper[index].TotalCountForItem[j] == 0)
                {
                    FormMain.jobHelper[index].YieldForItem[j] = null;
                }
                else
                {
                    FormMain.jobHelper[index].YieldForItem[j] = 1 -
                        (double)FormMain.jobHelper[index].FailCountForItem[j] / FormMain.jobHelper[index].TotalCountForItem[j];
                }

            }
            //job运行数加一
            FormMain.jobHelper[index].JobTotalRunCnt += 1;
            //job通过计数
            FormMain.jobHelper[index].JobTotalRunPass += totalResult ? 1 : 0;
            FormMain.jobHelper[index].JobTotalResult = totalResult;
            //计算通过率
            if (FormMain.jobHelper[index].JobTotalRunCnt == 0)
            {
                FormMain.jobHelper[index].JobYield = null;
            }
            else
            {
                FormMain.jobHelper[index].JobYield = FormMain.jobHelper[index].JobTotalRunPass / (double)FormMain.jobHelper[index].JobTotalRunCnt;
            }
            // 更新cogDisplayCtrl中的统计数据
            cogDisplayCtl[index].UpdateStatisticData(FormMain.jobHelper[index].JobTotalRunPass, FormMain.jobHelper[index].JobTotalRunCnt);
            // 更新OK/NG结果数据给CogDisplayCtl
            cogDisplayCtl[index].JobRunResult = FormMain.jobHelper[index].JobTotalResult;
            cogDisplayCtl[index].ToolRunResult = FormMain.jobHelper[index].PassFailResultForItem.ToArray();
            // 更新DataGridView中的结果数据
            UpdateDatagridViewResult(index);
        }

        /// <summary>
        /// CCD2 有结果需要更新时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="runResult"></param>
        public void Job2ResultAvailable(object sender, bool runResult)
        {
            CogToolBlock tool;
            for (int j = 0; j < FormMain.jobHelper[1].ToolBlockCnt; j++)
            {
                tool = (CogToolBlock)ToolBlock[1].Tools[j];
                FormMain.jobHelper[1].TotalCountForItem[j] += 1;
                FormMain.jobHelper[1].FailCountForItem[j] += tool.RunStatus.Result == CogToolResultConstants.Accept  ? 0 : 1;
                if (FormMain.jobHelper[1].TotalCountForItem[j] == 0)
                {
                    FormMain.jobHelper[1].YieldForItem[j] = null;
                }
                else
                {
                    FormMain.jobHelper[1].YieldForItem[j] = 1 -
                        (double)FormMain.jobHelper[1].FailCountForItem[j] / FormMain.jobHelper[1].TotalCountForItem[j];
                }
            }
            UpdateDatagridViewResult(1);
        }

        /// <summary>
        /// CCD3 有结果需要更新时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="runResult"></param>
        public void Job3ResultAvailable(object sender, bool runResult)
        {
            CogToolBlock tool;
            for (int j = 0; j < FormMain.jobHelper[2].ToolBlockCnt; j++)
            {
                tool = (CogToolBlock)ToolBlock[2].Tools[j];
                FormMain.jobHelper[2].TotalCountForItem[j] += 1;
                FormMain.jobHelper[2].FailCountForItem[j] += tool.RunStatus.Result == CogToolResultConstants.Accept ? 0 : 1;
                if (FormMain.jobHelper[2].TotalCountForItem[j] == 0)
                {
                    FormMain.jobHelper[2].YieldForItem[j] = null;
                }
                else
                {
                    FormMain.jobHelper[2].YieldForItem[j] = 1 -
                        (double)FormMain.jobHelper[2].FailCountForItem[j] / FormMain.jobHelper[2].TotalCountForItem[j];
                }
            }
            UpdateDatagridViewResult(2);
        }

        /// <summary>
        /// CCD4 有结果需要更新时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="runResult"></param>
        public void Job4ResultAvailable(object sender, bool runResult)
        {
            CogToolBlock tool;
            for (int j = 0; j < FormMain.jobHelper[3].ToolBlockCnt; j++)
            {
                tool = (CogToolBlock)ToolBlock[3].Tools[j];
                FormMain.jobHelper[3].TotalCountForItem[j] += 1;
                FormMain.jobHelper[3].FailCountForItem[j] += tool.RunStatus.Result == CogToolResultConstants.Accept ? 0 : 1;
                if (FormMain.jobHelper[3].TotalCountForItem[j] == 0)
                {
                    FormMain.jobHelper[3].YieldForItem[j] = null;
                }
                else
                {
                    FormMain.jobHelper[3].YieldForItem[j] = 1 -
                        (double)FormMain.jobHelper[3].FailCountForItem[j] / FormMain.jobHelper[3].TotalCountForItem[j];
                }
            }
            UpdateDatagridViewResult(3);
        }

        /// <summary>
        /// 传递AcqFifoTool和 ToolBlock变量到PageHome类中
        /// </summary>
        /// <param name="toolblock"></param>
        /// <param name="fifotool"></param>
        public void SetToolBlock(CogToolBlock[] toolblock, CogAcqFifoTool[] fifotool)
        {
            ToolBlock = new CogToolBlock[FormMain.camNumber];
            // default camera number is 4
            cogDisplayCtl = new CogDisplayCtl[FormMain.camNumber];
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                ToolBlock[i] = toolblock[i];
                cogDisplayCtl[i] = new CogDisplayCtl();
                cogDisplayCtl[i].ToolBlock = toolblock[i];
                cogDisplayCtl[i].AcqFifoTool = fifotool[i];
                cogDisplayCtl[i].CCDIndex = i;
                cogDisplayCtl[i].ToolRunResult = new bool[ToolBlock[i].Tools.Count];
            }
        }

        /// <summary>
        /// 换型后传递新的ToolBlock变量
        /// </summary>
        /// <param name="toolblock"></param>
        /// <param name="fifotool"></param>
        public void UpdateToolBlock(CogToolBlock[] toolblock, CogAcqFifoTool[] fifotool)
        {
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                ToolBlock[i] = toolblock[i];
                cogDisplayCtl[i].ToolBlock = toolblock[i];
                cogDisplayCtl[i].AcqFifoTool = fifotool[i];
                cogDisplayCtl[i].CCDIndex = i;
            }
        }

        /// <summary>
        /// 设置cogDisplay工具栏的可见状态
        /// </summary>
        /// <param name="visible"></param>
        public void SetDisplayToolBarVisible(bool visible)
        {
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                this.cogDisplayCtl[i].SetToolBarVisible(visible);
                //this.cogDisplayCtl[1].SetToolBarVisable(visible);
            }
        }

        /// <summary>
        /// 初始化DataGridView：初始化Column以及样式
        /// </summary>
        private void InitializeDataGridView()
        {
            // dgvResultView.DataSource = dtResultDetails;
            switch (FormMain.settingHelper.SoftwareLanguage)
            {
                case LanguageType.Chinese:
                    dtResultDetails.Columns.Add("检测项目", typeof(string));
                    dtResultDetails.Columns.Add("结果", typeof(Bitmap));
                    dtResultDetails.Columns.Add("总数", typeof(string));
                    dtResultDetails.Columns.Add("失效", typeof(string));
                    dtResultDetails.Columns.Add("通过率", typeof(string));
                    break;
                case LanguageType.English:
                    dtResultDetails.Columns.Add("Tool Name", typeof(string));
                    dtResultDetails.Columns.Add("Result", typeof(Bitmap));
                    dtResultDetails.Columns.Add("Total", typeof(string));
                    dtResultDetails.Columns.Add("Fail", typeof(string));
                    dtResultDetails.Columns.Add("Yield", typeof(string));
                    break;
                default:
                    break;
            }
            InitializeDataGridViewStyle();
        }

        /// <summary>
        /// 初始化DataGridView样式
        /// </summary>
        private void InitializeDataGridViewStyle()
        {
            System.Windows.Forms.DataGridViewTextBoxColumn Column0 = new DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn Column1 = new DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewImageColumn Column2 = new DataGridViewImageColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn Column3 = new DataGridViewTextBoxColumn();
            System.Windows.Forms.DataGridViewTextBoxColumn Column4 = new DataGridViewTextBoxColumn();

            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            switch (FormMain.settingHelper.SoftwareLanguage)
            {
                case LanguageType.Chinese:
                    // 
                    // Column0
                    // 
                    Column0.HeaderText = "名称";
                    Column0.Name = "Column0";
                    Column0.Width = 60;
                    // 
                    // Column1
                    // 
                    Column1.HeaderText = "检测项目";
                    Column1.Name = "Column1";
                    Column1.Width = 120;
                    // 
                    // Column2
                    // 
                    Column2.DefaultCellStyle = dataGridViewCellStyle2;
                    Column2.HeaderText = "结果";
                    Column2.Name = "Column2";
                    Column2.Width = 50;
                    // 
                    // Column3
                    // 
                    Column3.HeaderText = "失效";
                    Column3.Name = "Column4";
                    Column3.Width = 60;
                    // 
                    // Column4
                    // 
                    Column4.HeaderText = "通过率";
                    Column4.Name = "Column4";
                    Column4.Width = 70;
                    break;
                case LanguageType.English:
                    // 
                    // Column0
                    // 
                    Column0.HeaderText = "CCD";
                    Column0.Name = "Column0";
                    Column0.Width = 60;
                    // 
                    // Column1
                    // 
                    Column1.HeaderText = "Tool Name";
                    Column1.Name = "Column1";
                    Column1.Width = 120;
                    // 
                    // Column2
                    // 
                    Column2.DefaultCellStyle = dataGridViewCellStyle2;
                    Column2.HeaderText = "Result";
                    Column2.Name = "Column2";
                    Column2.Width = 50;
                    // 
                    // Column3
                    // 
                    Column3.HeaderText = "Fail";
                    Column3.Name = "Column4";
                    Column3.Width = 60;
                    // 
                    // Column4
                    // 
                    Column4.HeaderText = "Yield";
                    Column4.Name = "Column4";
                    Column4.Width = 70;
                    break;
                default:
                    break;
            }
            // 设置表格样式
            DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvResultView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvResultView.BackgroundColor = System.Drawing.SystemColors.Control;
            dgvResultView.GridColor = System.Drawing.SystemColors.Control;
            // 设置表格交替样式
            dgvResultView.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            // 添加表格列
            dgvResultView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            Column0,
            Column1,
            Column2,
            Column3,
            Column4});

            // 设置单元格的值为"Y", 对应的checkbox为true, 单元格值为"N", 对应的checkbox为false
            //((DataGridViewCheckBoxColumn)dgvResultView.Columns[0]).TrueValue = "Y";
            //((DataGridViewCheckBoxColumn)dgvResultView.Columns[0]).FalseValue = "N";
        }

        /// <summary>
        /// 初始化DataGridView内容
        /// </summary>
        public void InitDatagirdViewResult()
        {
            CogToolBlock tool;
            try
            {
                dgvResultView.Rows.Clear();
                for (int i = 0; i < FormMain.camNumber; i++)
                {
                    for (int j = 0; j < FormMain.jobHelper[i].ToolBlockCnt; j++)
                    {
                        tool = (CogToolBlock)ToolBlock[i].Tools[j];
                        object[] dr = new object[5];
                        dr[0] = FormMain.jobHelper[i].CcdName;
                        dr[1] = tool.Name;
                        dr[2] = Properties.Resources.accept;
                        dr[3] = "0";
                        dr[4] = "--.--%";
                        dgvResultView.Rows.Add(dr);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("表格创建失败！");
            }
        }

        /// <summary>
        /// CogDisplayCtrl有新的结果时，通知更新DataGridView
        /// </summary>
        /// <param name="index"></param>
        /// <param name="runResult"></param>
        public void UpdateDatagridViewResult(int index)
        {
            //DataRow dr;
            CogToolBlock tool;
            int columnOffset = 0;
            try
            {
                // FormMain.mJobData[index].DataRowNum = GetCogToolBlock[index].Tools.Count;
                for (int i = 0; i < index; i++)
                {
                    columnOffset += FormMain.jobHelper[i].ToolBlockCnt;
                }

                for (int j = 0; j < FormMain.jobHelper[index].ToolBlockCnt; j++)
                {
                    tool = (CogToolBlock)ToolBlock[index].Tools[j];
                    object[] dr = new object[5];
                    dr[0] = FormMain.jobHelper[index].CcdName;
                    dr[1] = tool.Name;
                    dr[2] = FormMain.jobHelper[index].PassFailResultForItem[j]? Properties.Resources.accept : Properties.Resources.reject;
                    dr[3] = FormMain.jobHelper[index].FailCountForItem[j].ToString();
                    dr[4] = FormMain.jobHelper[index].YieldForItem[j] == null ? "--.--%" :
                        ((double)(FormMain.jobHelper[index].YieldForItem[j] * 100)).ToString("0.00") + "%";

                    for (int k = 0; k < GetDataTable.Columns.Count; k++)
                    {
                        dgvResultView.Rows[columnOffset + j].Cells[k].Value = dr[k];
                    }
                }
            }
            catch
            {
                MessageBox.Show("表格更新失败！");
            }
        }

        /// <summary>
        /// 表格中Checkbox点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvResultView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnOffset = 0;
            int i = 0;
            int jobIndex = 0; //相机的序号
            int toolIndex = 0; //检测工具的序号

            //单击复选框时 
            if (e.ColumnIndex == 0)
            {
                DataGridViewCell dgcell = dgvResultView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                bool ischkBefore = (bool)dgcell.FormattedValue;         //选中前  
                bool ischkAfter = (bool)dgcell.EditedFormattedValue;   //选中后
                // 判断是否单击了CheckBox
                if (ischkBefore != ischkAfter)
                {
                    while (columnOffset <= e.RowIndex)
                    {
                        columnOffset += FormMain.jobHelper[i].ToolBlockCnt;
                        i++;
                    }
                    jobIndex = i - 1;
                    toolIndex = e.RowIndex - (columnOffset - FormMain.jobHelper[i - 1].ToolBlockCnt);
                    
                    FormMain.jobHelper[jobIndex].IsToolBlockEnabledForItem[toolIndex] = ischkAfter;
                    IniFile ConfigIniFile = new IniFile(FormMain.strCCDConfigFilePath[jobIndex]);
                    // 将CCD检测工具的使能状态写入ini文件中
                    StringBuilder builder = new StringBuilder("1,1,1,1,1,1,1,1,1,1", 20);
                    char oldChar, newChar;
                    if (ischkAfter)
                    {
                        oldChar = '0';
                        newChar = '1';
                    }
                    else
                    {
                        oldChar = '1';
                        newChar = '0';
                    }
                    builder.Replace(oldChar, newChar, toolIndex * 2, 1);
                    ConfigIniFile.IniWriteValue("CCD" + (jobIndex + 1), "ToolEnable", builder.ToString());
                }
            }
        }

        /// <summary>
        /// 更新CogDisplay，显示图片
        /// </summary>
        /// <param name="index"></param>
        /// <param name="image"></param>
        public void UpdateImageCogDisplayCtrlByExternalTrigger(int index, ICogImage image, bool runResult)
        {
            cogDisplayCtl[index].UpdateUIByExternalTrigger(image, runResult);
            cogDisplayCtl[index].UpdateStatisticData(FormMain.jobHelper[index].JobTotalRunPass,
                                                        FormMain.jobHelper[index].JobTotalRunCnt);
        }

        /// <summary>
        /// 更新CogDisplay，显示图片
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public void UpdateRecordInCogDisplayCtrlByExternalTrigger(int index, ICogImage image, ICogRecords records, string processingTime, bool runResult)
        {
            cogDisplayCtl[index].ToolRunResult = FormMain.jobHelper[index].PassFailResultForItem.ToArray();
            cogDisplayCtl[index].UpdateUIByExternalTrigger(image, records, processingTime, runResult);
            cogDisplayCtl[index].UpdateStatisticData(FormMain.jobHelper[index].JobTotalRunPass,
                                                        FormMain.jobHelper[index].JobTotalRunCnt);
        }

        /// <summary>
        /// 添加标记到CogDisplay
        /// </summary>
        /// <param name="index"></param>
        /// <param name="label"></param>
        public void AddGraphicLabelToCogDisplay(int index, CogGraphicLabel label, string groupName)
        {
            cogDisplayCtl[index].AddGraphicLabelToCogDisplay(label, groupName);
        }

        /// <summary>
        /// 将料号清单添加到comboBox里面
        /// </summary>
        /// <param name="list"></param>
        /// <param name="select"></param>
        public void InitializeComboBoxPartNumber(List<string> list, string select)
        {
            comboBox_ProductName.DataSource = list;
            comboSelectedIndex = list.IndexOf(select);
            comboBox_ProductName.SelectedIndex = comboSelectedIndex;
            comboBox_ProductName.SelectedIndexChanged += new System.EventHandler(this.comBox_PN_SelectedIndexChanged);
        }

        /// <summary>
        /// 将料号清单添加到comboBox里面
        /// </summary>
        /// <param name="list"></param>
        /// <param name="select"></param>
        public void UpdateComboBoxPartNumber(List<string> list, string select)
        {
            comboBox_ProductName.SelectedIndexChanged -= new System.EventHandler(this.comBox_PN_SelectedIndexChanged);
            comboBox_ProductName.DataSource = null;
            comboBox_ProductName.DataSource = list;
            comboSelectedIndex = list.IndexOf(select);
            comboBox_ProductName.SelectedIndex = comboSelectedIndex;
            comboBox_ProductName.SelectedIndexChanged += new System.EventHandler(this.comBox_PN_SelectedIndexChanged);
        }

        /// <summary>
        /// 手动切换料号，弹出料号切换确认窗口，取消切换料号时恢复之前的料号选择
        /// </summary>
        /// <param name="cancle"></param>
        public void CancleComboBoxIndexChange(bool cancle)
        {
            if (cancle)
            {
                // if the user cancel the the part number change, change the combobox selected index back to
                // the original value, before this, you should dis-attach the SelectedIndexChanged event and
                // re-attach this event after the selected index changed.
                comboBox_ProductName.SelectedIndexChanged -= new System.EventHandler(this.comBox_PN_SelectedIndexChanged);
                comboBox_ProductName.SelectedIndex = comboSelectedIndex;
                comboBox_ProductName.SelectedIndexChanged += new System.EventHandler(this.comBox_PN_SelectedIndexChanged);
            }
            else
            {
                // cancle = false, if the user confirm the part number change, record the combobox current selected index
                comboSelectedIndex = comboBox_ProductName.SelectedIndex;
            }
        }

        /// <summary>
        /// comboBox 切换触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comBox_PN_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            int newIndex = cmb.SelectedIndex;
            // 根据名字查询料号的序号
            DataRow[] dataRows;
            int partNoIndex;
            dataRows = FormMain.settingHelper.DataTablePartNoInfo.Select("PNName = " 
                + "'" + cmb.SelectedItem.ToString() + "'");
            if (dataRows.Length >= 1)
            {
                partNoIndex = (int)dataRows[0]["PNIndex"];
                ChangePartNumber?.Invoke(this, partNoIndex);
            }
        }

        /// <summary>
        /// 清除所有统计数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ClearStatistics_Click(object sender, EventArgs e)
        {
            ClearAllStastics();
        }

        private void btnClearDisplayImage_Click(object sender, EventArgs e)
        {
            ClearAllDisplayImage();
        }

        #region Clear the home page all statistics
        /// <summary>
        /// 在HomePage中清除所有统计数据
        /// </summary>
        public void ClearAllStastics()
        {
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                for (int j = 0; j < FormMain.jobHelper[i].ToolBlockCnt; j++)
                {
                    FormMain.jobHelper[i].TotalCountForItem[j] = 0;
                    FormMain.jobHelper[i].FailCountForItem[j] = 0;
                    FormMain.jobHelper[i].YieldForItem[j] = null;
                }
                FormMain.jobHelper[i].JobTotalRunCnt = 0;
                FormMain.jobHelper[i].JobTotalRunPass = 0;
                FormMain.jobHelper[i].JobYield = null;
                UpdateDatagridViewResult(i);
                ClearDisplayCtrlStatisticData(i);
            }
        }

        /// <summary>
        /// 在HomePage中清除所有CogDisplayCtrl的图像
        /// </summary>
        public void ClearAllDisplayImage()
        {
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                cogDisplayCtl[i].ClearDisplayImage();
            }
        }

        /// <summary>
        /// 清除单个CogDisplayCtrl中的统计数据
        /// </summary>
        /// <param name="index"></param>
        private void ClearDisplayCtrlStatisticData(int index)
        {
            cogDisplayCtl[index].ClearStatisticData();
        }

        /// <summary>
        /// 清除单个CogDisplayCtrl中的图像
        /// </summary>
        /// <param name="index"></param>
        private void ClearDisplayCtrlImage(int index)
        {
            cogDisplayCtl[index].ClearDisplayImage();
        }

        /// <summary>
        /// 在CogDisplayCtrl中点击复位Display
        /// </summary>
        /// <param name="sender"></param>
        private void ResetDisplay(object sender)
        {
            // 判断是哪个display触发的事件
            CogDisplayCtl control = sender as CogDisplayCtl;
            int index = Convert.ToInt16(control.Name.Substring(control.Name.Length - 1, 1)) - 1;
            for (int i = 0; i < FormMain.jobHelper[index].ToolBlockCnt; i++)
            {
                FormMain.jobHelper[index].TotalCountForItem[i] = 0;
                FormMain.jobHelper[index].FailCountForItem[i] = 0;
                FormMain.jobHelper[index].YieldForItem[i] = null;
            }
            FormMain.jobHelper[index].JobTotalRunCnt = 0;
            FormMain.jobHelper[index].JobTotalRunPass = 0;
            FormMain.jobHelper[index].JobYield = null;
            UpdateDatagridViewResult(index);
            ClearDisplayCtrlStatisticData(index);
            ClearDisplayCtrlImage(index);
        }
        #endregion

        #region Log in textbox, support cross thread access
        public delegate void LogAppendDelegate(Color color, string text);
        /// <summary> 
        /// 追加显示文本 
        /// </summary> 
        /// <param name="color">文本颜色</param> 
        /// <param name="text">显示文本</param> 
        public void LogAppend(Color color, string text)
        {
            // 如果行数超过500行，清除内容
            if (rtbReceiveMsgBox.Lines.Length > 500)
                rtbReceiveMsgBox.Clear();
            // 自动滚动到最新一条信息
            rtbReceiveMsgBox.ScrollToCaret();
            rtbReceiveMsgBox.SelectionColor = color;
            rtbReceiveMsgBox.AppendText(text);
            rtbReceiveMsgBox.AppendText("\n");
        }
        /// <summary> 
        /// 显示错误日志 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogError(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtbReceiveMsgBox.Invoke(la, Color.Red, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + text);
        }
        /// <summary> 
        /// 显示警告信息 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogWarning(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtbReceiveMsgBox.Invoke(la, Color.Violet, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + text);
        }
        /// <summary> 
        /// 显示信息 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtbReceiveMsgBox.Invoke(la, Color.Black, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + text);
        }

        /// <summary> 
        /// 显示通过信息
        /// </summary> 
        /// <param name="text"></param> 
        public void LogPass(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtbReceiveMsgBox.Invoke(la, Color.Green, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + text);
        }

        /// <summary> 
        /// 显示未通过日志 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogFail(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtbReceiveMsgBox.Invoke(la, Color.Red, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + text);
        }

        /// <summary>
        /// 清楚消息窗口的所有内容
        /// </summary>
        public void ClearMessageBox()
        {
            rtbReceiveMsgBox.Clear();
        }
        #endregion
    }
}
