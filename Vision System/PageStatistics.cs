using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Vision_System
{
    public partial class PageStatistics : UserControl
    {
        private Chart[] chart_FailureMode;
        private Dictionary<string, int>[] failureData;

        public PageStatistics()
        {
            InitializeComponent();
        }

        private void PageStatistics_Load(object sender, EventArgs e)
        {
            InitializeFailureModeChart();
            InitializeLayout();
        }

        /// <summary>
        /// 初始化 failure mode chart
        /// </summary>
        private void InitializeFailureModeChart()
        {
            chart_FailureMode = new Chart[FormMain.camNumber];
            failureData = new Dictionary<string, int>[FormMain.camNumber];
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                chart_FailureMode[i] = new Chart();
                failureData[i] = new Dictionary<string, int>();
                // 初始化failureData
                for (int j = 0; j < FormMain.jobHelper[i].FailuremodeKeyWd.Count; j++)
                {
                    failureData[i].Add(FormMain.jobHelper[i].FailuremodeKeyWd[j], 
                        FormMain.jobHelper[i].FailCountForKeyWd[j]);
                }

                ChartArea chartArea1 = new ChartArea();
                Legend legend1 = new Legend();
                Series series1 = new Series();
                Title title1 = new Title();
                chartArea1.AxisX.IsLabelAutoFit = false;
                chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font(
                    "Microsoft Sans Serif", 
                    7.8F, 
                    System.Drawing.FontStyle.Regular, 
                    System.Drawing.GraphicsUnit.Point, 
                    ((byte)(0)));
                chartArea1.AxisX.MajorGrid.Enabled = false;
                chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
                chartArea1.Name = "ChartArea1";
                chart_FailureMode[i].ChartAreas.Add(chartArea1);
                chart_FailureMode[i].Dock = System.Windows.Forms.DockStyle.Fill;
                legend1.Name = "Legend1";
                legend1.Alignment = System.Drawing.StringAlignment.Center;
                legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
                title1.Name = "Title1";
                title1.Text = FormMain.jobHelper[i].CcdName + "失效分布统计图";
                title1.BackColor = System.Drawing.Color.LightGray;
                title1.Font = new System.Drawing.Font("Microsoft YaHei", 
                    10.2F, 
                    System.Drawing.FontStyle.Bold, 
                    System.Drawing.GraphicsUnit.Point, 
                    ((byte)(134)));
                title1.Name = "Title1";
                chart_FailureMode[i].Titles.Add(title1);
                chart_FailureMode[i].Legends.Add(legend1);
                chart_FailureMode[i].Location = new System.Drawing.Point(3, 3);
                chart_FailureMode[i].Name = "chart_FailureMode" + i;
                series1.ChartArea = "ChartArea1";
                series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                series1.IsValueShownAsLabel = true;
                series1.Legend = "Legend1";
                series1.Name = "Series1";
                series1.YValuesPerPoint = 4;
                series1.Points.DataBindXY(failureData[i].Keys, failureData[i].Values);
                series1["PieLabelStyle"] = "Outside"; //将文字移到外侧
                series1["PieLineColor"] = "Black"; //绘制黑色的连线
                series1.Label = "#VALX: #PERCENT";
                series1.ToolTip = "失效数量: #VAL"; //显示提示用语
                series1.LegendText = "#VALX";
                //series1.Font = new System.Drawing.Font("Microsoft YaHei", 
                //    7.8F, 
                //    System.Drawing.FontStyle.Regular, 
                //    System.Drawing.GraphicsUnit.Point, 
                //    ((byte)(0)));
                chart_FailureMode[i].Series.Add(series1);
                chart_FailureMode[i].Size = new System.Drawing.Size(446, 316);
                chart_FailureMode[i].TabIndex = 0;
                chart_FailureMode[i].Text = "chart" + (i + 1);
            }
        }

        /// <summary>
        /// 根据相机数量初始化页面布局
        /// </summary>
        private void InitializeLayout()
        {
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
                        System.Windows.Forms.SizeType.Percent, 100 / (float)columncnt));
                }
                for (int i = 0; i < rowcnt; i++)
                {
                    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(
                        System.Windows.Forms.SizeType.Percent, 100 / (float)rowcnt));
                }
                for (int i = 0; i < FormMain.camNumber; i++)
                {
                    if (i < columncnt)
                        this.tableLayoutPanel1.Controls.Add(this.chart_FailureMode[i], i, 0);
                    else this.tableLayoutPanel1.Controls.Add(this.chart_FailureMode[i], i - columncnt, 1);
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
                        System.Windows.Forms.SizeType.Percent, 100 / (float)columncnt));
                }
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Percent, 100F));

                for (int i = 0; i < FormMain.camNumber; i++)
                {
                    this.tableLayoutPanel1.Controls.Add(this.chart_FailureMode[i], i, 0);
                }
            }
        }
    }
}
