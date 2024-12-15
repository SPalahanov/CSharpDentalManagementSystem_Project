namespace DentalManagementSystem.Web.ViewModels.Prescription
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DeletePrescriptionViewModel
    {
        public string PrescriptionId { get; set; } = null!;

        public string MedicationName { get; set; } = null!;

        public string? MedicationDescription { get; set; }

        public bool IsDeleted { get; set; }

        public Guid AppointmentId { get; set; }
    }
}
