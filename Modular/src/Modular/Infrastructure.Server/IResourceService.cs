namespace Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Infrastructure.Entities;

    [ServiceContract]
    public interface IResourceService
    {
        [OperationContract]
        ResourceString[] GetStrings(string moduleName, string culture, int skip, int take);
    }

    internal abstract class ResourceServiceContract : IResourceService
    {
        public ResourceString[] GetStrings(string moduleName, string culture, int skip, int take)
        {
            Contract.Requires(!string.IsNullOrEmpty(moduleName));
            Contract.Requires(!string.IsNullOrEmpty(culture));
            Contract.Requires(skip >= 0);
            Contract.Requires(take >= 0);

            Contract.Ensures(Contract.Result<ResourceString[]>() != null);

            return new ResourceString[0];
        }
    }
}
