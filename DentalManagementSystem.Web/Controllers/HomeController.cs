namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    public class HomeController : BaseController
    {
        private readonly IDentistService dentistService;
        public HomeController(IDentistService dentistService)
        {
            this.dentistService = dentistService;
        }

        public async Task<IActionResult> Index()
        {
            string? userId = User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return View("Index");
            }

            Guid dentistId = await dentistService.GetDentistIdByUserIdAsync(Guid.Parse(userId));

            if (dentistId != Guid.Empty)
            {
                DentistDashboardViewModel dentistDashboard = await dentistService.GetDentistDashboardAsync(dentistId);

                return View("Index", dentistDashboard);
            }

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
