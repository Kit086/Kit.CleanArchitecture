using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .Must(BeUniqueTitle).WithMessage("The specified title already exists.");
        // .MustAsync(BeUniqueTitleAsync).WithMessage("The specified title already exists.");
    }
    
    public bool BeUniqueTitle(UpdateTodoListCommand model, string title)
    {
        return _context.TodoLists
            .Where(l => l.Id != model.Id)
            .All(l => l.Title != title);
    }

    // public async Task<bool> BeUniqueTitleAsync(UpdateTodoListCommand model, string title, CancellationToken cancellationToken)
    // {
    //     return await _context.TodoLists
    //         .Where(l => l.Id != model.Id)
    //         .AllAsync(l => l.Title != title, cancellationToken);
    // }
}
