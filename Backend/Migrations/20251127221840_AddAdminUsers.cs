using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileCareAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4723), new DateTime(2025, 11, 17, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4742), new DateTime(2025, 11, 20, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4743) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4749), new DateTime(2025, 11, 10, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4751), new DateTime(2025, 11, 13, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4751) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 12, 2, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4753), new DateTime(2025, 11, 25, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4754) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "stefan-mueller-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7051e618-163a-4f95-91e3-f6d98bc0c65c", "AQAAAAIAAYagAAAAEHiw9c6GN7YbTZ4fu3KmEihM2ulGuu3qjJgGlsBXHcX5TinDuqWkIZ+7MxZt7LYx0A==", "bee1a959-4b0f-477d-b053-1120d2537481" });

            migrationBuilder.UpdateData(
                table: "DoctorUsers",
                keyColumn: "Id",
                keyValue: "dr-ahmed-hassan-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a6cb15ee-32ee-40be-b379-d1d349685ae7", "AQAAAAIAAYagAAAAECnnjM2M/12Ub+9rh+H1d5YljdfePN3C8Pe6imzPzit0dGe5QBKAjSO+f4jVQyg4Rw==", "442a16cf-718d-4f53-a9af-dc2c33c7993e" });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4796), new DateTime(2025, 11, 20, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4797), new DateTime(2025, 11, 12, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4800) });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 30, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4802), new DateTime(2025, 12, 4, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4802), new DateTime(2025, 11, 24, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4804) });

            migrationBuilder.UpdateData(
                table: "MedicalProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 27, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4250), new DateTime(2025, 11, 27, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4250) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 12, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4839), new DateTime(2025, 11, 17, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4836) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 19, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4843), new DateTime(2025, 11, 20, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4841) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 25, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4846), new DateTime(2025, 12, 2, 22, 18, 39, 896, DateTimeKind.Utc).AddTicks(4845) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(892), new DateTime(2025, 11, 17, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(922), new DateTime(2025, 11, 20, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(923) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(933), new DateTime(2025, 11, 10, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(935), new DateTime(2025, 11, 13, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(935) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 12, 2, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(937), new DateTime(2025, 11, 25, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(938) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "stefan-mueller-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "51e9a4cd-53e6-4670-b4da-ca766e49003f", "AQAAAAIAAYagAAAAEF9LcZVmdMcRvBoZuMCun5qiuGWGq8ID66aal8Pryu1Mq/Nb3u1g6v9g1Dupgpy+Jw==", "80a10a0a-c7d3-44e9-ac66-5474b496b211" });

            migrationBuilder.UpdateData(
                table: "DoctorUsers",
                keyColumn: "Id",
                keyValue: "dr-ahmed-hassan-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4b9cb515-b7f9-4fc4-8926-a64a1c4b6d4e", "AQAAAAIAAYagAAAAELXtPCeBGQ2ggaHs4rrXACcZvRQ7/XP4lup/Dvrb/hpcvSnU60wTZcNxHHJqhr7rxQ==", "ab1c8244-7016-46a4-82f5-568c87236389" });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1028), new DateTime(2025, 11, 20, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1029), new DateTime(2025, 11, 12, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1031) });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 30, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1037), new DateTime(2025, 12, 4, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1037), new DateTime(2025, 11, 24, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1039) });

            migrationBuilder.UpdateData(
                table: "MedicalProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 27, 20, 20, 12, 390, DateTimeKind.Utc).AddTicks(9993), new DateTime(2025, 11, 27, 20, 20, 12, 390, DateTimeKind.Utc).AddTicks(9995) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 12, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1098), new DateTime(2025, 11, 17, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1093) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 19, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1111), new DateTime(2025, 11, 20, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1109) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 25, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1128), new DateTime(2025, 12, 2, 20, 20, 12, 391, DateTimeKind.Utc).AddTicks(1127) });
        }
    }
}
