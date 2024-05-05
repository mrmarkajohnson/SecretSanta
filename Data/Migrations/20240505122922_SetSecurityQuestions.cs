using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SetSecurityQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecurityAnswer1",
                table: "Global_User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityAnswer2",
                table: "Global_User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityHint1",
                table: "Global_User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityHint2",
                table: "Global_User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityQuestion1",
                table: "Global_User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityQuestion2",
                table: "Global_User",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecurityAnswer1",
                table: "Global_User");

            migrationBuilder.DropColumn(
                name: "SecurityAnswer2",
                table: "Global_User");

            migrationBuilder.DropColumn(
                name: "SecurityHint1",
                table: "Global_User");

            migrationBuilder.DropColumn(
                name: "SecurityHint2",
                table: "Global_User");

            migrationBuilder.DropColumn(
                name: "SecurityQuestion1",
                table: "Global_User");

            migrationBuilder.DropColumn(
                name: "SecurityQuestion2",
                table: "Global_User");
        }
    }
}
