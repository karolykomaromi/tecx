namespace Infrastructure.ListViews
{
    public class ResourceKeyProvider : IResourceKeyProvider
    {
        public string GetResourceKey(ListViewId listViewId, string propertyName)
        {
            return listViewId.ModuleName + "." + propertyName;
        }
    }
}