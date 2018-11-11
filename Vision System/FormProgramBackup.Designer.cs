namespace Vision_System
{
    partial class FormProgramBackup
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
            this.radioDateFolderName = new System.Windows.Forms.RadioButton();
            this.radioCustomizedFolderName = new System.Windows.Forms.RadioButton();
            this.radioCustomizedFolderPath = new System.Windows.Forms.RadioButton();
            this.txtFolderName = new System.Windows.Forms.TextBox();
            this.btnBrowseFolder = new System.Windows.Forms.Button();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelFolderName = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioDateFolderName
            // 
            this.radioDateFolderName.AutoSize = true;
            this.radioDateFolderName.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioDateFolderName.Location = new System.Drawing.Point(26, 29);
            this.radioDateFolderName.Name = "radioDateFolderName";
            this.radioDateFolderName.Size = new System.Drawing.Size(198, 24);
            this.radioDateFolderName.TabIndex = 0;
            this.radioDateFolderName.TabStop = true;
            this.radioDateFolderName.Text = "按日期命名文件夹（默认）";
            this.radioDateFolderName.UseVisualStyleBackColor = true;
            this.radioDateFolderName.CheckedChanged += new System.EventHandler(this.radioDateFolderName_CheckedChanged);
            // 
            // radioCustomizedFolderName
            // 
            this.radioCustomizedFolderName.AutoSize = true;
            this.radioCustomizedFolderName.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioCustomizedFolderName.Location = new System.Drawing.Point(26, 71);
            this.radioCustomizedFolderName.Name = "radioCustomizedFolderName";
            this.radioCustomizedFolderName.Size = new System.Drawing.Size(142, 24);
            this.radioCustomizedFolderName.TabIndex = 0;
            this.radioCustomizedFolderName.TabStop = true;
            this.radioCustomizedFolderName.Text = "自定义文件夹名称";
            this.radioCustomizedFolderName.UseVisualStyleBackColor = true;
            this.radioCustomizedFolderName.CheckedChanged += new System.EventHandler(this.radioCustomizedFolderName_CheckedChanged);
            // 
            // radioCustomizedFolderPath
            // 
            this.radioCustomizedFolderPath.AutoSize = true;
            this.radioCustomizedFolderPath.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioCustomizedFolderPath.Location = new System.Drawing.Point(26, 145);
            this.radioCustomizedFolderPath.Name = "radioCustomizedFolderPath";
            this.radioCustomizedFolderPath.Size = new System.Drawing.Size(184, 24);
            this.radioCustomizedFolderPath.TabIndex = 0;
            this.radioCustomizedFolderPath.TabStop = true;
            this.radioCustomizedFolderPath.Text = "自定义文件夹位置及名称";
            this.radioCustomizedFolderPath.UseVisualStyleBackColor = true;
            this.radioCustomizedFolderPath.CheckedChanged += new System.EventHandler(this.radioCustomizedFolderPath_CheckedChanged);
            // 
            // txtFolderName
            // 
            this.txtFolderName.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFolderName.Location = new System.Drawing.Point(168, 107);
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.Size = new System.Drawing.Size(211, 25);
            this.txtFolderName.TabIndex = 1;
            // 
            // btnBrowseFolder
            // 
            this.btnBrowseFolder.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseFolder.Location = new System.Drawing.Point(45, 175);
            this.btnBrowseFolder.Name = "btnBrowseFolder";
            this.btnBrowseFolder.Size = new System.Drawing.Size(75, 28);
            this.btnBrowseFolder.TabIndex = 2;
            this.btnBrowseFolder.Text = "浏览 ...";
            this.btnBrowseFolder.UseVisualStyleBackColor = true;
            this.btnBrowseFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click);
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFolderPath.Location = new System.Drawing.Point(45, 209);
            this.txtFolderPath.Multiline = true;
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(334, 64);
            this.txtFolderPath.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(116, 305);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(241, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelFolderName);
            this.groupBox1.Controls.Add(this.btnBrowseFolder);
            this.groupBox1.Controls.Add(this.txtFolderPath);
            this.groupBox1.Controls.Add(this.txtFolderName);
            this.groupBox1.Controls.Add(this.radioCustomizedFolderPath);
            this.groupBox1.Controls.Add(this.radioCustomizedFolderName);
            this.groupBox1.Controls.Add(this.radioDateFolderName);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 284);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择保存方式";
            // 
            // labelFolderName
            // 
            this.labelFolderName.AutoSize = true;
            this.labelFolderName.Location = new System.Drawing.Point(41, 107);
            this.labelFolderName.Name = "labelFolderName";
            this.labelFolderName.Size = new System.Drawing.Size(121, 20);
            this.labelFolderName.TabIndex = 3;
            this.labelFolderName.Text = "输入文件夹名称：";
            // 
            // FormProgramBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 346);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormProgramBackup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "程序备份";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioDateFolderName;
        private System.Windows.Forms.RadioButton radioCustomizedFolderName;
        private System.Windows.Forms.RadioButton radioCustomizedFolderPath;
        private System.Windows.Forms.TextBox txtFolderName;
        private System.Windows.Forms.Button btnBrowseFolder;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelFolderName;
    }
}