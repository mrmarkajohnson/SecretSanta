namespace Data.Attributes;

internal class AuditAttribute : Attribute
{
    public AuditAttribute(string? name = null)
    {
        Name = name;
    }

    public string? Name { get; set; }

    /// <summary>
    /// Don't record changes to this property in the audit trail
    /// </summary>
    public bool NotAudited { get; set; }

    /// <summary>
    /// Record changes but don't show details when the audit trail is viewed
    /// </summary>
    public bool NoDetails { get; set; }
}
