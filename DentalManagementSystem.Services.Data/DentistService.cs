namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Data;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using DentalManagementSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public class DentistService : BaseService, IDentistService
    {
        private readonly IRepository<Dentist, Guid> dentistRepository;
        private readonly IRepository<Appointment, Guid> appointmentRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public DentistService(IRepository<Dentist, Guid> dentistRepository, IRepository<Appointment, Guid> appointmentRepository, UserManager<ApplicationUser> userManager)
        {
            this.dentistRepository = dentistRepository;
            this.userManager = userManager;
            this.appointmentRepository = appointmentRepository;
        }
        public async Task<Guid> GetDentistIdByUserIdAsync(Guid userId)
        {
            Dentist? dentist = await this.dentistRepository
                .GetAllAttached()
                .Where(d => d.IsDeleted == false)
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (dentist == null)
            {
                return Guid.Empty;
            }

            return dentist.DentistId;
        }

        public async Task<IEnumerable<AllDentistIndexViewModel>> GetAllDentistsAsync(AllDentistsSearchViewModel inputModel)
        {
            IQueryable<Dentist> allDentistsQuery = this.dentistRepository
                .GetAllAttached();

            if (!String.IsNullOrWhiteSpace(inputModel.SearchQuery))
            {
                allDentistsQuery = allDentistsQuery
                    .Where(d => d.Name.ToLower().Contains(inputModel.SearchQuery.ToLower()));
            }

            if (inputModel.CurrentPage.HasValue && inputModel.EntitiesPerPage.HasValue)
            {
                allDentistsQuery = allDentistsQuery
                    .Skip(inputModel.EntitiesPerPage.Value * (inputModel.CurrentPage.Value - 1))
                    .Take(inputModel.EntitiesPerPage.Value);
            }

            return await allDentistsQuery
                .Select(d => new AllDentistIndexViewModel()
                {
                    Id = d.DentistId.ToString(),
                    Name = d.Name,
                    PhoneNumber = d.PhoneNumber,
                    Address = d.Address,
                    Gender = d.Gender,
                    Specialty = d.Specialty,
                    LicenseNumber = d.LicenseNumber

                })
                .ToArrayAsync();
        }
        public async Task<DentistDashboardViewModel> GetDentistDashboardAsync(Guid dentistId)
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            DateTime monthStart = new DateTime(today.Year, today.Month, 1);

            List<Appointment> todayAppointments = await this.appointmentRepository.GetAllAttached()
                .Where(a => a.IsDeleted == false)
                .Where(a => a.DentistId == dentistId && a.AppointmentDate >= today.Date && a.AppointmentDate < tomorrow.Date)
                .Include(a => a.Patient)
                .ToListAsync();

            int monthlyPatientsCount = await this.appointmentRepository.GetAllAttached()
                .Where(a => a.IsDeleted == false)
                .Where(a => a.DentistId == dentistId && a.AppointmentDate >= monthStart && a.AppointmentDate < monthStart.AddMonths(1))
                .Select(a => a.PatientId)
                .Distinct()
                .CountAsync();

            return new DentistDashboardViewModel
            {
                TodayAppointments = todayAppointments.Select(a => new AppointmentViewModel
                {
                    PatientName = a.Patient.Name,
                    AppointmentDate = a.AppointmentDate.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                    AppointmentStatus = a.AppointmentStatus.ToString()
                }).ToList(),
                TodayAppointmentCount = todayAppointments.Count(),
                MonthlyPatientCount = monthlyPatientsCount
            };
        }

        public async Task<bool> CreateDentistAsync(string userId, BecomeDentistFormModel model)
        {
            Dentist dentist = new Dentist()
            {
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Gender = model.Gender,
                Specialty = model.Specialty,
                LicenseNumber = model.LicenseNumber,
                UserId = Guid.Parse(userId)
            };

            await this.dentistRepository.AddAsync(dentist);
            return true;
        }
        public async Task<bool> CreateDentistFromUserAsync(string userId, AddDentistInputModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SelectedUserId) || !Guid.TryParse(model.SelectedUserId, out Guid selectedUserGuid))
            {
                return false;
            }

            bool isAlreadyDentist = await this.dentistRepository
                .GetAllAttached()
                .Where(d => d.IsDeleted == false)
                .AnyAsync(d => d.UserId == selectedUserGuid);

            if (isAlreadyDentist)
            {
                return false;
            }

            Dentist dentist = new Dentist
            {
                UserId = selectedUserGuid,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Gender = model.Gender,
                Specialty = model.Specialty,
                LicenseNumber = model.LicenseNumber,
            };

            await this.dentistRepository.AddAsync(dentist);

            return true;
        }

        public async Task<IEnumerable<UserEmailViewModel>> GetUserEmailsAsync()
        {
            List<ApplicationUser> users = this.userManager.Users.ToList();

            List<UserEmailViewModel> userModel = new List<UserEmailViewModel>();

            foreach (var user in users)
            {
                userModel.Add(new UserEmailViewModel
                {
                    Id = user.Id.ToString(),
                    Email = user.Email
                });
            }

            return userModel;
        }
        public async Task<bool> IsUserDentist(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            bool result = await this.dentistRepository
                .GetAllAttached()
                .Where(d => d.IsDeleted == false)
                .AnyAsync(d => d.UserId.ToString().ToLower() == userId);

            return result;
        }

        public async Task<bool> DentistExistsByLicenseNumberAsync(string phoneNumber)
        {
            bool result = await this.dentistRepository
                .GetAllAttached()
                .AnyAsync(a => a.PhoneNumber == phoneNumber);

            return result;
        }
        public async Task<bool> DentistExistsByUserIdAsync(string userId)
        {
            bool result = await this.dentistRepository
                .GetAllAttached()
                .AnyAsync(a => a.UserId.ToString() == userId);

            return result;
        }

        public async Task<DentistDetailsViewModel?> GetDentistDetailsByIdAsync(Guid id)
        {
            Dentist? dentist = await this.dentistRepository
                .GetAllAttached()
                .Where(d => d.IsDeleted == false)
                .FirstOrDefaultAsync(d => d.DentistId == id);

            DentistDetailsViewModel? viewModel = null;

            if (dentist != null)
            {
                viewModel = new DentistDetailsViewModel()
                {
                    Name = dentist.Name,
                    PhoneNumber = dentist.PhoneNumber,
                    Address = dentist.Address,
                    Gender = dentist.Gender,
                    Specialty = dentist.Specialty,
                    LicenseNumber = dentist.LicenseNumber,
                };
            }

            return viewModel;
        }

        public async Task<EditDentistFormModel?> GetDentistForEditByIdAsync(Guid id)
        {
            EditDentistFormModel? dentistModel = await this.dentistRepository
                .GetAllAttached()
                .Where(d => d.IsDeleted == false)
                .Select(d => new EditDentistFormModel()
                {
                    Id = d.DentistId.ToString(),
                    Name = d.Name,
                    PhoneNumber = d.PhoneNumber,
                    Address = d.Address,
                    Gender = d.Gender,
                    Specialty = d.Specialty,
                    LicenseNumber = d.LicenseNumber,
                })
                .FirstOrDefaultAsync(d => d.Id.ToLower() == id.ToString().ToLower());

            return dentistModel;
        }
        public async Task<bool> EditDentistAsync(EditDentistFormModel model)
        {
            Dentist dentistEntity = await this.dentistRepository
                .GetByIdAsync(Guid.Parse(model.Id));

            if (dentistEntity == null)
            {
                return false;
            }

            dentistEntity.Name = model.Name;
            dentistEntity.PhoneNumber = model.PhoneNumber;
            dentistEntity.Address = model.Address;
            dentistEntity.Gender = model.Gender;
            dentistEntity.Specialty = model.Specialty;
            dentistEntity.LicenseNumber = model.LicenseNumber;

            bool result = await this.dentistRepository.UpdateAsync(dentistEntity);

            return result;
        }

        public async Task<DeleteDentistViewModel?> GetDentistForDeleteByIdAsync(Guid id)
        {
            DeleteDentistViewModel? dentistModel = await this.dentistRepository
                .GetAllAttached()
                .Where(d => d.IsDeleted == false)
                .Select(d => new DeleteDentistViewModel()
                {
                    Id = d.DentistId.ToString(),
                    Name = d.Name,
                    PhoneNumber = d.PhoneNumber,
                    Address = d.Address,
                    Gender = d.Gender,
                    Specialty = d.Specialty,
                    LicenseNumber = d.LicenseNumber,
                })
                .FirstOrDefaultAsync(d => d.Id.ToLower() == id.ToString().ToLower());

            return dentistModel;
        }
        public async Task<bool> SoftDeleteDentistAsync(Guid id)
        {
            Dentist dentistToDelete = await this.dentistRepository
                .FirstOrDefaultAsync(d => d.DentistId.ToString().ToLower() == id.ToString().ToLower());

            if (dentistToDelete == null)
            {
                return false;
            }

            dentistToDelete.IsDeleted = true;

            await this.dentistRepository.UpdateAsync(dentistToDelete);

            return true;
        }

        public async Task<int> GetDentistsCountByFilterAsync(AllDentistsSearchViewModel inputModel)
        {
            AllDentistsSearchViewModel inputModelCopy = new AllDentistsSearchViewModel()
            {
                CurrentPage = null,
                EntitiesPerPage = null,
                SearchQuery = inputModel.SearchQuery,
            };

            int dentistsCount = (await this.GetAllDentistsAsync(inputModelCopy)).Count();

            return dentistsCount;
        }
    }
}
