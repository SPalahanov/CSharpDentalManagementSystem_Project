namespace DentalManagementSystem.Web.Controllers
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Prescription;
    using DentalManagementSystem.Web.ViewModels.Procedure;
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

        [HttpPost]
        public async Task<IActionResult> SoftDeleteConfirmed(string id)
        {
            Guid prescriptionId = Guid.NewGuid();

            if (!this.IsGuidValid(id, ref prescriptionId))
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            string? userId = this.User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            bool isDentist = await this.dentistService.DentistExistsByUserIdAsync(userId);
            bool isAdmin = this.User.IsInRole("Admin");

            if (!isDentist && !isAdmin)
            {
                return this.RedirectToAction("Index", "Appointment");
            }

            DeletePrescriptionViewModel? prescriptionToDeleteViewModel = await this.prescriptionService.GetPrescriptionForDeleteByIdAsync(prescriptionId);

            bool isDeleted = await this.prescriptionService.SoftDeletePrescriptionAsync(prescriptionId);

            if (!isDeleted)
            {
                return this.RedirectToAction("Details", "Appointment", new { id = prescriptionToDeleteViewModel?.AppointmentId });
            }

            return this.RedirectToAction("Details", "Appointment", new { id = prescriptionToDeleteViewModel?.AppointmentId });
        }
    }
}
