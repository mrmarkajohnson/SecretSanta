using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SuggestionsAndMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUser_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUser_Santa_Users_GiftingGroupId",
                table: "Santa_GiftingGroupUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupYear_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupYear");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Partners_Santa_Users_Partner1Id",
                table: "Santa_Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Partners_Santa_Users_Partner2Id",
                table: "Santa_Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUser_Santa_GiftingGroupYear_YearId",
                table: "Santa_YearGroupUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUser_Santa_Users_GivingToUserId",
                table: "Santa_YearGroupUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUser_Santa_Users_UserId",
                table: "Santa_YearGroupUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Santa_YearGroupUser",
                table: "Santa_YearGroupUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Santa_GiftingGroupYear",
                table: "Santa_GiftingGroupYear");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Santa_GiftingGroupUser",
                table: "Santa_GiftingGroupUser");

            migrationBuilder.RenameTable(
                name: "Santa_YearGroupUser",
                newName: "Santa_YearGroupUsers");

            migrationBuilder.RenameTable(
                name: "Santa_GiftingGroupYear",
                newName: "Santa_GiftingGroupYears");

            migrationBuilder.RenameTable(
                name: "Santa_GiftingGroupUser",
                newName: "Santa_GiftingGroupUsers");

            migrationBuilder.RenameColumn(
                name: "Partner2Id",
                table: "Santa_Partners",
                newName: "SuggestedById");

            migrationBuilder.RenameColumn(
                name: "Partner1Id",
                table: "Santa_Partners",
                newName: "ConfirmedById");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Partners_Partner2Id",
                table: "Santa_Partners",
                newName: "IX_Santa_Partners_SuggestedById");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Partners_Partner1Id",
                table: "Santa_Partners",
                newName: "IX_Santa_Partners_ConfirmedById");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUser_YearId",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_YearId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUser_UserId",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUser_GivingToUserId",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_GivingToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupYear_GiftingGroupId",
                table: "Santa_GiftingGroupYears",
                newName: "IX_Santa_GiftingGroupYears_GiftingGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupUser_GiftingGroupId",
                table: "Santa_GiftingGroupUsers",
                newName: "IX_Santa_GiftingGroupUsers_GiftingGroupId");

            migrationBuilder.AddColumn<bool>(
                name: "GroupAdmin",
                table: "Santa_GiftingGroupUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Santa_YearGroupUsers",
                table: "Santa_YearGroupUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Santa_GiftingGroupYears",
                table: "Santa_GiftingGroupYears",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Santa_GiftingGroupUsers",
                table: "Santa_GiftingGroupUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Santa_Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateArchived = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    RecipientTypes = table.Column<int>(type: "int", nullable: false),
                    HeaderText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Important = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Santa_Messages_Santa_YearGroupUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Santa_YearGroupUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Santa_Suggestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuggesterId = table.Column<int>(type: "int", nullable: false),
                    MainSuggestion = table.Column<bool>(type: "bit", nullable: false),
                    SuggestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtherNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateArchived = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_Suggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Santa_Suggestions_Santa_YearGroupUsers_SuggesterId",
                        column: x => x.SuggesterId,
                        principalTable: "Santa_YearGroupUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Santa_MessageRecipients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    DateArchived = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_MessageRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Santa_MessageRecipients_Santa_GiftingGroupUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Santa_GiftingGroupUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Santa_MessageRecipients_Santa_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Santa_Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Santa_MessageReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalMessageId = table.Column<int>(type: "int", nullable: false),
                    ReplyMessageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_MessageReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Santa_MessageReplies_Santa_Messages_OriginalMessageId",
                        column: x => x.OriginalMessageId,
                        principalTable: "Santa_Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Santa_MessageReplies_Santa_Messages_ReplyMessageId",
                        column: x => x.ReplyMessageId,
                        principalTable: "Santa_Messages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Santa_MessageRecipients_MessageId",
                table: "Santa_MessageRecipients",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_MessageRecipients_RecipientId",
                table: "Santa_MessageRecipients",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_MessageReplies_OriginalMessageId",
                table: "Santa_MessageReplies",
                column: "OriginalMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_MessageReplies_ReplyMessageId",
                table: "Santa_MessageReplies",
                column: "ReplyMessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Messages_SenderId",
                table: "Santa_Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Suggestions_SuggesterId",
                table: "Santa_Suggestions",
                column: "SuggesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupUsers",
                column: "GiftingGroupId",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_GiftingGroupId",
                table: "Santa_GiftingGroupUsers",
                column: "GiftingGroupId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupYears_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupYears",
                column: "GiftingGroupId",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Partners_Santa_Users_ConfirmedById",
                table: "Santa_Partners",
                column: "ConfirmedById",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Partners_Santa_Users_SuggestedById",
                table: "Santa_Partners",
                column: "SuggestedById",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_GiftingGroupYears_YearId",
                table: "Santa_YearGroupUsers",
                column: "YearId",
                principalTable: "Santa_GiftingGroupYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_GivingToUserId",
                table: "Santa_YearGroupUsers",
                column: "GivingToUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_UserId",
                table: "Santa_YearGroupUsers",
                column: "UserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_GiftingGroupId",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupYears_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupYears");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Partners_Santa_Users_ConfirmedById",
                table: "Santa_Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Partners_Santa_Users_SuggestedById",
                table: "Santa_Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_GiftingGroupYears_YearId",
                table: "Santa_YearGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_GivingToUserId",
                table: "Santa_YearGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_UserId",
                table: "Santa_YearGroupUsers");

            migrationBuilder.DropTable(
                name: "Santa_MessageRecipients");

            migrationBuilder.DropTable(
                name: "Santa_MessageReplies");

            migrationBuilder.DropTable(
                name: "Santa_Suggestions");

            migrationBuilder.DropTable(
                name: "Santa_Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Santa_YearGroupUsers",
                table: "Santa_YearGroupUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Santa_GiftingGroupYears",
                table: "Santa_GiftingGroupYears");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Santa_GiftingGroupUsers",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropColumn(
                name: "GroupAdmin",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.RenameTable(
                name: "Santa_YearGroupUsers",
                newName: "Santa_YearGroupUser");

            migrationBuilder.RenameTable(
                name: "Santa_GiftingGroupYears",
                newName: "Santa_GiftingGroupYear");

            migrationBuilder.RenameTable(
                name: "Santa_GiftingGroupUsers",
                newName: "Santa_GiftingGroupUser");

            migrationBuilder.RenameColumn(
                name: "SuggestedById",
                table: "Santa_Partners",
                newName: "Partner2Id");

            migrationBuilder.RenameColumn(
                name: "ConfirmedById",
                table: "Santa_Partners",
                newName: "Partner1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Partners_SuggestedById",
                table: "Santa_Partners",
                newName: "IX_Santa_Partners_Partner2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Partners_ConfirmedById",
                table: "Santa_Partners",
                newName: "IX_Santa_Partners_Partner1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_YearId",
                table: "Santa_YearGroupUser",
                newName: "IX_Santa_YearGroupUser_YearId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_UserId",
                table: "Santa_YearGroupUser",
                newName: "IX_Santa_YearGroupUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_GivingToUserId",
                table: "Santa_YearGroupUser",
                newName: "IX_Santa_YearGroupUser_GivingToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupYears_GiftingGroupId",
                table: "Santa_GiftingGroupYear",
                newName: "IX_Santa_GiftingGroupYear_GiftingGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupUsers_GiftingGroupId",
                table: "Santa_GiftingGroupUser",
                newName: "IX_Santa_GiftingGroupUser_GiftingGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Santa_YearGroupUser",
                table: "Santa_YearGroupUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Santa_GiftingGroupYear",
                table: "Santa_GiftingGroupYear",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Santa_GiftingGroupUser",
                table: "Santa_GiftingGroupUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUser_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupUser",
                column: "GiftingGroupId",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUser_Santa_Users_GiftingGroupId",
                table: "Santa_GiftingGroupUser",
                column: "GiftingGroupId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupYear_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupYear",
                column: "GiftingGroupId",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Partners_Santa_Users_Partner1Id",
                table: "Santa_Partners",
                column: "Partner1Id",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Partners_Santa_Users_Partner2Id",
                table: "Santa_Partners",
                column: "Partner2Id",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUser_Santa_GiftingGroupYear_YearId",
                table: "Santa_YearGroupUser",
                column: "YearId",
                principalTable: "Santa_GiftingGroupYear",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUser_Santa_Users_GivingToUserId",
                table: "Santa_YearGroupUser",
                column: "GivingToUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUser_Santa_Users_UserId",
                table: "Santa_YearGroupUser",
                column: "UserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
