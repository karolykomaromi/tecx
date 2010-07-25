using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common;

namespace TecX.TestTools
{
    public class NotifyCompletedExpectation<T>
    {
        #region Fields

        private readonly T _owner;
        private readonly Action<T> _action;
        private bool _eventRaised;
        private readonly ManualResetEvent _manualResetEvent;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region c'tor

        public NotifyCompletedExpectation(T owner, Action<T> action)
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotNull(action, "action");

            _owner = owner;
            _action = action;

            _eventRaised = false;
            _manualResetEvent = new ManualResetEvent(false);
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        public void ShouldNotifyVia<TEventArgs>(string eventName)
            where TEventArgs : EventArgs
        {
            Guard.AssertNotEmpty(eventName, "eventName");

            var observable = Observable.FromEvent<TEventArgs>(_owner, eventName);

            AssertNotified(observable);
        }

        public void ShouldNotifyVia<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion,
                                                           Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
            where TEventArgs : EventArgs
        {
            Guard.AssertNotNull(conversion, "conversion");
            Guard.AssertNotNull(addHandler, "addHandler");
            Guard.AssertNotNull(removeHandler, "removeHandler");

            IObservable<IEvent<TEventArgs>> observable = Observable.FromEvent(conversion, addHandler, removeHandler);

            AssertNotified(observable);
        }

        private void AssertNotified<TEventArgs>(IObservable<IEvent<TEventArgs>> observable)
            where TEventArgs : EventArgs
        {
            _eventRaised = false;

            using (observable.Subscribe(e =>
                                            {
                                                _eventRaised = true;
                                                _manualResetEvent.Set();
                                            }))
            {
                _action(_owner);
                _manualResetEvent.WaitOne(1500, false);
            }

            Assert.IsTrue(_eventRaised);
        }

        //public void ShouldNotifyVia<TEventArgs>(Action<T> addOrRemoveHandler)
        //    where TEventArgs : EventArgs
        //{
        //    if (addOrRemoveHandler == null) throw new ArgumentException("addHandler");

        //    string eventName = GetEventName(addOrRemoveHandler);

        //    ShouldNotifyVia<TEventArgs>(eventName);
        //}

        //private static string GetEventName(Action<T> addHandler)
        //{
        //    Globals.LoadOpCodes();
        //    var mr = new MethodBodyReader(addHandler.Method);
        //    var i = mr.instructions;

        //    foreach (var ilInstruction in i)
        //    {
        //        var methodInfo = ilInstruction.Operand as System.Reflection.MethodInfo;
        //        if (ilInstruction.Code.Name != "callvirt" || methodInfo == null)
        //        {
        //            continue;
        //        }

        //        var name = methodInfo.Name;
        //        if (name.StartsWith("add_"))
        //        {
        //            return name.Substring(4);
        //        }
        //    }

        //    return string.Empty;
        //}
    }
}