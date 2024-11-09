namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using DentalManagementSystem.Web.ViewModels.Patient;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class DentistController : Controller
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
        public async Task<IActionResult> Details()
        {
            IEnumerable<AllDentistIndexViewModel> viewModel = await this.dentistService.GetAllDentistsAsync();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Become()
        {
            string? userId = this.User.GetId();

            bool isDentist = await dentistService.DentistExistsByUserIdAsync(userId);

            if (!isDentist)
            {
                //this.TempData[ErrorMessage] = "You are already an agent";

                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
