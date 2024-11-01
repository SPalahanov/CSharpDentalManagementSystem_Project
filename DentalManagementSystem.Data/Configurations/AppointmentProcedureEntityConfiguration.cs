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

    public class AppointmentProcedureEntityConfiguration : IEntityTypeConfiguration<AppointmentProcedure>
    {
        public void Configure(EntityTypeBuilder<AppointmentProcedure> builder)
        {
            builder.HasKey(ap => new 
            {
                ap.AppointmentId,
                ap.ProcedureId
            });

            builder
                .Property(ap => ap.IsDeleted)
                .HasDefaultValue(false);

            builder
                .HasOne(cm => cm.Appointment)             
                .WithMany(m => m.AppointmentProcedures)      
                .HasForeignKey(cm => cm.AppointmentId)    
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(cm => cm.Procedure)            
                .WithMany(c => c.AppointmentProcedures)      
                .HasForeignKey(cm => cm.ProcedureId)   
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
