using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileCreationV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "UserProfiles",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "UserProfiles",
                newName: "Country");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "UserProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "UserProfiles",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "UserProfiles",
                newName: "Description");
        }
    }
}
