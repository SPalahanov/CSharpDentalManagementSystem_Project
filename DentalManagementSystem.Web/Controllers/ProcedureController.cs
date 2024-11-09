namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ProcedureController : Controller
    {
        private readonly IProcedureService procedureService;

        public ProcedureController(IProcedureService procedureService)
        {
            this.procedureService = procedureService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProcedureIndexViewModel> procedures =
                await this.procedureService.IndexGetAllAsync();

            return View(procedures);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProcedureFormModel model)
        {
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

            ProcedureDetailsViewModel? viewModel = await this.procedureService
                .GetProcedureDetailsByIdAsync(id);

            if (viewModel == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

    }
}