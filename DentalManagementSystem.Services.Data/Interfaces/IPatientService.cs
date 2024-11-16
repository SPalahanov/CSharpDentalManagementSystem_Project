namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using DentalManagementSystem.Web.ViewModels.Patient;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPatientService
    {
        Task<IEnumerable<AllPatientsIndexViewModel>> GetAllPatientsAsync();

        Task<bool> PatientExistsByUserIdAsync(string userId);

        //Task<bool> AddPatientAsync(AddPatientInputModel inputModel);

        //Task<PatientDetailsViewModel?> GetPatientDetailsByIdAsync(Guid id);

        Task<bool> CreatePatientAsync(string userId, BecomePatientFormModel model);

        Task<bool> IsUserPatient(string userId);
    }
}
