using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJoinerApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Santa_GiftingGroupApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GiftingGroupId = table.Column<int>(type: "int", nullable: false),
                    ResponseByUserId = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Accepted = table.Column<bool>(type: "bit", nullable: true),
                    RejectionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Blocked = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateArchived = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_GiftingGroupApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Santa_GiftingGroupApplication_Santa_GiftingGroups_GiftingGroupId",
                        column: x => x.GiftingGroupId,
                        principalTable: "Santa_GiftingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Santa_GiftingGroupApplication_Santa_Users_ResponseByUserId",
                        column: x => x.ResponseByUserId,
                        principalTable: "Santa_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Santa_GiftingGroupApplication_Santa_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Santa_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Santa_GiftingGroupApplication_GiftingGroupId",
                table: "Santa_GiftingGroupApplication",
                column: "GiftingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_GiftingGroupApplication_ResponseByUserId",
                table: "Santa_GiftingGroupApplication",
                column: "ResponseByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_GiftingGroupApplication_UserId",
                table: "Santa_GiftingGroupApplication",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Santa_GiftingGroupApplication");
        }
    }
}
