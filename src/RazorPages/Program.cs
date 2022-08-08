using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using RazorPages.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add services to the container.

#region ConfigureServices

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddRazorPages();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();