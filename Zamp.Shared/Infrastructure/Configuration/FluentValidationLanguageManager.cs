using FluentValidation.Resources;

namespace Zamp.Shared.Infrastructure.Configuration;

public class FluentValidationLanguageManager : LanguageManager
{
    public FluentValidationLanguageManager()
    {
        AddTranslation("en", "MaximumLengthValidator", "'{PropertyName}' must be {MaxLength} characters or fewer. You entered {TotalLength}.");
        
        // default messages below; to change one, 1) copy the line to above, 2) uncomment and 3) edit the message
        // AddTranslation("en", "NotNullValidator", "'{PropertyName}' must not be empty.");
        // AddTranslation("en", "NotEmptyValidator", "'{PropertyName}' must not be empty.");
        // AddTranslation("en", "LengthValidator", "'{PropertyName}' must be between {MinLength} and {MaxLength} characters. You entered {TotalLength} characters.");
        // AddTranslation("en", "MinimumLengthValidator", "'{PropertyName}' must be at least {MinLength} characters. You entered {TotalLength} characters.");
        // AddTranslation("en", "MaximumLengthValidator", "'{PropertyName}' must be {MaxLength} characters or fewer. You entered {TotalLength} characters.");
        // AddTranslation("en", "EmailValidator", "'{PropertyName}' is not a valid email address.");
        // AddTranslation("en", "GreaterThanOrEqualValidator", "'{PropertyName}' must be greater than or equal to '{ComparisonValue}'.");
        // AddTranslation("en", "GreaterThanValidator", "'{PropertyName}' must be greater than '{ComparisonValue}'.");
        // AddTranslation("en", "LessThanOrEqualValidator", "'{PropertyName}' must be less than or equal to '{ComparisonValue}'.");
        // AddTranslation("en", "LessThanValidator", "'{PropertyName}' must be less than '{ComparisonValue}'.");
        // AddTranslation("en", "EqualValidator", "'{PropertyName}' must be equal to '{ComparisonValue}'.");
        // AddTranslation("en", "NotEqualValidator", "'{PropertyName}' must not be equal to '{ComparisonValue}'.");
        // AddTranslation("en", "RegularExpressionValidator", "'{PropertyName}' is not in the correct format.");
        // AddTranslation("en", "ExactLengthValidator", "'{PropertyName}' must be {MaxLength} characters in length. You entered {TotalLength} characters.");
        // AddTranslation("en", "InclusiveBetweenValidator", "'{PropertyName}' must be between {From} and {To}. You entered {Value}.");
        // AddTranslation("en", "ExclusiveBetweenValidator", "'{PropertyName}' must be between {From} and {To} (exclusive). You entered {Value}.");
        // AddTranslation("en", "CreditCardValidator", "'{PropertyName}' is not a valid credit card number.");
        // AddTranslation("en", "ScalePrecisionValidator", "'{PropertyName}' must not be more than {ExpectedPrecision} digits in total, with allowance for {ExpectedScale} decimals. {Digits} digits and {ActualScale} decimals were found.");
        // AddTranslation("en", "EmptyValidator", "'{PropertyName}' must be empty.");
        // AddTranslation("en", "NullValidator", "'{PropertyName}' must be empty.");
        // AddTranslation("en", "EnumValidator", "'{PropertyName}' has a range of values which does not include '{PropertyValue}'.");
    }
}