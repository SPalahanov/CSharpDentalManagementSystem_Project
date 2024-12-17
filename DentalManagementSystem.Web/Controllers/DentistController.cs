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
        public async Task<IActionResult> Index(AllDentistsSearchViewModel inputModel)
        {
            IEnumerable<AllDentistIndexViewModel> dentists = await this.dentistService.GetAllDentistsAsync(inputModel);

            int totalDentistsCount = await this.dentistService.GetDentistsCountByFilterAsync(inputModel);

            AllDentistsSearchViewModel viewModel = new AllDentistsSearchViewModel
            {
                Dentists = dentists,
                SearchQuery = inputModel.SearchQuery,
                CurrentPage = inputModel.CurrentPage ?? 1,
                EntitiesPerPage = inputModel.EntitiesPerPage ?? 10,
                TotalPages = (int)Math.Ceiling((double)totalDentistsCount / (inputModel.EntitiesPerPage ?? 10))
            };

            return this.View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            string? userId = this.User.GetUserId();

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

            Guid dentistId = await this.dentistService.GetDentistIdByUserIdAsync(Guid.Parse(userId));

            DentistDashboardViewModel dentistDashboard = await this.dentistService.GetDentistDashboardAsync(dentistId);

            return this.View(dentistDashboard);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(string? id)
        {
            Guid dentistGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref dentistGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction("Index", "Dentist");
            }

            DentistDetailsViewModel? viewModel = await this.dentistService.GetDentistDetailsByIdAsync(dentistGuid);

            if (viewModel == null)
            {
                return this.RedirectToAction("Index", "Dentist");
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

            return this.View();
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
                return this.View(model);
            }

            await this.dentistService.CreateDentistAsync(userId, model);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
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

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AddDentistInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Emails = await this.dentistService.GetUserEmailsAsync();

                return this.View(model);
            }

            var result = await this.dentistService.CreateDentistFromUserAsync(model.SelectedUserId, model);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "Failed to create patient. Please try again.");

                model.Emails = await this.dentistService.GetUserEmailsAsync();

                return this.View(model);
            }

            return this.RedirectToAction("Index", "Dentist");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            Guid dentistGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref dentistGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction("Index", "Dentist");
            }

            var valid = await this.dentistService.GetDentistDetailsByIdAsync(dentistGuid);
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

            EditDentistFormModel? formModel = await this.dentistService.GetDentistForEditByIdAsync(dentistGuid);

            if (formModel == null)
            {
                return this.RedirectToAction("Index", "Dentist");
            }

            return this.View(formModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditDentistFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            bool result = await this.dentistService.EditDentistAsync(formModel);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "Unexpected error occurred while updating the dentist!");

                return this.View(formModel);
            }

            return this.RedirectToAction(nameof(Details), "Dentist", new { id = formModel.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            Guid dentistGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref dentistGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction("Index", "Dentist");
            }

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

            if (await this.dentistService.GetDentistForDeleteByIdAsync(dentistGuid) == null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            DeleteDentistViewModel? dentistToDeleteViewModel = await this.dentistService.GetDentistForDeleteByIdAsync(dentistGuid);

            return this.View(dentistToDeleteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SoftDeleteConfirmed(DeleteDentistViewModel dentist)
        {
            Guid dentistGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(dentist.Id, ref dentistGuid);

            if (!this.IsGuidValid(dentist.Id, ref dentistGuid))
            {
                return this.RedirectToAction("Index", "Dentist");
            }

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

            bool isDeleted = await this.dentistService.SoftDeleteDentistAsync(dentistGuid);

            if (!isDeleted)
            {
                TempData["ErrorMessage"] = "Unexpected error occurred while trying to delete the dentist! Please contact system administrator!";

                return this.RedirectToAction(nameof(Delete), new { id = dentist.Id });
            }

            return this.RedirectToAction("Index", "Dentist");
        }
    }
}
