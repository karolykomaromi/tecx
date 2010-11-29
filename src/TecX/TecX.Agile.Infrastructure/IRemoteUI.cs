using TecX.Agile.Infrastructure.Events;
using TecX.Common.Event;

namespace TecX.Agile.Infrastructure
{
    public interface IRemoteUI : 
        ISubscribeTo<PropertyChanged>, 
        ISubscribeTo<StoryCardRescheduled>, 
        ISubscribeTo<StoryCardPostponed>, 
        ISubscribeTo<FieldHighlighted>
    {
    }
}