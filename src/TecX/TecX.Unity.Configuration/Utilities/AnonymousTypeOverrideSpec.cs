namespace TecX.Unity.Configuration.Utilities
{
    using System;
    using System.Linq;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Configuration.Extensions;
    using TecX.Unity.Injection;

    public class AnonymousTypeOverrideSpec
    {
        private readonly ConstructorArgumentCollection arguments;

        public AnonymousTypeOverrideSpec(object anonymous, Type to)
        {
            Guard.AssertNotNull(anonymous, "anonymous");

            this.arguments = new ConstructorArgumentCollection();

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

            // TODO weberse 2012-01-26 add the values of all non-string properties as well
        }

        public static implicit operator InjectionMember(AnonymousTypeOverrideSpec @override)
        {
            Guard.AssertNotNull(@override, "override");

            return new ClozeInjectionConstructor(@override.arguments);
        }
    }
}