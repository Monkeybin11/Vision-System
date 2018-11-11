namespace Vision_System
{
    partial class FormParamSetting
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRecordSelect = new System.Windows.Forms.ComboBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.btnCCDAcqDelaySave = new System.Windows.Forms.Button();
            this.txtDoOutTime = new System.Windows.Forms.TextBox();
            this.txtCCDAcqDelayTime = new System.Windows.Forms.TextBox();
            this.lblCCD1AcqDelayTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgvOverallInput = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkExternalControlLight = new System.Windows.Forms.CheckBox();
            this.chkSoftControlLight = new System.Windows.Forms.CheckBox();
            this.radioLightFlash = new System.Windows.Forms.RadioButton();
            this.radioLightON = new System.Windows.Forms.RadioButton();
            this.cmbLightSelect = new System.Windows.Forms.ComboBox();
            this.labelLight = new System.Windows.Forms.Label();
            this.btnLightSourceSave = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkHardwareTrigger = new System.Windows.Forms.CheckBox();
            this.chkSoftwareTrigger = new System.Windows.Forms.CheckBox();
            this.cmbTriggerSelect = new System.Windows.Forms.ComboBox();
            this.labelTrigger = new System.Windows.Forms.Label();
            this.btnTriggerSourceSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbShowDataSelect = new System.Windows.Forms.ComboBox();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOverallInput)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(155, 751);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 33);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(303, 751);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(98, 33);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "选择图像Record：";
            // 
            // cmbRecordSelect
            // 
            this.cmbRecordSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRecordSelect.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRecordSelect.FormattingEnabled = true;
            this.cmbRecordSelect.Location = new System.Drawing.Point(139, 19);
            this.cmbRecordSelect.Name = "cmbRecordSelect";
            this.cmbRecordSelect.Size = new System.Drawing.Size(426, 27);
            this.cmbRecordSelect.TabIndex = 6;
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label16.Location = new System.Drawing.Point(254, 26);
            this.Label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(107, 20);
            this.Label16.TabIndex = 17;
            this.Label16.Text = "信号输出脉冲：";
            // 
            // btnCCDAcqDelaySave
            // 
            this.btnCCDAcqDelaySave.BackColor = System.Drawing.SystemColors.Control;
            this.btnCCDAcqDelaySave.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCCDAcqDelaySave.Location = new System.Drawing.Point(477, 19);
            this.btnCCDAcqDelaySave.Name = "btnCCDAcqDelaySave";
            this.btnCCDAcqDelaySave.Size = new System.Drawing.Size(72, 35);
            this.btnCCDAcqDelaySave.TabIndex = 14;
            this.btnCCDAcqDelaySave.Text = "保存";
            this.btnCCDAcqDelaySave.UseVisualStyleBackColor = true;
            this.btnCCDAcqDelaySave.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // txtDoOutTime
            // 
            this.txtDoOutTime.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDoOutTime.Location = new System.Drawing.Point(361, 24);
            this.txtDoOutTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtDoOutTime.Name = "txtDoOutTime";
            this.txtDoOutTime.Size = new System.Drawing.Size(80, 25);
            this.txtDoOutTime.TabIndex = 16;
            this.txtDoOutTime.Text = "200";
            // 
            // txtCCDAcqDelayTime
            // 
            this.txtCCDAcqDelayTime.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCCDAcqDelayTime.Location = new System.Drawing.Point(127, 24);
            this.txtCCDAcqDelayTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtCCDAcqDelayTime.Name = "txtCCDAcqDelayTime";
            this.txtCCDAcqDelayTime.Size = new System.Drawing.Size(80, 25);
            this.txtCCDAcqDelayTime.TabIndex = 15;
            this.txtCCDAcqDelayTime.Text = "100";
            // 
            // lblCCD1AcqDelayTime
            // 
            this.lblCCD1AcqDelayTime.AutoSize = true;
            this.lblCCD1AcqDelayTime.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCCD1AcqDelayTime.Location = new System.Drawing.Point(17, 26);
            this.lblCCD1AcqDelayTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCCD1AcqDelayTime.Name = "lblCCD1AcqDelayTime";
            this.lblCCD1AcqDelayTime.Size = new System.Drawing.Size(112, 20);
            this.lblCCD1AcqDelayTime.TabIndex = 13;
            this.lblCCD1AcqDelayTime.Text = "CCD 拍照延时：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(211, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "ms";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(442, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "ms";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtDoOutTime);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtCCDAcqDelayTime);
            this.groupBox3.Controls.Add(this.btnCCDAcqDelaySave);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.Label16);
            this.groupBox3.Controls.Add(this.lblCCD1AcqDelayTime);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 99);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(556, 64);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "延时设置";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgvOverallInput);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 393);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(556, 344);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "输入参数调整";
            // 
            // dgvOverallInput
            // 
            this.dgvOverallInput.AllowUserToAddRows = false;
            this.dgvOverallInput.AllowUserToDeleteRows = false;
            this.dgvOverallInput.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvOverallInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOverallInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOverallInput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgvOverallInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOverallInput.Location = new System.Drawing.Point(3, 21);
            this.dgvOverallInput.Name = "dgvOverallInput";
            this.dgvOverallInput.RowHeadersVisible = false;
            this.dgvOverallInput.RowTemplate.Height = 24;
            this.dgvOverallInput.Size = new System.Drawing.Size(550, 320);
            this.dgvOverallInput.TabIndex = 20;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "变量名";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "变量类型";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "值";
            this.Column3.Name = "Column3";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.chkExternalControlLight);
            this.groupBox5.Controls.Add(this.chkSoftControlLight);
            this.groupBox5.Controls.Add(this.radioLightFlash);
            this.groupBox5.Controls.Add(this.radioLightON);
            this.groupBox5.Controls.Add(this.cmbLightSelect);
            this.groupBox5.Controls.Add(this.labelLight);
            this.groupBox5.Controls.Add(this.btnLightSourceSave);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(12, 268);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(556, 121);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "光源设置";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Info;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(13, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(483, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "(软件控制：上位机软件通过IO直接控制光源打开关闭，外部控制：光源交给外部PLC控制)";
            // 
            // chkExternalControlLight
            // 
            this.chkExternalControlLight.AutoSize = true;
            this.chkExternalControlLight.Location = new System.Drawing.Point(127, 27);
            this.chkExternalControlLight.Name = "chkExternalControlLight";
            this.chkExternalControlLight.Size = new System.Drawing.Size(87, 24);
            this.chkExternalControlLight.TabIndex = 21;
            this.chkExternalControlLight.Text = "外部控制";
            this.chkExternalControlLight.UseVisualStyleBackColor = true;
            this.chkExternalControlLight.CheckedChanged += new System.EventHandler(this.chkMultiple_CheckedChanged);
            // 
            // chkSoftControlLight
            // 
            this.chkSoftControlLight.AutoSize = true;
            this.chkSoftControlLight.Location = new System.Drawing.Point(17, 27);
            this.chkSoftControlLight.Name = "chkSoftControlLight";
            this.chkSoftControlLight.Size = new System.Drawing.Size(87, 24);
            this.chkSoftControlLight.TabIndex = 21;
            this.chkSoftControlLight.Text = "软件控制";
            this.chkSoftControlLight.UseVisualStyleBackColor = true;
            this.chkSoftControlLight.CheckedChanged += new System.EventHandler(this.chkMultiple_CheckedChanged);
            // 
            // radioLightFlash
            // 
            this.radioLightFlash.AutoSize = true;
            this.radioLightFlash.Checked = true;
            this.radioLightFlash.Location = new System.Drawing.Point(394, 58);
            this.radioLightFlash.Name = "radioLightFlash";
            this.radioLightFlash.Size = new System.Drawing.Size(58, 24);
            this.radioLightFlash.TabIndex = 20;
            this.radioLightFlash.TabStop = true;
            this.radioLightFlash.Text = "频闪";
            this.radioLightFlash.UseVisualStyleBackColor = true;
            // 
            // radioLightON
            // 
            this.radioLightON.AutoSize = true;
            this.radioLightON.Location = new System.Drawing.Point(311, 58);
            this.radioLightON.Name = "radioLightON";
            this.radioLightON.Size = new System.Drawing.Size(58, 24);
            this.radioLightON.TabIndex = 20;
            this.radioLightON.Text = "常亮";
            this.radioLightON.UseVisualStyleBackColor = true;
            // 
            // cmbLightSelect
            // 
            this.cmbLightSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLightSelect.FormattingEnabled = true;
            this.cmbLightSelect.Location = new System.Drawing.Point(127, 57);
            this.cmbLightSelect.Name = "cmbLightSelect";
            this.cmbLightSelect.Size = new System.Drawing.Size(149, 27);
            this.cmbLightSelect.TabIndex = 19;
            // 
            // labelLight
            // 
            this.labelLight.AutoSize = true;
            this.labelLight.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLight.Location = new System.Drawing.Point(17, 60);
            this.labelLight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLight.Name = "labelLight";
            this.labelLight.Size = new System.Drawing.Size(79, 20);
            this.labelLight.TabIndex = 17;
            this.labelLight.Text = "光源选择：";
            // 
            // btnLightSourceSave
            // 
            this.btnLightSourceSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnLightSourceSave.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLightSourceSave.Location = new System.Drawing.Point(477, 52);
            this.btnLightSourceSave.Name = "btnLightSourceSave";
            this.btnLightSourceSave.Size = new System.Drawing.Size(72, 35);
            this.btnLightSourceSave.TabIndex = 14;
            this.btnLightSourceSave.Text = "保存";
            this.btnLightSourceSave.UseVisualStyleBackColor = true;
            this.btnLightSourceSave.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.chkHardwareTrigger);
            this.groupBox6.Controls.Add(this.chkSoftwareTrigger);
            this.groupBox6.Controls.Add(this.cmbTriggerSelect);
            this.groupBox6.Controls.Add(this.labelTrigger);
            this.groupBox6.Controls.Add(this.btnTriggerSourceSave);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(12, 167);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(556, 95);
            this.groupBox6.TabIndex = 18;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "触发设置";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.Info;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(13, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(483, 17);
            this.label8.TabIndex = 20;
            this.label8.Text = "(软件触发：上位机软件捕获触发信号进行拍照，自动触发：PLC直接通过相机IO触发拍照)";
            // 
            // chkHardwareTrigger
            // 
            this.chkHardwareTrigger.AutoSize = true;
            this.chkHardwareTrigger.Location = new System.Drawing.Point(127, 27);
            this.chkHardwareTrigger.Name = "chkHardwareTrigger";
            this.chkHardwareTrigger.Size = new System.Drawing.Size(87, 24);
            this.chkHardwareTrigger.TabIndex = 22;
            this.chkHardwareTrigger.Text = "自动触发";
            this.chkHardwareTrigger.UseVisualStyleBackColor = true;
            this.chkHardwareTrigger.CheckedChanged += new System.EventHandler(this.chkMultiple_CheckedChanged);
            // 
            // chkSoftwareTrigger
            // 
            this.chkSoftwareTrigger.AutoSize = true;
            this.chkSoftwareTrigger.Location = new System.Drawing.Point(17, 27);
            this.chkSoftwareTrigger.Name = "chkSoftwareTrigger";
            this.chkSoftwareTrigger.Size = new System.Drawing.Size(87, 24);
            this.chkSoftwareTrigger.TabIndex = 23;
            this.chkSoftwareTrigger.Text = "软件触发";
            this.chkSoftwareTrigger.UseVisualStyleBackColor = true;
            this.chkSoftwareTrigger.CheckedChanged += new System.EventHandler(this.chkMultiple_CheckedChanged);
            // 
            // cmbTriggerSelect
            // 
            this.cmbTriggerSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTriggerSelect.FormattingEnabled = true;
            this.cmbTriggerSelect.Location = new System.Drawing.Point(311, 26);
            this.cmbTriggerSelect.Name = "cmbTriggerSelect";
            this.cmbTriggerSelect.Size = new System.Drawing.Size(159, 27);
            this.cmbTriggerSelect.TabIndex = 19;
            // 
            // labelTrigger
            // 
            this.labelTrigger.AutoSize = true;
            this.labelTrigger.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTrigger.Location = new System.Drawing.Point(241, 29);
            this.labelTrigger.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTrigger.Name = "labelTrigger";
            this.labelTrigger.Size = new System.Drawing.Size(65, 20);
            this.labelTrigger.TabIndex = 17;
            this.labelTrigger.Text = "触发源：";
            // 
            // btnTriggerSourceSave
            // 
            this.btnTriggerSourceSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnTriggerSourceSave.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTriggerSourceSave.Location = new System.Drawing.Point(477, 22);
            this.btnTriggerSourceSave.Name = "btnTriggerSourceSave";
            this.btnTriggerSourceSave.Size = new System.Drawing.Size(72, 35);
            this.btnTriggerSourceSave.TabIndex = 14;
            this.btnTriggerSourceSave.Text = "保存";
            this.btnTriggerSourceSave.UseVisualStyleBackColor = true;
            this.btnTriggerSourceSave.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "选择图表数据：";
            // 
            // cmbShowDataSelect
            // 
            this.cmbShowDataSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShowDataSelect.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbShowDataSelect.FormattingEnabled = true;
            this.cmbShowDataSelect.Location = new System.Drawing.Point(139, 60);
            this.cmbShowDataSelect.Name = "cmbShowDataSelect";
            this.cmbShowDataSelect.Size = new System.Drawing.Size(426, 27);
            this.cmbShowDataSelect.TabIndex = 6;
            // 
            // FormParamSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 796);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.cmbShowDataSelect);
            this.Controls.Add(this.cmbRecordSelect);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormParamSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "参数调整";
            this.Load += new System.EventHandler(this.FormParamSetting_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOverallInput)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRecordSelect;
        internal System.Windows.Forms.Label Label16;
        private System.Windows.Forms.Button btnCCDAcqDelaySave;
        internal System.Windows.Forms.TextBox txtDoOutTime;
        internal System.Windows.Forms.TextBox txtCCDAcqDelayTime;
        internal System.Windows.Forms.Label lblCCD1AcqDelayTime;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dgvOverallInput;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.GroupBox groupBox5;
        internal System.Windows.Forms.Label labelLight;
        private System.Windows.Forms.Button btnLightSourceSave;
        private System.Windows.Forms.ComboBox cmbLightSelect;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cmbTriggerSelect;
        internal System.Windows.Forms.Label labelTrigger;
        private System.Windows.Forms.Button btnTriggerSourceSave;
        private System.Windows.Forms.RadioButton radioLightFlash;
        private System.Windows.Forms.RadioButton radioLightON;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbShowDataSelect;
        private System.Windows.Forms.CheckBox chkExternalControlLight;
        private System.Windows.Forms.CheckBox chkSoftControlLight;
        private System.Windows.Forms.CheckBox chkHardwareTrigger;
        private System.Windows.Forms.CheckBox chkSoftwareTrigger;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}