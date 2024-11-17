﻿namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDentistService
    {
        Task<IEnumerable<AllDentistIndexViewModel>> GetAllDentistsAsync();

        Task<bool> DentistExistsByUserIdAsync(string userId);

        Task<bool> DentistExistsByLicenseNumberAsync(string phoneNumber);

        Task<DentistDetailsViewModel?> GetDentistDetailsByIdAsync(Guid id);

        Task CreateDentistAsync(string userId, BecomeDentistFormModel model);

        Task<bool> IsUserDentist(string userId);

        Task<IEnumerable<UserEmailViewModel>> GetUserEmailsAsync();

        Task<bool> CreateDentistFromUserAsync(string userId, AddDentistInputModel model);
    }
}
