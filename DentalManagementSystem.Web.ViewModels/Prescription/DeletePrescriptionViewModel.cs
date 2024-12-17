namespace DentalManagementSystem.Web.ViewModels.Prescription
{
    using System;

    public class DeletePrescriptionViewModel
    {
        public string PrescriptionId { get; set; } = null!;

        public string MedicationName { get; set; } = null!;

        public string? MedicationDescription { get; set; }

        public bool IsDeleted { get; set; }

        public Guid AppointmentId { get; set; }
    }
}
