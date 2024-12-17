namespace DentalManagementSystem.Services.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Prescription;

    using MockQueryable;

    using Moq;

    public class PrescriptionServiceTests
    {
        private IList<Prescription> prescriptionsData = new List<Prescription>()
        {
            new Prescription()
            {
                PrescriptionId = Guid.Parse("9cd1dab8-7983-498c-8d04-6d7754a028af"),
                MedicationName = "Amoxicillin",
                MedicationDescription = "20 tablets, 500mg, take 1 tablet every 8 hours.",
                AppointmentId = Guid.Parse("ef94990b-86fa-49d5-b093-402d64adfdab"),
                IsDeleted = false
            },
            new Prescription()
            {
                PrescriptionId = Guid.Parse("949bcfcc-6c18-4be7-8216-685431fdc49d"),
                MedicationName = "Chlorhexidine Mouthwash",
                MedicationDescription = "250ml, rinse 10ml twice daily after brushing.",
                AppointmentId = Guid.Parse("7463ded4-56d0-429b-8063-b73e453d0ab5"),
                IsDeleted = true
            }
        };

        private Mock<IRepository<Prescription, Guid>> prescriptionRepository;

        [SetUp]
        public void Setup()
        {
            this.prescriptionRepository = new Mock<IRepository<Prescription, Guid>>();
        }

        [Test]
        public async Task GetPrescriptionForDeleteByIdAsync()
        {
            IQueryable<Prescription> prescriptionsMockQueryable = prescriptionsData.BuildMock();

            Guid prescriptionId = Guid.Parse("9cd1dab8-7983-498c-8d04-6d7754a028af");

            this.prescriptionRepository
                .Setup(r => r.GetAllAttached())
                .Returns(prescriptionsMockQueryable);

            IPrescriptionService prescriptionService = new PrescriptionService(this.prescriptionRepository.Object);
            
            DeletePrescriptionViewModel? prescriptionsActual = await prescriptionService.GetPrescriptionForDeleteByIdAsync(prescriptionId);
            
            Assert.IsNotNull(prescriptionsActual);
        }

        [Test]
        public async Task AddPrescriptionAsync()
        {
            IQueryable<Prescription> prescriptionsMockQueryable = prescriptionsData.BuildMock();

            this.prescriptionRepository
                .Setup(r => r.GetAllAttached())
                .Returns(prescriptionsMockQueryable);

            IPrescriptionService prescriptionService = new PrescriptionService(this.prescriptionRepository.Object);

            CreatePrescriptionFormModel model = new CreatePrescriptionFormModel
            {
                MedicationName = "Chlorhexidine Mouthwash",
                MedicationDescription = "250ml, rinse 10ml twice daily after brushing.",
                AppointmentId = Guid.Parse("7463ded4-56d0-429b-8063-b73e453d0ab5"),
            };

            await prescriptionService.AddPrescriptionAsync(model);

            Assert.IsNotNull(prescriptionService);
        }

        [Test]
        public async Task SoftDeletePrescriptionAsync()
        {
            IEnumerable<Prescription> prescriptionsMockQueryable = prescriptionsData.BuildMock();
            Guid prescriptionId = Guid.Parse("949bcfcc-6c18-4be7-8216-685431fdc49d");

            this.prescriptionRepository
                .Setup(r => r.GetByIdAsync(prescriptionId))
                .ReturnsAsync(prescriptionsMockQueryable.First);

            IPrescriptionService prescriptionService = new PrescriptionService(this.prescriptionRepository.Object);

            bool result = await prescriptionService.SoftDeletePrescriptionAsync(prescriptionId);

            Assert.IsTrue(result);
        }
    }
}
