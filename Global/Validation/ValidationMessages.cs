using System.ComponentModel.DataAnnotations;

namespace Global.Validation;

public static class ValidationMessages
{
    public const string PasswordConfirmationError = "Password and Confirmation Password do not match.";
    
    public const string EmailError = "{0} is not a valid e-mail address.";
    public const string GreaterOrEqualError = "{0} must be at least {1}.";
    public const string GreaterThanError = "'{0}' must be greater than {1}.";
    public const string LengthError = "{0} must be {2} to {1} characters long.";
    public const string MaxLengthError = "{0} must be {1} characters or less.";
    public const string MinLengthError = "{0} must be at least {1} characters long.";
    public const string LessOrEqualError = "{0} cannot be more than {1}.";
    public const string LessThanError = "'{0}' must be less than '{1}'.";
    public const string RequiredError = "{0} is required.";
    public const string NotEqualError = "'{0}' cannot be {1}.";
    public const string EqualError = "'{0}' must be {1}.";
    public const string ExactLengthError = "{0} must be {1} characters long.";
    public const string InclusiveBetweenError = "{0} must be between {1} and {2}.";
    public const string ExclusiveBetweenError = "{0} must be less than {1} and more than {2}.";
    public const string CreditCardError = "{0} is not a valid credit card number.";
    public const string NotInRangeError = "{0} has a range of values which does not include {1}.";
    public const string EmptyError = "{0} must be empty.";
    public const string NotValidError = "{0} is not valid.";
    public const string NotInDropDownError = "Please select a {0} from the drop-down list.";

    internal static readonly IList<ValidationMessageLink> MessageLinks =
    [
        new ValidationMessageLink("EmailValidator", typeof(EmailAddressAttribute), EmailError),
        new ValidationMessageLink("GreaterThanOrEqualValidator", typeof(ValidationAttribute), GreaterOrEqualError, "ComparisonValue"),
        new ValidationMessageLink("GreaterThanValidator", typeof(ValidationAttribute), GreaterThanError, "ComparisonValue"),
        new ValidationMessageLink("LengthValidator", typeof(LengthAttribute), LengthError, "MaxLength", "MinLength"),
        new ValidationMessageLink("MadeUpNotExistingValidator", typeof(StringLengthAttribute), LengthError, "MaxLength", "MinLength"),
        new ValidationMessageLink("MinimumLengthValidator", typeof(MinLengthAttribute), MinLengthError, "MinLength"),
        new ValidationMessageLink("MaximumLengthValidator", typeof(MaxLengthAttribute), MaxLengthError, "MaxLength"),
        new ValidationMessageLink("LessThanOrEqualValidator", typeof(ValidationAttribute), LessOrEqualError, "ComparisonValue"),
        new ValidationMessageLink("LessThanValidator", typeof(ValidationAttribute), LessThanError, "ComparisonValue"),
        new ValidationMessageLink("NotEmptyValidator", typeof(RequiredAttribute), RequiredError),
        new ValidationMessageLink("NotEqualValidator", typeof(ValidationAttribute), NotEqualError, "ComparisonValue"),
        new ValidationMessageLink("NotNullValidator", typeof(RequiredAttribute), RequiredError),
        new ValidationMessageLink("PredicateValidator", typeof(ValidationAttribute), NotValidError),
        new ValidationMessageLink("AsyncPredicateValidator", typeof(ValidationAttribute), NotValidError),
        new ValidationMessageLink("RegularExpressionValidator", typeof(ValidationAttribute), "{0} is not in the correct format."),
        new ValidationMessageLink("EqualValidator", typeof(ValidationAttribute), EqualError, "ComparisonValue"),
        new ValidationMessageLink("ExactLengthValidator", typeof(ValidationAttribute), ExactLengthError, "MaxLength"),
        new ValidationMessageLink("InclusiveBetweenValidator", typeof(ValidationAttribute), InclusiveBetweenError, "From", "To"),
        new ValidationMessageLink("ExclusiveBetweenValidator", typeof(ValidationAttribute), ExclusiveBetweenError, "From", "To"),
        new ValidationMessageLink("CreditCardValidator", typeof(CreditCardAttribute), CreditCardError),
        new ValidationMessageLink("ScalePrecisionValidator", typeof(ValidationAttribute), "{0} must not be more than {ExpectedPrecision} digits in total, with allowance for {ExpectedScale} decimals. {Digits} digits and {ActualScale} decimals were found."),
        new ValidationMessageLink("EmptyValidator", typeof(ValidationAttribute), EmptyError),
        new ValidationMessageLink("NullValidator", typeof(ValidationAttribute), EmptyError),
        new ValidationMessageLink("EnumValidator", typeof(RangeAttribute), NotInRangeError, "PropertyValue"),
        // Additional fallback messages used by clientside validation integration.
        new ValidationMessageLink("Length_Simple", typeof(LengthAttribute), LengthError, "MaxLength", "MinLength"),
        new ValidationMessageLink("MinimumLength_Simple", typeof(MinLengthAttribute), MinLengthError, "MinLength"),
        new ValidationMessageLink("MaximumLength_Simple", typeof(MaxLengthAttribute), MaxLengthError, "MaxLength"),
        new ValidationMessageLink("ExactLength_Simple", typeof(ValidationAttribute), ExactLengthError, "MaxLength"),
        new ValidationMessageLink("InclusiveBetween_Simple", typeof(ValidationAttribute), InclusiveBetweenError),
        // Custom validators
        new ValidationMessageLink("NotNullOrEmptyValidator", typeof(ValidationAttribute), RequiredError)
    ];

    internal static List<ValidationMessageLink> AttributeMessageLinks = MessageLinks
            .Where(x => x.DataAttribute != null && x.DataAttribute?.UnderlyingSystemType != typeof(ValidationAttribute))
            .DistinctBy(x => x.DataAttribute)
            .ToList();
}