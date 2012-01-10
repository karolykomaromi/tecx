
using System;

using TecX.Agile.Infrastructure.Events;

namespace TecX.Agile.Remote
{
    public class MassTransitMessageRelay : IMessageRelay
    {
        //TODO weberse well this is a bit of day dreaming. i want to have an implementation of a messageRelay ui that uses the mass
        //transit service bus
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
