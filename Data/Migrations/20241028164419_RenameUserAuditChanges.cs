using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserAuditChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Global_User_AuditChange_Global_User_Audit_AuditId",
                table: "Global_User_AuditChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Global_User_AuditChange",
                table: "Global_User_AuditChange");

            migrationBuilder.RenameTable(
                name: "Global_User_AuditChange",
                newName: "Global_User_AuditChanges");

            migrationBuilder.RenameIndex(
                name: "IX_Global_User_AuditChange_AuditId",
                table: "Global_User_AuditChanges",
                newName: "IX_Global_User_AuditChanges_AuditId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Global_User_AuditChanges",
                table: "Global_User_AuditChanges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Global_User_AuditChanges_Global_User_Audit_AuditId",
                table: "Global_User_AuditChanges",
                column: "AuditId",
                principalTable: "Global_User_Audit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Global_User_AuditChanges_Global_User_Audit_AuditId",
                table: "Global_User_AuditChanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Global_User_AuditChanges",
                table: "Global_User_AuditChanges");

            migrationBuilder.RenameTable(
                name: "Global_User_AuditChanges",
                newName: "Global_User_AuditChange");

            migrationBuilder.RenameIndex(
                name: "IX_Global_User_AuditChanges_AuditId",
                table: "Global_User_AuditChange",
                newName: "IX_Global_User_AuditChange_AuditId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Global_User_AuditChange",
                table: "Global_User_AuditChange",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Global_User_AuditChange_Global_User_Audit_AuditId",
                table: "Global_User_AuditChange",
                column: "AuditId",
                principalTable: "Global_User_Audit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
