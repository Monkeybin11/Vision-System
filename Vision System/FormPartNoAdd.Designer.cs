namespace Vision_System
{
    partial class FormPartNoAdd
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
            this.btnOK = new System.Windows.Forms.Button();
            this.txtPartNoName = new System.Windows.Forms.TextBox();
            this.label_PartNoName = new System.Windows.Forms.Label();
            this.cmbPartNoType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCameraSelect = new System.Windows.Forms.Label();
            this.cmbCameraSelect = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(141, 162);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 31);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtPartNoName
            // 
            this.txtPartNoName.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPartNoName.Location = new System.Drawing.Point(141, 112);
            this.txtPartNoName.Name = "txtPartNoName";
            this.txtPartNoName.Size = new System.Drawing.Size(197, 25);
            this.txtPartNoName.TabIndex = 3;
            // 
            // label_PartNoName
            // 
            this.label_PartNoName.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PartNoName.Location = new System.Drawing.Point(33, 113);
            this.label_PartNoName.Name = "label_PartNoName";
            this.label_PartNoName.Size = new System.Drawing.Size(102, 22);
            this.label_PartNoName.TabIndex = 4;
            this.label_PartNoName.Text = "料号名称：";
            // 
            // cmbPartNoType
            // 
            this.cmbPartNoType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPartNoType.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPartNoType.FormattingEnabled = true;
            this.cmbPartNoType.Location = new System.Drawing.Point(141, 26);
            this.cmbPartNoType.Name = "cmbPartNoType";
            this.cmbPartNoType.Size = new System.Drawing.Size(197, 27);
            this.cmbPartNoType.TabIndex = 6;
            this.cmbPartNoType.SelectedIndexChanged += new System.EventHandler(this.cmbPartNoType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 22);
            this.label1.TabIndex = 4;
            this.label1.Text = "料号类型：";
            // 
            // labelCameraSelect
            // 
            this.labelCameraSelect.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCameraSelect.Location = new System.Drawing.Point(33, 71);
            this.labelCameraSelect.Name = "labelCameraSelect";
            this.labelCameraSelect.Size = new System.Drawing.Size(102, 22);
            this.labelCameraSelect.TabIndex = 4;
            this.labelCameraSelect.Text = "相机选择：";
            // 
            // cmbCameraSelect
            // 
            this.cmbCameraSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCameraSelect.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCameraSelect.FormattingEnabled = true;
            this.cmbCameraSelect.Location = new System.Drawing.Point(141, 70);
            this.cmbCameraSelect.Name = "cmbCameraSelect";
            this.cmbCameraSelect.Size = new System.Drawing.Size(197, 27);
            this.cmbCameraSelect.TabIndex = 6;
            // 
            // FormPartNoAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 205);
            this.Controls.Add(this.cmbCameraSelect);
            this.Controls.Add(this.cmbPartNoType);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labelCameraSelect);
            this.Controls.Add(this.txtPartNoName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_PartNoName);
            this.Name = "FormPartNoAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "料号";
            this.Load += new System.EventHandler(this.FormProductName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtPartNoName;
        private System.Windows.Forms.Label label_PartNoName;
        private System.Windows.Forms.ComboBox cmbPartNoType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCameraSelect;
        private System.Windows.Forms.ComboBox cmbCameraSelect;
    }
}