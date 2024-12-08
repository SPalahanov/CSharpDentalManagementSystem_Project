﻿namespace DentalManagementSystem.Data.Seeding
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Seeding.DataTransferObjects;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
