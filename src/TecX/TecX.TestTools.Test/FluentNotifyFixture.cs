using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.TestTools.Test
{
    [TestClass]
    public class FluentNotifyFixture
    {
        public FluentNotifyFixture() { }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldNotifyAfterCalling()
        {
            var sut = new AsyncTestClass();

            sut.AfterCalling(a => a.DoWork("first", "second"))
                .ShouldNotifyVia(
                    (EventHandler<CompletedEventArgs> ev) => new CompletedEventHandler(ev),
                    ev => sut.Completed += ev,
                    ev => sut.Completed -= ev);
        }

        [TestMethod]
        public void ShouldNotifyUsingEventName()
        {
            var sut = new AsyncTestClass();

            sut.AfterCalling(a => a.DoWork("first", "second"))
                .ShouldNotifyVia<CompletedEventArgs>("Completed");
        }
    }

    public class CompletedEventArgs : EventArgs
    {
    }

    public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);

    public class AsyncTestClass
    {
        public event CompletedEventHandler Completed = delegate { };

        public void DoWork(string a, string b)
        {
            Thread.Sleep(1200);

            Completed(null, new CompletedEventArgs());
        }
    }
}