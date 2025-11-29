using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileCareAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelUsersAndApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Hotels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Hotels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HotelUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_HotelUsers", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "HotelUsers",
                columns: new[] { "Id", "AccessFailedCount", "CompanyName", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "LastLoginAt", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "hotel-owner-nile-id", 0, "Nile Hotels Group", "c3835deb-de01-4500-841d-c2d9c043966a", new DateTime(2025, 11, 28, 14, 56, 18, 451, DateTimeKind.Utc).AddTicks(5604), "hotel@nilehotels.com", true, "Mohamed", null, "Farouk", false, null, "HOTEL@NILEHOTELS.COM", "HOTEL@NILEHOTELS.COM", "AQAAAAIAAYagAAAAEEgSql+bst02PoAoqXNnPla7eIIi6andYjiOVaepaoVSzJQ9e3sDYBmanqixqVUHhg==", "+20 100 555 1234", false, "/images/default-avatar.png", "249e4da4-607c-4b15-b30a-a7a4f630c0d8", false, "hotel@nilehotels.com" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ApprovalStatus", "CreatedAt", "OwnerId", "RejectionReason" },
                values: new object[] { "Approved", new DateTime(2025, 11, 28, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7250), "hotel-owner-nile-id", null });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ApprovalStatus", "CreatedAt", "OwnerId", "RejectionReason" },
                values: new object[] { "Approved", new DateTime(2025, 11, 28, 14, 56, 18, 517, DateTimeKind.Utc).AddTicks(7263), "hotel-owner-nile-id", null });

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

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_OwnerId",
                table: "Hotels",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_HotelUsers_OwnerId",
                table: "Hotels",
                column: "OwnerId",
                principalTable: "HotelUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_HotelUsers_OwnerId",
                table: "Hotels");

            migrationBuilder.DropTable(
                name: "HotelUsers");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_OwnerId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Hotels");

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
        }
    }
}
