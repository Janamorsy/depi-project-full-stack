using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileCareAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StripeSessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StripePaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6727), new DateTime(2025, 11, 17, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6747), new DateTime(2025, 11, 20, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6749) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6763), new DateTime(2025, 11, 10, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6765), new DateTime(2025, 11, 13, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6767) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 12, 2, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6772), new DateTime(2025, 11, 25, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6773) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "stefan-mueller-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d942078a-5861-4ed8-af1d-809f2d0f659e", "AQAAAAIAAYagAAAAEPHAAVFEKVYDDkVrumiH+Mc2UnUwHJcxfilUK9NbVNVColpiHLXxpWV1RfpQyvw33A==", "e7ba8116-19f1-4133-b48c-778f8a9b30f0" });

            migrationBuilder.UpdateData(
                table: "DoctorUsers",
                keyColumn: "Id",
                keyValue: "dr-ahmed-hassan-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d10053aa-4953-4304-875d-1f3edad2b409", "AQAAAAIAAYagAAAAEKqgepHfM0FpZfnK9aUSCmlL0y/6YkuTso0mBrc1TLl5t0ihk2xCWNDWoCq+fkyBLQ==", "c76e5dbb-fde3-488a-b632-7ab1cf95163b" });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6880), new DateTime(2025, 11, 20, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6883), new DateTime(2025, 11, 12, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6886) });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 30, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6889), new DateTime(2025, 12, 4, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6890), new DateTime(2025, 11, 24, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6892) });

            migrationBuilder.UpdateData(
                table: "MedicalProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 27, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(5592), new DateTime(2025, 11, 27, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(5592) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 12, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6958), new DateTime(2025, 11, 17, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6952) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 19, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6962), new DateTime(2025, 11, 20, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6960) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 25, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6975), new DateTime(2025, 12, 2, 20, 1, 35, 715, DateTimeKind.Utc).AddTicks(6971) });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(727), new DateTime(2025, 11, 17, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(740), new DateTime(2025, 11, 20, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(741) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(746), new DateTime(2025, 11, 10, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(747), new DateTime(2025, 11, 13, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(748) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 12, 2, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(749), new DateTime(2025, 11, 25, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(750) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "stefan-mueller-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2fc6316c-fd7c-4271-8adf-def65d8221c2", "AQAAAAIAAYagAAAAEGnRlqbAwY6gnrucES84uM9Pvz0nUOxh8vEyKAuYEivdPLDhJxyj2Nds/kUndnXe7Q==", "8f23e76a-3dc4-4a2b-b684-bf59814bed64" });

            migrationBuilder.UpdateData(
                table: "DoctorUsers",
                keyColumn: "Id",
                keyValue: "dr-ahmed-hassan-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "859f7c8f-6b84-4976-adb7-ebf6584be5d0", "AQAAAAIAAYagAAAAEA0+rjvgKrc0g9t0k/uEgWy/lHf03kbT5pY3fHU6d6Fpfw7hVqxUWeYhI0G0MB68Gw==", "0c6fe827-1e49-4593-96bf-b4bc90e3b5e4" });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(798), new DateTime(2025, 11, 20, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(802), new DateTime(2025, 11, 12, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(804) });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 30, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(806), new DateTime(2025, 12, 4, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(807), new DateTime(2025, 11, 24, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(808) });

            migrationBuilder.UpdateData(
                table: "MedicalProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 27, 16, 19, 11, 383, DateTimeKind.Utc).AddTicks(3370), new DateTime(2025, 11, 27, 16, 19, 11, 383, DateTimeKind.Utc).AddTicks(3371) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 12, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(856), new DateTime(2025, 11, 17, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(849) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 19, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(861), new DateTime(2025, 11, 20, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(859) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 25, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(863), new DateTime(2025, 12, 2, 16, 19, 11, 387, DateTimeKind.Utc).AddTicks(862) });
        }
    }
}
