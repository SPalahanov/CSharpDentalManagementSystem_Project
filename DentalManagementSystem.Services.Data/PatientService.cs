namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Data;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Patient;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PatientService : IPatientService
    {
        private readonly DentalManagementSystemDbContext dbContext;

        public PatientService(DentalManagementSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /*public Task<bool> AddPatientAsync(AddPatientInputModel inputModel)
        {
            throw new NotImplementedException();
        }*/

        public async Task<IEnumerable<AllPatientsIndexViewModel>> GetAllPatientsAsync()
        {
            IEnumerable<AllPatientsIndexViewModel> allPatients = await this.dbContext
                .Patients
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

            /*return await patientRepository
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
                .ToArrayAsync();*/
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
