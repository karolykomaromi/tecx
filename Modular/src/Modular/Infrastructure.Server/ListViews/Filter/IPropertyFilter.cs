namespace Infrastructure.ListViews.Filter
{
    public interface IPropertyFilter
    {
        bool IsMatch(string propertyName);
    }
}