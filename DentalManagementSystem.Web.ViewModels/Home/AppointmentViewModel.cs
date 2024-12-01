namespace DentalManagementSystem.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AppointmentViewModel
    {
        public string PatientName { get; set; } = null!;

        public string AppointmentDate { get; set; } = null!;

        public string AppointmentStatus { get; set; } = null!;
    }
}
