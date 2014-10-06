namespace TecX.Agile.Modules.Speech.Recognition
{
    using System.Diagnostics;
    using System.Speech.Recognition;

    using TecX.Common;

    [DebuggerDisplay("{Description}")]
    public abstract class SpeechRecognitionStrategy
    {
        public Grammar Grammar { get; protected set; }

        public abstract string Description { get; }

        protected object Message { get; set; }

        public void Process(SpeechRecognitionContext context)
        {
            Guard.AssertNotNull(context, "context");

            if (this.Message != null)
            {
                context.Message = this.Message;
                context.RecognitionCompleted = true;

                // reset the recognized message or the next time the chain is traversed an
                // old message might be returned!
                this.Message = null;
            }
        }
    }
}
