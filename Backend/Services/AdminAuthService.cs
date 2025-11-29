using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NileCareAPI.DTOs;
using NileCareAPI.Models;

namespace NileCareAPI.Services;

public class AdminAuthService : IAdminAuthService
{
    private readonly UserManager<AdminUser> _userManager;
    private readonly SignInManager<AdminUser> _signInManager;
    private readonly IConfiguration _configuration;
    private const string ADMIN_SECRET = "NileCareAdmin2025!"; // In production, store in config

    public AdminAuthService(
        UserManager<AdminUser> userManager,
        SignInManager<AdminUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<AdminDto?> RegisterAsync(AdminRegisterDto registerDto)
    {
        // Verify admin secret
        if (registerDto.AdminSecret != ADMIN_SECRET)
            return null;

        var admin = new AdminUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(admin, registerDto.Password);

        if (!result.Succeeded)
            return null;

        // Add Admin role
        await _userManager.AddToRoleAsync(admin, "Admin");

        var token = GenerateJwtToken(admin);

        return new AdminDto
        {
            Id = admin.Id,
            Email = admin.Email ?? string.Empty,
            FirstName = admin.FirstName,
            LastName = admin.LastName,
            ProfilePicture = admin.ProfilePicture,
            Token = token
        };
    }

    public async Task<AdminDto?> LoginAsync(AdminLoginDto loginDto)
    {
        var admin = await _userManager.FindByEmailAsync(loginDto.Email);
        if (admin == null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(admin, loginDto.Password, false);
        if (!result.Succeeded)
            return null;

        // Update last login
        admin.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(admin);

        var token = GenerateJwtToken(admin);

        return new AdminDto
        {
            Id = admin.Id,
            Email = admin.Email ?? string.Empty,
            FirstName = admin.FirstName,
            LastName = admin.LastName,
            ProfilePicture = admin.ProfilePicture,
            Token = token
        };
    }

    private string GenerateJwtToken(AdminUser admin)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, admin.Id),
            new Claim(ClaimTypes.Email, admin.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
