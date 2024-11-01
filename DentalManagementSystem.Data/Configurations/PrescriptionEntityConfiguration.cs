namespace DentalManagementSystem.Data.Configurations
{
    using DentalManagementSystem.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PrescriptionEntityConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder
                .HasOne(p => p.Procedure)
                .WithMany(proc => proc.Prescriptions)
                .HasForeignKey(p => p.ProcedureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.Patient)
                .WithMany(pat => pat.Prescriptions)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasData(this.GeneratePrescriptions());
        }

        // TODO: Add data to seed in Prescription

        /*private Prescription[] GeneratePrescriptions()
        {
            ICollection<Prescription> prescriptions = new HashSet<Prescription>();

            Prescription prescription;

            prescription = new Prescription()
            {

            };
            prescriptions.Add(prescription);

            return prescriptions.ToArray();
        }*/
    }
}
