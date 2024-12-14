using DentalManagementSystem.Data.Models;
using DentalManagementSystem.Data.Repository.Interfaces;
using DentalManagementSystem.Services.Data.Interfaces;
using DentalManagementSystem.Web.ViewModels.Prescription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalManagementSystem.Services.Data
{
    public class PrescriptionService : BaseService, IPrescriptionService
    {
        private IRepository<Prescription, Guid> prescriptionRepository;

        public PrescriptionService(IRepository<Prescription, Guid> prescriptionRepository)
        {
            this.prescriptionRepository = prescriptionRepository;
        }

        public async Task<bool> AddPrescriptionAsync(CreatePrescriptionFormModel model)
        {
            Prescription prescription = new Prescription()
            {
                MedicationName = model.MedicationName,
                MedicationDescription = model.MedicationDescription,
                AppointmentId = model.AppointmentId
            };

            await this.prescriptionRepository.AddAsync(prescription);

            return true;
        }
    }
}
