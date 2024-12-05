namespace DentalManagementSystem.Web.ViewModels.Dentist
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DeleteDentistViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string Specialty { get; set; } = null!;

        public string LicenseNumber { get; set; } = null!;
    }
}
