using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

using Ploeh.AutoFixture;

using TecX.Agile.Data.Json;
using TecX.Agile.Test;

namespace TecX.Agile.Data.Test
{
    [TestClass]
    public class JsonConversionFixture
    {
        private readonly Fixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConversionFixture"/> class
        /// </summary>
        public JsonConversionFixture()
        {
            _fixture = TestHelper.GetCustomizedFixture();
        }

        [TestMethod]
        public void CanConvertTrackableToJson()
        {
            Trackable trackable = _fixture.CreateAnonymous<Trackable>();

            JObject json = new JObject();

            trackable.ToJson(json);

            Trackable converted = new Trackable();

            converted.FromJson(json);

            Assert.AreEqual(trackable, converted);
        }

        [TestMethod]
        public void CanConvertVisualizableToJson()
        {
            Visualizable visualizable = _fixture.CreateAnonymous<Visualizable>();

            JObject json = new JObject();

            visualizable.ToJson(json);

            Visualizable converted = new Visualizable();

            converted.FromJson(json);

            Assert.AreEqual(visualizable, converted);
        }

        [TestMethod]
        public void CanConvertLegendToJson()
        {
            Legend legend = _fixture.CreateAnonymous<Legend>();

            JObject json = new JObject();

            legend.ToJson(json);

            Legend converted = new Legend();

            converted.FromJson(json);

            Assert.AreEqual(legend, converted);
        }

        [TestMethod]
        public void CanConvertStoryCardToJson()
        {
            StoryCard storyCard = _fixture.CreateAnonymous<StoryCard>();

            JObject json = storyCard.ToJson();

            StoryCard converted = new StoryCard();

            converted.FromJson(json);

            Assert.AreEqual(storyCard, converted);
        }

        [TestMethod]
        public void CanConvertBacklogToJson()
        {
            Backlog backlog = _fixture.CreateAnonymous<Backlog>();

            JObject json = backlog.ToJson();

            Backlog converted = new Backlog();

            converted.FromJson(json);

            Assert.AreEqual(backlog, converted);
            Assert.IsTrue(backlog.OrderBy(sc => sc.Id).SequenceEqual(converted.OrderBy(sc => sc.Id)));
        }

        [TestMethod]
        public void CanConvertIterationToJson()
        {
            Iteration iteration = _fixture.CreateAnonymous<Iteration>();

            JObject json = iteration.ToJson();

            Iteration converted = new Iteration();

            converted.FromJson(json);

            Assert.AreEqual(iteration, converted);
            Assert.IsTrue(iteration.OrderBy(sc => sc.Id).SequenceEqual(converted.OrderBy(sc => sc.Id)));
        }

        [TestMethod]
        public void CanConvertProjectToJson()
        {
            Project project = _fixture.CreateAnonymous<Project>();

            JObject json = project.ToJson();

            Project converted = new Project();

            converted.FromJson(json);

            Assert.AreEqual(project, converted);
            Assert.IsTrue(project.OrderBy(i => i.Id).SequenceEqual(converted.OrderBy(i => i.Id)));
        }
    }
}