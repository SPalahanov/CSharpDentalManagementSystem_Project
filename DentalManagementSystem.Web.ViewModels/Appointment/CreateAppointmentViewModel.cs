namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DentalManagementSystem.Common.Enums;

    using Microsoft.AspNetCore.Authorization;

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

        public IEnumerable<ProcedureAppointmentViewModel> AvailableProcedures { get; set; } = new List<ProcedureAppointmentViewModel>();  // New property
        public IEnumerable<int> SelectedProcedureIds { get; set; } = new List<int>();
    }
}
