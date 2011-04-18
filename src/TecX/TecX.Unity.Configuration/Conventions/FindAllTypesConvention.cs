using System;

using TecX.Common;
using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{

    public class FindAllTypesConvention : IRegistrationConvention
    {
        private readonly Type _from;
        private Func<Type, string> _getName;

        public FindAllTypesConvention(Type from)
        {
            Guard.AssertNotNull(from, "from");

            _from = from;
            _getName = type => { return type.FullName; };
        }

        public void Process(Type type, Registry registry)
        {
            if (type.CanBeCastTo(_from) && 
                Constructor.HasConstructors(type))
            {
                string name = _getName(type);

                registry.AddType(_from, type, name);
            }
        }

        public void NameBy(Func<Type, string> getName)
        {
            Guard.AssertNotNull(getName, "getName");

            _getName = getName;
        }
    }
}
