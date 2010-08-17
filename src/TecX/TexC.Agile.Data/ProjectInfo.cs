using System;
using System.Xml.Serialization;

namespace TexC.Agile.Data
{
    public class ProjectInfo
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public Guid Id { get; set; }

        [XmlAttribute]
        public DateTime Created { get; set; }

        [XmlAttribute]
        public DateTime LastModified { get; set; }
    }
}