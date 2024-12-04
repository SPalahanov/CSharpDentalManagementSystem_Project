namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class DentistController : BaseController
    {
        private readonly IDentistService dentistService;
        private readonly IPatientService patientService;

        public DentistController(IDentistService dentistService, IPatientService patientService)
        {
            this.dentistService = dentistService;
            this.patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllDentistIndexViewModel> viewModel = await this.dentistService.GetAllDentistsAsync();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            string? userId = User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.View("Index");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient || isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            Guid dentistId = await dentistService.GetDentistIdByUserIdAsync(Guid.Parse(userId));

            DentistDashboardViewModel dentistDashboard = await dentistService.GetDentistDashboardAsync(dentistId);

            return View(dentistDashboard);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(string? id)
        {
            Guid dentistGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref dentistGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            DentistDetailsViewModel? viewModel = await this.dentistService
                .GetDentistDetailsByIdAsync(dentistGuid);

            if (viewModel == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(viewModel);
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
        public async Task<IActionResult> Become(BecomeDentistFormModel model)
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
                return View(model);
            }

            await this.dentistService.CreateDentistAsync(userId, model);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.View("Index");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient || isDentist)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            IEnumerable<UserEmailViewModel> usersData = await this.dentistService.GetUserEmailsAsync();

            AddDentistInputModel model = new AddDentistInputModel
            {
                Emails = usersData
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AddDentistInputModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Emails = await dentistService.GetUserEmailsAsync();
                return View(model);
            }

            var result = await dentistService.CreateDentistFromUserAsync(model.SelectedUserId, model);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "Failed to create patient. Please try again.");

                model.Emails = await dentistService.GetUserEmailsAsync();

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            Guid dentistGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref dentistGuid);

            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.View("Index");
            }

            bool isPatient = await this.patientService.PatientExistsByUserIdAsync(userId);
            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (isPatient || isDentist)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isPatient && !isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            EditDentistFormModel? formModel = await this.dentistService.GetDentistForEditByIdAsync(dentistGuid);

            return this.View(formModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditDentistFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(formModel);
            }

            bool result = await this.dentistService
                .EditDentistAsync(formModel);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occurred while updating the dentist!");

                return this.View(formModel);
            }

            return this.RedirectToAction(nameof(Details), "Dentist", new { id = formModel.Id });
        }

    }
}
