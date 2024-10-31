namespace DentalManagementSystem.Data
{
    using DentalManagementSystem.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class DentalManagementSystemDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DentalManagementSystemDbContext()
        {
        }

        public DentalManagementSystemDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Procedure> Procedures { get; set; } = null!;
        public virtual DbSet<Prescription> Prescriptions { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<AppointmentType> AppointmentsTypes { get; set; } = null!;
        public virtual DbSet<AppointmentProcedure> AppointmentProcedures { get; set; } = null!;
        public virtual DbSet<Appointment> Appointments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
