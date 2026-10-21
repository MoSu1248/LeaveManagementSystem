using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.web.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultRolesAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24da7188-56a9-4bbc-84d3-7fe7d9a9d629", "22222222-2222-2222-2222-222222222222", "Supervisor", "SUPERVISOR" },
                    { "73dda014-59df-4122-b450-dd8b2c50f034", "33333333-3333-3333-3333-333333333333", "Employee", "EMPLOYEE" },
                    { "74a5d287-a501-4980-9d36-76b329b50b52", "11111111-1111-1111-1111-111111111111", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "09b9dd78-1e2a-4873-beff-e0904bd81334", 0, "44444444-4444-4444-4444-444444444444", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAEAACcQAAAAEOExampleStaticHashStringCopiedFromLocalRun==", null, false, "f1420e0d-750e-4a1b-9c2d-0123456789ab", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "74a5d287-a501-4980-9d36-76b329b50b52", "09b9dd78-1e2a-4873-beff-e0904bd81334" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24da7188-56a9-4bbc-84d3-7fe7d9a9d629");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73dda014-59df-4122-b450-dd8b2c50f034");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "74a5d287-a501-4980-9d36-76b329b50b52", "09b9dd78-1e2a-4873-beff-e0904bd81334" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74a5d287-a501-4980-9d36-76b329b50b52");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "09b9dd78-1e2a-4873-beff-e0904bd81334");
        }
    }
}
