using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AuditGiftingGroupYears : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Santa_GiftingGroupYear_Audit",
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
                    table.PrimaryKey("PK_Santa_GiftingGroupYear_Audit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Santa_GiftingGroupYear_Audit_Global_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Global_User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Santa_GiftingGroupYear_Audit_Santa_GiftingGroupYears_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Santa_GiftingGroupYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Santa_GiftingGroupYear_AuditChanges",
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
                    table.PrimaryKey("PK_Santa_GiftingGroupYear_AuditChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Santa_GiftingGroupYear_AuditChanges_Santa_GiftingGroupYear_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Santa_GiftingGroupYear_Audit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Santa_GiftingGroupYear_Audit_ParentId",
                table: "Santa_GiftingGroupYear_Audit",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_GiftingGroupYear_Audit_UserId",
                table: "Santa_GiftingGroupYear_Audit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_GiftingGroupYear_AuditChanges_AuditId",
                table: "Santa_GiftingGroupYear_AuditChanges",
                column: "AuditId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Santa_GiftingGroupYear_AuditChanges");

            migrationBuilder.DropTable(
                name: "Santa_GiftingGroupYear_Audit");
        }
    }
}
