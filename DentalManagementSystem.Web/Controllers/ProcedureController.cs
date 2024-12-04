namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ProcedureController : Controller
    {
        private readonly IProcedureService procedureService;
        private readonly IPatientService patientService;
        private readonly IDentistService dentistService;

        public ProcedureController(IProcedureService procedureService, IPatientService patientService, IDentistService dentistService)
        {
            this.procedureService = procedureService;
            this.patientService = patientService;
            this.dentistService = dentistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProcedureIndexViewModel> procedures = await this.procedureService.IndexGetAllAsync();

            return this.View(procedures);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            string? userId = this.User.GetUserId();

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

            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AddProcedureFormModel model)
        {
            string? userId = this.User.GetUserId();

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

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.procedureService.AddProcedureAsync(model);

            return this.RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return this.RedirectToAction(nameof(Index));
            }

            ProcedureDetailsViewModel? viewModel = await this.procedureService.GetProcedureDetailsByIdAsync(id);

            if (viewModel == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(viewModel);
        }
    }
}