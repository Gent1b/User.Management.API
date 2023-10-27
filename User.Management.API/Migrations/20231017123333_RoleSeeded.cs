using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class RoleSeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a55ed25-00ca-4044-89bc-f53782c12f0c", "2", "User", "User" },
                    { "75f93cf2-e2c2-4e84-a2f4-8539ff55554c", "1", "Admin", "Admin" },
                    { "95e7bfa2-16e2-4237-87cc-4874e19c9c7c", "3", "HR", "HR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a55ed25-00ca-4044-89bc-f53782c12f0c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75f93cf2-e2c2-4e84-a2f4-8539ff55554c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95e7bfa2-16e2-4237-87cc-4874e19c9c7c");
        }
    }
}
