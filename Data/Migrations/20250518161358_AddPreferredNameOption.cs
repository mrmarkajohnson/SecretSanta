using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPreferredNameOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreferredNameType",
                table: "Global_User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                update Global_User
                set PreferredNameType = case when PreferredFirstName = MiddleNames then 1 when PreferredIsNickname = 1 then 2 else 3 end
                where PreferredFirstName is not null");

            migrationBuilder.DropColumn(
                name: "PreferredIsNickname",
                table: "Global_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PreferredIsNickname",
                table: "Global_User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"
                update Global_User
                set PreferredIsNickname = 1
                where PreferredFirstName is not null and PreferredNameType = 2");

            migrationBuilder.DropColumn(
                name: "PreferredNameType",
                table: "Global_User");
        }
    }
}
