namespace TecX.Unity.Configuration.Test.TestObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common;
    using TecX.Unity.Configuration.Conventions;
    using TecX.Unity.Utility;

    public class CommandHandlerConvention : IRegistrationConvention
    {
        private Func<Type, string> getName;

        private readonly List<Type> wrapperTypes;

        public CommandHandlerConvention()
        {
            this.getName = t => t.Name;
            this.wrapperTypes = new List<Type>();
        }

        public void Process(Type type, ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotNull(builder, "builder");

            if (type.IsInterface)
            {
                return;
            }

            if (type.IsGenericType)
            {
                return;
            }

            var interfaces = type.AllInterfaces();

            var handlerInterface = interfaces.FirstOrDefault(i => i.IsGenericType &&
                                                                  i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

            if (handlerInterface != null)
            {
                var name = this.getName(type);

                if (this.wrapperTypes.Count > 0)
                {
                    var wrappers = this.wrapperTypes.ToArray().Reverse();

                    var closingType = handlerInterface.GetGenericArguments()[0];

                    foreach (Type wrapper in wrappers)
                    {
                        Type closedGeneric = wrapper.MakeGenericType(closingType);

                        builder.For(handlerInterface).Add(closedGeneric).Named(name);
                    }
                }

                builder.For(handlerInterface).Add(type).Named(name);
            }
        }

        public CommandHandlerConvention Named(Func<Type, string> getName)
        {
            Guard.AssertNotNull(getName, "getName");

            this.getName = getName;

            return this;
        }

        public CommandHandlerConvention WithTransaction()
        {
            this.wrapperTypes.Add(typeof(TransactionHandler<>));
            return this;
        }

        public CommandHandlerConvention WithDeadlockRetry()
        {
            this.wrapperTypes.Add(typeof(DeadlockRetryHandler<>));
            return this;
        }
    }
}