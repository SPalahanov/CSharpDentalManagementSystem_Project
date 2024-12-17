namespace DentalManagementSystem.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DentalManagementSystem.Web.ViewModels.Appointment;
    using DentalManagementSystem.Web.ViewModels.Patient;

    public interface IPatientService
    {
        Task<IEnumerable<AllPatientsIndexViewModel>> GetAllPatientsAsync(AllPatientsSearchViewModel inputModel);

        Task<int> GetPatientsCountByFilterAsync(AllPatientsSearchViewModel inputModel);

        Task<IEnumerable<AppointmentDetailsViewModel>> GetPatientDashboardAsync(Guid patientId);
        Task<Guid> GetPatientIdByUserIdAsync(Guid userId);

        Task<bool> PatientExistsByUserIdAsync(string userId);

        Task<PatientDetailsViewModel?> GetPatientDetailsByIdAsync(Guid id);

        Task<bool> CreatePatientAsync(string userId, BecomePatientFormModel model);

        Task<bool> IsUserPatient(string userId);

        Task<IEnumerable<UserEmailViewModel>> GetUserEmailsAsync();

        Task<bool> CreatePatientFromUserAsync(string userId, AddPatientInputModel model);

        Task<EditPatientFormModel?> GetPatientForEditByIdAsync(Guid id);

        Task<bool> EditPatientAsync(EditPatientFormModel model);

        Task<DeletePatientViewModel?> GetPatientForDeleteByIdAsync(Guid id);

        Task<bool> SoftDeletePatientAsync(Guid id);
    }
}
