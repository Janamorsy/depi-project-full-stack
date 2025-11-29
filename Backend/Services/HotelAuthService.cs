using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NileCareAPI.DTOs;
using NileCareAPI.Models;

namespace NileCareAPI.Services;

public class HotelAuthService : IHotelAuthService
{
    private readonly UserManager<HotelUser> _userManager;
    private readonly SignInManager<HotelUser> _signInManager;
    private readonly IConfiguration _configuration;

    public HotelAuthService(
        UserManager<HotelUser> userManager,
        SignInManager<HotelUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<HotelUserDto?> RegisterAsync(HotelUserRegisterDto registerDto)
    {
        var hotelUser = new HotelUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            CompanyName = registerDto.CompanyName,
            PhoneNumber = registerDto.PhoneNumber,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(hotelUser, registerDto.Password);

        if (!result.Succeeded)
            return null;

        var token = GenerateJwtToken(hotelUser);

        return new HotelUserDto
        {
            Id = hotelUser.Id,
            Email = hotelUser.Email ?? string.Empty,
            FirstName = hotelUser.FirstName,
            LastName = hotelUser.LastName,
            CompanyName = hotelUser.CompanyName,
            PhoneNumber = hotelUser.PhoneNumber ?? string.Empty,
            ProfilePicture = hotelUser.ProfilePicture,
            Token = token
        };
    }

    public async Task<HotelUserDto?> LoginAsync(LoginDto loginDto)
    {
        var hotelUser = await _userManager.FindByEmailAsync(loginDto.Email);
        if (hotelUser == null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(hotelUser, loginDto.Password, false);
        if (!result.Succeeded)
            return null;

        // Update last login
        hotelUser.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(hotelUser);

        var token = GenerateJwtToken(hotelUser);

        return new HotelUserDto
        {
            Id = hotelUser.Id,
            Email = hotelUser.Email ?? string.Empty,
            FirstName = hotelUser.FirstName,
            LastName = hotelUser.LastName,
            CompanyName = hotelUser.CompanyName,
            PhoneNumber = hotelUser.PhoneNumber ?? string.Empty,
            ProfilePicture = hotelUser.ProfilePicture,
            Token = token
        };
    }

    private string GenerateJwtToken(HotelUser hotelUser)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, hotelUser.Id),
            new Claim(ClaimTypes.Email, hotelUser.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, $"{hotelUser.FirstName} {hotelUser.LastName}"),
            new Claim(ClaimTypes.Role, "HotelOwner"),
            new Claim("CompanyName", hotelUser.CompanyName)
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
