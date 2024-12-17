namespace DentalManagementSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Procedure;

    using Microsoft.EntityFrameworkCore;

    public class ProcedureService : BaseService, IProcedureService
    {
        private IRepository<Procedure, int> procedureRepository;

        public ProcedureService(IRepository<Procedure, int> procedureRepository)
        {
            this.procedureRepository = procedureRepository;
        }
        public async Task<bool> ProcedureExistsAsync(int id)
        {
            bool result = await this.procedureRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .AnyAsync(p => p.ProcedureId == id);

            return result;
        }

        public async Task<IEnumerable<ProcedureIndexViewModel>> IndexGetAllAsync()
        {
            IEnumerable<ProcedureIndexViewModel> procedures = await this.procedureRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProcedureIndexViewModel
                {
                    Id = p.ProcedureId,
                    Name = p.Name
                })
                .ToArrayAsync();

            return procedures;
        }

        public async Task AddProcedureAsync(AddProcedureFormModel model)
        {
            Procedure procedure = new Procedure()
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
            };

            await this.procedureRepository.AddAsync(procedure);
        }

        public async Task<ProcedureDetailsViewModel?> GetProcedureDetailsByIdAsync(int id)
        {
            Procedure? procedure = await this.procedureRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .Include(p => p.AppointmentProcedures)
                .ThenInclude(p => p.Appointment)
                .FirstOrDefaultAsync(p => p.ProcedureId == id);

            ProcedureDetailsViewModel? viewModel = null;

            if (procedure != null)
            {
                viewModel = new ProcedureDetailsViewModel()
                {
                    Name = procedure.Name,
                    Price = procedure.Price,
                    Description = procedure.Description
                };
            };

            return viewModel;
        }

        public async Task<DeleteProcedureViewModel?> GetProcedureForDeleteByIdAsync(int id)
        {
            DeleteProcedureViewModel? procedureToDelete = await this.procedureRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted == false)
                .Select(p => new DeleteProcedureViewModel()
                {
                    Id = p.ProcedureId,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            return procedureToDelete;
        }
        public async Task<bool> SoftDeleteProcedureAsync(int id)
        {
            Procedure procedureToDelete = await this.procedureRepository
                .FirstOrDefaultAsync(p => p.ProcedureId == id);

            if (procedureToDelete == null)
            {
                return false;
            }

            procedureToDelete.IsDeleted = true;

            await this.procedureRepository.UpdateAsync(procedureToDelete);

            return true;
        }
    }
}
