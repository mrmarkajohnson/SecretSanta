using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InvitationMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Santa_Invitations",
                newName: "InvitationMessage");

            migrationBuilder.AlterColumn<string>(
                name: "InvitationMessage",
                table: "Santa_Invitations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionMessage",
                table: "Santa_Invitations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvitationMessage",
                table: "Santa_Invitations",
                newName: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Santa_Invitations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropColumn(
                name: "RejectionMessage",
                table: "Santa_Invitations");
        }
    }
}
