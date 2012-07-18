namespace TecX.Unity.Configuration.Conventions
{
    using System;
    using System.Linq;

    using TecX.Common;

    public class FindAllImplementationsOfOpenGenericInterfaceConvention : IRegistrationConvention
    {
        private readonly Type openGeneric;

        private Func<Type, string> getName;

        public FindAllImplementationsOfOpenGenericInterfaceConvention(Type type)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertCondition(type.IsInterface, type, "type");
            Guard.AssertCondition(type.IsGenericType, type, "type");

            this.openGeneric = type.GetGenericTypeDefinition();
            this.getName = t => t.Name;
        }

        public void Process(Type type, ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotNull(builder, "builder");

            var matches = type.AllInterfaces()
                .Where(i => i.IsGenericType &&
                       i.GetGenericTypeDefinition() == this.openGeneric);

            foreach (var match in matches)
            {
                builder.For(match).Add(type).Named(this.getName(type));
            }
        }

        public FindAllImplementationsOfOpenGenericInterfaceConvention Named(Func<Type, string> getName)
        {
            Guard.AssertNotNull(getName, "getName");

            this.getName = getName;

            return this;
        }
    }
}
