using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;



[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMedicalRecordService _medicalRecordService;

    public MedicalRecordsController(IMedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }



    [HttpPost("upload")]
    [ProducesResponseType(typeof(MedicalRecordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string description, [FromForm] string category)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var uploadDto = new UploadMedicalRecordDto
        {
            Description = description,
            Category = category
        };

        var record = await _medicalRecordService.UploadRecordAsync(file, uploadDto);
        return Ok(record);
    }



    [HttpGet]
    [ProducesResponseType(typeof(List<MedicalRecordDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecords()
    {
        var records = await _medicalRecordService.GetUserRecordsAsync();
        return Ok(records);
    }



    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Doctor")]
    [ProducesResponseType(typeof(List<MedicalRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPatientRecords(string patientId)
    {
        var records = await _medicalRecordService.GetPatientRecordsAsync(patientId);
        return Ok(records);
    }



    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRecord(int id)
    {
        await _medicalRecordService.DeleteRecordAsync(id);
        return Ok();
    }
}


