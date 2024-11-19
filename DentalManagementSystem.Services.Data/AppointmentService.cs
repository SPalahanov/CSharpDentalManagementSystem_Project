namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Common.Enums;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AppointmentService : BaseService, IAppointmentService
    {
        private readonly IRepository<Appointment, Guid> appointmentRepository;
        private readonly IRepository<Patient, Guid> patientRepository;
        private readonly IRepository<Dentist, Guid> dentistRepository;
        private readonly IRepository<Procedure, int> procedureRepository;
        private readonly IRepository<AppointmentType, int> appointmentTypeRepository;

        public AppointmentService(IRepository<Appointment, Guid> appointmentRepository, 
                                  IRepository<Patient, Guid> patientRepository,
                                  IRepository<Dentist, Guid> dentistRepository,
                                  IRepository<AppointmentType, int> appointmentTypeRepository,
                                  IRepository<Procedure, int> procedureRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.patientRepository = patientRepository;
            this.dentistRepository = dentistRepository;
            this.appointmentTypeRepository = appointmentTypeRepository;
            this.procedureRepository = procedureRepository;
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

        public async Task<CreateAppointmentViewModel> GetCreateAppointmentModelAsync()
        {
            IEnumerable<ProcedureAppointmentViewModel> procedures = await this.procedureRepository
                .GetAllAttached()
                .Select(p => new ProcedureAppointmentViewModel { Id = p.ProcedureId, Name = p.Name })
                .ToArrayAsync();

            return new CreateAppointmentViewModel
            {
                Patients = await this.patientRepository
                    .GetAllAttached()
                    .Select(p => new PatientAppointmentViewModel { Id = p.PatientId, Name = p.Name })
                    .ToArrayAsync(),
                Dentists = await this.dentistRepository
                    .GetAllAttached()
                    .Select(d => new DentistAppointmentViewModel { Id = d.DentistId, Name = d.Name })
                    .ToArrayAsync(),
                AppointmentTypes = await this.appointmentTypeRepository
                    .GetAllAttached()
                    .Select(at => new AppointmentTypeViewModel { Id = at.Id, Name = at.Name })
                    .ToArrayAsync(),
                AvailableProcedures = procedures
            };
        }

        public async Task<IEnumerable<ProcedureAppointmentViewModel>> GetAvailableProceduresAsync()
        {
            IEnumerable<Procedure> procedures = await this.procedureRepository
                .GetAllAsync();

            List<ProcedureAppointmentViewModel> procedureViewModels = procedures
                .Select(p => new ProcedureAppointmentViewModel
                {
                    Id = p.ProcedureId,
                    Name = p.Name
                }).ToList();

            return procedureViewModels;
        }

        public async Task<bool> CreateAppointmentAsync(CreateAppointmentViewModel model)
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = Guid.NewGuid(),
                AppointmentDate = model.AppointmentDate,
                AppointmentTypeId = model.AppointmentTypeId,
                PatientId = model.PatientId,
                DentistId = model.DentistId,
                AppointmentStatus = AppointmentStatus.Schedule
            };

            List<Procedure> selectedProcedures = await this.procedureRepository
                .GetAllAttached()
                .Where(p => model.SelectedProcedureIds.Contains(p.ProcedureId))
                .ToListAsync();

            foreach (var procedure in selectedProcedures)
            {
                appointment.AppointmentProcedures.Add(new AppointmentProcedure
                {
                    ProcedureId = procedure.ProcedureId,
                    IsDeleted = false
                });
            }

            await this.appointmentRepository.AddAsync(appointment);
            return true;
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
