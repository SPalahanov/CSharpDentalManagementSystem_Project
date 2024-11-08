namespace DentalManagementSystem.Web.Infrastructure.Extensions
{
    using DentalManagementSystem.Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

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
