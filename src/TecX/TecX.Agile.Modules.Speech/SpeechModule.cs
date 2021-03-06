﻿namespace TecX.Agile.Modules.Speech
{
    using System.Diagnostics;

    using TecX.CaliburnEx.Modularization;
    using TecX.Common;

    [DebuggerDisplay("{Description")]
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
