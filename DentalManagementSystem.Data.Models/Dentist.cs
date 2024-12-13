namespace DentalManagementSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Dentist;

    public class Dentist
    {
        public Dentist()
        {
            this.DentistId = Guid.NewGuid();
        }

        [Key]
        public Guid DentistId { get; set; }

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
        public string Gender { get; set; } = null!;

        [Required]
        public string Specialty { get; set; } = null!;

        [Required]
        [MaxLength(LicenseNumberMaxLength)]
        public string LicenseNumber { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public virtual ICollection<Appointment> Appointments { get; set; } =
            new HashSet<Appointment>();
    }
}
