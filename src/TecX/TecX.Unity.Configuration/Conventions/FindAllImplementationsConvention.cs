namespace TecX.Unity.Configuration.Conventions
{
    using System;

    using TecX.Common;
    using TecX.Common.Reflection;

    public class FindAllImplementationsConvention : IRegistrationConvention
    {
        private readonly Type @from;
        private Func<Type, string> getName;

        public FindAllImplementationsConvention(Type from)
        {
            Guard.AssertNotNull(from, "from");

            this.@from = from;
            this.getName = type => type.Name;
        }

        public void Process(Type type, ConfigurationBuilder builder)
        {
            if (type.CanBeCastTo(this.@from) && 
                Constructor.HasConstructors(type))
            {
                string name = this.getName(type);

                builder.For(this.@from).Add(type).Named(name);
            }
        }

        public void Named(Func<Type, string> getName)
        {
            Guard.AssertNotNull(getName, "getName");

            this.getName = getName;
        }
    }
}
