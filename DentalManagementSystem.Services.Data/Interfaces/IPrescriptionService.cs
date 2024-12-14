namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using DentalManagementSystem.Web.ViewModels.Prescription;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPrescriptionService
    {
        Task<bool> AddPrescriptionAsync(CreatePrescriptionFormModel model);
    }
}
