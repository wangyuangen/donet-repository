using Img.Model.Dtos;
using Img.Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.SaasAPIRequest
{
	public class AccountRequest
	{
		public List<DomainDto> CheckDomain(Tenant tenant)
		{
			string uri = "api/v1/account/Check";
			return HttpHelper.httpHelper.PostRequest<Tenant,List<DomainDto>>(uri,tenant);			 
		}

		public Tuple<bool,string> Logon(LogonDto logon)
		{
			string uri = "api/v1/account/login";
			string json = JsonConvert.SerializeObject(logon);
			Tuple<bool,string> result = HttpHelper.httpHelper.tokenRequest(uri,json);

            return result;
		}

		public bool LogOut(LogOutDto logout)
		{
			string uri = "api/v1/image/validate";
			string json = JsonConvert.SerializeObject(logout);
			return HttpHelper.httpHelper.boolRequest(uri, json);
		}
	}
}
