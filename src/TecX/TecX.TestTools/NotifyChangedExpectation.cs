namespace TecX.TestTools
{
    using System;
    using System.ComponentModel;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common;

    public class NotifyChangedExpectation<T> where T : INotifyPropertyChanged
    {
        private readonly T owner;
        private readonly string propertyName;
        private readonly bool eventExpected;

        public NotifyChangedExpectation(T owner, string propertyName, bool eventExpected)
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotEmpty(propertyName, "propertyName");

            this.owner = owner;
            this.propertyName = propertyName;
            this.eventExpected = eventExpected;
        }

        public void When(Action<T> action)
        {
            Guard.AssertNotNull(action, "action");

            bool eventWasRaised = false;

            this.owner.PropertyChanged += (sender, e) =>
                                          {
                                              if (e.PropertyName == this.propertyName)
                                              {
                                                  eventWasRaised = true;
                                              }
                                          };

            action(this.owner);

            Assert.AreEqual(this.eventExpected, eventWasRaised, "PropertyChanged on {0}", this.propertyName);
        }
    }
}