namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Data;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Patient;

    public class DentistService : BaseService, IDentistService
    {
        private readonly DentalManagementSystemDbContext dbContext;

        private readonly IRepository<Dentist, Guid> dentistRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public DentistService(IRepository<Dentist, Guid> dentistRepository, UserManager<ApplicationUser> userManager, DentalManagementSystemDbContext dbContext)
        {
            this.dentistRepository = dentistRepository;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public async Task CreateDentistAsync(string userId, BecomeDentistFormModel model)
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

            await this.dbContext.Dentists.AddAsync(dentist);
            await this.dbContext.SaveChangesAsync();
        }

        

        public async Task<bool> DentistExistsByLicenseNumberAsync(string phoneNumber)
        {
            bool result = await this.dbContext
                .Dentists
                .AnyAsync(a => a.PhoneNumber == phoneNumber);

            return result;
        }

        public async Task<bool> DentistExistsByUserIdAsync(string userId)
        {
            bool result = await this.dbContext
                .Dentists
                .AnyAsync(a => a.UserId.ToString() == userId);

            return result;
        }

        public async Task<IEnumerable<AllDentistIndexViewModel>> GetAllDentistsAsync()
        {
            IEnumerable<AllDentistIndexViewModel> allDentists = await this.dbContext
                .Dentists
                .Select(p => new AllDentistIndexViewModel()
                {
                    Id = p.DentistId.ToString(),
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    Gender = p.Gender,
                    Specialty = p.Specialty,
                    LicenseNumber = p.LicenseNumber

                })
                .ToArrayAsync();

            return allDentists;
        }

        public async Task<IEnumerable<UserEmailViewModel>> GetUserEmailsAsync()
        {
            List<ApplicationUser> users = userManager.Users.ToList();

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

        public async Task<bool> CreateDentistFromUserAsync(string userId, AddDentistInputModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SelectedUserId) || !Guid.TryParse(model.SelectedUserId, out Guid selectedUserGuid))
            {
                return false;
            }

            bool isAlreadyDentist = await dentistRepository
                .GetAllAttached()
                .AnyAsync(p => p.UserId == selectedUserGuid);

            if (isAlreadyDentist)
            {
                return false;
            }

            var dentist = new Dentist
            {
                UserId = selectedUserGuid,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Gender = model.Gender,
                Specialty = model.Specialty,
                LicenseNumber = model.LicenseNumber,
            };

            await dentistRepository.AddAsync(dentist);

            return true;
        }

        public async Task<bool> IsUserDentist(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            bool result = await this.dentistRepository
                .GetAllAttached()
                .AnyAsync(d => d.UserId.ToString().ToLower() == userId);

            return result;
        }

        public async Task<DentistDetailsViewModel?> GetDentistDetailsByIdAsync(Guid id)
        {
            Dentist? dentist = await this.dentistRepository
                .GetAllAttached()
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

            dentistEntity.Name = model.Name;
            dentistEntity.PhoneNumber = model.PhoneNumber;
            dentistEntity.Address = model.Address;
            dentistEntity.Gender = model.Gender;
            dentistEntity.Specialty = model.Specialty;
            dentistEntity.LicenseNumber = model.LicenseNumber;

            bool result = await this.dentistRepository.UpdateAsync(dentistEntity);

            return result;
        }

       
    }
}
