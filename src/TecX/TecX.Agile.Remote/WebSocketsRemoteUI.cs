using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.WebSockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;

using TecX.Agile.Infrastructure.Events;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.Remote
{
    public class WebSocketsRemoteUI : IRemoteUI
    {
        private readonly WebSocket _socket;
        private readonly Dictionary<Type, DataContractSerializer> _serializers;
        private readonly MessageHandlerChain _handlerChain;

        public WebSocketsRemoteUI(string url)
        {
            Guard.AssertNotEmpty(url, "url");

            _socket = new WebSocket(url);
            _socket.Open();

            _socket.OnData += OnData;

            _serializers = new Dictionary<Type, DataContractSerializer>();
        }

        private void OnData(object sender, WebSocketEventArgs e)
        {
            string message = e.TextData;

            if(!_handlerChain.Handle(message))
            {
                //TODO weberse 2011-04-29 throw or just log a message?
            }
        }

        public void Handle(PropertyUpdated message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StoryCardRescheduled message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StoryCardPostponed message)
        {
            throw new NotImplementedException();
        }

        public void Handle(FieldHighlighted message)
        {
            throw new NotImplementedException();
        }

        public void Handle(StoryCardMoved message)
        {
            throw new NotImplementedException();
        }

        public void Handle(CaretMoved message)
        {
            throw new NotImplementedException();
        }
    }

    public class MessageHandlerChain
    {
        private readonly List<MessageHandler> _handlers;

        public MessageHandlerChain(IEnumerable<MessageHandler> handlers)
        {
            Guard.AssertNotNull(handlers, "handlers");

            _handlers = new List<MessageHandler>(handlers);
        }

        public bool Handle(string message)
        {
            Guard.AssertNotEmpty(message, "message");

            foreach(var handler in _handlers)
            {
                if(handler.CanHandle(message))
                {
                    handler.Handle(message);

                    return true;
                }
            }

            return false;
        }
    }

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

            bool canHandle = typeof (TMessage).Name == typeName;

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
