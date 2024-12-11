namespace DentalManagementSystem.Web.Controllers
{
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
        public async Task<IActionResult> Index(AllAppointmentsFilterViewModel inputModel)
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
            int totalAppointmentsCount;

            if (isPatient)
            {
                Guid patientId = await this.patientService.GetPatientIdByUserIdAsync(userGuid);

                appointments = await this.appointmentService.GetAppointmentsByPatientIdAsync(patientId);

                totalAppointmentsCount = appointments.Count();

                AllAppointmentsFilterViewModel viewModel = new AllAppointmentsFilterViewModel
                {
                    Appointments = appointments,
                    CurrentPage = inputModel.CurrentPage ?? 1,
                    EntitiesPerPage = inputModel.EntitiesPerPage ?? 10,
                    TotalPages = (int)Math.Ceiling((double)totalAppointmentsCount / (inputModel.EntitiesPerPage ?? 10)),
                };

                return this.View(viewModel);
            }

            if (isDentist)
            {
                Guid dentistId = await dentistService.GetDentistIdByUserIdAsync(userGuid);

                appointments = await appointmentService.GetAppointmentsByDentistIdAsync(dentistId);

                totalAppointmentsCount = appointments.Count();

                AllAppointmentsFilterViewModel viewModel = new AllAppointmentsFilterViewModel
                {
                    Appointments = appointments,
                    CurrentPage = inputModel.CurrentPage ?? 1,
                    EntitiesPerPage = inputModel.EntitiesPerPage ?? 10,
                    TotalPages = (int)Math.Ceiling((double)totalAppointmentsCount / (inputModel.EntitiesPerPage ?? 10)),
                };

                return this.View(viewModel);
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            appointments = await this.appointmentService.GetAllAppointmentsAsync(inputModel);

            totalAppointmentsCount = await this.appointmentService.GetAppointmentsCountByFilterAsync(inputModel);

            AllAppointmentsFilterViewModel adminViewModel = new AllAppointmentsFilterViewModel
            {
                Appointments = appointments,
                CurrentPage = inputModel.CurrentPage ?? 1,
                EntitiesPerPage = inputModel.EntitiesPerPage ?? 10,
                TotalPages = (int)Math.Ceiling((double)totalAppointmentsCount / (inputModel.EntitiesPerPage ?? 10)),
                YearFilter = inputModel.YearFilter,
            };

            return this.View(adminViewModel);
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
                model.AvailableProcedures = await this.appointmentService.GetAvailableProceduresAsync();

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

            DeleteAppointmentViewModel? appointmentToDeleteViewModel = await this.appointmentService.GetAppointmentForDeleteByIdAsync(appointmentGuid);

            return this.View(appointmentToDeleteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SoftDeleteConfirmed(DeleteAppointmentViewModel appointment)
        {
            Guid appointmentGuid = Guid.Empty;

            if (!this.IsGuidValid(appointment.Id, ref appointmentGuid))
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

            bool isDeleted = await this.appointmentService.SoftDeleteAppointmentAsync(appointmentGuid);

            if (!isDeleted)
            {
                TempData["ErrorMessage"] = "Unexpected error occurred while trying to delete the appointment! Please contact system administrator!";
                return this.RedirectToAction(nameof(Delete), new { id = appointment.Id });
            }

            return this.RedirectToAction("Index", "Appointment");
        }
    }
}
