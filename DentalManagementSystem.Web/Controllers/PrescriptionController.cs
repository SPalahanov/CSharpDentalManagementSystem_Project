namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Appointment;
    using DentalManagementSystem.Web.ViewModels.Dentist;
    using DentalManagementSystem.Web.ViewModels.Prescription;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PrescriptionController : BaseController
    {
        private readonly IPrescriptionService prescriptionService;
        private readonly IDentistService dentistService;

        public PrescriptionController(IPrescriptionService prescriptionService, IDentistService dentistService)
        {
            this.prescriptionService = prescriptionService;
            this.dentistService = dentistService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid appointmentId)
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (!isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            CreatePrescriptionFormModel model = new CreatePrescriptionFormModel
            {
                AppointmentId = appointmentId
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePrescriptionFormModel model)
        {
            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Home");
            }

            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (!isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            Prescription prescription = new Prescription
            {
                PrescriptionId = Guid.NewGuid(),
                MedicationName = model.MedicationName,
                MedicationDescription = model.MedicationDescription,
                AppointmentId = model.AppointmentId,
                IsDeleted = false
            };

            await this.prescriptionService.AddPrescriptionAsync(model);

            return this.RedirectToAction("Details", "Appointment", new { id = model.AppointmentId });
        }
    }
}
