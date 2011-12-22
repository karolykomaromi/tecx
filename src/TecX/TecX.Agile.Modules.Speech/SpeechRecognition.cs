namespace TecX.Agile.Modules.Speech
{
    using System.Speech.Recognition;

    using TecX.Common;
    using TecX.Event;

    public class SpeechRecognition
    {
        private readonly IEventAggregator eventAggregator;

        private readonly SpeechRecognizer recognizer;

        public SpeechRecognition(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.eventAggregator = eventAggregator;
            this.recognizer = new SpeechRecognizer
                                   {
                                       PauseRecognizerOnRecognition = true
                                   };

            this.InitializeSpeechRecognizer();
        }

        private void InitializeSpeechRecognizer()
        {
            this.recognizer.UnloadAllGrammars();


            GrammarBuilder ssi = new GrammarBuilder(VoiceCommands.ShowSystemInfo) { Culture = Defaults.Culture };

            Grammar showSystemInfo = new Grammar(ssi);

            showSystemInfo.SpeechRecognized += this.OnShowSystemInfo;

            this.recognizer.LoadGrammar(showSystemInfo);


            GrammarBuilder @as = new GrammarBuilder(VoiceCommands.AddStoryCard) { Culture = Defaults.Culture };

            Grammar addStoryCard = new Grammar(@as);

            addStoryCard.SpeechRecognized += this.OnAddStoryCard;

            this.recognizer.LoadGrammar(addStoryCard);
        }

        private void OnShowSystemInfo(object sender, SpeechRecognizedEventArgs e)
        {
            //this.dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            //                                                                  {
            //                                                                      if (Commands.ShowSystemInfo.CanExecute(null))
            //                                                                          Commands.ShowSystemInfo.Execute(null);
            //                                                                  }));
        }

        private void OnAddStoryCard(object sender, SpeechRecognizedEventArgs e)
        {
            ////TODO weberse 2011-01-14 should put the card in the center of the screen in the future
            //StoryCardAdded commandArgs = new StoryCardAdded(Guid.NewGuid(), Guid.Empty, 0.0, 0.0, 0.0);

            //this.dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
            //                                                             {
            //                                                                 if (Commands.AddStoryCard.CanExecute(commandArgs))
            //                                                                     Commands.AddStoryCard.Execute(commandArgs);
            //                                                             }));
        }
    }
}
