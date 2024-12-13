namespace DentalManagementSystem.Data.Models
{
    using DentalManagementSystem.Common.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Appointment
    {
        public Appointment()
        {
            this.AppointmentId = Guid.NewGuid();
        }

        [Key]
        public Guid AppointmentId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public AppointmentStatus AppointmentStatus { get; set; } // Schedule, Completed, Cancelled
        
        [Required]
        public int AppointmentTypeId { get; set; }
        public virtual AppointmentType AppointmentType { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public Guid PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;

        [Required]
        public Guid DentistId { get; set; }
        public virtual Dentist Dentist { get; set; } = null!;

        public virtual ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } =
            new HashSet<AppointmentProcedure>();

        public virtual ICollection<Prescription> Prescriptions { get; set; } =
            new HashSet<Prescription>();
    }
}
