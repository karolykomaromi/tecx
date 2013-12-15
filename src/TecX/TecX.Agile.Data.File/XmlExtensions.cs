using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using TecX.Common;

namespace TecX.Agile.Data.File
{
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
                                            //OmitXmlDeclaration = true,
                                            Indent = true
                                        };

            StringBuilder sb = new StringBuilder(512);

            XmlWriter writer = XmlWriter.Create(sb, xws);

            serializer.Serialize(writer, obj, ns);

            string xml = sb.ToString();

            return xml;
        }

        public static void SerializePlain(this XmlSerializer serializer, object  obj, Stream stream)
        {
            Guard.AssertNotNull(serializer, "serializer");
            Guard.AssertNotNull(obj, "obj");
            Guard.AssertNotNull(stream, "stream");

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            XmlWriterSettings xws = new XmlWriterSettings
            {
                //OmitXmlDeclaration = true,
                Indent = true
            };

            XmlWriter writer = XmlWriter.Create(stream, xws);

            serializer.Serialize(writer, obj, ns);
            stream.Flush();
            stream.Close();
        }

        public static XElement ToXml(this ProjectInfo projectInfo)
        {
            XElement xml = new XElement("ProjectInfo",
                new XAttribute("Id", projectInfo.Id),
                new XAttribute("Name", projectInfo.Name),
                new XAttribute("Created", projectInfo.Created),
                new XAttribute("LastModified", projectInfo.LastModified));

            return xml;
        }

        public static ProjectInfo FromXml(this string xml)
        {
            XElement element = XElement.Parse(xml);

            ProjectInfo projectInfo = new ProjectInfo
                                          {
                                              Id = Guid.Parse(element.Attribute("Id").Value),
                                              Name = element.Attribute("Name").Value,
                                              Created = DateTime.Parse(element.Attribute("Created").Value),
                                              LastModified = DateTime.Parse(element.Attribute("LastModified").Value)
                                          };

            return projectInfo;
        }
    }
}