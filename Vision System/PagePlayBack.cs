using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ToolBlock;

namespace Vision_System
{
    public partial class PagePlayBack : UserControl
    {
        private CogImageFileTool OpenFileImage = new CogImageFileTool();
        private CogToolBlock[] CCD_ToolBlock;
        private ICogImage cogOriginalImage;
        private string _imagePath;
        private string[] _filelist_CCDImage;
        private int _currentFilelistIndex_CCDImage;
        private bool[] CCDRun_Result;
        private int _mSelectedCamIndex = 0;
        private bool _isAutoPlay = false;
        private bool _isToolRunning = false;

        public bool IsAutoPlay { get => _isAutoPlay; set => _isAutoPlay = value; }
        public bool IsToolRunning { get => _isToolRunning; set => _isToolRunning = value; }

        public PagePlayBack()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// 构造类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PagePlayBack_Load(object sender, EventArgs e)
        {
            cogDisplayStatusBarV21.Display = this.cogDisplay_PlayBack;
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                cmbCameraSelect.Items.Add(FormMain.jobHelper[i].CcdName);
            }
            if (FormMain.camNumber >= 1)
                cmbCameraSelect.SelectedIndex = 0;
        }

        /// <summary>
        /// 相机选择comboBox序号发生变化时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCameraSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            _mSelectedCamIndex = cmb.SelectedIndex;
        }

        /// <summary>
        /// 传递CogToolBlock变量到该类中
        /// </summary>
        /// <param name="toolBlock"></param>
        public void SetToolBlock(CogToolBlock[] toolBlock)
        {
            CCD_ToolBlock = toolBlock;
            CCDRun_Result = new bool[FormMain.camNumber];
        }

        /// <summary>
        /// 选择的图片文件路径变化时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MsgBox_SelectImagePath_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 按钮触发的事件：选择文件，上一张，下一张，自动播放，等...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ButtonClickEvent_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                // 选择图片的路径
                case "btn_SelectImagePath":
                    OpenImageDialog.ShowDialog();
                    _imagePath = OpenImageDialog.FileName;
                    // 如果没有选择图片文件，直接返回，不执行任何操作
                    if (string.IsNullOrEmpty(_imagePath)) return;
                    txtSelectImagePath.Text = OpenImageDialog.FileName;
                    _filelist_CCDImage = Directory.GetFiles(Directory.GetParent(_imagePath).FullName);
                    _currentFilelistIndex_CCDImage = Array.IndexOf(_filelist_CCDImage, _imagePath);

                    if (txtSelectImagePath.Text != "")
                    {
                        _imagePath = _filelist_CCDImage[_currentFilelistIndex_CCDImage];
                        FileInfo cf1 = new FileInfo(_imagePath);
                        if (cf1.Extension.ToLower() != ".bmp" && cf1.Extension.ToLower() != ".jpg")
                        {
                            MessageBox.Show("Image format Not bmp or jpg file!");
                            return;
                        }
                        OpenFileImage.Operator.Open(_imagePath, CogImageFileModeConstants.Read);
                        OpenFileImage.Run();
                        cogOriginalImage = (ICogImage)OpenFileImage.OutputImage;
                        RunInspection(_mSelectedCamIndex, cogOriginalImage);
                        UpdateDisplayAndResult(_mSelectedCamIndex);
                        // UpdateCogDisplay(_mSelectedCamIndex);
                    }
                    break;
                // 运行上一张图片
                case "btn_SelectPreviousImage":
                    if (txtSelectImagePath.Text != "")
                    {
                        if (_currentFilelistIndex_CCDImage >= 1)
                        {
                            _currentFilelistIndex_CCDImage -= 1;
                        }
                        else if (_currentFilelistIndex_CCDImage == 0)
                        {
                            _currentFilelistIndex_CCDImage = _filelist_CCDImage.Length - 1;
                            // MessageBox.Show("CCD1已达到最后一张图", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // Err1 = true;
                        }
                    }

                    if (txtSelectImagePath.Text != "")
                    {
                        _imagePath = _filelist_CCDImage[_currentFilelistIndex_CCDImage];
                        FileInfo cf1 = new FileInfo(_imagePath);
                        if (cf1.Extension.ToLower() != ".bmp" && cf1.Extension.ToLower() != ".jpg")
                        {
                            MessageBox.Show("CCD1 DIR Not BMP&JPG File!");
                            return;
                        }
                        txtSelectImagePath.Text = _imagePath;
                        OpenFileImage.Operator.Open(_imagePath, CogImageFileModeConstants.Read);
                        OpenFileImage.Run();
                        RunInspection(_mSelectedCamIndex, (ICogImage)OpenFileImage.OutputImage);
                        UpdateDisplayAndResult(_mSelectedCamIndex);
                        // UpdateCogDisplay(_mSelectedCamIndex);
                    }
                    break;
                // 运行下一张图片
                case "btn_SelectNextImage":
                    if (txtSelectImagePath.Text != "")
                    {
                        if (_currentFilelistIndex_CCDImage <= _filelist_CCDImage.Length - 2)
                        {
                            _currentFilelistIndex_CCDImage += 1;
                        }
                        else if (_currentFilelistIndex_CCDImage == _filelist_CCDImage.Length - 1)
                        {
                            _currentFilelistIndex_CCDImage = 0;
                            // MessageBox.Show("CCD1已达到最后一张图", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // Err1 = true;
                        }
                    }

                    if (txtSelectImagePath.Text != "")
                    {
                        _imagePath = _filelist_CCDImage[_currentFilelistIndex_CCDImage];
                        FileInfo cf1 = new FileInfo(_imagePath);
                        if (cf1.Extension.ToLower() != ".bmp" && cf1.Extension.ToLower() != ".jpg")
                        {
                            MessageBox.Show("CCD1 DIR Not BMP&JPG File!");
                            return;
                        }
                        txtSelectImagePath.Text = _imagePath;
                        OpenFileImage.Operator.Open(_imagePath, CogImageFileModeConstants.Read);
                        OpenFileImage.Run();
                        RunInspection(_mSelectedCamIndex, (ICogImage)OpenFileImage.OutputImage);
                        UpdateDisplayAndResult(_mSelectedCamIndex);
                        // UpdateCogDisplay(_mSelectedCamIndex);
                    }
                    break;
                // 运行第一张图片
                case "btn_SelectFirstImage":
                    _currentFilelistIndex_CCDImage = 0;
                    if (txtSelectImagePath.Text != "")
                    {
                        _imagePath = _filelist_CCDImage[_currentFilelistIndex_CCDImage];
                        FileInfo cf1 = new FileInfo(_imagePath);
                        if (cf1.Extension.ToLower() != ".bmp" && cf1.Extension.ToLower() != ".jpg")
                        {
                            MessageBox.Show("CCD1 DIR Not BMP&JPG File!");
                            return;
                        }
                        txtSelectImagePath.Text = _imagePath;
                        OpenFileImage.Operator.Open(_imagePath, CogImageFileModeConstants.Read);
                        OpenFileImage.Run();
                        RunInspection(_mSelectedCamIndex, (ICogImage)OpenFileImage.OutputImage);
                        UpdateDisplayAndResult(_mSelectedCamIndex);
                        // UpdateCogDisplay(_mSelectedCamIndex);
                    }
                    break;
                // 运行最后一张图片
                case "btn_SelectLastImage":
                    _currentFilelistIndex_CCDImage = _filelist_CCDImage.Length - 1;
                    if (txtSelectImagePath.Text != "")
                    {
                        _imagePath = _filelist_CCDImage[_currentFilelistIndex_CCDImage];
                        FileInfo cf1 = new FileInfo(_imagePath);
                        if (cf1.Extension.ToLower() != ".bmp" && cf1.Extension.ToLower() != ".jpg")
                        {
                            MessageBox.Show("CCD1 DIR Not BMP&JPG File!");
                            return;
                        }
                        txtSelectImagePath.Text = _imagePath;
                        OpenFileImage.Operator.Open(_imagePath, CogImageFileModeConstants.Read);
                        OpenFileImage.Run();
                        RunInspection(_mSelectedCamIndex, (ICogImage)OpenFileImage.OutputImage);
                        UpdateDisplayAndResult(_mSelectedCamIndex);
                        // UpdateCogDisplay(_mSelectedCamIndex);                        
                    }
                    break;
                // 自动播放图片
                case "btn_AutoPlay":
                    if (!IsAutoPlay)
                    {
                        IsAutoPlay = true;
                        btn_AutoPlay.BackgroundImage = Properties.Resources.pause;
                        btn_AutoPlay.Text = "停止";
                        UpdateControlEnabled();
                        timer_AutoPlay.Start();
                    }
                    else
                    {
                        IsAutoPlay = false;
                        btn_AutoPlay.BackgroundImage = Properties.Resources._continue;
                        btn_AutoPlay.Text = "播放";
                        UpdateControlEnabled();
                        timer_AutoPlay.Stop();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 自动播放事件，定时器运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_AutoPlay_Tick(object sender, EventArgs e)
        {
            if (txtSelectImagePath.Text != "")
            {
                _imagePath = _filelist_CCDImage[_currentFilelistIndex_CCDImage];
                FileInfo cf1 = new FileInfo(_imagePath);
                if (cf1.Extension.ToLower() != ".bmp" && cf1.Extension.ToLower() != ".jpg")
                {
                    MessageBox.Show("CCD1 DIR Not BMP&JPG File!");
                    return;
                }
                txtSelectImagePath.Text = _imagePath;
                OpenFileImage.Operator.Open(_imagePath, CogImageFileModeConstants.Read);
                OpenFileImage.Run();
                RunInspection(_mSelectedCamIndex, (ICogImage)OpenFileImage.OutputImage);
                UpdateDisplayAndResult(_mSelectedCamIndex);
                // UpdateCogDisplay(_mSelectedCamIndex);

                if (_currentFilelistIndex_CCDImage <= _filelist_CCDImage.Length - 2)
                {
                    _currentFilelistIndex_CCDImage += 1;
                }
                else if (_currentFilelistIndex_CCDImage == _filelist_CCDImage.Length - 1)
                {
                    _currentFilelistIndex_CCDImage = 0;
                    // MessageBox.Show("CCD1已达到最后一张图", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Err1 = true;
                }
            }
        }

        /// <summary>
        /// 运行图片处理工具，取出结果
        /// </summary>
        /// <param name="index"></param>
        /// <param name="image"></param>
        private void RunInspection(int index, ICogImage image)
        {
            CCD_ToolBlock[index].Inputs["InputImage"].Value = image;
            CCD_ToolBlock[index].Run();
            CCDRun_Result[index] = GetToolBlockTotalResult(CCD_ToolBlock[index]);
        }

        /// <summary>
        /// ToolBlock的结果判断规则：
        /// ToolBlock里面包含了若干个子ToolBlock，每一个子ToolBlock代表一种失效模式的判断
        /// 每一个子ToolBlock有一个Bool型输出Result，当Result为True时表示失效模式判断结果为OK，反之为NG
        /// ToolBlock的总结果需要汇总每一个子ToolBlock的结果，然后综合进行判断
        /// </summary>
        /// <param name="toolBlock"></param>
        /// <returns></returns>
        private bool GetToolBlockTotalResult(CogToolBlock toolBlock)
        {
            //适用于编写脚本判断结果的情况下，ToolBlock有一个输出为"Result"，
            //脚本控制Result的结果，这种情况下ToolBlock的判断结果为输出"Result" + 
            //工具运行的结果是否为Accept
            bool outputResult;
            CogToolBlock tool;
            bool toolResult; //每个子toolBlock的分结果
            bool totalResult = true; //ToolBlock的总结果
            for (int i = 0; i < toolBlock.Tools.Count; i++)
            {
                //每一个单项检测结果
                tool = (CogToolBlock)toolBlock.Tools[i];
                //默认情况下outputResult为Pass
                outputResult = true; 
                for (int j = 0; j < tool.Outputs.Count; j++)
                {
                    //检查ToolBlock的输出中是否有"result"一项，如果有，则需要取其判断结果
                    if (tool.Outputs[j].ValueType.ToString().ToLower() == "system.boolean" &&
                        tool.Outputs[j].Name.ToLower().Equals("result"))
                    {
                        outputResult = (bool)tool.Outputs[j].Value;
                    }
                }
                // 子ToolBlock的综合结果
                toolResult = outputResult && (tool.RunStatus.Result == CogToolResultConstants.Accept);
                // 子ToolBlock综合结果
                totalResult &= toolResult;
            }
            return totalResult;
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        /// <param name="index"></param>
        private void UpdateDisplayAndResult(int index)
        {
            UpdateCogDisplay(index);
            UpdateResultLabel(index);
            UpdateImageInfo(index);
        }

        /// <summary>
        /// 显示图片运行结果
        /// </summary>
        /// <param name="index"></param>
        private void UpdateCogDisplay(int index)
        {
            ICogRecord record = CCD_ToolBlock[index].CreateLastRunRecord().SubRecords[0];
            cogDisplay_PlayBack.StaticGraphics.Clear();
            cogDisplay_PlayBack.InteractiveGraphics.Clear();
            cogDisplay_PlayBack.Record = record;
            cogDisplay_PlayBack.Fit(true);
        }

        /// <summary>
        /// 更新工具运行结果
        /// </summary>
        /// <param name="index"></param>
        private void UpdateResultLabel(int index)
        {
            this.pictureBoxRunResult.Image = this.CCDRun_Result[index] ? Properties.Resources.green_light : Properties.Resources.red_light;
        }

        /// <summary>
        /// 更新图片信息
        /// </summary>
        /// <param name="index"></param>
        private void UpdateImageInfo(int index)
        {
            FileInfo cf1 = new FileInfo(_imagePath);
            this.labelCCDName.Text = FormMain.jobHelper[index].CcdName;
            this.labelPicTakenTime.Text = cf1.CreationTime.ToString();
            this.labelPicSize.Text = cogOriginalImage.Width + " * " + cogOriginalImage.Height + "pixel";
            this.labelPicCapacity.Text = ((double)cf1.Length / 1024 / 1024).ToString("0.00") + "MB";
            this.labelPicType.Text = cf1.Extension.TrimStart('.').ToUpper();
            this.labelImageName.Text = _imagePath.Split('\\').Last();
        }

        /// <summary>
        /// 更新控件的Enable属性，防止自动运行时误操作
        /// </summary>
        private void UpdateControlEnabled()
        {
            // 如果是自动播放模式，屏蔽窗口中的所有按钮
            if (IsAutoPlay || IsToolRunning)
            {
                this.cmbCameraSelect.Enabled = false;
                this.txtSelectImagePath.Enabled = false;
                this.btn_SelectImagePath.Enabled = false;
                this.btn_SelectFirstImage.Enabled = false;
                this.btn_SelectLastImage.Enabled = false;
                this.btn_SelectPreviousImage.Enabled = false;
                this.btn_SelectNextImage.Enabled = false;
            }
            // 如果自动播放停止，是能窗口中的所有按钮
            else
            {
                this.cmbCameraSelect.Enabled = true;
                this.txtSelectImagePath.Enabled = true;
                this.btn_SelectImagePath.Enabled = true;
                this.btn_SelectFirstImage.Enabled = true;
                this.btn_SelectLastImage.Enabled = true;
                this.btn_SelectPreviousImage.Enabled = true;
                this.btn_SelectNextImage.Enabled = true;
            }
        }
    }
}
