using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using RazorPages.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddRazorPagesServices(this IServiceCollection services)
    {
        services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.AddDatabaseDeveloperPageExceptionFilter();
        
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();
        
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();
        
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        
        services.AddRazorPages();

        return services;
    }
}