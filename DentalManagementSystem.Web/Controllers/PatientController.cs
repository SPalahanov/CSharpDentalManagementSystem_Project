namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using DentalManagementSystem.Web.ViewModels.Patient;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Patient;

    [Authorize]
    public class PatientController : BaseController
    {
        private readonly IPatientService patientService;
        private readonly IDentistService dentistService;

        public PatientController(IPatientService patientService, IDentistService dentistService)
        {
            this.patientService = patientService;
            this.dentistService = dentistService;
        }

        public async Task<IActionResult> Index()
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }
            
            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (await this.patientService.IsUserPatient(userId))
            {
                return this.RedirectToAction("Dashboard", "Patient");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            IEnumerable<AllPatientsIndexViewModel> viewModel = await this.patientService.GetAllPatientsAsync();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return View("Index");
            }
            Guid patientId = await patientService.GetPatientIdByUserIdAsync(Guid.Parse(userId));

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isDentist || isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
        }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            Guid patientId = await this.patientService.GetPatientIdByUserIdAsync(Guid.Parse(userId));

            IEnumerable<AppointmentDetailsViewModel> dentistDashboard = await this.patientService.GetPatientDashboardAsync(patientId);

            return this.View(dentistDashboard);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Become()
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.View("Index");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient || isDentist || isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Become(BecomePatientFormModel model)
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.View("Index");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient || isDentist || isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            bool result = await this.patientService.CreatePatientAsync(userId, model);

            if (result == false)
            {
                this.ModelState.AddModelError(nameof(model.DateOfBirth),
                    String.Format($"The Date Of Birth must be in the following format: {0}", DateOfBirthFormat));

                return this.View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            string? userId = this.User.GetUserId();

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            IEnumerable<UserEmailViewModel> usersData = await patientService.GetUserEmailsAsync();

            AddPatientInputModel model = new AddPatientInputModel
            {
                Emails = usersData
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddPatientInputModel model)
        {
            string? userId = this.User.GetUserId();

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                model.Emails = await patientService.GetUserEmailsAsync();
                return View(model);
            }

            var result = await patientService.CreatePatientFromUserAsync(model.SelectedUserId, model);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "Failed to create patient. Please try again.");

                model.Emails = await patientService.GetUserEmailsAsync();

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            Guid patientGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref patientGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction("Index", "Patient");
            }

            string? userId = this.User.GetUserId();

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            PatientDetailsViewModel? viewModel = await this.patientService
                .GetPatientDetailsByIdAsync(patientGuid);

            if (viewModel == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            Guid patientGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref patientGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction("Index", "Patient");
            }

            string? userId = this.User.GetUserId();

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            EditPatientFormModel? formModel = await this.patientService.GetPatientForEditByIdAsync(patientGuid);

            return this.View(formModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPatientFormModel formModel)
        {
            string? userId = this.User.GetUserId();

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            bool result = await this.patientService
                .EditPatientAsync(formModel);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occurred while updating the patient!");

                return this.View(formModel);
            }

            return this.RedirectToAction(nameof(Details), "Patient", new { id = formModel.Id });
        }
    }
}
