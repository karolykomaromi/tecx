using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Agile.Domain.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ProjectFixture
    {
        public ProjectFixture()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        public void CanCreateProject()
        {
            Project project = new Project();

            Assert.IsNotNull(project);
        }

        [TestMethod]
        public void HasVision()
        {
            Project project = new Project();

            Assert.IsNotNull(project.Vision);

            project.Vision = "this is the foundation for our future";

            Assert.AreEqual("this is the foundation for our future", project.Vision);
        }

        [TestMethod]
        public void HasName()
        {
            Project project = new Project();

            Assert.IsNotNull(project.Name);

            project.Name = "snappy name";

            Assert.AreEqual("snappy name", project.Name);
        }

        [TestMethod]
        public void HasBacklog()
        {
            Project project = new Project();

            Assert.IsNotNull(project.Backlog);
            Assert.AreEqual(0, project.Backlog.Count);

            project.Backlog.Add(new UserStory());

            Assert.AreEqual(1, project.Backlog.Count);
        }
    }
}
