namespace DentalManagementSystem.Web.ViewModels.Prescription
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Prescription;

    public class CreatePrescriptionFormModel
    {
        [Required]
        [MinLength(MedicationNameMinLength)]
        [MaxLength(MedicationNameMaxLength)]
        public string MedicationName { get; set; } = null!;

        [Required]
        [MinLength(MedicationDescriptionMinLength)]
        [MaxLength(MedicationDescriptionMaxLength)]
        public string? MedicationDescription { get; set; }

        [Required]
        public Guid AppointmentId { get; set; }
    }
}
