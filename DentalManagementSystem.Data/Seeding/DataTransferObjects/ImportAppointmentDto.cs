namespace DentalManagementSystem.Data.Seeding.DataTransferObjects
{
    using DentalManagementSystem.Common.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Appointment;

    public class ImportAppointmentDto
    {
        [Required]
        [MinLength(IdMinLength)]
        [MaxLength(IdMaxLength)]
        public string AppointmentId { get; set; } = null!;

        [Required]
        public string AppointmentDate { get; set; } = null!;

        [Required]
        public string AppointmentStatus { get; set; } = null!;

        [Required]
        public int AppointmentTypeId { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        [MinLength(IdMinLength)]
        [MaxLength(IdMaxLength)]
        public string PatientId { get; set; } = null!;

        [Required]
        [MinLength(IdMinLength)]
        [MaxLength(IdMaxLength)]
        public string DentistId { get; set; } = null!;
    }
}
