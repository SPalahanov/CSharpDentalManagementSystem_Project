namespace DentalManagementSystem.Services.Data.Interfaces
{
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IProcedureService
    {
        Task<IEnumerable<ProcedureIndexViewModel>> IndexGetAllAsync();

        Task AddProcedureAsync(AddProcedureFormModel model);

        Task<ProcedureDetailsViewModel?> GetProcedureDetailsByIdAsync(int id);
    }
}
