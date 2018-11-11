namespace Vision_System
{
    partial class PagePlayBack
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PagePlayBack));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cogDisplayStatusBarV21 = new Cognex.VisionPro.CogDisplayStatusBarV2();
            this.cogDisplay_PlayBack = new Cognex.VisionPro.CogRecordDisplay();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbCameraSelect = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_SelectImagePath = new System.Windows.Forms.Button();
            this.txtSelectImagePath = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelImageName = new System.Windows.Forms.Label();
            this.labelPicType = new System.Windows.Forms.Label();
            this.labelPicCapacity = new System.Windows.Forms.Label();
            this.labelPicSize = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelPicTakenTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelCCDName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBoxRunResult = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_SelectPreviousImage = new System.Windows.Forms.Button();
            this.btn_SelectNextImage = new System.Windows.Forms.Button();
            this.btn_SelectLastImage = new System.Windows.Forms.Button();
            this.btn_AutoPlay = new System.Windows.Forms.Button();
            this.btn_SelectFirstImage = new System.Windows.Forms.Button();
            this.OpenImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.timer_AutoPlay = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_PlayBack)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRunResult)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 600F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cogDisplayStatusBarV21, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cogDisplay_PlayBack, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1200, 672);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // cogDisplayStatusBarV21
            // 
            this.cogDisplayStatusBarV21.CoordinateSpaceName = "*\\#";
            this.cogDisplayStatusBarV21.CoordinateSpaceName3D = "*\\#";
            this.cogDisplayStatusBarV21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogDisplayStatusBarV21.Location = new System.Drawing.Point(603, 645);
            this.cogDisplayStatusBarV21.Name = "cogDisplayStatusBarV21";
            this.cogDisplayStatusBarV21.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cogDisplayStatusBarV21.Size = new System.Drawing.Size(594, 24);
            this.cogDisplayStatusBarV21.TabIndex = 374;
            this.cogDisplayStatusBarV21.Use3DCoordinateSpaceTree = false;
            // 
            // cogDisplay_PlayBack
            // 
            this.cogDisplay_PlayBack.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_PlayBack.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_PlayBack.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_PlayBack.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_PlayBack.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_PlayBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogDisplay_PlayBack.DoubleTapZoomCycleLength = 2;
            this.cogDisplay_PlayBack.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay_PlayBack.Location = new System.Drawing.Point(603, 3);
            this.cogDisplay_PlayBack.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_PlayBack.MouseWheelSensitivity = 1D;
            this.cogDisplay_PlayBack.Name = "cogDisplay_PlayBack";
            this.cogDisplay_PlayBack.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_PlayBack.OcxState")));
            this.cogDisplay_PlayBack.Size = new System.Drawing.Size(594, 636);
            this.cogDisplay_PlayBack.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.btn_SelectPreviousImage);
            this.panel1.Controls.Add(this.btn_SelectNextImage);
            this.panel1.Controls.Add(this.btn_SelectLastImage);
            this.panel1.Controls.Add(this.btn_AutoPlay);
            this.panel1.Controls.Add(this.btn_SelectFirstImage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(594, 636);
            this.panel1.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbCameraSelect);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(18, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(537, 64);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择相机";
            // 
            // cmbCameraSelect
            // 
            this.cmbCameraSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCameraSelect.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbCameraSelect.FormattingEnabled = true;
            this.cmbCameraSelect.Location = new System.Drawing.Point(35, 24);
            this.cmbCameraSelect.Name = "cmbCameraSelect";
            this.cmbCameraSelect.Size = new System.Drawing.Size(484, 27);
            this.cmbCameraSelect.TabIndex = 372;
            this.cmbCameraSelect.SelectedIndexChanged += new System.EventHandler(this.cmbCameraSelect_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_SelectImagePath);
            this.groupBox1.Controls.Add(this.txtSelectImagePath);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(18, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(537, 103);
            this.groupBox1.TabIndex = 372;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择图片来源";
            // 
            // btn_SelectImagePath
            // 
            this.btn_SelectImagePath.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_SelectImagePath.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_SelectImagePath.Location = new System.Drawing.Point(438, 25);
            this.btn_SelectImagePath.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SelectImagePath.Name = "btn_SelectImagePath";
            this.btn_SelectImagePath.Size = new System.Drawing.Size(81, 57);
            this.btn_SelectImagePath.TabIndex = 371;
            this.btn_SelectImagePath.Tag = "Move";
            this.btn_SelectImagePath.Text = "浏览 ...";
            this.btn_SelectImagePath.Click += new System.EventHandler(this.btn_ButtonClickEvent_Click);
            // 
            // txtSelectImagePath
            // 
            this.txtSelectImagePath.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSelectImagePath.Location = new System.Drawing.Point(35, 25);
            this.txtSelectImagePath.Margin = new System.Windows.Forms.Padding(4);
            this.txtSelectImagePath.Multiline = true;
            this.txtSelectImagePath.Name = "txtSelectImagePath";
            this.txtSelectImagePath.ReadOnly = true;
            this.txtSelectImagePath.Size = new System.Drawing.Size(395, 59);
            this.txtSelectImagePath.TabIndex = 370;
            this.txtSelectImagePath.TextChanged += new System.EventHandler(this.MsgBox_SelectImagePath_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelImageName);
            this.groupBox3.Controls.Add(this.labelPicType);
            this.groupBox3.Controls.Add(this.labelPicCapacity);
            this.groupBox3.Controls.Add(this.labelPicSize);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.labelPicTakenTime);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.labelCCDName);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.pictureBoxRunResult);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(18, 312);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(537, 321);
            this.groupBox3.TabIndex = 375;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "图片信息及结果";
            // 
            // labelImageName
            // 
            this.labelImageName.AutoSize = true;
            this.labelImageName.Location = new System.Drawing.Point(144, 229);
            this.labelImageName.Name = "labelImageName";
            this.labelImageName.Size = new System.Drawing.Size(18, 20);
            this.labelImageName.TabIndex = 3;
            this.labelImageName.Text = "...";
            // 
            // labelPicType
            // 
            this.labelPicType.AutoSize = true;
            this.labelPicType.Location = new System.Drawing.Point(144, 196);
            this.labelPicType.Name = "labelPicType";
            this.labelPicType.Size = new System.Drawing.Size(18, 20);
            this.labelPicType.TabIndex = 3;
            this.labelPicType.Text = "...";
            // 
            // labelPicCapacity
            // 
            this.labelPicCapacity.AutoSize = true;
            this.labelPicCapacity.Location = new System.Drawing.Point(144, 162);
            this.labelPicCapacity.Name = "labelPicCapacity";
            this.labelPicCapacity.Size = new System.Drawing.Size(45, 20);
            this.labelPicCapacity.TabIndex = 3;
            this.labelPicCapacity.Text = "... MB";
            // 
            // labelPicSize
            // 
            this.labelPicSize.AutoSize = true;
            this.labelPicSize.Location = new System.Drawing.Point(144, 127);
            this.labelPicSize.Name = "labelPicSize";
            this.labelPicSize.Size = new System.Drawing.Size(54, 20);
            this.labelPicSize.TabIndex = 3;
            this.labelPicSize.Text = "... pixel";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "图片名称：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(31, 196);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 20);
            this.label10.TabIndex = 1;
            this.label10.Text = "图片类型：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 20);
            this.label8.TabIndex = 1;
            this.label8.Text = "图片容量：";
            // 
            // labelPicTakenTime
            // 
            this.labelPicTakenTime.AutoSize = true;
            this.labelPicTakenTime.Location = new System.Drawing.Point(144, 90);
            this.labelPicTakenTime.Name = "labelPicTakenTime";
            this.labelPicTakenTime.Size = new System.Drawing.Size(18, 20);
            this.labelPicTakenTime.TabIndex = 3;
            this.labelPicTakenTime.Text = "...";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 1;
            this.label6.Text = "图片大小：";
            // 
            // labelCCDName
            // 
            this.labelCCDName.AutoSize = true;
            this.labelCCDName.Location = new System.Drawing.Point(144, 56);
            this.labelCCDName.Name = "labelCCDName";
            this.labelCCDName.Size = new System.Drawing.Size(18, 20);
            this.labelCCDName.TabIndex = 3;
            this.labelCCDName.Text = "...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "创建日期：";
            // 
            // pictureBoxRunResult
            // 
            this.pictureBoxRunResult.Image = global::Vision_System.Properties.Resources.green_light;
            this.pictureBoxRunResult.Location = new System.Drawing.Point(286, 69);
            this.pictureBoxRunResult.Name = "pictureBoxRunResult";
            this.pictureBoxRunResult.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxRunResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxRunResult.TabIndex = 2;
            this.pictureBoxRunResult.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "拍摄相机：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(347, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "运行结果：";
            // 
            // btn_SelectPreviousImage
            // 
            this.btn_SelectPreviousImage.BackgroundImage = global::Vision_System.Properties.Resources.navigate_left;
            this.btn_SelectPreviousImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_SelectPreviousImage.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_SelectPreviousImage.Location = new System.Drawing.Point(131, 225);
            this.btn_SelectPreviousImage.Name = "btn_SelectPreviousImage";
            this.btn_SelectPreviousImage.Size = new System.Drawing.Size(85, 67);
            this.btn_SelectPreviousImage.TabIndex = 2;
            this.btn_SelectPreviousImage.Text = "上一张";
            this.btn_SelectPreviousImage.UseVisualStyleBackColor = true;
            this.btn_SelectPreviousImage.Click += new System.EventHandler(this.btn_ButtonClickEvent_Click);
            // 
            // btn_SelectNextImage
            // 
            this.btn_SelectNextImage.BackgroundImage = global::Vision_System.Properties.Resources.navigate_right;
            this.btn_SelectNextImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_SelectNextImage.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_SelectNextImage.Location = new System.Drawing.Point(357, 225);
            this.btn_SelectNextImage.Name = "btn_SelectNextImage";
            this.btn_SelectNextImage.Size = new System.Drawing.Size(85, 67);
            this.btn_SelectNextImage.TabIndex = 2;
            this.btn_SelectNextImage.Text = "下一张";
            this.btn_SelectNextImage.UseVisualStyleBackColor = true;
            this.btn_SelectNextImage.Click += new System.EventHandler(this.btn_ButtonClickEvent_Click);
            // 
            // btn_SelectLastImage
            // 
            this.btn_SelectLastImage.BackgroundImage = global::Vision_System.Properties.Resources.hide_right;
            this.btn_SelectLastImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_SelectLastImage.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_SelectLastImage.Location = new System.Drawing.Point(470, 225);
            this.btn_SelectLastImage.Name = "btn_SelectLastImage";
            this.btn_SelectLastImage.Size = new System.Drawing.Size(85, 67);
            this.btn_SelectLastImage.TabIndex = 2;
            this.btn_SelectLastImage.Text = "最后一张";
            this.btn_SelectLastImage.UseVisualStyleBackColor = true;
            this.btn_SelectLastImage.Click += new System.EventHandler(this.btn_ButtonClickEvent_Click);
            // 
            // btn_AutoPlay
            // 
            this.btn_AutoPlay.BackgroundImage = global::Vision_System.Properties.Resources._continue;
            this.btn_AutoPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_AutoPlay.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AutoPlay.Location = new System.Drawing.Point(244, 225);
            this.btn_AutoPlay.Name = "btn_AutoPlay";
            this.btn_AutoPlay.Size = new System.Drawing.Size(85, 67);
            this.btn_AutoPlay.TabIndex = 2;
            this.btn_AutoPlay.Text = "播放";
            this.btn_AutoPlay.UseVisualStyleBackColor = true;
            this.btn_AutoPlay.Click += new System.EventHandler(this.btn_ButtonClickEvent_Click);
            // 
            // btn_SelectFirstImage
            // 
            this.btn_SelectFirstImage.BackgroundImage = global::Vision_System.Properties.Resources.hide_left;
            this.btn_SelectFirstImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_SelectFirstImage.Font = new System.Drawing.Font("Microsoft YaHei", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_SelectFirstImage.Location = new System.Drawing.Point(18, 225);
            this.btn_SelectFirstImage.Name = "btn_SelectFirstImage";
            this.btn_SelectFirstImage.Size = new System.Drawing.Size(85, 67);
            this.btn_SelectFirstImage.TabIndex = 2;
            this.btn_SelectFirstImage.Text = "最前一张";
            this.btn_SelectFirstImage.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btn_SelectFirstImage.UseVisualStyleBackColor = true;
            this.btn_SelectFirstImage.Click += new System.EventHandler(this.btn_ButtonClickEvent_Click);
            // 
            // OpenImageDialog
            // 
            this.OpenImageDialog.Filter = "BMP File | *.bmp|JPG File |*.JPG";
            this.OpenImageDialog.RestoreDirectory = true;
            // 
            // timer_AutoPlay
            // 
            this.timer_AutoPlay.Interval = 1000;
            this.timer_AutoPlay.Tick += new System.EventHandler(this.timer_AutoPlay_Tick);
            // 
            // PagePlayBack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PagePlayBack";
            this.Size = new System.Drawing.Size(1200, 672);
            this.Load += new System.EventHandler(this.PagePlayBack_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_PlayBack)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRunResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_SelectPreviousImage;
        private System.Windows.Forms.Button btn_SelectLastImage;
        private System.Windows.Forms.Button btn_SelectNextImage;
        private System.Windows.Forms.Button btn_SelectFirstImage;
        private System.Windows.Forms.Button btn_AutoPlay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btn_SelectImagePath;
        internal System.Windows.Forms.TextBox txtSelectImagePath;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.OpenFileDialog OpenImageDialog;
        private System.Windows.Forms.Timer timer_AutoPlay;
        private Cognex.VisionPro.CogRecordDisplay cogDisplay_PlayBack;
        private System.Windows.Forms.ComboBox cmbCameraSelect;
        private Cognex.VisionPro.CogDisplayStatusBarV2 cogDisplayStatusBarV21;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelCCDName;
        private System.Windows.Forms.PictureBox pictureBoxRunResult;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelPicCapacity;
        private System.Windows.Forms.Label labelPicSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelPicTakenTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelPicType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelImageName;
        private System.Windows.Forms.Label label1;
    }
}
