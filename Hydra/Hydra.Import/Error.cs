namespace Hydra.Import
{
    using System.Diagnostics.Contracts;

    public sealed class Error : ImportMessage
    {
        public Error(string message)
            : base(message)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(message));
        }
    }
}