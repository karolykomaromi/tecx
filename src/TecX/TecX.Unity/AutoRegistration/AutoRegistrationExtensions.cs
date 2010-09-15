using System;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    public static class AutoRegistrationExtensions
    {
        public static IAutoRegistration ExcludeSystemAssemblies(this IAutoRegistration registry)
        {
            Guard.AssertNotNull(registry, "registry");

            registry.Exclude(Filters.ForAssemblies.IsSystemAssembly());

            return registry;
        }

        public static IAutoRegistration EnableInterception(this IAutoRegistration registry)
        {
            Guard.AssertNotNull(registry, "registry");

            RunOnceRegistration registration = new RunOnceRegistration(
                (t, c) => c.AddExtension(new Interception()));

            registry.AddRegistration(registration);

            return registry;
        }

        public static IAutoRegistration Include(this IAutoRegistration registry,
                                                ContainerExtensionOptionsBuilder builder)
        {
            Guard.AssertNotNull(registry, "registry");
            Guard.AssertNotNull(builder, "builder");

            RunOnceRegistration registration = new RunOnceRegistration(
                builder.Build().Registrator);

            registry.AddRegistration(registration);

            return registry;
        }

        public static IAutoRegistration Include(this IAutoRegistration registry, Filter<Type> filter,
                                                RegistrationOptionsBuilder builder)
        {
            Guard.AssertNotNull(registry, "registry");
            Guard.AssertNotNull(filter, "filter");
            Guard.AssertNotNull(builder, "builder");

            registry.AddRegistration(new Registration(
                                         filter,
                                         (type, container) =>
                                             {
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

        public static IAutoRegistration Include(this IAutoRegistration registry, Filter<Type> filter,
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