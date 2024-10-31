namespace DentalManagementSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.AppointmentType;

    public class AppointmentType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual IEnumerable<Appointment> Appointments { get; set; } =
            new HashSet<Appointment>();
    }
}
