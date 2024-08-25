using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUserGroupLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_GiftingGroupId",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_GiftingGroupUsers_UserId",
                table: "Santa_GiftingGroupUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_UserId",
                table: "Santa_GiftingGroupUsers",
                column: "UserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_UserId",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_Santa_GiftingGroupUsers_UserId",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_GiftingGroupId",
                table: "Santa_GiftingGroupUsers",
                column: "GiftingGroupId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
