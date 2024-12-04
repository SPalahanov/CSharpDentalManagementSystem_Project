namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService appointmentService;
        private readonly IPatientService patientService;
        private readonly IDentistService dentistService;

        public AppointmentController(IAppointmentService appointmentService, IPatientService patientService, IDentistService dentistService)
        {
            this.appointmentService = appointmentService;
            this.patientService = patientService;
            this.dentistService = dentistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            Guid userGuid = Guid.Parse(userId);

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            IEnumerable<AllAppointmentsIndexViewModel> appointments;

            if (isPatient)
            {
                Guid patientId = await this.patientService.GetPatientIdByUserIdAsync(userGuid);

                appointments = await this.appointmentService.GetAppointmentsByPatientIdAsync(patientId);

                return this.View(appointments);
            }

            if (isDentist)
            {
                Guid dentistId = await dentistService.GetDentistIdByUserIdAsync(userGuid);

                appointments = await appointmentService.GetAppointmentsByDentistIdAsync(dentistId);

                return this.View(appointments);
            }

            if(!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            appointments = await this.appointmentService.GetAllAppointmentsAsync();

            return this.View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            CreateAppointmentViewModel model = await this.appointmentService.GetCreateAppointmentModelAsync();

            model.AvailableProcedures = await this.appointmentService.GetAvailableProceduresAsync();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentViewModel model)
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                model.Patients = (await this.appointmentService.GetCreateAppointmentModelAsync()).Patients;
                model.Dentists = (await this.appointmentService.GetCreateAppointmentModelAsync()).Dentists;
                model.AppointmentTypes = (await this.appointmentService.GetCreateAppointmentModelAsync()).AppointmentTypes;
                model.AvailableProcedures = await this.appointmentService.GetAvailableProceduresAsync();

                return this.View(model);
            }

            await this.appointmentService.CreateAppointmentAsync(model);

            return this.RedirectToAction("Index", "Appointment");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            Guid appointmentGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref appointmentGuid);

            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isIdValid)
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            AppointmentDetailsViewModel? viewModel = await this.appointmentService.GetAppointmentDetailsByIdAsync(appointmentGuid);

            if (viewModel == null)
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            Guid appointmentGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref appointmentGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient)
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            EditAppointmentFormModel? formModel = await this.appointmentService.GetAppointmentForEditByIdAsync(appointmentGuid);

            return this.View(formModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAppointmentFormModel model)
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient)
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                model.Patients = await this.appointmentService.GetPatientListAsync();
                model.Dentists = await this.appointmentService.GetDentistListAsync();
                model.AppointmentTypes = await this.appointmentService.GetAppointmentTypeListAsync();
                model.AvailableProcedures = await this.appointmentService.GetAvailableProcedureListAsync();

                return this.View(model);
            }

            bool isSuccess = await this.appointmentService.EditAppointmentAsync(model);

            if (!isSuccess)
            {
                model.Patients = await this.appointmentService.GetPatientListAsync();
                model.Dentists = await this.appointmentService.GetDentistListAsync();
                model.AppointmentTypes = await this.appointmentService.GetAppointmentTypeListAsync();
                model.AvailableProcedures = await this.appointmentService.GetAvailableProceduresAsync();

                return this.View(model);
            }

            return this.RedirectToAction("Index", "Appointment");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            Guid appointmentGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref appointmentGuid);

            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient)
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isIdValid)
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            //TODO: Implementing Delete..........

            return this.RedirectToAction("Index", "Appointment");
        }
    }
}
