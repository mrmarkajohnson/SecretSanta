using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class GroupInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Santa_Invitations",
                columns: table => new
                {
                    InvitationKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvitationGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromSantaUserKey = table.Column<int>(type: "int", nullable: false),
                    ToSantaUserKey = table.Column<int>(type: "int", nullable: true),
                    GiftingGroupKey = table.Column<int>(type: "int", nullable: false),
                    ToName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateArchived = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_Invitations", x => x.InvitationKey);
                    table.ForeignKey(
                        name: "FK_Santa_Invitations_Santa_GiftingGroups_GiftingGroupKey",
                        column: x => x.GiftingGroupKey,
                        principalTable: "Santa_GiftingGroups",
                        principalColumn: "GiftingGroupKey");
                    table.ForeignKey(
                        name: "FK_Santa_Invitations_Santa_Users_FromSantaUserKey",
                        column: x => x.FromSantaUserKey,
                        principalTable: "Santa_Users",
                        principalColumn: "SantaUserKey");
                    table.ForeignKey(
                        name: "FK_Santa_Invitations_Santa_Users_ToSantaUserKey",
                        column: x => x.ToSantaUserKey,
                        principalTable: "Santa_Users",
                        principalColumn: "SantaUserKey");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Invitations_FromSantaUserKey",
                table: "Santa_Invitations",
                column: "FromSantaUserKey");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Invitations_GiftingGroupKey",
                table: "Santa_Invitations",
                column: "GiftingGroupKey");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Invitations_ToSantaUserKey",
                table: "Santa_Invitations",
                column: "ToSantaUserKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Santa_Invitations");
        }
    }
}
