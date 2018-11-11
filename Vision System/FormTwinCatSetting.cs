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
    public partial class FormTwinCatSetting : Form
    {
        private TwincatHelper twinCat = new TwincatHelper();
        public FormTwinCatSetting()
        {
            InitializeComponent();
        }

        private void FormTwinCatSetting_Load(object sender, EventArgs e)
        {
            this.txtADSNetID.Text = FormMain.settingHelper.AdsAmsNetID;
            this.txtADSPortID.Text = FormMain.settingHelper.AdsPortNumber.ToString();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "btnSave":
                    FormMain.settingHelper.AdsAmsNetID = this.txtADSNetID.Text;
                    FormMain.settingHelper.AdsPortNumber = Convert.ToInt16(this.txtADSPortID.Text);
                    FormMain.mainConfigIniFile.IniWriteValue("TwinCat ADS", "AMSNetID", this.txtADSNetID.Text);
                    FormMain.mainConfigIniFile.IniWriteValue("TwinCat ADS", "PortNum", this.txtADSPortID.Text);
                    break;
                case "btnTestConnection":
                    bool success = false;
                    twinCat.AdsAmsNetID = this.txtADSNetID.Text;
                    twinCat.AdsPortNumber = Convert.ToInt16(this.txtADSPortID.Text);
                    success = twinCat.TestAdsConnection();
                    btnTestConnection.BackColor = (success ? Color.LawnGreen : Color.Red);
                    break;
                default:
                    break;
            }
        }
    }
}
