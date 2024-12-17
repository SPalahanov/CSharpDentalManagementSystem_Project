namespace DentalManagementSystem.Services.Test
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Services.Data;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using DentalManagementSystem.Web.ViewModels.Patient;

    using Microsoft.AspNetCore.Identity;

    using MockQueryable;

    using Moq;

    using NUnit.Framework;

    public class PatientServiceTests
    {
        private IList<Patient> patientsData = new List<Patient>()
        {
            new Patient()
            {
                PatientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5"),
                Name = "Hristo Dimitrov",
                PhoneNumber = "+359876543215",
                Address = "ul. Vasil Levski 32",
                DateOfBirth = DateTime.Parse("1985-05-15"),
                Gender = "Male",
                Allergies = "",
                InsuranceNumber = "BG-12349",
                EmergencyContact = "+359876543216",
                IsDeleted = false,
                UserId = Guid.Parse("9bdb04a4-4416-4b4e-876d-4d82e29f7199")
            },
            new Patient()
            {
                PatientId = Guid.Parse("fb2dd458-1ff6-4d55-92c8-aebc77bbf6f2"),
                Name = "Sofiya Ivanova",
                PhoneNumber = "+359876543217",
                Address = "bul. Bulgaria 10",
                DateOfBirth = DateTime.Parse("1992-08-12"),
                Gender = "Female",
                Allergies = "",
                InsuranceNumber = "BG-12350",
                EmergencyContact = "+359876543218",
                IsDeleted = false,
                UserId = Guid.Parse("eb3d7f22-efbb-4f81-be5e-e220ac5f36f5")
            }
        };

        private IList<ApplicationUser> usersData = new List<ApplicationUser>
        {
            new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Email = "borislava@patient.com"
            },
            new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Email = "georgi@dentist.com"
            }
        };

        private Mock<IRepository<Patient, Guid>> patientRepository;
        private Mock<IRepository<Appointment, Guid>> appointmentRepository;
        private Mock<UserManager<ApplicationUser>> userManager;

        [SetUp]
        public void Setup()
        {
            this.patientRepository = new Mock<IRepository<Patient, Guid>>();
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
                null  // ILogger<UserManager<ApplicationUser>>
            );
        }

        [Test]
        public async Task GetAllPatientsNoFilterPositive()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            IEnumerable<AllPatientsIndexViewModel> allPatientsActual = await patientService
                .GetAllPatientsAsync(new AllPatientsSearchViewModel());

            Assert.IsNotNull(allPatientsActual);
            Assert.AreEqual(this.patientsData.Count(), allPatientsActual.Count());

            allPatientsActual = allPatientsActual
                .OrderBy(p => p.Id)
                .ToList();

            int i = 0;

            foreach (AllPatientsIndexViewModel returnedPatient in allPatientsActual)
            {
                Assert.AreEqual(this.patientsData.OrderBy(p => p.PatientId).ToList()[i++].PatientId.ToString(), returnedPatient.Id);
            }
        }

        [Test]
        [TestCase("So")]
        [TestCase("so")]
        public async Task GetAllPatientsSearchQueryPositive(string searchQuery)
        {
            int expectedPatientsCount = 1;

            string expectedPatientId = "fb2dd458-1ff6-4d55-92c8-aebc77bbf6f2";

            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            IEnumerable<AllPatientsIndexViewModel> allPatientsActual = await patientService
                .GetAllPatientsAsync(new AllPatientsSearchViewModel()
                {
                    SearchQuery = searchQuery
                });

            Assert.IsNotNull(allPatientsActual);
            Assert.AreEqual(expectedPatientsCount, allPatientsActual.Count());
            Assert.AreEqual(expectedPatientId.ToLower(), allPatientsActual.First().Id.ToLower());
        }

        [Test]
        public async Task GetAllPatientsNullFilterNegative()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                IEnumerable<AllPatientsIndexViewModel> allMoviesActual = await patientService
                    .GetAllPatientsAsync(null);
            });
        }

        [Test]
        public async Task GetPatientIdByUserIdAsyncPositive()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            Guid userId = Guid.Parse("9bdb04a4-4416-4b4e-876d-4d82e29f7199");
            Guid expectedPatientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5");

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            Guid patientIdActual = await patientService.GetPatientIdByUserIdAsync(userId);

            Assert.IsNotNull(patientIdActual);
            Assert.AreEqual(expectedPatientId, patientsMockQueryable.First().PatientId);
        }

        [Test]
        public async Task GetPatientIdByUserIdAsyncNegative()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            Guid userId = Guid.NewGuid();
            
            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            Guid actualPatientId = await patientService.GetPatientIdByUserIdAsync(userId);

            Assert.AreEqual(Guid.Empty, actualPatientId);
        }

        [Test]
        public async Task GetPatientIdWhenIsDeletedByUserIdAsyncNegative()
        {
            IList<Patient> deletedPatient = new List<Patient>()
            {
                new Patient()
                {
                    PatientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5"),
                    UserId = Guid.Parse("9bdb04a4-4416-4b4e-876d-4d82e29f7199"),
                    IsDeleted = true
                },
            };

            IQueryable<Patient> patientsMockQueryable = deletedPatient.BuildMock();

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            PatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            Guid actualPatientId = await patientService.GetPatientIdByUserIdAsync(Guid.Parse("70d6cddb-1787-4e7c-822b-b4b8929f1c82"));

            Assert.AreEqual(Guid.Empty, actualPatientId);
        }

        [Test]
        public async Task CreatePatientFromUserAsyncPositive()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            string userId = "9bdb04a4-4416-4b4e-876d-4d82e29f7199";

            AddPatientInputModel model = new AddPatientInputModel
            {
                Name = "Hristo Dimitrov",
                PhoneNumber = "+359876543215",
                Address = "ul. Vasil Levski 32",
                DateOfBirth = "1985-05-15",
                Gender = "Male",
                Allergies = "",
                InsuranceNumber = "BG-12349",
                EmergencyContact = "+359876543216",
                SelectedUserId = userId
            };

            await patientService.CreatePatientFromUserAsync(userId, model);

            Patient createdPatient = patientsData.First();

            Assert.AreEqual(Guid.Parse(userId), createdPatient.UserId);
            Assert.AreEqual("Hristo Dimitrov", createdPatient.Name);
            Assert.AreEqual("+359876543215", createdPatient.PhoneNumber);
            Assert.AreEqual("ul. Vasil Levski 32", createdPatient.Address);
            Assert.AreEqual(new DateTime(1985, 5, 15), createdPatient.DateOfBirth);
            Assert.AreEqual("Male", createdPatient.Gender);
        }

        [Test]
        public async Task CreatePatientFromInvalidUserAsyncNegative()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            string userId = "9bdb04a4-4416-4b4e-876d-4d82e29f7199";

            AddPatientInputModel model = new AddPatientInputModel
            {
                Name = "Hristo Dimitrov",
                PhoneNumber = "+359876543215",
                Address = "ul. Vasil Levski 32",
                DateOfBirth = "1985-05-15",
                Gender = "Male",
                SelectedUserId = "invalid-guid"
            };

            bool result = await patientService.CreatePatientFromUserAsync(userId, model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreatePatientFromUserAlreadyPatientAsyncNegative()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            string userId = "9bdb04a4-4416-4b4e-876d-4d82e29f7199";

            AddPatientInputModel model = new AddPatientInputModel
            {
                Name = "Hristo Dimitrov",
                PhoneNumber = "+359876543215",
                Address = "ul. Vasil Levski 32",
                DateOfBirth = "1985-05-15",
                Gender = "Male",
                SelectedUserId = "9bdb04a4-4416-4b4e-876d-4d82e29f7199"
            };

            bool result = await patientService.CreatePatientFromUserAsync(userId, model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreatePatientFromUserWithInvalidDateOfBirthAsyncNegative()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            string userId = "fb2dd458-1ff6-4d55-92c8-aebc77bbf6f2";

            AddPatientInputModel model = new AddPatientInputModel
            {
                Name = "Hristo Dimitrov",
                PhoneNumber = "+359876543215",
                Address = "ul. Vasil Levski 32",
                DateOfBirth = "invalid-date",
                Gender = "Male",
                SelectedUserId = "fb2dd458-1ff6-4d55-92c8-aebc77bbf6f2"
            };

            bool result = await patientService.CreatePatientFromUserAsync(userId, model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsUserPatientPositive()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            Guid userId = Guid.Parse("eb3d7f22-efbb-4f81-be5e-e220ac5f36f5");

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            bool result = await patientService.IsUserPatient(userId.ToString());

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsUserPatientNegative()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            Guid userId = Guid.Parse("fb2dd458-1ff6-4d55-92c8-aebc77bbf6f2");
            
            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            bool result = await patientService.IsUserPatient(userId.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetPatientDetailsByIdAsyncPositive()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            Guid patientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5");
            string expectedPatientName = "Hristo Dimitrov";
            string expectedPatientDateOfBirth = "15/05/1985";
            string expectedPatientInsuranceNumber = "BG-12349";
            string expectedPatientEmergencyContact = "+359876543216";

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            PatientDetailsViewModel? patientDetailsActual = await patientService
                .GetPatientDetailsByIdAsync(patientId);

            Assert.IsNotNull(patientDetailsActual);
            Assert.AreEqual(expectedPatientName, patientDetailsActual.Name);
            Assert.AreEqual(expectedPatientDateOfBirth, patientDetailsActual.DateOfBirth);
            Assert.AreEqual(expectedPatientInsuranceNumber, patientDetailsActual.InsuranceNumber);
            Assert.AreEqual(expectedPatientEmergencyContact, patientDetailsActual.EmergencyContact);
        }

        [Test]
        public async Task GetPatientDetailsByIdAsyncNegative()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            Guid patientId = Guid.Empty;

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                PatientDetailsViewModel? patientDetailsActual = await patientService
                    .GetPatientDetailsByIdAsync(patientId);

                if (patientDetailsActual == null)
                {
                    throw new NullReferenceException();
                }
            });
        }

        [Test]
        public async Task GetPatientForEditByIdAsyncPositive()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            Guid patientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5");
            string expectedPatientName = "Hristo Dimitrov";
            string expectedPatientDateOfBirth = "15/05/1985";
            string expectedPatientGenre = "Male";
            string expectedPatientAddress = "ul. Vasil Levski 32";
            string expectedPatientPhoneNumber = "+359876543215";

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            EditPatientFormModel? patientDetailsActual = await patientService
                .GetPatientForEditByIdAsync(patientId);

            Assert.IsNotNull(patientDetailsActual);
            Assert.AreEqual(expectedPatientName, patientDetailsActual.Name);
            Assert.AreEqual(expectedPatientDateOfBirth, patientDetailsActual.DateOfBirth);
            Assert.AreEqual(expectedPatientGenre, patientDetailsActual.Gender);
            Assert.AreEqual(expectedPatientAddress, patientDetailsActual.Address);
            Assert.AreEqual(expectedPatientPhoneNumber, patientDetailsActual.PhoneNumber);
        }

        [Test]
        public async Task GetPatientForEditByIdAsyncNegative()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            Guid patientId = Guid.Empty;
            
            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                EditPatientFormModel? patientDetailsActual = await patientService
                    .GetPatientForEditByIdAsync(patientId);

                if (patientDetailsActual == null)
                {
                    throw new NullReferenceException();
                }
            });
        }

        [Test]
        public async Task EditPatientAsyncPositive()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            this.patientRepository
                .Setup(r => r.GetByIdAsync())
                .ReturnsAsync((Guid id) => patientsData.FirstOrDefault(p => p.PatientId == id));

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            EditPatientFormModel model = new EditPatientFormModel
            {
                Id = "aa0f15f1-5a46-4f0b-a3c1-88c048c180d5",
                Name = "Hristo Dimitrov",
                PhoneNumber = "+359876543215",
                Address = "ul. Vasil Levski 32",
                DateOfBirth = "1985-05-15",
                Gender = "Male",
                Allergies = "",
                InsuranceNumber = "BG-12349",
                EmergencyContact = "+359876543216"
            };

            bool result = await patientService.EditPatientAsync(model);

            Assert.IsNotNull(result);

            Patient updatedPatient = patientsData.First();

            Assert.AreEqual("Hristo Dimitrov", updatedPatient.Name);
            Assert.AreEqual("+359876543215", updatedPatient.PhoneNumber);
            Assert.AreEqual("ul. Vasil Levski 32", updatedPatient.Address);
            Assert.AreEqual(new DateTime(1985, 5, 15), updatedPatient.DateOfBirth);
            Assert.AreEqual("Male", updatedPatient.Gender);
        }

        [Test]
        public async Task EditPatientNotExistAsyncNegative()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            this.patientRepository
                .Setup(r => r.GetByIdAsync())
                .ReturnsAsync((Guid id) => patientsData.FirstOrDefault(p => p.PatientId == id));

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            EditPatientFormModel model = new EditPatientFormModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Hristo Dimitrov",
                PhoneNumber = "+359876543215",
                Address = "ul. Vasil Levski 32",
                DateOfBirth = "1985-05-15",
                Gender = "Male",
                Allergies = "",
                InsuranceNumber = "BG-12349",
                EmergencyContact = "+359876543216"
            };

            bool result = await patientService.EditPatientAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetPatientForDeleteByIdAsyncPositive()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            Guid patientId = Guid.Parse("fb2dd458-1ff6-4d55-92c8-aebc77bbf6f2");

            string expectedPatientId = "fb2dd458-1ff6-4d55-92c8-aebc77bbf6f2";
            string expectedPatientGenre = "Female";
            string expectedPatientAddress = "bul. Bulgaria 10";
            string expectedPatientPhoneNumber = "+359876543217";

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            DeletePatientViewModel? patientDetailsActual = await patientService
                .GetPatientForDeleteByIdAsync(patientId);

            Assert.IsNotNull(patientDetailsActual);
            Assert.AreEqual(expectedPatientId.ToLower(), patientDetailsActual.Id.ToLower());
            Assert.AreEqual(expectedPatientGenre, patientDetailsActual.Gender);
            Assert.AreEqual(expectedPatientAddress, patientDetailsActual.Address);
            Assert.AreEqual(expectedPatientPhoneNumber, patientDetailsActual.PhoneNumber);
        }

        [Test]
        public async Task GetPatientForDeleteByIdAsyncNegative()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            Guid patientId = Guid.Empty;

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                DeletePatientViewModel? patientDetailsActual = await patientService
                    .GetPatientForDeleteByIdAsync(patientId);

                if (patientDetailsActual == null)
                {
                    throw new NullReferenceException();
                } 
            });
        }

        [Test]
        public async Task GetPatientsCountByFilterAsync()
        {
            IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

            this.patientRepository
                .Setup(r => r.GetAllAttached())
                .Returns(patientsMockQueryable);

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            AllPatientsSearchViewModel inputModel = new AllPatientsSearchViewModel
            {
                SearchQuery = "Sof"
            };

            int count = await patientService.GetPatientsCountByFilterAsync(inputModel);

            Assert.AreEqual(1, count);
        }

        [Test]
        public async Task GetUserEmailsAsyncPositive()
        {
            this.userManager
                .Setup(um => um.Users)
                .Returns(usersData.AsQueryable().BuildMock());

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            IEnumerable<UserEmailViewModel> result = await patientService.GetUserEmailsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("borislava@patient.com", result.First().Email);
        }

        [Test]
        public async Task GetPatientDashboardAsyncPositive()
        {
            Guid patientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5");
            DateTime today = DateTime.Today;

            List<Appointment> appointmentsData = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = Guid.NewGuid(),
                    PatientId = patientId,
                    AppointmentDate = today.AddDays(-1),
                    Dentist = new Dentist()
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

            this.appointmentRepository
                .Setup(r => r.GetAllAttached())
                .Returns(appointmentsData.BuildMock());

            IPatientService patientService = new PatientService(this.patientRepository.Object, appointmentRepository.Object, userManager.Object);

            IEnumerable<AppointmentDetailsViewModel> result = await patientService.GetPatientDashboardAsync(patientId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Ivo Ivanov", result.First().DentistName);
        }
    }
}