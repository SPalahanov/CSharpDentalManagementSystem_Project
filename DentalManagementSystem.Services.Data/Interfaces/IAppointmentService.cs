using DentalManagementSystem.Web.ViewModels.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalManagementSystem.Services.Data.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAllAppointmentsAsync();

        Task<AppointmentDetailsViewModel?> GetAppointmentDetailsByIdAsync(Guid id);
    }
}
