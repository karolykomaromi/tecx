using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Common;
using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{

    public class FindAllTypesConvention : IRegistrationConvention
    {
        private readonly Type _pluginType;
        //private Func<Type, string> _getName = type => PluginCache.GetPlugin(type).ConcreteKey;
        private Func<Type, string> _getName = type => string.Empty;

        public FindAllTypesConvention(Type pluginType)
        {
            Guard.AssertNotNull(pluginType, "pluginType");

            _pluginType = pluginType;
        }

        public void Process(Type type, Registry configuration)
        {
            if (type.CanBeCastTo(_pluginType) && Constructor.HasConstructors(type))
            {
                string name = _getName(type);

                //configuration.AddType(_pluginType, type, name);
            }
        }

        public void NameBy(Func<Type, string> getName)
        {
            _getName = getName;
        }
    }
}
