namespace TecX.Unity.Configuration.Test.TestObjects
{
    public interface ICommandHandler<in TCommand> where TCommand : class
    {
        void Handle(TCommand command);
    }
}