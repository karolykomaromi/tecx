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

        public FindAllTypesConvention()
        {
            _getName = type => { return type.FullName; };
        }

        public FindAllTypesConvention(Type from)
        {
            Guard.AssertNotNull(from, "from");

            _from = from;
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
            _getName = getName;
        }
    }
}
