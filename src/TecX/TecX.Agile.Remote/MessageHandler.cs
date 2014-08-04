using System.IO;
using System.Runtime.Serialization;
using System.Xml;

using TecX.Agile.Infrastructure.Events;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.Remote
{
    public abstract class MessageHandler
    {
        private readonly IEventAggregator _eventAggregator;

        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
        }

        protected MessageHandler(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");
            _eventAggregator = eventAggregator;
        }

        public abstract bool CanHandle(string message);

        public abstract void Handle(string message);
    }

    public class MessageHandler<TMessage> : MessageHandler
        where TMessage : IDomainEvent
    {
        private readonly DataContractSerializer _serializer;

        public MessageHandler(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _serializer = new DataContractSerializer(typeof(TMessage));
        }

        public override bool CanHandle(string message)
        {
            Guard.AssertNotNull(message, "message");

            int firstBlank = message.IndexOf(" ");

            if (firstBlank < 0)
                return false;

            string typeName = message.Substring(1, firstBlank - 1);

            bool canHandle = typeof(TMessage).Name == typeName;

            return canHandle;
        }

        public override void Handle(string message)
        {
            Guard.AssertNotEmpty(message, "message");

            TextReader tr = new StringReader(message);

            XmlReader reader = XmlReader.Create(tr);

            TMessage msg = (TMessage)_serializer.ReadObject(reader);

            EventAggregator.Publish(msg);
        }
    }
}