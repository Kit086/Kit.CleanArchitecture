using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Files;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Interceptors;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseSqlite"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=CleanArchitectureDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                    new MariaDbServerVersion(new Version(10,6)),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        // services
        //     .AddDefaultIdentity<ApplicationUser>()
        //     .AddRoles<IdentityRole>()
        //     .AddEntityFrameworkStores<ApplicationDbContext>();

        // services.AddIdentityServer()
        //     .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        // services.AddAuthentication()
        //     .AddIdentityServerJwt();

        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}
