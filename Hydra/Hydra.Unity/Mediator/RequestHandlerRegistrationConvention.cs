namespace Hydra.Unity.Mediator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Hydra.Infrastructure.Mediator;
    using Hydra.Infrastructure.Reflection;
    using Microsoft.Practices.Unity;

    public class RequestHandlerRegistrationConvention : RegistrationConvention
    {
        private readonly IReadOnlyCollection<Type> decorators;

        public RequestHandlerRegistrationConvention(IReadOnlyCollection<Type> decorators)
        {
            Contract.Requires(decorators != null);

            this.decorators = decorators;
        }

        public override IEnumerable<Type> GetTypes()
        {
            Type[] handlers = AllTypes
                .FromHydraAssemblies()
                .Where(IsConcreteImplementationOfHandlerInterface)
                .ToArray();

            foreach (Type handler in handlers)
            {
                yield return handler;

                foreach (Type openGenericDecorator in decorators)
                {
                    Type concreteDecorator = MakeConcreteDecoratorType(handler, openGenericDecorator);

                    yield return concreteDecorator;
                }
            }
        }

        public override Func<Type, IEnumerable<Type>> GetFromTypes()
        {
            return WithMappings.FromAllInterfaces;
        }

        public override Func<Type, string> GetName()
        {
            return WithName.Default;
        }

        public override Func<Type, LifetimeManager> GetLifetimeManager()
        {
            return WithLifetime.Transient;
        }

        public override Func<Type, IEnumerable<InjectionMember>> GetInjectionMembers()
        {
            return WithInjectionMembers.None;
        }

        private static bool IsConcreteImplementationOfHandlerInterface(Type t)
        {
            return TypeHelper.ImplementsOpenGenericInterface(t, typeof(IRequestHandler<,>)) && !TypeHelper.IsOpenGeneric(t);
        }

        private static bool IsHandlerInterface(Type t, object filterCriteria)
        {
            return t.Name.StartsWith("IRequestHandler", StringComparison.Ordinal);
        }

        private static Type MakeConcreteDecoratorType(Type handler, Type openGenericDecorator)
        {
            // weberse 2015-02-27 all handler types implement 'IRequestHandler<,>' thus 'First' is OK
            Type[] genericArguments = handler.FindInterfaces(IsHandlerInterface, null).First().GenericTypeArguments;

            Type request = genericArguments[0];
            Type response = genericArguments[1];

            Type closedGenericDecorator = openGenericDecorator.MakeGenericType(request, response);
            return closedGenericDecorator;
        }
    }
}