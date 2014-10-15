namespace Hydra.Commands
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(CommandHandlerContract<,>))]
    public interface ICommandHandler<in TCommand, out TResult> 
        where TCommand : class, ICommand<TResult>
    {
        TResult Handle(TCommand command);
    }

    [ContractClassFor(typeof(ICommandHandler<,>))]
    internal abstract class CommandHandlerContract<TCommand, TResult> : ICommandHandler<TCommand, TResult> 
        where TCommand : class, ICommand<TResult>
    {
        public TResult Handle(TCommand command)
        {
            Contract.Requires(command != null);

            return default(TResult);
        }
    }
}