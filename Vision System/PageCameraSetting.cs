using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro;

namespace Vision_System
{
    public partial class PageCameraSetting : UserControl
    {
        private CogAcqFifoEditV2[] acqFifoEditV2;
        private TabPage[] tabPage;

        public PageCameraSetting()
        {
            InitializeComponent();
        }

        private void PageCameraSetting_Load(object sender, EventArgs e)
        {

        }

        public void SetupToolGroupEditSubject(CogToolGroup tool)
        {
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                acqFifoEditV2[i].Subject = (CogAcqFifoTool)tool.Tools[i];
            }
        }

        public void SetupAcqToolEditSubject(CogAcqFifoTool[] tool)
        {
            acqFifoEditV2 = new CogAcqFifoEditV2[FormMain.camNumber];
            tabPage = new TabPage[FormMain.camNumber];
            this.tabControl1.Controls.Clear();
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                // 
                // cogAcqFifoEdit
                // 
                this.acqFifoEditV2[i] = new CogAcqFifoEditV2();
                this.acqFifoEditV2[i].Dock = System.Windows.Forms.DockStyle.Fill;
                this.acqFifoEditV2[i].Location = new System.Drawing.Point(3, 3);
                this.acqFifoEditV2[i].Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                this.acqFifoEditV2[i].MinimumSize = new System.Drawing.Size(489, 0);
                this.acqFifoEditV2[i].Name = "cogAcqFifoEdit_CCD" + (i + 1);
                this.acqFifoEditV2[i].Size = new System.Drawing.Size(986, 562);
                this.acqFifoEditV2[i].SuspendElectricRuns = false;
                this.acqFifoEditV2[i].TabIndex = i + 2;
                // 
                // tabPage1
                // 
                this.tabPage[i] = new TabPage();
                this.tabPage[i].Controls.Add(this.acqFifoEditV2[i]);
                this.tabPage[i].Location = new System.Drawing.Point(4, 28);
                this.tabPage[i].Name = "tabPage" + (i + 1);
                this.tabPage[i].Padding = new System.Windows.Forms.Padding(3);
                this.tabPage[i].Size = new System.Drawing.Size(992, 568);
                this.tabPage[i].TabIndex = 0;
                this.tabPage[i].Text = FormMain.jobHelper[i].CcdName + "相机";
                this.tabPage[i].UseVisualStyleBackColor = true;
                // 
                // tabControl
                // 
                this.tabControl1.Controls.Add(tabPage[i]);
            }
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                acqFifoEditV2[i].Subject = tool[i];
            }
        }

        public void SetupAcqToolEditSubject(CogAcqFifoTool tool, int index)
        {
            acqFifoEditV2[index].Subject = tool;
        }
    }
}
