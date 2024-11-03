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

            //builder.HasData(GenerateAppointment());
        }

        private Appointment[] GenerateAppointment()
        { 
            ICollection<Appointment> appointments = new HashSet<Appointment>();

            Appointment appointment;

            appointment = new Appointment()
            {
                AppointmentDate = DateTime.UtcNow,
                AppointmentStatus = Common.Enums.AppointmentStatus.Schedule,
                AppointmentTypeId = 3,
                PatientId = Guid.Parse(""),
                DentistId = Guid.Parse(""),
            };
            appointments.Add(appointment);

            appointment = new Appointment()
            {
                AppointmentDate = DateTime.UtcNow,
                AppointmentStatus = Common.Enums.AppointmentStatus.Schedule,
                AppointmentTypeId = 1,
                PatientId = Guid.Parse(""),
                DentistId = Guid.Parse("FFA779BE-A8D5-4662-95F2-D5771AE8A22A"),
            };
            appointments.Add(appointment);

            return appointments.ToArray();
        }
    }
}
