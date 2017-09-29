using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Img.JobLogData;
using Img.Config;
using Img.Model.Models;
using Img.Model.Dtos;
using Img.Model.Job;
using Img.Config.config;
using Img.Model.Sidexis;
using Img.Config.Model;
using Img.Nlog;
using BitMiracle.LibTiff.Classic;

namespace Img.SironaData
{
    public class SironaImgUpload : ImgUploadBase, ILogger
    {
        private static Img.Nlog.Imp.Logger logger = new Nlog.Imp.Logger();

        private static ConfigBase config = ConfigManager.Instance.GetConfig("Sidexis");
        public SironaImgUpload(string domain, string officeKeywords, string directory, int category, string userName, string userPwd)
            : base(domain, officeKeywords, directory, category, userName, userPwd)
        {

        }

        public SironaImgUpload()
        {

        }
        #region 获取西诺德数据
        /// <summary>
        /// 从上一次结束的位置开始
        /// </summary>
        /// <param name="total">取多少行记录</param>
        /// <param name="maxId">上一次上传的最大id</param>
        /// <returns></returns>
        public IEnumerable<TRawDto> GetTRawDtosByIndex(int total, int maxId)
        {
            using (IDbConnection conn = GetSqlConnection(config.DbConnectString))
            {
                var sqlStr = string.Format(@"select top {0} i.tImgCulIntImgIDPk as ImageId,i.tImgTs as ImgCreateTime,p.tPatSSlidaID as PrivateId,
							   p.tPatSName as PatientName,i.tImgSFile as FilePath from TImageRaw i 
							   inner join TPatientRaw p on i.tImgCulIntPatIDFk = p.tPatCulIntPatIDPk 
							   where i.tImgCulIntImgIDPk > {1} order by ImageId", total, maxId);
                var dataList = conn.Query<TRawDto>(sqlStr);
                return dataList;
            }
        }

        /// <summary>
        /// 根据多个id查询
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public IEnumerable<TRawDto> GetTRawDtosByIds(IEnumerable<int> idList)
        {
            using (IDbConnection conn = GetSqlConnection(config.DbConnectString))
            {
                string idArray = string.Join(",", idList);
                var sqlStr = string.Format(@"select i.tImgCulIntImgIDPk as ImageId,i.tImgTs as ImgCreateTime,p.tPatSSlidaID as PrivateId,
							   p.tPatSName as PatientName,i.tImgSFile as FilePath from TImageRaw i 
							   inner join TPatientRaw p on i.tImgCulIntPatIDFk = p.tPatCulIntPatIDPk
							   where i.tImgCulIntImgIDPk in ({0})", idArray);
                var dataList = conn.Query<TRawDto>(sqlStr);
                return dataList;
            }
        }

        /// <summary>
        /// 获取某个时间段内上传失败与未上传的影像记录
        /// </summary>
        /// <param name="idList">成功记录</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public IEnumerable<TRawDto> GetTRawDtosByTime(IEnumerable<int> idList, DateTime startTime, DateTime? endTime = null)
        {
            using (IDbConnection conn = GetSqlConnection(config.DbConnectString))
            {
                JobLogDataService joblogService = new JobLogDataService();
                if (endTime == null)
                {
                    endTime = startTime.AddHours(23).AddMinutes(59).AddSeconds(59);
                }
                string idArray = string.Join(",", idList);
                var sqlStr = string.Format(@"select i.tImgCulIntImgIDPk as ImageId,i.tImgTs as ImgCreateTime,p.tPatSSlidaID as PrivateId,
							   p.tPatSName as PatientName,i.tImgSFile as FilePath from TImageRaw i 
							   inner join TPatientRaw p on i.tImgCulIntPatIDFk = p.tPatCulIntPatIDPk
							   where i.tImgTs between '{0}' and '{1}'",
                                     startTime, endTime);
                if (!string.IsNullOrWhiteSpace(idArray))
                {
                    sqlStr = string.Concat(sqlStr, string.Format(" and i.tImgCulIntImgIDPk not in ({0})", idArray));
                }
                var dataList = conn.Query<TRawDto>(sqlStr);
                var failedList = joblogService.GetFailedId(config.JobLogDb);
                string rootDir = config.RootDirectory.EndsWith("\\") == true ? config.RootDirectory : config.RootDirectory + "\\";
                var joblogs = joblogService.GetAllJobLogs(config.JobLogDb);

                foreach (var item in dataList)
                {
                    item.UploadStatus = "未上传";
                    if (failedList.Count(x => x == item.ImageId) > 0)
                    {
                        item.UploadStatus = "上传失败";
                    }
                    item.BrandType = "Sidexis";
                    item.FilePath = rootDir + item.FilePath;
                    item.Category = config.ImageCategory;
                    var joblog = joblogs.OrderByDescending(x => x.Id).FirstOrDefault(x => x.ImgId == item.ImageId.ToString());
                    if (joblog != null)
                    {
                        item.UploadStatus = joblog.UploadStatus == UploadStatus.Successed ? "已上传" : "上传失败";
                    }
                }
                return dataList;
            }
        }

        /// <summary>
        /// 获取某个时间段内上传失败与未上传的影像记录
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public Tuple<IEnumerable<TRawDto>, int> GetTRawDtosByTime_Page(int PageSize,
            DateTime startTime, DateTime endTime, int PageIndex = 0)
        {
            using (IDbConnection conn = GetSqlConnection(config.DbConnectString))
            {
                JobLogDataService joblogService = new JobLogDataService();
                endTime = endTime.AddHours(23).AddMinutes(59).AddSeconds(59);
                var sqlstrCount = string.Format(@"select count(0) from TImageRaw i 
							   inner join TPatientRaw p on i.tImgCulIntPatIDFk = p.tPatCulIntPatIDPk
							   where i.tImgTs between '{0}' and '{1}'",
                                     startTime, endTime);

                var sqlStr = string.Format(@"select * from (select i.tImgCulIntImgIDPk as ImageId,i.tImgTs as ImgCreateTime,p.tPatSSlidaID as PrivateId,
							   p.tPatSName as PatientName,i.tImgSFile as FilePath ,ROW_NUMBER() over(order by i.tImgTs desc) rowindex
                               from TImageRaw i 
							   inner join TPatientRaw p on i.tImgCulIntPatIDFk = p.tPatCulIntPatIDPk
							   where i.tImgTs between '{0}' and '{1}'",
                                     startTime, endTime);

                sqlStr = string.Concat(sqlStr, string.Format(" ) temp where temp.rowindex between {0} and {1}", (PageIndex - 1) * PageSize, PageIndex * PageSize));
                var count = conn.ExecuteScalar<int>(sqlstrCount);

                var dataList = conn.Query<TRawDto>(sqlStr);
                var joblogs = joblogService.GetAllJobLogs(config.JobLogDb);
                string rootDir = config.RootDirectory.EndsWith("\\") == true ? config.RootDirectory : config.RootDirectory + "\\";
                foreach (var item in dataList)
                {
                    item.UploadStatus = "未上传";
                    item.BrandType = "Sidexis";
                    item.FilePath = rootDir + item.FilePath;
                    item.Category = config.ImageCategory;
                    var joblog = joblogs.OrderByDescending(x => x.Id).FirstOrDefault(x => x.ImgId == item.ImageId.ToString());
                    if (joblog != null)
                    {
                        item.UploadStatus = joblog.UploadStatus == UploadStatus.Successed ? "已上传" : "上传失败";
                    }
                }
                return new Tuple<IEnumerable<TRawDto>, int>(dataList, count);
            }
        }
        #endregion

        public static SqlConnection GetSqlConnection(string connectString)
        {
            SqlConnection conn = new SqlConnection(connectString);		//连接数据库
            if (conn.State == ConnectionState.Closed)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
            return conn;
        }

        #region 西诺德影像上传
        public void UploadImg(IEnumerable<TRawDto> rawDtoList)
        {
            var joblogService = new JobLogDataService();

            logger.Info(string.Format("昨日未上传与上传失败的影像记录：{0}", rawDtoList.Count()));

            var result = new List<Tuple<Appointment, JobLog, string, string, int>>();
            string[] PicturePathArray = Directory.GetDirectories(directory);
            DirectoryInfo dir;
            var imgfileList = new List<string>();
            foreach (var item in PicturePathArray)
            {
                dir = new DirectoryInfo(item);
                var dirArray = dir.GetFileSystemInfos();
                foreach (var imgfile in dirArray)
                {
                    try
                    {
                        dir = new DirectoryInfo(imgfile.FullName);
                        var imgFiles = dir.GetFileSystemInfos();
                        //得到所有影像完整路径
                        imgfileList.AddRange(imgFiles.Select(x => x.FullName));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            logger.Info(string.Format("获取到所有影像的完整路径：{0}", imgfileList.Count));

            AppointmentSearch apptSearch;
            Appointment appt;
            AppointmentDto apptDto;

            JobLog joblog = null;
            foreach (var item in rawDtoList)
            {
                try
                {
                    if (!imgfileList.Exists(x => x.Contains(item.FilePath)))
                    {
                        WriteLog(joblogService, joblog, item, "没有找到影像文件");
                        continue;
                    }

                    var patient = GetSaasPatients(item.PrivateId);
                    if (patient == null)
                    {
                        WriteLog(joblogService, joblog, item, "未在Saas中匹配到该患者");
                        continue;
                    }
                    //如果有预约取第一个,没有则创建
                    apptSearch = new AppointmentSearch
                    {
                        TenantId = tenantId,
                        OfficeId = officeId,
                        StartTime = item.ImgCreateTime.Date,
                        EndTime = item.ImgCreateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59),
                        PrivateId = item.PrivateId,
                        Token = Token
                    };
                    apptDto = GetAppointment(apptSearch);
                    appt = new Appointment();
                    appt.PatientId = patient.Id;
                    appt.Token = Token;
                    appt.TenantId = tenantId;
                    appt.OfficeId = officeId;
                    if (apptDto == null)
                    {
                        appt.StartTime = item.ImgCreateTime;
                        appt.EndTime = item.ImgCreateTime.AddMinutes(30);
                        appt.Notes = "由当天影像记录创建的预约";
                        appt.SourceType = "普通";
                        appt.RecordCreatedTime = appt.StartTime;
                        appt.Id = InsertAppointment(appt);
                    }
                    else
                    {
                        appt.PatientId = apptDto.PatientId;
                        appt.Id = apptDto.Id;
                    }

                    var fullName = string.Concat(directory, item.FilePath);//根目录拼接

                    joblog = new JobLog();
                    joblog.ImgId = item.ImageId.ToString();
                    joblog.ImgCreateTime = item.ImgCreateTime;
                    joblog.PatientName = patient.Name;
                    joblog.PrivateId = patient.PrivateId;
                    logger.Info(string.Format("预约Id：{0};影像Id：{1};文件名：{2}", appt.Id, joblog.ImgId, fullName));
                    //////////////////////////////////////////////////////////////
                    //如果希诺德上传影像为tiff,则Vertical Flip
                    string uploadFileName = fullName;
                    if (IsTiff(fullName))
                    {
                        uploadFileName = ReplaceTiffName(fullName);
                        if (uploadFileName != string.Empty)
                        {
                            TiffVerticalFlip(fullName, uploadFileName);
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    result.Add(new Tuple<Appointment, JobLog, string, string, int>(appt, joblog, uploadFileName, "", category));
                }
                catch (Exception errorlog)
                {
                    WriteLog(joblogService, joblog, item, errorlog.Message);
                }
            }
            logger.Info(string.Format("成功匹配到的影像记录：{0}", result.Count));
            Uploading(result);
        }

        /// <summary>
        /// 手动上传影像
        /// </summary>
        /// <param name="list"></param>
        public void UploadImg_Page(IEnumerable<TRawDto> list)
        {
            var joblogService = new JobLogDataService();

            //要上传的影像记录
            var rawDtoList = list;
            logger.Info(string.Format("手动上传的影像记录：{0}", rawDtoList.Count()));

            var result = new List<Tuple<Appointment, JobLog, string, string, int>>();
            string[] PicturePathArray = Directory.GetDirectories(directory);
            DirectoryInfo dir;
            var imgfileList = new List<string>();
            foreach (var item in PicturePathArray)
            {
                dir = new DirectoryInfo(item);
                var dirArray = dir.GetFileSystemInfos();
                foreach (var imgfile in dirArray)
                {
                    try
                    {
                        dir = new DirectoryInfo(imgfile.FullName);
                        var imgFiles = dir.GetFileSystemInfos();
                        //得到所有影像完整路径
                        imgfileList.AddRange(imgFiles.Select(x => x.FullName));
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            logger.Info(string.Format("获取到所有影像的完整路径：{0}", imgfileList.Count));

            AppointmentSearch apptSearch;
            Appointment appt;
            AppointmentDto apptDto;

            JobLog joblog = null;
            foreach (var item in rawDtoList)
            {
                try
                {
                    if (!imgfileList.Exists(x => x.Contains(item.FilePath)))
                    {
                        WriteLog(joblogService, joblog, item, "没有找到影像文件");
                        continue;
                    }

                    var patient = GetSaasPatients(item.PrivateId);
                    if (patient == null)
                    {
                        WriteLog(joblogService, joblog, item, "未在Saas中匹配到该患者");
                        continue;
                    }
                    //如果有预约取第一个,没有则创建
                    apptSearch = new AppointmentSearch
                    {
                        TenantId = tenantId,
                        OfficeId = officeId,
                        StartTime = item.ImgCreateTime.Date,
                        EndTime = item.ImgCreateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59),
                        PrivateId = item.PrivateId,
                        Token = Token
                    };
                    apptDto = GetAppointment(apptSearch);
                    appt = new Appointment();
                    appt.PatientId = patient.Id;
                    appt.Token = Token;
                    appt.TenantId = tenantId;
                    appt.OfficeId = officeId;
                    if (apptDto == null)
                    {
                        appt.StartTime = item.ImgCreateTime;
                        appt.EndTime = item.ImgCreateTime.AddMinutes(30);
                        appt.Notes = "由当天影像记录创建的预约";
                        appt.SourceType = "普通";
                        appt.RecordCreatedTime = appt.StartTime;
                        appt.Id = InsertAppointment(appt);
                    }
                    else
                    {
                        appt.PatientId = apptDto.PatientId;
                        appt.Id = apptDto.Id;
                    }

                    var fullName = string.Concat(directory, item.FilePath);//根目录拼接

                    joblog = new JobLog();
                    joblog.ImgId = item.ImageId.ToString();
                    joblog.ImgCreateTime = item.ImgCreateTime;
                    joblog.PatientName = patient.Name;
                    joblog.PrivateId = patient.PrivateId;
                    logger.Info(string.Format("预约Id：{0};影像Id：{1};文件名：{2}", appt.Id, joblog.ImgId, fullName));
                    //////////////////////////////////////////////////////////////
                    //如果希诺德上传影像为tiff,则Vertical Flip
                    string uploadFileName = fullName;
                    if (IsTiff(fullName))
                    {
                        uploadFileName = ReplaceTiffName(fullName);
                        if (uploadFileName != string.Empty)
                        {
                            TiffVerticalFlip(fullName, uploadFileName);
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    result.Add(new Tuple<Appointment, JobLog, string, string, int>(appt, joblog, fullName, "", category));
                }
                catch (Exception errorlog)
                {
                    WriteLog(joblogService, joblog, item, errorlog.Message);
                }
            }
            logger.Info(string.Format("成功匹配到的影像记录：{0}", result.Count));
            Uploading(result);
        }
        #endregion

        public static void WriteLog(JobLogDataService joblogService, JobLog joblog, TRawDto item, string errorlog)
        {
            joblog = new JobLog();
            joblog.ImgId = item.ImageId.ToString();
            joblog.ImgCreateTime = item.ImgCreateTime;
            joblog.PatientName = item.PatientName;
            joblog.PrivateId = item.PrivateId;
            joblog.UploadStatus = UploadStatus.Failed;
            joblog.UploadTime = DateTime.Now;
            joblog.ErrorLog = errorlog;
            joblog.BrandType = "Sidexis";
            joblogService.InsertJobLog(joblog,config.JobLogDb);
        }

        public void Error(object msg, Exception exp = null)
        {
            
        }

        public void Debug(object msg, Exception exp = null)
        {
            
        }

        public void Info(object msg, Exception exp = null)
        {
            using (IDbConnection conn = GetSqlConnection(config.JobLogDb))
            {
                var sqlStr = @"insert into JobLog(ImgId,ImgCreateTime,UploadTime,PatientName,PrivateId,UploadStatus,ErrorLog,brandtype) 
							values(@ImgId,@ImgCreateTime,@UploadTime,@PatientName,@PrivateId,@UploadStatus,@ErrorLog,@brandtype)";
                conn.Execute(sqlStr, msg);
            }
        }

        public void Warn(object msg, Exception exp = null)
        {
            
        }

        private bool IsTiff(string uploadFileName)
        {
            string postFix = uploadFileName.Substring(uploadFileName.Length - 4, 4);
            if (postFix.ToLower() == ".tif")
            {
                return true;
            }

            return false;

        }


        private string ReplaceTiffName(string inputFileName)
        {
            try
            {
                int index = inputFileName.ToLower().IndexOf(".tif");
                if (index != -1)
                {
                    string s = inputFileName.Substring(index, 4);
                    inputFileName = inputFileName.Replace(s, "_lc.tif");
                    return inputFileName;
                }

                return string.Empty;
            }
            catch (Exception errorlog)
            {
                return string.Empty;
            }

        }


        private void TiffVerticalFlip(string inputFileName, string outputFileName)
        {
            using (Tiff input = Tiff.Open(inputFileName, "r"))
            {
                using (Tiff output = Tiff.Open(outputFileName, "w"))
                {
                    for (short page = 0; page < input.NumberOfDirectories(); page++)
                    {
                        input.SetDirectory(page);
                        output.SetDirectory(page);

                        int width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                        int height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                        int samplesPerPixel = input.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
                        int bitsPerSample = input.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();
                        int photo = input.GetField(TiffTag.PHOTOMETRIC)[0].ToInt();


                        int[] raster = new int[width * height];
                        input.ReadRGBAImageOriented(width, height, raster, Orientation.TOPLEFT);


                        output.SetField(TiffTag.IMAGEWIDTH, width);
                        output.SetField(TiffTag.IMAGELENGTH, height);
                        output.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                        output.SetField(TiffTag.BITSPERSAMPLE, 8);
                        output.SetField(TiffTag.ROWSPERSTRIP, height);
                        output.SetField(TiffTag.PHOTOMETRIC, photo);
                        output.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                        output.SetField(TiffTag.COMPRESSION, Compression.DEFLATE);
                        output.SetField(TiffTag.ORIENTATION, Orientation.BOTLEFT);

                        byte[] strip = rasterToRgbBuffer(raster);
                        output.WriteEncodedStrip(0, strip, strip.Length);

                        output.WriteDirectory();
                    }
                }
            }
        }

        private static byte[] rasterToRgbBuffer(int[] raster)
        {
            byte[] buffer = new byte[raster.Length * 3];
            for (int i = 0; i < raster.Length; i++)
                Buffer.BlockCopy(raster, i * 4, buffer, i * 3, 3);

            return buffer;
        }

        private static int[] rotate(int[] buffer, int angle, ref int width, ref int height)
        {
            int rotatedWidth = width;
            int rotatedHeight = height;
            int numberOf90s = angle / 90;
            if (numberOf90s % 2 != 0)
            {
                int tmp = rotatedWidth;
                rotatedWidth = rotatedHeight;
                rotatedHeight = tmp;
            }

            int[] rotated = new int[rotatedWidth * rotatedHeight];

            for (int h = 0; h < height; ++h)
            {
                for (int w = 0; w < width; ++w)
                {
                    int item = buffer[h * width + w];
                    int x = 0;
                    int y = 0;
                    switch (numberOf90s % 4)
                    {
                        case 0:
                            x = w;
                            y = h;
                            break;

                        case 1:
                            x = (height - h - 1);
                            y = (rotatedHeight - 1) - (width - w - 1);
                            break;

                        case 2:
                            x = (width - w - 1);
                            y = (height - h - 1);

                            break;

                        case 3:
                            x = (rotatedWidth - 1) - (height - h - 1);
                            y = (width - w - 1);
                            break;
                    }

                    rotated[y * rotatedWidth + x] = item;
                }
            }

            width = rotatedWidth;
            height = rotatedHeight;
            return rotated;
        }

    }
}
