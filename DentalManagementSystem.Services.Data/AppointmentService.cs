namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using static DentalManagementSystem.Common.Constants.EntityValidationConstants;
    using Microsoft.EntityFrameworkCore;

    public class AppointmentService : IAppointmentService
    {
        private readonly DentalManagementSystemDbContext dbContext;

        private IRepository<Appointment, int> appointmentRepository;

        public AppointmentService(IRepository<Appointment, int> appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAllAppointmentsAsync()
        {
            IEnumerable<AllAppointmentsIndexViewModel> appointments = await this.appointmentRepository
                .GetAllAttached()
                .Select(a => new AllAppointmentsIndexViewModel
                {
                    Id = a.AppointmentId.ToString(),
                    AppointmentDate = a.AppointmentDate.ToString("MM/dd/yyyy"),
                    AppointmentStatus = a.AppointmentStatus.ToString(),
                })
                .ToArrayAsync();

            return appointments;
        }

        public async Task<AppointmentDetailsViewModel?> GetAppointmentDetailsByIdAsync(Guid id)
        {
            Appointment? appointment = await this.appointmentRepository
                .GetAllAttached()
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.AppointmentProcedures)
                .ThenInclude(ap => ap.Procedure)
                .FirstOrDefaultAsync(p => p.AppointmentId == id);

            AppointmentDetailsViewModel? viewModel = null;

            if (appointment != null)
            {
                viewModel = new AppointmentDetailsViewModel()
                {
                    AppointmentDate = appointment.AppointmentDate.ToString("MM/dd/yyyy"),
                    PatientName = appointment.Patient != null ? appointment.Patient.Name : "N/A",
                    DentistName = appointment.Dentist != null ? appointment.Dentist.Name : "N/A",
                    Procedures = appointment.AppointmentProcedures
                     .Where(ap => ap.IsDeleted == false)
                     .Select(ap => new AppointmentProcedureViewModel()
                     {
                         Name = ap.Procedure.Name,
                         Price = ap.Procedure.Price,
                         Description = ap.Procedure.Description,
                     })
                     .ToArray()
                };
            }

            return viewModel;
        }
    }
}
