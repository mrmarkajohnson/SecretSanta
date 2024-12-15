using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ImproveRelationshipEndHandling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IgnoreOldRelationship",
                table: "Santa_PartnerLinks");

            migrationBuilder.RenameColumn(
                name: "ConfirmedByPartner2",
                table: "Santa_PartnerLinks",
                newName: "Confirmed");

            migrationBuilder.AddColumn<bool>(
                name: "SuggestedByIgnoreOld",
                table: "Santa_PartnerLinks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmedByIgnoreOld",
                table: "Santa_PartnerLinks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Santa_PartnerLinks");

            migrationBuilder.DropColumn(
                name: "ConfirmedByIgnoreOld",
                table: "Santa_PartnerLinks");

            migrationBuilder.RenameColumn(
                name: "SuggestedByIgnoreOld",
                table: "Santa_PartnerLinks",
                newName: "ConfirmedByPartner2");

            migrationBuilder.AddColumn<bool>(
                name: "IgnoreOldRelationship",
                table: "Santa_PartnerLinks",
                type: "bit",
                nullable: true);
        }
    }
}
