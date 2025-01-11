using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class MessageTweaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HeaderText",
                table: "Santa_Messages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GiftingGroupYearId",
                table: "Santa_Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Messages_GiftingGroupYearId",
                table: "Santa_Messages",
                column: "GiftingGroupYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Messages_Santa_GiftingGroupYears_GiftingGroupYearId",
                table: "Santa_Messages",
                column: "GiftingGroupYearId",
                principalTable: "Santa_GiftingGroupYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Messages_Santa_GiftingGroupYears_GiftingGroupYearId",
                table: "Santa_Messages");

            migrationBuilder.DropIndex(
                name: "IX_Santa_Messages_GiftingGroupYearId",
                table: "Santa_Messages");

            migrationBuilder.DropColumn(
                name: "GiftingGroupYearId",
                table: "Santa_Messages");

            migrationBuilder.AlterColumn<string>(
                name: "HeaderText",
                table: "Santa_Messages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
