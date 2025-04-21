using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SuggestionLinksAndAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Suggestions_Santa_YearGroupUsers_YearGroupUserKey",
                table: "Santa_Suggestions");

            migrationBuilder.DropColumn(
                name: "MainSuggestion",
                table: "Santa_Suggestions");

            migrationBuilder.RenameColumn(
                name: "YearGroupUserKey",
                table: "Santa_Suggestions",
                newName: "SantaUserKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Suggestions_YearGroupUserKey",
                table: "Santa_Suggestions",
                newName: "IX_Santa_Suggestions_SantaUserKey");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Santa_Suggestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Santa_Suggestion_Audit",
                columns: table => new
                {
                    AuditKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentKey = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GlobalUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_Suggestion_Audit", x => x.AuditKey);
                    table.ForeignKey(
                        name: "FK_Santa_Suggestion_Audit_Global_User_GlobalUserId",
                        column: x => x.GlobalUserId,
                        principalTable: "Global_User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Santa_Suggestion_Audit_Santa_Suggestions_ParentKey",
                        column: x => x.ParentKey,
                        principalTable: "Santa_Suggestions",
                        principalColumn: "SuggestionKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Santa_SuggestionLink",
                columns: table => new
                {
                    SuggestionLinkKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuggestionKey = table.Column<int>(type: "int", nullable: false),
                    YearGroupUserKey = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateArchived = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_SuggestionLink", x => x.SuggestionLinkKey);
                    table.ForeignKey(
                        name: "FK_Santa_SuggestionLink_Santa_Suggestions_SuggestionKey",
                        column: x => x.SuggestionKey,
                        principalTable: "Santa_Suggestions",
                        principalColumn: "SuggestionKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Santa_SuggestionLink_Santa_YearGroupUsers_YearGroupUserKey",
                        column: x => x.YearGroupUserKey,
                        principalTable: "Santa_YearGroupUsers",
                        principalColumn: "YearGroupUserKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Santa_Suggestion_AuditChange",
                columns: table => new
                {
                    AuditChangeKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditKey = table.Column<int>(type: "int", nullable: false),
                    ColumnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_Suggestion_AuditChange", x => x.AuditChangeKey);
                    table.ForeignKey(
                        name: "FK_Santa_Suggestion_AuditChange_Santa_Suggestion_Audit_AuditKey",
                        column: x => x.AuditKey,
                        principalTable: "Santa_Suggestion_Audit",
                        principalColumn: "AuditKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Santa_SuggestionLink_Audit",
                columns: table => new
                {
                    AuditKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentKey = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GlobalUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_SuggestionLink_Audit", x => x.AuditKey);
                    table.ForeignKey(
                        name: "FK_Santa_SuggestionLink_Audit_Global_User_GlobalUserId",
                        column: x => x.GlobalUserId,
                        principalTable: "Global_User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Santa_SuggestionLink_Audit_Santa_SuggestionLink_ParentKey",
                        column: x => x.ParentKey,
                        principalTable: "Santa_SuggestionLink",
                        principalColumn: "SuggestionLinkKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Santa_SuggestionLink_AuditChange",
                columns: table => new
                {
                    AuditChangeKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditKey = table.Column<int>(type: "int", nullable: false),
                    ColumnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_SuggestionLink_AuditChange", x => x.AuditChangeKey);
                    table.ForeignKey(
                        name: "FK_Santa_SuggestionLink_AuditChange_Santa_SuggestionLink_Audit_AuditKey",
                        column: x => x.AuditKey,
                        principalTable: "Santa_SuggestionLink_Audit",
                        principalColumn: "AuditKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Suggestion_Audit_GlobalUserId",
                table: "Santa_Suggestion_Audit",
                column: "GlobalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Suggestion_Audit_ParentKey",
                table: "Santa_Suggestion_Audit",
                column: "ParentKey");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Suggestion_AuditChange_AuditKey",
                table: "Santa_Suggestion_AuditChange",
                column: "AuditKey");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_SuggestionLink_SuggestionKey",
                table: "Santa_SuggestionLink",
                column: "SuggestionKey");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_SuggestionLink_YearGroupUserKey",
                table: "Santa_SuggestionLink",
                column: "YearGroupUserKey");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_SuggestionLink_Audit_GlobalUserId",
                table: "Santa_SuggestionLink_Audit",
                column: "GlobalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_SuggestionLink_Audit_ParentKey",
                table: "Santa_SuggestionLink_Audit",
                column: "ParentKey");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_SuggestionLink_AuditChange_AuditKey",
                table: "Santa_SuggestionLink_AuditChange",
                column: "AuditKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Suggestions_Santa_Users_SantaUserKey",
                table: "Santa_Suggestions",
                column: "SantaUserKey",
                principalTable: "Santa_Users",
                principalColumn: "SantaUserKey",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Suggestions_Santa_Users_SantaUserKey",
                table: "Santa_Suggestions");

            migrationBuilder.DropTable(
                name: "Santa_Suggestion_AuditChange");

            migrationBuilder.DropTable(
                name: "Santa_SuggestionLink_AuditChange");

            migrationBuilder.DropTable(
                name: "Santa_Suggestion_Audit");

            migrationBuilder.DropTable(
                name: "Santa_SuggestionLink_Audit");

            migrationBuilder.DropTable(
                name: "Santa_SuggestionLink");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Santa_Suggestions");

            migrationBuilder.RenameColumn(
                name: "SantaUserKey",
                table: "Santa_Suggestions",
                newName: "YearGroupUserKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Suggestions_SantaUserKey",
                table: "Santa_Suggestions",
                newName: "IX_Santa_Suggestions_YearGroupUserKey");

            migrationBuilder.AddColumn<bool>(
                name: "MainSuggestion",
                table: "Santa_Suggestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Suggestions_Santa_YearGroupUsers_YearGroupUserKey",
                table: "Santa_Suggestions",
                column: "YearGroupUserKey",
                principalTable: "Santa_YearGroupUsers",
                principalColumn: "YearGroupUserKey",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
