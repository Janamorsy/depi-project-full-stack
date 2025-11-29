using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NileCareAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsWheelchairAccessible = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialtyTags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hospital = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsultationFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialtyTags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hospital = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsultationFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    WheelchairAccessible = table.Column<bool>(type: "bit", nullable: false),
                    RollInShower = table.Column<bool>(type: "bit", nullable: false),
                    ElevatorAccess = table.Column<bool>(type: "bit", nullable: false),
                    GrabBars = table.Column<bool>(type: "bit", nullable: false),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WheelchairAccessible = table.Column<bool>(type: "bit", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    PricePerHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Features = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicalConditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessibilityNeeds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreatmentHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsentGiven = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalRecords_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LegacyDoctorId = table.Column<int>(type: "int", nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsultationFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_DoctorUsers_DoctorUserId",
                        column: x => x.DoctorUserId,
                        principalTable: "DoctorUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_LegacyDoctorId",
                        column: x => x.LegacyDoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessages_DoctorUsers_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Doctors_AvailabilitySlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
                    DoctorUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    DoctorId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors_AvailabilitySlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctors_AvailabilitySlots_DoctorUsers_DoctorUserId",
                        column: x => x.DoctorUserId,
                        principalTable: "DoctorUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Doctors_AvailabilitySlots_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doctors_AvailabilitySlots_Doctors_DoctorId1",
                        column: x => x.DoctorId1,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DoctorUsers_AvailabilitySlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
                    DoctorUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorUserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DoctorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorUsers_AvailabilitySlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorUsers_AvailabilitySlots_DoctorUsers_DoctorUserId",
                        column: x => x.DoctorUserId,
                        principalTable: "DoctorUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorUsers_AvailabilitySlots_DoctorUsers_DoctorUserId1",
                        column: x => x.DoctorUserId1,
                        principalTable: "DoctorUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoctorUsers_AvailabilitySlots_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HotelBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomRatePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialRequests = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelBookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelBookings_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransportBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransportId = table.Column<int>(type: "int", nullable: false),
                    PickupDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickupLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DropoffLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPassengers = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialRequests = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportBookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransportBookings_Transports_TransportId",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsWheelchairAccessible", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "stefan-mueller-id", 0, "2fc6316c-fd7c-4271-8adf-def65d8221c2", "stefan.mueller@email.com", true, "Stefan", false, "Mueller", false, null, "STEFAN.MUELLER@EMAIL.COM", "STEFAN.MUELLER@EMAIL.COM", "AQAAAAIAAYagAAAAEGnRlqbAwY6gnrucES84uM9Pvz0nUOxh8vEyKAuYEivdPLDhJxyj2Nds/kUndnXe7Q==", "+20 123 456 7890", false, "/images/default-avatar.png", "8f23e76a-3dc4-4a2b-b684-bf59814bed64", false, "stefan.mueller@email.com" });

            migrationBuilder.InsertData(
                table: "DoctorUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bio", "City", "ConcurrencyStamp", "ConsultationFee", "Email", "EmailConfirmed", "FirstName", "Hospital", "ImageUrl", "IsAvailable", "Languages", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Specialty", "SpecialtyTags", "TwoFactorEnabled", "UserName", "YearsOfExperience" },
                values: new object[] { "dr-ahmed-hassan-id", 0, "Leading cardiac electrophysiologist specializing in atrial fibrillation treatment", "Cairo", "859f7c8f-6b84-4976-adb7-ebf6584be5d0", 500m, "dr.ahmed@nileheart.com", true, "Ahmed", "Cairo Heart Institute", "/images/doctors/dr-ahmed-hassan.jpg", true, "Arabic,English,German", "Hassan", false, null, "DR.AHMED@NILEHEART.COM", "DR.AHMED@NILEHEART.COM", "AQAAAAIAAYagAAAAEA0+rjvgKrc0g9t0k/uEgWy/lHf03kbT5pY3fHU6d6Fpfw7hVqxUWeYhI0G0MB68Gw==", "+20 100 234 5678", false, "0c6fe827-1e49-4593-96bf-b4bc90e3b5e4", "Cardiology", "Atrial Fibrillation,Cardiac Ablation,Heart Arrhythmia,Cardiovascular", false, "dr.ahmed@nileheart.com", 18 });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "Bio", "City", "ConsultationFee", "Hospital", "ImageUrl", "Languages", "Name", "Rating", "Specialty", "SpecialtyTags", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, "Leading cardiac electrophysiologist", "Cairo", 500m, "Cairo Heart Institute", "/images/doctors/dr-ahmed-hassan.jpg", "Arabic,English,German", "Dr. Ahmed Hassan", 4.9000000000000004, "Cardiology", "Atrial Fibrillation,Cardiac Ablation,Heart Arrhythmia,Cardiovascular", 18 },
                    { 2, "Experienced cardiologist", "Cairo", 450m, "Nile Medical Center", "/images/doctors/dr-fatima-elsayed.jpg", "Arabic,English,French", "Dr. Fatima El-Sayed", 4.7000000000000002, "Cardiology", "Heart Disease,Cardiology,Preventive Cardiology", 15 }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "Amenities", "City", "Description", "ElevatorAccess", "GrabBars", "ImageUrl", "Name", "PricePerNight", "Rating", "RollInShower", "WheelchairAccessible" },
                values: new object[,]
                {
                    { 1, "12 Corniche El Nil, Downtown Cairo", "Free WiFi,Pool,Gym,Restaurant,Room Service", "Cairo", "Luxury hotel with full accessibility", true, true, "/images/hotels/nile-plaza.jpg", "Nile Plaza Hotel", 120m, 4.5, true, true },
                    { 2, "45 Tahrir Square, Cairo", "Free WiFi,Restaurant", "Cairo", "Comfortable hotel in central Cairo", true, true, "/images/hotels/cairo-grand.jpg", "Cairo Grand", 95m, 4.2000000000000002, false, true }
                });

            migrationBuilder.InsertData(
                table: "Transports",
                columns: new[] { "Id", "Capacity", "Description", "Features", "ImageUrl", "PricePerHour", "VehicleType", "WheelchairAccessible" },
                values: new object[,]
                {
                    { 1, 4, "Spacious van with hydraulic lift", "Hydraulic Lift,Wheelchair Locks,Air Conditioning", "/images/transport/wheelchair-van.jpg", 25m, "Wheelchair Accessible Van", true },
                    { 2, 4, "Comfortable sedan", "Air Conditioning,Professional Driver", "/images/transport/sedan.jpg", 15m, "Standard Sedan", false },
                    { 3, 6, "Premium SUV with extra space", "Leather Seats,Air Conditioning,WiFi,Professional Driver", "/images/transport/suv.jpg", 35m, "Luxury SUV", false },
                    { 4, 2, "Lightweight, easy to maneuver, ideal for quick trips.", "Compact storage space, easy parking.", "/images/transport/OIP.webp", 10m, "Scooter", false }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDate", "AppointmentType", "ConsultationFee", "CreatedAt", "DoctorNotes", "DoctorUserId", "LegacyDoctorId", "PatientNotes", "PaymentStatus", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 100, new DateTime(2025, 11, 20, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(727), "In-Person", 500m, new DateTime(2025, 11, 17, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(740), "Patient responding well to medication. ECG shows improvement.", "dr-ahmed-hassan-id", null, "Follow-up for atrial fibrillation treatment", "Paid", "Completed", new DateTime(2025, 11, 20, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(741), "stefan-mueller-id" },
                    { 101, new DateTime(2025, 11, 13, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(746), "In-Person", 450m, new DateTime(2025, 11, 10, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(747), "Comprehensive cardiac evaluation completed. Recommended specialist referral.", null, 2, "Initial consultation", "Paid", "Completed", new DateTime(2025, 11, 13, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(748), "stefan-mueller-id" },
                    { 102, new DateTime(2025, 12, 2, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(749), "In-Person", 500m, new DateTime(2025, 11, 25, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(750), "", "dr-ahmed-hassan-id", null, "Pre-procedure consultation", "Unpaid", "Confirmed", null, "stefan-mueller-id" }
                });

            migrationBuilder.InsertData(
                table: "DoctorUsers_AvailabilitySlots",
                columns: new[] { "Id", "Day", "DoctorId", "DoctorUserId", "DoctorUserId1", "End", "Start" },
                values: new object[,]
                {
                    { 1, 1, null, "dr-ahmed-hassan-id", null, new TimeSpan(0, 14, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) },
                    { 2, 3, null, "dr-ahmed-hassan-id", null, new TimeSpan(0, 16, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Doctors_AvailabilitySlots",
                columns: new[] { "Id", "Day", "DoctorId", "DoctorId1", "DoctorUserId", "End", "Start" },
                values: new object[,]
                {
                    { 1, 1, 1, null, null, new TimeSpan(0, 14, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) },
                    { 2, 3, 1, null, null, new TimeSpan(0, 16, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) },
                    { 3, 0, 2, null, null, new TimeSpan(0, 12, 30, 0, 0), new TimeSpan(0, 8, 30, 0, 0) },
                    { 4, 2, 2, null, null, new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 11, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "HotelBookings",
                columns: new[] { "Id", "CheckInDate", "CheckOutDate", "CreatedAt", "HotelId", "NumberOfGuests", "PaymentStatus", "RoomRatePerNight", "RoomType", "SpecialRequests", "Status", "UserId" },
                values: new object[,]
                {
                    { 100, new DateTime(2025, 11, 17, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(798), new DateTime(2025, 11, 20, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(802), new DateTime(2025, 11, 12, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(804), 1, 2, "Paid", 120m, "Deluxe Accessible Room", "Ground floor room with roll-in shower", "Completed", "stefan-mueller-id" },
                    { 101, new DateTime(2025, 11, 30, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(806), new DateTime(2025, 12, 4, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(807), new DateTime(2025, 11, 24, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(808), 2, 1, "Unpaid", 95m, "Standard Accessible Room", "Near elevator", "Confirmed", "stefan-mueller-id" }
                });

            migrationBuilder.InsertData(
                table: "MedicalProfiles",
                columns: new[] { "Id", "AccessibilityNeeds", "ConsentGiven", "CreatedAt", "MedicalConditions", "TreatmentHistory", "UpdatedAt", "UserId" },
                values: new object[] { 1, "Wheelchair access required", true, new DateTime(2025, 11, 27, 16, 19, 11, 383, DateTimeKind.Utc).AddTicks(3370), "Atrial Fibrillation", "Diagnosed 2023, currently on blood thinners", new DateTime(2025, 11, 27, 16, 19, 11, 383, DateTimeKind.Utc).AddTicks(3371), "stefan-mueller-id" });

            migrationBuilder.InsertData(
                table: "TransportBookings",
                columns: new[] { "Id", "CreatedAt", "DropoffLocation", "NumberOfPassengers", "PaymentStatus", "PickupDateTime", "PickupLocation", "SpecialRequests", "Status", "TotalPrice", "TransportId", "UserId" },
                values: new object[,]
                {
                    { 100, new DateTime(2025, 11, 12, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(856), "Nile Plaza Hotel", 2, "Paid", new DateTime(2025, 11, 17, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(849), "Cairo International Airport", "Wheelchair assistance needed", "Completed", 50m, 1, "stefan-mueller-id" },
                    { 101, new DateTime(2025, 11, 19, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(861), "Cairo Heart Institute", 1, "Paid", new DateTime(2025, 11, 20, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(859), "Nile Plaza Hotel", "Wheelchair accessible required", "Completed", 50m, 1, "stefan-mueller-id" },
                    { 102, new DateTime(2025, 11, 25, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(863), "Cairo Heart Institute", 1, "Unpaid", new DateTime(2025, 12, 2, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(862), "Cairo Grand Hotel", "", "Confirmed", 30m, 2, "stefan-mueller-id" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorUserId",
                table: "Appointments",
                column: "DoctorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_LegacyDoctorId",
                table: "Appointments",
                column: "LegacyDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_DoctorId",
                table: "ChatMessages",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_PatientId",
                table: "ChatMessages",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_AvailabilitySlots_DoctorId",
                table: "Doctors_AvailabilitySlots",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_AvailabilitySlots_DoctorId1",
                table: "Doctors_AvailabilitySlots",
                column: "DoctorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_AvailabilitySlots_DoctorUserId",
                table: "Doctors_AvailabilitySlots",
                column: "DoctorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorUsers_AvailabilitySlots_DoctorId",
                table: "DoctorUsers_AvailabilitySlots",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorUsers_AvailabilitySlots_DoctorUserId",
                table: "DoctorUsers_AvailabilitySlots",
                column: "DoctorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorUsers_AvailabilitySlots_DoctorUserId1",
                table: "DoctorUsers_AvailabilitySlots",
                column: "DoctorUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookings_HotelId",
                table: "HotelBookings",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookings_UserId",
                table: "HotelBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalProfiles_UserId",
                table: "MedicalProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_UserId",
                table: "MedicalRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportBookings_TransportId",
                table: "TransportBookings",
                column: "TransportId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportBookings_UserId",
                table: "TransportBookings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Doctors_AvailabilitySlots");

            migrationBuilder.DropTable(
                name: "DoctorUsers_AvailabilitySlots");

            migrationBuilder.DropTable(
                name: "HotelBookings");

            migrationBuilder.DropTable(
                name: "MedicalProfiles");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "TransportBookings");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DoctorUsers");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Transports");
        }
    }
}
