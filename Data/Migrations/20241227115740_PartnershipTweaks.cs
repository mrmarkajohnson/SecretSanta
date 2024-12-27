using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class PartnershipTweaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_ConfirmedById",
                table: "Santa_PartnerLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_SuggestedById",
                table: "Santa_PartnerLinks");

            migrationBuilder.RenameColumn(
                name: "SuggestedById",
                table: "Santa_PartnerLinks",
                newName: "SuggestedBySantaUserId");

            migrationBuilder.RenameColumn(
                name: "ConfirmedById",
                table: "Santa_PartnerLinks",
                newName: "ConfirmingSantaUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_PartnerLinks_SuggestedById",
                table: "Santa_PartnerLinks",
                newName: "IX_Santa_PartnerLinks_SuggestedBySantaUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_PartnerLinks_ConfirmedById",
                table: "Santa_PartnerLinks",
                newName: "IX_Santa_PartnerLinks_ConfirmingSantaUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_ConfirmingSantaUserId",
                table: "Santa_PartnerLinks",
                column: "ConfirmingSantaUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_SuggestedBySantaUserId",
                table: "Santa_PartnerLinks",
                column: "SuggestedBySantaUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_ConfirmingSantaUserId",
                table: "Santa_PartnerLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_SuggestedBySantaUserId",
                table: "Santa_PartnerLinks");

            migrationBuilder.RenameColumn(
                name: "SuggestedBySantaUserId",
                table: "Santa_PartnerLinks",
                newName: "SuggestedById");

            migrationBuilder.RenameColumn(
                name: "ConfirmingSantaUserId",
                table: "Santa_PartnerLinks",
                newName: "ConfirmedById");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_PartnerLinks_SuggestedBySantaUserId",
                table: "Santa_PartnerLinks",
                newName: "IX_Santa_PartnerLinks_SuggestedById");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_PartnerLinks_ConfirmingSantaUserId",
                table: "Santa_PartnerLinks",
                newName: "IX_Santa_PartnerLinks_ConfirmedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_ConfirmedById",
                table: "Santa_PartnerLinks",
                column: "ConfirmedById",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_SuggestedById",
                table: "Santa_PartnerLinks",
                column: "SuggestedById",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
