using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class MoreGroupProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Limit",
                table: "Santa_GiftingGroupYears",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CultureInfo",
                table: "Santa_GiftingGroups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCodeOverride",
                table: "Santa_GiftingGroups",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencySymbolOverride",
                table: "Santa_GiftingGroups",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Limit",
                table: "Santa_GiftingGroupYears");

            migrationBuilder.DropColumn(
                name: "CultureInfo",
                table: "Santa_GiftingGroups");

            migrationBuilder.DropColumn(
                name: "CurrencyCodeOverride",
                table: "Santa_GiftingGroups");

            migrationBuilder.DropColumn(
                name: "CurrencySymbolOverride",
                table: "Santa_GiftingGroups");
        }
    }
}
