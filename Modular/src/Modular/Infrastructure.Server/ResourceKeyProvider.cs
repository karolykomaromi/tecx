namespace Infrastructure
{
    using System;

    public class ResourceKeyProvider : IResourceKeyProvider
    {
        public string GetResourceKey(string listViewName, string propertyName)
        {
            int beforeFirstDot = listViewName.IndexOf(".", 0, StringComparison.Ordinal);

            string name = listViewName.Substring(0, beforeFirstDot);

            name = StringHelper.ToUpperCamelCase(name);

            return name + "." + propertyName;
        }
    }
}