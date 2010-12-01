using System;

namespace TecX.Agile.Infrastructure.Events
{
    public class IterationAdded : IDomainEvent
    {
        private readonly Guid _iterationId;
        private readonly Guid _to;

        public Guid To
        {
            get { return _to; }
        }

        public Guid IterationId
        {
            get { return _iterationId; }
        }

        public IterationAdded(Guid iterationId, Guid to)
        {
            _iterationId = iterationId;
            _to = to;
        }
    }
}