using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Agile;

namespace TexC.Agile.Data
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
