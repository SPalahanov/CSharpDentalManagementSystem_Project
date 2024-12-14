namespace DentalManagementSystem.Web.ViewModels.Prescription
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AppointmentPrescriptionViewModel
    {
        public Guid Id { get; set; }

        public string MedicationName { get; set; } = null!;

        public string? MedicationDescription { get; set; }
    }
}
