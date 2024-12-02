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
            string? userId = User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            Guid userGuid = Guid.Parse(userId);

            IEnumerable<AllAppointmentsIndexViewModel> appointments;

            if (await patientService.IsUserPatient(userId))
            {
                Guid patientId = await patientService.GetPatientIdByUserIdAsync(userGuid);

                appointments = await appointmentService.GetAppointmentsByPatientIdAsync(patientId);

                return View(appointments);
            }

            if (await dentistService.IsUserDentist(userId))
            {
                Guid dentistId = await dentistService.GetDentistIdByUserIdAsync(userGuid);

                appointments = await appointmentService.GetAppointmentsByDentistIdAsync(dentistId);

                return View(appointments);
            }

            appointments = await this.appointmentService.GetAllAppointmentsAsync();

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateAppointmentViewModel model = await this.appointmentService.GetCreateAppointmentModelAsync();

            model.AvailableProcedures = await this.appointmentService.GetAvailableProceduresAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Patients = (await appointmentService.GetCreateAppointmentModelAsync()).Patients;
                model.Dentists = (await appointmentService.GetCreateAppointmentModelAsync()).Dentists;
                model.AppointmentTypes = (await appointmentService.GetCreateAppointmentModelAsync()).AppointmentTypes;
                model.AvailableProcedures = await this.appointmentService.GetAvailableProceduresAsync();

                return View(model);
            }

            await this.appointmentService.CreateAppointmentAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            Guid appointmentGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref appointmentGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            AppointmentDetailsViewModel? viewModel = await this.appointmentService
                .GetAppointmentDetailsByIdAsync(appointmentGuid);

            if (viewModel == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            Guid appointmentGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref appointmentGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            string? userId = User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (await patientService.IsUserPatient(userId))
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            EditAppointmentFormModel? formModel = await this.appointmentService.GetAppointmentForEditByIdAsync(appointmentGuid);

            return this.View(formModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditAppointmentFormModel model)
        {
            string? userId = User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (await patientService.IsUserPatient(userId))
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            if (!ModelState.IsValid)
            {
                model.Patients = await appointmentService.GetPatientListAsync();
                model.Dentists = await appointmentService.GetDentistListAsync();
                model.AppointmentTypes = await appointmentService.GetAppointmentTypeListAsync();
                model.AvailableProcedures = await appointmentService.GetAvailableProcedureListAsync();

                return View(model);
            }

            bool isSuccess = await this.appointmentService.EditAppointmentAsync(model);

            if (!isSuccess)
            {
                model.Patients = await this.appointmentService.GetPatientListAsync();
                model.Dentists = await this.appointmentService.GetDentistListAsync();
                model.AppointmentTypes = await this.appointmentService.GetAppointmentTypeListAsync();
                model.AvailableProcedures = await this.appointmentService.GetAvailableProceduresAsync();

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            string? userId = User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (await patientService.IsUserPatient(userId))
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            Guid appointmentGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref appointmentGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            //TODO: Implementing Delete..........

            return this.RedirectToAction("Index", "Appointment");
        }
    }
}
