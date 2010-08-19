using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using TecX.Agile;
using TecX.Agile.Data.Xml;

namespace TecX.Agile.Data.Test
{
    [TestClass]
    public class XmlConversionFixture
    {
        [TestMethod]
        public void CanConvertProject()
        {
            XmlProjectToStringConverter converter = new XmlProjectToStringConverter();

            Fixture fixture = new Fixture();

            //TODO weberse customize project creation to add storycards and iterations
            //and entries to legend
            Project project = fixture.CreateAnonymous<Project>();

            string xml = converter.ConvertToString(project);

            Project converted = converter.ConvertToProject(xml);

            Assert.IsTrue(project.Equals(converted));
        }
    }
}
