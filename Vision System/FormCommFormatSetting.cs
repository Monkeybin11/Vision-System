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
    public partial class FormCommFormatSetting : Form
    {
        public FormCommFormatSetting()
        {
            InitializeComponent();
        }

        private void FormCommFormatSetting_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();
        }

        /// <summary>
        /// 初始化总ToolBlock的DataGridView：初始化Column以及样式
        /// </summary>
        private void InitializeDataGridView()
        {
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
            dgvCommDataList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dgvCommDataList.RowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvCommDataList.BackgroundColor = System.Drawing.SystemColors.Control;
            dgvCommDataList.GridColor = System.Drawing.SystemColors.Control;

            // 设置表格交替样式
            dgvCommDataList.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            dgvCommDataList.Columns[0].Width = 30; // 选择
            dgvCommDataList.Columns[1].Width = 80; // 输出序号，从0开始编号
            dgvCommDataList.Columns[2].Width = 80; // 地址
            dgvCommDataList.Columns[3].Width = dgvCommDataList.Width - 360; // 项目名称
            dgvCommDataList.Columns[4].Width = 80; // 当前数值
            dgvCommDataList.Columns[5].Width = 80; // 对应的相机序号，从1开始编号


            // 设置单元格的值为"Y", 对应的checkbox为true, 单元格值为"N", 对应的checkbox为false
            ((DataGridViewCheckBoxColumn)dgvCommDataList.Columns[0]).TrueValue = "Y";
            ((DataGridViewCheckBoxColumn)dgvCommDataList.Columns[0]).FalseValue = "N";

            // 初始化表格数据
            try
            {
                dgvCommDataList.Rows.Clear();
                int index = 0;
                foreach (DataRow dr in FormMain.dataTableFinsOutput.Rows)
                {
                    object[] newRow = new object[6];
                    int wordAddress = 5001 + index / 16; // 失效数据从5001开始存储，5000为料号信息
                    int bitAddress = index % 16; // 位
                    newRow[0] = "N";
                    newRow[1] = index + 1;
                    newRow[2] = string.Format("{0}:{1}", wordAddress, bitAddress);
                    newRow[3] = dr["Description"].ToString();
                    newRow[4] = dr["Value"].ToString();
                    newRow[5] = ((int)dr["CCDIndex"] + 1).ToString();
                    dgvCommDataList.Rows.Add(newRow);
                    index++;
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
            DataRow[] dataRows;
            string strName;
            int rowIndex;
            switch (btn.Name)
            {
                case "btnAddNew":
                    List<string> nameList = new List<string>();
                    foreach (DataRow dr in FormMain.dataTableFinsData.Rows)
                    {
                        nameList.Add(dr["Description"].ToString());
                    }
                    FormCommDataSelect frmSelect = new FormCommDataSelect(nameList);
                    if (frmSelect.ShowDialog() == DialogResult.OK)
                    {
                        strName = frmSelect.DataNameSelected;
                        frmSelect.Dispose();
                        // 查询是否是重复的数据：检查数据名称是否相同
                        // 要先判断dataTableFinsOutput是否为空，如果为空，不需要判断数据重复
                        if (FormMain.dataTableFinsOutput.Rows.Count >=1 )
                        {
                            dataRows = FormMain.dataTableFinsOutput.Select(
                                "Description = '" + strName + "'");
                            if (dataRows.Length >= 1)
                            {
                                MessageBox.Show("该数据已经添加到输出列表中，请重新选择！");
                                return;
                            }
                        }

                        // 从dataTableFinsData中查询数据，将其IsSelected属性改为true
                        dataRows = FormMain.dataTableFinsData.Select(
                            "Description = '" + strName + "'");
                        if (dataRows.Length >=1)
                        {
                            FormMain.settingHelper.FinsDataSelectedIndex.Add((int)dataRows[0]["No"]);
                            rowIndex = FormMain.dataTableFinsData.Rows.IndexOf(dataRows[0]);
                            FormMain.dataTableFinsData.Rows[rowIndex]["IsSelected"] = true;
                            FormMain.dataTableFinsData.Rows[rowIndex]["IndexInOutput"] = 
                                FormMain.settingHelper.FinsDataSelectedIndex.Count - 1;
                        }

                        // 更新mDataTableFinsOutput
                        // 选出IsSelected为true的行，将其数据拷贝到mDataTableFinsOutput中，并按照IndexInOutput升序排列
                        dataRows = FormMain.dataTableFinsData.Select("IsSelected = " + true, "IndexInOutput ASC");
                        // 判断dataRows数据是否为空
                        if (dataRows.Count() >= 1)
                        {
                            FormMain.dataTableFinsOutput = dataRows.CopyToDataTable();
                        }
                        else
                        {
                            FormMain.dataTableFinsOutput.Rows.Clear();
                        }

                        // 更新单个相机Fins数据Table
                        for (int i = 0; i < FormMain.camNumber; i++)
                        {
                            // 单个CCD的Fins数据
                            dataRows = FormMain.dataTableFinsData.Select("IsSelected = " + true +
                            " and CCDIndex = " + i, "IndexInOutput ASC");
                            if (dataRows.Count() >= 1)
                            {
                                FormMain.dataTableSingleCCDFins[i] = dataRows.CopyToDataTable();
                            }
                            else
                            {
                                FormMain.dataTableSingleCCDFins[i].Rows.Clear();
                            }
                        }

                        // 写入配置文件
                        SaveFinsDataSelectToConfigFile(FormMain.settingHelper.FinsDataSelectedIndex);
                        // 更新DataGridView
                        UpdateDataGridView();
                    }
                    break;
                case "btnDelete":
                    for (int i = 0; i < dgvCommDataList.Rows.Count; i++)
                    {
                        if ((bool)dgvCommDataList.Rows[i].Cells[0].EditedFormattedValue)
                        {
                            checkedIndex.Add(i);
                        }
                    }

                    // 如果勾选的选项数量为0
                    if (checkedIndex.Count == 0)
                    {
                        MessageBox.Show("没有选中项目, 请重新勾选！");
                        return;
                    }
                    else
                    {
                        // 从List中删除选中的项目对应项
                        for (int i = 0; i < checkedIndex.Count; i++)
                        {
                            dataRows = FormMain.dataTableFinsData.Select("No = " + 
                                FormMain.settingHelper.FinsDataSelectedIndex[checkedIndex[checkedIndex.Count - i - 1]]);
                            if (dataRows.Length >= 1)
                            {
                                rowIndex = FormMain.dataTableFinsData.Rows.IndexOf(dataRows[0]);
                                FormMain.dataTableFinsData.Rows[rowIndex]["IsSelected"] = false;
                            }
                            // 从后往前删除，避免index重新计算
                            FormMain.settingHelper.FinsDataSelectedIndex.RemoveAt(checkedIndex[checkedIndex.Count - i - 1]);
                        }

                        // 根据配置文件是否选中数据作为输出，初始化DataTableSelected
                        foreach (int item in FormMain.settingHelper.FinsDataSelectedIndex)
                        {
                            dataRows = FormMain.dataTableFinsData.Select("No = " + item);
                            if (dataRows.Length >= 1)
                            {
                                rowIndex = FormMain.dataTableFinsData.Rows.IndexOf(dataRows[0]);
                                FormMain.dataTableFinsData.Rows[rowIndex]["IsSelected"] = true;
                                FormMain.dataTableFinsData.Rows[rowIndex]["IndexInOutput"] = 
                                    FormMain.settingHelper.FinsDataSelectedIndex.IndexOf(item);
                            }
                        }

                        // 更新mDataTableFinsOutput
                        // 选出IsSelected为true的行，将其数据拷贝到mDataTableFinsOutput中，并按照IndexInOutput升序排列
                        dataRows = FormMain.dataTableFinsData.Select("IsSelected = " + true, "IndexInOutput ASC");
                        // 判断dataRows数据是否为空
                        if (dataRows.Count() >= 1)
                        {
                            FormMain.dataTableFinsOutput = dataRows.CopyToDataTable();
                        }
                        else
                        {
                            FormMain.dataTableFinsOutput.Rows.Clear();
                        }

                        // 更新单个相机Fins数据Table
                        for (int i = 0; i < FormMain.camNumber; i++)
                        {
                            // 单个CCD的Fins数据
                            dataRows = FormMain.dataTableFinsData.Select("IsSelected = " + true +
                            " and CCDIndex = " + i, "IndexInOutput ASC");
                            if (dataRows.Count() >= 1)
                            {
                                FormMain.dataTableSingleCCDFins[i] = dataRows.CopyToDataTable();
                            }
                            else
                            {
                                FormMain.dataTableSingleCCDFins[i].Rows.Clear();
                            }
                        }

                        // 写入配置文件
                        SaveFinsDataSelectToConfigFile(FormMain.settingHelper.FinsDataSelectedIndex);
                        // 更新DataGridView
                        UpdateDataGridView();
                    }
                    break;
                case "btnMoveUp":
                    for (int i = 0; i < dgvCommDataList.Rows.Count; i++)
                    {
                        if ((bool)dgvCommDataList.Rows[i].Cells[0].EditedFormattedValue)
                        {
                            checkedIndex.Add(i);
                        }
                    }
                    // 如果勾选的选项数量超过1
                    if (checkedIndex.Count != 1)
                    {
                        MessageBox.Show("只能勾选一个, 请重新选择！");
                        return;
                    }
                    else
                    {
                        int selectedIndex = checkedIndex[0];
                        // 如果是第一个数据，不能再向上移动！
                        if (selectedIndex == 0)
                        {
                            MessageBox.Show("选择的数据已经是第一个，不能再往上移！");
                            return;
                        }
                        // 更新FinsDataSelectedIndex
                        int value, valueAbove;
                        value = FormMain.settingHelper.FinsDataSelectedIndex[selectedIndex];
                        valueAbove = FormMain.settingHelper.FinsDataSelectedIndex[selectedIndex - 1];
                        FormMain.settingHelper.FinsDataSelectedIndex[selectedIndex - 1] = value;
                        FormMain.settingHelper.FinsDataSelectedIndex[selectedIndex] = valueAbove;

                        // 根据配置文件是否选中数据作为输出，初始化DataTableSelected
                        foreach (int item in FormMain.settingHelper.FinsDataSelectedIndex)
                        {
                            dataRows = FormMain.dataTableFinsData.Select("No = " + item);
                            if (dataRows.Length >= 1)
                            {
                                rowIndex = FormMain.dataTableFinsData.Rows.IndexOf(dataRows[0]);
                                FormMain.dataTableFinsData.Rows[rowIndex]["IsSelected"] = true;
                                FormMain.dataTableFinsData.Rows[rowIndex]["IndexInOutput"] =
                                    FormMain.settingHelper.FinsDataSelectedIndex.IndexOf(item);
                            }
                        }

                        // 更新mDataTableFinsOutput
                        // 选出IsSelected为true的行，将其数据拷贝到mDataTableFinsOutput中，并按照IndexInOutput升序排列
                        dataRows = FormMain.dataTableFinsData.Select("IsSelected = " + true, "IndexInOutput ASC");
                        // 判断dataRows数据是否为空
                        if (dataRows.Count() >= 1)
                        {
                            FormMain.dataTableFinsOutput = dataRows.CopyToDataTable();
                        }
                        else
                        {
                            FormMain.dataTableFinsOutput.Rows.Clear();
                        }

                        // 更新单个相机Fins数据Table
                        for (int i = 0; i < FormMain.camNumber; i++)
                        {
                            // 单个CCD的Fins数据
                            dataRows = FormMain.dataTableFinsData.Select("IsSelected = " + true +
                            " and CCDIndex = " + i, "IndexInOutput ASC");
                            if (dataRows.Count() >= 1)
                            {
                                FormMain.dataTableSingleCCDFins[i] = dataRows.CopyToDataTable();
                            }
                            else
                            {
                                FormMain.dataTableSingleCCDFins[i].Rows.Clear();
                            }
                        }

                        // 写入配置文件
                        SaveFinsDataSelectToConfigFile(FormMain.settingHelper.FinsDataSelectedIndex);
                        // 更新DataGridView
                        UpdateDataGridView();
                        // 更新DataGridView选择项目
                        UpdateDataGridViewSelect(selectedIndex - 1);
                    }
                    break;
                case "btnMoveDown":
                    for (int i = 0; i < dgvCommDataList.Rows.Count; i++)
                    {
                        if ((bool)dgvCommDataList.Rows[i].Cells[0].EditedFormattedValue)
                        {
                            checkedIndex.Add(i);
                        }
                    }
                    // 如果勾选的选项数量超过1
                    if (checkedIndex.Count != 1)
                    {
                        MessageBox.Show("只能勾选一个, 请重新选择！");
                        return;
                    }
                    else
                    {
                        int selectedIndex = checkedIndex[0];
                        // 如果是第一个数据，不能再向上移动！
                        if (selectedIndex == FormMain.settingHelper.FinsDataSelectedIndex.Count - 1)
                        {
                            MessageBox.Show("选择的数据已经是最后一个，不能再往下移！");
                            return;
                        }
                        // 更新FinsDataSelectedIndex
                        int value, valueBelow;
                        value = FormMain.settingHelper.FinsDataSelectedIndex[selectedIndex];
                        valueBelow = FormMain.settingHelper.FinsDataSelectedIndex[selectedIndex + 1];
                        FormMain.settingHelper.FinsDataSelectedIndex[selectedIndex + 1] = value;
                        FormMain.settingHelper.FinsDataSelectedIndex[selectedIndex] = valueBelow;

                        // 根据配置文件是否选中数据作为输出，初始化DataTableSelected
                        foreach (int item in FormMain.settingHelper.FinsDataSelectedIndex)
                        {
                            dataRows = FormMain.dataTableFinsData.Select("No = " + item);
                            if (dataRows.Length >= 1)
                            {
                                rowIndex = FormMain.dataTableFinsData.Rows.IndexOf(dataRows[0]);
                                FormMain.dataTableFinsData.Rows[rowIndex]["IsSelected"] = true;
                                FormMain.dataTableFinsData.Rows[rowIndex]["IndexInOutput"] =
                                    FormMain.settingHelper.FinsDataSelectedIndex.IndexOf(item);
                            }
                        }

                        // 更新mDataTableFinsOutput
                        // 选出IsSelected为true的行，将其数据拷贝到mDataTableFinsOutput中，并按照IndexInOutput升序排列
                        dataRows = FormMain.dataTableFinsData.Select("IsSelected = " + true, "IndexInOutput ASC");
                        // 判断dataRows数据是否为空
                        if (dataRows.Count() >= 1)
                        {
                            FormMain.dataTableFinsOutput = dataRows.CopyToDataTable();
                        }
                        else
                        {
                            FormMain.dataTableFinsOutput.Rows.Clear();
                        }

                        // 更新单个相机Fins数据Table
                        for (int i = 0; i < FormMain.camNumber; i++)
                        {
                            // 单个CCD的Fins数据
                            dataRows = FormMain.dataTableFinsData.Select("IsSelected = " + true +
                            " and CCDIndex = " + i, "IndexInOutput ASC");
                            if (dataRows.Count() >= 1)
                            {
                                FormMain.dataTableSingleCCDFins[i] = dataRows.CopyToDataTable();
                            }
                            else
                            {
                                FormMain.dataTableSingleCCDFins[i].Rows.Clear();
                            }
                        }

                        // 写入配置文件
                        SaveFinsDataSelectToConfigFile(FormMain.settingHelper.FinsDataSelectedIndex);
                        // 更新DataGridView
                        UpdateDataGridView();
                        // 更新DataGridView选择项目
                        UpdateDataGridViewSelect(selectedIndex + 1);
                    }
                    break;
                case "btnEdit":
                    for (int i = 0; i < dgvCommDataList.Rows.Count; i++)
                    {
                        if ((bool)dgvCommDataList.Rows[i].Cells[0].EditedFormattedValue)
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
                        // 如果勾选的选项数量为1，打开对话框输入新料号的名字
                        FormCommAddressEdit frmEdit = new FormCommAddressEdit(dgvCommDataList.Rows[selectedIndex].Cells[1].Value.ToString(),
                            dgvCommDataList.Rows[selectedIndex].Cells[2].Value.ToString(),
                            dgvCommDataList.Rows[selectedIndex].Cells[3].Value.ToString(),
                            dgvCommDataList.Rows[selectedIndex].Cells[4].Value.ToString());
                        frmEdit.ShowDialog();
                        // 如果点击的是"确认按钮
                        if (frmEdit.IsConfirmed)
                        {
                            // 更新dataTableFinsData的地址
                            dataRows = FormMain.dataTableFinsData.Select("No = " +
                            FormMain.settingHelper.FinsDataSelectedIndex[selectedIndex]);
                            if (dataRows.Length >= 1)
                            {
                                rowIndex = FormMain.dataTableFinsData.Rows.IndexOf(dataRows[0]);
                                FormMain.dataTableFinsData.Rows[rowIndex]["FinsAddress"] = frmEdit.Address;
                            }

                            // 更新mDataTableFinsOutput
                            // 选出IsSelected为true的行，将其数据拷贝到mDataTableFinsOutput中，并按照IndexInOutput升序排列
                            dataRows = FormMain.dataTableFinsData.Select("IsSelected = " + true, "IndexInOutput ASC");

                            // 判断dataRows数据是否为空
                            if (dataRows.Count() >= 1)
                            {
                                FormMain.dataTableFinsOutput = dataRows.CopyToDataTable();
                            }
                            else
                            {
                                FormMain.dataTableFinsOutput.Rows.Clear();
                            }

                            // 更新单个相机Fins数据Table
                            for (int i = 0; i < FormMain.camNumber; i++)
                            {
                                // 单个CCD的Fins数据
                                dataRows = FormMain.dataTableFinsData.Select("IsSelected = " + true +
                                " and CCDIndex = " + i, "IndexInOutput ASC");
                                if (dataRows.Count() >= 1)
                                {
                                    FormMain.dataTableSingleCCDFins[i] = dataRows.CopyToDataTable();
                                }
                                else
                                {
                                    FormMain.dataTableSingleCCDFins[i].Rows.Clear();
                                }
                            }

                            // 写入配置文件
                            SaveFinsDataSelectToConfigFile(FormMain.settingHelper.FinsDataSelectedIndex);
                            // 更新DataGridView
                            UpdateDataGridView();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 检查一个string List里面是否有重复字符串
        /// 如果没有重复项，返回true，否则返回false
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool CheckStringListDistinct(List<string> list)
        {
            if (list.Distinct<string>().Count() < list.Count())
            {
                MessageBox.Show("有重复地址定义,请检查后再保存!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将Fins select index写入配置文件中
        /// </summary>
        /// <param name="list"></param>
        private void SaveFinsDataSelectToConfigFile(List<int> list)
        {
            StringBuilder sb = new StringBuilder(200);
            foreach (int item in list)
            {
                sb.Append(item);
                sb.Append(",");
            }
            // 如果数据不为空，删除最后一个逗号，如果数据为空，写入空字符
            if (list.Count >= 1)
            {
                sb.Remove(sb.Length - 1, 1);
                FormMain.mainConfigIniFile.IniWriteValue("FinsOutput", "SelectedIndex", sb.ToString());
            }
            else
            {
                FormMain.mainConfigIniFile.IniWriteValue("FinsOutput", "SelectedIndex", "");
            }
        }

        /// <summary>
        /// 将Fins select index写入配置文件中
        /// </summary>
        /// <param name="list"></param>
        private void SaveFinsDataAddressToConfigFile(List<short> list)
        {
            StringBuilder sb = new StringBuilder(200);
            foreach (short item in list)
            {
                sb.Append(item);
                sb.Append(",");
            }
            // 如果数据不为空，删除最后一个逗号，如果数据为空，写入空字符
            if (list.Count >= 1)
            {
                sb.Remove(sb.Length - 1, 1);
                FormMain.mainConfigIniFile.IniWriteValue("FinsOutput", "Address", sb.ToString());
            }
            else
            {
                FormMain.mainConfigIniFile.IniWriteValue("FinsOutput", "Address", "");
            }
        }

        /// <summary>
        /// 更新Fins Data select 表格
        /// </summary>
        private void UpdateDataGridView()
        {
            // 更新表格数据
            try
            {
                dgvCommDataList.Rows.Clear();
                int index = 0;
                foreach (DataRow dr in FormMain.dataTableFinsOutput.Rows)
                {
                    object[] newRow = new object[6];
                    int wordAddress = 5001 + index / 16; // 失效数据从5001开始存储，5000为料号信息
                    int bitAddress = index % 16; // 位
                    newRow[0] = "N";
                    newRow[1] = index + 1;
                    newRow[2] = string.Format("{0}:{1}", wordAddress, bitAddress);
                    newRow[3] = dr["Description"].ToString();
                    newRow[4] = dr["Value"].ToString();
                    newRow[5] = ((int)dr["CCDIndex"] + 1).ToString();
                    dgvCommDataList.Rows.Add(newRow);
                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("表格创建失败!");
            }
        }

        /// <summary>
        /// 将Fins Data select 之前选中的某一行继续选中，为了方便继续操作
        /// </summary>
        private void UpdateDataGridViewSelect(int select)
        {
            // 更新表格数据
            try
            {
                dgvCommDataList.Rows[select].Cells[0].Value = "Y";
            }
            catch (Exception ex)
            {
                MessageBox.Show("表格更新失败!");
            }
        }
    }
}
