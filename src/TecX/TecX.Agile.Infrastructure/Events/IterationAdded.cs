using System;
using System.Runtime.Serialization;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class IterationAdded : IDomainEvent
    {
        [DataMember]
        private readonly Guid _iterationId;
        [DataMember]
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