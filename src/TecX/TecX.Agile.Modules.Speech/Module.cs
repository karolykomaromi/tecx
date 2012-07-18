using System.Windows.Threading;

using Microsoft.Practices.Prism.Modularity;

using TecX.Common;

namespace TecX.Agile.Moduls.Speech
{
    public class Module : IModule
    {
        private readonly Dispatcher _dispatcher;
        private SpeechRecognition _speechRecognition;

        public Module(Dispatcher dispatcher)
        {
            Guard.AssertNotNull(dispatcher, "dispatcher");

            _dispatcher = dispatcher;
        }

        public void Initialize()
        {
            _speechRecognition = new SpeechRecognition(_dispatcher);
        }
    }
}
