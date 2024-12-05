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

    public class DentistEntityConfiguration : IEntityTypeConfiguration<Dentist>
    {
        public void Configure(EntityTypeBuilder<Dentist> builder)
        {
            builder
                .Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
