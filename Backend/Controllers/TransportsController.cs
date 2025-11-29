using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.Models;
using NileCareAPI.Repositories;

namespace NileCareAPI.Controllers;



[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransportsController : ControllerBase
{
    private readonly ITransportRepository _transportRepository;

    public TransportsController(ITransportRepository transportRepository)
    {
        _transportRepository = transportRepository;
    }




    [HttpGet]
    [ProducesResponseType(typeof(List<Transport>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTransports()
    {
        var transports = await _transportRepository.GetAllAsync();
        return Ok(transports);
    }




    [HttpGet("accessible")]
    [ProducesResponseType(typeof(List<Transport>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAccessibleTransports()
    {
        var transports = await _transportRepository.GetAccessibleAsync();
        return Ok(transports);
    }
}


