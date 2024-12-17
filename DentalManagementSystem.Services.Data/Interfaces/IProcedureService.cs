namespace DentalManagementSystem.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DentalManagementSystem.Web.ViewModels.Procedure;

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
