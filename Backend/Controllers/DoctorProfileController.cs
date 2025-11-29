using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NileCareAPI.Data;
using NileCareAPI.Models;
using System.Security.Claims;

namespace NileCareAPI.Controllers;

[Authorize(Roles = "Doctor")]
[ApiController]
[Route("api/doctor/profile")]
public class DoctorProfileController : ControllerBase
{
    private readonly UserManager<DoctorUser> _userManager;
    private readonly NileCareDbContextV2 _context;

    public DoctorProfileController(UserManager<DoctorUser> userManager, NileCareDbContextV2 context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (doctorId == null) return Unauthorized();

        var doctor = await _context.DoctorUsers
            .Include(d => d.AvailabilitySlots)
            .FirstOrDefaultAsync(d => d.Id == doctorId);
        if (doctor == null) return NotFound();

        return Ok(new
        {
            id = doctor.Id,
            email = doctor.Email,
            firstName = doctor.FirstName,
            lastName = doctor.LastName,
            phoneNumber = doctor.PhoneNumber,
            specialty = doctor.Specialty,
            specialtyTags = doctor.SpecialtyTags,
            hospital = doctor.Hospital,
            city = doctor.City,
            languages = doctor.Languages,
            consultationFee = doctor.ConsultationFee,
            yearsOfExperience = doctor.YearsOfExperience,
            bio = doctor.Bio,
            imageUrl = doctor.ImageUrl,
            isAvailable = doctor.IsAvailable,
            availabilitySlots = doctor.AvailabilitySlots?.Select(s => new
            {
                id = s.Id,
                day = (int)s.Day,
                start = s.Start.ToString(@"hh\:mm"),
                end = s.End.ToString(@"hh\:mm")
            }).ToList()
        });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateDoctorProfileDto dto)
    {
        var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (doctorId == null) return Unauthorized();

        var doctor = await _userManager.FindByIdAsync(doctorId);
        if (doctor == null) return NotFound();

        doctor.FirstName = dto.FirstName;
        doctor.LastName = dto.LastName;
        doctor.PhoneNumber = dto.PhoneNumber;
        doctor.Specialty = dto.Specialty;
        doctor.SpecialtyTags = dto.SpecialtyTags;
        doctor.Hospital = dto.Hospital;
        doctor.City = dto.City;
        doctor.Languages = dto.Languages;
        doctor.ConsultationFee = dto.ConsultationFee;
        doctor.YearsOfExperience = dto.YearsOfExperience;
        doctor.Bio = dto.Bio;
        doctor.IsAvailable = dto.IsAvailable;
        
        if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
            doctor.ImageUrl = dto.ImageUrl;

        var result = await _userManager.UpdateAsync(doctor);
        if (!result.Succeeded)
            return BadRequest(new { message = "Failed to update profile" });

        return Ok(new { message = "Profile updated successfully" });
    }

    [HttpPut("availability")]
    public async Task<IActionResult> UpdateAvailability([FromBody] UpdateAvailabilityDto dto)
    {
        var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (doctorId == null) return Unauthorized();

        var doctor = await _context.DoctorUsers
            .Include(d => d.AvailabilitySlots)
            .FirstOrDefaultAsync(d => d.Id == doctorId);
        
        if (doctor == null) return NotFound();

        // Clear existing slots
        if (doctor.AvailabilitySlots != null)
        {
            doctor.AvailabilitySlots = new List<AvailabilitySlot>();
        }

        // Add new slots
        var newSlots = new List<AvailabilitySlot>();
        foreach (var slot in dto.Slots)
        {
            if (TimeSpan.TryParse(slot.Start, out var start) && TimeSpan.TryParse(slot.End, out var end))
            {
                newSlots.Add(new AvailabilitySlot
                {
                    Day = (DayOfWeek)slot.Day,
                    Start = start,
                    End = end,
                    DoctorUserId = doctorId
                });
            }
        }

        doctor.AvailabilitySlots = newSlots;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Availability updated successfully" });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAccount()
    {
        var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (doctorId == null) return Unauthorized();

        var doctor = await _userManager.FindByIdAsync(doctorId);
        if (doctor == null) return NotFound();

        var result = await _userManager.DeleteAsync(doctor);
        if (!result.Succeeded)
            return BadRequest(new { message = "Failed to delete account" });

        return Ok(new { message = "Account deleted successfully" });
    }
}

public class UpdateDoctorProfileDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string SpecialtyTags { get; set; } = string.Empty;
    public string Hospital { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public int YearsOfExperience { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}

public class UpdateAvailabilityDto
{
    public List<AvailabilitySlotDto> Slots { get; set; } = new();
}

public class AvailabilitySlotDto
{
    public int Day { get; set; }
    public string Start { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
}

