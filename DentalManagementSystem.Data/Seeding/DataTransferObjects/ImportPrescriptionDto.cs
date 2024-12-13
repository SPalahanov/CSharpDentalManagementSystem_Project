namespace DentalManagementSystem.Data.Seeding.DataTransferObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Prescription;

    public class ImportPrescriptionDto
    {
        [Required]
        [MinLength(IdMinLength)]
        [MaxLength(IdMaxLength)]
        public string PrescriptionId { get; set; } = null!;

        [Required]
        [MinLength(MedicationNameMinLength)]
        [MaxLength(MedicationNameMaxLength)]
        public string MedicationName { get; set; } = null!;

        [MinLength(MedicationDescriptionMinLength)]
        [MaxLength(MedicationDescriptionMaxLength)]
        public string? MedicationDescription { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        [MinLength(IdMinLength)]
        [MaxLength(IdMaxLength)]
        public string AppointmentId { get; set; } = null!;
    }
}
