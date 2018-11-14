using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision_System
{
    public partial class FormSaveSetting : Form
    {
        private CheckBox[] checkBoxes;
        
        public FormSaveSetting()
        {
            InitializeComponent();
            // 初始化单个CCD图片是否保存选项
            checkBoxes = new CheckBox[]{
                chkImageSaveCCD1,
                chkImageSaveCCD2,
                chkImageSaveCCD3,
                chkImageSaveCCD4,
                chkImageSaveCCD5,
                chkImageSaveCCD6,
                chkImageSaveCCD7,
                chkImageSaveCCD8};
        }

        private void FormSaveSetting_Load(object sender, EventArgs e)
        {
            // 初始化图片和数据保存设置
            this.chkSaveData.Checked = FormMain.settingHelper.IsSaveData ? true : false;
            this.chkSaveOKImage.Checked = FormMain.settingHelper.IsSaveOKImage ? true : false;
            this.chkSaveNGImage.Checked = FormMain.settingHelper.IsSaveNGImage ? true : false;
            switch (FormMain.settingHelper.ImageSize)
            {
                case SaveImageSize.Full:
                    this.cmbImageSize.SelectedIndex = 0;
                    break;
                case SaveImageSize.Half:
                    this.cmbImageSize.SelectedIndex = 1;
                    break;
                case SaveImageSize.Quater:
                    this.cmbImageSize.SelectedIndex = 2;
                    break;
                default:
                    break;
            }
            this.cmbImageFormat.SelectedIndex = (FormMain.settingHelper.ImageFormat == SaveImageFormat.BMP ? 0 : 1);
            this.txtDataKeepDays.Text = FormMain.settingHelper.DataKeepDays.ToString();
            this.txtImageKeepDays.Text = FormMain.settingHelper.ImageKeepDays.ToString();
            this.txtImageFolderAddress.Text = FormMain.settingHelper.StrSaveImageDirectoryPath;
            this.txtDataFolderAddress.Text = FormMain.settingHelper.StrSaveDataDirectoryPath;
            switch (FormMain.settingHelper.DataFormat)
            {
                case SaveDataFormat.Csv:
                    this.cmbDataFormat.SelectedIndex = 0;
                    break;
                case SaveDataFormat.DataBase:
                    this.cmbDataFormat.SelectedIndex = 1;
                    break;
                default:
                    break;
            }

            // 设置checkBox的可见性
            for (int i = 0; i < checkBoxes.Length; i++)
            {
                checkBoxes[i].Visible = false;
                checkBoxes[i].Checked = true;
                if (!FormMain.settingHelper.IsSaveOKImage && !FormMain.settingHelper.IsSaveNGImage)
                {
                    checkBoxes[i].Enabled = false;
                }
            }
            // 设置CheckBox是否勾选
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                checkBoxes[i].Visible = true;
                checkBoxes[i].Checked = FormMain.jobHelper[i].IsSaveImage;
            }
        }

        /// <summary>
        /// 实时调整CheckBox的Enable属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSaveOKImage_CheckedChanged(object sender, EventArgs e)
        {
            // 设置checkBox的Enable属性
            if (!chkSaveOKImage.Checked && !chkSaveNGImage.Checked)
            {
                for (int i = 0; i < checkBoxes.Length; i++)
                {
                    checkBoxes[i].Enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < checkBoxes.Length; i++)
                {
                    checkBoxes[i].Enabled = true;
                }
            }
        }

        /// <summary>
        /// 实时调整CheckBox的Enable属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSaveNGImage_CheckedChanged(object sender, EventArgs e)
        {
            // 设置checkBox的Enable属性
            if (!chkSaveOKImage.Checked && !chkSaveNGImage.Checked)
            {
                for (int i = 0; i < checkBoxes.Length; i++)
                {
                    checkBoxes[i].Enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < checkBoxes.Length; i++)
                {
                    checkBoxes[i].Enabled = true;
                }
            }
        }

        /// <summary>
        /// 多个控件的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "btnBrowseImageFolder":
                    // 浏览图片目录
                    System.Windows.Forms.FolderBrowserDialog dialogImage = new System.Windows.Forms.FolderBrowserDialog();
                    dialogImage.Description = "请选择文件夹";
                    dialogImage.SelectedPath = txtImageFolderAddress.Text;
                    if (dialogImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (string.IsNullOrEmpty(dialogImage.SelectedPath))
                        {
                            MessageBox.Show(this, "文件夹路径不能为空", "提示");
                            return;
                        }
                        txtImageFolderAddress.Text = dialogImage.SelectedPath;
                    }
                    break;
                case "btnBrowseDataFolder":
                    System.Windows.Forms.FolderBrowserDialog dialogData = new System.Windows.Forms.FolderBrowserDialog();
                    dialogData.Description = "请选择文件夹";
                    dialogData.SelectedPath = txtDataFolderAddress.Text;
                    if (dialogData.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (string.IsNullOrEmpty(dialogData.SelectedPath))
                        {
                            MessageBox.Show(this, "文件夹路径不能为空", "提示");
                            return;
                        }
                        txtDataFolderAddress.Text = dialogData.SelectedPath;
                    }
                    // 浏览数据目录
                    break;
                case "btnOpenImageFolder":
                    // 打开图片文件夹
                    System.Diagnostics.Process.Start("explorer.exe", FormMain.settingHelper.StrSaveImageDirectoryPath);
                    break;
                case "btnOpenDataFolder":
                    // 打开数据文件夹
                    System.Diagnostics.Process.Start("explorer.exe", FormMain.settingHelper.StrSaveDataDirectoryPath);
                    break;
                case "btnSave":
                    // 图片保存设置
                    FormMain.settingHelper.StrSaveImageDirectoryPath = this.txtImageFolderAddress.Text;
                    FormMain.settingHelper.ImageKeepDays = Convert.ToInt16(this.txtImageKeepDays.Text);
                    FormMain.settingHelper.IsSaveOKImage = this.chkSaveOKImage.Checked;
                    FormMain.settingHelper.IsSaveNGImage = this.chkSaveNGImage.Checked;
                    FormMain.mainConfigIniFile.IniWriteValue("Main", "IsSaveOKImage", FormMain.settingHelper.IsSaveOKImage ? 1 : 0);
                    FormMain.mainConfigIniFile.IniWriteValue("Main", "IsSaveNGImage", FormMain.settingHelper.IsSaveNGImage ? 1 : 0);
                    FormMain.mainConfigIniFile.IniWriteValue("SavePath", "ImagePath", this.txtImageFolderAddress.Text);
                    FormMain.mainConfigIniFile.IniWriteValue("Main", "ImageKeepDays", FormMain.settingHelper.ImageKeepDays);

                    // 单个CCD图片保存
                    for (int i = 0; i < FormMain.camNumber; i++)
                    {
                        FormMain.jobHelper[i].IsSaveImage = checkBoxes[i].Checked;
                        FormMain.ccdConfigIniFile[i].IniWriteValue("CCD" + (i + 1), "ImageSave", FormMain.jobHelper[i].IsSaveImage ? 1 : 0);
                    }
                    // 图片保存格式及大小
                    switch (this.cmbImageSize.SelectedIndex)
                    {
                        case 0:
                            FormMain.settingHelper.ImageSize = SaveImageSize.Full;
                            FormMain.mainConfigIniFile.IniWriteValue("Main", "ImageSize", "Full");
                            break;
                        case 1:
                            FormMain.settingHelper.ImageSize = SaveImageSize.Half;
                            FormMain.mainConfigIniFile.IniWriteValue("Main", "ImageSize", "Half");
                            break;
                        case 2:
                            FormMain.settingHelper.ImageSize = SaveImageSize.Quater;
                            FormMain.mainConfigIniFile.IniWriteValue("Main", "ImageSize", "Quater");
                            break;
                        default:
                            break;
                    }
                    switch (this.cmbImageFormat.SelectedIndex)
                    {
                        case 0:
                            FormMain.settingHelper.ImageFormat = SaveImageFormat.BMP;
                            FormMain.mainConfigIniFile.IniWriteValue("Main", "ImageFormat", "BMP");
                            break;
                        case 1:
                            FormMain.settingHelper.ImageFormat = SaveImageFormat.JPG;
                            FormMain.mainConfigIniFile.IniWriteValue("Main", "ImageFormat", "JPG");
                            break;
                        default:
                            break;
                    }
                    // 数据保存设置
                    FormMain.settingHelper.StrSaveDataDirectoryPath = this.txtDataFolderAddress.Text;
                    FormMain.settingHelper.IsSaveData = this.chkSaveData.Checked;
                    FormMain.settingHelper.DataKeepDays = Convert.ToInt16(this.txtDataKeepDays.Text);
                    switch (this.cmbDataFormat.SelectedIndex)
                    {
                        case 0:
                            FormMain.settingHelper.DataFormat = SaveDataFormat.Csv;
                            FormMain.mainConfigIniFile.IniWriteValue("Main", "DataFormat", "Csv");
                            break;
                        case 1:
                            FormMain.settingHelper.DataFormat = SaveDataFormat.DataBase;
                            FormMain.mainConfigIniFile.IniWriteValue("Main", "DataFormat", "Database");
                            break;
                        default:
                            break;
                    }
                    FormMain.mainConfigIniFile.IniWriteValue("SavePath", "DataPath", this.txtDataFolderAddress.Text);
                    FormMain.mainConfigIniFile.IniWriteValue("Main", "IsSaveData", FormMain.settingHelper.IsSaveData ? 1 : 0);
                    FormMain.mainConfigIniFile.IniWriteValue("Main", "DataKeepDays", FormMain.settingHelper.DataKeepDays);
                    this.Close();
                    break;
                default:
                    break;
            }
        }
    }
}
