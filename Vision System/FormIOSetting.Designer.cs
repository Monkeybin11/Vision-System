namespace Vision_System
{
    partial class FormIOSetting
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvIOInfoList = new System.Windows.Forms.DataGridView();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddTrigger = new System.Windows.Forms.Button();
            this.btnDeleteTrigger = new System.Windows.Forms.Button();
            this.btnAddLight = new System.Windows.Forms.Button();
            this.btnDeleteLight = new System.Windows.Forms.Button();
            this.btnAddOK = new System.Windows.Forms.Button();
            this.btnDeleteOK = new System.Windows.Forms.Button();
            this.btnAddNG = new System.Windows.Forms.Button();
            this.btnDeleteNG = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIOInfoList)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 20);
            this.label5.TabIndex = 60;
            this.label5.Text = "IO 列表信息：";
            // 
            // dgvIOInfoList
            // 
            this.dgvIOInfoList.AllowUserToAddRows = false;
            this.dgvIOInfoList.AllowUserToDeleteRows = false;
            this.dgvIOInfoList.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvIOInfoList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvIOInfoList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvIOInfoList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIOInfoList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dgvIOInfoList.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvIOInfoList.Location = new System.Drawing.Point(12, 36);
            this.dgvIOInfoList.Name = "dgvIOInfoList";
            this.dgvIOInfoList.RowHeadersVisible = false;
            this.dgvIOInfoList.RowTemplate.Height = 24;
            this.dgvIOInfoList.Size = new System.Drawing.Size(440, 563);
            this.dgvIOInfoList.TabIndex = 51;
            // 
            // Column2
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column2.HeaderText = "输入/输出";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Port";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Bit";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "端口名称";
            this.Column5.Name = "Column5";
            // 
            // btnAddTrigger
            // 
            this.btnAddTrigger.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTrigger.Location = new System.Drawing.Point(12, 621);
            this.btnAddTrigger.Name = "btnAddTrigger";
            this.btnAddTrigger.Size = new System.Drawing.Size(96, 29);
            this.btnAddTrigger.TabIndex = 61;
            this.btnAddTrigger.Text = "增加触发";
            this.btnAddTrigger.UseVisualStyleBackColor = true;
            this.btnAddTrigger.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // btnDeleteTrigger
            // 
            this.btnDeleteTrigger.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteTrigger.Location = new System.Drawing.Point(12, 656);
            this.btnDeleteTrigger.Name = "btnDeleteTrigger";
            this.btnDeleteTrigger.Size = new System.Drawing.Size(96, 29);
            this.btnDeleteTrigger.TabIndex = 61;
            this.btnDeleteTrigger.Text = "删除触发";
            this.btnDeleteTrigger.UseVisualStyleBackColor = true;
            this.btnDeleteTrigger.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // btnAddLight
            // 
            this.btnAddLight.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddLight.Location = new System.Drawing.Point(126, 621);
            this.btnAddLight.Name = "btnAddLight";
            this.btnAddLight.Size = new System.Drawing.Size(96, 29);
            this.btnAddLight.TabIndex = 61;
            this.btnAddLight.Text = "增加光源";
            this.btnAddLight.UseVisualStyleBackColor = true;
            this.btnAddLight.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // btnDeleteLight
            // 
            this.btnDeleteLight.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteLight.Location = new System.Drawing.Point(126, 656);
            this.btnDeleteLight.Name = "btnDeleteLight";
            this.btnDeleteLight.Size = new System.Drawing.Size(96, 29);
            this.btnDeleteLight.TabIndex = 61;
            this.btnDeleteLight.Text = "删除光源";
            this.btnDeleteLight.UseVisualStyleBackColor = true;
            this.btnDeleteLight.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // btnAddOK
            // 
            this.btnAddOK.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddOK.Location = new System.Drawing.Point(240, 621);
            this.btnAddOK.Name = "btnAddOK";
            this.btnAddOK.Size = new System.Drawing.Size(96, 29);
            this.btnAddOK.TabIndex = 61;
            this.btnAddOK.Text = "增加OK";
            this.btnAddOK.UseVisualStyleBackColor = true;
            this.btnAddOK.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // btnDeleteOK
            // 
            this.btnDeleteOK.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteOK.Location = new System.Drawing.Point(240, 656);
            this.btnDeleteOK.Name = "btnDeleteOK";
            this.btnDeleteOK.Size = new System.Drawing.Size(96, 29);
            this.btnDeleteOK.TabIndex = 61;
            this.btnDeleteOK.Text = "删除OK";
            this.btnDeleteOK.UseVisualStyleBackColor = true;
            this.btnDeleteOK.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // btnAddNG
            // 
            this.btnAddNG.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNG.Location = new System.Drawing.Point(354, 621);
            this.btnAddNG.Name = "btnAddNG";
            this.btnAddNG.Size = new System.Drawing.Size(96, 29);
            this.btnAddNG.TabIndex = 61;
            this.btnAddNG.Text = "增加NG";
            this.btnAddNG.UseVisualStyleBackColor = true;
            this.btnAddNG.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // btnDeleteNG
            // 
            this.btnDeleteNG.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteNG.Location = new System.Drawing.Point(354, 656);
            this.btnDeleteNG.Name = "btnDeleteNG";
            this.btnDeleteNG.Size = new System.Drawing.Size(96, 29);
            this.btnDeleteNG.TabIndex = 61;
            this.btnDeleteNG.Text = "删除NG";
            this.btnDeleteNG.UseVisualStyleBackColor = true;
            this.btnDeleteNG.Click += new System.EventHandler(this.btnMultiple_Click);
            // 
            // FormIOSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 697);
            this.Controls.Add(this.btnDeleteNG);
            this.Controls.Add(this.btnDeleteOK);
            this.Controls.Add(this.btnDeleteLight);
            this.Controls.Add(this.btnAddNG);
            this.Controls.Add(this.btnAddOK);
            this.Controls.Add(this.btnAddLight);
            this.Controls.Add(this.btnDeleteTrigger);
            this.Controls.Add(this.btnAddTrigger);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgvIOInfoList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormIOSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IO 信号设置和测试";
            this.Load += new System.EventHandler(this.FormIOSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIOInfoList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvIOInfoList;
        private System.Windows.Forms.Button btnAddTrigger;
        private System.Windows.Forms.Button btnDeleteTrigger;
        private System.Windows.Forms.Button btnAddLight;
        private System.Windows.Forms.Button btnDeleteLight;
        private System.Windows.Forms.Button btnAddOK;
        private System.Windows.Forms.Button btnDeleteOK;
        private System.Windows.Forms.Button btnAddNG;
        private System.Windows.Forms.Button btnDeleteNG;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}