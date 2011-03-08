using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Unity.Configuration.Conventions
{
    public class FindRegistriesConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry configuration)
        {
            if (Registry.IsPublicRegistry(type))
            {
                //configuration.Configure(x => x.ImportRegistry(type));
            }
        }
    }
}
