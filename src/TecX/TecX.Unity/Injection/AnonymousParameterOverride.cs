namespace TecX.Unity.Injection
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Configuration.Extensions;

    public class AnonymousParameterOverride : InjectionMember
    {
        private readonly List<ConstructorParameter> arguments;

        public AnonymousParameterOverride(object anonymous, Type to)
        {
            Guard.AssertNotNull(anonymous, "anonymous");

            this.arguments = new List<ConstructorParameter>();

            var properties = anonymous.PublicProperties();

            foreach (var property in properties.OfString())
            {
                var ctor = to.MostGreedyPublicCtor();

                var parameter = ctor.FindParameterNamed(property.Name);

                ConstructorParameter argument;
                string anonymousPropertyValue = (string)property.GetValue(anonymous, null);
                if (parameter.ParameterType == typeof(string))
                {
                    argument = new ConstructorParameter(
                        property.Name,
                        anonymousPropertyValue);
                }
                else
                {
                    argument = new ConstructorParameter(
                        property.Name,
                        new ResolvedParameter(parameter.ParameterType, anonymousPropertyValue));
                }

                this.arguments.Add(argument);
            }

            foreach (var property in properties.NotOfString())
            {
                object value = property.GetValue(anonymous, null);
                ConstructorParameter argument = new ConstructorParameter(property.Name, value);
                this.arguments.Add(argument);
            }
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(implementationType, "implementationType");
            Guard.AssertNotNull(policies, "policies");

            var ctor = new SmartConstructor(this.arguments);

            ctor.AddPolicies(serviceType, implementationType, name, policies);
        }
    }
}