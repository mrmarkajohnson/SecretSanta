using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SetExchangeGiftsOnRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConfirmedByIgnoreOld",
                table: "Santa_PartnerLinks",
                newName: "ExchangeGifts");

            migrationBuilder.AlterColumn<bool>(
                name: "Confirmed",
                table: "Santa_PartnerLinks",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmingUserIgnore",
                table: "Santa_PartnerLinks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("update dbo.Santa_PartnerLinks set Confirmed = null where Confirmed <> 1 and RelationshipEnded is null");
            migrationBuilder.Sql("update dbo.Santa_PartnerLinks set ExchangeGifts = 1 where Confirmed = 1 " +
                "and SuggestedByIgnoreOld = 1 and ConfirmingUserIgnore = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmingUserIgnore",
                table: "Santa_PartnerLinks");

            migrationBuilder.RenameColumn(
                name: "ExchangeGifts",
                table: "Santa_PartnerLinks",
                newName: "ConfirmedByIgnoreOld");

            migrationBuilder.AlterColumn<bool>(
                name: "Confirmed",
                table: "Santa_PartnerLinks",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
