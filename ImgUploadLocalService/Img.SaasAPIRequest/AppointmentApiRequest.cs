using Img.Model.Dtos;
using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.SaasAPIRequest
{
	public class AppointmentApiRequest
	{
        public int CreateAppointment(Appointment appointment) 
        {
            string uri = "api/v1/appointment/Create";
            int apptId = HttpHelper.httpHelper.PostRequest<Appointment, int>(uri, appointment);
            return apptId;
        }

        public List<AppointmentDto> GetAppointment(AppointmentSearch appSearch)
        {
            string uri = "api/v1/appointment/Get";
            List<AppointmentDto> appointment = HttpHelper.httpHelper.PostRequest<AppointmentSearch, List<AppointmentDto>>(uri, appSearch);
            return appointment;
        }
	}
}
