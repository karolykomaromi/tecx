using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common;

using TexC.Agile.Data;

namespace TecX.Agile.Data.Test
{
    [TestClass]
    public class FileRepositoryFixture
    {
        [TestMethod]
        public void CanXmlSerializeProjectInfo()
        {
            ProjectInfo pi = new ProjectInfo
                                 {
                                     Created = new DateTime(2010, 08, 13),
                                     Id = Guid.NewGuid(),
                                     LastModified = new DateTime(2010, 09, 7),
                                     Name = "To whom the bell tolls"
                                 };

            XmlSerializer serializer = new XmlSerializer(typeof(ProjectInfo));

            string xml = serializer.SerializePlain(pi);

            ProjectInfo deserialized = (ProjectInfo)serializer.Deserialize(new StringReader(xml));

            Assert.AreEqual(pi.Created, deserialized.Created);
            Assert.AreEqual(pi.Id, deserialized.Id);
            Assert.AreEqual(pi.LastModified, deserialized.LastModified);
            Assert.AreEqual(pi.Name, deserialized.Name);
        }
    }

    public static class XmlExtensions
    {
        public static string SerializePlain(this XmlSerializer serializer, object obj)
        {
            Guard.AssertNotNull(serializer, "serializer");
            Guard.AssertNotNull(obj, "obj");

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            XmlWriterSettings xws = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };

            StringBuilder sb = new StringBuilder(512);

            XmlWriter writer = XmlWriter.Create(sb, xws);

            serializer.Serialize(writer, obj, ns);

            string xml = sb.ToString();

            return xml;
        }
    }
}
