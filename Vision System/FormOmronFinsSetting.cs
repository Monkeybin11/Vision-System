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
    public partial class FormOmronFinsSetting : Form
    {
        private OmronFinsHelper fins = new OmronFinsHelper();

        public FormOmronFinsSetting()
        {
            InitializeComponent();
        }

        private void FormOmronFinsSetting_Load(object sender, EventArgs e)
        {
            txtPLCIP.Text = FormMain.settingHelper.PLCIP;
            txtPLCPort.Text = FormMain.settingHelper.PLCPort.ToString();
            txtDMStartAddress.Text = FormMain.settingHelper.DMStartAddress.ToString();
            txtDMDataLength.Text = FormMain.settingHelper.DMDataLength.ToString();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "btnSaveSetting":
                    FormMain.settingHelper.PLCIP = txtPLCIP.Text;
                    FormMain.settingHelper.PLCPort = Convert.ToInt16(txtPLCPort.Text);
                    FormMain.settingHelper.DMStartAddress = Convert.ToInt16(txtDMStartAddress.Text);
                    FormMain.settingHelper.DMDataLength = Convert.ToInt16(txtDMDataLength.Text);
                    FormMain.mainConfigIniFile.IniWriteValue("PLC Parameter", "IP", txtPLCIP.Text);
                    FormMain.mainConfigIniFile.IniWriteValue("PLC Parameter", "Port", txtPLCPort.Text);
                    FormMain.mainConfigIniFile.IniWriteValue("PLC Parameter", "StartAdd", txtDMStartAddress.Text);
                    FormMain.mainConfigIniFile.IniWriteValue("PLC Parameter", "DataLength", txtDMDataLength.Text);
                    break;
                case "btnTestConnection":
                    bool success = false;
                    fins.mPLCIP = txtPLCIP.Text;
                    fins.mPLCPort = Convert.ToInt16(txtPLCPort.Text);
                    success = fins.TestFinsConnection();
                    btnTestConnection.BackColor = (success ? Color.LawnGreen : Color.Red);
                    break;
            }
        }
    }
}
