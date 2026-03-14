using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnnecessaryDeletedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Santa_Users");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Santa_GiftingGroupYears");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Santa_GiftingGroups");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Santa_GiftingGroupApplications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Santa_Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Santa_GiftingGroupYears",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Santa_GiftingGroupUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Santa_GiftingGroups",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Santa_GiftingGroupApplications",
                type: "datetime2",
                nullable: true);
        }
    }
}
