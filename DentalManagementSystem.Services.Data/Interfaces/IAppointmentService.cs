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
        // Index.cshtml
        // List all appointments
        Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAllAppointmentsAsync();

        // Details.cshtml
        // Show appointment's details
        Task<AppointmentDetailsViewModel?> GetAppointmentDetailsByIdAsync(Guid id);

        // Create.cshtml
        // Load the list of Patients, Dentists, and AppointmentTypes in the form
        Task<CreateAppointmentViewModel> GetCreateAppointmentModelAsync();
        //save the appointment data to the database after the form has been submitted
        Task<bool> CreateAppointmentAsync(CreateAppointmentViewModel model);
    }
}
