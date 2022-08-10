using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.DeleteTodoList;
using CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Filters;

namespace RazorPages.Pages.TodoLists;

[RazorPageExceptionFilter]
public class Index : PageModel
{
    #region properties
    
    public TodosVm TodosVm { get; set; } = new();
    
    [BindProperty]
    public CreateTodoListCommand CreateTodoListCommandModel { get; set; }
    
    [BindProperty]
    public UpdateTodoListCommand UpdateTodoListCommandModel { get; set; }
    
    [BindProperty]
    public DeleteTodoListCommand DeleteTodoListCommandModel { get; set; }

    #endregion
    
    private readonly IMediator _mediator;

    public Index(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<IActionResult> OnGetAsync()
    {
        TodosVm = await _mediator.Send(new GetTodosQuery());

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _mediator.Send(CreateTodoListCommandModel);
        
        return RedirectToPage("/TodoLists/Index");
    }

    public async Task<IActionResult> OnPostUpdateAsync()
    {
        await _mediator.Send(UpdateTodoListCommandModel);
        
        return RedirectToPage("/TodoLists/Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        await _mediator.Send(DeleteTodoListCommandModel);
        
        return RedirectToPage("/TodoLists/Index");
    }
}