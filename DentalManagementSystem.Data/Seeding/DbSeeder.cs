namespace DentalManagementSystem.Data.Seeding
{
    using DentalManagementSystem.Common.Enums;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Seeding.DataTransferObjects;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Threading.Tasks;

    public static class DbSeeder
    {
        public static async Task SeedProceduresAsync(IServiceProvider services, string jsonPath)
        {
            await using DentalManagementSystemDbContext dbContext = services.GetRequiredService<DentalManagementSystemDbContext>();

            ICollection<Procedure> allProcedures = await dbContext
                .Procedures
                .ToArrayAsync();

            try
            {
                string jsonInput = await File.ReadAllTextAsync(jsonPath, Encoding.ASCII, CancellationToken.None);

                ImportProcedureDto[] procedureDtos = JsonConvert.DeserializeObject<ImportProcedureDto[]>(jsonInput);

                foreach (var procedureDto in procedureDtos)
                {
                    if (!IsValid(procedureDto))
                    {
                        continue;
                    }

                    if (allProcedures.Any(p => p.Name == procedureDto.Name))
                    {
                        continue;
                    }

                    Procedure procedure = new Procedure
                    {
                        Name = procedureDto.Name,
                        Price = procedureDto.Price,
                        Description = procedureDto.Description,
                        IsDeleted = procedureDto.IsDeleted
                    };

                    await dbContext.Procedures.AddAsync(procedure);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred while seeding the procedures in the database!");
            }
        }

        public static async Task SeedUsersAsync(IServiceProvider services, string jsonPath)
        {
            UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            try
            {
                string jsonInput = await File.ReadAllTextAsync(jsonPath, Encoding.UTF8);

                ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonInput);

                foreach (var userDto in userDtos)
                {
                    if (!IsValid(userDto))
                    {
                        continue;
                    }

                    Guid userGuid = Guid.Empty;

                    if (!IsGuidValid(userDto.Id, ref userGuid))
                    {
                        continue;
                    }
                    
                    if (await userManager.FindByEmailAsync(userDto.Email) != null)
                    {
                        continue;
                    }

                    ApplicationUser user = new ApplicationUser
                    {
                        Id = userGuid,
                        UserName = userDto.Username,
                        Email = userDto.Email,
                        EmailConfirmed = true
                    };

                    IdentityResult result = await userManager.CreateAsync(user, userDto.Password);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred while seeding the users in the database!");
            }
        }

        public static async Task SeedDentistsAsync(IServiceProvider services, string jsonPath)
        {
            await using DentalManagementSystemDbContext dbContext = services.GetRequiredService<DentalManagementSystemDbContext>();

            UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            ICollection<Dentist> allDentists = await dbContext
                .Dentists
                .ToArrayAsync();

            try
            {
                string jsonInput = await File.ReadAllTextAsync(jsonPath, Encoding.ASCII, CancellationToken.None);

                ImportDentistDto[]? dentistDtos = JsonConvert.DeserializeObject<ImportDentistDto[]>(jsonInput);

                foreach (var dentistDto in dentistDtos)
                {
                    if (!IsValid(dentistDto))
                    {
                        continue;
                    }

                    Guid dentistGuid = Guid.Empty;

                    if (!IsGuidValid(dentistDto.DentistId, ref dentistGuid))
                    {
                        continue;
                    }

                    Guid userGuid = Guid.Empty;

                    if (!IsGuidValid(dentistDto.UserId, ref userGuid))
                    {
                        continue;
                    }

                    var userExists = await userManager.FindByIdAsync(userGuid.ToString()) != null;

                    if (!userExists)
                    {
                        continue;
                    }

                    if (allDentists.Any(d => d.LicenseNumber == dentistDto.LicenseNumber))
                    {
                        continue;
                    }

                    Dentist dentist = new Dentist
                    {
                        DentistId = dentistGuid,
                        Name = dentistDto.Name,
                        PhoneNumber = dentistDto.PhoneNumber,
                        Address = dentistDto.Address,
                        Gender = dentistDto.Gender,
                        Specialty = dentistDto.Specialty,
                        LicenseNumber = dentistDto.LicenseNumber,
                        UserId = userGuid,
                        IsDeleted = dentistDto.IsDeleted
                    };

                    await dbContext.Dentists.AddAsync(dentist);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred while seeding the dentists in the database!");
            }
        }

        public static async Task SeedPatientsAsync(IServiceProvider services, string jsonPath)
        {
            await using DentalManagementSystemDbContext dbContext = services.GetRequiredService<DentalManagementSystemDbContext>();

            UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            ICollection<Patient> allPatients = await dbContext
                .Patients
                .ToArrayAsync();

            try
            {
                string jsonInput = await File.ReadAllTextAsync(jsonPath, Encoding.ASCII, CancellationToken.None);

                ImportPatientDto[]? patientDtos = JsonConvert.DeserializeObject<ImportPatientDto[]>(jsonInput);

                foreach (var patientDto in patientDtos)
                {
                    if (!IsValid(patientDto))
                    {
                        continue;
                    }

                    Guid patientGuid = Guid.Empty;

                    if (!IsGuidValid(patientDto.PatientId, ref patientGuid))
                    {
                        continue;
                    }

                    Guid userGuid = Guid.Empty;

                    if (!IsGuidValid(patientDto.UserId, ref userGuid))
                    {
                        continue;
                    }

                    bool isDateOfBirthValid = DateTime
                        .TryParse(patientDto.DateOfBirth, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth);

                    if (!isDateOfBirthValid)
                    {
                        continue;
                    }

                    var userExists = await userManager.FindByIdAsync(userGuid.ToString()) != null;

                    if (!userExists)
                    {
                        continue;
                    }

                    if (allPatients.Any(p => p.PatientId.ToString().ToLowerInvariant() == patientGuid.ToString().ToLowerInvariant()))
                    {
                        continue;
                    }

                    Patient patient = new Patient
                    {
                        PatientId = patientGuid,
                        Name = patientDto.Name,
                        PhoneNumber = patientDto.PhoneNumber,
                        Address = patientDto.Address,
                        DateOfBirth = dateOfBirth,
                        Gender = patientDto.Gender,
                        Allergies = patientDto.Allergies,
                        InsuranceNumber = patientDto.InsuranceNumber,
                        EmergencyContact = patientDto.EmergencyContact,
                        UserId = userGuid,
                        IsDeleted = patientDto.IsDeleted
                    };

                    await dbContext.Patients.AddAsync(patient);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred while seeding the patients in the database!");
            }
        }

        public static async Task SeedAppointmentsAsync(IServiceProvider services, string jsonPath)
        {
            await using DentalManagementSystemDbContext dbContext = services.GetRequiredService<DentalManagementSystemDbContext>();

            ICollection<Appointment> allAppointments = await dbContext
                .Appointments
                .ToArrayAsync();

            try
            {
                string jsonInput = await File.ReadAllTextAsync(jsonPath, Encoding.ASCII, CancellationToken.None);

                ImportAppointmentDto[]? appointmentDtos = JsonConvert.DeserializeObject<ImportAppointmentDto[]>(jsonInput);

                foreach (var appointmentDto in appointmentDtos)
                {
                    if (!IsValid(appointmentDto))
                    {
                        continue;
                    }

                    Guid appointmentGuid = Guid.Empty;

                    if (!IsGuidValid(appointmentDto.AppointmentId, ref appointmentGuid))
                    {
                        continue;
                    }

                    Guid dentistGuid = Guid.Empty;

                    if (!IsGuidValid(appointmentDto.DentistId, ref dentistGuid))
                    {
                        continue;
                    }

                    Guid patientGuid = Guid.Empty;

                    if (!IsGuidValid(appointmentDto.PatientId, ref patientGuid))
                    {
                        continue;
                    }

                    bool isAppointmentDateValid = DateTime
                        .TryParse(appointmentDto.AppointmentDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime appointmentDate);

                    if (!isAppointmentDateValid)
                    {
                        continue;
                    }

                    if (!Enum.TryParse<AppointmentStatus>(appointmentDto.AppointmentStatus, true, out var status))
                    {
                        continue;
                    }

                    if (allAppointments.Any(p => p.AppointmentId.ToString().ToLowerInvariant() == appointmentGuid.ToString().ToLowerInvariant()))
                    {
                        continue;
                    }

                    Appointment appointment = new Appointment
                    {
                        
                        AppointmentId = appointmentGuid,
                        AppointmentDate = appointmentDate,
                        AppointmentStatus = status,
                        AppointmentTypeId = appointmentDto.AppointmentTypeId,
                        IsDeleted = appointmentDto.IsDeleted,
                        PatientId = patientGuid,
                        DentistId = dentistGuid,
                    };

                    await dbContext.Appointments.AddAsync(appointment);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred while seeding the appointments in the database!");
            }
        }

        public static async Task SeedAppointmentProceduresAsync(IServiceProvider services, string jsonPath)
        {
            await using DentalManagementSystemDbContext dbContext = services.GetRequiredService<DentalManagementSystemDbContext>();

            ICollection<AppointmentProcedure> allAppointmentProcedures = await dbContext
                .AppointmentProcedures
                .ToArrayAsync();

            try
            {
                string jsonInput = await File.ReadAllTextAsync(jsonPath, Encoding.ASCII, CancellationToken.None);

                ImportAppointmentProcedureDto[]? appointmentProcedureDtos = JsonConvert.DeserializeObject<ImportAppointmentProcedureDto[]>(jsonInput);

                foreach (var appointmentProcedureDto in appointmentProcedureDtos)
                {
                    if (!IsValid(appointmentProcedureDto))
                    {
                        continue;
                    }

                    Guid appointmentProcedureGuid = Guid.Empty;

                    if (!IsGuidValid(appointmentProcedureDto.AppointmentId, ref appointmentProcedureGuid))
                    {
                        continue;
                    }

                    if (allAppointmentProcedures.Any(ap => ap.AppointmentId.ToString().ToLowerInvariant() == appointmentProcedureGuid.ToString().ToLowerInvariant()))
                    {
                        continue;
                    }

                    AppointmentProcedure appointmentProcedures = new AppointmentProcedure
                    {

                        AppointmentId = appointmentProcedureGuid,
                        ProcedureId = appointmentProcedureDto.ProcedureId,
                        IsDeleted = appointmentProcedureDto.IsDeleted,
                    };

                    await dbContext.AppointmentProcedures.AddAsync(appointmentProcedures);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred while seeding the appointment procedures in the database!");
            }
        }

        public static async Task SeedPrescriptionsAsync(IServiceProvider services, string jsonPath)
        {
            await using DentalManagementSystemDbContext dbContext = services.GetRequiredService<DentalManagementSystemDbContext>();

            ICollection<Prescription> allPrescriptions = await dbContext
                .Prescriptions
                .ToArrayAsync();

            try
            {
                string jsonInput = await File.ReadAllTextAsync(jsonPath, Encoding.ASCII, CancellationToken.None);

                ImportPrescriptionDto[]? prescriptionDtos = JsonConvert.DeserializeObject<ImportPrescriptionDto[]>(jsonInput);

                foreach (var prescriptionDto in prescriptionDtos)
                {
                    if (!IsValid(prescriptionDto))
                    {
                        continue;
                    }

                    Guid prescriptionGuid = Guid.Empty;

                    if (!IsGuidValid(prescriptionDto.PrescriptionId, ref prescriptionGuid))
                    {
                        continue;
                    }

                    Guid appointmentGuid = Guid.Empty;

                    if (!IsGuidValid(prescriptionDto.AppointmentId, ref appointmentGuid))
                    {
                        continue;
                    }

                    if (allPrescriptions.Any(p => p.PrescriptionId.ToString().ToLowerInvariant() == prescriptionGuid.ToString().ToLowerInvariant()))
                    {
                        continue;
                    }

                    Prescription prescription = new Prescription
                    {
                        PrescriptionId = prescriptionGuid,
                        MedicationName = prescriptionDto.MedicationName,
                        MedicationDescription = prescriptionDto.MedicationDescription,
                        AppointmentId = appointmentGuid,
                        IsDeleted = prescriptionDto.IsDeleted,
                    };

                    await dbContext.Prescriptions.AddAsync(prescription);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred while seeding the prescriptions in the database!");
            }
        }

        private static bool IsValid(object obj)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            var context = new ValidationContext(obj);

            var isValid = Validator.TryValidateObject(obj, context, validationResults);

            return isValid;
        }

        private static bool IsGuidValid(string id, ref Guid parsedGuid)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return false;
            }
           
            bool isGuidValid = Guid.TryParse(id, out parsedGuid);

            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }

        private static bool IsIntegerValid(string id)
        {
            if (int.TryParse(id, out int parsedValue))
            {
                return parsedValue >= 1;
            }

            return false;
        }
    }
}
