namespace DentalManagementSystem.Web
{
    using DentalManagementSystem.Data;
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Data.Repository;
    using DentalManagementSystem.Data.Repository.Interfaces;
    using DentalManagementSystem.Data.Seeding.DataTransferObjects;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Services.Mapping;
    using DentalManagementSystem.Web.Infrastructure.Extensions;
    using DentalManagementSystem.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
            string adminEmail = builder.Configuration.GetValue<string>("Administrator:Email")!;
            string adminUserName = builder.Configuration.GetValue<string>("Administrator:UserName")!;
            string adminPassword = builder.Configuration.GetValue<string>("Administrator:Password")!;
            string proceduresJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, builder.Configuration.GetValue<string>("Seed:ProceduresJson")!);
            string usersJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, builder.Configuration.GetValue<string>("Seed:UsersJson")!);
            string dentistsJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, builder.Configuration.GetValue<string>("Seed:DentistsJson")!);
            string patientsJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, builder.Configuration.GetValue<string>("Seed:PatientsJson")!);
            string appointmentsJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, builder.Configuration.GetValue<string>("Seed:AppointmentsJson")!);
            string appointmentProceduresJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, builder.Configuration.GetValue<string>("Seed:AppointmentProceduresJson")!);
            string prescriptionsJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, builder.Configuration.GetValue<string>("Seed:PrescriptionsJson")!);

            builder.Services.AddDbContext<DentalManagementSystemDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    ConfigureIdentity(builder, options);
                })
                .AddEntityFrameworkStores<DentalManagementSystemDbContext>()
                .AddRoles<IdentityRole<Guid>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserManager<UserManager<ApplicationUser>>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
            });

            builder.Services.RegisterRepositories(typeof(ApplicationUser).Assembly);
            builder.Services.RegisterUserDefinedServices(typeof(IProcedureService).Assembly);

            builder.Services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));

            builder.Services.AddControllersWithViews();
            builder.Services.AddControllersWithViews(cfg =>
            {
                cfg.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            builder.Services.AddRazorPages();

            WebApplication app = builder.Build();

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly, typeof(ImportProcedureDto).Assembly);

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error/500");
                app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

            if (app.Environment.IsDevelopment())
            {
                app.SeedAdministrator(adminEmail, adminUserName, adminPassword);
                app.SeedProcedures(proceduresJsonPath);
                app.SeedUsers(usersJsonPath);
                app.SeedDentists(dentistsJsonPath);
                app.SeedPatients(patientsJsonPath);
                app.SeedAppointments(appointmentsJsonPath);
                app.SeedAppointmentProcedures(appointmentProceduresJsonPath);
                app.SeedPrescriptions(prescriptionsJsonPath);
            }

            app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.ApplyMigrations();
            app.Run();
        }

        private static void ConfigureIdentity(WebApplicationBuilder builder, IdentityOptions options)
        {
            options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigits");
            options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
            options.Password.RequireUppercase =builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
            options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumerical");
            options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
            options.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>("Identity:Password:RequiredUniqueCharacters");

            options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");

            options.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("Identity:User:RequireUniqueEmail");
        }
    }
}
