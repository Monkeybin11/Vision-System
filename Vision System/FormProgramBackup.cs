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
    public partial class FormProgramBackup : Form
    {
        private string _folderPathBase; //保存传入的文件夹路径
        private string _folderPath; //保存最终的文件夹路径
        private bool _isConfirmed = false; //是否点击了确认，且信息格式都正确

        public string FolderPathBase { get => _folderPathBase; set => _folderPathBase = value; }
        public string FolderPath { get => _folderPath; set => _folderPath = value; }
        public bool IsConfirmed { get => _isConfirmed; set => _isConfirmed = value; }

        public FormProgramBackup(string path)
        {
            InitializeComponent();
            FolderPathBase = path;
            FolderPath = path;
            radioDateFolderName.Checked = true;
        }

        private void radioDateFolderName_CheckedChanged(object sender, EventArgs e)
        {
            labelFolderName.Enabled = false;
            txtFolderName.Enabled = false;
            btnBrowseFolder.Enabled = false;
            txtFolderPath.Enabled = false;
        }

        private void radioCustomizedFolderName_CheckedChanged(object sender, EventArgs e)
        {
            labelFolderName.Enabled = true;
            txtFolderName.Enabled = true;
            btnBrowseFolder.Enabled = false;
            txtFolderPath.Enabled = false;
        }

        private void radioCustomizedFolderPath_CheckedChanged(object sender, EventArgs e)
        {
            labelFolderName.Enabled = false;
            txtFolderName.Enabled = false;
            btnBrowseFolder.Enabled = true;
            txtFolderPath.Enabled = true;
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            dialog.SelectedPath = FolderPathBase;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                txtFolderPath.Text = dialog.SelectedPath;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (radioDateFolderName.Checked)
            {
                FolderPath = FolderPathBase + "\\" + string.Format("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now);
            }
            else if (radioCustomizedFolderName.Checked)
            {
                FolderPath = FolderPathBase + "\\" + txtFolderName.Text;
                if(string.IsNullOrWhiteSpace(txtFolderName.Text))
                {
                    MessageBox.Show("文本不能为空，请重新输入", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFolderName.Focus();
                    return;
                }
                if (FolderPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    //含有非法字符 \ / : * ? " < > | 等
                    MessageBox.Show("文件夹名中含有非法字符，请重新输入", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFolderName.Focus();
                    return;
                }
            }
            else if (radioCustomizedFolderPath.Checked)
            {
                FolderPath = txtFolderPath.Text;
                if (string.IsNullOrWhiteSpace(txtFolderPath.Text))
                {
                    MessageBox.Show("文本不能为空，请重新输入", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFolderPath.Focus();
                    return;
                }
                if (FolderPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    //含有非法字符 \ / : * ? " < > | 等
                    MessageBox.Show("文件夹名中含有非法字符，请重新输入", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFolderPath.Focus();
                    return;
                }
            }
            IsConfirmed = true;
            this.Close();
        }
    }
}
