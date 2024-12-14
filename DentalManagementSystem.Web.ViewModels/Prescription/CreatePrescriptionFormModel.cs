namespace DentalManagementSystem.Web.ViewModels.Prescription
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
