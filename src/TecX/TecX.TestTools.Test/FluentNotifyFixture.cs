using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.TestTools.Test.TestObjects;

namespace TecX.TestTools.Test
{
    [TestClass]
    public class FluentNotifyFixture
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void WhenCallingAction_ShouldNotifyViaEvent()
        {
            var sut = new AsyncTestClass();

            sut.AfterCalling(a => a.DoWork("first", "second"))
                .ShouldNotifyVia(
                    (EventHandler<CompletedEventArgs> ev) => new CompletedEventHandler(ev),
                    ev => sut.Completed += ev,
                    ev => sut.Completed -= ev)
                .AssertExpectations();
        }

        [TestMethod]
        public void WhenCallingAction_ShouldNotifyViaEventIdentifiedByEventName()
        {
            var sut = new AsyncTestClass();

            sut.AfterCalling(a => a.DoWork("first", "second"))
                .ShouldNotifyVia<CompletedEventArgs>("Completed")
                .AssertExpectations();
        }

        [TestMethod]
        public void WhenCallingAction_ShouldNotifyWithSpecifiedEventArgs()
        {
            var sut = new AsyncTestClass();

            sut.AfterCalling(a => a.DoWork("a", "b"))
                .ShouldNotifyVia(
                    (EventHandler<CompletedEventArgs> ev) => new CompletedEventHandler(ev),
                    ev => sut.Completed += ev,
                    ev => sut.Completed -= ev)
                .WithArgs(args => Assert.AreEqual("ab", args.Prop1))
                .AssertExpectations();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void WhenFailingToVerifyEventArgs_FinishesAssertExpectationsNonetheless()
        {
            var sut = new AsyncTestClass();

            sut.AfterCalling(a => a.DoWork("a", "b"))
                .ShouldNotifyVia(
                    (EventHandler<CompletedEventArgs> ev) => new CompletedEventHandler(ev),
                    ev => sut.Completed += ev,
                    ev => sut.Completed -= ev)
                .WithArgs(args => { throw new Exception(); })
                .AssertExpectations();
        }
    }
}