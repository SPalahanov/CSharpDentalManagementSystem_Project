namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using System;

    using DentalManagementSystem.Common.Enums;

    public class DeleteAppointmentViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime AppointmentDate { get; set; }

        public AppointmentStatus AppointmentStatus { get; set; }

        public int AppointmentTypeId { get; set; }

        public Guid PatientId { get; set; }

        public Guid DentistId { get; set; }
    }
}
