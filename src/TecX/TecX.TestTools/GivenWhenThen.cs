namespace TecX.TestTools
{
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A base class for tests written in the BDD style that provide standard
    /// methods to set up test actions and the "when" statements. "Then" is
    /// encapsulated by the [TestMethod]s themselves.
    /// </summary>
    [DebuggerStepThrough]
    public abstract class GivenWhenThen
    {
        [TestInitialize]
        public void MainSetup()
        {
            this.Given();
            this.When();
        }

        [TestCleanup]
        public void MainTeardown()
        {
            this.Teardown();
        }

        /// <summary>
        /// When overridden in a derived class, this method is used to
        /// set up the current state of the specs context.
        /// </summary>
        /// <remarks>This method is called automatically before every test,
        /// before the <see cref="When"/> method.</remarks>
        protected virtual void Given()
        {
        }

        /// <summary>
        /// When overridden in a derived class, this method is used to
        /// perform interactions against the system under test.
        /// </summary>
        /// <remarks>This method is called automatically after <see cref="Given"/>
        /// and before each test method runs.</remarks>
        protected virtual void When()
        {
        }

        /// <summary>
        /// When overridden in a derived class, this method is used to
        /// reset the state of the system after a test method has completed.
        /// </summary>
        /// <remarks>This method is called automatically after each TestMethod has run.</remarks>
        protected virtual void Teardown()
        {
        }
    }
}
