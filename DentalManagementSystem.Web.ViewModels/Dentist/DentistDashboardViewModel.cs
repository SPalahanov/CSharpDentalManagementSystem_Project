using DentalManagementSystem.Web.ViewModels.Home;

namespace DentalManagementSystem.Web.ViewModels.Dentist
{
    using System.Collections.Generic;

    public class DentistDashboardViewModel
    {
        public List<AppointmentViewModel> TodayAppointments { get; set; } = new List<AppointmentViewModel>();

        public int TodayAppointmentCount { get; set; }

        public int MonthlyPatientCount { get; set; }
    }
}
