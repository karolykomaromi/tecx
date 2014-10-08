namespace Infrastructure.ListViews
{
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Infrastructure.Entities;

    [ServiceContract]
    [ContractClass(typeof(ListViewServiceContract))]
    public interface IListViewService
    {
        [OperationContract]
        ListView GetListView(string listViewName, int skip, int take);
    }

    [ContractClassFor(typeof(IListViewService))]
    internal abstract class ListViewServiceContract : IListViewService
    {
        public ListView GetListView(string listViewName, int skip, int take)
        {
            Contract.Requires(!string.IsNullOrEmpty(listViewName));
            Contract.Requires(skip >= 0);
            Contract.Requires(take >= 0);

            Contract.Ensures(Contract.Result<ListView>() != null);

            return new ListView();
        }
    }
}