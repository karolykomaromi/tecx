using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.TestTools.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class FluentNotifyChangedFixture
    {
        public FluentNotifyChangedFixture() { }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

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

        private class MyClass : INotifyPropertyChanged
        {
            private string _myProperty;

            public string MyProperty
            {
                get { return _myProperty; }
                set
                {
                    if (value == _myProperty)
                        return;

                    _myProperty = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("MyProperty"));
                    PropertyChanged(this, new PropertyChangedEventArgs("MyDerivedProperty"));
                }
            }

            public string MyDerivedProperty
            {
                get { return MyProperty + "Derived"; }
            }

            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }
    }
}
