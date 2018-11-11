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
    public enum FormSettingOptionResult
    {
        None,
        PartNoManage,
        ImageDataSetting,
        IOSetting,
        TwinCatSetting,
        OmronFinsSetting,
        CommFormatSetting,
        FailureSetting
    }

    public partial class FormSetting : Form
    {
        private FormSettingOptionResult mSettingResult = FormSettingOptionResult.None;
        private bool enableIOSettingButton = false;
        private bool enableTwinCatSettingButton = false;
        private bool enableOmronFinsSettingButton = false;
        private bool enableCommFormatSettingButton = false;

        public FormSettingOptionResult Result
        {
            get { return mSettingResult; }
        }

        public FormSetting(bool enableio, bool enabletwincat, bool enablefins, bool enablecomm)
        {
            InitializeComponent();
            enableIOSettingButton = enableio;
            enableTwinCatSettingButton = enabletwincat;
            enableOmronFinsSettingButton = enablefins;
            enableCommFormatSettingButton = enablecomm;
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            btnIOSetting.Enabled = enableIOSettingButton;
            btnTwinCatSetting.Enabled = enableTwinCatSettingButton;
            btnOmronFinsSetting.Enabled = enableOmronFinsSettingButton;
            btnCommFormatSetting.Enabled = enableCommFormatSettingButton;
        }

        private void btnOptionSelect_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "btnProductManage":
                    mSettingResult = FormSettingOptionResult.PartNoManage;
                    this.Close();
                    break;
                case "btnImageDataSetting":
                    mSettingResult = FormSettingOptionResult.ImageDataSetting;
                    this.Close();
                    break;
                case "btnIOSetting":
                    mSettingResult = FormSettingOptionResult.IOSetting;
                    this.Close();
                    break;
                case "btnTwinCatSetting":
                    mSettingResult = FormSettingOptionResult.TwinCatSetting;
                    this.Close();
                    break;
                case "btnOmronFinsSetting":
                    mSettingResult = FormSettingOptionResult.OmronFinsSetting;
                    this.Close();
                    break;
                case "btnCommFormatSetting":
                    mSettingResult = FormSettingOptionResult.CommFormatSetting;
                    this.Close();
                    break;
                case "btnFailureSetting":
                    mSettingResult = FormSettingOptionResult.FailureSetting;
                    this.Close();
                    break;
                default:
                    break;
            }
        }
    }
}
