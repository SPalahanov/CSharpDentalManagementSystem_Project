namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class ProcedureController : Controller
    {
        private readonly IProcedureService procedureService;

        public ProcedureController(IProcedureService procedureService)
        {
            this.procedureService = procedureService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}