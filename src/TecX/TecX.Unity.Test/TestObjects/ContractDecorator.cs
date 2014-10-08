namespace TecX.Unity.Test.TestObjects
{
    public class ContractDecorator : IContract
    {
        public IContract Base { get; set; }

        public string Bar { get; set; }

        public ContractDecorator(IContract @base)
        {
            this.Base = @base;
        }
    }
}