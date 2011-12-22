namespace TecX.Agile.Modules.Speech
{
    using System;
    using System.Speech.Recognition;

    using TecX.Agile.Modules.Speech.Recognition;
    using TecX.Common;
    using TecX.Event;

    public class SpeechRecognition
    {
        private readonly IEventAggregator eventAggregator;

        private readonly SpeechStrategyChain chain;

        public SpeechRecognition(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.eventAggregator = eventAggregator;

            this.chain = new SpeechStrategyChain().Initialize();

            this.chain.SpeechRecognized += this.OnRecognized;
        }

        private void OnRecognized(object sender, EventArgs e)
        {
            SpeechRecognitionContext context = new SpeechRecognitionContext();

            this.chain.Process(context);

            if (context.RecognitionCompleted && context.Message != null)
            {
                MessagePublisher publisher = new MessagePublisher(this.eventAggregator);

                publisher.Publish(context.Message);
            }
        }
    }
}
