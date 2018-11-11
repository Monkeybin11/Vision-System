using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VisionPro.ToolBlock;

namespace Vision_System
{
    public partial class PageEdit : UserControl
    {
        private CogToolBlockEditV2[] cogToolBlockEditV2;
        private TabPage[] tabPage;

        public PageEdit()
        {
            InitializeComponent();
        }

        private void PageEdit_Load(object sender, EventArgs e)
        {

        }

        public void SetupToolBlockEditSubject(CogToolBlock[] tool)
        {
            cogToolBlockEditV2 = new CogToolBlockEditV2[FormMain.camNumber];
            tabPage = new TabPage[FormMain.camNumber];
            this.tabControl1.Controls.Clear();
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                // 
                // cogAcqFifoEdit
                // 
                this.cogToolBlockEditV2[i] = new CogToolBlockEditV2();
                this.cogToolBlockEditV2[i].Dock = System.Windows.Forms.DockStyle.Fill;
                this.cogToolBlockEditV2[i].Location = new System.Drawing.Point(3, 3);
                this.cogToolBlockEditV2[i].Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                this.cogToolBlockEditV2[i].MinimumSize = new System.Drawing.Size(489, 0);
                this.cogToolBlockEditV2[i].Name = "cogToolBlockEdit_CCD" + (i + 1);
                this.cogToolBlockEditV2[i].Size = new System.Drawing.Size(986, 562);
                this.cogToolBlockEditV2[i].SuspendElectricRuns = false;
                this.cogToolBlockEditV2[i].TabIndex = i + 2;
                // 
                // tabPage1
                // 
                this.tabPage[i] = new TabPage();
                this.tabPage[i].Controls.Add(this.cogToolBlockEditV2[i]);
                this.tabPage[i].Location = new System.Drawing.Point(4, 28);
                this.tabPage[i].Name = "tabPage" + (i + 1);
                this.tabPage[i].Padding = new System.Windows.Forms.Padding(3);
                this.tabPage[i].Size = new System.Drawing.Size(992, 568);
                this.tabPage[i].TabIndex = 0;
                this.tabPage[i].Text = FormMain.jobHelper[i].CcdName + "编辑";
                this.tabPage[i].UseVisualStyleBackColor = true;
                // 
                // tabControl
                // 
                this.tabControl1.Controls.Add(tabPage[i]);
            }
            for (int i = 0; i < FormMain.camNumber; i++)
            {
                cogToolBlockEditV2[i].Subject = tool[i];
            }
        }

        public void SetupToolBlockEditSubject(CogToolBlock tool, int index)
        {
            cogToolBlockEditV2[index].Subject = tool;
        }
    }
}
