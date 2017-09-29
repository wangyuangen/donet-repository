namespace Img.ManualUpload
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnSearch = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnUpload = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pagerControl1 = new Img.ManualUpload.Control.PagerControl();
            this.cbBrandType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AllCheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PatientId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VideoNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BrandType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(516, 60);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(109, 34);
            this.BtnSearch.TabIndex = 0;
            this.BtnSearch.Text = "搜 索";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(134, 20);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(585, 20);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(159, 21);
            this.dateTimePicker2.TabIndex = 2;
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Location = new System.Drawing.Point(63, 26);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(65, 12);
            this.lblStartTime.TabIndex = 3;
            this.lblStartTime.Text = "开始时间：";
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Location = new System.Drawing.Point(514, 26);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(65, 12);
            this.lblEndTime.TabIndex = 4;
            this.lblEndTime.Text = "结束时间：";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AllCheckBox,
            this.FilePath,
            this.CheckTime,
            this.PatientId,
            this.VideoNum,
            this.PatientName,
            this.Status,
            this.Category,
            this.BrandType});
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(792, 457);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(635, 60);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(109, 34);
            this.btnUpload.TabIndex = 9;
            this.btnUpload.Text = "上 传";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.pagerControl1);
            this.panel1.Location = new System.Drawing.Point(10, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(801, 498);
            this.panel1.TabIndex = 10;
            // 
            // pagerControl1
            // 
            this.pagerControl1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pagerControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(78)))), ((int)(((byte)(151)))));
            this.pagerControl1.JumpText = "Go";
            this.pagerControl1.Location = new System.Drawing.Point(0, 466);
            this.pagerControl1.Name = "pagerControl1";
            this.pagerControl1.PageIndex = 1;
            this.pagerControl1.PageSize = 20;
            this.pagerControl1.RecordCount = 0;
            this.pagerControl1.Size = new System.Drawing.Size(795, 29);
            this.pagerControl1.TabIndex = 8;
            // 
            // cbBrandType
            // 
            this.cbBrandType.FormattingEnabled = true;
            this.cbBrandType.Location = new System.Drawing.Point(134, 68);
            this.cbBrandType.Name = "cbBrandType";
            this.cbBrandType.Size = new System.Drawing.Size(200, 20);
            this.cbBrandType.TabIndex = 11;
            this.cbBrandType.SelectedIndexChanged += new System.EventHandler(this.cbBrandType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "影像品牌：";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsUpload";
            this.dataGridViewCheckBoxColumn1.HeaderText = "全选";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.ToolTipText = "全选";
            this.dataGridViewCheckBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CheckTime";
            this.dataGridViewTextBoxColumn1.HeaderText = "检查时间";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 160;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "PatientId";
            this.dataGridViewTextBoxColumn2.HeaderText = "患者Id";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "VideoNum";
            this.dataGridViewTextBoxColumn3.HeaderText = "影像Id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "PatientName";
            this.dataGridViewTextBoxColumn4.HeaderText = "患者姓名";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 180;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Status";
            this.dataGridViewTextBoxColumn5.HeaderText = "上传状态";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 120;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "FilePath";
            this.dataGridViewTextBoxColumn6.HeaderText = "文件路径";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "影像品牌";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // AllCheckBox
            // 
            this.AllCheckBox.DataPropertyName = "IsUpload";
            this.AllCheckBox.HeaderText = "全选";
            this.AllCheckBox.Name = "AllCheckBox";
            this.AllCheckBox.ReadOnly = true;
            this.AllCheckBox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AllCheckBox.ToolTipText = "全选";
            this.AllCheckBox.Width = 40;
            // 
            // FilePath
            // 
            this.FilePath.DataPropertyName = "FilePath";
            this.FilePath.HeaderText = "FilePath";
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            this.FilePath.Visible = false;
            this.FilePath.Width = 5;
            // 
            // CheckTime
            // 
            this.CheckTime.DataPropertyName = "CheckTime";
            this.CheckTime.HeaderText = "检查时间";
            this.CheckTime.Name = "CheckTime";
            this.CheckTime.ReadOnly = true;
            this.CheckTime.Width = 180;
            // 
            // PatientId
            // 
            this.PatientId.DataPropertyName = "PatientId";
            this.PatientId.HeaderText = "患者Id";
            this.PatientId.Name = "PatientId";
            this.PatientId.ReadOnly = true;
            // 
            // VideoNum
            // 
            this.VideoNum.DataPropertyName = "VideoNum";
            this.VideoNum.HeaderText = "影像Id";
            this.VideoNum.Name = "VideoNum";
            this.VideoNum.ReadOnly = true;
            // 
            // PatientName
            // 
            this.PatientName.DataPropertyName = "PatientName";
            this.PatientName.HeaderText = "患者姓名";
            this.PatientName.Name = "PatientName";
            this.PatientName.ReadOnly = true;
            this.PatientName.Width = 180;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "上传状态";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 120;
            // 
            // Category
            // 
            this.Category.DataPropertyName = "Category";
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            this.Category.Visible = false;
            this.Category.Width = 5;
            // 
            // BrandType
            // 
            this.BrandType.DataPropertyName = "BrandType";
            this.BrandType.HeaderText = "BrandType";
            this.BrandType.Name = "BrandType";
            this.BrandType.ReadOnly = true;
            this.BrandType.Visible = false;
            this.BrandType.Width = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(819, 618);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbBrandType);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.lblEndTime);
            this.Controls.Add(this.lblStartTime);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.BtnSearch);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "手动上传";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Control.PagerControl pagerControl1;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.ComboBox cbBrandType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AllCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientId;
        private System.Windows.Forms.DataGridViewTextBoxColumn VideoNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn BrandType;
    }
}

