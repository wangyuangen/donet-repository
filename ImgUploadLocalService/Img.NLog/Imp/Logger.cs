using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.Model;
namespace Img.Nlog.Imp
{
	public class Logger : ILogger
	{
		private  static NLog.Logger _logger ;

		public Logger()
		{
			if (_logger == null)
				_logger = NLog.LogManager.GetCurrentClassLogger();
		}

		public void Error(object msg, Exception exp = null)
		{
			if (exp == null)
				_logger.Error(msg);
			else
				_logger.Error(msg + " Exception:" + exp.ToString());
		}

		public void Debug(object msg, Exception exp = null)
		{
			if (exp == null)
				_logger.Debug(msg);
			else
				_logger.Debug(msg + " Exception:" + exp.ToString());
		}

		public void Info(object msg, Exception exp = null)
		{
			if (exp == null)
				_logger.Info(msg);
			else
				_logger.Info(msg + " Exception:" + exp.ToString());
		}

		public void Warn(object msg, Exception exp = null)
		{
			if (exp == null)
				_logger.Warn(msg);
			else
				_logger.Warn(msg + " Exception:" + exp.ToString());
		}

        public string Name
        {
            get
            {
                return "NLog";
            }
        }
	}
}
