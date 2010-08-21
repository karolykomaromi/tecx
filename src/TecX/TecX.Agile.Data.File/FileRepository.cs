using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile.Data.File
{
    public abstract class FileRepository : Repository
    {
        #region Fields

        private readonly DirectoryInfo _baseFolder;
        private readonly ProjectToStringConverter _converter;

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
        protected FileRepository(DirectoryInfo baseFolder, ProjectToStringConverter converter)
        {
            Guard.AssertNotNull(baseFolder, "baseFolder");
            Guard.AssertNotNull(converter, "converter");

            _baseFolder = baseFolder;

            if (!_baseFolder.Exists)
                _baseFolder.Create();

            _converter = converter;
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        public override IEnumerable<ProjectInfo> GetExistingProjects()
        {
            IEnumerable<ProjectInfo> infos = FileRepositoryHelper.GetProjectInfos(_baseFolder);

            return infos;
        }

        protected override Project DoCreateProject(Project project)
        {
            string projectFolderName = project.Id.ToString();

            CurrentFolder = FileRepositoryHelper.CreateDirectoryIfNotExists(_baseFolder, projectFolderName);

            string projectFileName = CurrentFolder.FullName + Path.PathSeparator + "project" + FileExtension;

            CurrentFile = new FileInfo(projectFileName);

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

            if (dirInfo != null)
            {
                FileInfo fileInfo = dirInfo.GetFiles("project" + FileExtension).SingleOrDefault();

                if (fileInfo != null)
                {
                    string projectString = fileInfo.OpenText().ReadToEnd();

                    Guard.AssertNotNull(projectString, "projectString");

                    Project project = _converter.ConvertToProject(projectString);

                    return project;
                }
            }

            throw new RepositoryException(TypeHelper.SafeFormat("Could not load project with id {0}", id))
                .WithAdditionalInfo("id", id);
        }

        protected override bool DoSaveProject(Project project)
        {
            string projectString = _converter.ConvertToString(project);

            using (TextWriter writer = new StreamWriter(CurrentFile.OpenWrite(), Encoding.UTF8))
            {
                writer.Write(projectString);
                return true;
            }
        }

    }
}
