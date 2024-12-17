namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Prescription;

    using Microsoft.EntityFrameworkCore;

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

        public async Task<DeletePrescriptionViewModel?> GetPrescriptionForDeleteByIdAsync(Guid id)
        {
            DeletePrescriptionViewModel? prescriptionToDelete = await this.prescriptionRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .Select(p => new DeletePrescriptionViewModel()
                {
                    PrescriptionId = p.PrescriptionId.ToString(),
                    MedicationName = p.MedicationName,
                    MedicationDescription = p.MedicationDescription,
                    AppointmentId = p.AppointmentId
                })
                .FirstOrDefaultAsync(p => p.PrescriptionId.ToLower() == id.ToString().ToLower());

            return prescriptionToDelete;
        }

        public async Task<bool> SoftDeletePrescriptionAsync(Guid id)
        {
            Prescription prescriptionToDelete = await this.prescriptionRepository
                .GetByIdAsync(id);

            if (prescriptionRepository == null)
            {
                return false;
            }

            prescriptionToDelete.IsDeleted = true;

            await this.prescriptionRepository.UpdateAsync(prescriptionToDelete);

            return true;
        }
        
    }
}
