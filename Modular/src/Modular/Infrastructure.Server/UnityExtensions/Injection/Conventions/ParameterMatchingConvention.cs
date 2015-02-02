namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System.Diagnostics.Contracts;
    using System.Reflection;

    [ContractClass(typeof(ParameterMatchingConventionContract))]
    public interface IParameterMatchingConvention
    {
        bool IsMatch(Parameter argument, ParameterInfo parameter);
    }

    [ContractClassFor(typeof(IParameterMatchingConvention))]
    internal abstract class ParameterMatchingConventionContract : IParameterMatchingConvention
    {
        public bool IsMatch(Parameter argument, ParameterInfo parameter)
        {
            Contract.Requires(argument != null);
            Contract.Requires(parameter != null);

            return false;
        }
    }
}