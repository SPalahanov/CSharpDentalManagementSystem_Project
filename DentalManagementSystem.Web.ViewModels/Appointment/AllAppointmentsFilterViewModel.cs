namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using System.Collections.Generic;

    public class AllAppointmentsFilterViewModel
    {
        public IEnumerable<AllAppointmentsIndexViewModel>? Appointments { get; set; }

        public string? YearFilter { get; set; }

        public int? CurrentPage { get; set; } = 1;

        public int? EntitiesPerPage { get; set; } = 7;

        public int? TotalPages { get; set; }
    }
}
