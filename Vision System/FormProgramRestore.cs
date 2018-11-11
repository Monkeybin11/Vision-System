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
    public partial class FormProgramRestore : Form
    {
        private string _folderPathBase; //保存传入的文件夹路径
        private string _folderPath; //保存最终的文件夹路径
        private bool _isConfirmed = false; //是否点击了确认，且信息格式都正确

        public string FolderPathBase { get => _folderPathBase; set => _folderPathBase = value; }
        public string FolderPath { get => _folderPath; set => _folderPath = value; }
        public bool IsConfirmed { get => _isConfirmed; set => _isConfirmed = value; }

        public FormProgramRestore(string path)
        {
            InitializeComponent();
            FolderPathBase = path;
            FolderPath = path;
        }

        private void FormProgramRestore_Load(object sender, EventArgs e)
        {
            string[] folders = Directory.GetDirectories(FolderPathBase);
            foreach (string folder in folders)
            {
                // 从文件夹路径全名中提取文件夹名
                string shortName = folder.Substring(folder.LastIndexOf("\\") + 1, 
                    (folder.Length - folder.LastIndexOf("\\") - 1));
                listBoxFolderName.Items.Add(shortName);
            }
        }

        private void btnGetDetails_Click(object sender, EventArgs e)
        {
            string path;
            string[] files;
            if (listBoxFolderName.SelectedItem == null)
            {
                MessageBox.Show("请先从左侧列表中选择一项后再点击", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            listBoxVppFileName.Items.Clear();
            path = FolderPathBase + "\\" + listBoxFolderName.SelectedItem.ToString();
            txtFolderPath.Text = path;
            files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                string shortName = file.Substring(file.LastIndexOf("\\") + 1,
                    (file.Length - file.LastIndexOf("\\") - 1));
                listBoxVppFileName.Items.Add(shortName);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtFolderPath.Text))
            {
                MessageBox.Show("请先从左侧列表中选择要恢复的程序", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FolderPath = txtFolderPath.Text;
            IsConfirmed = true;
            this.Close();
        }
    }
}
