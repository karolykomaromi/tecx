namespace Hydra.Import.Messages
{
    using System.Diagnostics.Contracts;

    public sealed class Warning : ImportMessage
    {
        public Warning(string message)
            : base(message)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(message));
        }
    }
}