using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Agile;

namespace TexC.Agile.Data
{
    public interface IRepository
    {
        Project GetProject(Guid id);

        Project GetProject(string name);

        bool SaveProject(Project project);
    }
}
