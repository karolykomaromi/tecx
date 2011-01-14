using System;
using System.Globalization;
using System.Speech.Recognition;
using System.Windows.Threading;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Common;

namespace TecX.Agile.Moduls.Speech
{
    public class SpeechRecognition
    {
        #region Constants

        private static class Constants
        {
            public static class VoiceCommands
            {
                /// <summary>add storycard</summary>
                public const string AddStoryCard = "add storycard";

                /// <summary>reveal</summary>
                public const string ShowSystemInfo = "reveal";
            }
        }


        #endregion Constants

        #region Fields

        private readonly Dispatcher _dispatcher;
        private readonly SpeechRecognizer _recognizer;

        #endregion Fields

        #region c'tor

        public SpeechRecognition(Dispatcher dispatcher)
        {
            Guard.AssertNotNull(dispatcher, "dispatcher");

            _dispatcher = dispatcher;

            _recognizer = new SpeechRecognizer
                                   {
                                       PauseRecognizerOnRecognition = true
                                   };

            InitializeSpeechRecognizer();
        }


        #endregion c'tor

        #region Initialization

        private void InitializeSpeechRecognizer()
        {
            _recognizer.UnloadAllGrammars();

            var culture = CultureInfo.CreateSpecificCulture("en-US");


            GrammarBuilder ssi = new GrammarBuilder(Constants.VoiceCommands.ShowSystemInfo) { Culture = culture };

            Grammar showSystemInfo = new Grammar(ssi);

            showSystemInfo.SpeechRecognized += OnShowSystemInfo;

            _recognizer.LoadGrammar(showSystemInfo);


            GrammarBuilder @as = new GrammarBuilder(Constants.VoiceCommands.AddStoryCard) { Culture = culture };

            Grammar addStoryCard = new Grammar(@as);

            addStoryCard.SpeechRecognized += OnAddStoryCard;

            _recognizer.LoadGrammar(addStoryCard);
        }

        #endregion Initialization

        #region EventHandling

        private void OnShowSystemInfo(object sender, SpeechRecognizedEventArgs e)
        {
            _dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                                                                              {
                                                                                  if (Commands.ShowSystemInfo.CanExecute(null))
                                                                                      Commands.ShowSystemInfo.Execute(null);
                                                                              }));
        }

        private void OnAddStoryCard(object sender, SpeechRecognizedEventArgs e)
        {
            //TODO weberse 2011-01-14 should put the card in the center of the screen in the future
            StoryCardAdded commandArgs = new StoryCardAdded(Guid.NewGuid(), Guid.Empty, 0.0, 0.0, 0.0);

            _dispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
                                                                         {
                                                                             if (Commands.AddStoryCard.CanExecute(commandArgs))
                                                                                 Commands.AddStoryCard.Execute(commandArgs);
                                                                         }));
        }

        #endregion EventHandling
    }
}
