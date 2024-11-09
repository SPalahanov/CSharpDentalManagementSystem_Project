namespace DentalManagementSystem.Web.ViewModels.Appointment
{
    using DentalManagementSystem.Common.Enums;
    using DentalManagementSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllAppointmentsIndexViewModel
    {
        public string Id { get; set; } = null!;

        public string AppointmentDate { get; set; } = null!;

        public string AppointmentStatus { get; set; } = null!;

        /*public int AppointmentTypeId { get; set; }
        public virtual AppointmentType AppointmentType { get; set; } = null!;

        [Required]
        public Guid PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;

        [Required]
        public Guid DentistId { get; set; }
        public virtual Dentist Dentist { get; set; } = null!;*/

        /*public virtual ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } =
            new HashSet<AppointmentProcedure>();*/
    }
}
