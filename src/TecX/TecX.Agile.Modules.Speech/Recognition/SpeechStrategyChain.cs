namespace TecX.Agile.Modules.Speech.Recognition
{
    using System;
    using System.Collections.Generic;
    using System.Speech.Recognition;

    using TecX.Common;

    public class SpeechStrategyChain
    {
        private readonly SpeechRecognizer recognizer;

        private readonly List<SpeechRecognitionStrategy> strategies;

        public SpeechStrategyChain()
        {
            this.recognizer = new SpeechRecognizer
                                  {
                                      PauseRecognizerOnRecognition = true
                                  };

            this.recognizer.UnloadAllGrammars();

            this.recognizer.SpeechRecognized += this.OnRecognized;

            this.strategies = new List<SpeechRecognitionStrategy>();
        }

        public event EventHandler SpeechRecognized = delegate { };

        public void Add(SpeechRecognitionStrategy strategy)
        {
            Guard.AssertNotNull(strategy, "strategy");
            Guard.AssertNotNull(strategy.Grammar, "strategy.Grammar");

            this.strategies.Add(strategy);
            this.recognizer.LoadGrammar(strategy.Grammar);
        }

        public void Process(SpeechRecognitionContext context)
        {
            Guard.AssertNotNull(context, "context");

            for (int i = 0; i < this.strategies.Count; i++)
            {
                this.strategies[i].Process(context);

                if (context.RecognitionCompleted)
                {
                    return;
                }
            }
        }

        private void OnRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            this.SpeechRecognized(this, EventArgs.Empty);
        }
    }
}