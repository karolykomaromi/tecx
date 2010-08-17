using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Xml.Linq;

using TecX.Common;
using TecX.Common.Extensions.Error;

using TexC.Agile;
using TexC.Agile.Data;
using System.Xml.Serialization;

namespace TecX.Agile.Data.File
{
    public abstract class FileRepository : Repository
    {
        #region Constants

        #endregion Constants

        ////////////////////////////////////////////////////////////

        #region Fields

        private readonly DirectoryInfo _baseFolder;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region Properties

        public abstract string FileExtension { get; }

        public FileInfo CurrentFile { get; protected set; }

        public DirectoryInfo CurrentFolder { get; protected set; }

        #endregion Properties

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileRepository"/> class
        /// </summary>
        protected FileRepository(DirectoryInfo baseFolder)
        {
            Guard.AssertNotNull(baseFolder, "baseFolder");

            _baseFolder = baseFolder;
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        protected override Project DoCreateProject(Project project)
        {
            if (!_baseFolder.Exists)
                _baseFolder.Create();

            string projectFolderName = project.Id.ToString();
            
            CurrentFolder = _baseFolder.CreateSubdirectory(projectFolderName);

            CurrentFile = new FileInfo(CurrentFolder.FullName + Path.PathSeparator + "project" + FileExtension);

            if (!CurrentFile.Exists)
                CurrentFile.Create();

            bool saved = SaveProject(project);

            if (saved)
                return project;

            throw new RepositoryException("Could not save project").WithAdditionalInfo("project", project);
        }

        protected override Project LoadProject(Guid id)
        {
            string projectSubFolderName = id.ToString();

            DirectoryInfo dirInfo = _baseFolder.GetDirectories(projectSubFolderName).SingleOrDefault();

            if(dirInfo != null)
            {
                FileInfo fileInfo = dirInfo.GetFiles("project" + FileExtension).SingleOrDefault();

                if (fileInfo != null)
                {
                    Project project = DoLoadProject(fileInfo);

                    return project;
                }
            }

            throw new RepositoryException(TypeHelper.SafeFormat("Could not load project with id {0}", id))
                .WithAdditionalInfo("id", id);
        }

        protected abstract Project DoLoadProject(FileInfo fileInfo);

        protected override bool DoSaveProject(Project project)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ProjectInfo> GetExistingProjects()
        {
            var subFolders = _baseFolder.GetDirectories();

            var projectSubFolders = subFolders.Where(sf => TypeHelper.IsGuid(sf.Name));

            IEnumerable<ProjectInfo> infos = FileRepositoryHelper.GetProjectInfosFromXmlFiles(projectSubFolders);

            return infos;
        }
    }
}
