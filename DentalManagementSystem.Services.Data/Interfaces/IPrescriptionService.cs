namespace DentalManagementSystem.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    using DentalManagementSystem.Web.ViewModels.Prescription;

    public interface IPrescriptionService
    {
        Task<bool> AddPrescriptionAsync(CreatePrescriptionFormModel model);

        Task<DeletePrescriptionViewModel?> GetPrescriptionForDeleteByIdAsync(Guid id);

        Task<bool> SoftDeletePrescriptionAsync(Guid id);
    }
}
