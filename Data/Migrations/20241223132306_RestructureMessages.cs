using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RestructureMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_GiftingGroupUsers_RecipientId",
                table: "Santa_MessageRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Messages_Santa_YearGroupUsers_SenderId",
                table: "Santa_Messages");

            migrationBuilder.AddColumn<bool>(
                name: "ShowAsFromSanta",
                table: "Santa_Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_Users_RecipientId",
                table: "Santa_MessageRecipients",
                column: "RecipientId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Messages_Santa_Users_SenderId",
                table: "Santa_Messages",
                column: "SenderId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_Users_RecipientId",
                table: "Santa_MessageRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Messages_Santa_Users_SenderId",
                table: "Santa_Messages");

            migrationBuilder.DropColumn(
                name: "ShowAsFromSanta",
                table: "Santa_Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_GiftingGroupUsers_RecipientId",
                table: "Santa_MessageRecipients",
                column: "RecipientId",
                principalTable: "Santa_GiftingGroupUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Messages_Santa_YearGroupUsers_SenderId",
                table: "Santa_Messages",
                column: "SenderId",
                principalTable: "Santa_YearGroupUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
