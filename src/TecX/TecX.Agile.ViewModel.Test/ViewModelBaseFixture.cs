using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Agile.ViewModel.Test.TestObjects;
using TecX.TestTools;

namespace TecX.Agile.ViewModel.Test
{
    [TestClass]
    public class ViewModelBaseFixture
    {
        [TestMethod]
        public void WhenPropertyValueChanged_NotifyChangedEventFires()
        {
            TestViewModel sut = new TestViewModel();

            sut.ShouldNotifyOn(s => s.Foo)
                .When(s => s.Foo = "Some value");

            Assert.AreEqual("Some value", sut.Foo);
        }
    }
}
