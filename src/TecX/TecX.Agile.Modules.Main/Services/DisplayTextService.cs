using System.Timers;

using TecX.Agile.Infrastructure.Services;
using TecX.Agile.ViewModel;

namespace TecX.Agile.Modules.Main.Services
{
    public class DisplayTextService : ViewModelBase, IDisplayText
    {
        private string _text;
        private readonly Timer _timer;

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                    return;

                _text = value;

                OnPropertyChanged(() => Text);
            }
        }

        public DisplayTextService()
        {
            _timer = new Timer(10000.0);
        }

        public void Show(string text)
        {
            Text = text;

            _timer.Elapsed += OnElapsed;

            _timer.Start();
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            Text = string.Empty;

            _timer.Elapsed -= OnElapsed;
        }
    }
}
