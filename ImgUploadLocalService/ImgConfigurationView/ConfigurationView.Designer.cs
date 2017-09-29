namespace ImgConfigurationView
{
	partial class ConfigurationView
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
			this.btn_Confirm = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.grp_autoConfig = new System.Windows.Forms.GroupBox();
			this.rbPointNix = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.rbEasyDent = new System.Windows.Forms.RadioButton();
			this.rbSidexis = new System.Windows.Forms.RadioButton();
			this.chk_SEtime = new System.Windows.Forms.CheckBox();
			this.dtEtime = new System.Windows.Forms.DateTimePicker();
			this.dtStime = new System.Windows.Forms.DateTimePicker();
			this.grp_uploadType = new System.Windows.Forms.GroupBox();
			this.chk_Manually = new System.Windows.Forms.CheckBox();
			this.chk_Auto = new System.Windows.Forms.CheckBox();
			this.lab_Domain = new System.Windows.Forms.Label();
			this.lab_Officekeyword = new System.Windows.Forms.Label();
			this.lab_LogAccount = new System.Windows.Forms.Label();
			this.lab_LogPwd = new System.Windows.Forms.Label();
			this.lab_RootDirectory = new System.Windows.Forms.Label();
			this.txt_domain = new System.Windows.Forms.TextBox();
			this.txt_officekeyword = new System.Windows.Forms.TextBox();
			this.txt_logAccount = new System.Windows.Forms.TextBox();
			this.txt_logPwd = new System.Windows.Forms.TextBox();
			this.txt_rootDirectory = new System.Windows.Forms.TextBox();
			this.lab_ImgbrandType = new System.Windows.Forms.Label();
			this.lab_DbAccount = new System.Windows.Forms.Label();
			this.lab_DbPwd = new System.Windows.Forms.Label();
			this.txt_dbAccount = new System.Windows.Forms.TextBox();
			this.txt_dbPwd = new System.Windows.Forms.TextBox();
			this.grp_Common = new System.Windows.Forms.GroupBox();
			this.dtp_runTime = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.cmb_imgBrandType = new System.Windows.Forms.ComboBox();
			this.gbConfig = new System.Windows.Forms.GroupBox();
			this.cbImageCategory = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.gbManually = new System.Windows.Forms.GroupBox();
			this.chk_PointNix = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.chk_EasyDent = new System.Windows.Forms.CheckBox();
			this.chk_Sidexis = new System.Windows.Forms.CheckBox();
			this.rbDbSwin = new System.Windows.Forms.RadioButton();
			this.ckDbSwin = new System.Windows.Forms.CheckBox();
			this.grp_autoConfig.SuspendLayout();
			this.grp_uploadType.SuspendLayout();
			this.grp_Common.SuspendLayout();
			this.gbConfig.SuspendLayout();
			this.gbManually.SuspendLayout();
			this.SuspendLayout();
			// 
			// btn_Confirm
			// 
			this.btn_Confirm.BackColor = System.Drawing.SystemColors.MenuHighlight;
			this.btn_Confirm.Location = new System.Drawing.Point(532, 325);
			this.btn_Confirm.Name = "btn_Confirm";
			this.btn_Confirm.Size = new System.Drawing.Size(75, 23);
			this.btn_Confirm.TabIndex = 2;
			this.btn_Confirm.Text = "确认";
			this.btn_Confirm.UseVisualStyleBackColor = false;
			this.btn_Confirm.Click += new System.EventHandler(this.btn_Confirm_Click);
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Location = new System.Drawing.Point(635, 325);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
			this.btn_Cancel.TabIndex = 3;
			this.btn_Cancel.Text = "关闭";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
			// 
			// grp_autoConfig
			// 
			this.grp_autoConfig.Controls.Add(this.rbDbSwin);
			this.grp_autoConfig.Controls.Add(this.rbPointNix);
			this.grp_autoConfig.Controls.Add(this.label3);
			this.grp_autoConfig.Controls.Add(this.rbEasyDent);
			this.grp_autoConfig.Controls.Add(this.rbSidexis);
			this.grp_autoConfig.Controls.Add(this.chk_SEtime);
			this.grp_autoConfig.Controls.Add(this.dtEtime);
			this.grp_autoConfig.Controls.Add(this.dtStime);
			this.grp_autoConfig.Location = new System.Drawing.Point(364, 78);
			this.grp_autoConfig.Name = "grp_autoConfig";
			this.grp_autoConfig.Size = new System.Drawing.Size(346, 110);
			this.grp_autoConfig.TabIndex = 5;
			this.grp_autoConfig.TabStop = false;
			this.grp_autoConfig.Text = "自动上传";
			// 
			// rbPointNix
			// 
			this.rbPointNix.AutoSize = true;
			this.rbPointNix.Location = new System.Drawing.Point(215, 29);
			this.rbPointNix.Name = "rbPointNix";
			this.rbPointNix.Size = new System.Drawing.Size(71, 16);
			this.rbPointNix.TabIndex = 37;
			this.rbPointNix.TabStop = true;
			this.rbPointNix.Text = "PointNix";
			this.rbPointNix.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(19, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 12);
			this.label3.TabIndex = 36;
			this.label3.Text = "影像类型：";
			// 
			// rbEasyDent
			// 
			this.rbEasyDent.AutoSize = true;
			this.rbEasyDent.Location = new System.Drawing.Point(146, 29);
			this.rbEasyDent.Name = "rbEasyDent";
			this.rbEasyDent.Size = new System.Drawing.Size(71, 16);
			this.rbEasyDent.TabIndex = 35;
			this.rbEasyDent.TabStop = true;
			this.rbEasyDent.Text = "EasyDent";
			this.rbEasyDent.UseVisualStyleBackColor = true;
			// 
			// rbSidexis
			// 
			this.rbSidexis.AutoSize = true;
			this.rbSidexis.Location = new System.Drawing.Point(84, 29);
			this.rbSidexis.Name = "rbSidexis";
			this.rbSidexis.Size = new System.Drawing.Size(65, 16);
			this.rbSidexis.TabIndex = 34;
			this.rbSidexis.TabStop = true;
			this.rbSidexis.Text = "Sidexis";
			this.rbSidexis.UseVisualStyleBackColor = true;
			// 
			// chk_SEtime
			// 
			this.chk_SEtime.AutoSize = true;
			this.chk_SEtime.Location = new System.Drawing.Point(10, 70);
			this.chk_SEtime.Name = "chk_SEtime";
			this.chk_SEtime.Size = new System.Drawing.Size(84, 16);
			this.chk_SEtime.TabIndex = 33;
			this.chk_SEtime.Text = "起止时间：";
			this.chk_SEtime.UseVisualStyleBackColor = true;
			this.chk_SEtime.CheckedChanged += new System.EventHandler(this.chk_SEtime_CheckedChanged);
			// 
			// dtEtime
			// 
			this.dtEtime.Enabled = false;
			this.dtEtime.Location = new System.Drawing.Point(223, 67);
			this.dtEtime.Name = "dtEtime";
			this.dtEtime.Size = new System.Drawing.Size(117, 21);
			this.dtEtime.TabIndex = 31;
			// 
			// dtStime
			// 
			this.dtStime.Enabled = false;
			this.dtStime.Location = new System.Drawing.Point(100, 67);
			this.dtStime.Name = "dtStime";
			this.dtStime.Size = new System.Drawing.Size(117, 21);
			this.dtStime.TabIndex = 30;
			// 
			// grp_uploadType
			// 
			this.grp_uploadType.Controls.Add(this.chk_Manually);
			this.grp_uploadType.Controls.Add(this.chk_Auto);
			this.grp_uploadType.Location = new System.Drawing.Point(364, 12);
			this.grp_uploadType.Name = "grp_uploadType";
			this.grp_uploadType.Size = new System.Drawing.Size(346, 50);
			this.grp_uploadType.TabIndex = 7;
			this.grp_uploadType.TabStop = false;
			this.grp_uploadType.Text = "上传方式";
			// 
			// chk_Manually
			// 
			this.chk_Manually.AutoSize = true;
			this.chk_Manually.Location = new System.Drawing.Point(206, 21);
			this.chk_Manually.Name = "chk_Manually";
			this.chk_Manually.Size = new System.Drawing.Size(72, 16);
			this.chk_Manually.TabIndex = 3;
			this.chk_Manually.Text = "手动上传";
			this.chk_Manually.UseVisualStyleBackColor = true;
			this.chk_Manually.CheckedChanged += new System.EventHandler(this.chk_Manually_CheckedChanged);
			// 
			// chk_Auto
			// 
			this.chk_Auto.AutoSize = true;
			this.chk_Auto.Location = new System.Drawing.Point(81, 21);
			this.chk_Auto.Name = "chk_Auto";
			this.chk_Auto.Size = new System.Drawing.Size(72, 16);
			this.chk_Auto.TabIndex = 2;
			this.chk_Auto.Text = "自动上传";
			this.chk_Auto.UseVisualStyleBackColor = true;
			this.chk_Auto.CheckedChanged += new System.EventHandler(this.chk_Auto_CheckedChanged);
			// 
			// lab_Domain
			// 
			this.lab_Domain.AutoSize = true;
			this.lab_Domain.Location = new System.Drawing.Point(75, 20);
			this.lab_Domain.Name = "lab_Domain";
			this.lab_Domain.Size = new System.Drawing.Size(65, 12);
			this.lab_Domain.TabIndex = 39;
			this.lab_Domain.Text = "SaaS代码：";
			// 
			// lab_Officekeyword
			// 
			this.lab_Officekeyword.AutoSize = true;
			this.lab_Officekeyword.Location = new System.Drawing.Point(63, 50);
			this.lab_Officekeyword.Name = "lab_Officekeyword";
			this.lab_Officekeyword.Size = new System.Drawing.Size(77, 12);
			this.lab_Officekeyword.TabIndex = 40;
			this.lab_Officekeyword.Text = "诊所关键字：";
			// 
			// lab_LogAccount
			// 
			this.lab_LogAccount.AutoSize = true;
			this.lab_LogAccount.Location = new System.Drawing.Point(51, 80);
			this.lab_LogAccount.Name = "lab_LogAccount";
			this.lab_LogAccount.Size = new System.Drawing.Size(89, 12);
			this.lab_LogAccount.TabIndex = 41;
			this.lab_LogAccount.Text = "SaaS登录账户：";
			// 
			// lab_LogPwd
			// 
			this.lab_LogPwd.AutoSize = true;
			this.lab_LogPwd.Location = new System.Drawing.Point(50, 110);
			this.lab_LogPwd.Name = "lab_LogPwd";
			this.lab_LogPwd.Size = new System.Drawing.Size(89, 12);
			this.lab_LogPwd.TabIndex = 42;
			this.lab_LogPwd.Text = "SaaS登录密码：";
			// 
			// lab_RootDirectory
			// 
			this.lab_RootDirectory.AutoSize = true;
			this.lab_RootDirectory.Location = new System.Drawing.Point(63, 58);
			this.lab_RootDirectory.Name = "lab_RootDirectory";
			this.lab_RootDirectory.Size = new System.Drawing.Size(77, 12);
			this.lab_RootDirectory.TabIndex = 43;
			this.lab_RootDirectory.Text = "影像根目录：";
			// 
			// txt_domain
			// 
			this.txt_domain.Location = new System.Drawing.Point(149, 15);
			this.txt_domain.Name = "txt_domain";
			this.txt_domain.Size = new System.Drawing.Size(136, 21);
			this.txt_domain.TabIndex = 44;
			// 
			// txt_officekeyword
			// 
			this.txt_officekeyword.Location = new System.Drawing.Point(149, 46);
			this.txt_officekeyword.Name = "txt_officekeyword";
			this.txt_officekeyword.Size = new System.Drawing.Size(136, 21);
			this.txt_officekeyword.TabIndex = 45;
			// 
			// txt_logAccount
			// 
			this.txt_logAccount.Location = new System.Drawing.Point(149, 77);
			this.txt_logAccount.Name = "txt_logAccount";
			this.txt_logAccount.Size = new System.Drawing.Size(136, 21);
			this.txt_logAccount.TabIndex = 46;
			// 
			// txt_logPwd
			// 
			this.txt_logPwd.Location = new System.Drawing.Point(149, 107);
			this.txt_logPwd.Name = "txt_logPwd";
			this.txt_logPwd.Size = new System.Drawing.Size(136, 21);
			this.txt_logPwd.TabIndex = 47;
			// 
			// txt_rootDirectory
			// 
			this.txt_rootDirectory.Location = new System.Drawing.Point(149, 54);
			this.txt_rootDirectory.Name = "txt_rootDirectory";
			this.txt_rootDirectory.Size = new System.Drawing.Size(136, 21);
			this.txt_rootDirectory.TabIndex = 48;
			// 
			// lab_ImgbrandType
			// 
			this.lab_ImgbrandType.AutoSize = true;
			this.lab_ImgbrandType.Location = new System.Drawing.Point(74, 23);
			this.lab_ImgbrandType.Name = "lab_ImgbrandType";
			this.lab_ImgbrandType.Size = new System.Drawing.Size(65, 12);
			this.lab_ImgbrandType.TabIndex = 49;
			this.lab_ImgbrandType.Text = "影像品牌：";
			// 
			// lab_DbAccount
			// 
			this.lab_DbAccount.AutoSize = true;
			this.lab_DbAccount.Location = new System.Drawing.Point(38, 121);
			this.lab_DbAccount.Name = "lab_DbAccount";
			this.lab_DbAccount.Size = new System.Drawing.Size(101, 12);
			this.lab_DbAccount.TabIndex = 52;
			this.lab_DbAccount.Text = "数据库登录账号：";
			// 
			// lab_DbPwd
			// 
			this.lab_DbPwd.AutoSize = true;
			this.lab_DbPwd.Location = new System.Drawing.Point(38, 150);
			this.lab_DbPwd.Name = "lab_DbPwd";
			this.lab_DbPwd.Size = new System.Drawing.Size(101, 12);
			this.lab_DbPwd.TabIndex = 53;
			this.lab_DbPwd.Text = "数据库登录密码：";
			// 
			// txt_dbAccount
			// 
			this.txt_dbAccount.Location = new System.Drawing.Point(149, 116);
			this.txt_dbAccount.Name = "txt_dbAccount";
			this.txt_dbAccount.Size = new System.Drawing.Size(136, 21);
			this.txt_dbAccount.TabIndex = 57;
			// 
			// txt_dbPwd
			// 
			this.txt_dbPwd.Location = new System.Drawing.Point(149, 146);
			this.txt_dbPwd.Name = "txt_dbPwd";
			this.txt_dbPwd.Size = new System.Drawing.Size(136, 21);
			this.txt_dbPwd.TabIndex = 58;
			// 
			// grp_Common
			// 
			this.grp_Common.Controls.Add(this.dtp_runTime);
			this.grp_Common.Controls.Add(this.label1);
			this.grp_Common.Controls.Add(this.txt_logPwd);
			this.grp_Common.Controls.Add(this.txt_logAccount);
			this.grp_Common.Controls.Add(this.txt_officekeyword);
			this.grp_Common.Controls.Add(this.txt_domain);
			this.grp_Common.Controls.Add(this.lab_LogPwd);
			this.grp_Common.Controls.Add(this.lab_LogAccount);
			this.grp_Common.Controls.Add(this.lab_Officekeyword);
			this.grp_Common.Controls.Add(this.lab_Domain);
			this.grp_Common.Location = new System.Drawing.Point(12, 12);
			this.grp_Common.Name = "grp_Common";
			this.grp_Common.Size = new System.Drawing.Size(336, 163);
			this.grp_Common.TabIndex = 6;
			this.grp_Common.TabStop = false;
			this.grp_Common.Text = "公共配置";
			// 
			// dtp_runTime
			// 
			this.dtp_runTime.CustomFormat = "HH:mm";
			this.dtp_runTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtp_runTime.Location = new System.Drawing.Point(149, 133);
			this.dtp_runTime.Name = "dtp_runTime";
			this.dtp_runTime.Size = new System.Drawing.Size(136, 21);
			this.dtp_runTime.TabIndex = 49;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(74, 139);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 12);
			this.label1.TabIndex = 29;
			this.label1.Text = "运行时间：";
			// 
			// cmb_imgBrandType
			// 
			this.cmb_imgBrandType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmb_imgBrandType.FormattingEnabled = true;
			this.cmb_imgBrandType.Items.AddRange(new object[] {
            "==请选择==",
            "Sidexis",
            "EasyDent",
            "PointNix",
            "DBSwin"});
			this.cmb_imgBrandType.Location = new System.Drawing.Point(149, 18);
			this.cmb_imgBrandType.Name = "cmb_imgBrandType";
			this.cmb_imgBrandType.Size = new System.Drawing.Size(136, 20);
			this.cmb_imgBrandType.TabIndex = 59;
			this.cmb_imgBrandType.SelectedIndexChanged += new System.EventHandler(this.cmb_imgBrandType_SelectedIndexChanged);
			// 
			// gbConfig
			// 
			this.gbConfig.Controls.Add(this.cmb_imgBrandType);
			this.gbConfig.Controls.Add(this.cbImageCategory);
			this.gbConfig.Controls.Add(this.lab_ImgbrandType);
			this.gbConfig.Controls.Add(this.label2);
			this.gbConfig.Controls.Add(this.txt_dbPwd);
			this.gbConfig.Controls.Add(this.txt_dbAccount);
			this.gbConfig.Controls.Add(this.lab_DbPwd);
			this.gbConfig.Controls.Add(this.lab_DbAccount);
			this.gbConfig.Controls.Add(this.lab_RootDirectory);
			this.gbConfig.Controls.Add(this.txt_rootDirectory);
			this.gbConfig.Location = new System.Drawing.Point(12, 181);
			this.gbConfig.Name = "gbConfig";
			this.gbConfig.Size = new System.Drawing.Size(336, 178);
			this.gbConfig.TabIndex = 8;
			this.gbConfig.TabStop = false;
			this.gbConfig.Text = "影像配置";
			// 
			// cbImageCategory
			// 
			this.cbImageCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbImageCategory.FormattingEnabled = true;
			this.cbImageCategory.Items.AddRange(new object[] {
            "全景",
            "小牙片",
            "CBCT",
            "照片",
            "其它"});
			this.cbImageCategory.Location = new System.Drawing.Point(149, 85);
			this.cbImageCategory.Name = "cbImageCategory";
			this.cbImageCategory.Size = new System.Drawing.Size(136, 20);
			this.cbImageCategory.TabIndex = 60;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(50, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 12);
			this.label2.TabIndex = 59;
			this.label2.Text = "默认上传类型：";
			// 
			// gbManually
			// 
			this.gbManually.Controls.Add(this.ckDbSwin);
			this.gbManually.Controls.Add(this.chk_PointNix);
			this.gbManually.Controls.Add(this.label4);
			this.gbManually.Controls.Add(this.chk_EasyDent);
			this.gbManually.Controls.Add(this.chk_Sidexis);
			this.gbManually.Location = new System.Drawing.Point(364, 204);
			this.gbManually.Name = "gbManually";
			this.gbManually.Size = new System.Drawing.Size(346, 66);
			this.gbManually.TabIndex = 28;
			this.gbManually.TabStop = false;
			this.gbManually.Text = "手动上传";
			// 
			// chk_PointNix
			// 
			this.chk_PointNix.AutoSize = true;
			this.chk_PointNix.Location = new System.Drawing.Point(214, 27);
			this.chk_PointNix.Name = "chk_PointNix";
			this.chk_PointNix.Size = new System.Drawing.Size(72, 16);
			this.chk_PointNix.TabIndex = 38;
			this.chk_PointNix.Text = "PointNix";
			this.chk_PointNix.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(21, 29);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 12);
			this.label4.TabIndex = 37;
			this.label4.Text = "影像类型：";
			// 
			// chk_EasyDent
			// 
			this.chk_EasyDent.AutoSize = true;
			this.chk_EasyDent.Location = new System.Drawing.Point(147, 27);
			this.chk_EasyDent.Name = "chk_EasyDent";
			this.chk_EasyDent.Size = new System.Drawing.Size(72, 16);
			this.chk_EasyDent.TabIndex = 1;
			this.chk_EasyDent.Text = "EasyDent";
			this.chk_EasyDent.UseVisualStyleBackColor = true;
			// 
			// chk_Sidexis
			// 
			this.chk_Sidexis.AutoSize = true;
			this.chk_Sidexis.Location = new System.Drawing.Point(86, 27);
			this.chk_Sidexis.Name = "chk_Sidexis";
			this.chk_Sidexis.Size = new System.Drawing.Size(66, 16);
			this.chk_Sidexis.TabIndex = 0;
			this.chk_Sidexis.Text = "Sidexis";
			this.chk_Sidexis.UseVisualStyleBackColor = true;
			// 
			// rbDbSwin
			// 
			this.rbDbSwin.AutoSize = true;
			this.rbDbSwin.Location = new System.Drawing.Point(283, 30);
			this.rbDbSwin.Name = "rbDbSwin";
			this.rbDbSwin.Size = new System.Drawing.Size(59, 16);
			this.rbDbSwin.TabIndex = 38;
			this.rbDbSwin.TabStop = true;
			this.rbDbSwin.Text = "DBSwin";
			this.rbDbSwin.UseVisualStyleBackColor = true;
			// 
			// ckDbSwin
			// 
			this.ckDbSwin.AutoSize = true;
			this.ckDbSwin.Location = new System.Drawing.Point(283, 27);
			this.ckDbSwin.Name = "ckDbSwin";
			this.ckDbSwin.Size = new System.Drawing.Size(60, 16);
			this.ckDbSwin.TabIndex = 39;
			this.ckDbSwin.Text = "DBSwin";
			this.ckDbSwin.UseVisualStyleBackColor = true;
			// 
			// ConfigurationView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(727, 365);
			this.Controls.Add(this.gbManually);
			this.Controls.Add(this.gbConfig);
			this.Controls.Add(this.grp_uploadType);
			this.Controls.Add(this.grp_Common);
			this.Controls.Add(this.grp_autoConfig);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.btn_Confirm);
			this.MaximizeBox = false;
			this.Name = "ConfigurationView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "影像上传配置";
			this.Load += new System.EventHandler(this.ConfigurationView_Load);
			this.grp_autoConfig.ResumeLayout(false);
			this.grp_autoConfig.PerformLayout();
			this.grp_uploadType.ResumeLayout(false);
			this.grp_uploadType.PerformLayout();
			this.grp_Common.ResumeLayout(false);
			this.grp_Common.PerformLayout();
			this.gbConfig.ResumeLayout(false);
			this.gbConfig.PerformLayout();
			this.gbManually.ResumeLayout(false);
			this.gbManually.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btn_Confirm;
		private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.GroupBox grp_autoConfig;
		private System.Windows.Forms.GroupBox grp_uploadType;
		private System.Windows.Forms.CheckBox chk_Manually;
		private System.Windows.Forms.CheckBox chk_Auto;
		private System.Windows.Forms.Label lab_Domain;
		private System.Windows.Forms.Label lab_Officekeyword;
		private System.Windows.Forms.Label lab_LogAccount;
		private System.Windows.Forms.Label lab_LogPwd;
		private System.Windows.Forms.Label lab_RootDirectory;
		private System.Windows.Forms.TextBox txt_domain;
		private System.Windows.Forms.TextBox txt_officekeyword;
		private System.Windows.Forms.TextBox txt_logAccount;
		private System.Windows.Forms.TextBox txt_logPwd;
		private System.Windows.Forms.TextBox txt_rootDirectory;
        private System.Windows.Forms.Label lab_ImgbrandType;
		private System.Windows.Forms.Label lab_DbAccount;
        private System.Windows.Forms.Label lab_DbPwd;
		private System.Windows.Forms.TextBox txt_dbAccount;
		private System.Windows.Forms.TextBox txt_dbPwd;
		private System.Windows.Forms.GroupBox grp_Common;
        private System.Windows.Forms.ComboBox cmb_imgBrandType;
        private System.Windows.Forms.GroupBox gbConfig;
        private System.Windows.Forms.GroupBox gbManually;
        private System.Windows.Forms.CheckBox chk_EasyDent;
        private System.Windows.Forms.CheckBox chk_Sidexis;
        private System.Windows.Forms.DateTimePicker dtEtime;
        private System.Windows.Forms.DateTimePicker dtStime;
        private System.Windows.Forms.CheckBox chk_SEtime;
        private System.Windows.Forms.ComboBox cbImageCategory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbEasyDent;
        private System.Windows.Forms.RadioButton rbSidexis;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton rbPointNix;
		private System.Windows.Forms.CheckBox chk_PointNix;
		private System.Windows.Forms.DateTimePicker dtp_runTime;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton rbDbSwin;
		private System.Windows.Forms.CheckBox ckDbSwin;
	}
}