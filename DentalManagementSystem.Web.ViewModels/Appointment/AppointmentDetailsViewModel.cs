namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using DentalManagementSystem.Web.ViewModels.Procedure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AppointmentDetailsViewModel
    {
        public string AppointmentDate { get; set; } = null!;

        public string PatientName { get; set; } = null!;

        public string DentistName { get; set; } = null!;

        public IEnumerable<AppointmentProcedureViewModel> Procedures { get; set; } =
            new HashSet<AppointmentProcedureViewModel>();
    }
}
