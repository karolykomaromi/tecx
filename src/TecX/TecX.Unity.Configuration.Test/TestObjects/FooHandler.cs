namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class FooHandler : ICommandHandler<FooCommand>
    {
        public void Handle(FooCommand command)
        {
            command.HandledBy += @"-->FooHandler";
        }
    }
}