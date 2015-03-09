namespace Hydra.Infrastructure.Mediator
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    [ContractClass(typeof(NotificationHandlerContract<>))]
    public interface INotificationHandler<in TNotification>
        where TNotification : class
    {
        Task Handle(TNotification notification);
    }

    [ContractClassFor(typeof(INotificationHandler<>))]
    internal abstract class NotificationHandlerContract<TNotification> : INotificationHandler<TNotification>
        where TNotification : class
    {
        public Task Handle(TNotification notification)
        {
            Contract.Requires(notification != null);

            return Default.Value<Task>();
        }
    }
}