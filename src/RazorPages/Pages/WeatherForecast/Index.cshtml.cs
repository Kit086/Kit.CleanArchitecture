using CleanArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.WeatherForecast;

public class Index : PageModel
{
    public IEnumerable<CleanArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts.WeatherForecast> WeatherForecasts { get; set; }
    
    readonly IMediator _mediator;

    public Index(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<IActionResult> OnGetAsync()
    {
        WeatherForecasts = await _mediator.Send(new GetWeatherForecastsQuery());
        
        return Page();
    }
}