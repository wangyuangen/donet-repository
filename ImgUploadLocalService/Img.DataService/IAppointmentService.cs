using Img.Model.Dtos;
using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.DataService
{
    public interface IAppointmentService
    {
        int CreateAppointment(Appointment appointment);
        List<AppointmentDto> GetAppointment(AppointmentSearch appSearch);
    }
}
