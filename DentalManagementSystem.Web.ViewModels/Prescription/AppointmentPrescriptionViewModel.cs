namespace DentalManagementSystem.Web.ViewModels.Prescription
{
    using System;

    public class AppointmentPrescriptionViewModel
    {
        public Guid Id { get; set; }

        public string MedicationName { get; set; } = null!;

        public string? MedicationDescription { get; set; }
    }
}
