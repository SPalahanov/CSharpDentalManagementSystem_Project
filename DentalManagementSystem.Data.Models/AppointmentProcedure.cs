namespace DentalManagementSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AppointmentProcedure
    {
        public Guid AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; } = null!;

        public int ProcedureId { get; set; }
        public virtual Procedure Procedure { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
