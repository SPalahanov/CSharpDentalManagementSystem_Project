namespace DentalManagementSystem.Data.Configurations
{
    using DentalManagementSystem.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AppointmentEntityConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder
                .HasOne(a => a.AppointmentType)
                .WithMany(at => at.Appointments)
                .HasForeignKey(a => a.AppointmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.Patient)
                .WithMany(pat => pat.Appointments)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(p => p.Dentist)
                .WithMany(pat => pat.Appointments)
                .HasForeignKey(p => p.DentistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(a => a.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
