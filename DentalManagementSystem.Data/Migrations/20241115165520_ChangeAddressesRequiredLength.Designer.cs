﻿// <auto-generated />
using System;
using DentalManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DentalManagementSystem.Data.Migrations
{
    [DbContext(typeof(DentalManagementSystemDbContext))]
    [Migration("20241115165520_ChangeAddressesRequiredLength")]
    partial class ChangeAddressesRequiredLength
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DentalManagementSystem.Data.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Appointment", b =>
                {
                    b.Property<Guid>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("AppointmentStatus")
                        .HasColumnType("int");

                    b.Property<int>("AppointmentTypeId")
                        .HasColumnType("int");

                    b.Property<Guid>("DentistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AppointmentId");

                    b.HasIndex("AppointmentTypeId");

                    b.HasIndex("DentistId");

                    b.HasIndex("PatientId");

                    b.ToTable("Appointments");

                    b.HasData(
                        new
                        {
                            AppointmentId = new Guid("d43c1c9b-da63-430d-a2f4-a79fac61e87c"),
                            AppointmentDate = new DateTime(2024, 11, 15, 16, 55, 19, 80, DateTimeKind.Utc).AddTicks(8618),
                            AppointmentStatus = 0,
                            AppointmentTypeId = 1,
                            DentistId = new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"),
                            PatientId = new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f")
                        });
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.AppointmentProcedure", b =>
                {
                    b.Property<Guid>("AppointmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ProcedureId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.HasKey("AppointmentId", "ProcedureId");

                    b.HasIndex("ProcedureId");

                    b.ToTable("AppointmentProcedures");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.AppointmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("AppointmentsTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Consultation"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Examination"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Routine care"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Surgery"
                        });
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Dentist", b =>
                {
                    b.Property<Guid>("DentistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Specialty")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DentistId");

                    b.HasIndex("UserId");

                    b.ToTable("Dentists");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Patient", b =>
                {
                    b.Property<Guid>("PatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("Allergies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmergencyContact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InsuranceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PatientId");

                    b.HasIndex("UserId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Prescription", b =>
                {
                    b.Property<Guid>("PrescriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DentistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ProcedureId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("PrescriptionId");

                    b.HasIndex("DentistId");

                    b.HasIndex("PatientId");

                    b.HasIndex("ProcedureId");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Procedure", b =>
                {
                    b.Property<int>("ProcedureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProcedureId"));

                    b.Property<Guid?>("DentistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ProcedureId");

                    b.HasIndex("DentistId");

                    b.ToTable("Procedures");

                    b.HasData(
                        new
                        {
                            ProcedureId = 1,
                            Description = "Professional cleaning to remove tartar and plaque using an ultrasonic scaler.",
                            Name = "Teeth Cleaning",
                            Price = 30m
                        },
                        new
                        {
                            ProcedureId = 2,
                            Description = "Treatment of dental cavities with high-quality composite filling to restore the tooth's function and aesthetics.",
                            Name = "Cavity Filling",
                            Price = 70m
                        },
                        new
                        {
                            ProcedureId = 3,
                            Description = "Removal of a tooth due to damage, infection, or orthodontic reasons.",
                            Name = "Tooth Extraction",
                            Price = 80m
                        },
                        new
                        {
                            ProcedureId = 4,
                            Description = "Professional whitening procedure using special gels and lamps to achieve a brighter smile.",
                            Name = "Teeth Whitening",
                            Price = 150m
                        },
                        new
                        {
                            ProcedureId = 5,
                            Description = "Placement of thin porcelain veneers to improve the appearance of front teeth, giving them a natural and attractive look",
                            Name = "Veneers",
                            Price = 250m
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Appointment", b =>
                {
                    b.HasOne("DentalManagementSystem.Data.Models.AppointmentType", "AppointmentType")
                        .WithMany("Appointments")
                        .HasForeignKey("AppointmentTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DentalManagementSystem.Data.Models.Dentist", "Dentist")
                        .WithMany("Appointments")
                        .HasForeignKey("DentistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DentalManagementSystem.Data.Models.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AppointmentType");

                    b.Navigation("Dentist");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.AppointmentProcedure", b =>
                {
                    b.HasOne("DentalManagementSystem.Data.Models.Appointment", "Appointment")
                        .WithMany("AppointmentProcedures")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DentalManagementSystem.Data.Models.Procedure", "Procedure")
                        .WithMany("AppointmentProcedures")
                        .HasForeignKey("ProcedureId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Procedure");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Dentist", b =>
                {
                    b.HasOne("DentalManagementSystem.Data.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Patient", b =>
                {
                    b.HasOne("DentalManagementSystem.Data.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Prescription", b =>
                {
                    b.HasOne("DentalManagementSystem.Data.Models.Dentist", "Dentist")
                        .WithMany("Prescriptions")
                        .HasForeignKey("DentistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DentalManagementSystem.Data.Models.Patient", "Patient")
                        .WithMany("Prescriptions")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DentalManagementSystem.Data.Models.Procedure", "Procedure")
                        .WithMany("Prescriptions")
                        .HasForeignKey("ProcedureId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Dentist");

                    b.Navigation("Patient");

                    b.Navigation("Procedure");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Procedure", b =>
                {
                    b.HasOne("DentalManagementSystem.Data.Models.Dentist", null)
                        .WithMany("Procedures")
                        .HasForeignKey("DentistId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("DentalManagementSystem.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("DentalManagementSystem.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DentalManagementSystem.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("DentalManagementSystem.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Appointment", b =>
                {
                    b.Navigation("AppointmentProcedures");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.AppointmentType", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Dentist", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Prescriptions");

                    b.Navigation("Procedures");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Patient", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("DentalManagementSystem.Data.Models.Procedure", b =>
                {
                    b.Navigation("AppointmentProcedures");

                    b.Navigation("Prescriptions");
                });
#pragma warning restore 612, 618
        }
    }
}