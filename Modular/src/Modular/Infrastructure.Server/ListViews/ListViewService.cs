namespace Infrastructure.ListViews
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Infrastructure.Data;
    using Infrastructure.Entities;
    using Infrastructure.ListViews.Filter;

    public class ListViewService : IListViewService
    {
        private readonly IResourceKeyProvider resourceKeyProvider;
        private readonly IPropertyFilter propertyFilter;

        public ListViewService(IResourceKeyProvider resourceKeyProvider, IPropertyFilter propertyFilter)
        {
            Contract.Requires(resourceKeyProvider != null);
            Contract.Requires(propertyFilter != null);

            this.resourceKeyProvider = resourceKeyProvider;
            this.propertyFilter = propertyFilter;
        }

        public ListView GetListView(string listViewName, int skip, int take)
        {
            ListViewId listViewId;
            if (!ListViewId.TryParse(listViewName, out listViewId))
            {
                string msg = string.Format("Format for names of list views is '<Module>.<Name of ListView>'. You provided the following name '{0}'.", listViewName);
                FormatException inner = new FormatException(msg);
                throw new ArgumentException("Invalid format for name of list view. See inner exception for details.", "listViewName", inner);
            }

            List<DataFromView> objects = new List<DataFromView>();

            for (int i = skip; i < skip + take; i++)
            {
                objects.Add(new DataFromView { Id = i, Foo = Guid.NewGuid().ToString(), Bar = i, Timestamp = TimeProvider.Now });
            }

            IDataReader reader = objects.AsDataReader();

            ListView listView = new ListView { Name = listViewId.ModuleQualifiedListViewName, Skip = skip, Take = take };

            var properties = new List<Property>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string propertyName = reader.GetName(i);

                if (!this.propertyFilter.IsMatch(propertyName))
                {
                    var property = new Property
                        {
                            PropertyName = propertyName,
                            PropertyType = TypeHelper.GetSilverlightCompatibleTypeName(reader.GetFieldType(i)),
                            ResourceKey = this.resourceKeyProvider.GetResourceKey(listViewId, propertyName)
                        };

                    properties.Add(property);
                }
            }

            listView.Properties = properties.ToArray();

            var rows = new List<ListViewRow>();

            while (reader.Read())
            {
                var row = new ListViewRow();

                var cells = new List<ListViewCell>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string propertyName = reader.GetName(i);
                    object value = reader.GetValue(i);

                    if (string.Equals("Id", propertyName, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Id = (long)value;
                    }
                    else
                    {
                        if (!this.propertyFilter.IsMatch(propertyName))
                        {
                            var cell = new ListViewCell
                                {
                                    PropertyName = propertyName,
                                    Value = value
                                };

                            cells.Add(cell);
                        }
                    }
                }

                row.Cells = cells.ToArray();

                rows.Add(row);
            }

            listView.Rows = rows.ToArray();

            return listView;
        }

        private class DataFromView
        {
            public long Id { get; set; }

            public string Foo { get; set; }

            public int Bar { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }
}
