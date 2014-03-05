namespace Infrastructure.ListViews
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

    public class ImplicitTemplateDataGridColumn : DataGridBoundColumn
    {
        private readonly DataTemplate template;
        private readonly string propertyName;

        public ImplicitTemplateDataGridColumn(DataTemplate template, string propertyName)
        {
            Contract.Requires(template != null);
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            this.template = template;
            this.propertyName = propertyName;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            ICustomTypeProvider customTypeProvider = dataItem as ICustomTypeProvider;

            Type type = customTypeProvider != null ? customTypeProvider.GetCustomType() : dataItem.GetType();

            PropertyInfo property = type.GetProperty(this.propertyName);

            if (property != null)
            {
                dataItem = property.GetValue(dataItem, null);
            }

            // need to be able to display empty cells when value is missing.
            if (dataItem == null)
            {
                return new Grid();
            }

            FrameworkElement element = (FrameworkElement)this.template.LoadContent();

            element.DataContext = dataItem;

            return element;
        }

        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            // weberse 2014-03-05 listviews are readonly.
            return null;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            // weberse 2014-03-05 listviews are readonly.
            return null;
        }
    }
}
