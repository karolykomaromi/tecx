using System;
using System.Collections.Generic;
using System.Linq;

using TecX.Agile.Builder;
using TecX.Common;

namespace TecX.Agile.Data
{
    public abstract class Repository : IRepository
    {
        #region Fields

        private Project _currentProject;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        public Project GetProjectBy(Guid id)
        {
            if (id == _currentProject.Id)
                return _currentProject;

            _currentProject = LoadProject(id);

            return _currentProject;
        }

        public Project GetProjectBy(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            Guid id = GetProjectId(name);

            return GetProjectBy(id);
        }

        public Project CreateProject()
        {
            Project project = New.Project();

            project = DoCreateProject(project);

            return project;
        }

        protected abstract Project DoCreateProject(Project project);

        public bool SaveProject(Project project)
        {
            Guard.AssertNotNull(project, "project");

            bool saved = DoSaveProject(project);

            return saved;
        }

        private Guid GetProjectId(string name)
        {
            IEnumerable<ProjectInfo> projects = GetExistingProjects();

            ProjectInfo info = projects.SingleOrDefault(p => p.Name == name);

            if(info != null)
            {
                return info.Id;
            }

            throw new InvalidOperationException("No project with name '" + name + "' found.");
        }

        protected abstract Project LoadProject(Guid id);

        protected abstract bool DoSaveProject(Project project);

        public abstract IEnumerable<ProjectInfo> GetExistingProjects();

    }
}