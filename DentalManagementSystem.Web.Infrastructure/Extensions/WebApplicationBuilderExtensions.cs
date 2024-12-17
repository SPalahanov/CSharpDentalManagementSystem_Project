namespace DentalManagementSystem.Web.Infrastructure.Extensions
{
    using DentalManagementSystem.Data;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Seeding;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using static DentalManagementSystem.Common.Constants.GeneralApplicationConstants;

    public static class WebApplicationBuilderExtensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            DentalManagementSystemDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<DentalManagementSystemDbContext>()!;
            dbContext.Database.Migrate();

            return app;
        }

        public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email, string username, string password)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();

            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            RoleManager<IdentityRole<Guid>>? roleManager = serviceProvider.GetService<RoleManager<IdentityRole<Guid>>>();
            IUserStore<ApplicationUser>? userStore = serviceProvider.GetService<IUserStore<ApplicationUser>>();
            UserManager<ApplicationUser>? userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager), $"Service for {typeof(RoleManager<IdentityRole<Guid>>)} cannot be obtained!");
            }

            if (userStore == null)
            {
                throw new ArgumentNullException(nameof(userStore), $"Service for {typeof(IUserStore<ApplicationUser>)} cannot be obtained!");
            }

            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager), $"Service for {typeof(UserManager<ApplicationUser>)} cannot be obtained!");
            }

            Task.Run(async () =>
            {
                bool roleExists = await roleManager.RoleExistsAsync(AdminRoleName);

                IdentityRole<Guid>? adminRole = null;

                if (!roleExists)
                {
                    adminRole = new IdentityRole<Guid>(AdminRoleName);

                    IdentityResult result = await roleManager.CreateAsync(adminRole);

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Error occurred while creating the {AdminRoleName} role!");
                    }
                }
                else
                {
                    adminRole = await roleManager.FindByNameAsync(AdminRoleName);
                }

                ApplicationUser? adminUser = await userManager.FindByEmailAsync(email);

                if (adminUser == null)
                {
                    adminUser = await
                        CreateAdminUserAsync(email, username, password, userStore, userManager);
                }

                if (await userManager.IsInRoleAsync(adminUser, AdminRoleName))
                {
                    return app;
                }

                IdentityResult userResult = await userManager.AddToRoleAsync(adminUser, AdminRoleName);

                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException($"Error occurred while adding the user {username} to the {AdminRoleName} role!");
                }

                return app;
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedProcedures(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();

            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedProceduresAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();
            
            return app;
        }

        public static IApplicationBuilder SeedUsers(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();

            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedUsersAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedDentists(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();

            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedDentistsAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedPatients(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();

            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedPatientsAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedAppointments(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();

            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedAppointmentsAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedAppointmentProcedures(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();

            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedAppointmentProceduresAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedPrescriptions(this IApplicationBuilder app, string jsonPath)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();

            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            Task.Run(async () =>
            {
                await DbSeeder.SeedPrescriptionsAsync(serviceProvider, jsonPath);
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        private static async Task<ApplicationUser?> CreateAdminUserAsync(string email, string username, string password, IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                Email = email
            };

            await userStore.SetUserNameAsync(applicationUser, username, CancellationToken.None);

            IdentityResult result = await userManager.CreateAsync(applicationUser, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error occurred while registering {AdminRoleName} user!");
            }

            return applicationUser;
        }
    }
}
