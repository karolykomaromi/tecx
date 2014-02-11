namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Infrastructure.Data;
    using Infrastructure.Entities;

    public class ListViewService : IListViewService
    {
        private readonly IResourceKeyProvider resourceKeyProvider;

        public ListViewService(IResourceKeyProvider resourceKeyProvider)
        {
            Contract.Requires(resourceKeyProvider != null);

            this.resourceKeyProvider = resourceKeyProvider;
        }

        public ListView GetListView(string listViewName, int skip, int take)
        {
            var objects = new[]
                {
                    new DataFromView { Foo = "abc", Bar = 123, Timestamp = TimeProvider.Now },
                    new DataFromView { Foo = "def", Bar = 456, Timestamp = TimeProvider.Now },
                    new DataFromView { Foo = "ghi", Bar = 789, Timestamp = TimeProvider.Now }
                };

            IDataReader reader = objects.AsDataReader();

            ListView listView = new ListView { Name = listViewName, Skip = skip, Take = take };

            var properties = new List<Property>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string propertyName = reader.GetName(i);

                var property = new Property
                    {
                        PropertyName = propertyName,
                        PropertyType = TypeHelper.GetSilverlightCompatibleTypeName(reader.GetFieldType(i)),
                        ResourceKey = this.resourceKeyProvider.GetResourceKey(listViewName, propertyName)
                    };

                properties.Add(property);
            }

            listView.Properties = properties.ToArray();

            var rows = new List<ListViewRow>();

            while (reader.Read())
            {
                var row = new ListViewRow();

                var cells = new List<ListViewCell>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var cell = new ListViewCell
                        {
                            PropertyName = reader.GetName(i),
                            Value = reader.GetValue(i)
                        };

                    cells.Add(cell);
                }

                row.Cells = cells.ToArray();

                rows.Add(row);
            }

            listView.Rows = rows.ToArray();

            return listView;
        }

        private class DataFromView
        {
            public string Foo { get; set; }

            public int Bar { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }
}
