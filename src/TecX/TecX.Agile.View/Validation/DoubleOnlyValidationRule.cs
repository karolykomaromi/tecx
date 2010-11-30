using System.Globalization;
using System.Windows.Controls;

namespace TecX.Agile.View.Validation
{
    public class DoubleOnlyValidationRule : ValidationRule
    {
        #region Overrides of ValidationRule

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string inputString = value as string;

            if(string.IsNullOrEmpty(inputString))
            {
                return new ValidationResult(true, null);
            }

            double inputNumber;
            if(!double.TryParse(inputString, NumberStyles.Float, cultureInfo, out inputNumber))
            {
                return new ValidationResult(false, "Only numbers are allowed");
            }

            return new ValidationResult(true, null);
        }

        #endregion
    }
}
