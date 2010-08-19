using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ploeh.AutoFixture;

using TecX.Agile.Data.File;

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

        [TestMethod]
        public void CanCreateProjectFolder()
        {
            string currentDirName = Directory.GetCurrentDirectory();

            DirectoryInfo current = new DirectoryInfo(currentDirName);

            Guid id = Guid.NewGuid();

            DirectoryInfo newDir = FileRepositoryHelper.CreateDirectoryIfNotExists(current, id.ToString());

            Assert.IsTrue(newDir.Exists);
        }

        [TestMethod]
        public void CanWriteAndReadProjectInfo()
        {
            Fixture fixture = new Fixture();

            ProjectInfo info = fixture.CreateAnonymous<ProjectInfo>();

            Assert.IsNotNull(info);
            Assert.AreNotEqual(Guid.Empty, info.Id);

            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());

            DirectoryInfo projectDir = FileRepositoryHelper.CreateDirectoryIfNotExists(current, info.Id.ToString());

            FileRepositoryHelper.WriteProjectInfo(projectDir, info);

            var infos = FileRepositoryHelper.GetProjectInfosFromXmlFiles(new[] { projectDir });

            Assert.AreEqual(1, infos.Count());

            ProjectInfo read = infos.Single();

            Assert.AreEqual(info.Created, read.Created);
            Assert.AreEqual(info.Id, read.Id);
            Assert.AreEqual(info.LastModified, read.LastModified);
            Assert.AreEqual(info.Name, read.Name);
        }
    }
}
