using TecX.Common.Event;

namespace TecX.Common.Test.TestClasses
{
    class CancelingSubscriber : ISubscribeTo<CancelMessage>
    {
        #region Implementation of ISubscribeTo<in CancelMessage>

        public void Handle(CancelMessage message)
        {
            message.Cancel = true;
        }

        #endregion
    }
}