using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Unity.Configuration.Expressions;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{
    public class GenericConnectionConvention : IRegistrationConvention
    {
        private readonly Type _openType;

        public GenericConnectionConvention(Type openType)
        {
            _openType = openType;

            if (!_openType.IsOpenGeneric())
            {
                throw new ApplicationException("This scanning convention can only be used with open generic types");
            }
        }

        public void Process(Type type, Registry registry)
        {
            var interfaceTypes = type.FindInterfacesThatClose(_openType);

            foreach (var interfaceType in interfaceTypes)
            {
                //TODO weberse 2011-03-09 how can I register open generic types?

                //registry.AddType(type, interfaceType, Guid.NewGuid().ToString());
            }
        }
    }
}
