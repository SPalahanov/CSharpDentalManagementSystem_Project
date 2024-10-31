namespace DentalManagementSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static DentalManagementSystem.Common.Constants.EntityValidationConstants.Prescription;

    public class Prescription
    {
        [Key]
        public Guid PrescriptionId { get; set; }

        [Required]
        [MaxLength(MedicineNameMaxLength)]
        public string MedicineName = null!;

        [Required]
        public int Quantity { get; set; }

        public Guid PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;

        public int ProcedureId { get; set; }
        public virtual Procedure Procedure { get; set; } = null!;
    }
}
