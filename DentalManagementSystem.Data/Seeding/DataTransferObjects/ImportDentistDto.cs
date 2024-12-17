namespace DentalManagementSystem.Data.Seeding.DataTransferObjects
{
    using System.ComponentModel.DataAnnotations;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Dentist;

    public class ImportDentistDto
    {
        [Required]
        [MinLength(IdMinLength)]
        [MaxLength(IdMaxLength)]
        public string DentistId { get; set; } = null!;

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
        public string Gender { get; set; } = null!;

        [Required]
        public string Specialty { get; set; } = null!;

        [Required]
        [RegularExpression(LicenseNumberRegex)]
        public string LicenseNumber { get; set; } = null!;

        [Required]
        [MinLength(IdMinLength)]
        [MaxLength(IdMaxLength)]
        public string UserId { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; }
    }
}
