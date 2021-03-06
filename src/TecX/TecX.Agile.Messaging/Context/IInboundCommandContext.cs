namespace TecX.Agile.Messaging.Context
{
    using System;

    public interface IInboundCommandContext : IDisposable
    {
        object Command { get; }

        bool MatchesEvent(object outboundEvent);
    }

    public interface IInboundCommandContext<out TCommand> : IInboundCommandContext
    {
        new TCommand Command { get; }
    }
}