using TecX.Event;

namespace TecX.Common.Test.TestObjects
{
    internal class CancelingSubscriber : ISubscribeTo<CancelMessage>
    {
        #region Implementation of ISubscribeTo<in CancelMessage>

        public void Handle(CancelMessage message)
        {
            message.Cancel = true;
        }

        #endregion
    }
}