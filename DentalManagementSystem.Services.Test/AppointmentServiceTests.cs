using DentalManagementSystem.Web.ViewModels.Appointment;

using MockQueryable;

namespace DentalManagementSystem.Services.Test;

using DentalManagementSystem.Common.Enums;
using DentalManagementSystem.Data.Models;
using DentalManagementSystem.Data.Repository.Interfaces;
using DentalManagementSystem.Services.Data;
using DentalManagementSystem.Services.Data.Interfaces;

using Moq;

public class AppointmentServiceTests
{
    private Mock<IRepository<Appointment, Guid>> appointmentRepository;
    private Mock<IRepository<Patient, Guid>> patientRepository;
    private Mock<IRepository<Dentist, Guid>> dentistRepository;
    private Mock<IRepository<Procedure, int>> procedureRepository;
    private Mock<IRepository<Prescription, Guid>> prescriptionRepository;
    private Mock<IRepository<AppointmentType, int>> appointmentTypeRepository;

    private IList<Appointment> appointmentsData = new List<Appointment>()
    {
        new Appointment()
        {
            AppointmentId = Guid.Parse("7463ded4-56d0-429b-8063-b73e453d0ab5"),
            AppointmentDate = DateTime.Parse("2025-10-20T15:00:00"),
            AppointmentStatus = AppointmentStatus.Scheduled,
            AppointmentTypeId = 2,
            IsDeleted = false,
            PatientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5"),
            DentistId = Guid.Parse("93e0c32c-553c-4f61-a767-31eb09e85ad5")
        },
        new Appointment()
        {
            AppointmentId = Guid.Parse("26edaf6d-cae6-42c9-8d5c-fba578baeaf7"),
            AppointmentDate = DateTime.Parse("2025-11-25T09:30:00"),
            AppointmentStatus = AppointmentStatus.Cancelled,
            AppointmentTypeId = 2,
            IsDeleted = false,
            PatientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5"),
            DentistId = Guid.Parse("05e829fb-8ba6-45d6-8afe-a6338d3ba6a8")
        },
        new Appointment()
        {
            AppointmentId = Guid.Parse("385ee459-337a-4df2-ba1b-b2f01c451b17"),
            AppointmentDate = DateTime.Parse("2023-11-25T09:30:00"),
            AppointmentStatus = AppointmentStatus.Completed,
            AppointmentTypeId = 2,
            IsDeleted = false,
            PatientId = Guid.Parse("0a116063-736f-4104-a5d3-2a516239463c"),
            DentistId = Guid.Parse("05e829fb-8ba6-45d6-8afe-a6338d3ba6a8")
        },
        new Appointment()
        {
            AppointmentId = Guid.Parse("70d6cddb-1787-4e7c-822b-b4b8929f1c82"),
            AppointmentDate = DateTime.Parse("2024-12-25T09:30:00"),
            AppointmentStatus = AppointmentStatus.Scheduled,
            AppointmentTypeId = 1,
            IsDeleted = true,
            PatientId = Guid.Parse("6844f781-c5bc-4da7-bf11-349fd5bbe9c0"),
            DentistId = Guid.Parse("05e829fb-8ba6-45d6-8afe-a6338d3ba6a8")
        },
        new Appointment()
        {
            AppointmentId = Guid.Parse("26edaf6d-cae6-42c9-8d5c-fba578baeaf7"),
            AppointmentDate = DateTime.Parse("2022-11-25T09:30:00"),
            AppointmentStatus = AppointmentStatus.Cancelled,
            AppointmentTypeId = 2,
            IsDeleted = false,
            PatientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5"),
            DentistId = Guid.Parse("385ee459-337a-4df2-ba1b-b2f01c451b17")
        },
        new Appointment()
        {
            AppointmentId = Guid.Parse("123ee459-337a-4df2-ba1b-b2f01c451b17"),
            AppointmentDate = DateTime.Parse("2023-11-25T09:30:00"),
            AppointmentStatus = AppointmentStatus.Completed,
            AppointmentTypeId = 2,
            IsDeleted = false,
            PatientId = Guid.Parse("0a116063-736f-4104-a5d3-2a516239463c"),
            DentistId = Guid.Parse("6844f781-c5bc-4da7-bf11-349fd5bbe9c0")
        },
    };

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

    private IList<AppointmentType> appointmentTypesData = new List<AppointmentType>
    {
        new AppointmentType()
        {
            Id = 1,
            Name = "Consultation"
        },
        new AppointmentType()
        {
            Id = 2,
            Name = "Examination"
        }
};

    private IList<Procedure> proceduresData = new List<Procedure>
    {
        new Procedure
        {
            ProcedureId = 1,
            Name = "Cleaning",
            IsDeleted = false
        },
        new Procedure
        {
            ProcedureId = 2,
            Name = "Filling",
            IsDeleted = false
        }
    };

    [SetUp]
    public void Setup()
    {
        this.appointmentRepository = new Mock<IRepository<Appointment, Guid>>();
        this.dentistRepository = new Mock<IRepository<Dentist, Guid>>();
        this.patientRepository = new Mock<IRepository<Patient, Guid>>();
        this.procedureRepository = new Mock<IRepository<Procedure, int>>();
        this.prescriptionRepository = new Mock<IRepository<Prescription, Guid>>();
        this.appointmentTypeRepository = new Mock<IRepository<AppointmentType, int>>();
    }

    [Test]
    public async Task GetAllAppointmentsNoFilterPositive()
    {
        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        
        IEnumerable<AllAppointmentsIndexViewModel> allAppointmentsActual = await appointmentService
            .GetAllAppointmentsAsync(new AllAppointmentsFilterViewModel());

        Assert.IsNotNull(allAppointmentsActual);
        Assert.AreEqual(this.appointmentsData.Count(), allAppointmentsActual.Count());

        allAppointmentsActual = allAppointmentsActual
            .OrderBy(p => p.Id)
            .ToList();

        int i = 0;

        foreach (AllAppointmentsIndexViewModel returnedAppointment in allAppointmentsActual)
        {
            Assert.AreEqual(this.appointmentsData.OrderBy(a => a.AppointmentId).ToList()[i++].AppointmentId.ToString(), returnedAppointment.Id);
        }
    }

    [Test]
    public async Task GetAllAppointmentsNullFilterNegative()
    {
        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        Assert.ThrowsAsync<NullReferenceException>(async () =>
        {
            IEnumerable<AllAppointmentsIndexViewModel> allAppointmentsActual = await appointmentService
                .GetAllAppointmentsAsync(null);
        });
    }

    [Test]
    public async Task GetUpcomingAppointmentsByPatientIdAsync()
    {
        Guid patientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5");

        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        IEnumerable<AllAppointmentsIndexViewModel> result =
            await appointmentService.GetAppointmentsByPatientIdAsync(patientId);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual("Scheduled", result.First().AppointmentStatus);
    }

    [Test]
    public async Task GetNoUpcomingAppointmentsByPatientIdAsync()
    {
        Guid patientId = Guid.Parse("0a116063-736f-4104-a5d3-2a516239463c");

        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        IEnumerable<AllAppointmentsIndexViewModel> result =
            await appointmentService.GetAppointmentsByPatientIdAsync(patientId);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [Test]
    public async Task GetDeletedAppointmentsByPatientIdAsync()
    {
        Guid patientId = Guid.Parse("6844f781-c5bc-4da7-bf11-349fd5bbe9c0");

        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        IEnumerable<AllAppointmentsIndexViewModel> result =
            await appointmentService.GetAppointmentsByPatientIdAsync(patientId);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [Test]
    public async Task GetUpcomingAppointmentsByDentistIdAsync()
    {
        Guid dentistId = Guid.Parse("93e0c32c-553c-4f61-a767-31eb09e85ad5");

        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        IEnumerable<AllAppointmentsIndexViewModel> result =
            await appointmentService.GetAppointmentsByDentistIdAsync(dentistId);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("Scheduled", result.First().AppointmentStatus);
    }

    [Test]
    public async Task GetNoUpcomingAppointmentsByDentistIdAsyncPositive()
    {
        Guid dentistId = Guid.Parse("385ee459-337a-4df2-ba1b-b2f01c451b17");

        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        IEnumerable<AllAppointmentsIndexViewModel> result =
            await appointmentService.GetAppointmentsByDentistIdAsync(dentistId);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
    }

    [Test]
    public async Task GetAppointmentCountByFilterAsyncPositive()
    {
        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        AllAppointmentsFilterViewModel inputModel = new AllAppointmentsFilterViewModel
        {
            YearFilter = "2022"
        };

        int count = await appointmentService.GetAppointmentsCountByFilterAsync(inputModel);

        Assert.AreEqual(1, count);
    }

    [Test]
    public async Task GetAppointmentCountByFilterRangeYearAsync()
    {
        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        AllAppointmentsFilterViewModel inputModel = new AllAppointmentsFilterViewModel
        {
            YearFilter = "2022 - 2024"
        };

        int count = await appointmentService.GetAppointmentsCountByFilterAsync(inputModel);

        Assert.AreEqual(4, count);
    }

    [Test]
    public async Task GetAppointmentDetailsByIdAsyncPositive()
    {
        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();
        
        Guid appointmentId = Guid.Parse("123ee459-337a-4df2-ba1b-b2f01c451b17");

        string expectedAppointmentDate = "25/11/2023 09:30 AM";
        int expectedAppointmentType = 2;

        Procedure expectedProceduresData = new Procedure
        {
            ProcedureId = 1,
            Name = "Cleaning",
            IsDeleted = false
        };

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        AppointmentDetailsViewModel? appointmentDetailsActual = await appointmentService
            .GetAppointmentDetailsByIdAsync(appointmentId);

        Assert.IsNotNull(appointmentDetailsActual);
        Assert.IsNotNull(appointmentDetailsActual.Procedures);
        Assert.AreEqual(expectedProceduresData.AppointmentProcedures, appointmentDetailsActual.Procedures);
    }

    [Test]
    public async Task GetAppointmentsDetailsByIdAsyncNegative()
    {
        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        Guid appointmentId = Guid.Empty;

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        Assert.ThrowsAsync<NullReferenceException>(async () =>
        {
            AppointmentDetailsViewModel? dentistDetailsActual = await appointmentService
                .GetAppointmentDetailsByIdAsync(appointmentId);

            if (dentistDetailsActual == null)
            {
                throw new NullReferenceException();
            }
        });
    }

    [Test]
    public async Task GetAppointmentForDeleteByIdAsyncPositive()
    {
        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        Guid appointmentId = Guid.Parse("385ee459-337a-4df2-ba1b-b2f01c451b17");

        string expectedAppointmentId = "385ee459-337a-4df2-ba1b-b2f01c451b17";
        DateTime expectedAppointmentDate = DateTime.Parse("2023-11-25 09:30:00");
        int expectedAppointmentType = 2;
        Guid expectedAppointmentDentistId = Guid.Parse("05e829fb-8ba6-45d6-8afe-a6338d3ba6a8");

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        DeleteAppointmentViewModel? appointmentDetailsActual = await appointmentService
            .GetAppointmentForDeleteByIdAsync(appointmentId);

        Assert.IsNotNull(appointmentDetailsActual);
        Assert.AreEqual(expectedAppointmentId.ToLower(), appointmentDetailsActual.Id.ToLower());
        Assert.AreEqual(expectedAppointmentDate, appointmentDetailsActual.AppointmentDate);
        Assert.AreEqual(expectedAppointmentType, appointmentDetailsActual.AppointmentTypeId);
        Assert.AreEqual(expectedAppointmentDentistId, appointmentDetailsActual.DentistId);
    }

    [Test]
    public async Task GetAppointmentForDeleteByIdAsyncNegative()
    {
        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        Guid appointmentId = Guid.Empty;

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        Assert.ThrowsAsync<NullReferenceException>(async () =>
        {
            DeleteAppointmentViewModel? appointmentDetailsActual = await appointmentService
                .GetAppointmentForDeleteByIdAsync(appointmentId);

            if (appointmentDetailsActual == null)
            {
                throw new NullReferenceException();
            }
        });
    }

    /*[Test]
    public async Task GetAppointmentDataForEditAsyncPositive()
    {
        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();

        Guid appointmentId = Guid.Parse("26edaf6d-cae6-42c9-8d5c-fba578baeaf7");
        string expectedAppointmentDate = "25.11.2025 г. 9:30:00";
        string expectedAppointmentStatus = "Cancelled";
        string expectedAppointmentTypeId = "2";
        Guid expectedPatientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5");
        Guid expectedDentistId = Guid.Parse("05e829fb-8ba6-45d6-8afe-a6338d3ba6a8");

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object);

        EditAppointmentFormModel? appointmentDetailsActual = await appointmentService
            .GetAppointmentDataForEditAsync(appointmentId);

        Assert.IsNotNull(appointmentDetailsActual);
        Assert.AreEqual(expectedAppointmentDate, appointmentDetailsActual.AppointmentDate.ToString());
        Assert.AreEqual(expectedAppointmentStatus, appointmentDetailsActual.AppointmentStatus.ToString());
        Assert.AreEqual(expectedAppointmentTypeId, appointmentDetailsActual.AppointmentTypeId.ToString());
    }*/

    [Test]
    public async Task CreateAppointmentAsync()
    {
        IQueryable<Appointment> appointmentsMockQueryable = appointmentsData.BuildMock();
        IQueryable<Procedure> proceduresMockQueryable = proceduresData.BuildMock();

        this.appointmentRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentsMockQueryable);

        this.procedureRepository
            .Setup(r => r.GetAllAttached())
            .Returns(proceduresMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        string appointmentId = "70d6cddb-1787-4e7c-822b-b4b8929f1c82";

        CreateAppointmentViewModel model = new CreateAppointmentViewModel
        {
            AppointmentDate = DateTime.Parse("2025-10-20T15:00:00"),
            AppointmentStatus = AppointmentStatus.Scheduled,
            AppointmentTypeId = 2,
            PatientId = Guid.Parse("aa0f15f1-5a46-4f0b-a3c1-88c048c180d5"),
            DentistId = Guid.Parse("93e0c32c-553c-4f61-a767-31eb09e85ad5"),
            SelectedProcedureIds = new List<int> {1, 2}
        };

        bool result = await appointmentService.CreateAppointmentAsync(model);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task GetDentistListAsyncPositive()
    {
        IQueryable<Dentist> dentistsMockQueryable = dentistsData.BuildMock();

        this.dentistRepository
            .Setup(r => r.GetAllAttached())
            .Returns(dentistsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        IEnumerable<DentistAppointmentViewModel> allAppointmentsDentists = await appointmentService
            .GetDentistListAsync();

        Assert.IsNotNull(allAppointmentsDentists);
    }

    [Test]
    public async Task GetPatientListAsyncPositive()
    {
        IQueryable<Patient> patientsMockQueryable = patientsData.BuildMock();

        this.patientRepository
            .Setup(r => r.GetAllAttached())
            .Returns(patientsMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        IEnumerable<PatientAppointmentViewModel> allAppointmentsPatients = await appointmentService
            .GetPatientListAsync();

        Assert.IsNotNull(allAppointmentsPatients);
    }

    [Test]
    public async Task GetAppointmentTypeListAsyncPositive()
    {
        IQueryable<AppointmentType> appointmentTypesMockQueryable = appointmentTypesData.BuildMock();

        this.appointmentTypeRepository
            .Setup(r => r.GetAllAttached())
            .Returns(appointmentTypesMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        IEnumerable<AppointmentTypeViewModel> allAppointmentTypes = await appointmentService
            .GetAppointmentTypeListAsync();

        Assert.IsNotNull(allAppointmentTypes);
    }

    [Test]
    public async Task GetAvailableProcedureAsync()
    {
        IQueryable<Procedure> proceduresMockQueryable = proceduresData.BuildMock();

        this.procedureRepository
            .Setup(r => r.GetAllAttached())
            .Returns(proceduresMockQueryable);

        IAppointmentService appointmentService = new AppointmentService(this.appointmentRepository.Object, this.patientRepository.Object, dentistRepository.Object, appointmentTypeRepository.Object, procedureRepository.Object, prescriptionRepository.Object);
        IEnumerable<ProcedureAppointmentViewModel> allAppointmentProcedures = await appointmentService
            .GetAvailableProceduresAsync();

        Assert.IsNotNull(allAppointmentProcedures);
    }
}