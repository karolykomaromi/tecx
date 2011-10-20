using Caliburn.Micro;

using TecX.Agile.Infrastructure;
using TecX.Common;

namespace TecX.Agile.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell
    {
        private readonly BindableCollection<Screen> _overlays;

        public ShellViewModel()
        {
            _overlays = new BindableCollection<Screen>();
        }

        public IObservableCollection<Screen> Overlays
        {
            get
            {
                return _overlays;
            }
        }

        private int counter = 0;

        public void AddItem()
        {
            if (counter % 2 == 0)
            {
                Items.Add(new StoryCardViewModel());
            }
            else
            {
                Items.Add(new IterationViewModel());
            }

            counter++;
        }

        public bool CanAddItem
        {
            get
            {
                return true;
            }
        }

        public void AddOverlay(Screen overlay)
        {
            Guard.AssertNotNull(overlay, "overlay");

            Overlays.Add(overlay);
        }
    }
}
