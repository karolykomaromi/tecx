using System;

namespace TecX.Agile.Infrastructure.Events
{
    public class IterationRemoved : IDomainEvent
    {
        private readonly Guid _iterationId;
        private readonly Guid _from;

        public Guid From
        {
            get { return _from; }
        }

        public Guid IterationId
        {
            get { return _iterationId; }
        }

        public IterationRemoved(Guid iterationId, Guid from)
        {
            _iterationId = iterationId;
            _from = from;
        }
    }
}