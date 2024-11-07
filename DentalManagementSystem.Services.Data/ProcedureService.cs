﻿namespace DentalManagementSystem.Services.Data
{
    using DentalManagementSystem.Data;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Services.Mapping;
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProcedureService : IProcedureService
    {
        private readonly DentalManagementSystemDbContext dbContext;

        private IRepository<Procedure, int> procedureRepository;

        public ProcedureService(IRepository<Procedure, int> procedureRepository)
        {
            this.procedureRepository = procedureRepository;
        }

        public async Task AddProcedureAsync(AddProcedureFormModel model)
        {
            Procedure procedure = new Procedure();

            AutoMapperConfig.MapperInstance.Map(model, procedure);

            await this.procedureRepository.AddAsync(procedure);
        }


        public async Task<ProcedureDetailsViewModel?> GetProcedureDetailsByIdAsync(int id)
        {
            Procedure? procedure = await this.procedureRepository
                .GetAllAttached()
                .Include(p => p.AppointmentProcedures)
                .ThenInclude(ap => ap.Appointment)
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

        public async Task<IEnumerable<ProcedureIndexViewModel>> IndexGetAllAsync()
        {
            IEnumerable<ProcedureIndexViewModel> procedures = await this.procedureRepository
                .GetAllAttached()
                .To<ProcedureIndexViewModel>()
                .ToArrayAsync();

            return procedures;
        }
    }
}
