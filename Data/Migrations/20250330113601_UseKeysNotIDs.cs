using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UseKeysNotIDs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Global_User_Audit_Global_User_UserId",
                table: "Global_User_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Global_User_AuditChanges_Global_User_Audit_AuditId",
                table: "Global_User_AuditChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroup_Audit_Global_User_UserId",
                table: "Santa_GiftingGroup_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroup_Audit_Santa_GiftingGroups_ParentId",
                table: "Santa_GiftingGroup_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroup_AuditChanges_Santa_GiftingGroup_Audit_AuditId",
                table: "Santa_GiftingGroup_AuditChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_ResponseByUserId",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_SantaUserId",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_SantaUserId",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupYear_Audit_Global_User_UserId",
                table: "Santa_GiftingGroupYear_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupYear_Audit_Santa_GiftingGroupYears_ParentId",
                table: "Santa_GiftingGroupYear_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupYear_AuditChanges_Santa_GiftingGroupYear_Audit_AuditId",
                table: "Santa_GiftingGroupYear_AuditChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupYears_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupYears");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_Messages_MessageId",
                table: "Santa_MessageRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_Users_RecipientId",
                table: "Santa_MessageRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_MessageReplies_Santa_Messages_OriginalMessageId",
                table: "Santa_MessageReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_MessageReplies_Santa_Messages_ReplyMessageId",
                table: "Santa_MessageReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Messages_Santa_GiftingGroupYears_GiftingGroupYearId",
                table: "Santa_Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Messages_Santa_Users_SenderId",
                table: "Santa_Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_ConfirmingSantaUserId",
                table: "Santa_PartnerLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_SuggestedBySantaUserId",
                table: "Santa_PartnerLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Suggestions_Santa_YearGroupUsers_SuggesterId",
                table: "Santa_Suggestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUser_Audit_Global_User_UserId",
                table: "Santa_YearGroupUser_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUser_Audit_Santa_YearGroupUsers_ParentId",
                table: "Santa_YearGroupUser_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUser_AuditChange_Santa_YearGroupUser_Audit_AuditId",
                table: "Santa_YearGroupUser_AuditChange");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_GiftingGroupYears_YearId",
                table: "Santa_YearGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_GivingToUserId",
                table: "Santa_YearGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_SantaUserId",
                table: "Santa_YearGroupUsers");

            migrationBuilder.RenameColumn(
                name: "SantaUserId",
                table: "Santa_YearGroupUsers",
                newName: "SantaUserKey");

            migrationBuilder.RenameColumn(
                name: "YearId",
                table: "Santa_YearGroupUsers",
                newName: "GiftingGroupYearKey");

            migrationBuilder.RenameColumn(
                name: "GivingToUserId",
                table: "Santa_YearGroupUsers",
                newName: "RecipientSantaUserKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_YearGroupUsers",
                newName: "YearGroupUserKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_YearId",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_GiftingGroupYearKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_SantaUserId",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_SantaUserKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_GivingToUserId",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_RecipientSantaUserKey");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "Santa_YearGroupUser_AuditChange",
                newName: "AuditKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_YearGroupUser_AuditChange",
                newName: "AuditChangeKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUser_AuditChange_AuditId",
                table: "Santa_YearGroupUser_AuditChange",
                newName: "IX_Santa_YearGroupUser_AuditChange_AuditKey");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Santa_YearGroupUser_Audit",
                newName: "GlobalUserId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Santa_YearGroupUser_Audit",
                newName: "ParentKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_YearGroupUser_Audit",
                newName: "AuditKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUser_Audit_UserId",
                table: "Santa_YearGroupUser_Audit",
                newName: "IX_Santa_YearGroupUser_Audit_GlobalUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUser_Audit_ParentId",
                table: "Santa_YearGroupUser_Audit",
                newName: "IX_Santa_YearGroupUser_Audit_ParentKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_Users",
                newName: "SantaUserKey");

            migrationBuilder.RenameColumn(
                name: "SuggesterId",
                table: "Santa_Suggestions",
                newName: "YearGroupUserKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_Suggestions",
                newName: "SuggestionKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Suggestions_SuggesterId",
                table: "Santa_Suggestions",
                newName: "IX_Santa_Suggestions_YearGroupUserKey");

            migrationBuilder.RenameColumn(
                name: "SuggestedBySantaUserId",
                table: "Santa_PartnerLinks",
                newName: "SuggestedBySantaUserKey");

            migrationBuilder.RenameColumn(
                name: "ConfirmingSantaUserId",
                table: "Santa_PartnerLinks",
                newName: "ConfirmingSantaUserKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_PartnerLinks",
                newName: "PartnerLinkKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_PartnerLinks_SuggestedBySantaUserId",
                table: "Santa_PartnerLinks",
                newName: "IX_Santa_PartnerLinks_SuggestedBySantaUserKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_PartnerLinks_ConfirmingSantaUserId",
                table: "Santa_PartnerLinks",
                newName: "IX_Santa_PartnerLinks_ConfirmingSantaUserKey");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Santa_Messages",
                newName: "SenderKey");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupYearId",
                table: "Santa_Messages",
                newName: "GiftingGroupYearKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_Messages",
                newName: "MessageKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Messages_SenderId",
                table: "Santa_Messages",
                newName: "IX_Santa_Messages_SenderKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Messages_GiftingGroupYearId",
                table: "Santa_Messages",
                newName: "IX_Santa_Messages_GiftingGroupYearKey");

            migrationBuilder.RenameColumn(
                name: "ReplyMessageId",
                table: "Santa_MessageReplies",
                newName: "ReplyMessageKey");

            migrationBuilder.RenameColumn(
                name: "OriginalMessageId",
                table: "Santa_MessageReplies",
                newName: "OriginalMessageKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_MessageReplies",
                newName: "MessageReplyKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_MessageReplies_ReplyMessageId",
                table: "Santa_MessageReplies",
                newName: "IX_Santa_MessageReplies_ReplyMessageKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_MessageReplies_OriginalMessageId",
                table: "Santa_MessageReplies",
                newName: "IX_Santa_MessageReplies_OriginalMessageKey");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                table: "Santa_MessageRecipients",
                newName: "RecipientSantaUserKey");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "Santa_MessageRecipients",
                newName: "MessageKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_MessageRecipients",
                newName: "MessageRecipientKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_MessageRecipients_RecipientId",
                table: "Santa_MessageRecipients",
                newName: "IX_Santa_MessageRecipients_RecipientSantaUserKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_MessageRecipients_MessageId",
                table: "Santa_MessageRecipients",
                newName: "IX_Santa_MessageRecipients_MessageKey");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupId",
                table: "Santa_GiftingGroupYears",
                newName: "GiftingGroupKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_GiftingGroupYears",
                newName: "GiftingGroupYearKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupYears_GiftingGroupId",
                table: "Santa_GiftingGroupYears",
                newName: "IX_Santa_GiftingGroupYears_GiftingGroupKey");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "Santa_GiftingGroupYear_AuditChanges",
                newName: "AuditKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_GiftingGroupYear_AuditChanges",
                newName: "AuditChangeKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupYear_AuditChanges_AuditId",
                table: "Santa_GiftingGroupYear_AuditChanges",
                newName: "IX_Santa_GiftingGroupYear_AuditChanges_AuditKey");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Santa_GiftingGroupYear_Audit",
                newName: "GlobalUserId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Santa_GiftingGroupYear_Audit",
                newName: "ParentKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_GiftingGroupYear_Audit",
                newName: "AuditKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupYear_Audit_UserId",
                table: "Santa_GiftingGroupYear_Audit",
                newName: "IX_Santa_GiftingGroupYear_Audit_GlobalUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupYear_Audit_ParentId",
                table: "Santa_GiftingGroupYear_Audit",
                newName: "IX_Santa_GiftingGroupYear_Audit_ParentKey");

            migrationBuilder.RenameColumn(
                name: "SantaUserId",
                table: "Santa_GiftingGroupUsers",
                newName: "SantaUserKey");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupId",
                table: "Santa_GiftingGroupUsers",
                newName: "GiftingGroupKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_GiftingGroupUsers",
                newName: "GiftingGroupUserKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupUsers_SantaUserId",
                table: "Santa_GiftingGroupUsers",
                newName: "IX_Santa_GiftingGroupUsers_SantaUserKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupUsers_GiftingGroupId",
                table: "Santa_GiftingGroupUsers",
                newName: "IX_Santa_GiftingGroupUsers_GiftingGroupKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_GiftingGroups",
                newName: "GiftingGroupKey");

            migrationBuilder.RenameColumn(
                name: "SantaUserId",
                table: "Santa_GiftingGroupApplications",
                newName: "SantaUserKey");

            migrationBuilder.RenameColumn(
                name: "ResponseByUserId",
                table: "Santa_GiftingGroupApplications",
                newName: "ResponseBySantaUserKey");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupId",
                table: "Santa_GiftingGroupApplications",
                newName: "GiftingGroupKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_GiftingGroupApplications",
                newName: "GroupApplicationKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_SantaUserId",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_SantaUserKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_ResponseByUserId",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_ResponseBySantaUserKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_GiftingGroupId",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_GiftingGroupKey");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "Santa_GiftingGroup_AuditChanges",
                newName: "AuditKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_GiftingGroup_AuditChanges",
                newName: "AuditChangeKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroup_AuditChanges_AuditId",
                table: "Santa_GiftingGroup_AuditChanges",
                newName: "IX_Santa_GiftingGroup_AuditChanges_AuditKey");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Santa_GiftingGroup_Audit",
                newName: "GlobalUserId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Santa_GiftingGroup_Audit",
                newName: "ParentKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Santa_GiftingGroup_Audit",
                newName: "AuditKey");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroup_Audit_UserId",
                table: "Santa_GiftingGroup_Audit",
                newName: "IX_Santa_GiftingGroup_Audit_GlobalUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroup_Audit_ParentId",
                table: "Santa_GiftingGroup_Audit",
                newName: "IX_Santa_GiftingGroup_Audit_ParentKey");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "Global_User_AuditChanges",
                newName: "AuditKey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Global_User_AuditChanges",
                newName: "AuditChangeKey");

            migrationBuilder.RenameIndex(
                name: "IX_Global_User_AuditChanges_AuditId",
                table: "Global_User_AuditChanges",
                newName: "IX_Global_User_AuditChanges_AuditKey");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Global_User_Audit",
                newName: "GlobalUserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Global_User_Audit",
                newName: "AuditKey");

            migrationBuilder.RenameIndex(
                name: "IX_Global_User_Audit_UserId",
                table: "Global_User_Audit",
                newName: "IX_Global_User_Audit_GlobalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Global_User_Audit_Global_User_GlobalUserId",
                table: "Global_User_Audit",
                column: "GlobalUserId",
                principalTable: "Global_User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Global_User_AuditChanges_Global_User_Audit_AuditKey",
                table: "Global_User_AuditChanges",
                column: "AuditKey",
                principalTable: "Global_User_Audit",
                principalColumn: "AuditKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroup_Audit_Global_User_GlobalUserId",
                table: "Santa_GiftingGroup_Audit",
                column: "GlobalUserId",
                principalTable: "Global_User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroup_Audit_Santa_GiftingGroups_ParentKey",
                table: "Santa_GiftingGroup_Audit",
                column: "ParentKey",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "GiftingGroupKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroup_AuditChanges_Santa_GiftingGroup_Audit_AuditKey",
                table: "Santa_GiftingGroup_AuditChanges",
                column: "AuditKey",
                principalTable: "Santa_GiftingGroup_Audit",
                principalColumn: "AuditKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_GiftingGroups_GiftingGroupKey",
                table: "Santa_GiftingGroupApplications",
                column: "GiftingGroupKey",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "GiftingGroupKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_ResponseBySantaUserKey",
                table: "Santa_GiftingGroupApplications",
                column: "ResponseBySantaUserKey",
                principalTable: "Santa_Users",
                principalColumn: "SantaUserKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_SantaUserKey",
                table: "Santa_GiftingGroupApplications",
                column: "SantaUserKey",
                principalTable: "Santa_Users",
                principalColumn: "SantaUserKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_GiftingGroups_GiftingGroupKey",
                table: "Santa_GiftingGroupUsers",
                column: "GiftingGroupKey",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "GiftingGroupKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_SantaUserKey",
                table: "Santa_GiftingGroupUsers",
                column: "SantaUserKey",
                principalTable: "Santa_Users",
                principalColumn: "SantaUserKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupYear_Audit_Global_User_GlobalUserId",
                table: "Santa_GiftingGroupYear_Audit",
                column: "GlobalUserId",
                principalTable: "Global_User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupYear_Audit_Santa_GiftingGroupYears_ParentKey",
                table: "Santa_GiftingGroupYear_Audit",
                column: "ParentKey",
                principalTable: "Santa_GiftingGroupYears",
                principalColumn: "GiftingGroupYearKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupYear_AuditChanges_Santa_GiftingGroupYear_Audit_AuditKey",
                table: "Santa_GiftingGroupYear_AuditChanges",
                column: "AuditKey",
                principalTable: "Santa_GiftingGroupYear_Audit",
                principalColumn: "AuditKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupYears_Santa_GiftingGroups_GiftingGroupKey",
                table: "Santa_GiftingGroupYears",
                column: "GiftingGroupKey",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "GiftingGroupKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_Messages_MessageKey",
                table: "Santa_MessageRecipients",
                column: "MessageKey",
                principalTable: "Santa_Messages",
                principalColumn: "MessageKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_Users_RecipientSantaUserKey",
                table: "Santa_MessageRecipients",
                column: "RecipientSantaUserKey",
                principalTable: "Santa_Users",
                principalColumn: "SantaUserKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_MessageReplies_Santa_Messages_OriginalMessageKey",
                table: "Santa_MessageReplies",
                column: "OriginalMessageKey",
                principalTable: "Santa_Messages",
                principalColumn: "MessageKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_MessageReplies_Santa_Messages_ReplyMessageKey",
                table: "Santa_MessageReplies",
                column: "ReplyMessageKey",
                principalTable: "Santa_Messages",
                principalColumn: "MessageKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Messages_Santa_GiftingGroupYears_GiftingGroupYearKey",
                table: "Santa_Messages",
                column: "GiftingGroupYearKey",
                principalTable: "Santa_GiftingGroupYears",
                principalColumn: "GiftingGroupYearKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Messages_Santa_Users_SenderKey",
                table: "Santa_Messages",
                column: "SenderKey",
                principalTable: "Santa_Users",
                principalColumn: "SantaUserKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_ConfirmingSantaUserKey",
                table: "Santa_PartnerLinks",
                column: "ConfirmingSantaUserKey",
                principalTable: "Santa_Users",
                principalColumn: "SantaUserKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_SuggestedBySantaUserKey",
                table: "Santa_PartnerLinks",
                column: "SuggestedBySantaUserKey",
                principalTable: "Santa_Users",
                principalColumn: "SantaUserKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Suggestions_Santa_YearGroupUsers_YearGroupUserKey",
                table: "Santa_Suggestions",
                column: "YearGroupUserKey",
                principalTable: "Santa_YearGroupUsers",
                principalColumn: "YearGroupUserKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUser_Audit_Global_User_GlobalUserId",
                table: "Santa_YearGroupUser_Audit",
                column: "GlobalUserId",
                principalTable: "Global_User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUser_Audit_Santa_YearGroupUsers_ParentKey",
                table: "Santa_YearGroupUser_Audit",
                column: "ParentKey",
                principalTable: "Santa_YearGroupUsers",
                principalColumn: "YearGroupUserKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUser_AuditChange_Santa_YearGroupUser_Audit_AuditKey",
                table: "Santa_YearGroupUser_AuditChange",
                column: "AuditKey",
                principalTable: "Santa_YearGroupUser_Audit",
                principalColumn: "AuditKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_GiftingGroupYears_GiftingGroupYearKey",
                table: "Santa_YearGroupUsers",
                column: "GiftingGroupYearKey",
                principalTable: "Santa_GiftingGroupYears",
                principalColumn: "GiftingGroupYearKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_RecipientSantaUserKey",
                table: "Santa_YearGroupUsers",
                column: "RecipientSantaUserKey",
                principalTable: "Santa_Users",
                principalColumn: "SantaUserKey",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_SantaUserKey",
                table: "Santa_YearGroupUsers",
                column: "SantaUserKey",
                principalTable: "Santa_Users",
                principalColumn: "SantaUserKey",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Global_User_Audit_Global_User_GlobalUserId",
                table: "Global_User_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Global_User_AuditChanges_Global_User_Audit_AuditKey",
                table: "Global_User_AuditChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroup_Audit_Global_User_GlobalUserId",
                table: "Santa_GiftingGroup_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroup_Audit_Santa_GiftingGroups_ParentKey",
                table: "Santa_GiftingGroup_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroup_AuditChanges_Santa_GiftingGroup_Audit_AuditKey",
                table: "Santa_GiftingGroup_AuditChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_GiftingGroups_GiftingGroupKey",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_ResponseBySantaUserKey",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_SantaUserKey",
                table: "Santa_GiftingGroupApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_GiftingGroups_GiftingGroupKey",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_SantaUserKey",
                table: "Santa_GiftingGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupYear_Audit_Global_User_GlobalUserId",
                table: "Santa_GiftingGroupYear_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupYear_Audit_Santa_GiftingGroupYears_ParentKey",
                table: "Santa_GiftingGroupYear_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupYear_AuditChanges_Santa_GiftingGroupYear_Audit_AuditKey",
                table: "Santa_GiftingGroupYear_AuditChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_GiftingGroupYears_Santa_GiftingGroups_GiftingGroupKey",
                table: "Santa_GiftingGroupYears");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_Messages_MessageKey",
                table: "Santa_MessageRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_Users_RecipientSantaUserKey",
                table: "Santa_MessageRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_MessageReplies_Santa_Messages_OriginalMessageKey",
                table: "Santa_MessageReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_MessageReplies_Santa_Messages_ReplyMessageKey",
                table: "Santa_MessageReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Messages_Santa_GiftingGroupYears_GiftingGroupYearKey",
                table: "Santa_Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Messages_Santa_Users_SenderKey",
                table: "Santa_Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_ConfirmingSantaUserKey",
                table: "Santa_PartnerLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_SuggestedBySantaUserKey",
                table: "Santa_PartnerLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_Suggestions_Santa_YearGroupUsers_YearGroupUserKey",
                table: "Santa_Suggestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUser_Audit_Global_User_GlobalUserId",
                table: "Santa_YearGroupUser_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUser_Audit_Santa_YearGroupUsers_ParentKey",
                table: "Santa_YearGroupUser_Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUser_AuditChange_Santa_YearGroupUser_Audit_AuditKey",
                table: "Santa_YearGroupUser_AuditChange");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_GiftingGroupYears_GiftingGroupYearKey",
                table: "Santa_YearGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_RecipientSantaUserKey",
                table: "Santa_YearGroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Santa_YearGroupUsers_Santa_Users_SantaUserKey",
                table: "Santa_YearGroupUsers");

            migrationBuilder.RenameColumn(
                name: "SantaUserKey",
                table: "Santa_YearGroupUsers",
                newName: "SantaUserId");

            migrationBuilder.RenameColumn(
                name: "RecipientSantaUserKey",
                table: "Santa_YearGroupUsers",
                newName: "GivingToUserId");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupYearKey",
                table: "Santa_YearGroupUsers",
                newName: "YearId");

            migrationBuilder.RenameColumn(
                name: "YearGroupUserKey",
                table: "Santa_YearGroupUsers",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_SantaUserKey",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_SantaUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_RecipientSantaUserKey",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_GivingToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUsers_GiftingGroupYearKey",
                table: "Santa_YearGroupUsers",
                newName: "IX_Santa_YearGroupUsers_YearId");

            migrationBuilder.RenameColumn(
                name: "AuditKey",
                table: "Santa_YearGroupUser_AuditChange",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "AuditChangeKey",
                table: "Santa_YearGroupUser_AuditChange",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUser_AuditChange_AuditKey",
                table: "Santa_YearGroupUser_AuditChange",
                newName: "IX_Santa_YearGroupUser_AuditChange_AuditId");

            migrationBuilder.RenameColumn(
                name: "ParentKey",
                table: "Santa_YearGroupUser_Audit",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "GlobalUserId",
                table: "Santa_YearGroupUser_Audit",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "AuditKey",
                table: "Santa_YearGroupUser_Audit",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUser_Audit_ParentKey",
                table: "Santa_YearGroupUser_Audit",
                newName: "IX_Santa_YearGroupUser_Audit_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_YearGroupUser_Audit_GlobalUserId",
                table: "Santa_YearGroupUser_Audit",
                newName: "IX_Santa_YearGroupUser_Audit_UserId");

            migrationBuilder.RenameColumn(
                name: "SantaUserKey",
                table: "Santa_Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "YearGroupUserKey",
                table: "Santa_Suggestions",
                newName: "SuggesterId");

            migrationBuilder.RenameColumn(
                name: "SuggestionKey",
                table: "Santa_Suggestions",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Suggestions_YearGroupUserKey",
                table: "Santa_Suggestions",
                newName: "IX_Santa_Suggestions_SuggesterId");

            migrationBuilder.RenameColumn(
                name: "SuggestedBySantaUserKey",
                table: "Santa_PartnerLinks",
                newName: "SuggestedBySantaUserId");

            migrationBuilder.RenameColumn(
                name: "ConfirmingSantaUserKey",
                table: "Santa_PartnerLinks",
                newName: "ConfirmingSantaUserId");

            migrationBuilder.RenameColumn(
                name: "PartnerLinkKey",
                table: "Santa_PartnerLinks",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_PartnerLinks_SuggestedBySantaUserKey",
                table: "Santa_PartnerLinks",
                newName: "IX_Santa_PartnerLinks_SuggestedBySantaUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_PartnerLinks_ConfirmingSantaUserKey",
                table: "Santa_PartnerLinks",
                newName: "IX_Santa_PartnerLinks_ConfirmingSantaUserId");

            migrationBuilder.RenameColumn(
                name: "SenderKey",
                table: "Santa_Messages",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupYearKey",
                table: "Santa_Messages",
                newName: "GiftingGroupYearId");

            migrationBuilder.RenameColumn(
                name: "MessageKey",
                table: "Santa_Messages",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Messages_SenderKey",
                table: "Santa_Messages",
                newName: "IX_Santa_Messages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_Messages_GiftingGroupYearKey",
                table: "Santa_Messages",
                newName: "IX_Santa_Messages_GiftingGroupYearId");

            migrationBuilder.RenameColumn(
                name: "ReplyMessageKey",
                table: "Santa_MessageReplies",
                newName: "ReplyMessageId");

            migrationBuilder.RenameColumn(
                name: "OriginalMessageKey",
                table: "Santa_MessageReplies",
                newName: "OriginalMessageId");

            migrationBuilder.RenameColumn(
                name: "MessageReplyKey",
                table: "Santa_MessageReplies",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_MessageReplies_ReplyMessageKey",
                table: "Santa_MessageReplies",
                newName: "IX_Santa_MessageReplies_ReplyMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_MessageReplies_OriginalMessageKey",
                table: "Santa_MessageReplies",
                newName: "IX_Santa_MessageReplies_OriginalMessageId");

            migrationBuilder.RenameColumn(
                name: "RecipientSantaUserKey",
                table: "Santa_MessageRecipients",
                newName: "RecipientId");

            migrationBuilder.RenameColumn(
                name: "MessageKey",
                table: "Santa_MessageRecipients",
                newName: "MessageId");

            migrationBuilder.RenameColumn(
                name: "MessageRecipientKey",
                table: "Santa_MessageRecipients",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_MessageRecipients_RecipientSantaUserKey",
                table: "Santa_MessageRecipients",
                newName: "IX_Santa_MessageRecipients_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_MessageRecipients_MessageKey",
                table: "Santa_MessageRecipients",
                newName: "IX_Santa_MessageRecipients_MessageId");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupKey",
                table: "Santa_GiftingGroupYears",
                newName: "GiftingGroupId");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupYearKey",
                table: "Santa_GiftingGroupYears",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupYears_GiftingGroupKey",
                table: "Santa_GiftingGroupYears",
                newName: "IX_Santa_GiftingGroupYears_GiftingGroupId");

            migrationBuilder.RenameColumn(
                name: "AuditKey",
                table: "Santa_GiftingGroupYear_AuditChanges",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "AuditChangeKey",
                table: "Santa_GiftingGroupYear_AuditChanges",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupYear_AuditChanges_AuditKey",
                table: "Santa_GiftingGroupYear_AuditChanges",
                newName: "IX_Santa_GiftingGroupYear_AuditChanges_AuditId");

            migrationBuilder.RenameColumn(
                name: "ParentKey",
                table: "Santa_GiftingGroupYear_Audit",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "GlobalUserId",
                table: "Santa_GiftingGroupYear_Audit",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "AuditKey",
                table: "Santa_GiftingGroupYear_Audit",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupYear_Audit_ParentKey",
                table: "Santa_GiftingGroupYear_Audit",
                newName: "IX_Santa_GiftingGroupYear_Audit_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupYear_Audit_GlobalUserId",
                table: "Santa_GiftingGroupYear_Audit",
                newName: "IX_Santa_GiftingGroupYear_Audit_UserId");

            migrationBuilder.RenameColumn(
                name: "SantaUserKey",
                table: "Santa_GiftingGroupUsers",
                newName: "SantaUserId");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupKey",
                table: "Santa_GiftingGroupUsers",
                newName: "GiftingGroupId");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupUserKey",
                table: "Santa_GiftingGroupUsers",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupUsers_SantaUserKey",
                table: "Santa_GiftingGroupUsers",
                newName: "IX_Santa_GiftingGroupUsers_SantaUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupUsers_GiftingGroupKey",
                table: "Santa_GiftingGroupUsers",
                newName: "IX_Santa_GiftingGroupUsers_GiftingGroupId");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupKey",
                table: "Santa_GiftingGroups",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SantaUserKey",
                table: "Santa_GiftingGroupApplications",
                newName: "SantaUserId");

            migrationBuilder.RenameColumn(
                name: "ResponseBySantaUserKey",
                table: "Santa_GiftingGroupApplications",
                newName: "ResponseByUserId");

            migrationBuilder.RenameColumn(
                name: "GiftingGroupKey",
                table: "Santa_GiftingGroupApplications",
                newName: "GiftingGroupId");

            migrationBuilder.RenameColumn(
                name: "GroupApplicationKey",
                table: "Santa_GiftingGroupApplications",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_SantaUserKey",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_SantaUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_ResponseBySantaUserKey",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_ResponseByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroupApplications_GiftingGroupKey",
                table: "Santa_GiftingGroupApplications",
                newName: "IX_Santa_GiftingGroupApplications_GiftingGroupId");

            migrationBuilder.RenameColumn(
                name: "AuditKey",
                table: "Santa_GiftingGroup_AuditChanges",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "AuditChangeKey",
                table: "Santa_GiftingGroup_AuditChanges",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroup_AuditChanges_AuditKey",
                table: "Santa_GiftingGroup_AuditChanges",
                newName: "IX_Santa_GiftingGroup_AuditChanges_AuditId");

            migrationBuilder.RenameColumn(
                name: "ParentKey",
                table: "Santa_GiftingGroup_Audit",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "GlobalUserId",
                table: "Santa_GiftingGroup_Audit",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "AuditKey",
                table: "Santa_GiftingGroup_Audit",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroup_Audit_ParentKey",
                table: "Santa_GiftingGroup_Audit",
                newName: "IX_Santa_GiftingGroup_Audit_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Santa_GiftingGroup_Audit_GlobalUserId",
                table: "Santa_GiftingGroup_Audit",
                newName: "IX_Santa_GiftingGroup_Audit_UserId");

            migrationBuilder.RenameColumn(
                name: "AuditKey",
                table: "Global_User_AuditChanges",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "AuditChangeKey",
                table: "Global_User_AuditChanges",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Global_User_AuditChanges_AuditKey",
                table: "Global_User_AuditChanges",
                newName: "IX_Global_User_AuditChanges_AuditId");

            migrationBuilder.RenameColumn(
                name: "GlobalUserId",
                table: "Global_User_Audit",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "AuditKey",
                table: "Global_User_Audit",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Global_User_Audit_GlobalUserId",
                table: "Global_User_Audit",
                newName: "IX_Global_User_Audit_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Global_User_Audit_Global_User_UserId",
                table: "Global_User_Audit",
                column: "UserId",
                principalTable: "Global_User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Global_User_AuditChanges_Global_User_Audit_AuditId",
                table: "Global_User_AuditChanges",
                column: "AuditId",
                principalTable: "Global_User_Audit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroup_Audit_Global_User_UserId",
                table: "Santa_GiftingGroup_Audit",
                column: "UserId",
                principalTable: "Global_User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroup_Audit_Santa_GiftingGroups_ParentId",
                table: "Santa_GiftingGroup_Audit",
                column: "ParentId",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroup_AuditChanges_Santa_GiftingGroup_Audit_AuditId",
                table: "Santa_GiftingGroup_AuditChanges",
                column: "AuditId",
                principalTable: "Santa_GiftingGroup_Audit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupApplications",
                column: "GiftingGroupId",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_ResponseByUserId",
                table: "Santa_GiftingGroupApplications",
                column: "ResponseByUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupApplications_Santa_Users_SantaUserId",
                table: "Santa_GiftingGroupApplications",
                column: "SantaUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupUsers",
                column: "GiftingGroupId",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupUsers_Santa_Users_SantaUserId",
                table: "Santa_GiftingGroupUsers",
                column: "SantaUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupYear_Audit_Global_User_UserId",
                table: "Santa_GiftingGroupYear_Audit",
                column: "UserId",
                principalTable: "Global_User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupYear_Audit_Santa_GiftingGroupYears_ParentId",
                table: "Santa_GiftingGroupYear_Audit",
                column: "ParentId",
                principalTable: "Santa_GiftingGroupYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupYear_AuditChanges_Santa_GiftingGroupYear_Audit_AuditId",
                table: "Santa_GiftingGroupYear_AuditChanges",
                column: "AuditId",
                principalTable: "Santa_GiftingGroupYear_Audit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_GiftingGroupYears_Santa_GiftingGroups_GiftingGroupId",
                table: "Santa_GiftingGroupYears",
                column: "GiftingGroupId",
                principalTable: "Santa_GiftingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_Messages_MessageId",
                table: "Santa_MessageRecipients",
                column: "MessageId",
                principalTable: "Santa_Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_MessageRecipients_Santa_Users_RecipientId",
                table: "Santa_MessageRecipients",
                column: "RecipientId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_MessageReplies_Santa_Messages_OriginalMessageId",
                table: "Santa_MessageReplies",
                column: "OriginalMessageId",
                principalTable: "Santa_Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_MessageReplies_Santa_Messages_ReplyMessageId",
                table: "Santa_MessageReplies",
                column: "ReplyMessageId",
                principalTable: "Santa_Messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Messages_Santa_GiftingGroupYears_GiftingGroupYearId",
                table: "Santa_Messages",
                column: "GiftingGroupYearId",
                principalTable: "Santa_GiftingGroupYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Messages_Santa_Users_SenderId",
                table: "Santa_Messages",
                column: "SenderId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_ConfirmingSantaUserId",
                table: "Santa_PartnerLinks",
                column: "ConfirmingSantaUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_PartnerLinks_Santa_Users_SuggestedBySantaUserId",
                table: "Santa_PartnerLinks",
                column: "SuggestedBySantaUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_Suggestions_Santa_YearGroupUsers_SuggesterId",
                table: "Santa_Suggestions",
                column: "SuggesterId",
                principalTable: "Santa_YearGroupUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUser_Audit_Global_User_UserId",
                table: "Santa_YearGroupUser_Audit",
                column: "UserId",
                principalTable: "Global_User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUser_Audit_Santa_YearGroupUsers_ParentId",
                table: "Santa_YearGroupUser_Audit",
                column: "ParentId",
                principalTable: "Santa_YearGroupUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Santa_YearGroupUser_AuditChange_Santa_YearGroupUser_Audit_AuditId",
                table: "Santa_YearGroupUser_AuditChange",
                column: "AuditId",
                principalTable: "Santa_YearGroupUser_Audit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Santa_YearGroupUsers_Santa_Users_SantaUserId",
                table: "Santa_YearGroupUsers",
                column: "SantaUserId",
                principalTable: "Santa_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
