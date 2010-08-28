using TecX.Common.Event;

namespace TecX.Common.Test.TestClasses
{
    class CancelMessage : ICancellationToken
    {
        public bool Cancel { get; set; }
    }
}