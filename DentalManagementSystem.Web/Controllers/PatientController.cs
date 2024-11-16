namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Patient;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Patient;

    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientService patientService;

        public PatientController(IPatientService patientService)
        {
            this.patientService = patientService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<AllPatientsIndexViewModel> viewModel = await this.patientService.GetAllPatientsAsync();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Become()
        {
            string? userId = this.User.GetUserId();

            bool isPatient = await patientService.PatientExistsByUserIdAsync(userId);

            if (isPatient)
            {
                //this.TempData[ErrorMessage] = "You are already an dentist";

                return this.RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Become(BecomePatientFormModel model)
        {
            string? userId = this.User.GetUserId();

            bool isPatient = await patientService.PatientExistsByUserIdAsync(userId);

            if (isPatient)
            {
                //this.TempData[ErrorMessage] = "You are already an patient";

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
    }
}
