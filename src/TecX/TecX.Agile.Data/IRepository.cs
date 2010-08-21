using System;
using System.Collections.Generic;

namespace TecX.Agile.Data
{
    public interface IRepository
    {
        Project GetProjectBy(Guid id);

        Project GetProjectBy(string name);

        IEnumerable<ProjectInfo> GetExistingProjects();

        Project CreateProject();

        bool SaveProject(Project project);
    }
}
