using System;
using System.Reflection;
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
        private Fixture AutoFixture { get; set; }

        public AgileBuilderFixture()
        {
            var fixture = new Fixture
            {
                Resolver = AutoFixtureExtensions.ResolveEnum
            };

            fixture.Customize<Visualizable>(ob => ob.With(vis => vis.Color,
                                                          fixture.CreateMany<byte>(4).ToArray()));

            fixture.Freeze<StoryCard>();
            
            AutoFixture = fixture;
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
                .WithCurrentSideUp(StoryCardSides.RichTestSide)
                .WithDescription("desc")
                .WithDescriptionHandwritingImage(new byte[] { 2, 3, 4, 5, 6, 7, 8, 9 })
                .WithId(id)
                .WithName("name")
                .WithTaskOwner("John Wayne")
                .WithTracking(
                    New.Tracking()
                        .WithMostLikelyEstimate(4.5m)
                        .WithActualEffort(1.2m)
                        .WithBestCaseEstimate(2.3m)
                        .WithPriority(Priority.Immediate)
                        .WithStatus(Status.Completed)
                        .WithWorstCaseEstimate(7.8m))
                .WithView(
                    New.View()
                        .WithHeight(3.4)
                        .WithColor(new byte[] { 1, 2, 3, 4 })
                        .WithRotationAngle(5.6)
                        .WithWidth(6.7)
                        .WithX(8.9)
                        .WithY(9.1));

            StoryCard storyCard = builder.Build();

            Assert.AreEqual(1.2m, storyCard.Tracking.ActualEffort);
            Assert.AreEqual(2.3m, storyCard.Tracking.BestCaseEstimate);
            Assert.AreEqual(4.5m, storyCard.Tracking.MostLikelyEstimate);
            Assert.AreEqual(Priority.Immediate, storyCard.Tracking.Priority);
            Assert.AreEqual(Status.Completed, storyCard.Tracking.Status);
            Assert.AreEqual(7.8m, storyCard.Tracking.WorstCaseEstimate);

            Assert.IsTrue(new byte[] { 1, 2, 3, 4 }.SequenceEqual(storyCard.View.Color));
            Assert.AreEqual(3.4, storyCard.View.Height);
            Assert.AreEqual(5.6, storyCard.View.RotationAngle);
            Assert.AreEqual(6.7, storyCard.View.Width);
            Assert.AreEqual(8.9, storyCard.View.X);
            Assert.AreEqual(9.1, storyCard.View.Y);

            Assert.AreEqual(StoryCardSides.RichTestSide, storyCard.CurrentSideUp);
            Assert.AreEqual("desc", storyCard.Description);
            Assert.IsTrue(new byte[] { 2, 3, 4, 5, 6, 7, 8, 9 }.SequenceEqual(storyCard.DescriptionHandwritingImage));

            Assert.AreEqual(id, storyCard.Id);

            Assert.AreEqual("name", storyCard.Name);



            Assert.AreEqual("John Wayne", storyCard.TaskOwner);


            //test implicit conversion
            StoryCard storyCard2 = builder;

            Assert.AreEqual(storyCard, storyCard2);

            Assert.AreNotSame(storyCard, storyCard2);

            storyCard2 = storyCard2.BuildUp().WithTaskOwner("Prinz Eisenherz");

            Assert.AreNotEqual(storyCard, storyCard2);
        }

        [TestMethod]
        public void CanCreateStoryCardUsingAutoFixture()
        {
            var fixture = new Fixture
                              {
                                  Resolver = AutoFixtureExtensions.ResolveEnum
                              };

            fixture.Customize<Visualizable>(ob => ob.With(vis => vis.Color,
                fixture.CreateMany<byte>(4).ToArray()));

            StoryCard storyCard = fixture.CreateAnonymous<StoryCard>();

            Assert.AreNotEqual(0, storyCard.View.Height);

            Assert.IsNotNull(storyCard.View.Color);
            Assert.IsTrue(storyCard.View.Color.Length == 4);
            Assert.IsFalse(storyCard.View.Color.SequenceEqual(new byte[] { 255, 255, 255, 255 }));
        }

        [TestMethod]
        public void CanBuildIteration()
        {
            Guid id = Guid.NewGuid();

            IterationBuilder builder = New.Iteration()
                .WithAvailableEffort(10.2m)
                .WithDescription("CanBuildIteration")
                .WithEndDate(new DateTime(2010, 8, 6))
                .WithId(id)
                .WithName("Clint Eastwood")
                .WithStartDate(new DateTime(2009, 1, 1))
                .WithTracking(
                    New.Tracking()
                        .WithActualEffort(2.3m)
                        .WithBestCaseEstimate(3.4m)
                        .WithMostLikelyEstimate(4.5m)
                        .WithPriority(Priority.Low)
                        .WithStatus(Status.Accepted)
                        .WithWorstCaseEstimate(7.8m))
                .WithView(
                    New.View()
                        .WithColor(new byte[] { 1, 2, 3, 4 })
                        .WithHeight(5.6)
                        .WithRotationAngle(6.7)
                        .WithWidth(7.8)
                        .WithX(8.9)
                        .WithY(9.1));

            Iteration iteration = builder.Build();

            Assert.AreEqual(10.2m, iteration.AvailableEffort);
            Assert.AreEqual("CanBuildIteration", iteration.Description);
            Assert.AreEqual(new DateTime(2010, 8, 6), iteration.EndDate);
            Assert.AreEqual(id, iteration.Id);
            Assert.AreEqual("Clint Eastwood", iteration.Name);
            Assert.AreEqual(new DateTime(2009, 1, 1), iteration.StartDate);

            Assert.AreEqual(2.3m, iteration.Tracking.ActualEffort);
            Assert.AreEqual(3.4m, iteration.Tracking.BestCaseEstimate);
            Assert.AreEqual(4.5m, iteration.Tracking.MostLikelyEstimate);
            Assert.AreEqual(Priority.Low, iteration.Tracking.Priority);
            Assert.AreEqual(Status.Accepted, iteration.Tracking.Status);
            Assert.AreEqual(7.8m, iteration.Tracking.WorstCaseEstimate);

            Assert.IsTrue(new byte[] { 1, 2, 3, 4 }.SequenceEqual(iteration.View.Color));
            Assert.AreEqual(5.6, iteration.View.Height);
            Assert.AreEqual(6.7, iteration.View.RotationAngle);
            Assert.AreEqual(7.8, iteration.View.Width);
            Assert.AreEqual(8.9, iteration.View.X);
            Assert.AreEqual(9.1, iteration.View.Y);

            Iteration iteration2 = builder;

            Assert.AreEqual(iteration, iteration2);

            builder.WithName("new name");

            iteration2 = builder;

            Assert.AreNotEqual(iteration, iteration2);
        }

        [TestMethod]
        public void CanAddAndRemoveStoryFromBacklog()
        {
            BacklogBuilder bb = New.Backlog();

            StoryCard storyCard = AutoFixture.CreateAnonymous<StoryCard>();

            bb.With(storyCard);

            Backlog backlog = bb;

            Assert.AreEqual(1, backlog.StoryCards.Count());

            Assert.AreEqual(storyCard, backlog.StoryCards.First());
            Assert.AreNotSame(storyCard, backlog.StoryCards.First());

            Assert.IsTrue(backlog.Remove(storyCard.Id));

            Assert.AreEqual(0, backlog.StoryCards.Count());
        }

        [TestMethod]
        public void CanAddAndRemoveStoryFromIteration()
        {
            StoryCard storyCard = AutoFixture.CreateAnonymous<StoryCard>();

            IterationBuilder ib = New.Iteration();

            ib.With(storyCard);

            Iteration iteration = ib;

            Assert.AreEqual(1, iteration.StoryCards.Count());

            Assert.AreEqual(storyCard, iteration.StoryCards.First());
            Assert.AreNotSame(storyCard, iteration.StoryCards.First());

            Assert.IsTrue(iteration.Remove(storyCard.Id));

            Assert.AreEqual(0, iteration.StoryCards.Count());
        }

        [TestMethod]
        public void CanBuildBacklog()
        {
            Guid id = Guid.NewGuid();

            BacklogBuilder builder = New.Backlog()
                .WithDescription("CanBuildBacklog")
                .WithId(id)
                .WithName("Butch Cassidy");

            Backlog backlog = builder.Build();

            Assert.AreEqual("CanBuildBacklog", backlog.Description);
            Assert.AreEqual(id, backlog.Id);
            Assert.AreEqual("Butch Cassidy", backlog.Name);

            Backlog backlog2 = builder;

            Assert.AreEqual(backlog, backlog2);
            Assert.AreNotSame(backlog, backlog2);

            builder.WithName("Sundance Kid");

            backlog2 = builder;

            Assert.AreNotEqual(backlog, backlog2);
        }

        [TestMethod]
        public void CanBuildProject()
        {
            Guid id = Guid.NewGuid();

            ProjectBuilder builder = New.Project()
                .WithDescription("CanBuildProject")
                .WithId(id)
                .WithName("Buffalo Bill")
                .WithLegend(
                    New.Legend()
                        .With("a", new byte[] {1, 2, 3, 4})
                        .With("b", new byte[] {2, 3, 4, 5}));

            Project project = builder.Build();

            Assert.AreEqual("CanBuildProject", project.Description);
            Assert.AreEqual(id, project.Id);
            Assert.AreEqual("Buffalo Bill", project.Name);
            Assert.IsTrue(new byte[]{1,2,3,4}.SequenceEqual(project.Legend["a"].Color));
            Assert.IsTrue(new byte[] { 2, 3, 4, 5 }.SequenceEqual(project.Legend["b"].Color));

            Project project2 = builder;

            Assert.AreEqual(project, project2);
            Assert.AreNotSame(project, project2);

            project2 = builder.WithLegend(project2.Legend.BuildUp().Replace("b", new byte[] {5, 6, 7, 8}));

            Assert.IsTrue(new byte[]{5,6,7,8}.SequenceEqual(project2.Legend["b"].Color));

            Assert.AreNotEqual(project, project2);
        }
    }
}
