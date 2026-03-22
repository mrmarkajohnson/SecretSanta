using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixReplyMessageTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				declare @MessageKey int, @NewRecipientType int

				declare reply_cursor cursor for select 
					sm.MessageKey,
					case when om.RecipientType = 2 then
					case when om.SenderKey = sm.SenderKey then 2 else 3 end
					else case when om.SenderKey <> sm.SenderKey then 2 else 3 end
					end
				from Santa_Messages sm
					inner join Santa_Messages om on sm.OriginalMessageKey = om.MessageKey 
						or (sm.OriginalMessageKey is null and sm.ReplyToMessageKey = om.MessageKey)
				where om.RecipientType in (2, 3)
					and sm.RecipientType not in (2, 3)
				order by sm.DateCreated

				open reply_cursor
				fetch next from reply_cursor into @MessageKey, @NewRecipientType

				while @@fetch_status = 0
				begin	
					select @MessageKey, @NewRecipientType
					-- select sm.*
					update sm set RecipientType = @NewRecipientType 
					from Santa_Messages sm where sm.MessageKey = @MessageKey
	
					fetch next from reply_cursor into @MessageKey, @NewRecipientType	
				end

				close reply_cursor
				deallocate reply_cursor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
