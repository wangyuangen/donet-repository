using Img.SaasAPIRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.Model.Models;
using Img.Model.Dtos;

namespace Img.DataService.Imp
{
    public class AppointmentService : IAppointmentService
    {
        private AppointmentApiRequest AppointmentApiRequest;
        public AppointmentService()
        {
            AppointmentApiRequest = new AppointmentApiRequest();
        }

        public int CreateAppointment(Appointment appointment)
        {
            return AppointmentApiRequest.CreateAppointment(appointment);
        }

        public List<AppointmentDto> GetAppointment(AppointmentSearch appSearch)
        {
            return AppointmentApiRequest.GetAppointment(appSearch);
        }
    }
}
