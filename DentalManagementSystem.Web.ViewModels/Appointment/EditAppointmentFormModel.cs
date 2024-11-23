namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using DentalManagementSystem.Common.Enums;
    using System.ComponentModel.DataAnnotations;

    public class EditAppointmentFormModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public AppointmentStatus AppointmentStatus { get; set; }

        [Required]
        public int AppointmentTypeId { get; set; }

        [Required]
        public Guid PatientId { get; set; }

        [Required]
        public Guid DentistId { get; set; }

        public IEnumerable<PatientAppointmentViewModel> Patients { get; set; } = new HashSet<PatientAppointmentViewModel>();

        public IEnumerable<DentistAppointmentViewModel> Dentists { get; set; } = new HashSet<DentistAppointmentViewModel>();

        public IEnumerable<AppointmentTypeViewModel> AppointmentTypes { get; set; } = new HashSet<AppointmentTypeViewModel>();

        public IEnumerable<ProcedureAppointmentViewModel> AvailableProcedures { get; set; } = new HashSet<ProcedureAppointmentViewModel>();
        public IEnumerable<int> SelectedProcedureIds { get; set; } = new List<int>();
    }
}
