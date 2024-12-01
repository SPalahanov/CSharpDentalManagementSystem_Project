using DentalManagementSystem.Web.ViewModels.Home;

namespace DentalManagementSystem.Web.ViewModels.Dentist
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DentistDashboardViewModel
    {
        public List<AppointmentViewModel> TodayAppointments { get; set; } = new List<AppointmentViewModel>();

        public int TodayAppointmentCount { get; set; }

        public int MonthlyPatientCount { get; set; }
    }
}
