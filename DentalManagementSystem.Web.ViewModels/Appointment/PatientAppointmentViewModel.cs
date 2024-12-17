namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using System;

    public class PatientAppointmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
