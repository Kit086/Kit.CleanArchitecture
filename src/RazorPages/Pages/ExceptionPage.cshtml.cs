using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Filters;

namespace RazorPages.Pages;

public class ExceptionPage : PageModel
{
    
    public FriendlyExceptionDetails FriendlyExceptionDetails { get; set; }
    
    public void OnGet(FriendlyExceptionDetails friendlyExceptionDetails)
    {
        FriendlyExceptionDetails = friendlyExceptionDetails;
    }
}