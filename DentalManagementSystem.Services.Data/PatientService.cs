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
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    public class PatientService : IPatientService
    {
        private readonly DentalManagementSystemDbContext dbContext;

        public PatientService(DentalManagementSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

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
        }
    }
}
