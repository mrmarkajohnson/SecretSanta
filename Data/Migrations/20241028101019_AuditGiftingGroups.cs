using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AuditGiftingGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Santa_GiftingGroup_Audit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_GiftingGroup_Audit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Santa_GiftingGroup_Audit_Global_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Global_User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Santa_GiftingGroup_Audit_Santa_GiftingGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Santa_GiftingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Santa_GiftingGroup_AuditChange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditId = table.Column<int>(type: "int", nullable: false),
                    ColumnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_GiftingGroup_AuditChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Santa_GiftingGroup_AuditChange_Santa_GiftingGroup_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Santa_GiftingGroup_Audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Santa_GiftingGroup_Audit_ParentId",
                table: "Santa_GiftingGroup_Audit",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_GiftingGroup_Audit_UserId",
                table: "Santa_GiftingGroup_Audit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_GiftingGroup_AuditChange_AuditId",
                table: "Santa_GiftingGroup_AuditChange",
                column: "AuditId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Santa_GiftingGroup_AuditChange");

            migrationBuilder.DropTable(
                name: "Santa_GiftingGroup_Audit");
        }
    }
}
