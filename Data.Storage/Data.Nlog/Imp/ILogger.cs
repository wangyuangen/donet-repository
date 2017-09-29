using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Nlog.Imp
{
	public interface ILogger
	{
		void Error(object msg, Exception exp = null);

		void Debug(object msg, Exception exp = null);

		void Info(object msg, Exception exp = null);

		void Warn(object msg, Exception exp = null);
	}
}
