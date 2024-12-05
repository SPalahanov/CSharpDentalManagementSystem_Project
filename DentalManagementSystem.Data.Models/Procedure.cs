namespace DentalManagementSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Procedure;

    public class Procedure
    {
        [Key]
        public int ProcedureId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; }

        public virtual ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } =
            new HashSet<AppointmentProcedure>();

        public virtual ICollection<Prescription> Prescriptions { get; set; } =
            new HashSet<Prescription>();
    }
}
