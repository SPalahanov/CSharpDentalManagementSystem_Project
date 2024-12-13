namespace DentalManagementSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Prescription;

    public class Prescription
    {
        public Prescription() 
        {
            this.PrescriptionId = Guid.NewGuid();
        }

        [Key]
        public Guid PrescriptionId { get; set; }

        [Required]
        [MaxLength(MedicationNameMaxLength)]
        public string MedicationName { get; set; } = null!;

        [MaxLength(MedicationDescriptionMaxLength)]
        public string? MedicationDescription { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public Guid AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
    }
}
