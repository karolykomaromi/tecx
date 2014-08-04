namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class TransactionHandler<TCommand> : ICommandHandler<TCommand> where TCommand : BaseCommand
    {
        private readonly ICommandHandler<TCommand> inner;

        public TransactionHandler(ICommandHandler<TCommand> inner)
        {
            this.inner = inner;
        }

        public void Handle(TCommand command)
        {
            command.HandledBy += @"-->Transaction";
            this.inner.Handle(command);
        }
    }
}