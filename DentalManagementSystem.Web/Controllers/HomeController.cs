namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using DentalManagementSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    public class HomeController : BaseController
    {
        private readonly IDentistService dentistService;
        private readonly IPatientService patientService;

        public HomeController(IDentistService dentistService, IPatientService patientService)
        {
            this.dentistService = dentistService;
            this.patientService = patientService;
        }

        public async Task<IActionResult> Index()
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.View("Index");
            }

            Guid dentistId = await this.dentistService.GetDentistIdByUserIdAsync(Guid.Parse(userId));

            if (dentistId != Guid.Empty)
            {
                DentistDashboardViewModel dentistDashboard = await this.dentistService.GetDentistDashboardAsync(dentistId);

                return this.RedirectToAction("Dashboard", "Dentist");
            }

            Guid patientId = await this.patientService.GetPatientIdByUserIdAsync(Guid.Parse(userId));

            if (patientId != Guid.Empty)
            {
                IEnumerable<AppointmentDetailsViewModel> patientDashboard = await this.patientService.GetPatientDashboardAsync(patientId);

                return this.RedirectToAction("Dashboard", "Patient");
            }

            if (this.User.IsInRole("Admin"))
            {
                return this.Redirect("/Admin");
            }

            return this.View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
