namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class BarHandler : ICommandHandler<BarCommand>
    {
        public void Handle(BarCommand command)
        {
            command.HandledBy += @"-->BarHandler";
        }
    }
}