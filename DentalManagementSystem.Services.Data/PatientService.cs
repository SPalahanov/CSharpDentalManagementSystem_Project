namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Data;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Patient;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Patient;

    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient, Guid> patientRepository;

        public PatientService(IRepository<Patient, Guid> patientRepository)
        {
            this.patientRepository = patientRepository;
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

        public async Task<IEnumerable<AllPatientsIndexViewModel>> GetAllPatientsAsync()
        {
            IEnumerable<AllPatientsIndexViewModel> allPatients = await this.patientRepository
                .GetAllAttached()
                .Select(p => new AllPatientsIndexViewModel()
                {
                    Id = p.PatientId.ToString(),
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    DateOfBirth = p.DateOfBirth.ToString(),
                    Gender = p.Gender,
                    Allergies = p.Allergies,
                    InsuranceNumber = p.InsuranceNumber,
                    EmergencyContact = p.EmergencyContact

                })
                .ToArrayAsync();

            return allPatients;
        }

        public async Task<bool> IsUserPatient(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            bool result = await this.patientRepository
                .GetAllAttached()
                .AnyAsync(p => p.UserId.ToString().ToLower() == userId);

            return result;
        }

        public async Task<bool> PatientExistsByUserIdAsync(string userId)
        {
            bool result = await this.patientRepository
                .GetAllAttached()
                .AnyAsync(a => a.UserId.ToString() == userId);

            return result;
        }

        /*public async Task<PatientDetailsViewModel?> GetPatientDetailsByIdAsync(Guid id)
        {
            Patient? movie = await this.patientRepository
                .GetByIdAsync(id);

            PatientDetailsViewModel? viewModel = null;

            if (movie != null)
            {
                //AutoMapperConfig.MapperInstance.Map(movie, viewModel);
            }

            return viewModel;
        }*/
    }
}
