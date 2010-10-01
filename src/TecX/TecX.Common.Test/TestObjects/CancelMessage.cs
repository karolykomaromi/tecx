using TecX.Common.Event;

namespace TecX.Common.Test.TestObjects
{
    internal class CancelMessage : ICancellationToken
    {
        public bool Cancel { get; set; }
    }
}