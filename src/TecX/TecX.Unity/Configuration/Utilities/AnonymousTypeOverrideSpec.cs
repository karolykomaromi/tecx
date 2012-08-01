namespace TecX.Unity.Configuration.Utilities
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Configuration.Extensions;
    using TecX.Unity.Injection;

    public class AnonymousTypeOverrideSpec
    {
        private readonly List<ConstructorArgument> arguments;

        public AnonymousTypeOverrideSpec(object anonymous, Type to)
        {
            Guard.AssertNotNull(anonymous, "anonymous");

            this.arguments = new List<ConstructorArgument>();

            var properties = anonymous.PublicProperties();

            foreach (var property in properties.OfString())
            {
                var ctor = to.MostGreedyPublicCtor();

                var parameter = ctor.FindParameterNamed(property.Name);

                ConstructorArgument argument;
                string anonymousPropertyValue = (string)property.GetValue(anonymous, null);
                if (parameter.ParameterType == typeof(string))
                {
                    argument = new ConstructorArgument(
                        property.Name,
                        anonymousPropertyValue);
                }
                else
                {
                    argument = new ConstructorArgument(
                        property.Name,
                        new ResolvedParameter(parameter.ParameterType, anonymousPropertyValue));
                }

                this.arguments.Add(argument);
            }

            foreach (var property in properties.NotOfString())
            {
                object value = property.GetValue(anonymous, null);
                ConstructorArgument argument = new ConstructorArgument(property.Name, value);
                this.arguments.Add(argument);
            }
        }

        public static implicit operator InjectionMember(AnonymousTypeOverrideSpec @override)
        {
            Guard.AssertNotNull(@override, "override");

            return new SmartConstructor(@override.arguments);
        }
    }
}