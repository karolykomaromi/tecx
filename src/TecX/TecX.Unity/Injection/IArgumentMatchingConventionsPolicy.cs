namespace TecX.Unity.Injection
{
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;

    public interface IArgumentMatchingConventionsPolicy : IBuilderPolicy
    {
        bool Matches(ConstructorArgument argument, ParameterInfo parameter);

        void Add(ArgumentMatchingConvention convention);
    }
}