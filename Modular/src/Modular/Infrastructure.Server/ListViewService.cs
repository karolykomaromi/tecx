namespace Infrastructure
{
    using Infrastructure.Entities;

    public class ListViewService : IListViewService
    {
        public ListView GetListView(string listViewName, int pageNumber, int pageSize)
        {
            return new ListView
                {
                    Properties = new[] { new Property { PropertyName = "Foo", PropertyType = "System.String, mscorlib", ResourceKey = "SEARCH.LABEL_SEARCHRESULTS" } },
                    Name = "DUMMY",
                    PageNumber = 1,
                    PageSize = 50,
                    Rows = new[] { new ListViewRow { Cells = new[] { new ListViewCell { PropertyName = "Foo", Value = "Bar" } } } }
                };
        }
    }
}
