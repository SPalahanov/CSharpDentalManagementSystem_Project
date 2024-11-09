namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await this.appointmentService.GetCreateAppointmentModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                model.Patients = (await appointmentService.GetCreateAppointmentModelAsync()).Patients;
                model.Dentists = (await appointmentService.GetCreateAppointmentModelAsync()).Dentists;
                model.AppointmentTypes = (await appointmentService.GetCreateAppointmentModelAsync()).AppointmentTypes;

                return View(model);
            }

            await this.appointmentService.CreateAppointmentAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
