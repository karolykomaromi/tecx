using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TecX.Agile.Data
{
    public class InMemoryRepository<TArtefact> : IRepository<TArtefact>
        where TArtefact : PlanningArtefact
    {
        //#region Fields

        //private Project _currentProject;

        //#endregion Fields

        //public Project GetProjectBy(Guid id)
        //{
        //    if (id == _currentProject.Id)
        //        return _currentProject;

        //    _currentProject = LoadProject(id);

        //    return _currentProject;
        //}

        //public Project GetProjectBy(string name)
        //{
        //    Guard.AssertNotEmpty(name, "name");

        //    Guid id = GetProjectId(name);

        //    return GetProjectBy(id);
        //}

        //public Project CreateProject()
        //{
        //    Project project = New.Project();

        //    project = DoCreateProject(project);

        //    return project;
        //}

        //protected abstract Project DoCreateProject(Project project);

        //public bool SaveProject(Project project)
        //{
        //    Guard.AssertNotNull(project, "project");

        //    bool saved = DoSaveProject(project);

        //    return saved;
        //}

        //private Guid GetProjectId(string name)
        //{
        //    IEnumerable<ProjectInfo> projects = GetExistingProjects();

        //    ProjectInfo info = projects.SingleOrDefault(p => p.Name == name);

        //    if (info != null)
        //    {
        //        return info.Id;
        //    }

        //    throw new InvalidOperationException("No project with name '" + name + "' found.");
        //}

        //protected abstract Project LoadProject(Guid id);

        //protected abstract bool DoSaveProject(Project project);

        //public abstract IEnumerable<ProjectInfo> GetExistingProjects();

        private readonly HashSet<TArtefact> _set;
        private readonly IQueryable<TArtefact> _queryableSet;

        public InMemoryRepository()
            : this(Enumerable.Empty<TArtefact>())
        {
        }

        public InMemoryRepository(IEnumerable<TArtefact> artefacts)
        {
            _set = new HashSet<TArtefact>();

            foreach (TArtefact artefact in artefacts)
            {
                _set.Add(artefact);
            }

            _queryableSet = _set.AsQueryable();
        }

        public IQueryable<TArtefact> FindAll()
        {
            return _queryableSet;
        }

        public IQueryable<TArtefact> FindWhere(Expression<Func<TArtefact, bool>> predicate)
        {
            return _queryableSet.Where(predicate);
        }

        public TArtefact FindById(Guid id)
        {
            return _queryableSet.SingleOrDefault(a => a.Id == id);
        }

        public void Add(TArtefact newArtefact)
        {
            _set.Add(newArtefact);
        }

        public void Remove(TArtefact artefact)
        {
            _set.Remove(artefact);
        }
    }
}