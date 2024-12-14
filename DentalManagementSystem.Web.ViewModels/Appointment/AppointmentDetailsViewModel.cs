namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using DentalManagementSystem.Web.ViewModels.Prescription;
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AppointmentDetailsViewModel
    {
        public Guid AppointmentId { get; set; }

        public string AppointmentDate { get; set; } = null!;

        public string AppointmentStatus {  get; set; } = null!;

        public string PatientName { get; set; } = null!;

        public string DentistName { get; set; } = null!;

        public IEnumerable<AppointmentProcedureViewModel> Procedures { get; set; } =
            new HashSet<AppointmentProcedureViewModel>();

        public IEnumerable<AppointmentPrescriptionViewModel> Prescriptions { get; set; } =
            new HashSet<AppointmentPrescriptionViewModel>();
    }
}
