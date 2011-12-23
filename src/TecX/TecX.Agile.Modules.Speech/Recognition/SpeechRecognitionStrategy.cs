﻿namespace TecX.Agile.Modules.Speech.Recognition
{
    using System.Speech.Recognition;

    using TecX.Common;

    public abstract class SpeechRecognitionStrategy
    {
        protected object Message { get; set; }

        public Grammar Grammar { get; protected set; }

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