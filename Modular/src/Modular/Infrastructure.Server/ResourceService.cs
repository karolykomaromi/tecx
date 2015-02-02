namespace Infrastructure
{
    using Infrastructure.Entities;

    public class ResourceService : IResourceService
    {
        public ResourceString[] GetStrings(string moduleName, string culture, int skip, int take)
        {
            return new ResourceString[0];
        }
    }
}