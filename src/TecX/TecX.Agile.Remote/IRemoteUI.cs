﻿using TecX.Agile.Infrastructure.Events;
using TecX.Common.Event;

namespace TecX.Agile.Remote
{
    public interface IRemoteUI : 
        ISubscribeTo<PropertyUpdated>, 
        ISubscribeTo<StoryCardRescheduled>, 
        ISubscribeTo<StoryCardPostponed>, 
        ISubscribeTo<FieldHighlighted>,
        ISubscribeTo<StoryCardMoved>,
        ISubscribeTo<CaretMoved>
    {
    }
}