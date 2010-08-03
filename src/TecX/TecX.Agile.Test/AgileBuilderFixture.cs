using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ploeh.AutoFixture;

using TecX.Agile.Builder;
using TecX.TestTools;

namespace TecX.Agile.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class AgileBuilderFixture
    {
        public AgileBuilderFixture()
        {
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
        public void CanBuildStoryCard()
        {
            Guid id = Guid.NewGuid();

            StoryCardBuilder builder = New.StoryCard()
                .WithActualEffort(1.2m)
                .WithBestCaseEstimate(2.3m)
                .WithColor(new byte[] { 1, 2, 3, 4 })
                .WithCurrentSideUp(StoryCardSides.RichTestSide)
                .WithDescription("desc")
                .WithDescriptionHandwritingImage(new byte[] { 2, 3, 4, 5, 6, 7, 8, 9 })
                .WithHeight(3.4)
                .WithId(id)
                .WithMostLikelyEstimate(4.5m)
                .WithName("name")
                .WithPriority(Priority.Immediate)
                .WithRotationAngle(5.6)
                .WithStatus(Status.Completed)
                .WithTaskOwner("John Wayne")
                .WithWidth(6.7)
                .WithWorstCaseEstimate(7.8m)
                .WithX(8.9)
                .WithY(9.1);

            StoryCard storyCard = builder.Build();

            Assert.AreEqual(1.2m, storyCard.Tracking.ActualEffort);
            Assert.AreEqual(2.3m, storyCard.Tracking.BestCaseEstimate);
            Assert.IsTrue(new byte[] { 1, 2, 3, 4 }.SequenceEqual(storyCard.View.Color));
            Assert.AreEqual(StoryCardSides.RichTestSide, storyCard.CurrentSideUp);
            Assert.AreEqual("desc", storyCard.Description);
            Assert.IsTrue(new byte[] { 2, 3, 4, 5, 6, 7, 8, 9 }.SequenceEqual(storyCard.DescriptionHandwritingImage));
            Assert.AreEqual(3.4, storyCard.View.Height);
            Assert.AreEqual(id, storyCard.Id);
            Assert.AreEqual(4.5m, storyCard.Tracking.MostLikelyEstimate);
            Assert.AreEqual("name", storyCard.Name);
            Assert.AreEqual(Priority.Immediate, storyCard.Tracking.Priority);
            Assert.AreEqual(5.6, storyCard.View.RotationAngle);
            Assert.AreEqual(Status.Completed, storyCard.Tracking.Status);
            Assert.AreEqual("John Wayne", storyCard.TaskOwner);
            Assert.AreEqual(6.7, storyCard.View.Width);
            Assert.AreEqual(7.8m, storyCard.Tracking.WorstCaseEstimate);
            Assert.AreEqual(8.9, storyCard.View.X);
            Assert.AreEqual(9.1, storyCard.View.Y);

            //test implicit conversion
            StoryCard storyCard2 = builder;

            Assert.AreEqual(storyCard, storyCard2);

            Assert.AreNotSame(storyCard, storyCard2);

            storyCard2 = storyCard2.BuildUp().WithTaskOwner("Prinz Eisenherz");

            Assert.AreNotEqual(storyCard, storyCard2);
        }

        [TestMethod]
        public void CanUseAutoFixture()
        {
            var fixture = new Fixture
                              {
                                  Resolver = FixtureResolver.ResolveEnum
                              };

            StoryCard storyCard = fixture.CreateAnonymous<StoryCard>();
            storyCard.View.Color = fixture.CreateAnonymous<byte[]>();

            //fixture.Build<StoryCard>().With()

            Assert.AreNotEqual(0, storyCard.View.Height);
        }
    }
}
