namespace Hydra.Import
{
    using DocumentFormat.OpenXml.Validation;

    public class ExcelValidationError : Error
    {
        private readonly ValidationErrorInfo validationError;

        public ExcelValidationError(string message, ValidationErrorInfo validationError) 
            : base(message)
        {
            this.validationError = validationError;
        }

        public ValidationErrorInfo ValidationError
        {
            get { return this.validationError; }
        }
    }
}