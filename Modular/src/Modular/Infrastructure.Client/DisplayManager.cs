namespace Infrastructure
{
    using System.Windows.Controls;

    public class DisplayManager : IDisplayManager
    {
        private readonly BusyIndicator busyIndicator;

        public DisplayManager()
        {
            this.busyIndicator = new BusyIndicator();
        }

        public void ShowBusy()
        {
            this.busyIndicator.IsBusy = true;
        }

        public void HideBusy()
        {
            this.busyIndicator.IsBusy = false;
        }
    }
}