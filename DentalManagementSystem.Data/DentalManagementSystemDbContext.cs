﻿namespace DentalManagementSystem.Data
{
    using System.Reflection;

    using DentalManagementSystem.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class DentalManagementSystemDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DentalManagementSystemDbContext()
        {

        }

        public DentalManagementSystemDbContext(DbContextOptions<DentalManagementSystemDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<Dentist> Dentists { get; set; } = null!;
        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Procedure> Procedures { get; set; } = null!;
        public virtual DbSet<AppointmentProcedure> AppointmentProcedures { get; set; } = null!;
        public virtual DbSet<AppointmentType> AppointmentsTypes { get; set; } = null!;
        public virtual DbSet<Prescription> Prescriptions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
