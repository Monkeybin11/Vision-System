namespace Vision_System
{
    partial class FormOmronFinsSetting
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
            this.GroupBox6 = new System.Windows.Forms.GroupBox();
            this.txtDMDataLength = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDMStartAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPLCPort = new System.Windows.Forms.TextBox();
            this.Label62 = new System.Windows.Forms.Label();
            this.btnSaveSetting = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.txtPLCIP = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.bt_FinsRead = new System.Windows.Forms.Button();
            this.bt_FinsWrite = new System.Windows.Forms.Button();
            this.tb_Value = new System.Windows.Forms.TextBox();
            this.tb_Address = new System.Windows.Forms.TextBox();
            this.Label34 = new System.Windows.Forms.Label();
            this.Label33 = new System.Windows.Forms.Label();
            this.comBox_MemrryType = new System.Windows.Forms.ComboBox();
            this.Label28 = new System.Windows.Forms.Label();
            this.cBox_FinsEnable = new System.Windows.Forms.CheckBox();
            this.GroupBox6.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox6
            // 
            this.GroupBox6.Controls.Add(this.txtDMDataLength);
            this.GroupBox6.Controls.Add(this.label1);
            this.GroupBox6.Controls.Add(this.txtDMStartAddress);
            this.GroupBox6.Controls.Add(this.label2);
            this.GroupBox6.Controls.Add(this.txtPLCPort);
            this.GroupBox6.Controls.Add(this.Label62);
            this.GroupBox6.Controls.Add(this.btnSaveSetting);
            this.GroupBox6.Controls.Add(this.btnTestConnection);
            this.GroupBox6.Controls.Add(this.txtPLCIP);
            this.GroupBox6.Controls.Add(this.Label9);
            this.GroupBox6.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox6.Location = new System.Drawing.Point(23, 13);
            this.GroupBox6.Margin = new System.Windows.Forms.Padding(4);
            this.GroupBox6.Name = "GroupBox6";
            this.GroupBox6.Padding = new System.Windows.Forms.Padding(4);
            this.GroupBox6.Size = new System.Drawing.Size(259, 238);
            this.GroupBox6.TabIndex = 49;
            this.GroupBox6.TabStop = false;
            this.GroupBox6.Text = "PLC 设置";
            // 
            // txtDMDataLength
            // 
            this.txtDMDataLength.Location = new System.Drawing.Point(100, 148);
            this.txtDMDataLength.Margin = new System.Windows.Forms.Padding(4);
            this.txtDMDataLength.Name = "txtDMDataLength";
            this.txtDMDataLength.Size = new System.Drawing.Size(136, 25);
            this.txtDMDataLength.TabIndex = 8;
            this.txtDMDataLength.Text = "16";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 150);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "数据长度：";
            // 
            // txtDMStartAddress
            // 
            this.txtDMStartAddress.Location = new System.Drawing.Point(100, 110);
            this.txtDMStartAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtDMStartAddress.Name = "txtDMStartAddress";
            this.txtDMStartAddress.Size = new System.Drawing.Size(136, 25);
            this.txtDMStartAddress.TabIndex = 6;
            this.txtDMStartAddress.Text = "4000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 112);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "起始地址：";
            // 
            // txtPLCPort
            // 
            this.txtPLCPort.Location = new System.Drawing.Point(100, 72);
            this.txtPLCPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPLCPort.Name = "txtPLCPort";
            this.txtPLCPort.Size = new System.Drawing.Size(136, 25);
            this.txtPLCPort.TabIndex = 4;
            this.txtPLCPort.Text = "6001";
            // 
            // Label62
            // 
            this.Label62.AutoSize = true;
            this.Label62.Location = new System.Drawing.Point(21, 74);
            this.Label62.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label62.Name = "Label62";
            this.Label62.Size = new System.Drawing.Size(65, 20);
            this.Label62.TabIndex = 3;
            this.Label62.Text = "端口号：";
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.Location = new System.Drawing.Point(21, 186);
            this.btnSaveSetting.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(90, 37);
            this.btnSaveSetting.TabIndex = 2;
            this.btnSaveSetting.Text = "保存";
            this.btnSaveSetting.UseVisualStyleBackColor = true;
            this.btnSaveSetting.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(131, 186);
            this.btnTestConnection.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(90, 37);
            this.btnTestConnection.TabIndex = 2;
            this.btnTestConnection.Text = "连接测试";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btn_Click);
            // 
            // txtPLCIP
            // 
            this.txtPLCIP.Location = new System.Drawing.Point(100, 34);
            this.txtPLCIP.Margin = new System.Windows.Forms.Padding(4);
            this.txtPLCIP.Name = "txtPLCIP";
            this.txtPLCIP.Size = new System.Drawing.Size(136, 25);
            this.txtPLCIP.TabIndex = 1;
            this.txtPLCIP.Text = "192.168.0.2";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(21, 36);
            this.Label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(65, 20);
            this.Label9.TabIndex = 0;
            this.Label9.Text = "PLC IP：";
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.bt_FinsRead);
            this.GroupBox4.Controls.Add(this.bt_FinsWrite);
            this.GroupBox4.Controls.Add(this.tb_Value);
            this.GroupBox4.Controls.Add(this.tb_Address);
            this.GroupBox4.Controls.Add(this.Label34);
            this.GroupBox4.Controls.Add(this.Label33);
            this.GroupBox4.Controls.Add(this.comBox_MemrryType);
            this.GroupBox4.Controls.Add(this.Label28);
            this.GroupBox4.Controls.Add(this.cBox_FinsEnable);
            this.GroupBox4.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox4.Location = new System.Drawing.Point(305, 13);
            this.GroupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.GroupBox4.Size = new System.Drawing.Size(299, 238);
            this.GroupBox4.TabIndex = 48;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Omron Fins 测试";
            // 
            // bt_FinsRead
            // 
            this.bt_FinsRead.Enabled = false;
            this.bt_FinsRead.Location = new System.Drawing.Point(179, 183);
            this.bt_FinsRead.Margin = new System.Windows.Forms.Padding(4);
            this.bt_FinsRead.Name = "bt_FinsRead";
            this.bt_FinsRead.Size = new System.Drawing.Size(95, 40);
            this.bt_FinsRead.TabIndex = 50;
            this.bt_FinsRead.Text = "读取";
            this.bt_FinsRead.UseVisualStyleBackColor = true;
            // 
            // bt_FinsWrite
            // 
            this.bt_FinsWrite.Enabled = false;
            this.bt_FinsWrite.Location = new System.Drawing.Point(30, 183);
            this.bt_FinsWrite.Margin = new System.Windows.Forms.Padding(4);
            this.bt_FinsWrite.Name = "bt_FinsWrite";
            this.bt_FinsWrite.Size = new System.Drawing.Size(95, 40);
            this.bt_FinsWrite.TabIndex = 49;
            this.bt_FinsWrite.Text = "写入";
            this.bt_FinsWrite.UseVisualStyleBackColor = true;
            // 
            // tb_Value
            // 
            this.tb_Value.Enabled = false;
            this.tb_Value.Location = new System.Drawing.Point(114, 137);
            this.tb_Value.Margin = new System.Windows.Forms.Padding(4);
            this.tb_Value.Name = "tb_Value";
            this.tb_Value.Size = new System.Drawing.Size(160, 25);
            this.tb_Value.TabIndex = 48;
            // 
            // tb_Address
            // 
            this.tb_Address.Enabled = false;
            this.tb_Address.Location = new System.Drawing.Point(114, 98);
            this.tb_Address.Margin = new System.Windows.Forms.Padding(4);
            this.tb_Address.Name = "tb_Address";
            this.tb_Address.Size = new System.Drawing.Size(160, 25);
            this.tb_Address.TabIndex = 47;
            // 
            // Label34
            // 
            this.Label34.AutoSize = true;
            this.Label34.Location = new System.Drawing.Point(27, 141);
            this.Label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label34.Name = "Label34";
            this.Label34.Size = new System.Drawing.Size(79, 20);
            this.Label34.TabIndex = 46;
            this.Label34.Text = "目标数值：";
            // 
            // Label33
            // 
            this.Label33.AutoSize = true;
            this.Label33.Location = new System.Drawing.Point(27, 102);
            this.Label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label33.Name = "Label33";
            this.Label33.Size = new System.Drawing.Size(79, 20);
            this.Label33.TabIndex = 45;
            this.Label33.Text = "写入地址：";
            // 
            // comBox_MemrryType
            // 
            this.comBox_MemrryType.Enabled = false;
            this.comBox_MemrryType.FormattingEnabled = true;
            this.comBox_MemrryType.Items.AddRange(new object[] {
            "CIO",
            "WR",
            "DM"});
            this.comBox_MemrryType.Location = new System.Drawing.Point(114, 59);
            this.comBox_MemrryType.Margin = new System.Windows.Forms.Padding(4);
            this.comBox_MemrryType.Name = "comBox_MemrryType";
            this.comBox_MemrryType.Size = new System.Drawing.Size(160, 27);
            this.comBox_MemrryType.TabIndex = 44;
            this.comBox_MemrryType.Text = "DM";
            // 
            // Label28
            // 
            this.Label28.AutoSize = true;
            this.Label28.Location = new System.Drawing.Point(27, 59);
            this.Label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label28.Name = "Label28";
            this.Label28.Size = new System.Drawing.Size(79, 20);
            this.Label28.TabIndex = 43;
            this.Label28.Text = "内存类型：";
            // 
            // cBox_FinsEnable
            // 
            this.cBox_FinsEnable.AutoSize = true;
            this.cBox_FinsEnable.Location = new System.Drawing.Point(10, 25);
            this.cBox_FinsEnable.Margin = new System.Windows.Forms.Padding(4);
            this.cBox_FinsEnable.Name = "cBox_FinsEnable";
            this.cBox_FinsEnable.Size = new System.Drawing.Size(113, 24);
            this.cBox_FinsEnable.TabIndex = 42;
            this.cBox_FinsEnable.Text = "手动测试Fins";
            this.cBox_FinsEnable.UseVisualStyleBackColor = true;
            // 
            // FormOmronFinsSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 277);
            this.Controls.Add(this.GroupBox6);
            this.Controls.Add(this.GroupBox4);
            this.Name = "FormOmronFinsSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Omron Fins设置";
            this.Load += new System.EventHandler(this.FormOmronFinsSetting_Load);
            this.GroupBox6.ResumeLayout(false);
            this.GroupBox6.PerformLayout();
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox6;
        internal System.Windows.Forms.TextBox txtPLCPort;
        internal System.Windows.Forms.Label Label62;
        internal System.Windows.Forms.Button btnTestConnection;
        internal System.Windows.Forms.TextBox txtPLCIP;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.GroupBox GroupBox4;
        internal System.Windows.Forms.Button bt_FinsRead;
        internal System.Windows.Forms.Button bt_FinsWrite;
        internal System.Windows.Forms.TextBox tb_Value;
        internal System.Windows.Forms.TextBox tb_Address;
        internal System.Windows.Forms.Label Label34;
        internal System.Windows.Forms.Label Label33;
        internal System.Windows.Forms.ComboBox comBox_MemrryType;
        internal System.Windows.Forms.Label Label28;
        internal System.Windows.Forms.CheckBox cBox_FinsEnable;
        internal System.Windows.Forms.Button btnSaveSetting;
        internal System.Windows.Forms.TextBox txtDMDataLength;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox txtDMStartAddress;
        internal System.Windows.Forms.Label label2;
    }
}