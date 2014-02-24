namespace Infrastructure
{
    using System;

    public class ResourceKeyProvider : IResourceKeyProvider
    {
        public string GetResourceKey(string listViewName, string propertyName)
        {
            return listViewName.Substring(0, 1) + listViewName.Substring(1, listViewName.IndexOf(".", 0, StringComparison.Ordinal) - 1).ToLowerInvariant() + "." + propertyName;
        }
    }
}