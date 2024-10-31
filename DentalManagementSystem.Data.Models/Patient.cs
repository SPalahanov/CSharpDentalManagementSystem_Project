namespace DentalManagementSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Patient;

    public class Patient
    {
        public Patient()
        {
            this.PatientId = Guid.NewGuid();
        }

        [Key]
        public Guid PatientId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = null!;

        public string Allergies { get; set; } = null!;

        public string EmergencyContact { get; set; } = null!;

        public virtual ICollection<Appointment> Appointments { get; set; } =
            new HashSet<Appointment>();
    }
}
