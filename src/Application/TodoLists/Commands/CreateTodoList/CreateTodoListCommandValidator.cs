using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoListCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .Must(BeUniqueTitle).WithMessage("The specified title already exists.");
        // .MustAsync(BeUniqueTitleAsync).WithMessage("The specified title already exists.");
    }

    private bool BeUniqueTitle(string title)
    {
        return _context.TodoLists.All(l => l.Title != title);
    }

    // public async Task<bool> BeUniqueTitleAsync(string title, CancellationToken cancellationToken)
    // {
    //     return await _context.TodoLists
    //         .AllAsync(l => l.Title != title, cancellationToken);
    // }
}
