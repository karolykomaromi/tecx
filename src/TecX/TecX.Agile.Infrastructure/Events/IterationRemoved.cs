using System;
using System.Runtime.Serialization;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class IterationRemoved : IDomainEvent
    {
        [DataMember]
        private readonly Guid _iterationId;
        [DataMember]
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