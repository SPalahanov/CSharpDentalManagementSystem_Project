namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using Microsoft.AspNetCore.Mvc;

    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllAppointmentsIndexViewModel> procedures = await this.appointmentService.GetAllAppointmentsAsync();

            return View(procedures);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            Guid appointmentGuid = Guid.Empty;

            bool isIdValid = this.IsGuidValid(id, ref appointmentGuid);

            if (!isIdValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            AppointmentDetailsViewModel? viewModel = await this.appointmentService
                .GetAppointmentDetailsByIdAsync(appointmentGuid);

            if (viewModel == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(viewModel);
        }
    }
}
