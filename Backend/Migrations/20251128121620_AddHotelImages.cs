using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileCareAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelImages_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8683), new DateTime(2025, 11, 18, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8708), new DateTime(2025, 11, 21, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8709) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 14, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8719), new DateTime(2025, 11, 11, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8721), new DateTime(2025, 11, 14, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8722) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 12, 3, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8724), new DateTime(2025, 11, 26, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8725) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "stefan-mueller-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "18d94093-cd43-499e-97e7-4effa12383cc", "AQAAAAIAAYagAAAAEAhSMo+7CP5Ms04o0Y3fszSyHWcsGaxkyI0YvHo3w3wtHAUQ7r73RyIsCYDFmlFCww==", "b102c33c-98d5-43c1-b0fd-be2946e9e5fb" });

            migrationBuilder.UpdateData(
                table: "DoctorUsers",
                keyColumn: "Id",
                keyValue: "dr-ahmed-hassan-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8bae3755-9ca6-49ee-8a72-c18654c86509", "AQAAAAIAAYagAAAAEL8KNt1Yb7ZoXCxgwRzdpurfP2wPb1y4zjUaxaMPIF69z43kvzh4ceqVwaN4l2bwuA==", "de53e384-cdbe-4371-9a82-4ae56ac0a134" });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 18, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8808), new DateTime(2025, 11, 21, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8809), new DateTime(2025, 11, 13, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8819) });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 12, 1, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8822), new DateTime(2025, 12, 5, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8822), new DateTime(2025, 11, 25, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8824) });

            migrationBuilder.UpdateData(
                table: "MedicalProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 28, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(6764), new DateTime(2025, 11, 28, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(6764) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 13, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8870), new DateTime(2025, 11, 18, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8865) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 20, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8874), new DateTime(2025, 11, 21, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8873) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 26, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8884), new DateTime(2025, 12, 3, 12, 16, 19, 896, DateTimeKind.Utc).AddTicks(8883) });

            migrationBuilder.CreateIndex(
                name: "IX_HotelImages_HotelId",
                table: "HotelImages",
                column: "HotelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelImages");

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
    }
}
