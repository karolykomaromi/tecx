using System;

using TecX.Agile.Infrastructure.Events;

namespace TecX.Agile.Remote
{
    class AzureServiceBusMessageRelay : IMessageRelay
    {
        //TODO weberse if I ever finish the wcf implementation of a messageRelay UI this will be the second implementation
        //to cross network segment boundaries
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
