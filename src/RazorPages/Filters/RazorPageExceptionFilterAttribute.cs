using CleanArchitecture.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RazorPages.Filters;

public class RazorPageExceptionFilterAttribute : Attribute, IPageFilter
{
    private readonly IDictionary<Type, Action<PageHandlerExecutedContext>> _exceptionHandlers;

    public RazorPageExceptionFilterAttribute()
    {
        // Register known exception types and handlers
        _exceptionHandlers = new Dictionary<Type, Action<PageHandlerExecutedContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException }
        };
    }

    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
    }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
        if (context.Exception is not null)
        {
            HandleException(context);
        }
    }

    private void HandleException(PageHandlerExecutedContext context)
    {
        Type type = context.Exception!.GetType();

        if (_exceptionHandlers.TryGetValue(type, out var action))
        {
            action.Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }
    }

    private void HandleValidationException(PageHandlerExecutedContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleInvalidModelStateException(PageHandlerExecutedContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(PageHandlerExecutedContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(PageHandlerExecutedContext context)
    {
        var friendlyExceptionDetails = new FriendlyExceptionDetails(StatusCodes.Status401Unauthorized, 
            "Unauthorized",
            "Please register or log in");
        
        context.ExceptionHandled = true;

        context.Result = new RedirectToPageResult("/ExceptionPage", friendlyExceptionDetails);
    }

    private void HandleForbiddenAccessException(PageHandlerExecutedContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        };

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status403Forbidden };

        context.ExceptionHandled = true;
    }
}

public record FriendlyExceptionDetails(int Status, string Title, string Message);