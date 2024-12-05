namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Web.ViewModels.Patient;
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProcedureService
    {
        Task<IEnumerable<ProcedureIndexViewModel>> IndexGetAllAsync();
        Task<ProcedureDetailsViewModel?> GetProcedureDetailsByIdAsync(int id);

        Task AddProcedureAsync(AddProcedureFormModel model);

        Task<bool> ProcedureExistsAsync(int id);

        Task<DeleteProcedureViewModel?> GetProcedureForDeleteByIdAsync(int id);
        Task<bool> SoftDeleteProcedureAsync(int id);
    }
}
