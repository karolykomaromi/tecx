namespace Hydra.Unity.Tracking
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure;
    using Microsoft.Practices.ObjectBuilder2;

    [ContractClass(typeof(CurrentBuildNodePolicyContract))]
    public interface ICurrentBuildNodePolicy : IBuilderPolicy
    {
        Stack<string> Tags { get; }
    }

    [ContractClassFor(typeof(ICurrentBuildNodePolicy))]
    internal abstract class CurrentBuildNodePolicyContract : ICurrentBuildNodePolicy
    {
        public Stack<string> Tags
        {
            get
            {
                Contract.Ensures(Contract.Result<Stack<string>>() != null);

                return Default.Value<Stack<string>>();
            }
        }
    }
}