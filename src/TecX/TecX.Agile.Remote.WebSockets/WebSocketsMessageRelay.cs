using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Agile.Infrastructure.Events;

namespace TecX.Agile.Remote.WebSockets
{
    public class WebSocketsMessageRelay : IMessageRelay
    {
        public WebSocketsMessageRelay()
        {
            
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
}
