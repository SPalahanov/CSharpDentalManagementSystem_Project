namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Web.ViewModels.Patient;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPatientService
    {
        Task<IEnumerable<AllPatientsIndexViewModel>> GetAllPatientsAsync();

        Task<bool> PatientExistsByUserIdAsync(string userId);

        Task<PatientDetailsViewModel?> GetPatientDetailsByIdAsync(Guid id);

        Task<bool> CreatePatientAsync(string userId, BecomePatientFormModel model);

        Task<bool> IsUserPatient(string userId);

        Task<IEnumerable<UserEmailViewModel>> GetUserEmailsAsync();

        Task<bool> CreatePatientFromUserAsync(string userId, AddPatientInputModel model);
    }
}
