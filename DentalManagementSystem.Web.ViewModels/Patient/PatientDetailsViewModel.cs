namespace DentalManagementSystem.Web.ViewModels.Patient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PatientDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string DateOfBirth { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string? Allergies { get; set; }

        public string? InsuranceNumber { get; set; }

        public string? EmergencyContact { get; set; }
    }
}
