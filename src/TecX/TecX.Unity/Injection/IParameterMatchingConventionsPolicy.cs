namespace TecX.Unity.Injection
{
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;

    public interface IParameterMatchingConventionsPolicy : IBuilderPolicy
    {
        bool Matches(ConstructorParameter argument, ParameterInfo parameter);

        void Add(ParameterMatchingConvention convention);
    }
}