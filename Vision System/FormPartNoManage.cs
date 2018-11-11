using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision_System
{
    public partial class FormPartNoManage : Form
    {
        public FormPartNoManage()
        {
            InitializeComponent();
        }

        private void FormProductManage_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();
        }

        /// <summary>
        /// 初始化总ToolBlock的DataGridView：初始化Column以及样式
        /// </summary>
        private void InitializeDataGridView()
        {
            // 清空现有的列表
            dgvProductList.Columns.Clear();
            // 重新初始化列
            DataGridViewCheckBoxColumn column1 = new DataGridViewCheckBoxColumn();
            column1.Name = "Column1";
            column1.HeaderText = "";
            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.Name = "Column2";
            column2.HeaderText = "序号";
            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.Name = "Column3";
            column3.HeaderText = "成品料号";
            DataGridViewTextBoxColumn[] columns = new DataGridViewTextBoxColumn[FormMain.camNumber];
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                // 类数组一定要对数组每个成员进行初始化
                columns[i] = new DataGridViewTextBoxColumn();
                columns[i].Name = "Column" + (i + 4);
                columns[i].HeaderText = "CCD" + (i + 1) + "料号";
            }
            // 添加列
            dgvProductList.Columns.Add(column1);
            dgvProductList.Columns.Add(column2);
            dgvProductList.Columns.Add(column3);
            dgvProductList.Columns.AddRange(columns);
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
            dgvProductList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dgvProductList.RowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvProductList.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dgvProductList.GridColor = System.Drawing.SystemColors.ControlLight;
            // 设置表格交替样式
            dgvProductList.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // 设置表格的宽度
            dgvProductList.Columns[0].Width = 30;
            dgvProductList.Columns[1].Width = 80;
            dgvProductList.Columns[2].Width = 120;
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                dgvProductList.Columns["Column" + (i + 4)].Width = (dgvProductList.Width - 230) / FormMain.camNumber - 2;
            }

            // 设置单元格的值为"Y", 对应的checkbox为true, 单元格值为"N", 对应的checkbox为false
            ((DataGridViewCheckBoxColumn)dgvProductList.Columns[0]).TrueValue = "Y";
            ((DataGridViewCheckBoxColumn)dgvProductList.Columns[0]).FalseValue = "N";

            // 初始化表格数据
            try
            {
                dgvProductList.Rows.Clear();
                foreach (DataRow dr in FormMain.settingHelper.DataTablePartNoInfo.Rows)
                {
                    object[] newRow = new object[FormMain.camNumber + 3];
                    newRow[0] = "N";
                    newRow[1] = dr["PNIndex"].ToString();
                    newRow[2] = dr["PNName"].ToString();
                    for (int i = 0; i < FormMain.camNumber; i++)
                    {
                        newRow[i + 3] = dr["PNCCD" + (i + 1)].ToString();
                    }
                    dgvProductList.Rows.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("表格创建失败!");
            }
        }

        /// <summary>
        /// 多个按钮的触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMultiple_Click(object sender, EventArgs e)
        {
            PictureBox btn = sender as PictureBox;
            // 获取打勾选项的列表
            List<int> checkedIndex = new List<int>();
            string strName, strTemp;
            DataRow row;
            bool isFinishGoodPN = true;
            int ccdIndex = 0;
            switch (btn.Name)
            {
                case "btnAddNew":
                    FormPartNoAdd frmAdd = new FormPartNoAdd(FormMain.settingHelper.PartNoList, FormMain.camNumber);
                    frmAdd.Text = "输入新增料号名称";
                    frmAdd.ShowDialog();
                    if (frmAdd.IsConfirmed)
                    {
                        isFinishGoodPN = frmAdd.IsFinishGoodPartNo;
                        // 如果添加的是成品料号
                        if (isFinishGoodPN)
                        {
                            strName = frmAdd.strProductName;
                            frmAdd.Dispose();
                            row = FormMain.settingHelper.DataTablePartNoInfo.NewRow();
                            int cnt = FormMain.settingHelper.DataTablePartNoInfo.Rows.Count;
                            row[0] = (int)FormMain.settingHelper.DataTablePartNoInfo.Rows[cnt - 1]["PNIndex"] + 1;
                            row[1] = strName;
                            for (int i = 0; i < FormMain.camNumber; i++)
                            {
                                // 新增料号的配置参照料号序号为0的产品，如果0号产品有子料号，则拷贝同样的料号，如果没有，则为NA
                                strTemp = FormMain.settingHelper.DataTablePartNoInfo.Rows[0]["PNCCD" + (i + 1)].ToString();
                                // 如果为NA，该工位配方不共享，新增料号也按照这个格式进行配置
                                if (strTemp == "NA")
                                {
                                    row[i + 2] = "NA";
                                }
                                else
                                {
                                    row[i + 2] = strTemp;
                                }
                            }
                            FormMain.settingHelper.DataTablePartNoInfo.Rows.Add(row);
                            FormMain.settingHelper.PartNoList.Add(strName);

                            SavePartNoToConfigFile(FormMain.settingHelper.DataTablePartNoInfo,
                                FormMain.settingHelper.MaximumProductNoNum);

                            // 重新初始化表格数据
                            UpdateDataGridView(FormMain.settingHelper.DataTablePartNoInfo);

                            // 拷贝模板程序到新建料号文件夹
                            string strSourceFileAcq, strTargetFileAcq;
                            string strSourceFileJob, strTargetFileJob;
                            string strFolderPath;
                            try
                            {
                                // 为新的料号新建空白文件夹
                                strFolderPath = FormMain.strBaseDirectory + "Recipe" +
                                        "\\" + FormMain.settingHelper.PartNoList.Last();
                                if (!Directory.Exists(strFolderPath))
                                {
                                    Directory.CreateDirectory(strFolderPath);
                                }
                                // 拷贝模板Vpp文件到新料号文件夹
                                for (int i = 0; i < FormMain.camNumber; i++)
                                {
                                    // 相机设定vpp文件
                                    strSourceFileAcq = FormMain.strTemplateAcqFifoPath;
                                    strTargetFileAcq = strFolderPath + "\\" +
                                        "AcqFifo_CCD" + (i + 1) + ".vpp";
                                    // 图像处理vpp文件
                                    strSourceFileJob = FormMain.strTemplateJobPath;
                                    strTargetFileJob = strFolderPath + "\\" +
                                        "Job_CCD" + (i + 1) + ".vpp";
                                    File.Copy(strSourceFileAcq, strTargetFileAcq, true);
                                    File.Copy(strSourceFileJob, strTargetFileJob, true);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                        // 如果添加的是单个CCD的子料号
                        else
                        {
                            strName = frmAdd.strProductName;
                            ccdIndex = frmAdd.CcdIndex;
                            // 回收窗口
                            frmAdd.Dispose();
                            FormMain.jobHelper[ccdIndex].SingleCCDPartNoList.Add(strName);
                            SaveSingleCCDPartNoToConfigFile(ccdIndex, FormMain.jobHelper[ccdIndex].SingleCCDPartNoList);

                            // 重新初始化表格数据
                            UpdateDataGridView(FormMain.settingHelper.DataTablePartNoInfo);

                            // 拷贝模板程序到新建料号文件夹
                            string strSourceFileAcq, strTargetFileAcq;
                            string strSourceFileJob, strTargetFileJob;
                            string strFolderPath;
                            try
                            {
                                // 为新的料号新建空白文件夹
                                strFolderPath = FormMain.strBaseDirectory + "Recipe" + "\\" + "CCD" + (ccdIndex + 1) + " Share";
                                if (!Directory.Exists(strFolderPath))
                                {
                                    Directory.CreateDirectory(strFolderPath);
                                }
                                // 拷贝模板Vpp文件到新料号文件夹
                                for (int i = 0; i < FormMain.camNumber; i++)
                                {
                                    // 相机设定vpp文件
                                    strSourceFileAcq = FormMain.strTemplateAcqFifoPath;
                                    strTargetFileAcq = strFolderPath + "\\" + "AcqFifo_" +
                                        FormMain.jobHelper[ccdIndex].SingleCCDPartNoList.Last() + ".vpp";
                                    // 图像处理vpp文件
                                    strSourceFileJob = FormMain.strTemplateJobPath;
                                    strTargetFileJob = strFolderPath + "\\" + "Job_" +
                                        FormMain.jobHelper[ccdIndex].SingleCCDPartNoList.Last() + ".vpp";
                                    File.Copy(strSourceFileAcq, strTargetFileAcq, true);
                                    File.Copy(strSourceFileJob, strTargetFileJob, true);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    break;
                case "btnDelete":
                    for (int i = 0; i < dgvProductList.Rows.Count; i++)
                    {
                        if ((bool)dgvProductList.Rows[i].Cells[0].EditedFormattedValue)
                        {
                            checkedIndex.Add(i);
                        }
                    }

                    // 如果勾选的选项数量为0
                    if (checkedIndex.Count == 0)
                    {
                        MessageBox.Show("没有勾选项目, 请重新选择！");
                        return;
                    }
                    else
                    {
                        // 如果勾选的项目是目前正在生产的料号，提示不允许删除
                        foreach (int item in checkedIndex)
                        {
                            if (dgvProductList.Rows[item].Cells[2].Value.ToString() == FormMain.strSelectedPartNoName)
                            {
                                MessageBox.Show("你选择的料号正在生产，不允许删除！");
                                return;
                            }
                        }

                        // 如果不是正在生产的料号，提示是否确认删除，删除之后无法恢复，有风险
                        if (MessageBox.Show("即将删除料号和对应的Vpp文件，确认删除吗？",
                            "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            // 从List中删除选中的项目对应的料号
                            for (int i = 0; i < checkedIndex.Count; i++)
                            {
                                // 删除对应的Vpp文件夹
                                string strFolderPath;
                                strName = FormMain.settingHelper.PartNoList[checkedIndex[checkedIndex.Count - i - 1]];
                                strFolderPath = FormMain.strBaseDirectory + "Recipe" +
                                                                    "\\" + strName;
                                Directory.Delete(strFolderPath, true);

                                // 从后往前删除，避免index重新计算
                                FormMain.settingHelper.PartNoList.RemoveAt(checkedIndex[checkedIndex.Count - i - 1]);
                                FormMain.settingHelper.DataTablePartNoInfo.Rows.RemoveAt(checkedIndex[checkedIndex.Count - i - 1]);
                            }

                            // 保存到配置文件中
                            SavePartNoToConfigFile(FormMain.settingHelper.DataTablePartNoInfo, FormMain.camNumber);

                            // 重新初始化表格数据
                            UpdateDataGridView(FormMain.settingHelper.DataTablePartNoInfo);
                        }
                    }
                    break;
                case "btnCopy":
                    for (int i = 0; i < dgvProductList.Rows.Count; i++)
                    {
                        if ((bool)dgvProductList.Rows[i].Cells[0].EditedFormattedValue)
                        {
                            checkedIndex.Add(i);
                        }
                    }
                    // 如果勾选的选项数量超过1
                    if (checkedIndex.Count == 0)
                    {
                        MessageBox.Show("没有勾选, 请重新选择!");
                        return;
                    }
                    else if (checkedIndex.Count > 1)
                    {
                        MessageBox.Show("勾选了超过一个, 请重新选择!");
                        return;
                    }
                    else
                    {
                        int selectedIndex = checkedIndex[0];
                        // 如果勾选的选项数量为1，打开对话框输入新料号的名字
                        FormPartNoCopy frmCopy = new FormPartNoCopy(FormMain.settingHelper.PartNoList.Count,
                            FormMain.settingHelper.PartNoList);
                        frmCopy.Text = "输入拷贝料号名称";
                        frmCopy.ShowDialog();
                        // 如果点击的是"确认按钮
                        if (frmCopy.IsConfirmed)
                        {
                            strName = frmCopy.strProductName;
                            FormMain.settingHelper.PartNoList.Add(strName);

                            row = FormMain.settingHelper.DataTablePartNoInfo.NewRow();
                            row[0] = FormMain.settingHelper.DataTablePartNoInfo.Rows.Count + 1;
                            row[1] = strName;
                            for (int i = 0; i < FormMain.camNumber; i++)
                            {
                                // 新增料号的配置参照被拷贝料号，如果有子料号，则拷贝同样的料号，如果没有，则为NA
                                strTemp = FormMain.settingHelper.DataTablePartNoInfo.Rows[selectedIndex]["PNCCD" + (i + 1)].ToString();
                                // 如果为NA，该工位配方不共享，新增料号也按照这个格式进行配置
                                if (strTemp == "NA")
                                {
                                    row[i + 2] = "NA";
                                }
                                else
                                {
                                    row[i + 2] = strTemp;
                                }
                            }
                            FormMain.settingHelper.DataTablePartNoInfo.Rows.Add(row);
                            SavePartNoToConfigFile(FormMain.settingHelper.DataTablePartNoInfo, FormMain.camNumber);

                            // 重新初始化表格数据
                            UpdateDataGridView(FormMain.settingHelper.DataTablePartNoInfo);

                            // 拷贝现有程序到新建料号文件夹
                            string targetFolder;
                            string sourceFolder;
                            string[] filesPathArray;
                            try
                            {
                                // 为新的料号新建空白文件夹
                                sourceFolder = FormMain.strBaseDirectory + "Recipe" +
                                        "\\" + FormMain.settingHelper.PartNoList[selectedIndex];
                                targetFolder = FormMain.strBaseDirectory + "Recipe" +
                                        "\\" + FormMain.settingHelper.PartNoList.Last();
                                if (!Directory.Exists(targetFolder))
                                {
                                    Directory.CreateDirectory(targetFolder);
                                }

                                // 拷贝现有料号Vpp文件到新料号文件夹
                                filesPathArray = Directory.GetFiles(sourceFolder);
                                for (int i = 0; i < filesPathArray.Length; i++)
                                {
                                    FileInfo fi = new FileInfo(filesPathArray[i]);
                                    string extension = fi.Extension.ToLower();
                                    if (extension == ".vpp")
                                    {
                                        File.Copy(filesPathArray[i], targetFolder + "\\" + fi.Name, true);
                                    }
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                        // 回收
                        frmCopy.Dispose();
                    }
                    break;
                case "btnEdit":
                    for (int i = 0; i < dgvProductList.Rows.Count; i++)
                    {
                        if ((bool)dgvProductList.Rows[i].Cells[0].EditedFormattedValue)
                        {
                            checkedIndex.Add(i);
                        }
                    }
                    // 如果勾选的选项数量超过1
                    if (checkedIndex.Count == 0)
                    {
                        MessageBox.Show("没有勾选, 请重新选择!");
                        return;
                    }
                    else if (checkedIndex.Count > 1)
                    {
                        MessageBox.Show("勾选了超过一个, 请重新选择！");
                        return;
                    }
                    else
                    {
                        int selectedIndex = checkedIndex[0];
                        List<string>[] ccdPartNoList = new List<string>[FormMain.camNumber];
                        for (int i = 0; i < FormMain.camNumber; i++)
                        {
                            ccdPartNoList[i] = FormMain.jobHelper[i].SingleCCDPartNoList;
                        }
                        // 如果勾选的选项数量为1，打开对话框输入新料号的名字
                        FormPartNoEdit frmEdit = new FormPartNoEdit(FormMain.settingHelper.DataTablePartNoInfo.Rows[selectedIndex],
                            FormMain.settingHelper.PartNoList,
                            ccdPartNoList,
                            FormMain.camNumber,
                            FormMain.settingHelper.PartNoList[selectedIndex]);
                        frmEdit.ShowDialog();
                        // 如果点击的是"确认按钮
                        if (frmEdit.IsConfirmed)
                        {
                            strName = frmEdit.strProductName;
                            // 重命名对应的Vpp文件夹
                            string oldName, newName;
                            oldName = FormMain.strBaseDirectory + "Recipe" + "\\" + 
                                FormMain.settingHelper.PartNoList[selectedIndex];
                            newName = FormMain.strBaseDirectory + "Recipe" + "\\" + strName;
                            // 如果文件夹路径相同，不执行文件夹移动操作
                            if (oldName != newName)
                            {
                                Directory.Move(oldName, newName);
                            }

                            // 更新PartNoList
                            FormMain.settingHelper.PartNoList[selectedIndex] = (string)frmEdit.DataRowEdit["PNName"];
                            // 更新DataTablePartNoInfo
                            FormMain.settingHelper.DataTablePartNoInfo.Rows[selectedIndex]["PNName"] = frmEdit.DataRowEdit["PNName"];
                            for (int i = 0; i < FormMain.camNumber; i++)
                            {
                                FormMain.settingHelper.DataTablePartNoInfo.Rows[selectedIndex]["PNCCD" + (i + 1)] =
                                    frmEdit.DataRowEdit["PNCCD" + (i + 1)];
                            }
                            // 保存到配置文件中
                            SavePartNoToConfigFile(FormMain.settingHelper.DataTablePartNoInfo, FormMain.camNumber);

                            // 重新初始化表格数据
                            UpdateDataGridView(FormMain.settingHelper.DataTablePartNoInfo);
                        }
                        // 回收
                        frmEdit.Dispose();
                    }
                    break;
                default:
                    break;
            } 
        }

        private void UpdateDataGridView(DataTable table)
        {
            // 初始化表格数据
            try
            {
                dgvProductList.Rows.Clear();
                foreach (DataRow dr in table.Rows)
                {
                    object[] newRow = new object[FormMain.camNumber + 3];
                    newRow[0] = "N";
                    newRow[1] = dr["PNIndex"].ToString();
                    newRow[2] = dr["PNName"].ToString();
                    for (int i = 0; i < FormMain.camNumber; i++)
                    {
                        newRow[i + 3] = dr["PNCCD" + (i + 1)].ToString();
                    }
                    dgvProductList.Rows.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("表格创建失败!");
            }
        }

        /// <summary>
        /// 将product list写入配置文件中
        /// </summary>
        /// <param name="list"></param>
        private void SavePartNoToConfigFile(DataTable table, int maxNum)
        {
            DataRow[] dataRows;
            StringBuilder sb = new StringBuilder(200);
            try
            {
                for (int i = 0; i < maxNum; i++)
                {
                    sb.Clear();
                    dataRows = table.Select("PNIndex = " + (i + 1));
                    if (dataRows.Length >= 1)
                    {
                        sb.Append(dataRows[0]["PNName"]);
                        sb.Append(":");
                        for (int j = 0; j < table.Columns.Count - 2; j++)
                        {
                            sb.Append(dataRows[0]["PNCCD" + (j + 1)]);
                            sb.Append(",");
                        }
                        sb.Remove(sb.Length - 1, 1);
                        FormMain.mainConfigIniFile.IniWriteValue("PartNo", (i + 1).ToString(), sb.ToString());
                    }
                    else
                    {
                        FormMain.mainConfigIniFile.IniWriteValue("PartNo", (i + 1).ToString(), "");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("写入配置文件出错");
            }
        }

        /// <summary>
        /// 将单个CCD的料号列表写入配置文件中
        /// </summary>
        /// <param name="list"></param>
        private void SaveSingleCCDPartNoToConfigFile(int index, List<string> list)
        {
            try
            {
                StringBuilder sb = new StringBuilder(200);
                IniFile ConfigIniFile = new IniFile(FormMain.strCCDConfigFilePath[index]);
                foreach (string item in list)
                {
                    sb.Append(item);
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                ConfigIniFile.IniWriteValue("CCD" + (index + 1), "SingleCCDPNList", sb.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("写入配置文件出错");
            }
        }
    }
}
