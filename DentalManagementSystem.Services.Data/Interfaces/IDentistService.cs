﻿namespace DentalManagementSystem.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DentalManagementSystem.Web.ViewModels.Dentist;

    public interface IDentistService
    {
        Task<IEnumerable<AllDentistIndexViewModel>> GetAllDentistsAsync(AllDentistsSearchViewModel inputModel);
        Task<int> GetDentistsCountByFilterAsync(AllDentistsSearchViewModel inputModel);

        Task<DentistDashboardViewModel> GetDentistDashboardAsync(Guid dentistId);
        Task<Guid> GetDentistIdByUserIdAsync(Guid userId);

        Task<bool> DentistExistsByUserIdAsync(string userId);

        Task<bool> DentistExistsByLicenseNumberAsync(string phoneNumber);

        Task<DentistDetailsViewModel?> GetDentistDetailsByIdAsync(Guid id);

        Task<bool> CreateDentistAsync(string userId, BecomeDentistFormModel model);

        Task<bool> IsUserDentist(string userId);

        Task<IEnumerable<UserEmailViewModel>> GetUserEmailsAsync();

        Task<bool> CreateDentistFromUserAsync(string userId, AddDentistInputModel model);

        Task<EditDentistFormModel?> GetDentistForEditByIdAsync(Guid id);

        Task<bool> EditDentistAsync(EditDentistFormModel model);

        Task<DeleteDentistViewModel?> GetDentistForDeleteByIdAsync(Guid id);
        Task<bool> SoftDeleteDentistAsync(Guid id);
    }
}
