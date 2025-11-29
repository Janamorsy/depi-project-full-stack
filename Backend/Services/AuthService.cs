using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NileCareAPI.DTOs;
using NileCareAPI.Models;

namespace NileCareAPI.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<UserDto?> RegisterAsync(RegisterDto registerDto)
    {
        var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PhoneNumber = registerDto.PhoneNumber,
            ProfilePicture = string.IsNullOrWhiteSpace(registerDto.ProfilePicture) 
                ? "/images/default-avatar.png" 
                : registerDto.ProfilePicture,
            IsWheelchairAccessible = registerDto.IsWheelchairAccessible
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return null;

        var token = GenerateJwtToken(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePicture,
            PhoneNumber = user.PhoneNumber,
            IsWheelchairAccessible = user.IsWheelchairAccessible,
            Token = token
        };
    }

    public async Task<UserDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
            return null;

        var token = GenerateJwtToken(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePicture,
            PhoneNumber = user.PhoneNumber,
            IsWheelchairAccessible = user.IsWheelchairAccessible,
            Token = token
        };
    }

    public async Task<UserDto?> UpdateProfileAsync(string userId, UpdateProfileDto updateDto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return null;

        if (!string.IsNullOrWhiteSpace(updateDto.FirstName))
            user.FirstName = updateDto.FirstName;
        
        if (!string.IsNullOrWhiteSpace(updateDto.LastName))
            user.LastName = updateDto.LastName;
        
        if (!string.IsNullOrWhiteSpace(updateDto.PhoneNumber))
            user.PhoneNumber = updateDto.PhoneNumber;
        
        if (!string.IsNullOrWhiteSpace(updateDto.ProfilePicture))
            user.ProfilePicture = updateDto.ProfilePicture;
        
        if (updateDto.IsWheelchairAccessible.HasValue)
            user.IsWheelchairAccessible = updateDto.IsWheelchairAccessible.Value;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return null;

        var token = GenerateJwtToken(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePicture,
            PhoneNumber = user.PhoneNumber,
            IsWheelchairAccessible = user.IsWheelchairAccessible,
            Token = token
        };
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
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


