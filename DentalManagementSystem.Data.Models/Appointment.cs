namespace DentalManagementSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DentalManagementSystem.Common.Enums;

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
        public TimeSpan AppointmentTime { get; set; }

        [Required]
        public AppointmentStatus AppointmentStatus { get; set; } // Schedule, Completed, Cancelled
        
        [Required]
        public int AppointmentTypeId { get; set; }
        public virtual AppointmentType AppointmentType { get; set; } = null!;

        public virtual ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } =
            new HashSet<AppointmentProcedure>();
    }
}
