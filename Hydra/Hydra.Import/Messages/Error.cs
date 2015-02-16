namespace Hydra.Import.Messages
{
    using System.Diagnostics.Contracts;

    public class Error : ImportMessage
    {
        public Error(string message)
            : base(message)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(message));
        }
    }
}