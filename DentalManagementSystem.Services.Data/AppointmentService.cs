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
    using System.Globalization;
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
                .Where(a => a.IsDeleted == false)
                .OrderBy(a => a.AppointmentDate.ToString())
                .Select(a => new AllAppointmentsIndexViewModel
                {
                    Id = a.AppointmentId.ToString(),
                    AppointmentDate = a.AppointmentDate.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                    AppointmentStatus = a.AppointmentStatus.ToString(),
                })
                .ToArrayAsync();

            return appointments;
        }

        public async Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAppointmentsByPatientIdAsync(Guid patientId)
        {
            DateTime today = DateTime.Today;

            IEnumerable<AllAppointmentsIndexViewModel> appointments = await this.appointmentRepository
                .GetAllAttached()
                .Where(a => a.PatientId == patientId)
                .Where(a => a.AppointmentDate >= today.Date)
                .Where(a => a.IsDeleted == false)
                .OrderBy(a => a.AppointmentDate)
                .Select(a => new AllAppointmentsIndexViewModel
                {
                    Id = a.AppointmentId.ToString(),
                    AppointmentDate = a.AppointmentDate.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                    AppointmentStatus = a.AppointmentStatus.ToString(),
                })
                .ToListAsync();

            return appointments;
        }

        public async Task<IEnumerable<AllAppointmentsIndexViewModel>> GetAppointmentsByDentistIdAsync(Guid dentistId)
        {
            IEnumerable<AllAppointmentsIndexViewModel> appointments = await this.appointmentRepository
                .GetAllAttached()
                .Where(a => a.DentistId == dentistId)
                .Where(a => a.IsDeleted == false)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => new AllAppointmentsIndexViewModel
                {
                    Id = a.AppointmentId.ToString(),
                    AppointmentDate = a.AppointmentDate.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                    AppointmentStatus = a.AppointmentStatus.ToString(),
                })
                .ToListAsync();

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
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .ToListAsync();

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
                .Where(p => p.IsDeleted == false)
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
                .Where(a => a.IsDeleted == false)
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
                    AppointmentDate = appointment.AppointmentDate.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                    PatientName = appointment.Patient != null ? appointment.Patient.Name : "N/A",
                    DentistName = appointment.Dentist != null ? appointment.Dentist.Name : "N/A",
                    AppointmentStatus = appointment.AppointmentStatus.ToString(),
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

        public async Task<EditAppointmentFormModel?> GetAppointmentDataForEditAsync(Guid id)
        {
            EditAppointmentFormModel? appointmentModel = await this.appointmentRepository
                .GetAllAttached()
                .Where(a => a.IsDeleted == false)
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.AppointmentProcedures)
                .ThenInclude(ap => ap.Procedure)
                .Select(a => new EditAppointmentFormModel()
                {
                    Id = a.AppointmentId.ToString(),
                    AppointmentDate = a.AppointmentDate,
                    AppointmentStatus = a.AppointmentStatus,
                    AppointmentTypeId = a.AppointmentTypeId,
                    PatientId = a.PatientId,
                    DentistId = a.DentistId,
                })
                .FirstOrDefaultAsync(a => a.Id.ToLower() == id.ToString().ToLower());

            return appointmentModel;
        }
        public async Task<IEnumerable<PatientAppointmentViewModel>> GetPatientListAsync()
        {
            IEnumerable<PatientAppointmentViewModel> patientModel = await this.patientRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .Select(p => new PatientAppointmentViewModel { Id = p.PatientId, Name = p.Name })
                .ToListAsync();

            return patientModel;
        }
        public async Task<IEnumerable<DentistAppointmentViewModel>> GetDentistListAsync()
        {
            IEnumerable<DentistAppointmentViewModel> dentistModel = await this.dentistRepository
                .GetAllAttached()
                .Where(d => d.IsDeleted == false)
                .Select(d => new DentistAppointmentViewModel { Id = d.DentistId, Name = d.Name })
                .ToListAsync();

            return dentistModel;
        }
        public async Task<IEnumerable<AppointmentTypeViewModel>> GetAppointmentTypeListAsync()
        {
            IEnumerable<AppointmentTypeViewModel> appointmentTypeModel = await this.appointmentTypeRepository
                .GetAllAttached()
                .Select(at => new AppointmentTypeViewModel { Id = at.Id, Name = at.Name })
                .ToListAsync();

            return appointmentTypeModel;
        }
        public async Task<IEnumerable<ProcedureAppointmentViewModel>> GetAvailableProcedureListAsync()
        {
            IEnumerable<ProcedureAppointmentViewModel> procedureEntity =  await this.procedureRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProcedureAppointmentViewModel { Id = p.ProcedureId, Name = p.Name })
                .ToListAsync();

            return procedureEntity;
        }
        public async Task<EditAppointmentFormModel?> GetAppointmentForEditByIdAsync(Guid id)
        {
            EditAppointmentFormModel? appointmentModel = await GetAppointmentDataForEditAsync(id);

            if (appointmentModel == null)
            {
                return null;
            }

            EditAppointmentFormModel? editModel = new EditAppointmentFormModel
            {
                Id = appointmentModel.Id.ToString(),
                AppointmentDate = appointmentModel.AppointmentDate,
                AppointmentStatus = appointmentModel.AppointmentStatus,
                AppointmentTypeId = appointmentModel.AppointmentTypeId,
                PatientId = appointmentModel.PatientId,
                DentistId = appointmentModel.DentistId,
                Patients = await GetPatientListAsync(),
                Dentists = await GetDentistListAsync(),
                AppointmentTypes = await GetAppointmentTypeListAsync(),
                AvailableProcedures = await GetAvailableProceduresAsync()
            };

            return editModel;
        }
        public async Task<bool> EditAppointmentAsync(EditAppointmentFormModel model)
        {
            Appointment? appointment = await this.appointmentRepository
                .GetAllAttached()
                .Include(a => a.AppointmentProcedures)
                .FirstOrDefaultAsync(a => a.AppointmentId == Guid.Parse(model.Id));

            if (appointment == null)
            {
                return false;
            }

            appointment.AppointmentDate = model.AppointmentDate;
            appointment.AppointmentStatus = model.AppointmentStatus;
            appointment.AppointmentTypeId = model.AppointmentTypeId;
            appointment.PatientId = model.PatientId;
            appointment.DentistId = model.DentistId;

            IEnumerable<int> selectedProcedureIds = model.SelectedProcedureIds;

            IEnumerable<int> existingProcedureIds = appointment.AppointmentProcedures.Select(ap => ap.ProcedureId);

            IEnumerable<Procedure> newProcedures = await this.procedureRepository
                .GetAllAttached()
                .Where(p => selectedProcedureIds.Contains(p.ProcedureId) && !existingProcedureIds.Contains(p.ProcedureId))
                .ToListAsync();

            foreach (var procedure in newProcedures)
            {
                appointment.AppointmentProcedures.Add(new AppointmentProcedure
                {
                    ProcedureId = procedure.ProcedureId,
                    IsDeleted = false
                });
            }

            await this.appointmentRepository.UpdateAsync(appointment);

            return true;
        }

        public async Task<DeleteAppointmentViewModel?> GetAppointmentForDeleteByIdAsync(Guid id)
        {
            DeleteAppointmentViewModel? patientToDelete = await this.appointmentRepository
                .GetAllAttached()
                .Where(a => a.IsDeleted == false)
                .Select(a => new DeleteAppointmentViewModel()
                {
                    Id = p.AppointmentId.ToString(),
                    AppointmentDate = p.AppointmentDate,
                    AppointmentStatus = p.AppointmentStatus,
                    AppointmentTypeId = p.AppointmentTypeId,
                    PatientId = p.PatientId,
                    DentistId = p.DentistId,
                })
                .FirstOrDefaultAsync(p => p.Id.ToLower() == id.ToString().ToLower());

            return patientToDelete;
        }

        public async Task<bool> SoftDeleteAppointmentAsync(Guid id)
        {
            Appointment appointmentToDelete = await this.appointmentRepository
                .FirstOrDefaultAsync(p => p.AppointmentId.ToString().ToLower() == id.ToString().ToLower());

            if (appointmentRepository == null)
            {
                return false;
            }

            appointmentToDelete.IsDeleted = true;

            await this.appointmentRepository.UpdateAsync(appointmentToDelete);

            return true;
        }
    }
}
