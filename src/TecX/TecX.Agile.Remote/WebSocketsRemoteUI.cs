using System.Net;
using System.ServiceModel.WebSockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TecX.Agile.Infrastructure.Events;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.Remote
{
    public class WebSocketsRemoteUI : IRemoteUI
    {
        private readonly WebSocket _socket;
        private readonly MessageHandlerChain _handlerChain;

        public WebSocketsRemoteUI(string url, MessageHandlerChain handlerChain)
        {
            Guard.AssertNotEmpty(url, "url");
            Guard.AssertNotNull(handlerChain, "handlerChain");

            _socket = new WebSocket(url);
            _socket.Open();

            _socket.OnData += OnData;

            _handlerChain = handlerChain;
        }

        private void OnData(object sender, WebSocketEventArgs e)
        {
            string message = e.TextData;

            if (!_handlerChain.Handle(message))
            {
                //TODO weberse 2011-04-29 throw or just log a message?
            }
        }

        private void SendMessage(IDomainEvent @event)
        {
            Guard.AssertNotNull(@event, "@event");

            string msg = SerializationHelper.Serialize(@event);

            _socket.SendMessage(msg);
        }

        public void Handle(PropertyUpdated message)
        {
            SendMessage(message);
        }

        public void Handle(StoryCardRescheduled message)
        {
            SendMessage(message);
        }

        public void Handle(StoryCardPostponed message)
        {
            SendMessage(message);
        }

        public void Handle(FieldHighlighted message)
        {
            SendMessage(message);
        }

        public void Handle(StoryCardMoved message)
        {
            SendMessage(message);
        }

        public void Handle(CaretMoved message)
        {
            SendMessage(message);
        }
    }
}
