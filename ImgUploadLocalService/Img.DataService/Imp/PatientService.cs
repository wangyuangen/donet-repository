using Img.SaasAPIRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.Model.Models;

namespace Img.DataService.Imp
{
    public class PatientService : IPatientService
    {
        private PatientRequest PatientRequest;
        public PatientService()
        {
            PatientRequest = new PatientRequest();
        }
        public Patient GetPatientByBlh(PatientSearch pt)
        {
            return PatientRequest.GetPatientByBlh(pt);
        }
    }
}
