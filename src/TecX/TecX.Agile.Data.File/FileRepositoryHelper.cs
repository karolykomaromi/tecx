using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using TecX.Common;

using TexC.Agile.Data;

namespace TecX.Agile.Data.File
{
    public static class FileRepositoryHelper
    {
        public static IEnumerable<ProjectInfo> GetProjectInfosFromXmlFiles(IEnumerable<DirectoryInfo> projectSubFolders)
        {
            List<ProjectInfo> projectInfos = new List<ProjectInfo>();

            foreach (DirectoryInfo psf in projectSubFolders)
            {
                FileInfo info = psf.GetFiles(Constants.ProjectInfoFileName).SingleOrDefault();

                if (info != null)
                {
                    Stream stream = info.OpenRead();

                    XmlSerializer serializer = new XmlSerializer(typeof (ProjectInfo));

                    ProjectInfo pi = (ProjectInfo) serializer.Deserialize(stream);

                    projectInfos.Add(pi);
                }
            }

            return projectInfos;
        }

        public static IEnumerable<DirectoryInfo> GetProjectSubDirectories(DirectoryInfo baseFolder)
        {
            Guard.AssertNotNull(baseFolder, "baseFolder");

            IEnumerable<DirectoryInfo> projectSubDirectories =
                baseFolder.GetDirectories().Where(di => TypeHelper.IsGuid(di.Name));

            return projectSubDirectories;
        }

        public static DirectoryInfo CreateDirectoryIfNotExists(DirectoryInfo baseFolder, string subFolderName)
        {
            Guard.AssertNotNull(baseFolder, "baseFolder");
            Guard.AssertNotEmpty(subFolderName, "subFolderName");

            IEnumerable<DirectoryInfo> subFolders = baseFolder.GetDirectories(subFolderName, 
                SearchOption.TopDirectoryOnly);

            if (subFolders.Count() == 0)
            {
                DirectoryInfo subFolder = baseFolder.CreateSubdirectory(subFolderName);
                return subFolder;
            }

            if(subFolders.Count() == 1)
                return subFolders.First();

            throw new InvalidOperationException("");
        }
    }
}