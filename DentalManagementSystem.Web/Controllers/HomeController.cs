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
            string? userId = User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return View("Index");
            }

            Guid dentistId = await dentistService.GetDentistIdByUserIdAsync(Guid.Parse(userId));

            if (dentistId != Guid.Empty)
            {
                DentistDashboardViewModel dentistDashboard = await dentistService.GetDentistDashboardAsync(dentistId);

                return RedirectToAction("Dashboard", "Dentist");
            }

            Guid patientId = await patientService.GetPatientIdByUserIdAsync(Guid.Parse(userId));

            if (patientId != Guid.Empty)
            {
                IEnumerable<AppointmentDetailsViewModel> patientDashboard = await patientService.GetPatientDashboardAsync(patientId);

                return RedirectToAction("Dashboard", "Patient");
            }

            if (User.IsInRole("Admin"))
            {
                return Redirect("/Admin");
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
