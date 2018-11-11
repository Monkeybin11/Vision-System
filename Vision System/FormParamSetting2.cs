using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vision_System
{
    public partial class FormParamSetting2 : Form
    {
        private CogToolBlock _cogToolBlock;
        private int _ccdIndex;
        private int _selectRecordIndex = 0;
        private List<string> listRecordList = new List<string>();
        private TabPage[] tabPage;
        private DataGridView[] dataGridViews;

        public CogToolBlock ToolBlock { get => _cogToolBlock; set => _cogToolBlock = value; }
        public int CCDIndex { get => _ccdIndex; set => _ccdIndex = value; }
        public int SelectRecordIndex { get => _selectRecordIndex; set => _selectRecordIndex = value; }

        public FormParamSetting2(CogToolBlock toolBlock, int index, int recordIndex)
        {
            InitializeComponent();
            ToolBlock = toolBlock;
            CCDIndex = index;
            SelectRecordIndex = recordIndex;
        }

        /// <summary>
        /// 加载界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormParamSetting_Load(object sender, EventArgs e)
        {
            // 初始化标题
            this.Text = "CCD" + (CCDIndex + 1) + "参数调整";

            // 初始化延时时间
            this.txtCCDAcqDelayTime.Text = FormMain.jobHelper[CCDIndex].CCDAcqDelayTimeMilliSecond.ToString();
            this.txtDoOutTime.Text = FormMain.jobHelper[CCDIndex].DoOutTimePeriod.ToString();

            // 初始化触发源列表
            DataRow[] dataRows;
            string strName;
            dataRows = FormMain.iOHelper.DataTableIOInfo.Select("Type = " + 1 + " and Name LIKE 'Trigger%'");
            if (dataRows.Length >= 1)
            {
                foreach (DataRow dr in dataRows)
                {
                    strName = (string)dr["Name"];
                    cmbTriggerSelect.Items.Add(strName);
                    cmbTriggerSelect.Items.Add(strName + "Completed");
                }
            }

            // 选择配置好的触发源
            cmbTriggerSelect.SelectedItem = FormMain.jobHelper[CCDIndex].TriggerSourceName;

            // 初始化光源列表
            dataRows = FormMain.iOHelper.DataTableIOInfo.Select("Type = " + 2 + " and Name LIKE 'Light%'");
            if (dataRows.Length >= 1)
            {
                foreach (DataRow dr in dataRows)
                {
                    strName = (string)dr["Name"];
                    cmbLightSelect.Items.Add(strName);
                }
            }

            // 选择配置好的光源
            cmbLightSelect.SelectedItem = FormMain.jobHelper[CCDIndex].LightSourceName;

            // 光源是否频闪
            if (FormMain.jobHelper[CCDIndex].IsLightFlash)
            {
                radioLightON.Checked = false;
                radioLightFlash.Checked = true;
            }
            else
            {
                radioLightON.Checked = true;
                radioLightFlash.Checked = false;
            }

            try
            {
                InitializeOverallToolBlockInputDataGridView();
                // InitializeParameterTab();
                // 初始化DataGridView：ToolBlock输入参数设置
                //InitializeSingleToolBlockInputDataGridView();
                // 初始化Record列表
                InitializeComboBoxRecordList();
                // 初始化检测工具列表
                TreeNode TopNode = new TreeNode();
                for (int i = 0; i < ToolBlock.Tools.Count; i++)
                {
                    TopNode = treeView1.Nodes.Add(ToolBlock.Tools[i].Name);
                    TopNode.Tag = ToolBlock.Tools[i];
                    AddChildnode((CogToolBlock)ToolBlock.Tools[i], TopNode);
                }
            }
            catch (Exception)
            {
                throw;
            }
            // 默认情况下，展开treeview中的所有工具
            treeView1.ExpandAll();
        }

        /// <summary>
        /// 初始化参数Tab
        /// </summary>
        //private void InitializeParameterTab()
        //{
        //    int toolCnt = ToolBlock.Tools.Count;
        //    tabPage = new TabPage[toolCnt];
        //    dataGridViews = new DataGridView[toolCnt];

        //    this.tabControl1.Controls.Clear();
        //    for (int i = 0; i < toolCnt; i++)
        //    {
        //        dataGridViews[i] = new DataGridView();
        //        DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
        //        DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
        //        DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
        //        // 
        //        // Column1
        //        // 
        //        column1.HeaderText = "变量名";
        //        column1.Name = "Column1";
        //        // 
        //        // Column2
        //        // 
        //        column2.HeaderText = "变量类型";
        //        column2.Name = "Column2";
        //        // 
        //        // Column3
        //        // 
        //        column3.HeaderText = "值";
        //        column3.Name = "Column3";

        //        DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
        //        dataGridViews[i].AllowUserToAddRows = false;
        //        dataGridViews[i].AllowUserToDeleteRows = false;
        //        dataGridViews[i].BorderStyle = System.Windows.Forms.BorderStyle.None;
        //        dataGridViews[i].CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
        //        dataGridViews[i].ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        //        dataGridViews[i].Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
        //            column1,
        //            column2,
        //            column3});
        //        dataGridViews[i].Dock = System.Windows.Forms.DockStyle.Fill;
        //        dataGridViews[i].Location = new System.Drawing.Point(3, 3);
        //        dataGridViews[i].Name = "dgvToolBlockInput" + i;
        //        dataGridViews[i].RowHeadersVisible = false;
        //        dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 
        //            7.8F,
        //            System.Drawing.FontStyle.Regular, 
        //            System.Drawing.GraphicsUnit.Point, 
        //            ((byte)(134)));
        //        dataGridViews[i].CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
        //        dataGridViews[i].BackgroundColor = System.Drawing.SystemColors.ControlLight;
        //        dataGridViews[i].RowsDefaultCellStyle = dataGridViewCellStyle2;
        //        dataGridViews[i].RowTemplate.Height = 24;
        //        dataGridViews[i].Size = new System.Drawing.Size(768, 183);
        //        dataGridViews[i].TabIndex = 100 + i;
        //        // 
        //        // tabPage1
        //        // 
        //        tabPage[i] = new TabPage();
        //        tabPage[i].Controls.Add(dataGridViews[i]);
        //        tabPage[i].Location = new System.Drawing.Point(4, 28);
        //        tabPage[i].Name = "tabPage" + (i + 1);
        //        tabPage[i].Padding = new System.Windows.Forms.Padding(3);
        //        tabPage[i].Size = new System.Drawing.Size(992, 568);
        //        tabPage[i].TabIndex = 0;
        //        tabPage[i].Text = ToolBlock.Tools[i].Name;
        //        tabPage[i].UseVisualStyleBackColor = true;
        //        // 
        //        // tabControl
        //        // 
        //        this.tabControl1.Controls.Add(tabPage[i]);
        //    }
        //}

        /// <summary>
        /// 采用递归的方式添加所有TookBlock工具的节点
        /// </summary>
        /// <param name="block"></param>
        /// <param name="pnode"></param>
        private void AddChildnode(CogToolBlock block, TreeNode pnode)
        {
            ICogTool tool;
            string toolFullType;
            string[] allString;
            string toolType;
            for (int i = 0; i < block.Tools.Count; i++)
            {
                tool = block.Tools[i];
                // cnode为下一级节点
                TreeNode cnode = new TreeNode();
                cnode = pnode.Nodes.Add(tool.Name);
                // 如果不是ToolBlock，说明为单个检测工具，设置Tag为工具，传递该参数
                cnode.Tag = tool;
                // 判断该节点为tool block还是tool
                toolFullType = tool.GetType().ToString();
                allString = toolFullType.Split('.');
                toolType = allString[allString.Length - 1];
                // 如果是ToolBlock，则需要递归，循环下去，找到下一级的ICogTool
                if (toolType == "CogToolBlock")
                {
                    AddChildnode((CogToolBlock)tool, cnode);
                }
            }
        }

        /// <summary>
        /// 当单击每一项时，右边出来对应的参数调整界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string toolFullType;
            string[] allString;
            string toolType;
            TreeNode mCurrentNode = e.Node;
            ICogTool tool = (ICogTool)mCurrentNode.Tag;
            toolFullType = tool.GetType().ToString();
            allString = toolFullType.Split('.');
            toolType = allString[allString.Length - 1];
            switch (toolType)
            {
                case "CogToolBlock":
                    break;
                case "CogPMAlignTool":
                    break;
                case "CogBlobTool":
                    break;
                case "CogCaliperTool":
                    break;
                case "CogImageConvertTool":
                    break;
                case "CogCopyRegionTool":
                    break;
                case "CogIPOneImageTool":
                    break;
                case "CogIPTwoImageSubtractTool":
                    break;
                case "CogCreateLineTool":
                    break;
                case "CogFindCornerTool":
                    break;
                case "CogFindLineTool":
                    break;
                case "CogFindCircleTool":
                    break;
                case "CogHistogramTool":
                    break;
                case "CogResultsAnalysisTool":
                    break;
                case "CogDataAnalysisTool":
                    break;
                case "CogColorExtractorTool":
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 将Run record清单添加到comboBox里面
        /// </summary>
        /// <param name="list"></param>
        /// <param name="select"></param>
        public void InitializeComboBoxRecordList()
        {
            ICogRecords records = ToolBlock.CreateLastRunRecord().SubRecords;
            for (int i = 0; i < records.Count; i++)
            {
                listRecordList.Add(records[i].RecordKey);
            }
            cmbRecordSelect.DataSource = listRecordList;
            cmbRecordSelect.SelectedIndex = SelectRecordIndex;
            cmbRecordSelect.SelectedIndexChanged += new System.EventHandler(this.comBox_SelectRecordIndexChanged);
        }

        /// <summary>
        /// 选择不同的Record显示方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comBox_SelectRecordIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            int newIndex = cmb.SelectedIndex;
        }

        /// <summary>
        /// 初始化总ToolBlock的DataGridView：初始化Column以及样式
        /// </summary>
        private void InitializeOverallToolBlockInputDataGridView()
        {
            // 设置表格样式
            DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            //dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new System.Drawing.Font(
                "Microsoft YaHei", 7.8F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvOverallInput.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dgvOverallInput.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dgvOverallInput.RowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvOverallInput.GridColor = System.Drawing.SystemColors.ControlLight;
            // 设置表格交替样式
            dgvOverallInput.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            
            dgvOverallInput.Columns[0].Width = 100;
            dgvOverallInput.Columns[1].Width = (dgvOverallInput.Width - 100) / 2 - 2;
            dgvOverallInput.Columns[2].Width = (dgvOverallInput.Width - 100) / 2 - 2;
            // 初始化表格数据
            try
            {
                dgvOverallInput.Rows.Clear();
                for (int i = 0; i < ToolBlock.Inputs.Count; i++)
                {
                    object[] dr = new object[3];
                    dr[0] = ToolBlock.Inputs[i].Name;
                    dr[1] = ToolBlock.Inputs[i].ValueType.ToString();
                    dr[2] = ToolBlock.Inputs[i].Value.ToString();
                    dgvOverallInput.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("表格创建失败！");
            }
        }

        /// <summary>
        /// 初始化单个检测ToolBlock的DataGridView：初始化Column以及样式
        /// </summary>
        private void InitializeSingleToolBlockInputDataGridView()
        {
            // 设置表格样式
            InitializeDataGridViewStyle();
            // 初始化表格数据
            InitDatagirdViewData();
        }

        /// <summary>
        /// 初始化DataGridView样式
        /// </summary>
        private void InitializeDataGridViewStyle()
        {
            for (int i = 0; i < ToolBlock.Tools.Count; i++)
            {
                // 设置表格样式
                DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
                //dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                dataGridViewCellStyle1.BackColor = Color.Linen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font(
                    "Microsoft YaHei", 7.8F,
                    System.Drawing.FontStyle.Regular,
                    System.Drawing.GraphicsUnit.Point,
                    ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                dataGridViews[i].RowsDefaultCellStyle = dataGridViewCellStyle1;
                dataGridViews[i].BackgroundColor = System.Drawing.SystemColors.Control;
                dataGridViews[i].GridColor = System.Drawing.SystemColors.ControlLight;
                // 设置表格交替样式
                dataGridViews[i].AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                dataGridViews[i].Columns[0].Width = 100;
                dataGridViews[i].Columns[1].Width = (dataGridViews[i].Width - 100) / 2 - 2;
                dataGridViews[i].Columns[2].Width = (dataGridViews[i].Width - 100) / 2 - 2;
            }
        }

        /// <summary>
        /// 初始化DataGridView内容
        /// </summary>
        public void InitDatagirdViewData()
        {
            for (int i = 0; i < ToolBlock.Tools.Count; i++)
            {
                CogToolBlock toolBlock = (CogToolBlock)ToolBlock.Tools[i];
                try
                {
                    dataGridViews[i].Rows.Clear();
                    for (int j = 0; j < toolBlock.Inputs.Count; j++)
                    {
                        object[] dr = new object[3];
                        dr[0] = toolBlock.Inputs[j].Name;
                        dr[1] = toolBlock.Inputs[j].ValueType.ToString();
                        dr[2] = toolBlock.Inputs[j].Value.ToString();
                        dataGridViews[i].Rows.Add(dr);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("表格创建失败！");
                }
            }
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string inputFullType;
            string[] allString;
            string inputType;
            bool validateSuccess;
            string inputValue;
            SelectRecordIndex =  cmbRecordSelect.SelectedIndex;
            try
            {
                // 总ToolBlock的输入参数更新
                for (int i = 0; i < ToolBlock.Inputs.Count; i++)
                {
                    inputFullType = ToolBlock.Inputs[i].ValueType.ToString();
                    allString = inputFullType.Split('.');
                    inputType = allString[allString.Length - 1];
                    inputValue = dgvOverallInput.Rows[i].Cells[2].Value.ToString();
                    switch (inputType.ToLower())
                    {
                        case "int16":
                            validateSuccess = ValidateUtil.IsValidInt(inputValue);
                            // 如果输入格式不正确
                            if (!validateSuccess)
                            {
                                MessageBox.Show("输入格式不正确，请检查后重新输入！");
                                return;
                            }
                            ToolBlock.Inputs[i].Value = Convert.ToInt16(inputValue);
                            break;
                        case "int32":
                            validateSuccess = ValidateUtil.IsValidInt(inputValue);
                            // 如果输入格式不正确
                            if (!validateSuccess)
                            {
                                MessageBox.Show("输入格式不正确，请检查后重新输入！");
                                return;
                            }
                            ToolBlock.Inputs[i].Value = Convert.ToInt32(inputValue);
                            break;
                        case "double":
                            validateSuccess = ValidateUtil.IsDecimalSign(inputValue);
                            // 如果输入格式不正确
                            if (!validateSuccess)
                            {
                                MessageBox.Show("输入格式不正确，请检查后重新输入！");
                                return;
                            }
                            ToolBlock.Inputs[i].Value = Convert.ToDouble(inputValue);
                            break;
                        case "string":
                            ToolBlock.Inputs[i].Value = inputValue;
                            break;
                        default:
                            break;
                    }
                }

                // 单个失效模式输入参数更新
                /*for (int i = 0; i < ToolBlock.Tools.Count; i++)
                {
                    CogToolBlock toolBlock = (CogToolBlock)ToolBlock.Tools[i];

                    for (int j = 0; j < toolBlock.Inputs.Count; j++)
                    {
                        inputFullType = toolBlock.Inputs[j].ValueType.ToString();
                        allString = inputFullType.Split('.');
                        inputType = allString[allString.Length - 1];
                        inputValue = dataGridViews[i].Rows[j].Cells[2].Value.ToString();
                        switch (inputType)
                        {
                            case "Double":
                                validateSuccess = ValidateUtil.IsDecimalSign(inputValue);
                                // 如果输入格式不正确
                                if (!validateSuccess)
                                {
                                    MessageBox.Show("输入格式不正确，请检查后重新输入！");
                                    return;
                                }
                                toolBlock.Inputs[j].Value = Convert.ToDouble(inputValue);
                                break;
                            case "Int32":
                                validateSuccess = ValidateUtil.IsValidInt(inputValue);
                                // 如果输入格式不正确
                                if (!validateSuccess)
                                {
                                    MessageBox.Show("输入格式不正确，请检查后重新输入！");
                                    return;
                                }
                                toolBlock.Inputs[j].Value = Convert.ToInt32(inputValue);
                                break;
                            case "Int16":
                                validateSuccess = ValidateUtil.IsValidInt(inputValue);
                                // 如果输入格式不正确
                                if (!validateSuccess)
                                {
                                    MessageBox.Show("输入格式不正确，请检查后重新输入！");
                                    return;
                                }
                                toolBlock.Inputs[j].Value = Convert.ToInt16(inputValue);
                                break;
                            default:
                                break;
                        }
                    }
                }*/
                // ToolBlock.Run();
            }
            catch (Exception)
            {
                MessageBox.Show("数据保存失败!");
            }
            this.Close();
        }

        /// <summary>
        /// 保存CCD延时参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMultiple_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            IniFile ConfigIniFile = new IniFile(FormMain.strCCDConfigFilePath[CCDIndex]);
            switch (btn.Name)
            {
                case "btnCCDAcqDelaySave":
                    FormMain.jobHelper[CCDIndex].CCDAcqDelayTimeMilliSecond = Convert.ToInt16(txtCCDAcqDelayTime.Text);
                    FormMain.jobHelper[CCDIndex].DoOutTimePeriod = Convert.ToInt16(txtDoOutTime.Text);
                    // 将相机取像延时时间写入INI文件
                    ConfigIniFile.IniWriteValue(
                        "CCD" + (CCDIndex + 1),
                        "DelayTime",
                        FormMain.jobHelper[CCDIndex].CCDAcqDelayTimeMilliSecond);
                    ConfigIniFile.IniWriteValue("CCD" + (CCDIndex + 1),
                        "DoOutTime",
                        FormMain.jobHelper[CCDIndex].DoOutTimePeriod);
                    break;
                case "btnTriggerSourceSave":
                    string strName;
                    // 触发源名称
                    strName = cmbTriggerSelect.SelectedItem.ToString();
                    FormMain.jobHelper[CCDIndex].TriggerSourceName = strName;

                    // 是否二次触发
                    FormMain.jobHelper[CCDIndex].TriggerMode = strName.Contains("Completed") ?
                        TriggerMode.AttachedTrigger : TriggerMode.FirstOrIndependentTrigger;

                    // 更新触发源列表
                    FormMain.triggerNameAndCCDIndex[FormMain.jobHelper[CCDIndex].TriggerSourceName] = CCDIndex.ToString();

                    // 保存配置文件
                    SaveTriggerNameToConfigFile(CCDIndex);

                    // 更新IO端口Table
                    FormMain.iOHelper.LoadIODefinitionFromDataTable();
                    break;
                case "btnLightSourceSave":
                    // 是否频闪
                    FormMain.jobHelper[CCDIndex].IsLightFlash = radioLightFlash.Checked;
                    ConfigIniFile.IniWriteValue("CCD" + (CCDIndex + 1),
                        "LightFlash",
                        FormMain.jobHelper[CCDIndex].IsLightFlash ? "1" : "0");

                    // 光源名称
                    FormMain.jobHelper[CCDIndex].LightSourceName = cmbLightSelect.SelectedItem.ToString();
                    ConfigIniFile.IniWriteValue("CCD" + (CCDIndex + 1),
                        "LightName",
                        FormMain.jobHelper[CCDIndex].LightSourceName);

                    // 更新IO端口
                    FormMain.iOHelper.LoadIODefinitionFromDataTable();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 保存TriggerName到配置文件
        /// </summary>
        private void SaveTriggerNameToConfigFile(int index)
        {
            string strName;
            string strIndex;
            IniFile ConfigIniFile = new IniFile(FormMain.strCCDConfigFilePath[index]);
            strName = FormMain.jobHelper[index].TriggerSourceName;

            // TriggerName保存到配置文件
            ConfigIniFile.IniWriteValue("CCD" + (index + 1),
                "TriggerName",
                FormMain.jobHelper[index].TriggerSourceName);

            switch (FormMain.jobHelper[index].TriggerMode)
            {
                case TriggerMode.FirstOrIndependentTrigger:
                    FormMain.jobHelper[index].AttachedCCDIndex = index;
                    break;
                case TriggerMode.AttachedTrigger:
                    strIndex = FormMain.triggerNameAndCCDIndex[strName.Remove(strName.Length - 9)];
                    // 判断strIndex，如果为空值，表示没有对应的相机序号
                    if (!string.IsNullOrWhiteSpace(strIndex))
                    {
                        FormMain.jobHelper[index].AttachedCCDIndex = Convert.ToInt32(strIndex);
                    }
                    break;
                default:
                    break;
            }
            // AttachedCCDIndex保存到配置文件
            ConfigIniFile.IniWriteValue("CCD" + (index + 1),
                "AttachedCCDIndex",
                (FormMain.jobHelper[index].AttachedCCDIndex + 1).ToString());
        }
    }
}
