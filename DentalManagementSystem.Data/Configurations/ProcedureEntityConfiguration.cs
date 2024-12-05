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

    public class ProcedureEntityConfiguration : IEntityTypeConfiguration<Procedure>
    {
        public void Configure(EntityTypeBuilder<Procedure> builder)
        {
            builder
                .Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasData(GenerateProcedure());
        }

        private Procedure[] GenerateProcedure()
        {
            ICollection<Procedure> procedures = new HashSet<Procedure>();

            Procedure procedure;

            procedure = new Procedure() 
            {
                ProcedureId = 1,
                Name = "Teeth Cleaning",
                Price = 30,
                Description = "Professional cleaning to remove tartar and plaque using an ultrasonic scaler."
            };
            procedures.Add(procedure);

            procedure = new Procedure()
            {
                ProcedureId = 2,
                Name = "Cavity Filling",
                Price = 70,
                Description = "Treatment of dental cavities with high-quality composite filling to restore the tooth's function and aesthetics."
            };
            procedures.Add(procedure);

            procedure = new Procedure()
            {
                ProcedureId = 3,
                Name = "Tooth Extraction",
                Price = 80,
                Description = "Removal of a tooth due to damage, infection, or orthodontic reasons."
            };
            procedures.Add(procedure);

            procedure = new Procedure()
            {
                ProcedureId = 4,
                Name = "Teeth Whitening",
                Price = 150,
                Description = "Professional whitening procedure using special gels and lamps to achieve a brighter smile."
            };
            procedures.Add(procedure);

            procedure = new Procedure()
            {
                ProcedureId = 5,
                Name = "Veneers",
                Price = 250,
                Description = "Placement of thin porcelain veneers to improve the appearance of front teeth, giving them a natural and attractive look"
            };
            procedures.Add(procedure);

            return procedures.ToArray();
        }
    }
}
