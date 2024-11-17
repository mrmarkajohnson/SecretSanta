using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class GiftingGroupYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_UserId",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_UserId",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_UserId",
                table: "Santa_YearGroupUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Santa_YearGroupUsers",
                newName: "SantaUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_UserId",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_SantaUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Santa_GiftingGroupUsers",
                newName: "SantaUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupUsers_UserId",
                table: "Santa_GiftingGroupUsers",
                newName: "IX_Santa_GiftingGroupUsers_SantaUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Santa_GiftingGroupApplications",
                newName: "SantaUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_UserId",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_SantaUserId");

            migrationBuilder.AddColumn<bool>(
                name: "Included",
                table: "Santa_YearGroupUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Limit",
                table: "Santa_GiftingGroupYears",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)",
                oldPrecision: 10,
                oldScale: 4,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_SantaUserId",
                table: "Santa_GiftingGroupApplications",
                column: "SantaUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_SantaUserId",
                table: "Santa_GiftingGroupUsers",
                column: "SantaUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_SantaUserId",
                table: "Santa_YearGroupUsers",
                column: "SantaUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_SantaUserId",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_SantaUserId",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_SantaUserId",
                table: "Santa_YearGroupUsers");

            migrationBuilder.DropColumn(
                name: "Included",
                table: "Santa_YearGroupUsers");

            migrationBuilder.RenameColumn(
                name: "SantaUserId",
                table: "Santa_YearGroupUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_SantaUserId",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_UserId");

            migrationBuilder.RenameColumn(
                name: "SantaUserId",
                table: "Santa_GiftingGroupUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupUsers_SantaUserId",
                table: "Santa_GiftingGroupUsers",
                newName: "IX_Santa_GiftingGroupUsers_UserId");

            migrationBuilder.RenameColumn(
                name: "SantaUserId",
                table: "Santa_GiftingGroupApplications",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_SantaUserId",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_UserId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Limit",
                table: "Santa_GiftingGroupYears",
                type: "decimal(10,4)",
                precision: 10,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_UserId",
                table: "Santa_GiftingGroupApplications",
                column: "UserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_UserId",
                table: "Santa_GiftingGroupUsers",
                column: "UserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_UserId",
                table: "Santa_YearGroupUsers",
                column: "UserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
