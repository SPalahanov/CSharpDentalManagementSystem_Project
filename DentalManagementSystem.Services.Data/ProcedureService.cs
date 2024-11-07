using DentalManagementSystem.Services.Data.Interfaces;

namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Data;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProcedureService : IProcedureService
    {
        private readonly DentalManagementSystemDbContext dbContext;

        public ProcedureService(DentalManagementSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task AddProcedureAsync(AddProcedureFormModel model)
        {
            throw new NotImplementedException();
        }


        public Task<ProcedureDetailsViewModel?> GetProcedureDetailsByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProcedureIndexViewModel>> IndexGetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
