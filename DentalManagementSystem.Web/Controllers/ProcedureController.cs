namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Procedure;
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
        public async Task<IActionResult> Details(string? id)
        {
            /*Guid cinemaGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref cinemaGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }*/

            ProcedureDetailsViewModel? viewModel = await this.procedureService
                .GetProcedureDetailsByIdAsync(int.Parse(id));

            if (viewModel == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(viewModel);
        }
    }
}