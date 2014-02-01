namespace Infrastructure.Dynamic
{
    using System.Diagnostics.Contracts;
    using System.Windows;

    [ContractClass(typeof(ControlAdapterFactoryContract))]
    public interface IControlAdapterFactory
    {
        IControl CreateAdapter(FrameworkElement element);
    }

    [ContractClassFor(typeof(IControlAdapterFactory))]
    internal abstract class ControlAdapterFactoryContract : IControlAdapterFactory
    {
        public IControl CreateAdapter(FrameworkElement element)
        {
            Contract.Requires(element != null);
            Contract.Ensures(Contract.Result<IControl>() != null);

            return new NullControlAdapter();
        }
    }
}