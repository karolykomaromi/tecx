namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System.Reflection;
    using Microsoft.Practices.ObjectBuilder2;

    public interface IParameterMatchingPolicy : IBuilderPolicy
    {
        bool Matches(Parameter argument, ParameterInfo parameter);

        void Add(ParameterMatchingConvention convention);
    }
}