using System.Linq;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ploeh.AutoFixture;

using TecX.Agile.Data.Xml;
using TecX.Agile.Test;

namespace TecX.Agile.Data.Test
{
    [TestClass]
    public class XmlConversionFixture
    {
        private readonly Fixture _fixture;

        public XmlConversionFixture()
        {
            _fixture = TestHelper.GetCustomizedFixture();
        }

        [TestMethod]
        public void CanConvertVisualizableToXml()
        {
            Visualizable visualizable = _fixture.CreateAnonymous<Visualizable>();

            XElement xml = new XElement("Visualizable");

            visualizable.ToXml(xml);

            Visualizable converted = new Visualizable();

            converted.FromXml(xml);

            Assert.AreEqual(visualizable, converted);
        }

        [TestMethod]
        public void CanConvertTrackableToXml()
        {
            Trackable trackable = _fixture.CreateAnonymous<Trackable>();

            XElement xml = new XElement("Trackable");

            trackable.ToXml(xml);

            Trackable converted = new Trackable();

            converted.FromXml(xml);

            Assert.AreEqual(trackable, converted);
        }

        [TestMethod]
        public void CanConvertLegendToXml()
        {
            Legend legend = _fixture.CreateAnonymous<Legend>();

            XElement xml = legend.ToXml();

            Legend converted = new Legend();
            converted.FromXml(xml);

            Assert.IsTrue(legend.Count() > 0);
            Assert.AreEqual(legend, converted);
        }

        [TestMethod]
        public void CanConvertStoryCardToXml()
        {
            StoryCard storyCard = _fixture.CreateAnonymous<StoryCard>();

            XElement xml = storyCard.ToXml();

            StoryCard converted = new StoryCard();
            converted.FromXml(xml);

            Assert.AreEqual(storyCard, converted);
        }

        [TestMethod]
        public void CanConvertBacklogToXml()
        {
            Backlog backlog = _fixture.CreateAnonymous<Backlog>();

            XElement xml = backlog.ToXml();

            Backlog converted = new Backlog();
            converted.FromXml(xml);

            Assert.AreEqual(backlog,converted);
            Assert.IsTrue(backlog.OrderBy(sc => sc.Id).SequenceEqual(converted.OrderBy(sc => sc.Id)));
        }

        [TestMethod]
        public void CanConvertIterationToXml()
        {
            Iteration iteration = _fixture.CreateAnonymous<Iteration>();

            XElement xml = iteration.ToXml();
            Iteration converted = new Iteration();
            converted.FromXml(xml);

            Assert.AreEqual(iteration, converted);
            Assert.IsTrue(iteration.OrderBy(sc => sc.Id).SequenceEqual(converted.OrderBy(sc => sc.Id)));
        }

        [TestMethod]
        public void CanConvertProjectToXml()
        {
            XmlProjectToStringConverter converter = new XmlProjectToStringConverter();

            Project project = _fixture.CreateAnonymous<Project>();

            string xml = converter.ConvertToString(project);

            Project converted = converter.ConvertToProject(xml);

            Assert.AreEqual(project, converted);
            Assert.IsTrue(project.OrderBy(i => i.Id).SequenceEqual(converted.OrderBy(i => i.Id)));

        }
    }
}
