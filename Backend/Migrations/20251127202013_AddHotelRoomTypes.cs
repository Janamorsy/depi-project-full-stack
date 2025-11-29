using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileCareAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelRoomTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeluxeRoomMaxGuests",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DeluxeRoomPrice",
                table: "Hotels",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "FamilyRoomMaxGuests",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "FamilyRoomPrice",
                table: "Hotels",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "StandardRoomMaxGuests",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "StandardRoomPrice",
                table: "Hotels",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SuiteRoomMaxGuests",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SuiteRoomPrice",
                table: "Hotels",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeluxeRoomMaxGuests", "DeluxeRoomPrice", "FamilyRoomMaxGuests", "FamilyRoomPrice", "StandardRoomMaxGuests", "StandardRoomPrice", "SuiteRoomMaxGuests", "SuiteRoomPrice" },
                values: new object[] { 3, 180m, 6, 350m, 2, 120m, 4, 280m });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeluxeRoomMaxGuests", "DeluxeRoomPrice", "FamilyRoomMaxGuests", "FamilyRoomPrice", "StandardRoomMaxGuests", "StandardRoomPrice", "SuiteRoomMaxGuests", "SuiteRoomPrice" },
                values: new object[] { 3, 140m, 6, 280m, 2, 95m, 4, 220m });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeluxeRoomMaxGuests",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "DeluxeRoomPrice",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "FamilyRoomMaxGuests",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "FamilyRoomPrice",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "StandardRoomMaxGuests",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "StandardRoomPrice",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "SuiteRoomMaxGuests",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "SuiteRoomPrice",
                table: "Hotels");

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
        }
    }
}
