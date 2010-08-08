using System;

using TecX.Agile;
using TecX.Common;

namespace TexC.Agile.Data
{
    public abstract class Repository : IRepository
    {
        #region Fields

        private Project _currentProject;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        public Project GetProject(Guid id)
        {
            if (id == _currentProject.Id)
                return _currentProject;

            _currentProject = LoadProject(id);

            return _currentProject;
        }

        public Project GetProject(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            Guid id = GetProjectId(name);

            return GetProject(id);
        }

        public bool SaveProject(Project project)
        {
            Guard.AssertNotNull(project, "project");

            bool saved = SaveProjectInternal(project);

            return saved;
        }

        protected abstract Guid GetProjectId(string name);

        protected abstract Project LoadProject(Guid id);

        protected abstract bool SaveProjectInternal(Project project);
    }
}