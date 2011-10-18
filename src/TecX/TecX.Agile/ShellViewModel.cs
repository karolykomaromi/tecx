using System.Collections.Generic;

using Caliburn.Micro;

using TecX.Agile.Infrastructure;
using TecX.Common;

namespace TecX.Agile
{
    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private readonly IEnumerable<IModule> _modules;

        public ShellViewModel(IEnumerable<IModule> modules)
        {
            Guard.AssertNotNull(modules, "modules");

            _modules = modules;
        }
    }
}
