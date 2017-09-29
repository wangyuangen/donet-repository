using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using Img.Config;
using Img.Config.config;
using Img.Config.Model;
using Img.Model.EasyDent;
using Img.DataService;
using System.Runtime.InteropServices;

namespace ImgConfigurationView
{
    public partial class ConfigurationView : Form
    {
        private Global GlobalConfig = ConfigManager.Instance.Global;
        private ConfigBase config;
        private Dictionary<string, ConfigBase> configs;
        public ConfigurationView()
        {
            InitializeComponent();
            configs = StartUp.LoadConfig();
        }

        /// <summary>
        /// 服务名称
        /// </summary>
        private static string ServiceName = "ImgAutoUploadService";

        private static Img.Nlog.Imp.Logger log = new Img.Nlog.Imp.Logger();

        /// <summary>
        /// 确认事件触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            if (chk_Auto.Checked)
            {
                if (!rbSidexis.Checked && !rbEasyDent.Checked && !rbPointNix.Checked && !rbDbSwin.Checked)
                {
                    MessageBox.Show("影像类型不能为空", "系统提示");
                    return;
                }
                if (cmb_imgBrandType.SelectedItem.ToString() == "==请选择==")
                {
                    MessageBox.Show("影像品牌不能为空", "系统提示");
                    return;
                }
            }

            if (txt_domain.Text == "")
            {
                MessageBox.Show("SaaS代码不能为空", "系统提示");
                return;
            }
            if (txt_officekeyword.Text == "")
            {
                MessageBox.Show("诊所关键字不能为空", "系统提示");
                return;
            }

            if (txt_logAccount.Text == "")
            {
                MessageBox.Show("SaaS登录账户不能为空", "系统提示");
                return;
            }
            if (txt_logPwd.Text == "")
            {
                MessageBox.Show("SaaS密码不能为空", "系统提示");
                return;
            }

            if (txt_rootDirectory.Text == "")
            {
                MessageBox.Show("影像根目录不能为空", "系统提示");
                return;
            }
			//if (txt_dbAccount.Text == "")
			//{
			//	MessageBox.Show("数据库登录账号不能为空", "系统提示");
			//	return;
			//}
			//if (txt_dbPwd.Text == "")
			//{
			//	MessageBox.Show("数据库登录密码不能为空", "系统提示");
			//	return;
			//}

            if (chk_Manually.Checked)
            {
                if (!chk_Sidexis.Checked && !chk_EasyDent.Checked &&!chk_PointNix.Checked && !ckDbSwin.Checked)
                {
                    MessageBox.Show("请至少选择一个手动上传类型");
                    return;
                }
            }

            if (chk_SEtime.Checked)
            {
                if (dtStime.Text == "" || dtEtime.Text == "")
                {
                    MessageBox.Show("请选择起止时间");
                    return;
                }

                DateTime startTime = Convert.ToDateTime(dtStime.Text);
                DateTime endTime = Convert.ToDateTime(dtEtime.Text);
                if (DateTime.Compare(startTime, endTime) > 0)
                {
                    MessageBox.Show("结束时间必须大于开始时间");
                    return;
                }
            }

            SetConfig();
            var currentBrandType = cmb_imgBrandType.SelectedItem.ToString();
            StartUp.CreateLogDataBase(config.Master);

            //Runcmd(string.Format("Net Stop {0}", ServiceName));
            //var str = "修改配置完成!";
            //if (chk_Auto.Checked)
            //{
                //var runTime = dt_runTime.Value.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);
                //str += string.Format("自动上传服务将于{0}点执行.", runTime);

            //    if (!ServiceIsExisted())
            //    {
            //        Runcmd(System.AppDomain.CurrentDomain.BaseDirectory + "Install.bat");
            //    }
            //    Runcmd(string.Format("Net Start {0}", ServiceName));
            //    Runcmd(string.Format("sc config {0} start= auto", ServiceName));
            //}
            //else
            //{
            //    Runcmd(string.Format("sc config {0} start= demand", ServiceName));
            //}

            MessageBox.Show("保存成功");
            //MessageBox.Show(str, "系统提示");
        }

        /// <summary>
        /// 执行cmd命令
        /// </summary>
        /// <param name="cmdStr"></param>
        private void Runcmd(string cmdStr)
        {
            try
            {
                //ShellExecute(IntPtr.Zero, "runas", @"c:\test.flv", "", "", ShowCommands.SW_SHOWNORMAL);
                //ShellExecute(0, "runas", LPCSTR("cmd.exe"), LPCSTR("/c net user administrator /active:yes"), "", SW_HIDE);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;//不显示程序窗口
                p.Start();//启动程序
                //向cmd窗口发送输入信息
                p.StandardInput.WriteLine(cmdStr + "&exit");

                p.StandardInput.AutoFlush = true;
                string output = p.StandardOutput.ReadToEnd();
                //p.StandardInput.WriteLine("exit");
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
            }
        }

        /// <summary>
        /// 校验影像上传服务是否存在
        /// </summary>
        /// <returns></returns>
        private bool ServiceIsExisted()
        {
            ServiceController[] services = ServiceController.GetServices();

            foreach (var item in services)
                if (item.ServiceName == ServiceName)
                    return true;

            return false;
        }

        /// <summary>
        /// 选中事件触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_Auto_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Auto.Checked)
                grp_autoConfig.Enabled = true;
            else
                grp_autoConfig.Enabled = false;
        }


        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        /// <summary>
        /// 界面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigurationView_Load(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(GlobalConfig.AutoUploadBrandType))
            {
                return;
            }
            GetConfig(GetBrandType());
        }

        private ConfigBase GetBrandType(string DefaultBrandType = "")
        {
            //首次加载窗体
            if (DefaultBrandType == "")
            {
                DefaultBrandType = GlobalConfig.AutoUploadBrandType;
            }
            
            foreach(var item in configs)
            {
                if(item.Key.Contains(DefaultBrandType))
                {
                    config = item.Value;
                    break;
                }
            }
            //if (DefaultBrandType == "Sidexis")
            //{
            //    config = ConfigManager.Instance.Sidexis;
            //}
            //else if (DefaultBrandType == "EasyDent")
            //{
            //    config = ConfigManager.Instance.EasyDent;
            //}
            return config;
        }
        #region Config
        /// <summary>
        /// 读取配置为控件赋值
        /// </summary>
        private void GetConfig(ConfigBase config)
        {
            string AutoBrandType = GlobalConfig.AutoUploadBrandType;
            string ManualBrandType = GlobalConfig.ManualUploadBrandType;
            rbSidexis.Checked = rbSidexis.Text == AutoBrandType;
            rbEasyDent.Checked = rbEasyDent.Text == AutoBrandType;
			rbPointNix.Checked = rbPointNix.Text == AutoBrandType;
			rbDbSwin.Checked = rbDbSwin.Text == AutoBrandType;
            cmb_imgBrandType.SelectedItem = AutoBrandType;
            ManualUploadConfig(gbManually, ManualBrandType);

            txt_domain.Text = GlobalConfig.Domain;
            txt_officekeyword.Text = GlobalConfig.OfficeName;
            txt_logAccount.Text = GlobalConfig.UserName;
            txt_logPwd.Text = GlobalConfig.UserPwd;
			txt_rootDirectory.Text = config.RootDirectory;
			dtp_runTime.Text = GlobalConfig.RunTime;
            //dt_runTime.Text = GlobalConfig.RunTime;
            txt_dbAccount.Text = Common.ParseDbConnectString(config.DbConnectString).Item3;
            txt_dbPwd.Text = Common.ParseDbConnectString(config.DbConnectString).Item4;
            cbImageCategory.SelectedIndex = Convert.ToInt32(config.ImageCategory) - 1;

            int FixedPeriod = GlobalConfig.FixedPeriod;
            chk_SEtime.Checked = FixedPeriod == 1 ? true : false;

            if (FixedPeriod == 1)
            {
                dtStime.Text = GlobalConfig.StartTime.ToShortDateString();
                dtEtime.Text = GlobalConfig.EndTime.ToShortDateString();
            }

            var type = GlobalConfig.UploadType;
            grp_autoConfig.Enabled = type.Contains("自动上传");
            if (type.Contains("手动上传"))
            {
                chk_Manually.Checked = true;
            }
            if (type.Contains("自动上传"))
            {
                chk_Auto.Checked = true;
            }
        }

        private void ManualUploadConfig(Control control, string configStr)
        {
            if (string.IsNullOrEmpty(configStr))
            {
                return;
            }

            List<string> listConfig = new List<string>();
            if (configStr.Contains("|"))
            {
                listConfig = configStr.Split('|').ToList();
            }
            else
            {
                listConfig.Add(configStr);
            }

            foreach (var item in control.Controls)
            {
                if (!(item is CheckBox))
                {
                    continue;
                }
                foreach (var str in listConfig)
                {

                    if ((item as CheckBox).Text == str)
                    {
                        (item as CheckBox).Checked = true;
                    }
                }
            }
        }

        private void SetConfig()
        {
            string BrandType = "";
            if (rbEasyDent.Checked)
            {
                BrandType = rbEasyDent.Text;
            }
            if (rbSidexis.Checked)
            {
                BrandType = rbSidexis.Text;
            }
			if(rbPointNix.Checked)
			{
				BrandType = rbPointNix.Text;
			}
			if(rbDbSwin.Checked)
			{
				BrandType = rbDbSwin.Text;
			}
            //cmb_imgBrandType.SelectedItem.ToString();
            var config = GetBrandType(BrandType);
            GlobalConfig.Domain = txt_domain.Text;
            GlobalConfig.OfficeName = txt_officekeyword.Text;
            GlobalConfig.UserName = txt_logAccount.Text;
            GlobalConfig.UserPwd = txt_logPwd.Text;
			GlobalConfig.RunTime = dtp_runTime.Text;
            //GlobalConfig.RunTime = dt_runTime.Value.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);
            GlobalConfig.FixedPeriod = chk_SEtime.Checked == true ? 1 : 0;
            if (chk_SEtime.Checked)
            {
                GlobalConfig.StartTime = Convert.ToDateTime(dtStime.Text);
                GlobalConfig.EndTime = Convert.ToDateTime(dtEtime.Text);
            }

            string uploadType = "";
            if (chk_Auto.Checked)
            {
                uploadType = "自动上传";
            }
            if (chk_Manually.Checked)
            {
                uploadType += "|手动上传";
                List<string> manualUpload = new List<string>();
                string SidexisUpload = chk_Sidexis.Checked == true ? "Sidexis" : "";
                if (SidexisUpload != "")
                {
                    manualUpload.Add(SidexisUpload);
                }
                string EasyDentUpload = chk_EasyDent.Checked == true ? "EasyDent" : "";
                if (EasyDentUpload != "")
                {
                    manualUpload.Add(EasyDentUpload);
                }
				string pointNixUpload = chk_PointNix.Checked == true ? "Sidexis" : "";
				if (pointNixUpload != "")
				{
					manualUpload.Add(pointNixUpload);
				}
				string dbswinUpload = ckDbSwin.Checked == true ? "DBSwin" : "";
				if (dbswinUpload != "")
				{
					manualUpload.Add(dbswinUpload);
				}
                GlobalConfig.ManualUploadBrandType = string.Join("|", manualUpload);
            }
            GlobalConfig.UploadType = uploadType;
            GlobalConfig.AutoUploadBrandType = BrandType;

            config.RootDirectory = txt_rootDirectory.Text;
            config.DbConnectString = Common.ConvertDbConnectString(".", "", txt_dbAccount.Text, txt_dbPwd.Text,config,cmb_imgBrandType.SelectedItem.ToString());
            config.Master = Common.ConvertDbConnectString(".", "master", txt_dbAccount.Text, txt_dbPwd.Text,config);
            config.JobLogDb = Common.ConvertDbConnectString(".", "JobLogDb", txt_dbAccount.Text, txt_dbPwd.Text,config);
            config.ImageCategory = cbImageCategory.SelectedIndex + 1;

            XmlHelper.WriteXmlNode(GlobalConfig.Name, GlobalConfig);
            XmlHelper.WriteXmlNode(config.Name, config);
        }
        #endregion

        private void cmb_imgBrandType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string BrandType =cmb_imgBrandType.SelectedItem.ToString();
            if(BrandType=="==请选择==")
            {
                txt_rootDirectory.Text = "";
                txt_dbAccount.Text = "";
                txt_dbPwd.Text = "";
                return;
            }
            else
            {
                foreach(var item in configs)
                {
                    if(item.Key.Contains(BrandType))
                    {
                        config = item.Value;
                    }
                }
            }

            txt_rootDirectory.Text = config.RootDirectory;
            txt_dbAccount.Text = Common.ParseDbConnectString(config.DbConnectString).Item3;
            txt_dbPwd.Text = Common.ParseDbConnectString(config.DbConnectString).Item4;
        }

        private void chk_SEtime_CheckedChanged(object sender, EventArgs e)
        {
            dtStime.Enabled = chk_SEtime.Checked;
            dtEtime.Enabled = chk_SEtime.Checked;
        }

        private void chk_Manually_CheckedChanged(object sender, EventArgs e)
        {
            gbManually.Enabled = chk_Manually.Checked;
        }
    }
}
