using Img.Model.Dtos;
using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.DataService
{
    public interface IAccountService
    {
        List<DomainDto> CheckDomain(Tenant tenant);
        Tuple<bool, string> Logon(LogonDto logon);
        bool LogOut(LogOutDto logout);
    }
}
