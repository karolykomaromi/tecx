using TecX.Agile.Infrastructure.Events;

namespace TecX.Agile.Remote
{
    public interface IMessageFilter<in TMessage>
        where TMessage : IDomainEvent
    {
        bool ShouldLetPass(TMessage outboundMessage);

        void Enqueue(TMessage inboundMessage);
    }
}