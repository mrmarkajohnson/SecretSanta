namespace Data.Attributes;

internal class NotAuditedAttribute : AuditAttribute
{
	public NotAuditedAttribute()
	{
		NotAudited = true;
	}
}
