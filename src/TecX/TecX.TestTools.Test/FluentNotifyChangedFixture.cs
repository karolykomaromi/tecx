namespace TecX.TestTools.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.TestTools.Test.TestObjects;

    [TestClass]
    public class FluentNotifyChangedFixture
    {
        [TestMethod]
        public void ChangingMyPropertyWillRaiseNotifyEvent()
        {
            // Fixture setup
            var sut = new MyClass();

            // Exercise system and verify outcome
            sut.ShouldNotifyOn(s => s.MyProperty).When(s => s.MyProperty = "Some new value");
        }

        [TestMethod]
        public void ChangingMyPropertyWillRaiseNotifyForDerived()
        {
            // Fixture setup
            var sut = new MyClass();

            // Exercise system and verify outcome
            sut.ShouldNotifyOn(s => s.MyDerivedProperty).When(s => s.MyProperty = "Some new value");
        }
    }
}
