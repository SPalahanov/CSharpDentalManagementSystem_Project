using DentalManagementSystem.Data.Models;
using DentalManagementSystem.Data.Repository.Interfaces;
using DentalManagementSystem.Services.Data.Interfaces;
using DentalManagementSystem.Services.Data;
using DentalManagementSystem.Web.ViewModels.Procedure;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;
using DentalManagementSystem.Web.ViewModels.Dentist;
using DentalManagementSystem.Common.Enums;
using DentalManagementSystem.Web.ViewModels.Appointment;

namespace DentalManagementSystem.Services.Test;

public class ProcedureServiceTests
{
    private IList<Procedure> proceduresData = new List<Procedure>()
    {
        new Procedure()
        {
            Name = "Cavity Filling",
            Price = 70.00m,
            Description = "Treatment of dental cavities with high-quality composite filling to restore the tooth's function and aesthetics.",
            IsDeleted = false
        },
        new Procedure()
        {
            Name = "Teeth Cleaning",
            Price = 30.00m,
            Description = "Professional cleaning to remove tartar and plaque using an ultrasonic scaler.",
            IsDeleted = false
        }
    };

    private Mock<IRepository<Procedure, int>> procedureRepository;

    [SetUp]
    public void Setup()
    {
        this.procedureRepository = new Mock<IRepository<Procedure, int>>();
    }

    [Test]
    public async Task ProcedureExistsAsyncPositive()
    {
        int procedureId = 1;

        IQueryable<Procedure> proceduresMockQueryable = proceduresData.BuildMock();

        this.procedureRepository
            .Setup(r => r.GetAllAttached())
        .Returns(proceduresMockQueryable);

        IProcedureService procedureService = new ProcedureService(this.procedureRepository.Object);

        bool result = await procedureService.ProcedureExistsAsync(procedureId);

        Assert.IsFalse(result);
    }

    [Test]
    public async Task IndexGetAllAsyncPositive()
    {
        int procedureId = 1;

        IQueryable<Procedure> proceduresMockQueryable = proceduresData.BuildMock();

        this.procedureRepository
            .Setup(r => r.GetAllAttached())
            .Returns(proceduresMockQueryable);

        IProcedureService procedureService = new ProcedureService(this.procedureRepository.Object);

        IEnumerable<ProcedureIndexViewModel> allProceduresActual = await procedureService
            .IndexGetAllAsync();

        Assert.IsNotNull(allProceduresActual);
        Assert.AreEqual(this.proceduresData.Count(), allProceduresActual.Count());

        allProceduresActual = allProceduresActual
            .OrderBy(d => d.Id)
            .ToList();

        int i = 0;

        foreach (ProcedureIndexViewModel returnedProcedures in allProceduresActual)
        {
            Assert.AreEqual(this.proceduresData.OrderBy(p => p.ProcedureId).ToList()[i++].ProcedureId,
                returnedProcedures.Id);
        }
    }

    [Test]
    public async Task GetProcedureDetailsByIdAsyncNegative()
    {
        IQueryable<Procedure> proceduresMockQueryable = proceduresData.BuildMock();

        int procedureId = 10;

        this.procedureRepository
            .Setup(r => r.GetAllAttached())
            .Returns(proceduresMockQueryable);

        IProcedureService procedureService = new ProcedureService(this.procedureRepository.Object);

        ProcedureDetailsViewModel? procedureDetailsActual = await procedureService
            .GetProcedureDetailsByIdAsync(procedureId);

        Assert.IsNull(procedureDetailsActual);
    }

    [Test]
    public async Task GetProcedureForDeleteByIdAsyncNegative()
    {
        IQueryable<Procedure> proceduresMockQueryable = proceduresData.BuildMock();

        int procedureId = 115;

        this.procedureRepository
            .Setup(r => r.GetAllAttached())
            .Returns(proceduresMockQueryable);

        IProcedureService procedureService = new ProcedureService(this.procedureRepository.Object);

        DeleteProcedureViewModel? procedureDetailsActual = await procedureService
            .GetProcedureForDeleteByIdAsync(procedureId);

        Assert.IsNull(procedureDetailsActual);
    }

    [Test]
    public async Task AddProcedureAsync()
    {
        IQueryable<Procedure> proceduresMockQueryable = proceduresData.BuildMock();

        this.procedureRepository
            .Setup(r => r.GetAllAttached())
            .Returns(proceduresMockQueryable);

        IProcedureService procedureService = new ProcedureService(this.procedureRepository.Object); 

        AddProcedureFormModel model = new AddProcedureFormModel
        {
            Name = "Cavity Filling",
            Price = 70.00m,
            Description = "Treatment of dental cavities with high-quality composite filling to restore the tooth's function and aesthetics.",
        };

        await procedureService.AddProcedureAsync(model);

        Assert.IsNotNull(procedureService);
    }
}