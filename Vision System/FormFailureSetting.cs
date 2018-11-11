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
    public partial class FormFailureSetting : Form
    {
        public FormFailureSetting()
        {
            InitializeComponent();
        }

        private void FormFailureSetting_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                cmbCameraSelect.Items.Add(FormMain.jobHelper[i].CcdName);
            }
            cmbCameraSelect.SelectedIndex = 0;
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
            dgvFailureKWList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dgvFailureKWList.RowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvFailureKWList.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dgvFailureKWList.GridColor = System.Drawing.SystemColors.ControlLight;

            // 设置表格交替样式
            dgvFailureKWList.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            dgvFailureKWList.Columns[0].Width = 30;
            dgvFailureKWList.Columns[1].Width = 60;
            dgvFailureKWList.Columns[2].Width = (dgvFailureKWList.Width - 90) - 5;

            // 设置单元格的值为"Y", 对应的checkbox为true, 单元格值为"N", 对应的checkbox为false
            ((DataGridViewCheckBoxColumn)dgvFailureKWList.Columns[0]).TrueValue = "Y";
            ((DataGridViewCheckBoxColumn)dgvFailureKWList.Columns[0]).FalseValue = "N";

            // 初始化表格数据
            try
            {
                dgvFailureKWList.Rows.Clear();
                for (int i = 0; i < FormMain.jobHelper[cmbCameraSelect.SelectedIndex].FailuremodeKeyWd.Count; i++)
                {
                    object[] dr = new object[3];
                    dr[0] = "N";
                    dr[1] = i.ToString();
                    dr[2] = FormMain.jobHelper[cmbCameraSelect.SelectedIndex].FailuremodeKeyWd[i];
                    dgvFailureKWList.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("表格创建失败!");
            }
        }

        private void cmbCameraSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 初始化表格数据
            try
            {
                dgvFailureKWList.Rows.Clear();
                for (int i = 0; i < FormMain.jobHelper[cmbCameraSelect.SelectedIndex].FailuremodeKeyWd.Count; i++)
                {
                    object[] dr = new object[3];
                    dr[0] = "N";
                    dr[1] = i.ToString();
                    dr[2] = FormMain.jobHelper[cmbCameraSelect.SelectedIndex].FailuremodeKeyWd[i];
                    dgvFailureKWList.Rows.Add(dr);
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
            string strName;
            int ccdIndex = cmbCameraSelect.SelectedIndex;
            switch (btn.Name)
            {
                case "btnAddNew":
                    List<string> nameList = new List<string>();
                    nameList = FormMain.jobHelper[ccdIndex].ToolBlockNameDictionary.Values.ToList();
                    FormFailureSelect frmSelect = new FormFailureSelect(nameList);
                    if (frmSelect.ShowDialog() == DialogResult.OK)
                    {
                        strName = frmSelect.FailureKWSelected;
                        frmSelect.Dispose();

                        // 查询是否是重复的失效模式名称
                        if(FormMain.jobHelper[ccdIndex].FailuremodeKeyWd.IndexOf(strName) != -1)
                        { 
                            MessageBox.Show("该失效模式已经添加到输出列表中，请重新选择！");
                            return;
                        }

                        // 更新mJobHelper中的统计数据
                        FormMain.jobHelper[ccdIndex].FailuremodeKeyWd.Add(strName);
                        FormMain.jobHelper[ccdIndex].FailResultForKeyWd.Add(true);
                        FormMain.jobHelper[ccdIndex].FailCountForKeyWd.Add(0);

                        // 保存到配置文件
                        SaveKeyWordListToConfigFile(ccdIndex, FormMain.jobHelper[ccdIndex].FailuremodeKeyWd);

                        // 重新初始化表格数据
                        try
                        {
                            dgvFailureKWList.Rows.Clear();
                            for (int i = 0; i < FormMain.jobHelper[ccdIndex].FailuremodeKeyWd.Count; i++)
                            {
                                object[] dr = new object[3];
                                dr[0] = "N";
                                dr[1] = i.ToString();
                                dr[2] = FormMain.jobHelper[ccdIndex].FailuremodeKeyWd[i];
                                dgvFailureKWList.Rows.Add(dr);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("表格创建失败!");
                        }
                    }
                    frmSelect.Dispose();
                    break;
                case "btnDelete":
                    for (int i = 0; i < dgvFailureKWList.Rows.Count; i++)
                    {
                        if ((bool)dgvFailureKWList.Rows[i].Cells[0].EditedFormattedValue)
                        {
                            checkedIndex.Add(i);
                        }
                    }

                    // 如果勾选的选项数量超过1
                    if (checkedIndex.Count == 0)
                    {
                        MessageBox.Show("没有选中项目, 请重新勾选！");
                        return;
                    }
                    else
                    {
                        // 从List中删除选中的项目对应的料号
                        for (int i = 0; i < checkedIndex.Count; i++)
                        {
                            // 从后往前删除，避免index重新计算
                            // 更新mJobHelper中的统计数据
                            FormMain.jobHelper[ccdIndex].FailuremodeKeyWd.RemoveAt(checkedIndex[checkedIndex.Count - i - 1]);
                            FormMain.jobHelper[ccdIndex].FailResultForKeyWd.RemoveAt(checkedIndex[checkedIndex.Count - i - 1]);
                            FormMain.jobHelper[ccdIndex].FailCountForKeyWd.RemoveAt(checkedIndex[checkedIndex.Count - i - 1]);
                        }
                        SaveKeyWordListToConfigFile(ccdIndex, FormMain.jobHelper[ccdIndex].FailuremodeKeyWd);

                        // 重新初始化表格数据
                        try
                        {
                            dgvFailureKWList.Rows.Clear();
                            for (int i = 0; i < FormMain.jobHelper[ccdIndex].FailuremodeKeyWd.Count; i++)
                            {
                                object[] dr = new object[3];
                                dr[0] = "N";
                                dr[1] = i.ToString();
                                dr[2] = FormMain.jobHelper[ccdIndex].FailuremodeKeyWd[i];
                                dgvFailureKWList.Rows.Add(dr);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("表格创建失败!");
                        }
                    }
                    break;
                default:
                    break;
            } 
        }

        /// <summary>
        /// 将key word list写入配置文件中
        /// </summary>
        /// <param name="list"></param>
        private void SaveKeyWordListToConfigFile(int index,List<string> list)
        {
            StringBuilder sb = new StringBuilder(200);
            IniFile ConfigIniFile = new IniFile(FormMain.strCCDConfigFilePath[index]);
            foreach (string item in list)
            {
                sb.Append(item);
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            ConfigIniFile.IniWriteValue("CCD" + (index + 1), "FailModeKW", sb.ToString());
        }

        /// <summary>
        /// 更新JobHelper中的统计数据
        /// </summary>
        /// <param name="index"></param>
        private void UpdateJobHelperStatisticData(int index)
        {
            // 初始化失效模式统计数据
            FormMain.jobHelper[index].FailResultForKeyWd.Clear();
            FormMain.jobHelper[index].FailCountForKeyWd.Clear();
            for (int k = 0; k < FormMain.jobHelper[index].FailuremodeKeyWd.Count; k++)
            {
                FormMain.jobHelper[index].FailResultForKeyWd.Add(true);
                FormMain.jobHelper[index].FailCountForKeyWd.Add(0);
            }
        }
    }
}
