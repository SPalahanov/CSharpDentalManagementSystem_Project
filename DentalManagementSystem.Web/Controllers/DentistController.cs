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

        public DentistController(IDentistService dentistService)
        {
            this.dentistService = dentistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllDentistIndexViewModel> viewModel = await this.dentistService.GetAllDentistsAsync();

            return View(viewModel);
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
        public async Task<IActionResult> Become()
        {
            string? userId = this.User.GetUserId();

            bool isDentist = await dentistService.DentistExistsByUserIdAsync(userId);

            if (isDentist)
            {
                //this.TempData[ErrorMessage] = "You are already an dentist";

                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeDentistFormModel model)
        {
            string? userId = this.User.GetUserId();

            bool isDentist = await dentistService.DentistExistsByUserIdAsync(userId);

            if (isDentist)
            {
                //this.TempData[ErrorMessage] = "You are already an dentist";

                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            await this.dentistService.CreateDentistAsync(userId, model);

            return this.RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<UserEmailViewModel> usersData = await dentistService.GetUserEmailsAsync();

            AddDentistInputModel model = new AddDentistInputModel
            {
                Emails = usersData
            };

            return View(model);
        }

        [HttpPost]
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
    }
}
