namespace Vision_System
{
    partial class CogDisplayCtl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                this.bgwCCDLive.CancelAsync();
                this.bgwCCDLive.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CogDisplayCtl));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cogRecordDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripBtnLive = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnImportImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnManualTrigger = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnSaveImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnShowChart = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnVSplit = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnHSplit = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnParamSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnInfo = new System.Windows.Forms.ToolStripButton();
            this.cogDisplayStatusBar = new Cognex.VisionPro.CogDisplayStatusBarV2();
            this.bgwCCDLive = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblRunData = new System.Windows.Forms.Label();
            this.pictureBox20 = new System.Windows.Forms.PictureBox();
            this.pictureBox19 = new System.Windows.Forms.PictureBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox18 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.pictureBox17 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox16 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox15 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox14 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox100 = new System.Windows.Forms.PictureBox();
            this.chartData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvDataResultView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox100)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataResultView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cogRecordDisplay
            // 
            this.cogRecordDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogRecordDisplay.ColorMapLowerRoiLimit = 0D;
            this.cogRecordDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogRecordDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogRecordDisplay.ColorMapUpperRoiLimit = 1D;
            this.cogRecordDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogRecordDisplay.DoubleTapZoomCycleLength = 2;
            this.cogRecordDisplay.DoubleTapZoomSensitivity = 2.5D;
            this.cogRecordDisplay.Location = new System.Drawing.Point(3, 3);
            this.cogRecordDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogRecordDisplay.MouseWheelSensitivity = 1D;
            this.cogRecordDisplay.Name = "cogRecordDisplay";
            this.cogRecordDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogRecordDisplay.OcxState")));
            this.cogRecordDisplay.Size = new System.Drawing.Size(679, 241);
            this.cogRecordDisplay.TabIndex = 1;
            this.cogRecordDisplay.DoubleClick += new System.EventHandler(this.cogRecordDisplay_DoubleClick);
            // 
            // toolStrip
            // 
            this.toolStrip.AutoSize = false;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnLive,
            this.toolStripBtnImportImage,
            this.toolStripBtnManualTrigger,
            this.toolStripBtnSaveImage,
            this.toolStripBtnShowChart,
            this.toolStripBtnVSplit,
            this.toolStripBtnHSplit,
            this.toolStripBtnParamSetting,
            this.toolStripBtnReset,
            this.toolStripBtnInfo});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(691, 30);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripBtnLive
            // 
            this.toolStripBtnLive.AutoSize = false;
            this.toolStripBtnLive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnLive.Image = global::Vision_System.Properties.Resources.video_mode;
            this.toolStripBtnLive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnLive.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnLive.Name = "toolStripBtnLive";
            this.toolStripBtnLive.Size = new System.Drawing.Size(28, 30);
            this.toolStripBtnLive.Text = "实时模式";
            this.toolStripBtnLive.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnLive.Click += new System.EventHandler(this.toolStripButton_ItemClicked);
            // 
            // toolStripBtnImportImage
            // 
            this.toolStripBtnImportImage.AutoSize = false;
            this.toolStripBtnImportImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnImportImage.Image = global::Vision_System.Properties.Resources.load_from_computer;
            this.toolStripBtnImportImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnImportImage.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnImportImage.Name = "toolStripBtnImportImage";
            this.toolStripBtnImportImage.Size = new System.Drawing.Size(28, 30);
            this.toolStripBtnImportImage.Text = "从电脑中加载图片";
            this.toolStripBtnImportImage.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnImportImage.Click += new System.EventHandler(this.toolStripButton_ItemClicked);
            // 
            // toolStripBtnManualTrigger
            // 
            this.toolStripBtnManualTrigger.AutoSize = false;
            this.toolStripBtnManualTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnManualTrigger.Image = global::Vision_System.Properties.Resources.run_once;
            this.toolStripBtnManualTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnManualTrigger.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnManualTrigger.Name = "toolStripBtnManualTrigger";
            this.toolStripBtnManualTrigger.Size = new System.Drawing.Size(28, 30);
            this.toolStripBtnManualTrigger.Text = "手动触发";
            this.toolStripBtnManualTrigger.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnManualTrigger.Click += new System.EventHandler(this.toolStripButton_ItemClicked);
            // 
            // toolStripBtnSaveImage
            // 
            this.toolStripBtnSaveImage.AutoSize = false;
            this.toolStripBtnSaveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnSaveImage.Image = global::Vision_System.Properties.Resources.saveimage;
            this.toolStripBtnSaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnSaveImage.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnSaveImage.Name = "toolStripBtnSaveImage";
            this.toolStripBtnSaveImage.Size = new System.Drawing.Size(28, 30);
            this.toolStripBtnSaveImage.Text = "保存图片到电脑";
            this.toolStripBtnSaveImage.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnSaveImage.Click += new System.EventHandler(this.toolStripButton_ItemClicked);
            // 
            // toolStripBtnShowChart
            // 
            this.toolStripBtnShowChart.AutoSize = false;
            this.toolStripBtnShowChart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnShowChart.Image = global::Vision_System.Properties.Resources.data_chart;
            this.toolStripBtnShowChart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnShowChart.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnShowChart.Name = "toolStripBtnShowChart";
            this.toolStripBtnShowChart.Size = new System.Drawing.Size(28, 30);
            this.toolStripBtnShowChart.Text = "显示图表";
            this.toolStripBtnShowChart.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnShowChart.Click += new System.EventHandler(this.toolStripButton_ItemClicked);
            // 
            // toolStripBtnVSplit
            // 
            this.toolStripBtnVSplit.AutoSize = false;
            this.toolStripBtnVSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnVSplit.Image = global::Vision_System.Properties.Resources.H_Split;
            this.toolStripBtnVSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnVSplit.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnVSplit.Name = "toolStripBtnVSplit";
            this.toolStripBtnVSplit.Size = new System.Drawing.Size(28, 30);
            this.toolStripBtnVSplit.Text = "上下布局";
            this.toolStripBtnVSplit.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnVSplit.Click += new System.EventHandler(this.toolStripButton_ItemClicked);
            // 
            // toolStripBtnHSplit
            // 
            this.toolStripBtnHSplit.AutoSize = false;
            this.toolStripBtnHSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnHSplit.Image = global::Vision_System.Properties.Resources.V_Split;
            this.toolStripBtnHSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnHSplit.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnHSplit.Name = "toolStripBtnHSplit";
            this.toolStripBtnHSplit.Size = new System.Drawing.Size(28, 30);
            this.toolStripBtnHSplit.Text = "左右布局";
            this.toolStripBtnHSplit.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnHSplit.Click += new System.EventHandler(this.toolStripButton_ItemClicked);
            // 
            // toolStripBtnParamSetting
            // 
            this.toolStripBtnParamSetting.AutoSize = false;
            this.toolStripBtnParamSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnParamSetting.Image = global::Vision_System.Properties.Resources.display_setting;
            this.toolStripBtnParamSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnParamSetting.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnParamSetting.Name = "toolStripBtnParamSetting";
            this.toolStripBtnParamSetting.Size = new System.Drawing.Size(28, 30);
            this.toolStripBtnParamSetting.Text = "参数设置";
            this.toolStripBtnParamSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnParamSetting.Click += new System.EventHandler(this.toolStripButton_ItemClicked);
            // 
            // toolStripBtnReset
            // 
            this.toolStripBtnReset.AutoSize = false;
            this.toolStripBtnReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnReset.Image = global::Vision_System.Properties.Resources.clear;
            this.toolStripBtnReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnReset.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnReset.Name = "toolStripBtnReset";
            this.toolStripBtnReset.Size = new System.Drawing.Size(28, 30);
            this.toolStripBtnReset.Text = "复位";
            this.toolStripBtnReset.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnReset.Click += new System.EventHandler(this.toolStripButton_ItemClicked);
            // 
            // toolStripBtnInfo
            // 
            this.toolStripBtnInfo.AutoSize = false;
            this.toolStripBtnInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnInfo.Image = global::Vision_System.Properties.Resources.information;
            this.toolStripBtnInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnInfo.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnInfo.Name = "toolStripBtnInfo";
            this.toolStripBtnInfo.Size = new System.Drawing.Size(28, 30);
            this.toolStripBtnInfo.Text = "数据";
            this.toolStripBtnInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripBtnInfo.Visible = false;
            this.toolStripBtnInfo.Click += new System.EventHandler(this.toolStripButton_ItemClicked);
            // 
            // cogDisplayStatusBar
            // 
            this.cogDisplayStatusBar.CoordinateSpaceName = "*\\#";
            this.cogDisplayStatusBar.CoordinateSpaceName3D = "*\\#";
            this.cogDisplayStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cogDisplayStatusBar.Location = new System.Drawing.Point(0, 550);
            this.cogDisplayStatusBar.Margin = new System.Windows.Forms.Padding(0);
            this.cogDisplayStatusBar.Name = "cogDisplayStatusBar";
            this.cogDisplayStatusBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cogDisplayStatusBar.Size = new System.Drawing.Size(691, 22);
            this.cogDisplayStatusBar.TabIndex = 0;
            this.cogDisplayStatusBar.Use3DCoordinateSpaceTree = false;
            // 
            // bgwCCDLive
            // 
            this.bgwCCDLive.WorkerReportsProgress = true;
            this.bgwCCDLive.WorkerSupportsCancellation = true;
            this.bgwCCDLive.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgW_Live_CCD_DoWork);
            this.bgwCCDLive.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BgW_Live_CCD_ProgressChanged);
            this.bgwCCDLive.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgW_Live_CCD_RunWorkerCompleted);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.cogRecordDisplay, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.chartData, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.dgvDataResultView, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(685, 514);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.pictureBox20);
            this.panel1.Controls.Add(this.pictureBox19);
            this.panel1.Controls.Add(this.pictureBox10);
            this.panel1.Controls.Add(this.pictureBox18);
            this.panel1.Controls.Add(this.pictureBox9);
            this.panel1.Controls.Add(this.pictureBox17);
            this.panel1.Controls.Add(this.pictureBox8);
            this.panel1.Controls.Add(this.pictureBox16);
            this.panel1.Controls.Add(this.pictureBox7);
            this.panel1.Controls.Add(this.pictureBox15);
            this.panel1.Controls.Add(this.pictureBox6);
            this.panel1.Controls.Add(this.pictureBox14);
            this.panel1.Controls.Add(this.pictureBox5);
            this.panel1.Controls.Add(this.pictureBox13);
            this.panel1.Controls.Add(this.pictureBox4);
            this.panel1.Controls.Add(this.pictureBox12);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.pictureBox11);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pictureBox100);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 247);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(685, 20);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblRunData);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(465, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(220, 20);
            this.panel2.TabIndex = 12;
            // 
            // lblRunData
            // 
            this.lblRunData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRunData.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRunData.Location = new System.Drawing.Point(0, 0);
            this.lblRunData.Name = "lblRunData";
            this.lblRunData.Size = new System.Drawing.Size(220, 20);
            this.lblRunData.TabIndex = 10;
            this.lblRunData.Text = "Total:";
            this.lblRunData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox20
            // 
            this.pictureBox20.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox20.Image")));
            this.pictureBox20.Location = new System.Drawing.Point(547, 2);
            this.pictureBox20.Name = "pictureBox20";
            this.pictureBox20.Size = new System.Drawing.Size(20, 20);
            this.pictureBox20.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox20.TabIndex = 8;
            this.pictureBox20.TabStop = false;
            // 
            // pictureBox19
            // 
            this.pictureBox19.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox19.Image")));
            this.pictureBox19.Location = new System.Drawing.Point(520, 2);
            this.pictureBox19.Name = "pictureBox19";
            this.pictureBox19.Size = new System.Drawing.Size(20, 20);
            this.pictureBox19.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox19.TabIndex = 7;
            this.pictureBox19.TabStop = false;
            // 
            // pictureBox10
            // 
            this.pictureBox10.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox10.Image")));
            this.pictureBox10.Location = new System.Drawing.Point(277, 2);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(20, 20);
            this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox10.TabIndex = 8;
            this.pictureBox10.TabStop = false;
            // 
            // pictureBox18
            // 
            this.pictureBox18.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox18.Image")));
            this.pictureBox18.Location = new System.Drawing.Point(493, 2);
            this.pictureBox18.Name = "pictureBox18";
            this.pictureBox18.Size = new System.Drawing.Size(20, 20);
            this.pictureBox18.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox18.TabIndex = 8;
            this.pictureBox18.TabStop = false;
            // 
            // pictureBox9
            // 
            this.pictureBox9.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox9.Image")));
            this.pictureBox9.Location = new System.Drawing.Point(250, 2);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(20, 20);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox9.TabIndex = 7;
            this.pictureBox9.TabStop = false;
            // 
            // pictureBox17
            // 
            this.pictureBox17.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox17.Image")));
            this.pictureBox17.Location = new System.Drawing.Point(466, 2);
            this.pictureBox17.Name = "pictureBox17";
            this.pictureBox17.Size = new System.Drawing.Size(20, 20);
            this.pictureBox17.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox17.TabIndex = 7;
            this.pictureBox17.TabStop = false;
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox8.Image")));
            this.pictureBox8.Location = new System.Drawing.Point(223, 2);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(20, 20);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox8.TabIndex = 8;
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox16
            // 
            this.pictureBox16.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox16.Image")));
            this.pictureBox16.Location = new System.Drawing.Point(439, 2);
            this.pictureBox16.Name = "pictureBox16";
            this.pictureBox16.Size = new System.Drawing.Size(20, 20);
            this.pictureBox16.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox16.TabIndex = 6;
            this.pictureBox16.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(196, 2);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(20, 20);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox7.TabIndex = 7;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox15
            // 
            this.pictureBox15.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox15.Image")));
            this.pictureBox15.Location = new System.Drawing.Point(412, 2);
            this.pictureBox15.Name = "pictureBox15";
            this.pictureBox15.Size = new System.Drawing.Size(20, 20);
            this.pictureBox15.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox15.TabIndex = 5;
            this.pictureBox15.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(169, 2);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(20, 20);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox6.TabIndex = 6;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox14
            // 
            this.pictureBox14.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox14.Image")));
            this.pictureBox14.Location = new System.Drawing.Point(385, 2);
            this.pictureBox14.Name = "pictureBox14";
            this.pictureBox14.Size = new System.Drawing.Size(20, 20);
            this.pictureBox14.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox14.TabIndex = 4;
            this.pictureBox14.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(142, 2);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(20, 20);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 5;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox13
            // 
            this.pictureBox13.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox13.Image")));
            this.pictureBox13.Location = new System.Drawing.Point(358, 2);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(20, 20);
            this.pictureBox13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox13.TabIndex = 3;
            this.pictureBox13.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(115, 2);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(20, 20);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 4;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox12
            // 
            this.pictureBox12.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox12.Image")));
            this.pictureBox12.Location = new System.Drawing.Point(331, 2);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(20, 20);
            this.pictureBox12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox12.TabIndex = 2;
            this.pictureBox12.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(88, 2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(20, 20);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox11
            // 
            this.pictureBox11.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox11.Image")));
            this.pictureBox11.Location = new System.Drawing.Point(304, 2);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(20, 20);
            this.pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox11.TabIndex = 1;
            this.pictureBox11.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(61, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(34, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox100
            // 
            this.pictureBox100.Image = global::Vision_System.Properties.Resources.arrow_right;
            this.pictureBox100.Location = new System.Drawing.Point(3, 3);
            this.pictureBox100.Name = "pictureBox100";
            this.pictureBox100.Size = new System.Drawing.Size(30, 20);
            this.pictureBox100.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox100.TabIndex = 0;
            this.pictureBox100.TabStop = false;
            // 
            // chartData
            // 
            this.chartData.BackColor = System.Drawing.SystemColors.Control;
            chartArea2.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea2.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea2.AxisX.IsLabelAutoFit = false;
            chartArea2.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisX.LabelStyle.IsEndLabelVisible = false;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisX.MaximumAutoSize = 15F;
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chartArea2.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea2.AxisY.IsLabelAutoFit = false;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.BackColor = System.Drawing.SystemColors.ControlLight;
            chartArea2.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea2.Name = "ChartArea1";
            this.chartData.ChartAreas.Add(chartArea2);
            this.chartData.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.chartData.Legends.Add(legend2);
            this.chartData.Location = new System.Drawing.Point(0, 267);
            this.chartData.Margin = new System.Windows.Forms.Padding(0);
            this.chartData.Name = "chartData";
            this.chartData.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            this.chartData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chartData.Size = new System.Drawing.Size(685, 98);
            this.chartData.TabIndex = 3;
            this.chartData.Text = "chartData";
            // 
            // dgvDataResultView
            // 
            this.dgvDataResultView.AllowUserToAddRows = false;
            this.dgvDataResultView.AllowUserToDeleteRows = false;
            this.dgvDataResultView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDataResultView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDataResultView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataResultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDataResultView.Location = new System.Drawing.Point(3, 368);
            this.dgvDataResultView.Name = "dgvDataResultView";
            this.dgvDataResultView.RowHeadersVisible = false;
            this.dgvDataResultView.RowTemplate.Height = 24;
            this.dgvDataResultView.Size = new System.Drawing.Size(679, 143);
            this.dgvDataResultView.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(691, 520);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // CogDisplayCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cogDisplayStatusBar);
            this.Controls.Add(this.toolStrip);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CogDisplayCtl";
            this.Size = new System.Drawing.Size(691, 572);
            this.Load += new System.EventHandler(this.CogDisplayCtl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox100)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataResultView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.CogRecordDisplay cogRecordDisplay;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripBtnLive;
        private Cognex.VisionPro.CogDisplayStatusBarV2 cogDisplayStatusBar;
        private System.Windows.Forms.ToolStripButton toolStripBtnImportImage;
        private System.Windows.Forms.ToolStripButton toolStripBtnManualTrigger;
        private System.Windows.Forms.ToolStripButton toolStripBtnSaveImage;
        private System.Windows.Forms.ToolStripButton toolStripBtnParamSetting;
        internal System.ComponentModel.BackgroundWorker bgwCCDLive;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox100;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartData;
        private System.Windows.Forms.ToolStripButton toolStripBtnShowChart;
        private System.Windows.Forms.DataGridView dgvDataResultView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripButton toolStripBtnHSplit;
        private System.Windows.Forms.ToolStripButton toolStripBtnVSplit;
        private System.Windows.Forms.ToolStripButton toolStripBtnReset;
        private System.Windows.Forms.PictureBox pictureBox10;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.PictureBox pictureBox20;
        private System.Windows.Forms.PictureBox pictureBox19;
        private System.Windows.Forms.PictureBox pictureBox18;
        private System.Windows.Forms.PictureBox pictureBox17;
        private System.Windows.Forms.PictureBox pictureBox16;
        private System.Windows.Forms.PictureBox pictureBox15;
        private System.Windows.Forms.PictureBox pictureBox14;
        private System.Windows.Forms.PictureBox pictureBox13;
        private System.Windows.Forms.PictureBox pictureBox12;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.Label lblRunData;
        private System.Windows.Forms.ToolStripButton toolStripBtnInfo;
        private System.Windows.Forms.Panel panel2;
    }
}
