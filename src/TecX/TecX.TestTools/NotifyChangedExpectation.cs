using System;
using System.ComponentModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common;

namespace TecX.TestTools
{
    public class NotifyChangedExpectation<T> where T : INotifyPropertyChanged
    {
        #region Fields

        private readonly T _owner;
        private readonly string _propertyName;
        private readonly bool _eventExpected;

        #endregion Fields

        #region c'tor

        public NotifyChangedExpectation(T owner, string propertyName, bool eventExpected)
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotEmpty(propertyName, "propertyName");

            _owner = owner;
            _propertyName = propertyName;
            _eventExpected = eventExpected;
        }

        #endregion c'tor

        public void When(Action<T> action)
        {
            Guard.AssertNotNull(action, "action");

            bool eventWasRaised = false;

            _owner.PropertyChanged += (sender, e) =>
                                          {
                                              if (e.PropertyName == _propertyName)
                                              {
                                                  eventWasRaised = true;
                                              }
                                          };

            action(_owner);
            Assert.AreEqual(_eventExpected, eventWasRaised, "PropertyChanged on {0}",
                _propertyName);
        }
    }
}