namespace DentalManagementSystem.Data.Seeding.DataTransferObjects
{
    using System.ComponentModel.DataAnnotations;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.AppointmentProcedures;

    public class ImportAppointmentProcedureDto
    {
        [Required]
        [MinLength(IdMinLength)]
        [MaxLength(IdMaxLength)]
        public string AppointmentId { get; set; } = null!;

        [Required]
        public int ProcedureId { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
