namespace DentalManagementSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using DentalManagementSystem.Web.ViewModels.Patient;
    using DentalManagementSystem.Web.ViewModels.Procedure;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Patient;

    public class PatientService : BaseService, IPatientService
    {
        private readonly IRepository<Patient, Guid> patientRepository;
        private readonly IRepository<Appointment, Guid> appointmentRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public PatientService(IRepository<Patient, Guid> patientRepository, IRepository<Appointment, Guid> appointmentRepository, UserManager<ApplicationUser> userManager)
        {
            this.patientRepository = patientRepository;
            this.appointmentRepository = appointmentRepository;
            this.userManager = userManager;
        }

        public async Task<Guid> GetPatientIdByUserIdAsync(Guid userId)
        {
            Patient? patient = await this.patientRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
            {
                return Guid.Empty;
            }

            return patient.PatientId;
        }

        public async Task<IEnumerable<AllPatientsIndexViewModel>> GetAllPatientsAsync(AllPatientsSearchViewModel inputModel)
        {
            IQueryable<Patient> allPatientsQuery = this.patientRepository
                .GetAllAttached()
                .OrderBy(p => p.Name);

            if (!String.IsNullOrWhiteSpace(inputModel.SearchQuery))
            {
                allPatientsQuery = allPatientsQuery
                    .Where(p => p.Name.ToLower().Contains(inputModel.SearchQuery.ToLower()));
            }

            if (inputModel.CurrentPage.HasValue && inputModel.EntitiesPerPage.HasValue)
            {
                allPatientsQuery = allPatientsQuery
                    .Skip(inputModel.EntitiesPerPage.Value * (inputModel.CurrentPage.Value - 1))
                    .Take(inputModel.EntitiesPerPage.Value);
            }

            return await allPatientsQuery
                .Select(p => new AllPatientsIndexViewModel()
                {
                    Id = p.PatientId.ToString(),
                    Name = p.Name,
                    Gender = p.Gender
                })
                .ToArrayAsync();
        }
        public async Task<IEnumerable<AppointmentDetailsViewModel>> GetPatientDashboardAsync(Guid patientId)
        {
            DateTime today = DateTime.Today;

            IEnumerable<AppointmentDetailsViewModel> appointments = await this.appointmentRepository
                .GetAllAttached()
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Dentist)
                .Include(a => a.AppointmentProcedures)
                .ThenInclude(ap => ap.Procedure)
                .Where(a => a.AppointmentDate < today.Date)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => new AppointmentDetailsViewModel
                {
                    AppointmentDate = a.AppointmentDate.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                    DentistName = a.Dentist.Name,
                    AppointmentStatus = a.AppointmentStatus.ToString(),
                    Procedures = a.AppointmentProcedures
                        .Where(ap => !ap.IsDeleted)
                        .Select(ap => new AppointmentProcedureViewModel
                        {
                            Name = ap.Procedure.Name,
                            Price = ap.Procedure.Price,
                            Description = ap.Procedure.Description
                        }).ToList()
                })
                .ToListAsync();

            return appointments;
        }

        public async Task<bool> CreatePatientFromUserAsync(string userId, AddPatientInputModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SelectedUserId) || !Guid.TryParse(model.SelectedUserId, out Guid selectedUserGuid))
            {
                return false;
            }

            bool isAlreadyPatient = await this.patientRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .AnyAsync(p => p.UserId == selectedUserGuid);

            if (isAlreadyPatient)
            {
                return false;
            }

            bool isDateOfBirthValid = DateTime
                .TryParseExact(model.DateOfBirth, DateOfBirthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime dateOfBirth);

            if (!isDateOfBirthValid)
            {
                return false;
            }

            var patient = new Patient
            {
                UserId = selectedUserGuid,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Gender = model.Gender,
                DateOfBirth = dateOfBirth,
                Allergies = model.Allergies,
                InsuranceNumber = model.InsuranceNumber,
                EmergencyContact = model.EmergencyContact,
            };

            await this.patientRepository.AddAsync(patient);

            return true;
        }
        public async Task<bool> CreatePatientAsync(string userId, BecomePatientFormModel model)
        {
            bool isDateOfBirth = DateTime
                .TryParseExact(model.DateOfBirth, DateOfBirthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime dateOfBirth);

            if (!isDateOfBirth)
            {
                return false;
            }

            Patient patient = new Patient()
            {
                UserId = Guid.Parse(userId),
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Gender = model.Gender,
                DateOfBirth = dateOfBirth,
                Allergies = model.Allergies,
                InsuranceNumber = model.InsuranceNumber,
                EmergencyContact = model.EmergencyContact,
            };

            await this.patientRepository.AddAsync(patient);
            return true;
        }

        public async Task<IEnumerable<UserEmailViewModel>> GetUserEmailsAsync()
        {
            var users = this.userManager.Users.ToList();

            var userEmailViewModels = new List<UserEmailViewModel>();

            foreach (var user in users)
            {
                userEmailViewModels.Add(new UserEmailViewModel
                {
                    Id = user.Id.ToString(),
                    Email = user.Email
                });
            }

            return userEmailViewModels;
        }
        public async Task<bool> IsUserPatient(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            bool result = await this.patientRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .AnyAsync(p => p.UserId.ToString().ToLower() == userId);

            return result;
        }

        public async Task<bool> PatientExistsByUserIdAsync(string userId)
        {
            bool result = await this.patientRepository
                .GetAllAttached()
                .AnyAsync(p => p.UserId.ToString() == userId);

            return result;
        }

        public async Task<PatientDetailsViewModel?> GetPatientDetailsByIdAsync(Guid id)
        {
            Patient? patient = await this.patientRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            PatientDetailsViewModel? viewModel = null;

            if (patient != null)
            {
                viewModel = new PatientDetailsViewModel()
                {
                    Name = patient.Name,
                    PhoneNumber = patient.PhoneNumber,
                    Address = patient.Address,
                    Gender = patient.Gender,
                    DateOfBirth = patient.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allergies = patient.Allergies,
                    InsuranceNumber = patient.InsuranceNumber,
                    EmergencyContact =patient.EmergencyContact,
                };
            }

            return viewModel;
        }

        public async Task<EditPatientFormModel?> GetPatientForEditByIdAsync(Guid id)
        {
            EditPatientFormModel? patientModel = await this.patientRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .Select(p => new EditPatientFormModel()
                {
                    Id = p.PatientId.ToString(),
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    Gender = p.Gender,
                    DateOfBirth = p.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Allergies = p.Allergies,
                    InsuranceNumber = p.InsuranceNumber,
                    EmergencyContact = p.EmergencyContact,
                })
                .FirstOrDefaultAsync(p => p.Id.ToLower() == id.ToString().ToLower());

            if (patientModel == null)
            {
                return null;
            }

            EditPatientFormModel editModel = new EditPatientFormModel
            {
                Id = patientModel.Id,
                Name = patientModel.Name,
                PhoneNumber = patientModel.PhoneNumber,
                Address = patientModel.Address,
                Gender = patientModel.Gender,
                DateOfBirth = patientModel.DateOfBirth,
                Allergies = patientModel.Allergies,
                InsuranceNumber = patientModel.InsuranceNumber,
                EmergencyContact = patientModel.EmergencyContact,
            };

            return editModel;
        }
        public async Task<bool> EditPatientAsync(EditPatientFormModel model)
        {
            bool isDateOfBirth = DateTime
                .TryParseExact(model.DateOfBirth, DateOfBirthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime dateOfBirth);

            if (!isDateOfBirth)
            {
                return false;
            }

            Patient? patientEntity = await this.patientRepository
                .GetByIdAsync(Guid.Parse(model.Id));

            patientEntity.Name = model.Name;
            patientEntity.PhoneNumber = model.PhoneNumber;
            patientEntity.Address = model.Address;
            patientEntity.Gender = model.Gender;
            patientEntity.DateOfBirth = dateOfBirth;
            patientEntity.Allergies = model.Allergies;
            patientEntity.InsuranceNumber = model.InsuranceNumber;
            patientEntity.EmergencyContact = model.EmergencyContact;

            bool result = await this.patientRepository.UpdateAsync(patientEntity);

            return result;
        }

        public async Task<DeletePatientViewModel?> GetPatientForDeleteByIdAsync(Guid id)
        {
            DeletePatientViewModel? patientToDelete = await this.patientRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .Select(p => new DeletePatientViewModel()
                {
                    Id = p.PatientId.ToString(),
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    Gender = p.Gender,
                    DateOfBirth = p.DateOfBirth.ToString(DateOfBirthFormat),
                    Allergies = p.Allergies,
                    InsuranceNumber = p.InsuranceNumber,
                    EmergencyContact = p.EmergencyContact,
                })
                .FirstOrDefaultAsync(p => p.Id.ToLower() == id.ToString().ToLower());

            return patientToDelete;
        }
        public async Task<bool> SoftDeletePatientAsync(Guid id)
        {
            Patient patientToDelete = await this.patientRepository
                .FirstOrDefaultAsync(p => p.PatientId.ToString().ToLower() == id.ToString().ToLower());

            if (patientToDelete == null)
            {
                return false;
            }
            patientToDelete.IsDeleted = true;

            await this.patientRepository.UpdateAsync(patientToDelete);

            return true;
        }

        public async Task<int> GetPatientsCountByFilterAsync(AllPatientsSearchViewModel inputModel)
        {
            AllPatientsSearchViewModel inputModelCopy = new AllPatientsSearchViewModel()
            {
                CurrentPage = null,
                EntitiesPerPage = null,
                SearchQuery = inputModel.SearchQuery,
            };

            int patientsCount = (await this.GetAllPatientsAsync(inputModelCopy)).Count();

            return patientsCount;
        }
    }
}