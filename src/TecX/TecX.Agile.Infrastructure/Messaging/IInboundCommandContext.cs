namespace TecX.Agile.Infrastructure.Messaging
{
    using System;

    public interface IInboundCommandContext : IDisposable
    {
        object Command { get; }

        bool MatchesEvent(object outboundEvent);
    }

    public interface IInboundCommandContext<out TCommand> : IInboundCommandContext
    {
        TCommand Command { get; }
    }
}