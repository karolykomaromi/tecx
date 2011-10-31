using System;

using TecX.Common;
using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{
    using TecX.Common.Reflection;

    public class FindAllTypesConvention : IRegistrationConvention
    {
        private readonly Type _from;
        private Func<Type, string> _getName;

        public FindAllTypesConvention(Type from)
        {
            Guard.AssertNotNull(from, "from");

            _from = from;
            _getName = type => type.FullName;
        }

        public void Process(Type type, ConfigurationBuilder builder)
        {
            if (type.CanBeCastTo(_from) && 
                Constructor.HasConstructors(type))
            {
                string name = _getName(type);

                builder.For(_from).Add(type).Named(name);
            }
        }

        public void NameBy(Func<Type, string> getName)
        {
            Guard.AssertNotNull(getName, "getName");

            _getName = getName;
        }
    }
}
