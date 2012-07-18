using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Unity.ContextualBinding.Test.TestObjects
{
    public class ParentWithDependency
    {
        public IMyInterface Dependency { get; set; }

        public ParentWithDependency(IMyInterface dependency)
        {
            Dependency = dependency;
        }
    }
}
