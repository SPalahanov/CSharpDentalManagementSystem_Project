namespace DentalManagementSystem.Web.Infrastructure.Extensions
{
    using DentalManagementSystem.Data;
    using DentalManagementSystem.Services.Data;
    using DentalManagementSystem.Services.Data.Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public static class WebApplicationBuilderExtensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            DentalManagementSystemDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<DentalManagementSystemDbContext>()!;
            dbContext.Database.Migrate();

            return app;
        }
    }
}
