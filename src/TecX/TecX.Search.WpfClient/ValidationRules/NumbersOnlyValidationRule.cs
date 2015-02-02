namespace TecX.Search.WpfClient.ValidationRules
{
    using System.Globalization;
    using System.Windows.Controls;

    public class NumbersOnlyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            int number;
            if (!int.TryParse(text, out number))
            {
                return new ValidationResult(false, "Only numbers are allowed.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
