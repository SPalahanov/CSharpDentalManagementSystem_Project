namespace DentalManagementSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Procedure
    {
        [Key]
        public int ProcedureId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        public virtual ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } =
            new HashSet<AppointmentProcedure>();
    }
}
