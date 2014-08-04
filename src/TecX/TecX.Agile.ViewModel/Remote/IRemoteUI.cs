using TecX.Agile.ViewModel.Messages;
using TecX.Common.Event;

namespace TecX.Agile.ViewModel.Remote
{
    public interface IRemoteUI : 
        ISubscribeTo<PropertyChanged>, 
        ISubscribeTo<StoryCardRescheduled>, 
        ISubscribeTo<StoryCardPostponed>, 
        ISubscribeTo<FieldHighlighted>
    {
    }
}