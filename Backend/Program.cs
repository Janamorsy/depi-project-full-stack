using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NileCareAPI.Data;
using NileCareAPI.Models;
using NileCareAPI.Repositories;
using NileCareAPI.Services;
using Stripe; // Required for StripeConfiguration
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NileCareDbContextV2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<NileCareDbContextV2>()
.AddDefaultTokenProviders();

builder.Services.AddIdentityCore<DoctorUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<NileCareDbContextV2>()
.AddSignInManager<SignInManager<DoctorUser>>()
.AddDefaultTokenProviders();

builder.Services.AddIdentityCore<AdminUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<NileCareDbContextV2>()
.AddSignInManager<SignInManager<AdminUser>>()
.AddDefaultTokenProviders();

builder.Services.AddIdentityCore<HotelUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<NileCareDbContextV2>()
.AddSignInManager<SignInManager<HotelUser>>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IMedicalProfileRepository, MedicalProfileRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<ITransportRepository, TransportRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IHotelBookingRepository, HotelBookingRepository>();
builder.Services.AddScoped<ITransportBookingRepository, TransportBookingRepository>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IDoctorUserRepository, DoctorUserRepository>();

// --- NEW STRIPE SERVICE REGISTRATION ---
builder.Services.AddScoped<IStripeService, StripeService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDoctorAuthService, DoctorAuthService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IMedicalProfileService, MedicalProfileService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IDoctorDashboardService, DoctorDashboardService>();
builder.Services.AddScoped<IAdminAuthService, AdminAuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IHotelAuthService, HotelAuthService>();
builder.Services.AddScoped<IHotelOwnerService, HotelOwnerService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});
////////////////////////////////////////////////////////////////////
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NileCare API V2",
        Version = "v2",
        Description = "Intelligent Medical Tourism Platform with Booking, Chat, and Medical Records",
        Contact = new OpenApiContact
        {
            Name = "NileCare Support",
            Email = "support@nilecare.com"
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token in the format: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

StripeConfiguration.ApiKey = builder.Configuration["StripeSettings:SecretKey"];

var app = builder.Build();

// Apply pending migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NileCareDbContextV2>();
    await db.Database.MigrateAsync();
}

// Seed Admin role and default admin user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("Doctor"))
    {
        await roleManager.CreateAsync(new IdentityRole("Doctor"));
    }
    if (!await roleManager.RoleExistsAsync("HotelOwner"))
    {
        await roleManager.CreateAsync(new IdentityRole("HotelOwner"));
    }
    
    // Seed default admin user
    // Note: AdminUser is stored in a separate table (AdminUsers), not AspNetUsers.
    // Role assignment via Identity's AddToRoleAsync won't work because it expects 
    // users in AspNetUsers. Admin authentication is handled by checking if the user
    // exists in the AdminUsers table, not by role claims.
    var adminUserManager = scope.ServiceProvider.GetRequiredService<UserManager<AdminUser>>();
    var adminEmail = "admin@nilecare.com";
    var existingAdmin = await adminUserManager.FindByEmailAsync(adminEmail);
    if (existingAdmin == null)
    {
        var adminUser = new AdminUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Admin",
            CreatedAt = DateTime.UtcNow
        };
        await adminUserManager.CreateAsync(adminUser, "Admin123!");
    }
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "NileCare API V2");
    options.RoutePrefix = "swagger";
});

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads"
});

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
