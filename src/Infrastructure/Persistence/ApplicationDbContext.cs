using System.Reflection;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private readonly ILogger<ApplicationDbContext> _logger;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
        ILogger<ApplicationDbContext> logger) 
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        _logger = logger;
    }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);

        optionsBuilder.LogTo(s => _logger.LogInformation(s));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
