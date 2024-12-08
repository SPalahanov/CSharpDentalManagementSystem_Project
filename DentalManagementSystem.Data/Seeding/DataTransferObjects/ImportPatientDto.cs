namespace DentalManagementSystem.Data.Seeding.DataTransferObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Patient;

    public class ImportPatientDto
    {
        [Required]
        public string PatientId { get; set; } = null!;

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(PhoneNumberMinLength)]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MinLength(AddressMinLength)]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        public string DateOfBirth { get; set; } = null!;

        [Required]
        public string Gender { get; set; } = null!;

        public string? Allergies { get; set; }

        public string? InsuranceNumber { get; set; }

        public string? EmergencyContact { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
    }
}
