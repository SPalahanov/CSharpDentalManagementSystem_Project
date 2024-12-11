namespace DentalManagementSystem.Services.Test
{
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.VisualBasic;
    using MockQueryable;
    using Moq;
    using NUnit.Framework;
    using System.Globalization;
    using System.Net;
    using System.Reflection;

    public class DentistServiceTests
    {
        private IList<Dentist> dentistsData = new List<Dentist>()
        {
            new Dentist()
            {
                DentistId = Guid.Parse("6844f781-c5bc-4da7-bf11-349fd5bbe9c0"),
                Name = "Georgi Petrov",
                PhoneNumber = "+359876543212",
                Address = "ul. Slivnitsa 42",
                Gender = "Male",
                Specialty = "Endodontics",
                LicenseNumber = "XX-007-XX",
                IsDeleted = false,
                UserId = Guid.Parse("70d6cddb-1787-4e7c-822b-b4b8929f1c82")
            },
            new Dentist()
            {
                DentistId = Guid.Parse("385ee459-337a-4df2-ba1b-b2f01c451b17"),
                Name = "Ivo Ivanov",
                PhoneNumber = "+359876543211",
                Address = "ul. Maritsa 23",
                Gender = "Male",
                Specialty = "Pedodontist",
                LicenseNumber = "XX-006-XX",
                IsDeleted = false,
                UserId = Guid.Parse("0a116063-736f-4104-a5d3-2a516239463c")
            }
        };

        private IList<ApplicationUser> usersData = new List<ApplicationUser>
        {
            new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Email = "meri@dentist.com"
            },
            new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Email = "desislava@dentist.com"
            }
        };

        private Mock<IRepository<Dentist, Guid>> dentistRepository;
        private Mock<IRepository<Appointment, Guid>> appointmentRepository;
        private Mock<UserManager<ApplicationUser>> userManager;

        [SetUp]
        public void Setup()
        {
            this.dentistRepository = new Mock<IRepository<Dentist, Guid>>();
            this.appointmentRepository = new Mock<IRepository<Appointment, Guid>>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            this.userManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object,
                null, // IOptions<IdentityOptions>
                null, // IPasswordHasher<ApplicationUser>
                null, // IEnumerable<IUserValidator<ApplicationUser>>
                null, // IEnumerable<IPasswordValidator<ApplicationUser>>
                null, // ILookupNormalizer
                null, // IdentityErrorDescriber
                null, // IServiceProvider
                null // ILogger<UserManager<ApplicationUser>>
            );
        }

        [Test]
        public async Task GetAllDentistsNoFilterPositive()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            IEnumerable<AllDentistIndexViewModel> allDentistsActual = await dentistService
                .GetAllDentistsAsync(new AllDentistsSearchViewModel());

            Assert.IsNotNull(allDentistsActual);
            Assert.AreEqual(this.dentistsData.Count(), allDentistsActual.Count());

            allDentistsActual = allDentistsActual
                .OrderBy(d => d.Id)
                .ToList();

            int i = 0;

            foreach (AllDentistIndexViewModel returnedDentists in allDentistsActual)
            {
                Assert.AreEqual(this.dentistsData.OrderBy(d => d.DentistId).ToList()[i++].DentistId.ToString(),
                    returnedDentists.Id);
            }
        }

        [Test]
        [TestCase("Et")]
        [TestCase("et")]
        [TestCase("eT")]
        public async Task GetAllDentistsSearchQueryPositive(string searchQuery)
        {
            int expectedDentistsCount = 1;

            string expectedDentistId = "6844f781-c5bc-4da7-bf11-349fd5bbe9c0";

            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            IEnumerable<AllDentistIndexViewModel> allDentistsActual = await dentistService
                .GetAllDentistsAsync(new AllDentistsSearchViewModel()
                {
                    SearchQuery = searchQuery
                });

            Assert.IsNotNull(allDentistsActual);
            Assert.AreEqual(expectedDentistsCount, allDentistsActual.Count());
            Assert.AreEqual(expectedDentistId.ToLower(), allDentistsActual.First().Id.ToLower());
        }

        [TestCase("qwerty")]
        [TestCase("123456")]
        [TestCase("1q2w3e")]
        public async Task GetAllDentistsSearchQueryNegative(string searchQuery)
        {
            int expectedDentistsCount = 0;

            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            IEnumerable<AllDentistIndexViewModel> allDentistsActual = await dentistService
                .GetAllDentistsAsync(new AllDentistsSearchViewModel()
                {
                    SearchQuery = searchQuery
                });

            Assert.IsNotNull(allDentistsActual);
            Assert.AreEqual(expectedDentistsCount, allDentistsActual.Count());
        }

        [Test]
        public async Task GetAllDentistsNullFilterNegative()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                IEnumerable<AllDentistIndexViewModel> allMoviesActual = await dentistService
                    .GetAllDentistsAsync(null);
            });
        }

        [Test]
        public async Task GetDentistIdByUserIdAsyncPositive()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            Guid userId = Guid.Parse("70d6cddb-1787-4e7c-822b-b4b8929f1c82");
            Guid expectedDentistId = Guid.Parse("6844f781-c5bc-4da7-bf11-349fd5bbe9c0");

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            Guid dentistIdActual = await dentistService.GetDentistIdByUserIdAsync(userId);

            Assert.IsNotNull(dentistIdActual);
            Assert.AreEqual(expectedDentistId, dentistsMockQueryable.First().DentistId);
        }

        [Test]
        public async Task GetDentistIdByUserIdAsyncNegative()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            Guid userId = Guid.NewGuid();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            Guid actualDentistId = await dentistService.GetDentistIdByUserIdAsync(userId);

            Assert.AreEqual(Guid.Empty, actualDentistId);
        }

        [Test]
        public async Task GetDentistIdWhenIsDeletedByUserIdAsyncNegative()
        {
            IList<Dentist> deletedDentist = new List<Dentist>()
            {
                new Dentist()
                {
                    DentistId = Guid.Parse("6844f781-c5bc-4da7-bf11-349fd5bbe9c0"),
                    UserId = Guid.Parse("70d6cddb-1787-4e7c-822b-b4b8929f1c82"),
                    IsDeleted = true
                },
            };

            IQueryable<Dentist> dentistsMockQueryable = deletedDentist.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            Guid actualDentistId =
                await dentistService.GetDentistIdByUserIdAsync(Guid.Parse("70d6cddb-1787-4e7c-822b-b4b8929f1c82"));

            Assert.AreEqual(Guid.Empty, actualDentistId);

        }

        [Test]
        public async Task CreateDentistFromUserAsyncPositive()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            string userId = "70d6cddb-1787-4e7c-822b-b4b8929f1c82";

            AddDentistInputModel model = new AddDentistInputModel
            {
                Name = "Georgi Petrov",
                PhoneNumber = "+359876543212",
                Address = "ul. Slivnitsa 42",
                Gender = "Male",
                Specialty = "Endodontics",
                LicenseNumber = "XX-007-XX"
            };

            await dentistService.CreateDentistFromUserAsync(userId, model);

            Dentist createdDentist = dentistsData.First();

            Assert.AreEqual(Guid.Parse(userId), createdDentist.UserId);
            Assert.AreEqual("Georgi Petrov", createdDentist.Name);
            Assert.AreEqual("+359876543212", createdDentist.PhoneNumber);
            Assert.AreEqual("ul. Slivnitsa 42", createdDentist.Address);
            Assert.AreEqual("Male", createdDentist.Gender);
        }

        [Test]
        public async Task CreateDentistFromInvalidUserAsyncNegative()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            string userId = "70d6cddb-1787-4e7c-822b-b4b8929f1c82";

            AddDentistInputModel model = new AddDentistInputModel
            {
                Name = "Georgi Petrov",
                PhoneNumber = "+359876543212",
                Address = "ul. Slivnitsa 42",
                Gender = "Male",
                Specialty = "Endodontics",
                LicenseNumber = "XX-007-XX",
                SelectedUserId = "invalid-guid"
            };

            bool result = await dentistService.CreateDentistFromUserAsync(userId, model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateDentistFromUserAlreadyDentistAsyncNegative()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            string userId = "70d6cddb-1787-4e7c-822b-b4b8929f1c82";

            AddDentistInputModel model = new AddDentistInputModel
            {
                Name = "Georgi Petrov",
                PhoneNumber = "+359876543212",
                Address = "ul. Slivnitsa 42",
                Gender = "Male",
                Specialty = "Endodontics",
                LicenseNumber = "XX-007-XX",
                SelectedUserId = "70d6cddb-1787-4e7c-822b-b4b8929f1c82"
            };

            bool result = await dentistService.CreateDentistFromUserAsync(userId, model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateDentistFromUserWithInvalidLicenseNumberAsyncNegative()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            string userId = "7126cddb-1787-4e7c-822b-b4b8929f1c82";

            AddDentistInputModel model = new AddDentistInputModel
            {
                Name = "Georgi Petrov",
                PhoneNumber = "+359876543212",
                Address = "ul. Slivnitsa 42",
                Gender = "Male",
                Specialty = "Endodontics",
                LicenseNumber = null,
                SelectedUserId = "7126cddb-1787-4e7c-822b-b4b8929f1c82"
            };

            bool result = await dentistService.CreateDentistFromUserAsync(userId, model);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsUserDentistPositive()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            Guid userId = Guid.Parse("0a116063-736f-4104-a5d3-2a516239463c");

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            bool result = await dentistService.IsUserDentist(userId.ToString());

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsUserDentistNegative()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            Guid userId = Guid.Parse("385ee459-337a-4df2-ba1b-b2f01c451b17");

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            bool result = await dentistService.IsUserDentist(userId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetDentistDetailsByIdAsyncPositive()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            Guid patientId = Guid.Parse("6844f781-c5bc-4da7-bf11-349fd5bbe9c0");
            string expectedDentistName = "Georgi Petrov";
            string expectedDentistPhoneNumber = "+359876543212";
            string expectedDentistLicenseNumber = "XX-007-XX";
            string expectedDentistSpecialty = "Endodontics";

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            DentistDetailsViewModel? dentistDetailsActual = await dentistService
                .GetDentistDetailsByIdAsync(patientId);

            Assert.IsNotNull(dentistDetailsActual);
            Assert.AreEqual(expectedDentistName, dentistDetailsActual.Name);
            Assert.AreEqual(expectedDentistPhoneNumber, dentistDetailsActual.PhoneNumber);
            Assert.AreEqual(expectedDentistLicenseNumber, dentistDetailsActual.LicenseNumber);
            Assert.AreEqual(expectedDentistSpecialty, dentistDetailsActual.Specialty);
        }

        [Test]
        public async Task GetDentistDetailsByIdAsyncNegative()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            Guid patientId = Guid.Empty;

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                DentistDetailsViewModel? dentistDetailsActual = await dentistService
                    .GetDentistDetailsByIdAsync(patientId);

                if (dentistDetailsActual == null)
                {
                    throw new NullReferenceException();
                }
            });
        }

        [Test]
        public async Task GetDentistForEditByIdAsyncPositive()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            Guid patientId = Guid.Parse("6844f781-c5bc-4da7-bf11-349fd5bbe9c0");
            string expectedDentistName = "Georgi Petrov";
            string expectedDentistGenre = "Male";
            string expectedDentistPhoneNumber = "+359876543212";
            string expectedDentistLicenseNumber = "XX-007-XX";
            string expectedDentistSpecialty = "Endodontics";


            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            EditDentistFormModel? dentistDetailsActual = await dentistService
                .GetDentistForEditByIdAsync(patientId);

            Assert.IsNotNull(dentistDetailsActual);
            Assert.AreEqual(expectedDentistName, dentistDetailsActual.Name);
            Assert.AreEqual(expectedDentistGenre, dentistDetailsActual.Gender);
            Assert.AreEqual(expectedDentistPhoneNumber, dentistDetailsActual.PhoneNumber);
            Assert.AreEqual(expectedDentistLicenseNumber, dentistDetailsActual.LicenseNumber);
            Assert.AreEqual(expectedDentistSpecialty, dentistDetailsActual.Specialty);
        }

        [Test]
        public async Task GetDentistForEditByIdAsyncNegative()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            Guid patientId = Guid.Empty;

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                EditDentistFormModel? dentistDetailsActual = await dentistService
                    .GetDentistForEditByIdAsync(patientId);

                if (dentistDetailsActual == null)
                {
                    throw new NullReferenceException();
                }
            });
        }

        [Test]
        public async Task EditDentistAsyncPositive()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetByIdAsync())
                .ReturnsAsync((Guid id) => dentistsData.FirstOrDefault(d => d.DentistId == id));

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            EditDentistFormModel model = new EditDentistFormModel
            {
                Id = "6844f781-c5bc-4da7-bf11-349fd5bbe9c0",
                Name = "Georgi Petrov",
                PhoneNumber = "+359876543212",
                Address = "ul. Slivnitsa 41",
                Gender = "Male",
                Specialty = "Endodontics",
                LicenseNumber = "XX-007-XX"
            };

            bool result = await dentistService.EditDentistAsync(model);

            Assert.IsNotNull(result);

            Dentist updatedDentist = dentistsData.First();

            Assert.AreEqual("Georgi Petrov", updatedDentist.Name);
            Assert.AreEqual("+359876543212", updatedDentist.PhoneNumber);
            Assert.AreEqual("ul. Slivnitsa 42", updatedDentist.Address);
            Assert.AreEqual("Male", updatedDentist.Gender);
            Assert.AreEqual("Endodontics", updatedDentist.Specialty);
            Assert.AreEqual("XX-007-XX", updatedDentist.LicenseNumber);
        }

        [Test]
        public async Task EditDentistNotExistAsyncNegative()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetByIdAsync())
                .ReturnsAsync((Guid id) => dentistsData.FirstOrDefault(d => d.DentistId == id));

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            EditDentistFormModel model = new EditDentistFormModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Georgi Petrov",
                PhoneNumber = "+359876543212",
                Address = "ul. Slivnitsa 42",
                Gender = "Male",
                Specialty = "Endodontics",
                LicenseNumber = "XX-007-XX"
            };

            bool result = await dentistService.EditDentistAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetDentistForDeleteByIdAsyncPositive()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            Guid dentistId = Guid.Parse("385ee459-337a-4df2-ba1b-b2f01c451b17");

            string expectedDentistId = "385ee459-337a-4df2-ba1b-b2f01c451b17";
            string expectedDentistGenre = "Male";
            string expectedDentistAddress = "ul. Maritsa 23";
            string expectedDentistPhoneNumber = "+359876543211";

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            DeleteDentistViewModel? dentistDetailsActual = await dentistService
                .GetDentistForDeleteByIdAsync(dentistId);

            Assert.IsNotNull(dentistDetailsActual);
            Assert.AreEqual(expectedDentistId.ToLower(), dentistDetailsActual.Id.ToLower());
            Assert.AreEqual(expectedDentistGenre, dentistDetailsActual.Gender);
            Assert.AreEqual(expectedDentistAddress, dentistDetailsActual.Address);
            Assert.AreEqual(expectedDentistPhoneNumber, dentistDetailsActual.PhoneNumber);
        }

        [Test]
        public async Task GetPatientForDeleteByIdAsyncNegative()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            Guid dentistId = Guid.Empty;

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                DeleteDentistViewModel? dentistDetailsActual = await dentistService
                    .GetDentistForDeleteByIdAsync(dentistId);

                if (dentistDetailsActual == null)
                {
                    throw new NullReferenceException();
                }
            });
        }

        [Test]
        public async Task GetPatientsCountByFilterAsync()
        {
            IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

            this.dentistRepository
                .Setup(r => r.GetAllAttached())
                .Returns(dentistsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            AllDentistsSearchViewModel inputModel = new AllDentistsSearchViewModel
            {
                SearchQuery = "Iv"
            };

            int count = await dentistService.GetDentistsCountByFilterAsync(inputModel);

            Assert.AreEqual(1, count);
        }

        [Test]
        public async Task GetUserEmailsAsyncPositive()
        {
            this.userManager
                .Setup(um => um.Users)
                .Returns(usersData.AsQueryable().BuildMock());

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            IEnumerable<UserEmailViewModel> result = await dentistService.GetUserEmailsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("meri@dentist.com", result.First().Email);
        }

        [Test]
        public async Task GetUserEmailsAsyncnegative()
        {
            this.userManager
                .Setup(um => um.Users)
                .Returns(usersData.AsQueryable().BuildMock());

            IDentistService dentistService = new DentistService(this.dentistRepository.Object,
                appointmentRepository.Object, userManager.Object);

            IEnumerable<UserEmailViewModel> result = await dentistService.GetUserEmailsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreNotEqual("desislava@dentist.com", result.First().Email);
        }

        [Test]
        public async Task GetDentistDashboardAsyncPositive()
        {
            Guid dentistId = Guid.Parse("6844f781-c5bc-4da7-bf11-349fd5bbe9c0");
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);

            List<Appointment> appointmentsData = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = Guid.NewGuid(),
                    PatientId = Guid.NewGuid(),
                    AppointmentDate = today,
                    DentistId = dentistId,
                    IsDeleted = false,
                    Patient = new Patient
                    {
                        Name = "Ivo Ivanov"
                    },
                    AppointmentProcedures = new List<AppointmentProcedure>
                    {
                        new AppointmentProcedure
                        {
                            Procedure = new Procedure
                            {
                                Name = "Cleaning",
                                Price = 100,
                                Description = "Teeth cleaning"
                            },
                            IsDeleted = false
                        }
                    }
                },
                new Appointment
                {
                    AppointmentId = Guid.NewGuid(),
                    DentistId = dentistId,
                    PatientId = Guid.NewGuid(),
                    AppointmentDate = today.AddMonths(-1),
                    Patient = new Patient
                    {
                        Name = "Ivo Ivanov"
                    },
                    AppointmentProcedures = new List<AppointmentProcedure>
                    {
                        new AppointmentProcedure
                        {
                            Procedure = new Procedure
                            {
                                Name = "Cleaning",
                                Price = 100,
                                Description = "Teeth cleaning"
                            },
                            IsDeleted = false
                        }
                    }
                }
            };
            IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

            this.appointmentRepository
                .Setup(r => r.GetAllAttached())
                .Returns(appointmentsMockQueryable);

            IDentistService dentistService = new DentistService(this.dentistRepository.Object, appointmentRepository.Object, userManager.Object);

            // Act
            DentistDashboardViewModel dashboard = await dentistService.GetDentistDashboardAsync(dentistId);

            // Assert
            Assert.IsNotNull(dashboard);
            Assert.AreEqual(1, dashboard.TodayAppointmentCount);
            Assert.AreEqual(1, dashboard.TodayAppointments.Count);
            Assert.AreEqual(1, dashboard.MonthlyPatientCount);
        }
    }
}

