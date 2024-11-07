namespace DentalManagementSystem.Web.ViewModels.Dentist
{
    using System.ComponentModel.DataAnnotations;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Dentist;

    public class BecomeDentistFormModel
    {
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
        public string Gender { get; set; } = null!;

        [Required]
        public string Specialty { get; set; } = null!;

        [Required]
        [StringLength(LicenseNumberMaxLength, MinimumLength = LicenseNumberMinLength)]
        public string LicenseNumber { get; set; } = null!;
    }
}
