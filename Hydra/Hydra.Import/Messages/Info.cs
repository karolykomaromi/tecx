namespace Hydra.Import.Messages
{
    using System.Diagnostics.Contracts;

    public sealed class Info : ImportMessage
    {
        public Info(string message)
            : base(message)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(message));
        }
    }
}