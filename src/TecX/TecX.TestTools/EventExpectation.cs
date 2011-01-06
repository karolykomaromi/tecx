using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common;

namespace TecX.TestTools
{
    public class EventExpectation<T, TEventArgs>
        where TEventArgs : EventArgs
    {
        private bool _eventRaised;
        private readonly ManualResetEvent _manualResetEvent;
        private readonly Action<T> _action;

        private readonly T _owner;
        private readonly IObservable<IEvent<TEventArgs>> _observable;
        private Action<TEventArgs> _verify;

        public EventExpectation(T owner, Action<T> action, IObservable<IEvent<TEventArgs>> observable)
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotNull(observable, "observable");
            Guard.AssertNotNull(owner, "owner");

            this._action = action;
            this._owner = owner;
            this._observable = observable;

            _eventRaised = false;
            _manualResetEvent = new ManualResetEvent(false);
        }

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
                                                     if(_verify != null)
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