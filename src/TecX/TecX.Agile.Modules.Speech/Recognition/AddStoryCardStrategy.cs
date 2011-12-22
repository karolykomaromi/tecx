namespace TecX.Agile.Modules.Speech.Recognition
{
    using System;
    using System.Speech.Recognition;

    using TecX.Agile.Infrastructure.Commands;

    using Defaults = TecX.Agile.Modules.Speech.Defaults;

    public class AddStoryCardStrategy : SpeechRecognitionStrategy
    {
        public AddStoryCardStrategy()
        {
            GrammarBuilder builder = new GrammarBuilder(VoiceCommands.AddStoryCard) { Culture = Defaults.Culture };

            this.Grammar = new Grammar(builder);

            this.Grammar.SpeechRecognized += this.OnRecognized;
        }

        private void OnRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // TODO weberse 2011-12-22 check wether confidence is good enough

            //Point center = Surface.Current.Center;

            this.Message = new AddStoryCard(Guid.NewGuid(), 0.0, 0.0, 0.0);
        }
    }
}
