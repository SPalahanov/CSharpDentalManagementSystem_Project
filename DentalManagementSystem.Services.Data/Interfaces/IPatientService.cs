namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Web.ViewModels.Patient;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPatientService
    {
        Task<IEnumerable<AllPatientsIndexViewModel>> GetAllPatientsAsync();
    }
}
