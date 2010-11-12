using System;

namespace TecX.Common.Event
{
    public interface IMessageFilter<in TMessage>
    {
        Predicate<TMessage> ShouldHandle { get; }
    }
}