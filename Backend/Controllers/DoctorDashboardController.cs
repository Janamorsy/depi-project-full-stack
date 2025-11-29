using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileCareAPI.DTOs;
using NileCareAPI.Services;

namespace NileCareAPI.Controllers;



[Authorize(Roles = "Doctor")]
[ApiController]
[Route("api/doctor")]
public class DoctorDashboardController : ControllerBase
{
    private readonly IDoctorDashboardService _dashboardService;

    public DoctorDashboardController(IDoctorDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }



    [HttpGet("appointments")]
    [ProducesResponseType(typeof(List<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppointments()
    {
        var appointments = await _dashboardService.GetDoctorAppointmentsAsync();
        return Ok(appointments);
    }



    [HttpGet("patient/{patientId}/profile")]
    [ProducesResponseType(typeof(MedicalProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientProfile(string patientId)
    {
        var profile = await _dashboardService.GetPatientProfileAsync(patientId);
        if (profile == null)
            return NotFound();

        return Ok(profile);
    }



    [HttpPut("appointment/{appointmentId}/notes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateNotes(int appointmentId, [FromBody] UpdateNotesDto dto)
    {
        await _dashboardService.UpdateAppointmentNotesAsync(appointmentId, dto.DoctorNotes);
        return Ok();
    }



    [HttpPut("appointment/{appointmentId}/status")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, [FromBody] UpdateAppointmentStatusDto dto)
    {
        var appointment = await _dashboardService.UpdateAppointmentStatusAsync(appointmentId, dto.Status);
        if (appointment == null)
            return NotFound("Appointment not found or you don't have permission to update it.");
        return Ok(appointment);
    }



    [HttpDelete("appointment/{appointmentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAppointment(int appointmentId)
    {
        var result = await _dashboardService.DeleteAppointmentAsync(appointmentId);
        if (!result)
            return NotFound("Appointment not found or you don't have permission to delete it.");
        return Ok(new { message = "Appointment deleted successfully" });
    }
}

public class UpdateNotesDto
{
    public string DoctorNotes { get; set; } = string.Empty;
}

public class UpdateAppointmentStatusDto
{
    public string Status { get; set; } = string.Empty;
}

