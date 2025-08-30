using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SysAdmins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SystemAdmin",
                table: "Global_User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"update gu set SystemAdmin = 1
                from dbo.Santa_Users su
	                inner join dbo.Global_User gu on su.GlobalUserId = gu.Id
                    where su.SantaUserKey = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemAdmin",
                table: "Global_User");
        }
    }
}
