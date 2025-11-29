using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileCareAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNationalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NationalIdBackImageUrl",
                table: "DoctorUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NationalIdFrontImageUrl",
                table: "DoctorUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NationalIdBackImageUrl",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NationalIdFrontImageUrl",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AppointmentDate", "CreatedAt", "QueueNumber", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5409), new DateTime(2025, 11, 19, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5424), 1, new DateTime(2025, 11, 22, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5425) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "AppointmentDate", "CreatedAt", "QueueNumber", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 15, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5430), new DateTime(2025, 11, 12, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5432), 1, new DateTime(2025, 11, 15, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5433) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "AppointmentDate", "CreatedAt", "QueueNumber" },
                values: new object[] { new DateTime(2025, 12, 4, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5436), new DateTime(2025, 11, 27, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5438), 1 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "stefan-mueller-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "deef8f73-765e-4278-b5c5-dc40ae067ffe", "AQAAAAIAAYagAAAAECDcitxGbYAO6kypcOz+pwQtjOf8Fncb5Ga4uwZVSA4iOaWzyv+Q7KGTDmd6vHPgYw==", "ef126607-2841-4f64-8624-8d9183e500b6" });

            migrationBuilder.UpdateData(
                table: "DoctorUsers",
                keyColumn: "Id",
                keyValue: "dr-ahmed-hassan-id",
                columns: new[] { "ConcurrencyStamp", "NationalIdBackImageUrl", "NationalIdFrontImageUrl", "PasswordHash", "SecurityStamp" },
                values: new object[] { "925b2efc-9367-454b-a6de-d006b0896ca2", "", "", "AQAAAAIAAYagAAAAECU3eHUTMlyiZYBNWmoJwmITDntdJ48saBRSblC1JOKPAIvXGizlZXQveEOPV/TD3w==", "a7752e6d-d404-46d7-b439-5d4b9b12d003" });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "NationalIdBackImageUrl", "NationalIdFrontImageUrl" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "NationalIdBackImageUrl", "NationalIdFrontImageUrl" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "Status" },
                values: new object[] { new DateTime(2025, 11, 19, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5480), new DateTime(2025, 11, 22, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5481), new DateTime(2025, 11, 14, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5484), "Confirmed" });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "PaymentStatus", "Status" },
                values: new object[] { new DateTime(2025, 12, 2, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5487), new DateTime(2025, 12, 6, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5488), new DateTime(2025, 11, 26, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5491), "Paid", "Pending" });

            migrationBuilder.InsertData(
                table: "HotelBookings",
                columns: new[] { "Id", "CheckInDate", "CheckOutDate", "CreatedAt", "HotelId", "NumberOfGuests", "PaymentStatus", "RoomRatePerNight", "RoomType", "SpecialRequests", "Status", "UserId" },
                values: new object[] { 102, new DateTime(2025, 12, 9, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5499), new DateTime(2025, 12, 13, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5500), new DateTime(2025, 11, 28, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5502), 1, 2, "Paid", 200m, "Suite", "Late check-in requested", "Pending", "stefan-mueller-id" });

            migrationBuilder.UpdateData(
                table: "HotelUsers",
                keyColumn: "Id",
                keyValue: "hotel-owner-nile-id",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ee4e9e13-98df-4ae6-bfc8-7522a13d32e4", new DateTime(2025, 11, 29, 15, 27, 0, 89, DateTimeKind.Utc).AddTicks(7670), "AQAAAAIAAYagAAAAEO1hGAa1eYJhrel28LGFzMetWI+FUsPr4P7C+qxJFC97DQ7AS00skyY5nM+UhcD4nQ==", "f12fd096-3007-472b-a90f-95d49f9f5e13" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 29, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5285));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 29, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5294));

            migrationBuilder.UpdateData(
                table: "MedicalProfiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 29, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(4999), new DateTime(2025, 11, 29, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5000) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 14, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5561), new DateTime(2025, 11, 19, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5555) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 21, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5566), new DateTime(2025, 11, 22, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5563) });

            migrationBuilder.UpdateData(
                table: "TransportBookings",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedAt", "PickupDateTime" },
                values: new object[] { new DateTime(2025, 11, 27, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5569), new DateTime(2025, 12, 4, 15, 27, 0, 181, DateTimeKind.Utc).AddTicks(5568) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DropColumn(
                name: "NationalIdBackImageUrl",
                table: "DoctorUsers");

            migrationBuilder.DropColumn(
                name: "NationalIdFrontImageUrl",
                table: "DoctorUsers");

            migrationBuilder.DropColumn(
                name: "NationalIdBackImageUrl",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "NationalIdFrontImageUrl",
                table: "Doctors");

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
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "Status" },
                values: new object[] { new DateTime(2025, 11, 18, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5547), new DateTime(2025, 11, 21, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5547), new DateTime(2025, 11, 13, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5550), "Completed" });

            migrationBuilder.UpdateData(
                table: "HotelBookings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "PaymentStatus", "Status" },
                values: new object[] { new DateTime(2025, 12, 1, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5551), new DateTime(2025, 12, 5, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5552), new DateTime(2025, 11, 25, 15, 28, 40, 659, DateTimeKind.Utc).AddTicks(5554), "Unpaid", "Confirmed" });

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
    }
}
