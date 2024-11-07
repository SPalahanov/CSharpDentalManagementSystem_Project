namespace DentalManagementSystem.Services.Data.Interfaces
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

        Task Create(string userId, BecomeDentistFormModel model);
    }
}
