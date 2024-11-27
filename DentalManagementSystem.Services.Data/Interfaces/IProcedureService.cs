namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProcedureService
    {
        Task<IEnumerable<ProcedureIndexViewModel>> IndexGetAllAsync();

        Task AddProcedureAsync(AddProcedureFormModel model);

        Task<ProcedureDetailsViewModel?> GetProcedureDetailsByIdAsync(int id);
    }
}
