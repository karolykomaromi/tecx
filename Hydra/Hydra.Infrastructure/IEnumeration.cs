namespace Hydra.Infrastructure
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(EnumerationContract))]
    internal interface IEnumeration
    {
        string Name { get; }

        int Value { get; }
    }

    [ContractClassFor(typeof(IEnumeration))]
    internal abstract class EnumerationContract : IEnumeration
    {
        public string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return string.Empty;
            }
        }

        public abstract int Value { get; }
    }
}