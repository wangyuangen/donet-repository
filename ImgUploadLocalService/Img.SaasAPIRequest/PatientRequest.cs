using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.SaasAPIRequest
{
	public class PatientRequest
	{
		public Patient GetPatientByBlh(PatientSearch pt)
		{
			string uri = "api/v1/patient/Get";
			return HttpHelper.httpHelper.PostRequest<PatientSearch, Patient>(uri, pt);
		}
	}
}
