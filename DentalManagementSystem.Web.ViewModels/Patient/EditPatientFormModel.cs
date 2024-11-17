namespace DentalManagementSystem.Web.ViewModels.Patient
{
    using System.ComponentModel.DataAnnotations;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Patient;

    public class EditPatientFormModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Phone]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; set; } = null!;

        [Required]
        public string DateOfBirth { get; set; } = null!;

        [Required]
        public string Gender { get; set; } = null!;

        public string? Allergies { get; set; }

        public string? InsuranceNumber { get; set; }

        public string? EmergencyContact { get; set; }
    }
}
