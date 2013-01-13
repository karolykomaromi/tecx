namespace TecX.Agile.Phone.ViewModels
{
    using System.Windows;

    using Caliburn.Micro;

    using Microsoft.Phone.Tasks;

    public class TabViewModel : Screen, IHandle<TaskCompleted<PhoneNumberResult>>
    {
        string text;
        readonly IEventAggregator events;

        public TabViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                this.NotifyOfPropertyChange(() => this.Text);
            }
        }

        public void Choose()
        {
            this.events.RequestTask<PhoneNumberChooserTask>();
        }

        public void Handle(TaskCompleted<PhoneNumberResult> message)
        {
            MessageBox.Show("The result was " + message.Result.TaskResult, this.DisplayName, MessageBoxButton.OK);
        }

        protected override void OnActivate()
        {
            this.events.Subscribe(this);
            base.OnActivate();
        }

        protected override void OnDeactivate(bool close)
        {
            this.events.Unsubscribe(this);
            base.OnDeactivate(close);
        }
    }
}
