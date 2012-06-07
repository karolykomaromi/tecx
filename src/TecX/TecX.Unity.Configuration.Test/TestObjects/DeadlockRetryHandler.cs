namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class DeadlockRetryHandler<TCommand> : ICommandHandler<TCommand> where TCommand : BaseCommand
    {
        private readonly ICommandHandler<TCommand> inner;

        public DeadlockRetryHandler(ICommandHandler<TCommand> inner)
        {
            this.inner = inner;
        }

        public void Handle(TCommand command)
        {
            command.HandledBy += @"-->Retry";
            this.inner.Handle(command);
        }
    }
}