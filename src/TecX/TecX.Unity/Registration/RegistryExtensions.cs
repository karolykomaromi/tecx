using System;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using TecX.Common;

namespace TecX.Unity.Registration
{
    public static class RegistryExtensions
    {
        public static IRegistry ExcludeSystemAssemblies(this IRegistry registry)
        {
            Guard.AssertNotNull(registry, "registry");

            registry.Exclude(Filters.ForAssemblies.IsSystemAssembly());

            return registry;
        }

        public static IRegistry EnableInterception(this IRegistry registry)
        {
            Guard.AssertNotNull(registry, "registry");

            RunOnceRegistration registration = new RunOnceRegistration(
                (t, c) => c.AddExtension(new Interception()));

            registry.AddRegistration(registration);

            return registry;
        }

        public static IRegistry Include(this IRegistry registry, ContainerExtensionOptionsBuilder builder)
        {
            Guard.AssertNotNull(registry, "registry");
            Guard.AssertNotNull(builder, "builder");

            RunOnceRegistration registration = new RunOnceRegistration(
                builder.Build().Registrator);

            registry.AddRegistration(registration);

            return registry;
        }

        public static IRegistry Include(this IRegistry registry, Filter<Type> filter, 
            RegistrationOptionsBuilder builder)
        {
            Guard.AssertNotNull(registry, "registry");
            Guard.AssertNotNull(filter, "filter");
            Guard.AssertNotNull(builder, "builder");

            registry.AddRegistration(new Registration(
                                         filter,
                                         (type, container) =>
                                             {
                                                 //TODO weberse should prohibit abstract types being registered
                                                 //as mapping targets
                                                 if (type.IsAbstract)
                                                     return;

                                                 builder.MappingTo(type);

                                                 foreach (RegistrationOptions registration in builder.Build())
                                                 {
                                                     container.RegisterType(
                                                         registration.From,
                                                         registration.To,
                                                         registration.Name,
                                                         registration.LifetimeManager,
                                                         registration.InjectionMembers);
                                                 }
                                             }));
            return registry;
        }

        public static IRegistry Include(this IRegistry registry, Filter<Type> filter,
                                                InterceptionOptionsBuilder builder)
        {
            Guard.AssertNotNull(registry, "registry");
            Guard.AssertNotNull(filter, "filter");
            Guard.AssertNotNull(builder, "builder");

            registry.AddRegistration(new Registration(
                                         filter,
                                         (type, container) =>
                                             {
                                                 builder.ApplyForType(type);

                                                 InterceptionOptions registration = builder.Build();

                                                 container.RegisterType(
                                                     registration.Type,
                                                     registration.InjectionMembers);
                                             }));
            return registry;
        }
    }
}