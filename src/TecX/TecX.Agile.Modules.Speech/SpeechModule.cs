namespace TecX.Agile.Modules.Speech
{
    using TecX.Common;
    using TecX.CaliburnEx.Modularization;

    public class SpeechModule : Module
    {
        private readonly SpeechRecognition speechRecognition;

        public SpeechModule(SpeechRecognition speechRecognition)
        {
            Guard.AssertNotNull(speechRecognition, "speechRecognition");

            this.speechRecognition = speechRecognition;
        }

        public override string Description
        {
            get
            {
                return "Speech Recognition";
            }
        }
    }
}
