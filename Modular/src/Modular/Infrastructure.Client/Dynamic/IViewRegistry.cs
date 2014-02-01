namespace Infrastructure.Dynamic
{
    using System.Diagnostics.Contracts;
    using System.Windows;

    [ContractClass(typeof(ViewRegistryContract))]
    public interface IViewRegistry
    {
        void Register(FrameworkElement element);

        bool TryFindById(ControlId id, out IControl control);
    }

    [ContractClassFor(typeof(IViewRegistry))]
    internal abstract class ViewRegistryContract : IViewRegistry
    {
        public void Register(FrameworkElement element)
        {
            Contract.Requires(element != null);
        }

        public bool TryFindById(ControlId id, out IControl control)
        {
            control = null;
            return false;
        }
    }
}