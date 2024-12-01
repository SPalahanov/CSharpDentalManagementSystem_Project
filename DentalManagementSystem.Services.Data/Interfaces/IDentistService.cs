namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using DentalManagementSystem.Web.ViewModels.Home;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDentistService
    {
        Task<IEnumerable<AllDentistIndexViewModel>> GetAllDentistsAsync();

        Task<DentistDashboardViewModel> GetDentistDashboardAsync(Guid dentistId);
        Task<Guid> GetDentistIdByUserIdAsync(Guid userId);

        Task<bool> DentistExistsByUserIdAsync(string userId);

        Task<bool> DentistExistsByLicenseNumberAsync(string phoneNumber);

        Task<DentistDetailsViewModel?> GetDentistDetailsByIdAsync(Guid id);

        Task CreateDentistAsync(string userId, BecomeDentistFormModel model);

        Task<bool> IsUserDentist(string userId);

        Task<IEnumerable<UserEmailViewModel>> GetUserEmailsAsync();

        Task<bool> CreateDentistFromUserAsync(string userId, AddDentistInputModel model);

        Task<EditDentistFormModel?> GetDentistForEditByIdAsync(Guid id);

        Task<bool> EditDentistAsync(EditDentistFormModel model);
    }
}
