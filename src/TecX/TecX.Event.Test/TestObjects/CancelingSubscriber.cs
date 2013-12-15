namespace TecX.Event.Test.TestObjects
{
    using TecX.Event;

    public class CancelingSubscriber : ISubscribeTo<CancelMessage>
    {
        #region Implementation of ISubscribeTo<in CancelMessage>

        public void Handle(CancelMessage message)
        {
            message.Cancel = true;
        }

        #endregion
    }
}