namespace TecX.Agile.Phone
{
    using System;
    using System.Windows;
    using System.Windows.Threading;

    public class DefaultDispatcher : IDispatcher
    {
        private readonly Dispatcher dispatcher;

        public DefaultDispatcher()
        {
            this.dispatcher = Deployment.Current.Dispatcher;
        }

        public void BeginInvoke(Action action)
        {
            Guard.AssertNotNull(action, "action");

            this.dispatcher.BeginInvoke(action);
        }

        public void BeginInvoke(Delegate d, params object[] args)
        {
            Guard.AssertNotNull(d, "d");

            this.dispatcher.BeginInvoke(d, args);
        }
    }
}
