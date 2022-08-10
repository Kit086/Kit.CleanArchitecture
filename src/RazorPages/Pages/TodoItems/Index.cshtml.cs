using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.DeleteTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItemDetail;
using CleanArchitecture.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.TodoItems;

public class Index : PageModel
{
    #region properties

    public PaginatedList<TodoItemBriefDto> TodoItemBriefDtoPaginatedList { get; set; }
    
    [BindProperty]
    public int ListId { get; set; }

    // public TodosVm AllTodoLists { get; set; }

    [BindProperty]
    public CreateTodoItemCommand CreateTodoItemCommandModel { get; set; }
    
    [BindProperty]
    public UpdateTodoItemCommand UpdateTodoItemCommandModel { get; set; }
    
    [BindProperty]
    public UpdateTodoItemDetailCommand UpdateTodoItemDetailCommandModel { get; set; }
    
    [BindProperty]
    public DeleteTodoItemCommand DeleteTodoItemCommandModel { get; set; }

    [BindProperty]
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    #endregion

    private readonly IMediator _mediator;

    public Index(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> OnGetAsync(int listId)
    {
        ListId = listId;
        
        TodoItemBriefDtoPaginatedList = await _mediator.Send(new GetTodoItemsWithPaginationQuery
        {
            ListId = listId, PageNumber = PageNumber, PageSize = PageSize
        });

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _mediator.Send(CreateTodoItemCommandModel);

        return RedirectToPage("/TodoItems/Index", new { listId = CreateTodoItemCommandModel.ListId });
    }

    public async Task<IActionResult> OnPostUpdateAsync()
    {
        await _mediator.Send(UpdateTodoItemCommandModel);

        return RedirectToPage("/TodoItems/Index", new { listId = ListId });
    }

    public async Task<IActionResult> OnPostUpdateDetailAsync()
    {
        await _mediator.Send(UpdateTodoItemDetailCommandModel);
        
        return RedirectToPage("/TodoItems/Index", new { listId = UpdateTodoItemDetailCommandModel.ListId });
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        await _mediator.Send(DeleteTodoItemCommandModel);
        
        return RedirectToPage("/TodoItems/Index", new { listId = ListId });
    }
}