using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class OriginalMessageReplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginalMessageKey",
                table: "Santa_Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReplyToMessageKey",
                table: "Santa_Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Messages_OriginalMessageKey",
                table: "Santa_Messages",
                column: "OriginalMessageKey");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_Messages_ReplyToMessageKey",
                table: "Santa_Messages",
                column: "ReplyToMessageKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Messages_Santa_Messages_OriginalMessageKey",
                table: "Santa_Messages",
                column: "OriginalMessageKey",
                principalTable: "Santa_Messages",
                principalColumn: "MessageKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Messages_Santa_Messages_ReplyToMessageKey",
                table: "Santa_Messages",
                column: "ReplyToMessageKey",
                principalTable: "Santa_Messages",
                principalColumn: "MessageKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"update rm set ReplyToMessageKey = om.MessageKey
                from dbo.Santa_MessageReplies mr
	                inner join dbo.Santa_Messages rm on mr.ReplyMessageKey = rm.MessageKey
	                inner join dbo.Santa_Messages om on mr.OriginalMessageKey = om.MessageKey");

            migrationBuilder.DropTable(
                name: "Santa_MessageReplies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.CreateTable(
                name: "Santa_MessageReplies",
                columns: table => new
                {
                    MessageReplyKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalMessageKey = table.Column<int>(type: "int", nullable: false),
                    ReplyMessageKey = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Santa_MessageReplies", x => x.MessageReplyKey);
                    table.ForeignKey(
                        name: "FK_Santa_MessageReplies_Santa_Messages_OriginalMessageKey",
                        column: x => x.OriginalMessageKey,
                        principalTable: "Santa_Messages",
                        principalColumn: "MessageKey",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Santa_MessageReplies_Santa_Messages_ReplyMessageKey",
                        column: x => x.ReplyMessageKey,
                        principalTable: "Santa_Messages",
                        principalColumn: "MessageKey");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Santa_MessageReplies_OriginalMessageKey",
                table: "Santa_MessageReplies",
                column: "OriginalMessageKey");

            migrationBuilder.CreateIndex(
                name: "IX_Santa_MessageReplies_ReplyMessageKey",
                table: "Santa_MessageReplies",
                column: "ReplyMessageKey",
                unique: true);

            migrationBuilder.Sql(@"insert into dbo.Santa_MessageReplies(OriginalMessageKey, ReplyMessageKey, DateCreated)
                select om.MessageKey, rm.MessageKey, getdate()
                from dbo.Santa_Messages rm
	                inner join dbo.Santa_Messages om on rm.ReplyToMessageKey = om.MessageKey");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Messages_Santa_Messages_OriginalMessageKey",
                table: "Santa_Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Messages_Santa_Messages_ReplyToMessageKey",
                table: "Santa_Messages");

            migrationBuilder.DropIndex(
                name: "IX_Santa_Messages_OriginalMessageKey",
                table: "Santa_Messages");

            migrationBuilder.DropIndex(
                name: "IX_Santa_Messages_ReplyToMessageKey",
                table: "Santa_Messages");

            migrationBuilder.DropColumn(
                name: "OriginalMessageKey",
                table: "Santa_Messages");

            migrationBuilder.DropColumn(
                name: "ReplyToMessageKey",
                table: "Santa_Messages");
        }
    }
}
