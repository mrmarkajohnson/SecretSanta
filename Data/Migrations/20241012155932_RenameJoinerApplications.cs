using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameJoinerApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplication_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplication_Santa_Users_ResponseByUserId",
                table: "Santa_GiftingGroupApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplication_Santa_Users_UserId",
                table: "Santa_GiftingGroupApplication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Santa_GiftingGroupApplication",
                table: "Santa_GiftingGroupApplication");

            migrationBuilder.RenameTable(
                name: "Santa_GiftingGroupApplication",
                newName: "Santa_GiftingGroupApplications");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplication_UserId",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplication_ResponseByUserId",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_ResponseByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplication_GiftingGroupId",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_GiftingGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Santa_GiftingGroupApplications",
                table: "Santa_GiftingGroupApplications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupApplications",
                column: "GiftingGroupId",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_ResponseByUserId",
                table: "Santa_GiftingGroupApplications",
                column: "ResponseByUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_UserId",
                table: "Santa_GiftingGroupApplications",
                column: "UserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_ResponseByUserId",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_UserId",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Santa_GiftingGroupApplications",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.RenameTable(
                name: "Santa_GiftingGroupApplications",
                newName: "Santa_GiftingGroupApplication");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_UserId",
                table: "Santa_GiftingGroupApplication",
                newName: "IX_Santa_GiftingGroupApplication_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_ResponseByUserId",
                table: "Santa_GiftingGroupApplication",
                newName: "IX_Santa_GiftingGroupApplication_ResponseByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_GiftingGroupId",
                table: "Santa_GiftingGroupApplication",
                newName: "IX_Santa_GiftingGroupApplication_GiftingGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Santa_GiftingGroupApplication",
                table: "Santa_GiftingGroupApplication",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplication_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupApplication",
                column: "GiftingGroupId",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplication_Santa_Users_ResponseByUserId",
                table: "Santa_GiftingGroupApplication",
                column: "ResponseByUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplication_Santa_Users_UserId",
                table: "Santa_GiftingGroupApplication",
                column: "UserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
