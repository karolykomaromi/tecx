namespace Infrastructure
{
    public class ResourceKeyProvider : IResourceKeyProvider
    {
        public string GetResourceKey(string listViewName, string propertyName)
        {
            return (listViewName + "." + propertyName).ToUpperInvariant();
        }
    }
}