namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Data;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DentistService : BaseService, IDentistService
    {
        private readonly DentalManagementSystemDbContext dbContext;

        private readonly IRepository<Dentist, Guid> dentistRepository;

        public DentistService(IRepository<Dentist, Guid> dentistRepository, DentalManagementSystemDbContext dbContext)
        {
            this.dentistRepository = dentistRepository;
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
    }
}
