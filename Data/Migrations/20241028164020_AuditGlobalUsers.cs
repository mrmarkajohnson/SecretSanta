using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AuditGlobalUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroup_AuditChange_Santa_GiftingGroup_Audit_AuditId",
                table: "Santa_GiftingGroup_AuditChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Santa_GiftingGroup_AuditChange",
                table: "Santa_GiftingGroup_AuditChange");

            migrationBuilder.RenameTable(
                name: "Santa_GiftingGroup_AuditChange",
                newName: "Santa_GiftingGroup_AuditChanges");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroup_AuditChange_AuditId",
                table: "Santa_GiftingGroup_AuditChanges",
                newName: "IX_Santa_GiftingGroup_AuditChanges_AuditId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Santa_GiftingGroup_AuditChanges",
                table: "Santa_GiftingGroup_AuditChanges",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Global_User_Audit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Global_User_Audit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Global_User_Audit_Global_User_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Global_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Global_User_Audit_Global_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Global_User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Global_User_AuditChange",
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
                    table.PrimaryKey("PK_Global_User_AuditChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Global_User_AuditChange_Global_User_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Global_User_Audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Global_User_Audit_ParentId",
                table: "Global_User_Audit",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Global_User_Audit_UserId",
                table: "Global_User_Audit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Global_User_AuditChange_AuditId",
                table: "Global_User_AuditChange",
                column: "AuditId");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroup_AuditChanges_Santa_GiftingGroup_Audit_AuditId",
                table: "Santa_GiftingGroup_AuditChanges",
                column: "AuditId",
                principalTable: "Santa_GiftingGroup_Audit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroup_AuditChanges_Santa_GiftingGroup_Audit_AuditId",
                table: "Santa_GiftingGroup_AuditChanges");

            migrationBuilder.DropTable(
                name: "Global_User_AuditChange");

            migrationBuilder.DropTable(
                name: "Global_User_Audit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Santa_GiftingGroup_AuditChanges",
                table: "Santa_GiftingGroup_AuditChanges");

            migrationBuilder.RenameTable(
                name: "Santa_GiftingGroup_AuditChanges",
                newName: "Santa_GiftingGroup_AuditChange");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroup_AuditChanges_AuditId",
                table: "Santa_GiftingGroup_AuditChange",
                newName: "IX_Santa_GiftingGroup_AuditChange_AuditId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Santa_GiftingGroup_AuditChange",
                table: "Santa_GiftingGroup_AuditChange",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroup_AuditChange_Santa_GiftingGroup_Audit_AuditId",
                table: "Santa_GiftingGroup_AuditChange",
                column: "AuditId",
                principalTable: "Santa_GiftingGroup_Audit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
