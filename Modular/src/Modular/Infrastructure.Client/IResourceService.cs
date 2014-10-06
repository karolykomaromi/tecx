namespace Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Infrastructure.Entities;

    [ServiceContract]
    public interface IResourceService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetStrings(string moduleName, string culture, int skip, int take, AsyncCallback callback, object asyncState);

        ResourceString[] EndGetStrings(IAsyncResult result);
    }

    internal abstract class ResourceServiceContract : IResourceService
    {
        public IAsyncResult BeginGetStrings(string moduleName, string culture, int skip, int take, AsyncCallback callback, object asyncState)
        {
            Contract.Requires(!string.IsNullOrEmpty(moduleName));
            Contract.Requires(!string.IsNullOrEmpty(culture));
            Contract.Requires(skip >= 0);
            Contract.Requires(take >= 0);

            return null;
        }

        public ResourceString[] EndGetStrings(IAsyncResult result)
        {
            Contract.Ensures(Contract.Result<ResourceString[]>() != null);

            return new ResourceString[0];
        }
    }
}