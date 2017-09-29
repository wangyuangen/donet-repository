using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.Model.Dtos;
using Img.Model.Models;
using Img.SaasAPIRequest;

namespace Img.DataService.Imp
{
    public class AccountService:IAccountService
    {
        private AccountRequest AccountRequest;
        public AccountService()
        {
            AccountRequest = new AccountRequest();
        }

        public List<DomainDto> CheckDomain(Tenant tenant)
        {
            return AccountRequest.CheckDomain(tenant);
        }

        public Tuple<bool, string> Logon(LogonDto logon)
        {
            return AccountRequest.Logon(logon);
        }

        public bool LogOut(LogOutDto logout)
        {
            return AccountRequest.LogOut(logout);
        }
    }
}
