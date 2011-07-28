using System;
using System.Threading;
using System.Reactive;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common;

namespace TecX.TestTools
{
    public class EventExpectation<T, TEventArgs>
        where TEventArgs : EventArgs
    {
        #region Fields

        private bool _eventRaised;
        private readonly ManualResetEvent _manualResetEvent;
        private readonly Action<T> _action;

        private readonly T _owner;
        private readonly IObservable<EventPattern<TEventArgs>> _observable;
        private Action<TEventArgs> _verify;

        #endregion Fields

        #region c'tor

        public EventExpectation(T owner, Action<T> action, IObservable<EventPattern<TEventArgs>> observable)
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotNull(observable, "observable");
            Guard.AssertNotNull(owner, "owner");

            _action = action;
            _owner = owner;
            _observable = observable;

            _eventRaised = false;
            _manualResetEvent = new ManualResetEvent(false);
        }

        #endregion c'tor

        public EventExpectation<T, TEventArgs> WithArgs(Action<TEventArgs> verify)
        {
            Guard.AssertNotNull(verify, "verify");

            _verify = verify;

            return this;
        }

        public void AssertExpectations()
        {
            _eventRaised = false;

            using (_observable.Subscribe(e =>
                                             {
                                                 try
                                                 {
                                                     if (_verify != null)
                                                     {
                                                         _verify(e.EventArgs);
                                                     }

                                                     _eventRaised = true;
                                                 }
                                                 finally
                                                 {
                                                     _manualResetEvent.Set();
                                                 }
                                             }))
            {
                _action(_owner);
                _manualResetEvent.WaitOne(1500, false);
            }

            Assert.IsTrue(_eventRaised);
        }
    }
}