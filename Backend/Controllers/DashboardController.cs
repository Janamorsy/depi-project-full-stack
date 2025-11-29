using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;



[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }




    [HttpGet]
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = await _dashboardService.GetDashboardDataAsync();
        return Ok(dashboard);
    }
}


