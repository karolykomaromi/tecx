namespace Hydra.Commands
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    [ContractClass(typeof(CommandHandlerContract<,>))]
    public interface ICommandHandler<in TCommand, TResult> 
        where TCommand : class, ICommand<TResult>
    {
        Task<TResult> Handle(TCommand command);
    }

    [ContractClassFor(typeof(ICommandHandler<,>))]
    internal abstract class CommandHandlerContract<TCommand, TResult> : ICommandHandler<TCommand, TResult> 
        where TCommand : class, ICommand<TResult>
    {
        public Task<TResult> Handle(TCommand command)
        {
            Contract.Requires(command != null);

            return default(Task<TResult>);
        }
    }
}