using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.DataService
{
    public interface IPatientService
    {
        Patient GetPatientByBlh(PatientSearch pt);
    }
}
