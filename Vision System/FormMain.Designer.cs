namespace Vision_System
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripHome = new System.Windows.Forms.ToolStripButton();
            this.toolStripCameraSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripPlayBack = new System.Windows.Forms.ToolStripButton();
            this.toolStripStatistics = new System.Windows.Forms.ToolStripButton();
            this.toolStripResetLayout = new System.Windows.Forms.ToolStripButton();
            this.toolStripBackup = new System.Windows.Forms.ToolStripButton();
            this.toolStripRestore = new System.Windows.Forms.ToolStripButton();
            this.toolStripLogin = new System.Windows.Forms.ToolStripButton();
            this.toolStripSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel_PLCOnline = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_CCDStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_IO = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_SerialPort = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_Login = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel_StartupPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.Timer_Auto = new System.Windows.Forms.Timer(this.components);
            this.Timer_Debug = new System.Windows.Forms.Timer(this.components);
            this.Timer_TwinCatAds = new System.Windows.Forms.Timer(this.components);
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.AutoSize = false;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(60, 60);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripHome,
            this.toolStripCameraSetting,
            this.toolStripEdit,
            this.toolStripSetting,
            this.toolStripPlayBack,
            this.toolStripStatistics,
            this.toolStripResetLayout,
            this.toolStripBackup,
            this.toolStripRestore,
            this.toolStripLogin,
            this.toolStripSave,
            this.toolStripPause,
            this.toolStripLabel1});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1532, 65);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripHome
            // 
            this.toolStripHome.AutoSize = false;
            this.toolStripHome.BackColor = System.Drawing.Color.White;
            this.toolStripHome.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripHome.Image = global::Vision_System.Properties.Resources.home;
            this.toolStripHome.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripHome.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripHome.Name = "toolStripHome";
            this.toolStripHome.Size = new System.Drawing.Size(50, 65);
            this.toolStripHome.Text = "主页";
            this.toolStripHome.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripHome.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripCameraSetting
            // 
            this.toolStripCameraSetting.AutoSize = false;
            this.toolStripCameraSetting.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripCameraSetting.Image = global::Vision_System.Properties.Resources.camera_setting;
            this.toolStripCameraSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCameraSetting.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripCameraSetting.Name = "toolStripCameraSetting";
            this.toolStripCameraSetting.Size = new System.Drawing.Size(50, 65);
            this.toolStripCameraSetting.Text = "取像";
            this.toolStripCameraSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripCameraSetting.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripEdit
            // 
            this.toolStripEdit.AutoSize = false;
            this.toolStripEdit.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripEdit.Image = global::Vision_System.Properties.Resources.edit;
            this.toolStripEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripEdit.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripEdit.Name = "toolStripEdit";
            this.toolStripEdit.Size = new System.Drawing.Size(50, 65);
            this.toolStripEdit.Text = "配方";
            this.toolStripEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripEdit.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripSetting
            // 
            this.toolStripSetting.AutoSize = false;
            this.toolStripSetting.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripSetting.Image = global::Vision_System.Properties.Resources.setting;
            this.toolStripSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSetting.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripSetting.Name = "toolStripSetting";
            this.toolStripSetting.Size = new System.Drawing.Size(50, 65);
            this.toolStripSetting.Text = "系统";
            this.toolStripSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripSetting.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripPlayBack
            // 
            this.toolStripPlayBack.AutoSize = false;
            this.toolStripPlayBack.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripPlayBack.Image = global::Vision_System.Properties.Resources.folder;
            this.toolStripPlayBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPlayBack.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripPlayBack.Name = "toolStripPlayBack";
            this.toolStripPlayBack.Size = new System.Drawing.Size(50, 65);
            this.toolStripPlayBack.Text = "回放";
            this.toolStripPlayBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripPlayBack.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripStatistics
            // 
            this.toolStripStatistics.AutoSize = false;
            this.toolStripStatistics.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatistics.Image = global::Vision_System.Properties.Resources.chart;
            this.toolStripStatistics.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStatistics.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripStatistics.Name = "toolStripStatistics";
            this.toolStripStatistics.Size = new System.Drawing.Size(50, 65);
            this.toolStripStatistics.Text = "统计";
            this.toolStripStatistics.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripStatistics.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripResetLayout
            // 
            this.toolStripResetLayout.AutoSize = false;
            this.toolStripResetLayout.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripResetLayout.Image = global::Vision_System.Properties.Resources.Reset_Layout;
            this.toolStripResetLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripResetLayout.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripResetLayout.Name = "toolStripResetLayout";
            this.toolStripResetLayout.Size = new System.Drawing.Size(50, 65);
            this.toolStripResetLayout.Text = "排列";
            this.toolStripResetLayout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripResetLayout.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripBackup
            // 
            this.toolStripBackup.AutoSize = false;
            this.toolStripBackup.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripBackup.Image = global::Vision_System.Properties.Resources.backup_restore;
            this.toolStripBackup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBackup.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripBackup.Name = "toolStripBackup";
            this.toolStripBackup.Size = new System.Drawing.Size(50, 65);
            this.toolStripBackup.Text = "备份";
            this.toolStripBackup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripBackup.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripRestore
            // 
            this.toolStripRestore.AutoSize = false;
            this.toolStripRestore.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripRestore.Image = global::Vision_System.Properties.Resources.download;
            this.toolStripRestore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripRestore.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripRestore.Name = "toolStripRestore";
            this.toolStripRestore.Size = new System.Drawing.Size(50, 65);
            this.toolStripRestore.Text = "恢复";
            this.toolStripRestore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripRestore.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripLogin
            // 
            this.toolStripLogin.AutoSize = false;
            this.toolStripLogin.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLogin.Image = global::Vision_System.Properties.Resources.login;
            this.toolStripLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLogin.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripLogin.Name = "toolStripLogin";
            this.toolStripLogin.Size = new System.Drawing.Size(50, 65);
            this.toolStripLogin.Text = "登录";
            this.toolStripLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripLogin.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripSave
            // 
            this.toolStripSave.AutoSize = false;
            this.toolStripSave.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripSave.Image = global::Vision_System.Properties.Resources.save;
            this.toolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSave.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripSave.Name = "toolStripSave";
            this.toolStripSave.Size = new System.Drawing.Size(50, 65);
            this.toolStripSave.Text = "保存";
            this.toolStripSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripSave.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripPause
            // 
            this.toolStripPause.AutoSize = false;
            this.toolStripPause.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripPause.Image = global::Vision_System.Properties.Resources.pause;
            this.toolStripPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPause.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripPause.Name = "toolStripPause";
            this.toolStripPause.Size = new System.Drawing.Size(50, 65);
            this.toolStripPause.Text = "暂停";
            this.toolStripPause.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripPause.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel1.Image = global::Vision_System.Properties.Resources.Logo_small;
            this.toolStripLabel1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(149, 62);
            this.toolStripLabel1.Text = ".....................................................";
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 65);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1532, 791);
            this.panelMain.TabIndex = 1;
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel_PLCOnline,
            this.StatusLabel_CCDStatus,
            this.StatusLabel_IO,
            this.StatusLabel_SerialPort,
            this.StatusLabel_Login,
            this.StatusLabel_StartupPath});
            this.statusStrip.Location = new System.Drawing.Point(0, 856);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip.Size = new System.Drawing.Size(1532, 29);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // StatusLabel_PLCOnline
            // 
            this.StatusLabel_PLCOnline.AutoSize = false;
            this.StatusLabel_PLCOnline.BackColor = System.Drawing.SystemColors.Control;
            this.StatusLabel_PLCOnline.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
            this.StatusLabel_PLCOnline.Name = "StatusLabel_PLCOnline";
            this.StatusLabel_PLCOnline.Size = new System.Drawing.Size(120, 24);
            this.StatusLabel_PLCOnline.Text = "PLC 在线";
            // 
            // StatusLabel_CCDStatus
            // 
            this.StatusLabel_CCDStatus.AutoSize = false;
            this.StatusLabel_CCDStatus.BackColor = System.Drawing.SystemColors.Control;
            this.StatusLabel_CCDStatus.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
            this.StatusLabel_CCDStatus.Name = "StatusLabel_CCDStatus";
            this.StatusLabel_CCDStatus.Size = new System.Drawing.Size(120, 24);
            this.StatusLabel_CCDStatus.Text = "CCD 在线";
            // 
            // StatusLabel_IO
            // 
            this.StatusLabel_IO.AutoSize = false;
            this.StatusLabel_IO.BackColor = System.Drawing.SystemColors.Control;
            this.StatusLabel_IO.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
            this.StatusLabel_IO.Name = "StatusLabel_IO";
            this.StatusLabel_IO.Size = new System.Drawing.Size(120, 24);
            this.StatusLabel_IO.Text = "IO板卡已连接";
            // 
            // StatusLabel_SerialPort
            // 
            this.StatusLabel_SerialPort.AutoSize = false;
            this.StatusLabel_SerialPort.BackColor = System.Drawing.SystemColors.Control;
            this.StatusLabel_SerialPort.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.StatusLabel_SerialPort.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
            this.StatusLabel_SerialPort.Name = "StatusLabel_SerialPort";
            this.StatusLabel_SerialPort.Size = new System.Drawing.Size(120, 24);
            this.StatusLabel_SerialPort.Text = "TCP 已连接";
            // 
            // StatusLabel_Login
            // 
            this.StatusLabel_Login.AutoSize = false;
            this.StatusLabel_Login.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.StatusLabel_Login.Name = "StatusLabel_Login";
            this.StatusLabel_Login.Size = new System.Drawing.Size(260, 25);
            this.StatusLabel_Login.Text = "当前登陆用用户：Operator";
            // 
            // StatusLabel_StartupPath
            // 
            this.StatusLabel_StartupPath.Name = "StatusLabel_StartupPath";
            this.StatusLabel_StartupPath.Size = new System.Drawing.Size(727, 24);
            this.StatusLabel_StartupPath.Spring = true;
            this.StatusLabel_StartupPath.Text = "StartupPath";
            this.StatusLabel_StartupPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Timer_Auto
            // 
            this.Timer_Auto.Interval = 10;
            this.Timer_Auto.Tick += new System.EventHandler(this.Timer_Auto_Tick);
            // 
            // Timer_Debug
            // 
            this.Timer_Debug.Interval = 1000;
            this.Timer_Debug.Tick += new System.EventHandler(this.Timer_Debug_Tick);
            // 
            // Timer_TwinCatAds
            // 
            this.Timer_TwinCatAds.Tick += new System.EventHandler(this.Timer_TwinCatAds_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1532, 885);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TE Vision";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.StatusStrip statusStrip;
        internal System.Windows.Forms.Timer Timer_Auto;
        private System.Windows.Forms.ToolStripButton toolStripHome;
        private System.Windows.Forms.ToolStripButton toolStripEdit;
        private System.Windows.Forms.ToolStripButton toolStripCameraSetting;
        private System.Windows.Forms.ToolStripButton toolStripPlayBack;
        private System.Windows.Forms.ToolStripButton toolStripStatistics;
        private System.Windows.Forms.ToolStripButton toolStripLogin;
        private System.Windows.Forms.ToolStripButton toolStripSave;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_SerialPort;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_IO;
        private System.Windows.Forms.ToolStripButton toolStripSetting;
        private System.Windows.Forms.ToolStripButton toolStripPause;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_CCDStatus;
        private System.Windows.Forms.Timer Timer_Debug;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Login;
        private System.Windows.Forms.ToolStripButton toolStripBackup;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        internal System.Windows.Forms.Timer Timer_TwinCatAds;
        private System.Windows.Forms.ToolStripButton toolStripResetLayout;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_StartupPath;
        private System.Windows.Forms.ToolStripButton toolStripRestore;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_PLCOnline;
    }
}

