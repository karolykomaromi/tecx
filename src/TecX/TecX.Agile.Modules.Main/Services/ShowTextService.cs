#if SILVERLIGHT

using System.Threading;

#else

using System;
using System.Timers;

#endif

using TecX.Agile.Infrastructure.Services;
using TecX.Agile.ViewModel;

namespace TecX.Agile.Modules.Main.Services
{
    public class ShowTextService : ViewModelBase, IShowText
    {
        private string _text;

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

#if SILVERLIGHT

        private Timer _timer;

        public void Show(string text)
        {
            Text = text;

            _timer = new Timer(OnElapsed);

        }

        private void OnElapsed(object state)
        {
            Text = string.Empty;

            _timer.Dispose();

            _timer = null;
        }

#else
        private readonly Timer _timer;

        public ShowTextService()
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
#endif
    }
}
