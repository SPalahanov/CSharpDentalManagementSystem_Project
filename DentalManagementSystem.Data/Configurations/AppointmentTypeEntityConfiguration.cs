namespace DentalManagementSystem.Data.Configurations
{
    using DentalManagementSystem.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System.Collections.Generic;
    using System.Linq;

    public class AppointmentTypeEntityConfiguration : IEntityTypeConfiguration<AppointmentType>
    {
        public void Configure(EntityTypeBuilder<AppointmentType> builder)
        {
            builder.HasData(this.GenerateAppointmentType());
        }

        private AppointmentType[] GenerateAppointmentType()
        {
            ICollection<AppointmentType> appointmentTypes = new HashSet <AppointmentType>();

            AppointmentType appointmentType;

            appointmentType = new AppointmentType() 
            { 
                Id = 1,
                Name = "Consultation"
            };
            appointmentTypes.Add(appointmentType);

            appointmentType = new AppointmentType()
            {
                Id = 2,
                Name = "Examination"
            };
            appointmentTypes.Add(appointmentType);

            appointmentType = new AppointmentType()
            {
                Id = 3,
                Name = "Routine care"
            };
            appointmentTypes.Add(appointmentType);

            appointmentType = new AppointmentType()
            {
                Id = 4,
                Name = "Surgery"
            };
            appointmentTypes.Add(appointmentType);

            return appointmentTypes.ToArray();
        }
    }
}
