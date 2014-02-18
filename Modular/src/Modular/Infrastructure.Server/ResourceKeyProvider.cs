namespace Infrastructure
{
    using System;

    public class ResourceKeyProvider : IResourceKeyProvider
    {
        public string GetResourceKey(string listViewName, string propertyName)
        {
            if (string.Equals(listViewName, "RECIPES", StringComparison.OrdinalIgnoreCase) || 
                string.Equals(listViewName, "INGREDIENTS", StringComparison.OrdinalIgnoreCase))
            {
                listViewName = "Recipe";
            }

            return listViewName + "." + propertyName;
        }
    }
}