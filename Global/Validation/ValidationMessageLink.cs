namespace Global.Validation;

internal class ValidationMessageLink
{ 
    public ValidationMessageLink(string fluentValidatorName, Type? dataAttribute, string errorMessage, 
        string? property1 = null, string? property2 = null)         
    {
        FluentValidatorName = fluentValidatorName;
        DataAttribute = dataAttribute;
        ErrorMessage = errorMessage;
        Property1 = property1 ?? "1";
        Property2 = property2 ?? "2";
    }

    public string FluentValidatorName { get; set; }
    public Type? DataAttribute { get; set; }
    public string ErrorMessage { get; set; }
    public string Property1 { get; set; }
    public string Property2 { get; set; }

    public string ConvertMessageForFluentValidation()
    {
        return ErrorMessage.Replace("{0}", "{PropertyName}").Replace("{1}", $"{{{Property1}}}").Replace("{2}", $"{{{Property2}}}");
    }
}
