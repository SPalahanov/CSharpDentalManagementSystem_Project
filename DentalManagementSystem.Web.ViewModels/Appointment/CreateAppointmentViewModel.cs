namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using DentalManagementSystem.Common.Enums;
    using Microsoft.AspNetCore.Authorization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Authorize]
    public class CreateAppointmentViewModel
    {
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

        public IEnumerable<PatientAppointmentViewModel> Patients { get; set; } = new List<PatientAppointmentViewModel>();

        public IEnumerable<DentistAppointmentViewModel> Dentists { get; set; } = new List<DentistAppointmentViewModel>();

        public IEnumerable<AppointmentTypeViewModel> AppointmentTypes { get; set; } = new List<AppointmentTypeViewModel>();
    }
}
