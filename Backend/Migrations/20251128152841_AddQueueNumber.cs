using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileCareAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddQueueNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QueueNumber",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AppointmentDate", "CreatedAt", "QueueNumber", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5462), new DateTime(2025, 11, 18, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5475), 0, new DateTime(2025, 11, 21, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5476) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "AppointmentDate", "CreatedAt", "QueueNumber", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 14, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5489), new DateTime(2025, 11, 11, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5490), 0, new DateTime(2025, 11, 14, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5491) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "AppointmentDate", "CreatedAt", "QueueNumber" },
                values: new object[] { new DateTime(2025, 12, 3, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5493), new DateTime(2025, 11, 26, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5495), 0 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "stefan-mueller-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f25889d4-b1a1-4fd4-918f-88062c3a1699", "AQAAAAIAAYagAAAAENFt5Rstu1CZVfxJnjPBjRkKEqW7kjlryGQgMhJhxHH8mDB3P+Fbt8Vuai11rWN+tg==", "d4a88dbe-8684-4fd7-99bf-caa1070ca734" });

            migrationBuilder.UpdateData(
                table: "DoctorUsers",
                keyColumn: "Id",
                keyValue: "dr-ahmed-hassan-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c623b03e-2fc7-42fb-aec2-816c536e3388", "AQAAAAIAAYagAAAAEDdxliK5ARDi/nmrqd3f9ZUhTva3EdGfZ1h3ssK369/2aAqdl+jm4I8NhJdv0xXb2Q==", "9d53fb80-fdb4-47c9-9c1b-1d4709f1247b" });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 18, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5547), new DateTime(2025, 11, 21, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5547), new DateTime(2025, 11, 13, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5550) });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 12, 1, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5551), new DateTime(2025, 12, 5, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5552), new DateTime(2025, 11, 25, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5554) });

            migrationBuilder.UpdateData(
                table: "HotelUsers",
                keyColumn: "Id",
                keyValue: "hotel-owner-nile-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "96a621a0-3e99-428b-84f7-d7b5c4d99891", new DateTime(2025, 11, 28, 15, 28, 40, 585, DateTimeKind.Utc).AddTicks(2731), "AQAAAAIAAYagAAAAEA1r+a3pSFgdLCt07hH+aIdR1ZPdGWcm/5dMjhDSH1631t0lchnkNeEPaWhmBneKZA==", "e87cda2b-74ce-4132-a8a4-76b1de2c7cf7" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5339));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5345));

            migrationBuilder.UpdateData(
                table: "MedicalProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 28, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5061), new DateTime(2025, 11, 28, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5062) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 13, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5800), new DateTime(2025, 11, 18, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5794) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 20, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5804), new DateTime(2025, 11, 21, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5802) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 26, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5806), new DateTime(2025, 12, 3, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5805) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QueueNumber",
                table: "Appointments");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 21, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7671), new DateTime(2025, 11, 18, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7695), new DateTime(2025, 11, 21, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7695) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "AppointmentDate", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 14, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7706), new DateTime(2025, 11, 11, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7709), new DateTime(2025, 11, 14, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7709) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "AppointmentDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 12, 3, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7713), new DateTime(2025, 11, 26, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7714) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "stefan-mueller-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7a9f07e9-3bbb-4915-bf5e-b2fe7ce64461", "AQAAAAIAAYagAAAAEFwqrmnhMpGVp7m1THntjGnGBRjxTSkWpfy0WktTfSHV29Xm4AvXPkCeLG0j4BemeQ==", "7bf9ad23-807a-4c97-a297-789244c91b17" });

            migrationBuilder.UpdateData(
                table: "DoctorUsers",
                keyColumn: "Id",
                keyValue: "dr-ahmed-hassan-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ee0baf83-f8ec-474f-91e4-586c331e66e8", "AQAAAAIAAYagAAAAECox8uZkWdSVjOqI1JfPeKcI9HB3qbzZPa6guTHmL7eL3IXCI7np2Ojt+/gIUCRUPA==", "ce814f9d-3b7b-4e49-b010-ffda2ccba81c" });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 11, 18, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7788), new DateTime(2025, 11, 21, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7789), new DateTime(2025, 11, 13, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7793) });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2025, 12, 1, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7798), new DateTime(2025, 12, 5, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7799), new DateTime(2025, 11, 25, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7801) });

            migrationBuilder.UpdateData(
                table: "HotelUsers",
                keyColumn: "Id",
                keyValue: "hotel-owner-nile-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c3835deb-de01-4500-841d-c2d9c043966a", new DateTime(2025, 11, 28, 14, 56, 18, 451, DateTimeKind.Utc).AddTicks(5604), "AQAAAAIAAYagAAAAEEgSql+bst02PoAoqXNnPla7eIIi6andYjiOVaepaoVSzJQ9e3sDYBmanqixqVUHhg==", "249e4da4-607c-4b15-b30a-a7a4f630c0d8" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7250));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7263));

            migrationBuilder.UpdateData(
                table: "MedicalProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 28, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(6950), new DateTime(2025, 11, 28, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(6952) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 13, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7954), new DateTime(2025, 11, 18, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7951) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 20, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7960), new DateTime(2025, 11, 21, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7956) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 26, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7968), new DateTime(2025, 12, 3, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7965) });
        }
    }
}
